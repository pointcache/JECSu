using UnityEngine;
using System;
using System.Collections.Generic;

public class ui_set_active : MonoBehaviour {

    public GameObject target;

    public void Set(bool value)
    {
        target.SetActive(value);
    }
}
