using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NowTime : MonoBehaviour
{
    private Text saveTimeText;

    // Start is called before the first frame update
    void Start()
    {
        saveTimeText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //display1の現在時刻表示
        saveTimeText.text = DateTime.Now.ToLongTimeString();
    }
}
