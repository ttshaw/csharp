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
        public object message;
        public object[] results;

        public Waitil(object message)
        {
            this.message = message;
        }

        public override bool Equals(object obj)
        {
            return message.Equals(obj);
        }

        public override int GetHashCode()
        {
            return message.GetHashCode();
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
            Coroutine.Endon(waitany.message);

            yield return new Waitil(message);
            Coroutine.Send(waitany.message, message);
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
                Coroutine.Send(waitall.message);
        }
    }
}
