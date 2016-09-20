/* namespace JECSU.Input
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using System.Collections;

    public class InputModule : MonoBehaviour
    {
        public static bool
            W, W_down, W_up,
            A, A_down, A_up,
            S, S_down, S_up,
            D, D_down, D_up,
            LAlt, LAlt_down, LAlt_up,
            LShift, LShift_down, LShift_up,
            m0, m0_down, m0_up,
            m1, m1_down, m1_up,
            m2, m2_down, m2_up;

        public static Vector2 mousePosition;

        public static event Action<Vector2> OnMouseLeftDown;
        public static event Action<Vector2> OnMouseLeftUp;
        public static event Action<Vector2> OnMouseLeft;

        public static event Action OnForward;
        public static event Action OnStrafeleft;
        public static event Action OnStraferight;
        public static event Action OnBackward;
        public static event Action OnJump;
        public static event Action OnJumpUp;
        public static event Action OnJumpDown;
        public static event Action OnAnyKey;
        public static event Action OnRun;
        public static event Action OnRunDown;
        public static event Action OnRunUp;

        public static event Action<bool> OnMovement;

        public static event Action<float> HorizontalAxis, VerticalAxis;

        private Vector2 lastClick;
        private int PressCount, MovementCount;
        public Options options;

        [System.Serializable]
        public class Options
        {
            public float AxisDeadzone;
            public bool UseMainCameraForRaycast;
            public Camera raycastCam;
            public KeyCode forward;
            public KeyCode strafeleft;
            public KeyCode straferight;
            public KeyCode backward;
            public KeyCode jump;
            public KeyCode run;
            public string axisHorizontal;
            public string axisVertical;
        }

        public static InputModule New(Options opts)
        {
            GameObject go = new GameObject("InputModule");
            InputModule component = go.AddComponent<InputModule>();
            component.options = opts;
            return component;
        }

        // Update is called once per frame
        void Update()
        {
            mousePosition = Input.mousePosition;

            W = W_down = W_up = A = A_down = A_up = S = S_down = S_up = D = D_down = D_up = LAlt = LAlt_up = LAlt_down
                = LShift = LShift_down = LShift_up = false;
            //Mouse Input

            if (Input.GetKey(KeyCode.W))
                W = true;
            if (Input.GetKeyDown(KeyCode.W))
                W_down = true;
            if (Input.GetKeyUp(KeyCode.W))
                W_up = true;

             if (Input.GetKey(KeyCode.A))
                A = true;
            if (Input.GetKeyDown(KeyCode.A))
                A_down = true;
            if (Input.GetKeyUp(KeyCode.A))
                A_up = true;

             if (Input.GetKey(KeyCode.S))
                S = true;
            if (Input.GetKeyDown(KeyCode.S))
                S_down = true;
            if (Input.GetKeyUp(KeyCode.S))
                S_up = true;

             if (Input.GetKey(KeyCode.D))
                D = true;
            if (Input.GetKeyDown(KeyCode.D))
                D_down = true;
            if (Input.GetKeyUp(KeyCode.D))
                D_up = true;

            if (Input.GetKey(KeyCode.LeftAlt))
                LAlt = true;
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                LAlt_down = true;
            if (Input.GetKeyUp(KeyCode.LeftAlt))
                LAlt_up = true;

            if (Input.GetKey(KeyCode.LeftShift))
                LShift = true;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                LShift_down = true;
            if (Input.GetKeyUp(KeyCode.LeftShift))
                LShift_up = true;

            if (Input.GetKey(KeyCode.Mouse0))
                m0 = true;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                m0_down = true;
            if (Input.GetKeyUp(KeyCode.Mouse0))
                m0_up = true;

            if (Input.GetKey(KeyCode.Mouse1))
                m1 = true;
            if (Input.GetKeyDown(KeyCode.Mouse1))
                m1_down = true;
            if (Input.GetKeyUp(KeyCode.Mouse1))
                m1_up = true;

            if (Input.GetKey(KeyCode.Mouse2))
                m2 = true;
            if (Input.GetKeyDown(KeyCode.Mouse2))
                m2_down = true;
            if (Input.GetKeyUp(KeyCode.Mouse2))
                m2_up = true;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (OnMouseLeft != null)
                    OnMouseLeft(CastRayFromCamera());
                PressCount++;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (OnMouseLeftDown != null)
                    OnMouseLeftDown(CastRayFromCamera());
                PressCount++;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (OnMouseLeftUp != null)
                    OnMouseLeftUp(CastRayFromCamera());
                PressCount++;
            }
            //Axis Inputs

            //Horizontal Axis
            if (HorizontalAxis != null)
            {
                float HorAx = Input.GetAxis(options.axisHorizontal);
                if (HorAx > options.AxisDeadzone)
                {
                    HorizontalAxis(HorAx);
                    MovementCount++;
                }
                else
                {
                    HorizontalAxis(0f);
                }
            }

            if (VerticalAxis != null)
            {
                //Vertical Axis
                float VerAx = Input.GetAxis(options.axisVertical);
                if (VerAx > options.AxisDeadzone)
                {
                    VerticalAxis(VerAx);
                    MovementCount++;
                }
                else
                {
                    VerticalAxis(0f);
                }
            }

            //Movement Input

            if (Input.GetKey(options.forward))
            {
                if (OnForward != null)
                    OnForward.Invoke();

                if (VerticalAxis != null)
                    VerticalAxis(1);


                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.backward))
            {
                if (OnBackward != null)
                    OnBackward.Invoke();


                if (VerticalAxis != null)
                    VerticalAxis(-1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.straferight))
            {
                if (OnStraferight != null)
                    OnStraferight.Invoke();

                if (HorizontalAxis != null)
                    HorizontalAxis(1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.strafeleft))
            {
                if (OnStrafeleft != null)
                    OnStrafeleft.Invoke();

                if (HorizontalAxis != null)
                    HorizontalAxis(-1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.jump))
            {
                OnJump.NullCheckInvoke();
                PressCount++;
            }
            if (Input.GetKeyDown(options.jump))
            {
                OnJumpDown.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyUp(options.jump))
            {
                OnJumpUp.NullCheckInvoke();
            }

            if (Input.GetKey(options.run))
            {
                OnRun.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyDown(options.run))
            {
                OnRunDown.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyUp(options.run))
            {
                OnRunUp.NullCheckInvoke();
            }


            if (OnMovement != null)
            {
                if (MovementCount > 0)
                {
                    OnMovement(true);
                }
                else
                {
                    OnMovement(false);
                }
            }

            if (PressCount > 0)
            {
                OnAnyKey.NullCheckInvoke();
                PressCount = 0;
            }
        }

        Vector2 CastRayFromCamera()
        {
            Vector3 point3;
            if (options.UseMainCameraForRaycast)
            {
                point3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                point3 = options.raycastCam.ScreenToWorldPoint(Input.mousePosition);
            }

            Vector2 point = new Vector2(point3.x, point3.y);
            lastClick = point;
            return point;
        }
    }

    public static class InputExtensions
    {
        public static void NullCheckInvoke(this Action act)
        {
            if (act != null)
                act.Invoke();
        }
    }

}
*/