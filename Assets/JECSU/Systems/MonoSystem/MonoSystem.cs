namespace JECSU
{
	using UnityEngine;
	
    /// <summary>
    /// In theory this should work with unity as a entity system, however it brings several challenges on the table and needs to be thought through,
    /// if its even needed.
    /// </summary>
    public class MonoSystem : MonoBehaviour
    {
        public bool isActive { get; set; }
        public Matcher matcher { get; set; }

        protected virtual void Awake()
        {
            Systems.RegisterMonoSystem(this);
        }
    }
}