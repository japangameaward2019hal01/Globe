﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : SceneBase
{
    [SerializeField]
    private RectTransform Selecter;

    [Space(8)]
    [SerializeField]
    private GameObject SelectDisplay;
    [SerializeField]
    private GameObject[] select = new GameObject[4];

    [Space(4),SerializeField]
    private float WaitTime = 10f;

    private float time = 0f;

    //定数系
    private int MaxNum = 0;

    //変数系
    private int SelectNum = 0;

    private bool bUpdate = false;

    private bool bInput = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxNum = select.Length;

        Selecter.localPosition = select[0].transform.localPosition + (Vector3.right * Selecter.localPosition.x);

        for (int i = 0; i < MaxNum; i++)
        {
            select[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        select[0].transform.GetChild(0).gameObject.SetActive(true);

        time = 0f;
        bInput = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        time += Time.deltaTime;

        if (MySceneManager.bOption) return;

        int n = SelectNum;

        float selecter = Input.GetAxis(InputManager.Y_Selecter);

        if(selecter == 0)
            bInput = true;
        else if(bInput)
        {
            if (selecter >= 0.5f)
            {
                SelectNum--;
                bInput = false;
            }
            else if (selecter <= -0.5f)
            {
                SelectNum++;
                bInput = false;
            }
        }

        if (SelectNum <= -1) SelectNum = MaxNum - 1;
        if (n != SelectNum)
        {
            SelectNum = SelectNum % MaxNum;

            for (int i = 0; i < MaxNum; i++)
            {
                select[i].transform.GetChild(0).gameObject.SetActive(SelectNum == i);
            }

            Selecter.localPosition = select[SelectNum].transform.localPosition + (Vector3.right * Selecter.localPosition.x);
        }

        if (Input.GetButtonDown(InputManager.Submit))
        {
            switch (SelectNum)
            {
                case 0:
                    MySceneManager.FadeInLoad(MySceneManager.GalaxySelect);
                    break;
                case 1:
                    MySceneManager.FadeInLoad(MySceneManager.GalaxySelect);
                    break;
                case 2:
                    SceneManager.LoadScene(MySceneManager.OpsitionScene, LoadSceneMode.Additive);
                    break;
                case 3:
                    MySceneManager.Game_Exit();
                    break;
                default:
                    MySceneManager.FadeInLoad(MySceneManager.GalaxySelect);
                    break;
            }
        }

        //何か入力があれば
        if (Input.anyKey)
        {
            time = 0f;
        }
        else if(time >= WaitTime)
        {
            MySceneManager.FadeInLoad(MySceneManager.OpeningScene);
        }
    }

    private void bUpdate_Change()
    {
        bUpdate = true;
    }
}
