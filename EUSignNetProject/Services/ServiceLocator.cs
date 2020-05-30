using System;
using System.Collections.Generic;

namespace EUSignNetProject.Services
{
    /// <summary>
    /// Basic service locator.
    /// </summary>
    public static class ServiceLocator
    {
        // TODO: ConcurrentDictionary?
        // TODO: weak references?
        private static readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();
        private static readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a concrete instance to return when the specified type is resolved.
        /// </summary>
        /// <typeparam name="TInstance">The type to register.</typeparam>
        /// <param name="instance">The instance.</param>
        public static void Register<TInstance>(TInstance instance)
        {
            // TODO: remove type from services if present
            if (_instances.ContainsKey(typeof(TInstance)))
            {
                _instances[typeof(TInstance)] = instance;
            }
            else
            {
                _instances.Add(typeof(TInstance), instance);
            }
        }

        /// <summary>
        /// Registers a resolver function which will returns a new instance of the specific type when it is resolved.
        /// </summary>
        /// <typeparam name="TInstance">The type to register.</typeparam>
        /// <param name="resolver">The resolver function that will resolve the type.</param>
        public static void Register<TInstance>(Func<TInstance> resolver)
        {
            // TODO: remove type from instances if present
            if (_services.ContainsKey(typeof(TInstance)))
            {
                _services[typeof(TInstance)] = () => resolver();
            }
            else
            {
                _services.Add(typeof(TInstance), () => resolver());
            }
        }

        /// <summary>
        /// Resolves a service by type.
        /// </summary>
        /// <typeparam name="TInstance">The type of the service to be resolved.</typeparam>
        /// <returns>The resolved service.</returns>
        public static TInstance Resolve<TInstance>()
            where TInstance : class
        {
            if (_instances.ContainsKey(typeof(TInstance)))
            {
                return _instances[typeof(TInstance)] as TInstance;
            }
            else if (_services.ContainsKey(typeof(TInstance)))
            {
                return _services[typeof(TInstance)]() as TInstance;
            }
            else
            {
                return default(TInstance);
            }
        }

        /// <summary>
        /// Clears the dictionaries.
        /// </summary>
        public static void Clear()
        {
            _services.Clear();
            _instances.Clear();
        }
    }
}
