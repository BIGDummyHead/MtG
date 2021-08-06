using Gungeon.Debug;
using System;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Used for copying Fields and Properties from one type to another.
    /// </summary>
    public static class CopyCat
    {
        private const BindingFlags All = (BindingFlags)(-1);

        /// <summary>
        /// Copy a one object to another
        /// </summary>
        /// <param name="copyFrom"></param>
        /// <param name="copyTo"></param>
        /// <returns></returns>
        public static void Copy(this object copyFrom, object copyTo)
        {
            Type copyType = copyFrom.GetType();

            foreach (FieldInfo field in copyType.GetFields(All))
            {
                try
                {
                    copyTo.GetType().GetField(field.Name, All).SetValue(copyTo, field.GetValue(copyFrom));

                }
                catch
                {
                    $"Could not copy field : {field.Name}".LogError();
                }
            }

            foreach (PropertyInfo property in copyType.GetProperties(All))
            {
                try
                {
                    copyTo.GetType().GetProperty(property.Name, All).SetValue(copyTo, property.GetValue(copyFrom, null), null);

                }
                catch
                {
                    $"Could not copy property : {property.Name}".LogError();
                }
            }
        }


        /// <summary>
        /// Create an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="copyFrom"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static T Copy<T>(this object copyFrom, params object[] constructorArgs)
        {
            Type b = typeof(T);

            var c = b.GetConstructor(GetConstArgs(constructorArgs));

            if (c == null)
            {
                "Constructor does not exist".LogError();
                return default;
            }

            object instance = Activator.CreateInstance(b, constructorArgs);

            Copy(copyFrom, instance);

            return (T)instance;
        }

        static Type[] GetConstArgs(object[] args)
        {
            Type[] _a = new Type[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                _a[i] = args[i].GetType();
            }

            return _a;
        }

    }
}
