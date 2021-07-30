using System;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Extensions for <see cref="Method"/> and <seealso cref="Variable"/>
    /// </summary>
    public static class ReflectionHandler
    {
        internal static BindingFlags All => (BindingFlags)(-1);

        /// <summary>
        /// Reflect a method; Public, Internal, and or Private.
        /// </summary>
        /// <param name="instance">An instance of an object, used for type and invoking.</param>
        /// <param name="name">Name</param>
        /// <param name="typeArgs">Arguments in types</param>
        /// <returns></returns>
        public static Method GetMethod(this object instance, string name, params Type[] typeArgs)
        {
            return new Method(instance?.GetType(), name, typeArgs, instance);
        }

        /// <summary>
        /// Reflect a static method; Public, Internal, and or Private.
        /// </summary>
        /// <param name="owner">Owner type</param>
        /// <param name="name">Name</param>
        /// <param name="typeArgs">Arguments in types</param>
        /// <returns></returns>
        public static Method GetStaticMethod(this Type owner, string name, params Type[] typeArgs)
        {
            return new Method(owner, name, typeArgs, null);
        }

        /// <summary>
        /// Get a field or property by name
        /// </summary>
        /// <param name="instance">Object instance</param>
        /// <param name="name">Name of field or property</param>
        /// <returns></returns>
        public static Variable GetVariable(this object instance, string name)
        {
            return new Variable(instance, instance?.GetType(), name);
        }

        /// <summary>
        /// Get a static field or property by name
        /// </summary>
        /// <param name="owner">Static field or property owner</param>
        /// <param name="name">Name of field or property</param>
        /// <returns></returns>
        public static Variable GetStaticVariable(this Type owner, string name)
        {
            return new Variable(null, owner, name);
        }
    }
}
