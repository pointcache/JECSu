namespace JECSU
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// This object wraps around Matcher System 
    /// It is used by Systems class
    /// </summary>
    public class MatcherSystemController
    {
        IMatcherSystem system;
        Matcher matcher;
        //which types we will match
        List<Type> systemTypes = new List<Type>();

        //here we get all generic types from system and create matcher out of them
        public void Initialize(IMatcherSystem _system)
        {
            system = _system;
            var interfaces = system.GetType().GetInterfaces();
            
            foreach (var type in interfaces)
            {
                if (!type.IsGenericType)
                    continue;
                Type gendef = type.GetGenericTypeDefinition();
                if (
                    gendef == typeof(IMatcherSystem<>) ||
                    gendef == typeof(IMatcherSystem<,>) ||
                    gendef == typeof(IMatcherSystem<,,>) ||
                    gendef == typeof(IMatcherSystem<,,,>))

                    systemTypes.AddRange(type.GetGenericArguments());
            }

            matcher = new Matcher();
            matcher.OnMatchAdded += system.OnMatchAdded;
            matcher.OnMatchRemoved += system.OnMatchRemoved;
           
            matcher.SetIs(systemTypes.ToArray());
        }
    }
}