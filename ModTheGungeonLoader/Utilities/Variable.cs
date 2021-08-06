using System;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Reflection that covers both fields and properties.
    /// </summary>
    public sealed class Variable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Owner type
        /// </summary>
        public Type Owner { get; private set; }

        /// <summary>
        /// Is this variable a field?
        /// </summary>
        public bool IsField { get; private set; }

        /// <summary>
        /// Is this variable a property?
        /// </summary>
        public bool IsProperty => !IsField;

        private object instance;
        private FieldInfo _field;
        private PropertyInfo _prop;

        /// <exception cref="Exception"/>
        /// <exception cref="ArgumentNullException"/>
        internal Variable(object instance, Type own, string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.instance = instance;
            Owner = own ?? throw new ArgumentNullException(nameof(own));

            MemberInfo tmp;

            if ((tmp = Owner.GetField(name, CodeExtensions.All)) != null)
            {
                _field = tmp as FieldInfo;
                IsField = true;
            }
            else if ((tmp = Owner.GetProperty(name, CodeExtensions.All)) != null)
            {
                _prop = tmp as PropertyInfo;
                IsField = false;
            }
            else
                throw new Exception("This field or property does not exist.");
        }

        /// <summary>
        /// Get the value of a field or property
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (IsField)
                return _field.GetValue(instance);

            return _prop.GetValue(instance, null);
        }

        /// <summary>
        /// Get a T type of your <see cref="GetValue"/>
        /// </summary>
        /// <typeparam name="T">Return type of your field or property</typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return (T)GetValue();
        }

        /// <summary>
        /// Set the value of your field or property
        /// </summary>
        /// <param name="newVal"></param>
        /// <exception cref="Exception"/>
        public void SetValue(object newVal)
        {
            if (IsField)
            {
                if (_field.FieldType != newVal.GetType())
                    throw new Exception("Your new value's type must be the same as the field's return type!");

                _field.SetValue(instance, newVal);
            }
            else
            {
                if (_prop.PropertyType != newVal.GetType())
                    throw new Exception("Your new value's type must be the same as the property's return type!");

                _prop.SetValue(instance, newVal, null);
            }
        }
    }
}
