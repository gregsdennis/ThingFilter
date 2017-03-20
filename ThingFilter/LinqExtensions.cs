using System.Collections.Generic;

namespace ThingFilter
{
	public static class LinqExtensions
	{
		public static IEnumerable<IFilterResult<T>> Filter<T>(this IEnumerable<T> collection, IFilter<T> filter, string query)
		{
			return filter.Apply(collection, query);
		}
	}
}
