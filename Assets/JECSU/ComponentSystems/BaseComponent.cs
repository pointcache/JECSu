namespace JECSU
{
    using System;
 
    public class BaseComponent
    { 
        private Entity _owner;
        public int ownerid { get; set; }
        public string ownername { get; set; }
        public Entity owner
        {
            get { return _owner; }
            set { _owner = value; ownerid = value.id; ownername = value.name; }
        }
        Type _type;
        public Type type
        { get { if (_type == null) _type = GetType(); return _type; } }

        //The pool subscribes to this event so it will get notified when the component is dirty.
        public event Action<BaseComponent> onDirty;
        
        /// <summary>
        /// Notify corresponding pool that this component was changed and needs to get updated.
        /// </summary>
        public void Dirty()
        {
            if (onDirty != null)
                onDirty(this);
        }
    }
}