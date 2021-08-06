using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gungeon.Utilities
{
	/// <summary>
	/// <see cref="StringTableManager.StringCollection"/>
	/// </summary>
    internal sealed class ImplicitStringCollection : StringTableManager.StringCollection
    {
		/// <summary>
		/// 
		/// </summary>
		/// <returns>1</returns>
		public override int Count()
		{
			return 1;
		}

		/// <summary>
		/// Weight does not matter.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="weight"></param>
		public override void AddString(string value, float weight)
		{
			this.singleString = value;
		}

		/// <summary>
		/// Returns string value
		/// </summary>
		/// <returns></returns>
		public override string GetCombinedString()
		{
			return this.singleString;
		}

		/// <summary>
		/// Non-indexable
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public override string GetExactString(int index)
		{
			return this.singleString;
		}

		/// <summary>
		/// Non-weighted
		/// </summary>
		/// <returns></returns>
		public override string GetWeightedString()
		{
			return this.singleString;
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="lastIndex"></param>
		/// <param name="isLast"></param>
		/// <param name="repeatLast"></param>
		/// <returns></returns>
		public override string GetWeightedStringSequential(ref int lastIndex, out bool isLast, bool repeatLast = false)
		{
			isLast = true;
			return this.singleString;
		}

		/// <summary>
		/// Implicit pass
		/// </summary>
		/// <param name="impl"></param>
		public static implicit operator ImplicitStringCollection(string impl)
        {
			ImplicitStringCollection imp = new ImplicitStringCollection();
			imp.AddString(impl, 1);
			return imp;
        }

		private string singleString;
	}
}
