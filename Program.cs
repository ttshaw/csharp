﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Constructor |
        AttributeTargets.Field |
        AttributeTargets.Method |
        AttributeTargets.Property,
        AllowMultiple = true)]
    public class UberAttribute : System.Attribute
    {
        string hint;
        public UberAttribute(string hint)
        {
            this.hint = hint;
        }
    }

    class Charater : Entity<Charater>
    {
        [UberAttribute("test")]
        public int id = 0;

        public Charater(int id = 0)
        {
            this.id = id;
        }
    }

    class Trigger : Entity<Trigger>
    {
        public Trigger()
        {
            new Component(this);

            Coroutine.Invoke(OnSomething());
        }

        IEnumerator OnSomething()
        {
            yield return this.Waitil(0).Endon(-1);
        }
    }

    class Program
    {
        static IEnumerator Hello()
        {
            for (Waitil waitil; ; )
            {
                yield return waitil = new WaitilAll(0, 1).Endon(-1, () =>
                {
                    Console.WriteLine("end...");
                });
                break;
            }
        }

        static void Main(string[] args)
        {
            foreach (Type entityType in
                    from i in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                    where i.BaseType.IsGenericType && i.BaseType.GetGenericTypeDefinition() == typeof(Entity<>)
                    select i)
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(entityType.BaseType.TypeHandle);

            Charater player = new Charater();
            Charater player2 = new Charater();
            Console.WriteLine(player.id++);

            Trigger trigger = new Trigger();
            Component trigger_component = trigger.GetComponent<Component>();
            var trigger_components = trigger.GetComponents<Component>();

            var infs = from i in typeof(Charater).GetFields() where Attribute.IsDefined(i, typeof(UberAttribute)) select i;
            foreach (System.Reflection.FieldInfo i in infs)
            {
                var value = i.GetValue(player);
            }

            Coroutine.Invoke(Hello());
            Coroutine.Send(0);
            Coroutine.Send(-1);
            Coroutine.Send(1);
            player.Send(0);

            Console.WriteLine("hello world");
        }
    }
}
