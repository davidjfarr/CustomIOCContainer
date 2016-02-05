using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DFIoC
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Fixture
    {
        [Test]
        public void CanCreateParameterlessTYpe()
        {
            var container = CreateContainer();
            var foo = container.Resolve<Foo>();
            Assert.NotNull(foo);
            Assert.IsInstanceOf<Foo>(foo);
        }

        [Test]
        public void CanResolveParameterlessType()
        {
            var container = CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            var foo = container.Resolve<IFoo>();
            Assert.NotNull(foo);
            Assert.IsInstanceOf<Foo>(foo);
        }


        [Test]
        public void CanResolveTypeWithDependencies()
        {
            var container = CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            container.Register<IBar, Bar>(Lifetime.Instance);
            var bar = container.Resolve<IBar>();
            Assert.NotNull(bar);
            Assert.IsInstanceOf<Bar>(bar);
        }


        [Test]
        public void CanCreateTypeWithDependencies()
        {
            IFoo foo = new Foo();
            IBar bar = new Bar(foo);
            Assert.NotNull(bar);
            Assert.IsInstanceOf<Bar>(bar);
        }

        //If the type isnt registerd fail.


        IContainer CreateContainer()
        {
            return new DavidContainer();//******************   Implement me   ******************   
        }
    }


    public interface IContainer
    {
        void Register<TContract, TImplementation>(Lifetime expectedLifetime) where TImplementation : class, TContract;
        TContract Resolve<TContract>();
    }
    public enum Lifetime
    {
        Singleton,
        Instance
    }



    public interface IFoo
    {
        string GetFoo();
    }
    public interface IBar
    {
        string GetBar();
    }

    public class Foo : IFoo
    {
        static int instanceCount = 0;
        private readonly int myInstaceId;
        public Foo()
        {
            instanceCount++;
            myInstaceId = instanceCount;
        }
        public string GetFoo()
        {
            return "This is Foo " + myInstaceId + " of " + instanceCount;
        }
    }
    public class Bar : IBar
    {
        static int instanceCount = 0;
        private readonly int myInstaceId;
        private readonly IFoo foo;
        public Bar(IFoo foo)
        {
            instanceCount++;
            myInstaceId = instanceCount;
            this.foo = foo;
        }
        public string GetBar()
        {
            return "This is Bar " + myInstaceId + " of " + instanceCount + ", and I have a Foo : " + foo.GetFoo();
        }
    }


    [Serializable]
    public class NotRegisteredException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NotRegisteredException()
        {
        }

        public NotRegisteredException(string message)
            : base(message)
        {
        }

        public NotRegisteredException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotRegisteredException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class DavidContainer : IContainer
    {
        // need to pair a particular type with a instance of this type
        private readonly Dictionary<Type, Type> _registrations;

        public DavidContainer()
        {
            _registrations = new Dictionary<Type, Type>();
        }

        public void Register<TContract, TImplementation>(Lifetime expectedLifetime) where TImplementation : class, TContract
        {
            this._registrations.Add(typeof(TContract), typeof(TImplementation));

            // expectedLifetime either Singleton or Instance


        }

        public TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public object Resolve(Type typeResolve)
        {
            if (typeResolve.IsAbstract)
            {
                var implementationType = _registrations[typeResolve];
                var ctor = implementationType.GetConstructors()[0];
                var parameterInfos = ctor.GetParameters();

                if (parameterInfos.Length == 0)
                {
                    return CreateInstance(implementationType);
                }
                else
                {
                    //var paramInstances = new object[parameterInfos.Length];
                    //for (int index = 0; index < parameterInfos.Length; index++)
                    //{
                    //    paramInstances[index] = Resolve(parameterInfos[index].ParameterType);
                    //}

                    //var i = 0;
                    //foreach (var parameterInfo in parameterInfos)
                    //{
                    //    var type = parameterInfo.ParameterType;
                    //    var instance = Resovle(type);
                    //    paramInstances[i++] = instance;
                    //}
                    var paramInstances = parameterInfos.Select(info => info.ParameterType)
                        .Select(Resolve)
                        .ToArray();

                    return ctor.Invoke(paramInstances);
                }

                // if parameters size is 0 (no parms)
                // do normal
                // else
                //      get params types and lookup in dict


            }
            else
            {
                return CreateInstance(typeResolve);
            }
            // 
        }

        private object GetInstance(Type type, Lifetime expectedLifetime)
        {
            throw new NotImplementedException();
        }

        private static T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
        private static object CreateInstance(Type type)
        {
            var ctr = type.GetConstructor(new Type[0]);
            return ctr.Invoke(new object[0]);
        }
    }
}