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

        public Coroutine coroutine;

        public EntityBase()
        {
            components = new List<Component>();
            coroutine = new Coroutine();
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

        public void Coroutine(System.Collections.IEnumerator coroutine)
        {
            this.coroutine.Create(coroutine);
        }

        public void Send(object message)
        {
            coroutine.Send(message);
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
