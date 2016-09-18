namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    //adding serializeable allows us to see it in inspector and save the component, using Save system
    [Serializable]
    public class GameRes : BaseComponent, IComponent
    {
        public string prefabID;

    }
}