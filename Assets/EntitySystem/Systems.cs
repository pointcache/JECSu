using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
namespace EntitySystem
{
    /// <summary>
    /// this class Handles systems
    /// </summary>
    public class Systems
    {
        public static List<iInitializeSystem> initSystems = new List<iInitializeSystem>();
        public static List<iExecuteSystem> executeSystems = new List<iExecuteSystem>();
        public static List<iMatcherSystem> matcherSystems = new List<iMatcherSystem>();
        public static List<MatcherSystemController> matcherSystemControllers = new List<MatcherSystemController>();

        /// <summary>
        /// Use this to create new systems
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T New<T>() where T : iEntitySystem, new()
        {
            var sys = new T();
            Register(sys as iEntitySystem);
            return sys;
        }

        /// <summary>
        /// Do not directly call as MonoSystems do that in their virtual Awake
        /// </summary>
        /// <param name="system"></param>
        public static void RegisterMonoSystem(MonoSystem system)
        {
            if ((typeof(iEntitySystem).IsAssignableFrom(system.GetType()))){
                Register(system as iEntitySystem);
            }
        }

        /// <summary>
        /// Registers systems, system can implement one of each of these
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        static iEntitySystem Register(iEntitySystem system)
        {
            if (typeof(iInitializeSystem).IsAssignableFrom(system.GetType()))
            {
                var sys = system as iInitializeSystem;
                sys.Initialize();
                initSystems.Add(sys);
            }
            if (typeof(iExecuteSystem).IsAssignableFrom(system.GetType()))
            {
                executeSystems.Add(system as iExecuteSystem);
            }
            if (typeof(iMatcherSystem).IsAssignableFrom(system.GetType()))
            {
                RegisterMatcherSystem(system as iMatcherSystem);
            }
            
            return system;
        }

        static void RegisterMatcherSystem(iMatcherSystem system)
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


    public class BaseSystem {
        public bool isActive { get; set; }
        public Matcher matcher { get; set; }
    }

    public class MonoSystem : MonoBehaviour
    {
        public bool isActive { get; set; }
        public Matcher matcher { get; set; }

        protected virtual void Awake()
        {
            Systems.RegisterMonoSystem(this);
        }
    }


    public interface iEntitySystem
    {
        bool isActive { get; set; }
    }

    public interface iInitializeSystem : iEntitySystem
    {
        void Initialize();
    }

    public interface iExecuteSystem : iEntitySystem
    {
        void Execute();
    }

    public interface iMatcherSystem : iEntitySystem
    {
        Matcher matcher { get; set; }
        void OnMatchAdded(Entity ent, Type t);
        void OnMatchRemoved(Entity ent, Type t);
    }

    public interface iMatcherSystem<T> : iMatcherSystem   {}
    public interface iMatcherSystem<T, T1> : iMatcherSystem   {}
    public interface iMatcherSystem<T, T1, T2> : iMatcherSystem   {}
    public interface iMatcherSystem<T, T1, T2, T3> : iMatcherSystem   {}
    public interface iMatcherSystem<T, T1, T2, T3, T4> : iMatcherSystem   {}

    public class MatcherSystemController
    {
        iMatcherSystem system;
        Matcher matcher;
        //which types we will match
        List<Type> systemTypes = new List<Type>();

        //here we get all generic types from system and create matcher out of them
        public void Initialize(iMatcherSystem _system)
        {
            system = _system;
            var interfaces = system.GetType().GetInterfaces();
            
            foreach (var type in interfaces)
            {
                if (!type.IsGenericType)
                    continue;
                Type gendef = type.GetGenericTypeDefinition();
                if (
                    gendef == typeof(iMatcherSystem<>) ||
                    gendef == typeof(iMatcherSystem<,>) ||
                    gendef == typeof(iMatcherSystem<,,>) ||
                    gendef == typeof(iMatcherSystem<,,,>))

                    systemTypes.AddRange(type.GetGenericArguments());
            }

            matcher = new Matcher();
            matcher.OnMatchAdded += system.OnMatchAdded;
            matcher.OnMatchRemoved += system.OnMatchRemoved;
           
            matcher.SetIs(systemTypes.ToArray());
        }
    }
}