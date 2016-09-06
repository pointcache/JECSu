using UnityEngine;
using System;
using System.Collections.Generic;

namespace EntitySystem
{
    public static class Helpers
    {
        public static class Math
        {

        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
