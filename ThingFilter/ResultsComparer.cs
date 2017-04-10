using System.Collections.Generic;

namespace ThingFilter {
	internal class ResultsComparer<T> : IEqualityComparer<FilterResult<T>>
	{
		public static ResultsComparer<T> Instance { get; }

		static ResultsComparer()
		{
			Instance = new ResultsComparer<T>();
		}
		private ResultsComparer() { }

		public bool Equals(FilterResult<T> x, FilterResult<T> y)
		{
			return Equals(x.Item, y.Item);
		}
		public int GetHashCode(FilterResult<T> obj)
		{
			return obj.Item.GetHashCode();
		}
	}
}