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
			if (query.Tag == null && target.Tag != null && target.RequiresTag) return false;
			if (query.Tag != null && query.Tag != target.Tag) return false;
			
			var contains = target.Value.Contains(query.Value, _comparison);
			return contains;
		}
		public int GetHashCode(TaggedValue obj)
		{
			return 0;
		}
	}
}