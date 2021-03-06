﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class HitUI : MonoBehaviour {

    public Billbord uiA;
    private bool hitGoal;

    // Use this for initialization
    void Start () {
        uiA.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(hitGoal && PlanetScene.Instance.state == PlanetScene.STATE.MAINGAME)
        {
            uiA.gameObject.SetActive(true);
        }
        else
        {
            uiA.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            hitGoal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Finish")
        {
            hitGoal = false;
        }
    }
}
