using UnityEngine;
using System;
using System.Collections.Generic;
namespace EntitySystem
{
    /// <summary>
    /// Represents entity abstraction as an object
    /// </summary>
    [Serializable]
    public class Entity
    {

        public string name { get; set; }
        //this id is set to entities by entity manager, its unique
        public int id { get; private set; }
        /// <summary>
        /// Use this to create entities
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        public static Entity New(string _name)
        {
            Entity ent = new Entity();
            ent.id = EntityManager.GetUID();
            ent.name = _name;
            EntityManager.RegisterEntity(ent);
            return ent;
        }

        /// <summary>
        /// Use this to add a component to entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Entity AddComponent<T>() where T : BaseComponent
        {
            iComponent comp = ComponentFactory.MakeNew<T>();
            if (comp == null)
                comp = Activator.CreateInstance<T>() as iComponent;
            comp.owner = this;
                        
            EntityManager.RegisterComponent(comp);
            return this;
        }

        /// <summary>
        /// Use this to add existing component to entity
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public Entity AddComponent(iComponent comp)
        {
            comp.owner = this;
            EntityManager.RegisterComponent(comp);
            return this;
        }

        /// <summary>
        /// Returns requested component if there is one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : BaseComponent
        {
            return Pool.ConcreteGetIfnoneMakeNew<T>().GetByEntity(id);
        }

        /// <summary>
        /// Returns all components
        /// </summary>
        /// <returns></returns>
        public List<iComponent> GetComponents() 
        {
            if (EntityManager.components_database_id.ContainsKey(id))
            {
                if(EntityManager.components_database_id[id].Count > 0)
                {
                    return EntityManager.components_database_id[id];
                }
            }
            return null;
        }

        /// <summary>
        /// Check if entity has a component of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : BaseComponent
        {
            return Pool.CheckComponent<T>(id);
        }

        /// <summary>
        /// More expensive version
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HasComponent(Type t) 
        {
            var comps = GetComponents();
            int count = comps.Count;
            for (int i = 0; i < count; i++)
            {
                if (comps[i].type == t)
                    return true;
            }
            return false;
        }
    }
}