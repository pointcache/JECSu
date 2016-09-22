using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ui_blocker : MonoBehaviour, IPointerClickHandler
{
    public event Action onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick();
        Destroy(this.gameObject);
    }
}
