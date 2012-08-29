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

        public EntityBase()
        {
            components = new List<Component>();
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
