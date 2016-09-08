namespace JECSU.Components
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
	/// <summary>
    /// See InterpolationHandler class for instruction on how to use, also see Pong scene for example
    /// </summary>
    public class Interpolator : BaseComponent, IComponent
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
}