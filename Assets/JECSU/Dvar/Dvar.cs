namespace JECSU.Dvar
{
    using System;

    //Currently not used, but later will be
    //the concept of dynamic variable is that it allows you to observe and interact with 
    //basic types as if they were objects, this allows for ui binding, config and other types
    //of behaviors.
    public class dvarBase
    {
        public string name;
        public object obj_value;
        public Type type;

        /// <summary>
        /// Hack to allow subscribing to variables of unknown type
        /// Does the same as "Subscribe()"
        /// </summary>
        public Action OnChangedBase;

        public override string ToString()
        {
            return obj_value.ToString();
        }
    }


    public class dvar<T> : dvarBase where T : new()
    {
        Action<T> OnChanged;
        private T value;
        BaseComponent parent;
        Type parentType;

        public dvar<T> Set(T val)
        {
            //SLog.print("var>" + name + " set:" + val.ToString() + parent.log());
            value = val;
            if (OnChanged != null)
                OnChanged(val);
            if (OnChangedBase != null)
                OnChangedBase();
            return this;
        }
        /// <summary>
        /// Will not raise notification
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public dvar<T> SetSilent(T val)
        {
            value = val;
            obj_value = val;
            return this;
        }

        public static implicit operator T(dvar<T> var)
        {
            return var.value;
        }

        public void Initialise(Type parentType, BaseComponent parent)
        {

        }

        public dvar<T> SetFromString(string value)
        {
            Set((T)System.Convert.ChangeType(value, typeof(T)));
            return this;
        }

        public string GetToString()
        {
            return value.ToString();
        }

        public dvar<T> ClearSubscribers()
        {
            OnChanged = null;
            return this;
        }

        public dvar<T> Subscribe(Action<T> callback)
        {
            OnChanged += callback;
            return this;
        }

        public dvar<T> Unsubscribe(Action<T> callback)
        {
            OnChanged -= callback;
            return this;
        }
        /// <summary>
        /// Will raise event for all subscribers
        /// </summary>
        public dvar<T> Update()
        {
            Set(value);
            return this;
        }
    }
}
