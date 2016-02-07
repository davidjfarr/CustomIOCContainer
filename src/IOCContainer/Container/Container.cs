using System;
using System.Collections.Generic;
using System.Linq;
using IOCContainer.CustomException;

namespace IOCContainer.Container
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type>  _registrations = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> _singletonInstances = new Dictionary<Type, object>();

        public void Register<TContract, TImplementation>(Lifetime expectedLifetime) where TImplementation : class, TContract
        {
            if (_registrations.ContainsKey(typeof(TContract)))
                throw new ArgumentException("TContract type is already registered");

            _registrations.Add(typeof (TContract), typeof (TImplementation));
            if (expectedLifetime != Lifetime.Singleton) return;
            var instance = Resolve(typeof (TContract));
            _singletonInstances.Add(typeof (TContract), instance);

            // TODO: Need to check for case where unsupported Lifetime value is provided.
        }

        public TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public object Resolve(Type resolveType)
        {
            if (_singletonInstances.ContainsKey(resolveType))
                return _singletonInstances[resolveType];
            
            Type implementationType;
            if (_registrations.TryGetValue(resolveType, out implementationType))
                return CreateInstance(implementationType);

            throw new NotRegisteredException("No registration for type: " + resolveType);
        }

        private object CreateInstance(Type implementationType)
        {
            var constructor = implementationType.GetConstructors().Single();
            if (constructor == null)
                throw new InvalidOperationException("No valid constructor found for type: " + implementationType);

            var parameterInfo = constructor.GetParameters();
            if (parameterInfo.Length == 0) 
                return Activator.CreateInstance(implementationType);

            var parameterInstances = parameterInfo.Select(info => info.ParameterType)
                .Select(Resolve)
                .ToArray();
            return Activator.CreateInstance(implementationType, parameterInstances);
        }
    }
}
