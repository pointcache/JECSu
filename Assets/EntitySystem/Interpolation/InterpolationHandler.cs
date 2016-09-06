using UnityEngine;
using System;
using System.Collections.Generic;
namespace EntitySystem {
    /// <summary>
    /// About:
    ///     Manages all interpolators in the scene
    /// Purpose: 
    ///     Interpolates position of a view GameObject in the scene.
    /// How to use:
    ///     Its assumed that your entity has Position component.
    ///     Its assumed you already have a GameObject that represents your entity in scene.
    ///     Add Interpolator component to your entity.
    ///     initialize it with Interpolator.Init( target transform );
    ///     Supply interpolator COMPONENT with target position, through "position" property.
    ///     
    /// Remarks:
    ///     Interpolator assumes that entity system tick runs in FixedUpdate, thus Time.fixedTimeDelta is used
    /// </summary>
    public class InterpolationHandler : MonoBehaviour {


        #region SINGLETON
        private static InterpolationHandler _instance;
        public static InterpolationHandler instance { get { if (!_instance) _instance = GameObject.FindObjectOfType<InterpolationHandler>(); return _instance; } }
        #endregion


        public List<TransformInterpolator> interpolators = new List<TransformInterpolator>();

        /// <summary>
        /// Constructs interpolationHandler in scene
        /// </summary>
        static void MakeNew()
        {
            GameObject root = new GameObject("InterpolationHandler");
            root.AddComponent<InterpolationHandler>();
        }

        /// <summary>
        /// Do not directly call, its automated
        /// </summary>
        /// <param name="interpolator"></param>
        public static void Register(TransformInterpolator interpolator)
        {
            if (instance == null)
                MakeNew();

            instance.register(interpolator);
        }

        void register(TransformInterpolator interpolator)
        {
            interpolators.Add(interpolator);
        }

        void Update()
        {
            int count = interpolators.Count;
            for (int i = 0; i < count; i++)
            {
                interpolators[i].Update();
            }
        }
    }

    /// <summary>
    /// Object that performs interpolation, works in tandem with Interpolator component
    /// </summary>
    public class TransformInterpolator
    {
        /// <summary>
        /// target transform
        /// </summary>
        Transform transform;
        /// <summary>
        /// Set this to set next target position to interpolate to
        /// </summary>
        public Vector3 position { get { return transform.position; } set { was_posUpdated = true; lastPos = posUpdate; posUpdate = value; } }

        bool was_posUpdated;
        Vector3 posUpdate, lastPos;
        /// <summary>
        /// holds time since last pos_update
        /// </summary>
        float interpolationDriver;

        public TransformInterpolator(Transform target)
        {
            transform = target;
            InterpolationHandler.Register(this);
        }

        public void Update()
        {
            //if received new position reset timer
            if (was_posUpdated )
            {
                was_posUpdated = false;
                interpolationDriver = 0f;
            }
            
            interpolationDriver += Time.deltaTime;
            //remap time to 0-1
            float lerp = interpolationDriver.Remap(0f, Time.fixedDeltaTime, 0f, 1f);
            //set target position
            transform.position = Vector3.Lerp(lastPos, posUpdate,lerp );
        }

        
    }
}