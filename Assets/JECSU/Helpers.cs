using System.Collections.Generic;
using UnityEngine;

namespace JECSU
{
    public static class Helpers
    {
        public static class Math
        {

        }

        /// <summary>
        /// Changes float from range, to range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static void DestroyChildren(this Transform tr)
        {
            List<Transform> list = new List<Transform>();
            foreach (Transform child in tr)
            {
                list.Add(child);
            }
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(list[i].gameObject);
            }
        }

        public static GameObject SpawnPrefab(string path)
        {
            var pref = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(pref);
        }
    }
}
