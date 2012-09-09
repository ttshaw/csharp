using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Coroutine
    {
        class Factory
        {
            public readonly Dictionary<object, LinkedList<IEnumerator>> routines = new Dictionary<object, LinkedList<IEnumerator>>();

            public event Action<LinkedListNode<IEnumerator>> OnDoneDuringInvoking;

            public LinkedListNode<IEnumerator> Invoke(IEnumerator enumerator)
            {
                OnDoneDuringInvoking = null;

                if (!enumerator.MoveNext())
                    return null;

                LinkedList<IEnumerator> list;
                if (!routines.TryGetValue(enumerator.Current, out list))
                {
                    list = new LinkedList<IEnumerator>();
                    routines.Add(enumerator.Current, list);
                }

                LinkedListNode<IEnumerator> node = new LinkedListNode<IEnumerator>(enumerator);
                list.AddLast(node);

                if (OnDoneDuringInvoking != null) OnDoneDuringInvoking(node);

                return node;
            }

            public void Send(object message, params object[] results)
            {
                LinkedList<IEnumerator> list;
                if (!routines.TryGetValue(message, out list))
                    return;

                routines.Remove(message);

                while (list.First != null)
                {
                    IEnumerator enumerator = list.First.Value;
                    list.RemoveFirst();
                    if (enumerator.Current is Waitil)
                        ((Waitil)enumerator.Current).results = results;
                    Invoke(enumerator);
                }
            }
        }

        static Factory theFactory = new Factory();

        static public LinkedListNode<IEnumerator> Invoke(IEnumerator enumerator)
        {
            return theFactory.Invoke(enumerator);
        }

        static public void Send(object message, params object[] results)
        {
            theFactory.Send(message, results); 
        }

        static public void Endon(object message, Action callback = null)
        {
            theFactory.OnDoneDuringInvoking += (node) => 
            {
                Invoke(EndonHelper(message, node, callback));
            };
        }

        static IEnumerator EndonHelper(object message, LinkedListNode<IEnumerator> node, Action callback)
        {
            yield return message;

            if (node.List != null)
            {
                node.List.Remove(node);
                if (callback != null) callback();
            }
        }
    }
}
