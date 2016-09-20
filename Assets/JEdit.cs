using UnityEngine;
using System;
using System.Collections.Generic;
using JECSU;

[RequireComponent(typeof(EntityManager))]
public class JEdit : MonoBehaviour {

    public bool LoadEditor;
	void Start()
    {
        if(LoadEditor)
            Helpers.SpawnPrefab("JEdit/JECSUEditor");
    }
}
