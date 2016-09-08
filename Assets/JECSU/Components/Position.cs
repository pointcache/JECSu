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

        //Overriding tostring is not necessary, but useful sometimes when you want to output component to console or somehting like that.
        public override string ToString()
        {
            return "Position:" + position.ToString();
        }
    }
}