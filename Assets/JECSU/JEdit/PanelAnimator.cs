using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class PanelAnimator : MonoBehaviour {

    public UnityEvent OnStart, OnEnd;
    public bool Locked;
    public enum Mode { anchorPosition, OffsetAnimation}
    public Mode mode;
    public Vector2 start_pos;
    public Vector2 end_pos;
    public Offset startOffset;
    public Offset endOffset;

    public Spos OnStartMoveTo;
    public Ease easing;
    public float transition_duration = 1;
    RectTransform _rect;
    RectTransform rect { get { if (!_rect) _rect = GetComponent<RectTransform>(); return _rect; } }
    bool end;

    [System.Serializable]
    public class Offset
    {
        public float Top, Bottom, Right, Left;

        public Vector2 max { get { return new Vector2(Right, Top); } }
        public Vector2 min { get { return new Vector2(Left, Bottom); } }

        public Offset(float bottom, float left, float top,  float right )
        {
            Top = top;
            Bottom = bottom;
            Right = right;
            Left = left;
        }

        public Offset(RectTransform rt)
        {
            Top = rt.offsetMax.y;
            Bottom = rt.offsetMin.y;
            Right = rt.offsetMax.x;
            Left = rt.offsetMin.x;
        }
        public void ApplyToRect(RectTransform rt)
        {
            rt.offsetMin = new Vector2(Left, Bottom);
            rt.offsetMax = new Vector2(Right,Top);
        }
    }

    public enum Spos
    {
        start, end
    }

    public void ToggleLock()
    {
        Locked = !Locked;
    }

    public void SetLock(bool val)
    {
        Locked = val;
    }

    void Start()
    {
        if(OnStartMoveTo == Spos.end)
        {
            GoEnd();
        }
        else
        {
            GoStart();
        }
    }

    /// <summary>
    /// Do not use, internal
    /// </summary>
    public void JumpToStart()
    {
        switch (mode)
        {
            case Mode.anchorPosition:
                rect.anchoredPosition = start_pos;
                break;
            case Mode.OffsetAnimation:
                startOffset.ApplyToRect(rect);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Do not use, internal
    /// </summary>
    public void JumpToEnd()
    {
        switch (mode)
        {
            case Mode.anchorPosition:
                rect.anchoredPosition = end_pos;
                break;
            case Mode.OffsetAnimation:
                endOffset.ApplyToRect(rect);
                break;
            default:
                break;
        }
    }

    public void ForceEnd()
    {
        go_end();
    }

    public void GoEnd()
    {
        if (Locked)
            return;
        go_end();
    }

    void go_end()
    {
        end = true;
        switch (mode)
        {
            case Mode.anchorPosition:
                rect.DOAnchorPos(end_pos, transition_duration).SetEase(easing);
                break;
            case Mode.OffsetAnimation:
                DOTween.To(() => rect.offsetMax, x => rect.offsetMax = x, endOffset.max, transition_duration);
                DOTween.To(() => rect.offsetMin, x => rect.offsetMin = x, endOffset.min, transition_duration);
                break;
            default:
                break;
        }
        OnEnd.Invoke();
    }

    public void ForceStart()
    {
        go_start();
    }

    public void GoStart()
    {
        if (Locked)
            return;
        go_start();
    }

    void go_start()
    {
        end = false;
        switch (mode)
        {
            case Mode.anchorPosition:rect.DOAnchorPos(start_pos, transition_duration).SetEase(easing);
        
                break;
            case Mode.OffsetAnimation:
                DOTween.To(() => rect.offsetMax, x => rect.offsetMax = x, startOffset.max, transition_duration);
                DOTween.To(() => rect.offsetMin, x => rect.offsetMin = x, startOffset.min, transition_duration);
                break;
            default:
                break;
        }
        OnStart.Invoke();
    }

    public void Toggle()
    {
        if (Locked)
            return;
        if (end)
            GoStart();
        else
            GoEnd();
    }
}
