using UnityEngine;
using System;
using System.Collections.Generic;
using EntitySystem;
/// <summary>
/// This class is used to "match" entities based on whether or not the have specified components.
/// It has static methods that return lists, and it can be made into an object that will dinamically track when new entities match the condition.
/// </summary>
public class Matcher
{
    //which types are included
    private List<Type> _is = new List<Type>();
    //which types are excluded
    private List<Type> _isnot = new List<Type>();
    //which pools are being observed for matches
    private List<Pool> observedPools = new List<Pool>();

    private List<Entity> _matching = new List<Entity>();
    //all entities that match
    public List<Entity> matching { get { return _matching; } }
    //this event is risen when match is found
    public event Action<Entity, Type> OnMatchAdded;
    //this event is risen when match is lost
    public event Action<Entity, Type> OnMatchRemoved;
    /// <summary>
    /// all matching components by type
    /// </summary>
    public Dictionary<Type, List<iComponent>> matching_components = new Dictionary<Type, List<iComponent>>();

    /// <summary>
    /// Set this matcher iclusion types
    /// </summary>
    /// <param name="types"></param>
    public void SetIs(params Type[] types)
    {
        foreach (var type in types)
        {
            observedPools.Add(Pool.GetIfnoneMakeNew(type));
            _is.Add(type);
        }
        RegisterInPools();

    }
    /// <summary>
    /// add one type as inclusion condition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void SetIs<T>() where T : EntitySystem.BaseComponent
    {
        observedPools.Add(Pool.GetIfnoneMakeNew<T>());
        _is.Add(typeof(T));

        RegisterInPools();
    }
    /// <summary>
    /// add 2 types as inclusion condition
    /// </summary>
    public void SetIs<T,T1>() where T : EntitySystem.BaseComponent where T1 : EntitySystem.BaseComponent 
    {
        if (typeof(T) != typeof(EntitySystem.BaseComponent))
            throw new Exception("Matcher can only work with components");
        observedPools.Add(Pool.GetIfnoneMakeNew<T>());
        _is.Add(typeof(T));

        if (typeof(T1) != typeof(EntitySystem.BaseComponent))
            throw new Exception("Matcher can only work with components");
        observedPools.Add(Pool.GetIfnoneMakeNew<T1>());
        _is.Add(typeof(T1));

        RegisterInPools();
    }
    /// <summary>
    /// add 3 types as inclusion condition
    /// </summary>
    public void SetIs<T, T1, T2>() where T : EntitySystem.BaseComponent where T1 : EntitySystem.BaseComponent where T2 : EntitySystem.BaseComponent
    {
        if (typeof(T) != typeof(EntitySystem.BaseComponent))
            throw new Exception("Matcher can only work with components");
        observedPools.Add(Pool.GetIfnoneMakeNew<T>());
        _is.Add(typeof(T));

        if (typeof(T1) != typeof(EntitySystem.BaseComponent))
            throw new Exception("Matcher can only work with components");
        observedPools.Add(Pool.GetIfnoneMakeNew<T1>());
        _is.Add(typeof(T1));

        if (typeof(T2) != typeof(EntitySystem.BaseComponent))
            throw new Exception("Matcher can only work with components");
        observedPools.Add(Pool.GetIfnoneMakeNew<T2>());
        _is.Add(typeof(T2));

        RegisterInPools();
    }

    //When component is added to observed pool it will get processed by this method, it determines if the component matches this matcher condition
    void OnAddedToPool(Entity ent, Type t)
    {
        //If matched then we process 
        if (!_matching.Contains(ent) && checkMatching(ent))
        {
            _matching.Add(ent);
            
            CacheComponents(ent);
            if(OnMatchAdded!=null)
                OnMatchAdded(ent, t);
            
        }
    }
    //if the component is removed from any observed pool it means it by default doesnt match condition and is removed
    void OnRemovedFromPool(Entity ent, Type t)
    {
        if (_matching.Contains(ent))
        {
            _matching.Remove(ent);
            OnMatchRemoved(ent, t);
        }
    }


    /// <summary>
    /// Saves all matched components internally for optimization
    /// </summary>
    /// <param name="ent"></param>
    void CacheComponents(Entity ent)
    {
        foreach (var comp in ent.GetComponents())
        {
            if(_is.Contains(comp.type))
            {
                if (matching_components.ContainsKey(comp.type))
                {
                    matching_components[comp.type].Add(comp);
                }
                else
                {
                    matching_components.Add(comp.type, new List<iComponent>());
                    matching_components[comp.type].Add(comp);
                }

            }
        }
    }
    /// <summary>
    /// Returns all matching components
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<iComponent> GetComponents<T>()
    {
        if (matching_components.ContainsKey(typeof(T)))
            return matching_components[typeof(T)];
        else
            return null;
    }
    /// <summary>
    /// Returns all matching components
    /// </summary>
    public List<iComponent> GetComponents(Type t)
    {
        if (matching_components.ContainsKey(t))
            return matching_components[t];
        else
            return null;
    }
    /// <summary>
    /// Check if entity satisfies matcher condition
    /// </summary>
    /// <param name="ent"></param>
    /// <returns></returns>
    public bool isMatching(Entity ent)
    {
        return checkMatching(ent);
    }
    /// <summary>
    /// Check if component satisfies matcher condition
    /// </summary>
    /// <param name="comp"></param>
    /// <returns></returns>
    public bool isMatching(BaseComponent comp)
    {
        return checkMatching(comp.owner);
    }

    
    bool checkMatching(Entity ent)
    {
        bool matches = true;
        int count = _is.Count;
        //for all included types
        for(int i = 0; i<count; i++)
        {
            if (!ent.HasComponent(_is[i]))
                matches = false;
        }
        count = _isnot.Count;
        //for all excluded types
        for(int i = 0; i<count; i++)
        {
            if (ent.HasComponent(_isnot[i]))
                matches = false;
        }

        return matches;
    }

    //Hooks this matcher to all observed pools events
    void RegisterInPools()
    {
        _matching.Clear();
        Pool biggest = null;
        //we find the biggest pool and register add/remove events for all of them

        foreach (var pool in observedPools)
        {
            if (biggest == null || pool.icomponents.Count> biggest.icomponents.Count)
                biggest = pool;
            pool.OnAdded += OnAddedToPool;
            pool.OnRemoved += OnRemovedFromPool;
        }
        //after we found the biggest pool we check all of its components for match
        foreach (var comp in biggest.icomponents)
        {
            OnAddedToPool(comp.owner, comp.type);
        }
    }

    //so that we dont generate garbage much
    static List<Entity> swaplist = new List<Entity>();

    /// <summary>
    /// Returns new list, null if no entities match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<Entity> Is<T>() where T : EntitySystem.BaseComponent
    {
        if (Pool<T>.current == null)
            Pool.CreatePool(typeof(T));
        int count = Pool<T>.components.Count;

        if (count == 0)
            return null;

        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < count; i++)
        {
            entities.Add(Pool<T>.components[i].owner);
        }

        return entities;
    }
    /// <summary>
    /// uses provided list as basis for match check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static List<Entity> Is<T>(List<Entity> entities) where T : EntitySystem.BaseComponent
    {
        if (entities == null)
            return null;
        int count = entities.Count;
        if (count == 0)
            return null;
        swaplist.Clear();
        swaplist.AddRange(entities);
        

        for (int i = 0; i < count; i++)
        {
            if (entities[i].HasComponent<T>())
                swaplist.Remove(entities[i]);
        }
        entities.Clear();
        entities.AddRange(swaplist);
        return entities;
    }

    public static List<Entity> IsNot<T>() where T : EntitySystem.BaseComponent
    {
        
        int count = EntityManager.entities.Count;

        if (count == 0)
            return null;
        List<Entity> list = new List<Entity>();
        for (int i = 0; i < count; i++)
        {
            if (!EntityManager.entities[i].HasComponent<T>())
                list.Add(EntityManager.entities[i]);
        }

        return list;
    }

    public static List<Entity> IsNot<T>(List<Entity> entities) where T : EntitySystem.BaseComponent
    {
        if (entities == null)
            return null;
        int count = entities.Count;
        if (count == 0)
            return null;
        swaplist.Clear();
        swaplist.AddRange(entities);

        entities.Clear();
        for (int i = 0; i < count; i++)
        {
            if (swaplist[i].HasComponent<T>())
                continue;
            entities.Add(swaplist[i]);
        }

        return entities;
    }
}
