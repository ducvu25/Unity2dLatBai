using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject goLeft;
    [SerializeField] List<Sprite> spritesLeft;
    [SerializeField] GameObject goRight;
    [SerializeField] List<Sprite> spritesRight;

    [SerializeField] TextMeshProUGUI txtScore;

    [Header("\nEND\n")]
    [SerializeField]
    Sprite[] imgEnds;
    [SerializeField] Image imgEnd;
    [SerializeField] TextMeshProUGUI txtEndName;
    [SerializeField] TextMeshProUGUI txtEndScore;
    [SerializeField]
    Sprite[] imgTxtEnd;

    [SerializeField] TMP_InputField txtMembership;
    [SerializeField] TextMeshProUGUI txtMem;

    Animator animator;
    int score;
    int level;
    int chuoi;
    int typeTxt;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        chuoi = 0;
        typeTxt = 0;
    }

    
    public void Click(bool left)
    {
        if(left)
        {
            animator.SetTrigger("ClickLeft");
        }
        else
        {
            animator.SetTrigger("ClickRight");
        }
        txtMem.gameObject.SetActive(true);
        txtMem.text = "Membership: " + txtMembership.text;

        txtMembership.gameObject.SetActive(false);
    }
    bool isLeft = false;
    public void ShowLeft(int i) {
        StartCoroutine(ShowI(true, i));
    }
    public void ShowRight(int i)
    {
        StartCoroutine(ShowI(false, i));
    }
    
    IEnumerator ShowI(bool left, int i)
    {
        isLeft = left;
        GameObject go = left ? goLeft : goRight;
        int[] values = InitValue();
        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).GetChild(0).GetComponent<Button>().interactable = false;

            go.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Convert(values[j]);
        }
        yield return new WaitForSeconds(0.5f);
        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).GetChild(0).GetComponent<Button>().interactable = false;
            if (i != j)
            {
                go.transform.GetChild(j).GetChild(0).GetComponent<Animator>().SetTrigger("Show");
            }
        }

        StartCoroutine(UpdateScore(values[i]));
        if (values[i] > 0)
        {
            chuoi++;
        }
        else
            chuoi--;
        print(chuoi);
    }
    Sprite Convert(int value)
    {
        switch (value)
        {
            case -2:
                typeTxt = 1;
                return isLeft ? spritesLeft[0] : spritesRight[0];
            case -1:
                typeTxt = 2;
                return isLeft ? spritesLeft[1] : spritesRight[1];
            case 5:
                return spritesRight[2];
            case 10:
                return isLeft ? spritesLeft[2] : spritesRight[3];
            case 20:
                return isLeft ? spritesLeft[3] : spritesRight[4];
            case 50:
                return spritesLeft[4];
            default:
                return isLeft ? spritesLeft[5 + value/100] : spritesRight[5 + value/100];
        }
    }
    IEnumerator UpdateScore(int value)
    {
        int d = value > 0 ? (score + value) : 0;
        int delta = d > 0 ? 1 : -1;
        if (value%100 == 0)
        {
            d = score * (value / 100);
            delta = 1;
        }
        while(score != d)
        {
            score += delta;
            txtScore.text = "$" + score.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        if (score == 0)
        {
            EndGame(typeTxt);
        }else
        StartCoroutine(End());
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1);
        animator.SetTrigger("End");
    }
    public void GO()
    {
        animator.SetTrigger(isLeft ? "InitLeft" : "InitRight");
        GameObject go = isLeft ? goLeft : goRight;
        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).GetChild(0).GetComponent<Button>().interactable = true;

            go.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = isLeft ? spritesLeft[5] : spritesRight[5];
        }
        if (level < 4)
        {
            level++;
        }
        else if(chuoi != 4 || level > 4)
        {
            EndGame();
        }
    }
    public void STOP()
    {
        EndGame();
    }
    int[] InitValue()
    {
        int[] A = { -1, -2 };
        int[,] B = { { 5, 10, 20 }, { 10, 20, 50 } };
        int[,] type = { { -1, -1, 1, 1, 1},
                        {-1, -1, 1, 1, 1},
                        {-1, -1, 1, 1, 1},
                        {-1, -1, 1, 1, 1} };
        int[] result = new int[5];
        if (level == 4)
        {
            int[] C = { 100, 200, 300, 400, 500 };
            for (int i = 0; i < 5; i++)
            {
                while (true)
                {
                    int x = C[Random.Range(0, 5)];
                    bool check = false;
                    for (int j = 0; j < i; j++)
                        if (result[j] == x)
                        {
                            check = true;
                            break;
                        }
                    if (!check)
                    {
                        result[i] = x;
                        break;
                    }
                }
            }
            return result;
        }
        
        for(int i=0; i<5; i++)
        {
            if (type[level, i] == 1)
            {
                while (true)
                {
                    int x = B[isLeft ? 1 : 0, Random.Range(0, 3)];
                    bool check = false;
                    for(int j=0; j<i; j++)
                        if (result[j] == x)
                        {
                            check = true;
                            break;
                        }
                    if (!check)
                    {
                        result[i] = x;
                        break;
                    }
                }
            }
            else
            {
                result[i] = A[Random.Range(0, 2)];
            }
        }
        List<int> list = new List<int>();
        int[] r = { 0, 0, 0, 0, 0 };
        for(int i=0; i<5; i++)
        {
            while(true)
            {
                int x = Random.Range(0, 5);
                if (!list.Contains(x))
                {
                    list.Add(x);
                    r[i] = result[x];
                    break;
                }
            }
        }
        return r;
    }

    void EndGame(int type = 0)
    {
        imgEnd.sprite = imgEnds[isLeft ? 0 : 1];
        if(score > 0)
        {
            txtEndName.text = "YOU WON";
        }
        else
        {
            txtEndName.text = "YOU LOSE";
        }
        string[] value = { "$" + score.ToString(), "OOPS !!!", "OMG !!!" };
        txtEndScore.text = value[type];
        
        animator.SetTrigger("EndGame");
        level = 0;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
