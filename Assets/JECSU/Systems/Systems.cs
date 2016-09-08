namespace JECSU
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// this class Handles systems
    /// </summary>
    public class Systems
    {
        public static List<IInitializeSystem> initSystems = new List<IInitializeSystem>();
        public static List<IExecuteSystem> executeSystems = new List<IExecuteSystem>();
        public static List<IMatcherSystem> matcherSystems = new List<IMatcherSystem>();
        public static List<MatcherSystemController> matcherSystemControllers = new List<MatcherSystemController>();

        /// <summary>
        /// Use this to create new systems
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T New<T>() where T : IEntitySystem, new()
        {
            var sys = new T();
            Register(sys as IEntitySystem);
            return sys;
        }

        /// <summary>
        /// Do not directly call as MonoSystems do that in their virtual Awake
        /// </summary>
        /// <param name="system"></param>
        public static void RegisterMonoSystem(MonoSystem system)
        {
            if ((typeof(IEntitySystem).IsAssignableFrom(system.GetType()))){
                Register(system as IEntitySystem);
            }
        }

        /// <summary>
        /// Registers systems, system can implement one of each of these
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        static IEntitySystem Register(IEntitySystem system)
        {
            if (typeof(IInitializeSystem).IsAssignableFrom(system.GetType()))
            {
                var sys = system as IInitializeSystem;
                sys.Initialize();
                initSystems.Add(sys);
            }
            if (typeof(IExecuteSystem).IsAssignableFrom(system.GetType()))
            {
                executeSystems.Add(system as IExecuteSystem);
            }
            if (typeof(IMatcherSystem).IsAssignableFrom(system.GetType()))
            {
                RegisterMatcherSystem(system as IMatcherSystem);
            }
            
            return system;
        }

        static void RegisterMatcherSystem(IMatcherSystem system)
        {
            if(matcherSystems.Contains(system))
            {
                throw new Exception("Trying to register already existing react system which is well, and error.");
            }
            else
            {
                //Matcher systems are wrapped into a controller
                MatcherSystemController controller = new MatcherSystemController();
                controller.Initialize(system);
            }
        }
    }
}