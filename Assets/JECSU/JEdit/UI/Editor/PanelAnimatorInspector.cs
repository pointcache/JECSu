using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PanelAnimator))]
public class PanelAnimatorInspector : Editor {

    public override void OnInspectorGUI()
    {
        var t = (PanelAnimator)target;
        var rt = ((RectTransform)t.transform);
        
        GUILayout.BeginHorizontal();
        GUI.color = Color.red;
        if(GUILayout.Button("Store start"))
        {
            t.start_pos = rt.anchoredPosition;
            t.startOffset = new PanelAnimator.Offset(rt);
        }
        if (GUILayout.Button("Store end"))
        {
            t.end_pos = rt.anchoredPosition;
            t.endOffset = new PanelAnimator.Offset(rt);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUI.color = Color.green;
        if (GUILayout.Button("Move to start"))
        {
            t.JumpToStart();
        }
        if (GUILayout.Button("Move to end"))
        {
            t.JumpToEnd();
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        base.OnInspectorGUI();
    }
}
