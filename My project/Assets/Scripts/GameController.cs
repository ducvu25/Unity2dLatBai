using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("\n-----Left---------")]
    [SerializeField] GameObject goLeft;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] List<Sprite> spritesLeft;
    [SerializeField] List<Sprite> spritesXLeft;

    [Header("\n-----Right---------")]
    [SerializeField] GameObject goRight;
    [SerializeField] Sprite spriteRight;
    [SerializeField] List<Sprite> spritesRight;
    [SerializeField] List<Sprite> spritesXRight;

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

    [SerializeField] GameObject goEffect;
    [SerializeField] Vector2 p1, p2;

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
        Time.timeScale =  2f; // tốc độ
    }

    
    public void Click(bool left)
    {
        if (txtMembership.text == "") return;
        if(left)
        {
            animator.SetTrigger("ClickLeft");
        }
        else
        {
            animator.SetTrigger("ClickRight");
        }
        txtMem.gameObject.SetActive(true);
        txtMem.text = txtMembership.text;

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
        int[] values = Caculater.InitValue(level);
        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).GetChild(0).GetComponent<Button>().interactable = false;
            if (values[j] < 0)
            {
                go.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = left ? spritesXLeft[-values[j] - 1] : spritesXRight[-values[j] - 1];
            }
            else
            {
                go.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = left ? spritesLeft[values[j]] : spritesRight[values[j]];
            }
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
        switch (values[i]) {
            case -4:
            case -5:
                {
                    StartCoroutine(StartSound((int)(isLeft ? TypeEffecySound.RED_BIG_CARD : TypeEffecySound.GREEN_BIG_CARD), 0));
                    break;
                }
            case -3:
                {
                    StartCoroutine(StartSound((int)(isLeft ? TypeEffecySound.X3_RED : TypeEffecySound.X3_GREEN), 0));
                    break;
                }
            case -2:
                {
                    StartCoroutine(StartSound((int)(isLeft ? TypeEffecySound.X2_RED : TypeEffecySound.X2_GREEN), 0));
                    break;
                }
            case -1:
                {
                    StartCoroutine(StartSound((int)(isLeft ? TypeEffecySound.X1_RED : TypeEffecySound.X1_GREEN), 0));
                    break;
                }
            case 4:
            case 5:
            case 6:
                {
                    StartCoroutine(StartSound((int)(isLeft ? TypeEffecySound.RED_BIG_CARD : TypeEffecySound.GREEN_BIG_CARD), 0));
                    break;
                }
            case 0:
                {
                    StartCoroutine(StartSound((int)(TypeEffecySound.LOSE), 0));
                    break;
                }
            default:
                {
                    StartCoroutine(StartSound((int)(TypeEffecySound.SMALL_CARD), 0));
                    break;
                }
        }
        StartCoroutine(UpdateScore(values[i]));
        if (values[i] != 0)
        {
            chuoi++;
        }
        else
            chuoi--;
    }
    IEnumerator UpdateScore(int value)
    {
        int d;
        int delta;
        if (value < 0)
        {
            d = score * (-value);
            delta = -value * 5;
        }
        else if (value > 0)
        {
            d = score + Caculater.scoreValue[(isLeft ? 1 : 0), value];
            delta = 1;
        }
        else {
            d = 0;
            delta = -1;
        }
        if (d != 0)
            StartCoroutine(StartSound((int)TypeEffecySound.ADD_COIN, 0));
        while ((score < d && delta > 0) || (score > d && delta < 0))
        {
            score += delta;
            if (score > d)
                score = d;
            txtScore.text = "$" + score.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        if (score == 0 || level == 4)
        {
            StartCoroutine(EndGame(0, 0.5f));
        }
        else
            StartCoroutine(End());

    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1);
        if(isLeft)
            animator.SetTrigger("EndLeft");
        else
        {
            animator.SetTrigger("EndRight");
        }
        if (isLeft) goLeft.SetActive(true);
        else goRight.SetActive(true);
        
    }
    public void GO()
    {
        StartCoroutine(StartSound((int)TypeEffecySound.BUTTON, 0));
        animator.SetTrigger(isLeft ? "InitLeft" : "InitRight");
        GameObject go = isLeft ? goLeft : goRight;
        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).GetChild(0).GetComponent<Button>().interactable = true;

            go.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Image>().sprite = isLeft ? spriteLeft : spriteRight;
        }
        if (level < 4)
        {
            level++;
        }
        else if(chuoi != 4 || level > 4)
        {
            StartCoroutine(EndGame());
        }
    }
    public void STOP()
    {
        StartCoroutine(StartSound((int)TypeEffecySound.BUTTON, 0));
        StartCoroutine(EndGame());
    }
    

    IEnumerator EndGame(int type = 0, float delay = 0.3f)
    {
        yield return new WaitForSeconds(delay);
        imgEnd.sprite = imgEnds[isLeft ? 0 : 1];
        if(score > 0)
        {
            txtEndName.text = "YOU WON";
            StartCoroutine(StartSound((int)TypeEffecySound.WIN));
            //StartCoroutine(Spawn());
            //Invoke("EndSpawn", 20);
        }
        else
        {
            txtEndName.text = "GAME OVER";
            StartCoroutine(StartSound((int)TypeEffecySound.LOSE));
        }
        string[] value = { "$" + score.ToString(), "OOPS !!!"};
        txtEndScore.text = value[type];
        
        animator.SetTrigger("EndGame");
        //Invoke("LoadScene", 40);
        level = 0;
    }
    IEnumerator StartSound(int index, float delay = 0.2f)
    {
        yield return new WaitForSeconds(delay);
        AudioController.instance.Play(index);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
    /*void EndSpawn()
    {
        StopCoroutine(Spawn());
    }*/
    /*IEnumerator Spawn()
    {
        *//*while (true)
        {
            int n = Random.RandomRange(0, 3);
            for (int i = 0; i < n; i++)
            {
                GameObject go = Instantiate(goEffect, new Vector2(Random.Range(p1.x, p2.x), Random.Range(p1.y, p2.y)), Quaternion.identity);
                Destroy(go, 2);
            }
            yield return new WaitForSeconds(Random.RandomRange(0.4f, 1.5f));
        }*//*
    }*/
}
