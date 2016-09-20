using UnityEngine;
using System;
using System.Collections.Generic;

public class GameViewSelection : MonoBehaviour {

    Renderer rd;
    Transform selectionRendering;

    Material selectionMat;

	void OnEnable()
    {
        rd = GetComponent<Renderer>();
        selectionMat = Resources.Load<Material>("JEdit/Visual/jecsu_selection");
        GameObject clone = new GameObject("SelectionRendering");
        selectionRendering = clone.transform;
        clone.hideFlags = HideFlags.HideInHierarchy;
        var meshfilter = clone.AddComponent<MeshFilter>();
        var renderer = clone.AddComponent<MeshRenderer>();

        meshfilter.mesh = GetComponent<MeshFilter>().sharedMesh;

        renderer.materials = rd.materials;
        var mats = renderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = selectionMat;
        }

        renderer.materials = mats;
    }

    void OnDisable()
    {
        if(selectionRendering)
            Destroy(selectionRendering.gameObject);
    }

    void Update()
    {
        selectionRendering.position = transform.position;
        selectionRendering.rotation = transform.rotation;
    }
}
