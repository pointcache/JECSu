/// <summary>
/// This is the manager class of entity system.
/// It must be present in scene for system to work.
/// </summary>
namespace JECSU
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Serialization;
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager current;
        public bool initDatabase;
        //All our entities
        static List<Entity> _entities = new List<Entity>(10000);
        //Cached entities by entity id
        static Dictionary<int, Entity> _entities_by_id = new Dictionary<int, Entity>(10000);
        /// <summary>
        /// All entities
        /// </summary>
        public static List<Entity> entities { get { return _entities; } }
        /// <summary>
        /// Cached list of components stored using Entity id as a key.
        /// </summary>
        public static Dictionary<int, List<IComponent>> components_database_id = new Dictionary<int, List<IComponent>>(100000);
        /// <summary>
        /// Last used id for a new entity.
        /// </summary>
        static int currentId = 0;

        public static event Action<Entity> OnEntityRegistered;

        public static TemplateDatabase database;
        public static JECSUConfig config;
        void Awake()
        {
            current = this;
            if (initDatabase)
            {
                database = new TemplateDatabase();
            }
        }

        void FixedUpdate()
        {
            ExecuteSystems();
        }

        /// <summary>
        /// Returns new unique id number.
        /// </summary>
        /// <returns></returns>
        public static int GetUID()
        {
            currentId++;
            return currentId;
        }

        /// <summary>
        /// Executes all execute systems in order of creation
        /// </summary>
        void ExecuteSystems()
        {
            int count = Systems.executeSystems.Count;
            for (int i = 0; i < count; i++)
            {
                Systems.executeSystems[i].Execute();
            }
        }

        /// <summary>
        /// When entity is created it has to be processed by the system.
        /// </summary>
        /// <param name="ent"></param>
        public static void RegisterEntity(Entity ent)
        {
            if (_entities_by_id.ContainsKey(ent.id))
                throw new Exception("Trying to register already existing entity, some programming error there.");
            if(!components_database_id.ContainsKey(ent.id))
            {
                //Add component list to our database
                components_database_id.Add(ent.id, new List<IComponent>());
            }
            _entities.Add(ent);
            _entities_by_id.Add(ent.id, ent);

            if (OnEntityRegistered != null)
                OnEntityRegistered(ent);
        }

        /// <summary>
        /// New components have to be registered in the system
        /// </summary>
        /// <param name="comp"></param>
        public static void RegisterComponent(IComponent comp)
        {
            if (components_database_id.ContainsKey(comp.ownerid))
                components_database_id[comp.ownerid].Add(comp);
            else
            {
                components_database_id.Add(comp.ownerid, new List<IComponent>());
                 components_database_id[comp.ownerid].Add(comp);
            }
            //Send the component to pool, it will handle pool creation and component storage
            Pool.AddToPool(comp);
        }


    }
}