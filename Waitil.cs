using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Engine
{
    public class Waitil
    {
        public object Message;
        public object[] Results;
        public Action<LinkedListNode<IEnumerator>> Init = null;

        public Waitil(object message)
        {
            Message = message;
        }

        public virtual Waitil Endon(object message, Action callback = null)
        {
            Debug.Assert(message != Message);

            Init += (node) =>
            {
                Coroutine.Invoke(EndonHelper(message, node, Message, callback));
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

            if (node.List != null)
                node.List.Remove(node);
        }

        static IEnumerator EndonHelper(object message, LinkedListNode<IEnumerator> node, object endon, Action callback)
        {
            LinkedListNode<IEnumerator> helper = null;
            yield return new Waitil(message)
            {
                Init = (enumerator) =>
                {
                    helper = Coroutine.Invoke(EndonHelper(endon, enumerator));
                }
            };

            if (helper.List != null)
                helper.List.Remove(helper);

            if (node.List != null)
            {
                node.List.Remove(node);

                if (callback != null)
                    callback();
            }
        }
    }

    public class WaitilAny : Waitil
    {
        public WaitilAny(params object[] messages)
            : base(new object())
        {
            foreach (object message in messages)
                Coroutine.Invoke(Helper(message, this));
        }

        static IEnumerator Helper(object message, WaitilAny waitany)
        {
            yield return new Waitil(message).Endon(waitany.Message);
            Coroutine.Send(waitany.Message, message);
        }
    }

    public class WaitilAll : Waitil
    {
        int HowManyToWait = 0;

        public WaitilAll(params object[] messages)
            : base(new object())
        {
            HowManyToWait = messages.Length;

            foreach (object message in messages)
                Coroutine.Invoke(Helper(message, this));
        }

        static IEnumerator Helper(object message, WaitilAll waitall)
        {
            yield return message;

            if (--waitall.HowManyToWait <= 0)
                Coroutine.Send(waitall.Message);
        }
    }
}
