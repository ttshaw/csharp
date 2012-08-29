using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Coroutine
    {
        public delegate IEnumerator Post(LinkedListNode<IEnumerator> node);

        public Dictionary<object, LinkedList<IEnumerator>> routines;

        public Coroutine()
        {
            routines = new Dictionary<object, LinkedList<IEnumerator>>();
        }

        public void Create(IEnumerator coroutine)
        {
            if (!coroutine.MoveNext())
                return;

            LinkedListNode<IEnumerator> node = new LinkedListNode<IEnumerator>(coroutine);

            if (coroutine.Current is Post)
            {
                Create(((Post)coroutine.Current)(node));
                if (!coroutine.MoveNext())
                    return;
            }

            try
            {
                routines[coroutine.Current.GetType()].AddLast(node);
            }
            catch (KeyNotFoundException)
            {
                LinkedList<IEnumerator> list = new LinkedList<IEnumerator>();
                list.AddLast(node);
                routines.Add(coroutine.Current, list);
            }
        }

        public void Send(object message)
        {
            LinkedList<IEnumerator> value;
            if (!routines.TryGetValue(message, out value))
                return;

            routines.Remove(message);

            foreach (IEnumerator coroutine in value)
                Create(coroutine);
        }
    }
}
