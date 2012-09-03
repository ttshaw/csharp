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
            return component.Waitil(message);
        }

        public static Waitil WaitilAny(this Component component, params object[] messages)
        {
            return component.WaitilAny(messages);
        }

        public static Waitil WaitilAll(this Component component, params object[] messages)
        {
            return component.WaitilAll(messages);
        }

        public static void Send(this Component component, object message, params object[] results)
        {
            component.Send(results);
        }
    }
}
