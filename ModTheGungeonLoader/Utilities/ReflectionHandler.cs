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
            return new Method(instance.GetType(), name, typeArgs, instance);
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
    }
}
