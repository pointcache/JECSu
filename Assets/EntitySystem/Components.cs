using UnityEngine;
using System;
using System.Collections.Generic;
using EntitySystem;
namespace EntitySystem.Components
{
    /*Here lie "native" or "default" components that are maintained by the entity system.
    How to:
        to create new component just copy paste other one or manually create using other as template.
        after you've done this don't forget to go to unity top menu and > EntitySystem > Compile 
        so that all components will get compiled to ComponentFactory class, its NOT NECESSARY
        (the system will work fine without it) , but it speeds up component creation a bit.
    
    */

    //adding serializeable allows us to see it in inspector and save the component, using Save system
    [Serializable]
    public class Position : BaseComponent, iComponent
    {
        public Vector3 position;

        //Overriding tostring is not necessary, but useful sometimes when you want to output component to console or somehting like that.
        public override string ToString()
        {
            return "Position:" + position.ToString();
        }
    }

    /// <summary>
    /// See InterpolationHandler class for instruction on how to use, also see Pong scene for example
    /// </summary>
    public class Interpolator : BaseComponent, iComponent
    {
        /// <summary>
        /// Linked interpolator
        /// </summary>
        public TransformInterpolator interpolator;
        /// <summary>
        /// Set this to update interpolator position
        /// </summary>
        public Vector3 position
        {
            get { if (interpolator == null) return Vector3.zero; return interpolator.position; }
            set
            {
                if (interpolator == null)
                    return;
                interpolator.position = value;
            }
        }
        /// <summary>
        /// Initializes interpolator
        /// </summary>
        /// <param name="tr"></param>
        public void Init( Transform tr)
        {
            interpolator = new TransformInterpolator(tr);
           
        }
    }

    [Serializable]
    public class ColorComponent : BaseComponent, iComponent
    {
        public Color color;
    }
    
    /// <summary>
    /// Add this to entities you want to be saved by Save()
    /// </summary>
    public class Serializeable : BaseComponent, iComponent
    {
    }
}

