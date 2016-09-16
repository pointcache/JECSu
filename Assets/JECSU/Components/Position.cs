namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    //adding serializeable allows us to see it in inspector and save the component, using Save system
    [Serializable]
    public class Position : BaseComponent, IComponent
    {

        public Vector3 position;
        public int tempint;
        public float tempfloat;
        public Vector2 tempvec2;
        public long templong;

    }
}