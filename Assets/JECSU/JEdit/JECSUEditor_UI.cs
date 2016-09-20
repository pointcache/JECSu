namespace JECSU.Editor
{

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using UnityEngine.UI;

    [RequireComponent(typeof(JECSUEditor))]
    public class JECSUEditor_UI : MonoBehaviour
    {
        public static bool isCameraInput;
        [SerializeField]
        private GameObject
            NewEntityBar,
            LeftPanel;
        [SerializeField]
        private RectTransform entityListContent;
        [SerializeField]
        private Text selectedText;

        public void SetSelectedName(string name)
        {
            selectedText.text = name;
        }

        void Awake()
        {
            EntityManager.OnEntityRegistered += PopulateEntityList;
        }

        void PopulateEntityList(Entity entity)
        {
            GameObject prefab = Resources.Load("JEdit/UI/EntityListElem") as GameObject;
            entityListContent.DestroyChildren();
            if(EntityManager.current != null)
            {
                foreach (var ent in EntityManager.entities)
                {
                    var go = Instantiate(prefab);
                    go.transform.SetParent(entityListContent, false);
                    go.GetComponentInChildren<Text>().text = ent.name;
                }
            }
        }

        public void OnNewEntityButton(bool state)
        {
            NewEntityBar.SetActive(state);
        }

        public void LeftPanelState(bool state)
        {
            if (state)
                LeftPanel.GetComponent<PanelAnimator>().GoEnd();
            else
                LeftPanel.GetComponent<PanelAnimator>().GoStart();

        }
    }
}