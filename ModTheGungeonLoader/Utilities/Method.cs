using System;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Represents a reflected method
    /// </summary>
    public sealed class Method
    {
        /// <summary>
        /// The type this method belongs to
        /// </summary>
        public Type Owner { get; private set; }

        /// <summary>
        /// The name of the method you are reflecting
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The Type[] arguments for a specific method.
        /// </summary>
        public Type[] Arguments { get; private set; }

        internal readonly MethodInfo _method;
        internal readonly object instance;

        internal Method(Type owner, string name, Type[] args, object instance)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Name = name;
            Arguments = args ?? throw new ArgumentNullException(nameof(args));
            this.instance = instance;

            _method = owner.GetMethod(name, ReflectionHandler.All, null, default, args, new ParameterModifier[0]) ?? throw new Exception("Method could not be found");
        }

        /// <summary>
        /// Invoke the method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(params object[] args)
        {
            return _method.Invoke(instance, args);
        }

        /// <summary>
        /// <see cref="Invoke(object[])"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[params object[] index] => Invoke(index);
    }
}
