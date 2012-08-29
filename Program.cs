using System;
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
        }
    }

    class Program
    {
        class _EndOn
        {
            public object message;
            public IEnumerator Post(LinkedListNode<IEnumerator> node)
            {
                yield return message;
                node.List.Remove(node);
            }
        }

        static Coroutine.Post EndOn(object message)
        {
            return new Coroutine.Post(new _EndOn() { message = message }.Post);
        }

        static IEnumerator Hello()
        {
            yield return EndOn(0);
            yield return 2;
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

            Coroutine co = new Coroutine();
            co.Create(Hello());
            co.Send(2);
            co.Send(0);

            Console.WriteLine("hello world");
        }
    }
}
