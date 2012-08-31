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
            public Dictionary<object, LinkedList<IEnumerator>> Routines = new Dictionary<object, LinkedList<IEnumerator>>();

            public LinkedListNode<IEnumerator> Invoke(IEnumerator enumerator)
            {
                if (!enumerator.MoveNext())
                    return null;

                LinkedList<IEnumerator> list;
                if (!Routines.TryGetValue(enumerator.Current, out list))
                {
                    list = new LinkedList<IEnumerator>();
                    Routines.Add(enumerator.Current, list);
                }

                LinkedListNode<IEnumerator> node = new LinkedListNode<IEnumerator>(enumerator);
                list.AddLast(node);

                if (enumerator.Current is Waitil && ((Waitil)enumerator.Current).Init != null)
                    ((Waitil)enumerator.Current).Init(node);

                return node;
            }

            public void Send(object message, params object[] results)
            {
                LinkedList<IEnumerator> value;
                if (!Routines.TryGetValue(message, out value))
                    return;

                Routines.Remove(message);

                foreach (IEnumerator coroutine in value)
                {
                    if (coroutine.Current is Waitil)
                        ((Waitil)coroutine.Current).Results = results;
                    Invoke(coroutine);
                }
            }
        }

        static Factory TheFactory = new Factory();

        static public LinkedListNode<IEnumerator> Invoke(IEnumerator enumerator)
        {
            return TheFactory.Invoke(enumerator);
        }

        static public void Send(object message, params object[] results)
        {
            TheFactory.Send(message, results); 
        }
    }
}
