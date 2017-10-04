using System.Collections.Generic;

namespace ThingFilter
{
	internal class FilterResult<T> : IFilterResult<T>
	{
		public T Item { get; set; }
		public IEnumerable<string> Warnings { get; set; }
		public IEnumerable<string> Matches { get; set; }
		public int Score { get; set; }
	}
}
