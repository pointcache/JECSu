using UnityEngine;
using System;
using System.Collections.Generic;
using JECSU;
using JECSU.Components;

public class GameViewBehavior : MonoBehaviour {

    public Entity entity;
    GameView gameview;
    PRS prs;
    Transform tr;
    public void Initialize(Entity _entity)
    {
        entity = _entity;
        prs = entity.GetComponent<PRS>();
        gameview = entity.GetComponent<GameView>();
        tr = transform;

        Pool.AddDirtyHandler<PRS>(OnPRSChange);
        SetPRS(prs);
    }

    void SetPRS(PRS _prs)
    {
        tr.position = _prs.position;
        tr.rotation = Quaternion.Euler(_prs.rotation);
        tr.localScale = _prs.scale;
    }

    void OnPRSChange(PRS _prs)
    {
        if (prs != _prs)
            return;
        SetPRS(_prs);

    }
    
}
