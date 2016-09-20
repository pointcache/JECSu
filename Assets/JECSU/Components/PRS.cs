namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    //adding serializeable allows us to see it in inspector and save the component, using Save system
    [Serializable]
    public class PRS : BaseComponent, IComponent
    {
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;
        public Vector3 position;

    }
}