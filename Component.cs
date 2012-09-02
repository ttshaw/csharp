using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Component
    {
        public readonly EntityBase Entity;

        public Component(EntityBase entity)
        {
            Entity = entity;
            Entity.Components.Add(this);
        }
    }

    public static partial class Extension
    {
        public static Waitil Waitil(this Component component, object message)
        {
            return new WaitilEntity(component.Entity, new Waitil(Tuple.Create(component.Entity, message)));
        }

        public static Waitil WaitilAny(this Component component, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(component.Entity, messages[i]);

            return new WaitilEntity(component.Entity, new WaitilAny(messages));
        }

        public static Waitil WaitilAll(this Component component, params object[] messages)
        {
            for (int i = 0, size = messages.Length; i < size; ++i)
                messages[i] = Tuple.Create(component.Entity, messages[i]);

            return new WaitilEntity(component.Entity, new WaitilAll(messages));
        }

        public static void Send(this Component component, object message, params object[] results)
        {
            Coroutine.Send(Tuple.Create(component.Entity, message), results);
        }
    }
}
