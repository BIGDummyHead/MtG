using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gungeon.Utilities.DatabaseIDs
{
    /// <summary>
    /// A read only dictionary that can not be added to.
    /// </summary>
    /// <typeparam name="In"></typeparam>
    /// <typeparam name="Out"></typeparam>
    public sealed class ReadOnlyDictionary<In, Out>
    {
        private Dictionary<In, Out> __dic = new Dictionary<In, Out>();

        /// <summary>
        /// Init from dictionary
        /// </summary>
        /// <param name="values"></param>
        public ReadOnlyDictionary(IEnumerable<KeyValuePair<In, Out>> values)
        {
            foreach (var item in values)
            {
                __dic.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Index through Dictionary
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Out this[In index] => __dic[index];

        /// <summary>
        /// Has key in dictionary?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool  HasKey(In key)
        {
            return __dic.ContainsKey(key);
        }

        /// <summary>
        /// Has value in dictionary?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HasValue(Out value)
        {
            return __dic.ContainsValue(value);
        }
        /// <summary>
        /// Try to get value without throwing error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(In key, out Out value)
        {
            if(HasKey(key))
            {
                value = __dic[key];
                return true;
            }

            value = default;
            return false;
        }
    }
}
