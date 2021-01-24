using System;
using System.Collections.Generic;

namespace RightTurn
{
    internal class TurnDirections : Dictionary<Type, object>, ITurnDirections
    {       
        public T Add<T>(T value)
        {
            this[typeof(T)] = value;
            return value;
        }

        public bool Have<T>()
        {
            return ContainsKey(typeof(T));
        }

        public bool Have<T>(out T value)
        {
            return (value = TryGet<T>()) != null;
        }

        public T Get<T>()
        {
            return (T)this[typeof(T)];
        }

        public T TryGet<T>()
        {
            if (!ContainsKey(typeof(T)))
                return default;

            return (T)this[typeof(T)];
        }
        
    }
}
