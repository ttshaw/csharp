using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class EntityBase
    {
        static public Dictionary<Type, List<object>> entities = new Dictionary<Type, List<object>>();

        public List<Component> components;
        public Dictionary<object, List<System.Collections.IEnumerator>> coroutines;

        public EntityBase()
        {
            components = new List<Component>();
            coroutines = new Dictionary<object, List<System.Collections.IEnumerator>>();
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in GetComponents<T>())
                return (T)component;
            return null;
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return (IEnumerable<T>)from i in components where i.GetType() == typeof(T) select i;
        }

        public void StartCoroutine(System.Collections.IEnumerator coroutine)
        {
            if (!coroutine.MoveNext())
                return;

            try
            {
                coroutines[coroutine.Current].Add(coroutine);
            }
            catch (KeyNotFoundException)
            {
                coroutines.Add(coroutine.Current, new List<System.Collections.IEnumerator>() { coroutine });
            }
        }

        public void SendCoroutine(object message)
        {
            List<System.Collections.IEnumerator> value;
            if (!coroutines.TryGetValue(message, out value))
                return;

            coroutines.Remove(message);

            foreach (System.Collections.IEnumerator coroutine in value)
                coroutine.MoveNext();
        }
    }

    class Entity<T> : EntityBase
    {
        static Entity()
        {
            entities.Add(typeof(T), new List<object>());
        }

        public Entity()
        {
            entities[typeof(T)].Add(this);
        }
    }
}
