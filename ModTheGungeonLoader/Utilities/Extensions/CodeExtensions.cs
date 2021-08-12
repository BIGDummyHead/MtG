using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Extensions for <see cref="Method"/> and <seealso cref="Variable"/>
    /// </summary>
    public static class CodeExtensions
    {
        /// <summary>
        /// A recursive method that checks all base types of <paramref name="child"/> and compares to <paramref name="baseType"/>
        /// </summary>
        /// <param name="child"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool Inherits(this Type child, Type baseType)
        {
            if (child == baseType || child.BaseType == baseType)
                return true;
            else if (child.BaseType == typeof(Object))
                return false;
            return child.BaseType.Inherits(baseType);
        }

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
        /// Get a random item from any IEnumerable item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public static T Random<T>(this IEnumerable<T> ts, Func<T, bool> requirements = null)
        {
            T[] ar = default;

            if (requirements != null)
                ar = ts.Where(requirements).ToArray();
            else
                ar = ts.ToArray();

            int ray = UnityEngine.Random.Range(0, ar.Length - 1);

            //get it ;)
            return ar[ray];
        }

        /// <summary>
        /// Check if a type inherits an interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inter"></param>
        /// <returns></returns>
        public static bool HasInterface(this Type type, Type inter)
        {
            if (!inter.IsInterface)
                throw new Exception("Must pass interface to check if type has interface.");

            Type t = type.GetInterfaces().FirstOrDefault(x => x == inter);

            return t != default;
        }

        /// <summary>
        /// Check if a type inherits an interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasInterface<T>(this Type type)
        {
            return type.HasInterface(typeof(T));
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

        /// <summary>
        /// Try parse string to enum
        /// </summary>
        /// <param name="etype">ONLY ENUMS</param>
        /// <param name="parse">String to parse</param>
        /// <param name="ignoreCase">Ignore Enum casing rules?</param>
        /// <param name="parsed">Final parsed enum.</param>
        /// <returns></returns>
        public static bool TryParse(Type etype, string parse, bool ignoreCase, out object parsed)
        {
            string[] names = Enum.GetNames(etype);

            if (ignoreCase)
            {
                names = LowerAll(names);
                parse = parse.ToLower();
            }

            List<string> _contains = names.ToList();

            if (_contains.Contains(parse))
            {
                Array values = Enum.GetValues(etype);
                parsed = null;

                StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                foreach (object value in values)
                {
                    if (value.ToString().Equals(parse, comparison))
                        parsed = value;
                }

                return true;
            }

            parsed = null;
            return false;
        }

        /// <summary>
        /// Try and parse an enum by string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse">String to parse</param>
        /// <param name="ignoreCase">Ignore Enum casing rules?</param>
        /// <param name="parsed">Final parsed enum.</param>
        /// <returns></returns>
        public static bool TryParse<T>(string parse, bool ignoreCase, out T parsed) where T : Enum
        {
            bool ret = TryParse(typeof(T), parse, ignoreCase, out object b);

            if (!ret)
            {
                parsed = default;
                return false;
            }

            parsed = (T)b;
            return true;
        }

        /// <summary>
        /// Transform any <see cref="IEnumerable{T}"/> into a <seealso cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<T> instance)
        {
            return new List<T>(instance);
        }

        private static string[] LowerAll(string[] toLower)
        {
            for (int i = 0; i < toLower.Length; i++)
            {
                toLower[i] = toLower[i].ToLower();
            }

            return toLower;
        }
    }
}
