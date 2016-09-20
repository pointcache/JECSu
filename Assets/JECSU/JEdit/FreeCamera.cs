namespace JECSU.Camera
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;


    public class FreeCamera : MonoBehaviour
    {
        public bool useFovLerp;
        public Transform target;
        public Vector3 targetOffset;
        float distance = 5.0f;
        public float maxDistanceToTarget = 40;
        public float minDistanceToTarget = 2f;


        public int yMinLimit = -80;
        public int yMaxLimit = 80;
        public int zoomRate = 1000;
        float panSpeed = 0.3f;

        public float rotationDamp = 100f;
        public float zoomMultiplier = 5.0f;
        public float PanSpeedMin = 0.5f;
        public float PanSpeedMax = 1f;
        public float minFov;
        public float maxFov;

        Camera mainCam;

        [SerializeField]
        float xDeg = 0.0f;
        [SerializeField]
        float yDeg = 0.0f;
        float currentDistance = 5f;
        float desiredDistance;
        Quaternion currentRotation;
        Quaternion desiredRotation;
        Quaternion rotation;
        Vector3 position;
        float invLerp;
        float lerp;
        Vector3 tempVec3;


        //dont touch, this stores pixel speed
        float xSpeed = 200.0f;
        float ySpeed = 200.0f;
        //this is mouse sensitivity in screens, where 1 = full screen in pixels and 0.2 is very good value
        public float xSpeed01 = 0.2f;
        public float ySpeed01 = 0.2f;
        //related to mouse sensitivity
        Vector2 prevMousePos;
        Vector2 screenSize;
        Vector2 MousePos;
        Vector2 MouseDelta;
        float mx, my;

        Vector3 initialTargetpos;


        void OnEnable()
        {
            if(Camera.main)
                Camera.main.gameObject.SetActive(false);


            mainCam = this.transform.GetComponent<Camera>();

            //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
            if (!target)
            {
                GameObject go = new GameObject("Cam Target");
                go.transform.position = transform.position + (transform.forward * currentDistance);
                target = go.transform;
            }

            distance = Vector3.Distance(transform.position, target.position);
            currentDistance = distance;
            desiredDistance = distance;

            //be sure to grab the current rotations as starting points.
            position = transform.position;
            rotation = transform.rotation;
            currentRotation = transform.rotation;
            desiredRotation = transform.rotation;

            int xsign = xDeg < 0 ? -1 : 1;
            xDeg = Vector3.Angle(Vector3.right, transform.right) * xsign;
            yDeg = Vector3.Angle(Vector3.up, transform.up);

            //setup of sensitivity values
            //they are bound to screen size for perfect feel
            xSpeed = this.mainCam.pixelWidth * xSpeed01;
            ySpeed = this.mainCam.pixelHeight * ySpeed01;

            screenSize = new Vector2(this.mainCam.pixelWidth, this.mainCam.pixelHeight);
        }

        void OnDisable()
        {
            if(target)
            Destroy(target.gameObject);
        }

        void Update()
        {
            //=================================================== CAMERA CONTROLS ===================================================//
            screenSize = new Vector2(this.mainCam.pixelWidth, this.mainCam.pixelHeight);

            MousePos = Input.mousePosition;
            mx = Mathf.InverseLerp(0, screenSize.x, MousePos.x);
            my = Mathf.InverseLerp(0, screenSize.y, MousePos.y);
            MousePos = new Vector2(mx, my);
            MouseDelta = prevMousePos - MousePos;

            prevMousePos = MousePos;
            // If Control and Alt and Middle button? ZOOM!
            UpdatePanSpeedWithDistance();
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                JECSU.Editor.JECSUEditor_UI.isCameraInput = true;
                if (Input.GetMouseButton(1))
                {
                    desiredDistance -= Input.GetAxis("Mouse Y") * UnityEngine.Time.deltaTime * zoomRate * 0.060f * Mathf.Abs(desiredDistance);
                }
                // If middle mouse and left alt are selected? ORBIT
                //else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
                else if (Input.GetMouseButton(0))
                {
                    xDeg -= xSpeed * MouseDelta.x;
                    yDeg += ySpeed * MouseDelta.y;

                    ////////OrbitAngle

                    //Clamp the vertical axis for the orbit
                    yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
                    // set camera rotation 
                    desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
                    currentRotation = transform.rotation;

                    rotation = Quaternion.Lerp(currentRotation, desiredRotation, UnityEngine.Time.deltaTime * rotationDamp);
                    transform.rotation = rotation;
                }
                // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
                else if (Input.GetMouseButton(2))
                {
                    //grab the rotation of the camera so we can move in a psuedo local XY space
                    target.rotation = transform.rotation;
                    tempVec3 = transform.right * -Input.GetAxis("Mouse X") * panSpeed;
                    target.Translate(tempVec3, Space.World);
                    tempVec3 = transform.up * -Input.GetAxis("Mouse Y") * panSpeed;
                    target.Translate(tempVec3, Space.World);
                }
            }else
                JECSU.Editor.JECSUEditor_UI.isCameraInput = false;
            

            // affect the desired Zoom distance if we roll the scrollwheel
            desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * UnityEngine.Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
            //clamp the zoom min/max
            desiredDistance = Mathf.Clamp(desiredDistance, minDistanceToTarget, maxDistanceToTarget);
            // For smoothing of the zoom, lerp distance
            currentDistance = Mathf.Lerp(currentDistance, desiredDistance, UnityEngine.Time.deltaTime * zoomMultiplier);

            // calculate position based on the new currentDistance 
            position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);

            transform.position = position;


            //change fov by distance
            ModifyFov();
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        void UpdatePanSpeedWithDistance()
        {
            invLerp = Mathf.InverseLerp(minDistanceToTarget, maxDistanceToTarget, currentDistance);
            lerp = Mathf.Lerp(PanSpeedMin, PanSpeedMax, invLerp);
            panSpeed = lerp;
        }

        void ModifyFov()
        {
            if (useFovLerp)
            {
                mainCam.fieldOfView = Mathf.Lerp(minFov, maxFov, invLerp);
            }
        }
    }
}