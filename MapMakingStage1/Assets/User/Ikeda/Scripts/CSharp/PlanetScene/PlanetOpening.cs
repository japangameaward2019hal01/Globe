﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlanetOpening : MonoBehaviour 
{
    [SerializeField]
    public PopUpScript popUpScript;
    [SerializeField]
    private ItemUI ItemPopUp;

    //--- Attribute -----------------------------------------------------------
    [Header("StageName State")]
    [SerializeField]
    private GameObject StageLabel;
    [SerializeField, Tooltip("エリア名")]
    private TextMeshProUGUI tm_GalaxyName;
    [SerializeField,Tooltip("ステージ名")]
    private TextMeshProUGUI tm_StageName;
    [SerializeField, Tooltip("ステージ番号")]
    private TextMeshProUGUI tm_StageNum;

    //--- MonoBehaviour -------------------------------------------------------

    // Use this for initialization
    void Start ()
    {
        tm_GalaxyName.text = MySceneManager.Get_GalaxyName();
        tm_StageName.text = MySceneManager.Get_PlanetName();
        tm_StageNum.text = (DataManager.Instance.playerData.SelectGalaxy + 1).ToString("0") + "-" + (DataManager.Instance.playerData.SelectPlanet+1).ToString("0");
    }

    //--- Method --------------------------------------------------------------

    public void Begin()
    {
        PlanetScene.Instance.SetOpening();
        popUpScript.PopUp();
    }

    public void End()
    {
        popUpScript.PopDown();
        ItemPopUp.PopUp();
    }

}
