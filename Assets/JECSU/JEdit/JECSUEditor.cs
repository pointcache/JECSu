namespace JECSU.Editor {
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Components;

    public class JECSUEditor : MonoBehaviour {

        Entity selected, prevSelected;
        JECSUEditor_UI editorUI;

        void Awake()
        {
            editorUI = GetComponent<JECSUEditor_UI>();
        }

        void Update()
        {
            TrackSelection();
        }

        void TrackSelection()
        {
            if (Input.GetKey(KeyCode.Mouse0) && !JECSUEditor_UI.isCameraInput)
            {
                if (!Camera.current)
                    return;
                Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    var gameview = hit.collider.gameObject.GetComponent<GameViewBehavior>();
                    if (gameview && gameview.entity != selected)
                    {
                        if(selected!=null)
                        {
                            DisableHighlightOn(selected);
                        }
                        prevSelected = selected;
                        
                        gameview.gameObject.AddComponent<GameViewSelection>();
                        selected = gameview.entity;
                        editorUI.SetSelectedName(selected.name);
                    }
                }
                else
                {
                    if (selected != null)
                        DisableHighlightOn(selected);
                    selected = null;
                }
            }
        }

        void DisableHighlightOn(Entity ent)
        {
            var gameviewcomponent = ent.GetComponent<GameView>();

            if (gameviewcomponent != null && gameviewcomponent.view.GetComponent<GameViewSelection>())
            {
                Destroy(gameviewcomponent.view.GetComponent<GameViewSelection>());
            }
        }

        public void tryCreateNewEntity(string databaseID)
        {
            Entity.FromID(databaseID);
        }
    }
}