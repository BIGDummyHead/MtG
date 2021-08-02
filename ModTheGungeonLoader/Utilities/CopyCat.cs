using Gungeon.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
            Type pasteType = copyTo.GetType();

            if(!ChildInheritsParent(pasteType, copyType) || copyType != pasteType)
            {
                "Types do not match and conversion could not be made.".LogError();
                return;
            }

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

        static bool ChildInheritsParent(Type child, Type parent)
        {
            if (child.BaseType == null)
                return false;

            if (child.BaseType == parent)
                return true;
            else
                return ChildInheritsParent(child.BaseType, parent);
        }

    }
}
