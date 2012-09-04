using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public abstract class EntityBase
    {
        static public Dictionary<Type, LinkedList<WeakReference>> Entities = new Dictionary<Type, LinkedList<WeakReference>>();

        public readonly List<Component> Components;
        public readonly WeakReference TheWeakReference;

        public EntityBase()
        {
            Components = new List<Component>();
            TheWeakReference = new WeakReference(this);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in GetComponents<T>())
                return (T)component;
            return null;
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return (IEnumerable<T>)from i in Components where i.GetType() == typeof(T) select i;
        }
    }

    public class Entity<T> : EntityBase
    {
        LinkedListNode<WeakReference> Node;

        static Entity()
        {
            Entities.Add(typeof(T), new LinkedList<WeakReference>());
        }

        public Entity()
        {
            Entities[typeof(T)].AddLast(Node = new LinkedListNode<WeakReference>(TheWeakReference));
        }

        ~Entity()
        {
            Node.List.Remove(Node);
        }
    }

    public static partial class Extension
    {
        class WaitilEntity : Waitil
        {
            WeakReference Entity;

            public WaitilEntity(EntityBase entity, Waitil waitil)
                : base(waitil)
            {
                Entity = entity.TheWeakReference;
            }

            public override Engine.Waitil Endon(object message, Action callback = null)
            {
                return base.Endon(Tuple.Create(Entity, message), callback);
            }
        }

        public static Waitil Waitil(this EntityBase entity, object message)
        {
            return new WaitilEntity(entity, new Waitil(Tuple.Create(entity.TheWeakReference, message)));
        }

        public static Waitil WaitilAny(this EntityBase entity, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(entity.TheWeakReference, messages[i]);

            return new WaitilEntity(entity, new WaitilAny(messages));
        }

        public static Waitil WaitilAll(this EntityBase entity, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(entity.TheWeakReference, messages[i]);

            return new WaitilEntity(entity, new WaitilAll(messages));
        }

        public static void Send(this EntityBase entity, object message, params object[] results)
        {
            Coroutine.Send(Tuple.Create(entity.TheWeakReference, message), results);
        }
    }
}
