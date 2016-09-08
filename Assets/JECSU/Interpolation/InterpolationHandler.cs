namespace JECSU {

    using UnityEngine;
    using System.Collections.Generic;

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
}