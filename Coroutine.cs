using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Coroutine
    {
        public Dictionary<object, List<System.Collections.IEnumerator>> coroutines;

        public Coroutine()
        {
            coroutines = new Dictionary<object, List<System.Collections.IEnumerator>>();
        }

        public void Create(System.Collections.IEnumerator coroutine)
        {
            if (!coroutine.MoveNext())
                return;

            try
            {
                coroutines[coroutine.Current.GetType()].Add(coroutine);
            }
            catch (KeyNotFoundException)
            {
                coroutines.Add(coroutine.Current, new List<System.Collections.IEnumerator>() { coroutine });
            }
        }

        public void Send(object message)
        {
            List<System.Collections.IEnumerator> value;
            if (!coroutines.TryGetValue(message, out value))
                return;

            coroutines.Remove(message);

            foreach (System.Collections.IEnumerator coroutine in value)
                Create(coroutine);
        }
    }
}
