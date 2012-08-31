using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Waitil
    {
        public object Message;
        public object[] Results;
        public Action<Coroutine.Factory, LinkedListNode<IEnumerator>> Init = null;
        public Coroutine.Factory Factory;

        public Waitil(object message)
        {
            Message = message;
            Init += (factory, node) =>
                {
                    Factory = factory;
                };
        }

        public Waitil Endon(object message, Action callback = null)
        {
            Init += (factory, node) =>
            {
                factory.Invoke(EndonHelper(message, node, Message, callback));
            };
            return this;
        }

        public override bool Equals(object obj)
        {
            return Message.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Message.GetHashCode();
        }

        static IEnumerator EndonHelper(object message, LinkedListNode<IEnumerator> node)
        {
            yield return message;
            node.List.Remove(node);
        }

        static IEnumerator EndonHelper(object message, LinkedListNode<IEnumerator> node, object endon, Action callback)
        {
            LinkedListNode<IEnumerator> helper = null;
            yield return new Waitil(message)
            {
                Init = (factory, enumerator) =>
                {
                    helper = factory.Invoke(EndonHelper(endon, enumerator));
                }
            };
            node.List.Remove(node);
            helper.List.Remove(helper);
            if (callback != null)
                callback();
        }
    }
}
