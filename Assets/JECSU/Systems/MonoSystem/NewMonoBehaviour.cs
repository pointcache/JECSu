using UnityEngine;
using System.Collections;

    public class LevelPieceData
    {
        public Vector3 startnode, endNode;
    }

    public class LevelPiece : MonoBehaviour {

        public LevelPieceData data;

        public void Initialize()
        {
            //search for start node / endnode
            data.startnode = transform.FindChild("startnode").position;
            data.endNode = transform.FindChild("endnode").position;
        }
    }
