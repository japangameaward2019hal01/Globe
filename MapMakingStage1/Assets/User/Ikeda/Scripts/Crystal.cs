﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : CrystalBase
{
    [HideInInspector]public CrystalHandle handle = null;

	// Use this for initialization
	void Start ()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            //未取得であれば
            if(!IsGet) IsGet = true;
            if (handle == null) handle.HitCrystal(this.gameObject);
        }
    }

}
