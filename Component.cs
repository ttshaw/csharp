using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Component
    {
        public readonly EntityBase entity;

        public Component(EntityBase entity)
        {
            this.entity = entity;
            this.entity.components.Add(this);
        }
    }

    public static partial class Extension
    {
        public static Waitil Waitil(this Component component, object message)
        {
            return component.entity.Waitil(message);
        }

        public static Waitil WaitilAny(this Component component, params object[] messages)
        {
            return component.entity.WaitilAny(messages);
        }

        public static Waitil WaitilAll(this Component component, params object[] messages)
        {
            return component.entity.WaitilAll(messages);
        }

        public static void Send(this Component component, object message, params object[] results)
        {
            component.entity.Send(results);
        }

        public static void Endon(this Component component, object message, Action callback = null)
        {
            component.entity.Endon(message, callback);
        }
    }
}
