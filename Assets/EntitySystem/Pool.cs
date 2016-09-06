using UnityEngine;
using System;
using System.Collections.Generic;
namespace EntitySystem
{
    /// <summary>
    /// Stores components.
    /// </summary>
    public class Pool
    {
        /// <summary>
        /// When entity is added to pool
        /// </summary>
        public event Action<Entity, Type> OnAdded;
        public event Action<Entity, Type> OnRemoved;
        /// <summary>
        /// All pools by type
        /// </summary>
        public static Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>(100);
        /// <summary>
        /// all components in pool
        /// </summary>
        public List<iComponent> icomponents = new List<iComponent>(1000);

        /// <summary>
        /// Get first component of type T from corresponding pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFirst<T>() where T : BaseComponent
        {
            return Pool<T>.GetFirst();
        }

        /// <summary>
        /// Register component in corresponding pool
        /// </summary>
        /// <param name="icomp"></param>
        public static void AddToPool(iComponent icomp)
        {
            Pool pool = null;
            if (pools.ContainsKey(icomp.type))
            {
                pool = pools[icomp.type];
                pool.addToPool((BaseComponent)icomp);
            }
            else
            {
                pool = CreatePool(icomp.type);
                pool.addToPool((BaseComponent)icomp);

            }

            pool.icomponents.Add(icomp);
        }

        /// <summary>
        /// Register some method for callback when a component was changed of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void AddDirtyHandler<T>(Action<T> handler) where T : BaseComponent
        {
            Pool<T> pool = ConcreteGetIfnoneMakeNew<T>();
            pool.AddDirtyHandler(handler);
        }

        /// <summary>
        /// Query pool for existance of component T on entity by entity id
        /// </summary>
        public static bool CheckComponent<T>(T t, int entityid) where T : BaseComponent
        {
            return ConcreteGetIfnoneMakeNew<T>().EntityHasComponent(entityid);
        }
        /// <summary>
        /// Query pool for existance of component T on entity by entity id
        /// </summary>
        public static bool CheckComponent<T>(int entityid) where T : BaseComponent
        {
            return ConcreteGetIfnoneMakeNew<T>().EntityHasComponent(entityid);
        }

        /// <summary>
        /// Simply creates a pool of type t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Pool CreatePool(Type t)
        {
            if (!typeof(BaseComponent).IsAssignableFrom(t)){
                throw new Exception("Tried to create a pool with non Component type");
            }
            Type elementType = Type.GetType(t.ToString());
            Type[] types = new Type[] { elementType };
            Type listType = typeof(Pool<>);
            Type genericType = listType.MakeGenericType(types);
            Pool pool = (Pool)Activator.CreateInstance(genericType);
            ((iPoolAccessor)pool).Initialize();
            pools.Add(t, pool);
            return pool;
        }

        /// <summary>
        /// safe way to get a pool, if it doesnt exists we create a new one.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public static Pool GetIfnoneMakeNew(Type componentType)
        {
            if (pools.ContainsKey(componentType))
            {
                return pools[componentType];
            }
            else
            {
                return CreatePool(componentType);
            }
        }
        /// <summary>
        /// Safe way to get a pool, if it doesnt exists we create a new one.
        /// </summary>
        public static Pool GetIfnoneMakeNew<T>() where T : BaseComponent
        {
            if (pools.ContainsKey(typeof(T)))
            {
                return pools[typeof(T)];
            }
            else
            {

                return CreatePool(typeof(T));
            }
        }
        /// <summary>
        /// Safe way to get a pool, if it doesnt exists we create a new one.
        /// Returns concrete pool of type T
        /// </summary>
        public static Pool<T> ConcreteGetIfnoneMakeNew<T>() where T : BaseComponent
        {
            if (pools.ContainsKey(typeof(T)))
            {
                return pools[typeof(T)] as Pool<T>;
            }
            else
            {
                return CreatePool(typeof(T)) as Pool<T>;
            }
        }

        //hack to allow concrete pools access from base
        public virtual void addToPool(BaseComponent comp)
        {}

        /// <summary>
        /// raises event when component is added to pool
        /// </summary>
        protected void raiseAddedEvent(BaseComponent comp, Type t)
        {
            if (OnAdded != null)
                OnAdded(comp.owner, t);
        }

    }
    /// <summary>
    /// used to have access to unknown pool type
    /// </summary>
    public interface iPoolAccessor
    {
        void Initialize();
    }

    /// <summary>
    /// A concrete generic implementation of the pool, mixes static components with dynamic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> : Pool, iPoolAccessor where T : BaseComponent
    {
        /// <summary>
        /// all components
        /// </summary>
        public static List<T> components = new List<T>(1000);
        /// <summary>
        /// reference to this pool instance
        /// </summary>
        public static Pool<T> current;
        //all the handlers that are watching this pool for "onDirty" events
        Action<T> handlers ;
        /// <summary>
        /// entity id/ component pair
        /// </summary>
        static Dictionary<int, T> components_by_entity = new Dictionary<int, T>(1000);

        /// <summary>
        /// when component is marked as dirty this handler will relay that even to handlers in "handlers" Action
        /// </summary>
        /// <param name="comp"></param>
        void dirtyHandler(BaseComponent comp)
        {
            if(handlers != null)
                handlers(comp as T);
        }

        /// <summary>
        /// Returns first component if there is one
        /// </summary>
        /// <returns></returns>
        public static T GetFirst()
        {
            if (components != null && components.Count > 0)
                return components[0];
            else
                return null;
        }

        /// <summary>
        /// Returns component by entity id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByEntity(int id)
        {
            if (components_by_entity.ContainsKey(id))
                return components_by_entity[id];
            else
                return null;
        }

        /// <summary>
        /// Checks if some entity has a component of this type
        /// </summary>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public bool EntityHasComponent(int entityid)
        {
            return components_by_entity.ContainsKey(entityid);
        }

        /// <summary>
        /// Processes component so its added to pool
        /// </summary>
        /// <param name="comp"></param>
        public override void addToPool(BaseComponent comp)
        {
            base.addToPool(comp);
            //register dirtyhandler in component
            comp.onDirty += dirtyHandler;
            var obj = comp as T;
            if (components_by_entity.ContainsKey(comp.ownerid))
            {
                return;
            }
            else
            {
                components.Add(obj);
                components_by_entity.Add(comp.ownerid, obj);
            }
            //raise on added event
            raiseAddedEvent(comp, typeof(T));
        }
        /// <summary>
        /// used to init this pool
        /// </summary>
        public void Initialize()
        {
            current = this;
        }
        /// <summary>
        /// Use this to register a "ondirty" handler. This method will be called back when component of type T
        /// was marked as dirty.
        /// </summary>
        /// <param name="handler"></param>
        public void AddDirtyHandler(Action<T> handler)
        {
            if (handlers == null)
                handlers += handler;
            else
            {
                 handlers -= handler;
                handlers += handler;
            }
            return;
        }

        public void RemoveDirtyHandler(Action<T> handler)
        {
            if (handlers == null)
                return;
            handlers -= handler;
        }
    }
}




