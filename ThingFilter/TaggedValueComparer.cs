using System;
using System.Collections.Generic;
using System.Globalization;

namespace ThingFilter
{
	internal class TaggedValueComparer : IEqualityComparer<TaggedValue>
	{
		private readonly CompareOptions _comparison;

		public static TaggedValueComparer CaseSensitive { get; }
		public static TaggedValueComparer CaseInsensitive { get; }

		static TaggedValueComparer()
		{
			CaseSensitive = new TaggedValueComparer(CompareOptions.None);
			CaseInsensitive = new TaggedValueComparer(CompareOptions.IgnoreCase);
		}
		private TaggedValueComparer(CompareOptions comparison)
		{
			_comparison = comparison;
		}


		public bool Equals(TaggedValue query, TaggedValue target)
		{
			if (target.Value == null) return false;
			if (query.Tag == null && target.Tag != null && target.RequiresTag) return false;
			if (query.Tag != null && query.Tag != target.Tag) return false;

			var contains = _PerformGeneralComparison((string) query.Value, target.Value);
			return contains;
		}
		public int GetHashCode(TaggedValue obj)
		{
			return 0;
		}

		private bool _PerformGeneralComparison(string query, object target)
		{
			if (target is bool)
				return _PerformBooleanComparison(query, (bool) target);
			if (target is sbyte || target is byte || target is char ||
			    target is short || target is ushort ||
			    target is int || target is uint ||
			    target is long || target is ulong ||
			    target is float || target is double ||
			    target is decimal)
				return _PerformNumericComparison(query, Convert.ToDouble(target));

			return target.ToString().Contains(query, _comparison);
		}

		private bool _PerformBooleanComparison(string query, bool target)
		{
			bool boolQuery;
			return bool.TryParse(query, out boolQuery) && boolQuery == target;
		}

		private bool _PerformNumericComparison(string query, double target)
		{
			double doubleQuery;
			return double.TryParse(query, out doubleQuery) && doubleQuery == target;
		}
	}
}