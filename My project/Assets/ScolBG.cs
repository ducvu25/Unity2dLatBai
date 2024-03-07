using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScolBG : MonoBehaviour
{
    [SerializeField] Vector2 speed;

    Material material;
    // Start is called before the first frame update
    void Awake()
    {
        material = transform.GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += speed * Time.deltaTime;
    }
}
