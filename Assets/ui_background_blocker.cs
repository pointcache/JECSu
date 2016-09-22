using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ui_background_blocker : MonoBehaviour {

    /// <summary>
    /// When we click on the blocker background, basically when the block will be cancelled
    /// </summary>
    public UnityEvent OnClick;
    GameObject blocker;
    public void Toggle(bool val)
    {
        if (val)
            DoBlock();
        else
            if(blocker)
                Destroy(blocker);
    }

    public void ToggleOn()
    {
        DoBlock();
    }

    public void ToggleOff()
    {
        if (blocker)
            Destroy(blocker);
    }

	void DoBlock()
    {
        blocker = new GameObject("blocker");
        blocker.transform.SetParent(transform.parent);
        blocker.transform.SetSiblingIndex(transform.GetSiblingIndex());

        Image img = blocker.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0);
        img.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        img.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        var blockComponent = blocker.AddComponent<ui_blocker>();
        blockComponent.onClick += OnClick.Invoke;
    }

    public void Test()
    {
        Debug.Log("stsrada");
    }
}
