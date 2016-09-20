namespace JECSU.Editor
{

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;
    using DG.Tweening;
    using UnityEngine.UI;
    using UnityEngine.Events;

    /// <summary>
    /// Special Class replacing standard stupid button
    /// </summary>
    public class ui_button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {



        public Color normal = new Color32(0, 0, 0, 127);
        public Color highlight =new Color32(64, 154, 255, 255);
        public Color press = new Color32(139, 255, 0, 255);
        public Color toggled_color = new Color32(255, 144, 4, 255);

        public bool isToggle;
        public enum toggleClick
        {
            left, right, middle
        }
        public toggleClick ToggleClick;

        Tweener colortweener;
        Image img;

        float color_trans_duration = 0.2f;
        Ease easing = Ease.Linear;
        [SerializeField]
        bool toggled;

        [Serializable]
        public class onToggle : UnityEvent<bool> {}
        public onToggle OnToggle;
        public UnityEvent OnLeftClick, OnRightClick, OnMiddleClick;

        void Awake()
        {
            img = GetComponent<Image>();
            if (img == null) {
                Debug.LogError("No image found, aborting", gameObject);
                enabled = false;
                return;
            }
            colortweener = img.DOColor(normal, color_trans_duration).SetEase(easing).SetAutoKill(false);
            colortweener.ChangeValues(highlight, press).Pause();
            tweenToNormal();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            colorTweenTo(highlight);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tweenToNormal();
        }

        void tweenToNormal()
        {
            if (toggled)
                colorTweenTo(toggled_color);
            else
                colorTweenTo(normal);
        }

        void colorTweenTo(Color col)
        {
            colortweener.ChangeValues(img.color, col);
            colortweener.Restart();
            colortweener.PlayForward();
        }

        public void Toggle()
        {
            toggled = !toggled;
            if(OnToggle!=null)
                OnToggle.Invoke(toggled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
            if (eventData.pointerId == -1)
            {
                OnLeftClick.Invoke();
                if (isToggle && ToggleClick == toggleClick.left)
                    Toggle();
            }
            else if (eventData.pointerId == -2)
            {
                OnRightClick.Invoke();
                if (isToggle && ToggleClick == toggleClick.right)
                    Toggle();
            }
            else if (eventData.pointerId == -3)
            {
                OnMiddleClick.Invoke();
                if (isToggle && ToggleClick == toggleClick.middle)
                    Toggle();
            }
            img.color = press;
            tweenToNormal();
        }
    }
}