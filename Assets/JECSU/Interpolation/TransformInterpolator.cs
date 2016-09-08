namespace JECSU
{
    using UnityEngine;

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
            if (was_posUpdated)
            {
                was_posUpdated = false;
                interpolationDriver = 0f;
            }

            interpolationDriver += Time.deltaTime;
            //remap time to 0-1
            float lerp = interpolationDriver.Remap(0f, Time.fixedDeltaTime, 0f, 1f);
            //set target position
            transform.position = Vector3.Lerp(lastPos, posUpdate, lerp);
        }
    }
}