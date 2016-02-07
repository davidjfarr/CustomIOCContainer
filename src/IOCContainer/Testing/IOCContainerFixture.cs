using System;
using IOCContainer.Container;
using IOCContainer.CustomException;
using IOCContainer.Testing.Resources;
using Xunit;

namespace IOCContainer.Testing
{
    public class IocContainerFixture
    {
        [Fact]
        public void GivenRegisteredTypeCanResolveParameterlessType()
        {
            var container = Builder.CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            var foo = container.Resolve<IFoo>();
            Assert.NotNull(foo);
            Assert.IsType<Foo>(foo);
        }

        [Fact]
        public void RegisteredTypeCanResolveTypeWithDependencies()
        {
            var container = Builder.CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            container.Register<IBar, Bar>(Lifetime.Instance);
            var bar = container.Resolve<IBar>();
            Assert.NotNull(bar);
            Assert.IsType<Bar>(bar);
        }

        [Fact]
        public void RegisteredTypeWithInstanceLifelineResolvesNewInstance()
        {
            var container = Builder.CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();
            Assert.NotEqual(foo1, foo2);
        }

        [Fact]
        public void TypeRegisteredWithSingletonLifetimeAlwaysResolvesSameInstance()
        {
            var container = Builder.CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Singleton);
            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();
            Assert.Equal(foo1, foo2);
        }

        [Fact]
        public void ExceptionIsThrownIfUnregisteredTypeIsProvided()
        {
            var container = Builder.CreateContainer();
            Assert.Throws<NotRegisteredException>(() => container.Resolve<string>());
        }

        [Fact]
        public void ExceptionIsThrownIfDuplicateContractTypeIsRegistered()
        {
            var container = Builder.CreateContainer();
            container.Register<IFoo, Foo>(Lifetime.Instance);
            Assert.Throws<ArgumentException>(() => container.Register<IFoo, Foo>(Lifetime.Instance));
        }
    }
}
