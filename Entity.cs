using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public abstract class EntityBase
    {
        static public Dictionary<Type, LinkedList<WeakReference>> entities = new Dictionary<Type, LinkedList<WeakReference>>();

        public readonly List<Component> components = new List<Component>();
        public readonly WeakReference theWeakReference;

        public EntityBase()
        {
            theWeakReference = new WeakReference(this);
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

    public class Entity<T> : EntityBase
    {
        LinkedListNode<WeakReference> node;

        static Entity()
        {
            entities.Add(typeof(T), new LinkedList<WeakReference>());
        }

        public Entity()
        {
            entities[typeof(T)].AddLast(node = new LinkedListNode<WeakReference>(theWeakReference));
        }

        ~Entity()
        {
            node.List.Remove(node);
        }
    }

    public static partial class Extension
    {
        class WaitilEntity : Waitil
        {
            WeakReference entity;

            public WaitilEntity(EntityBase entity, Waitil waitil)
                : base(waitil)
            {
                this.entity = entity.theWeakReference;
            }

            public override Engine.Waitil Endon(object message, Action callback = null)
            {
                return base.Endon(Tuple.Create(entity, message), callback);
            }
        }

        public static Waitil Waitil(this EntityBase entity, object message)
        {
            return new WaitilEntity(entity, new Waitil(Tuple.Create(entity.theWeakReference, message)));
        }

        public static Waitil WaitilAny(this EntityBase entity, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(entity.theWeakReference, messages[i]);

            return new WaitilEntity(entity, new WaitilAny(messages));
        }

        public static Waitil WaitilAll(this EntityBase entity, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(entity.theWeakReference, messages[i]);

            return new WaitilEntity(entity, new WaitilAll(messages));
        }

        public static void Send(this EntityBase entity, object message, params object[] results)
        {
            Coroutine.Send(Tuple.Create(entity.theWeakReference, message), results);
        }
    }
}
