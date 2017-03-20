using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ThingFilter
{
	internal class FilteredEnumerable<T> : IFilteredEnumerable<T>
	{
		public IEnumerable<T> Results { get; set; }

		public IEnumerable<string> Warnings { get; set; }

		public IEnumerator<T> GetEnumerator()
		{
			return Results?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
