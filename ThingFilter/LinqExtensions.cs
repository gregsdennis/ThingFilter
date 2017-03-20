using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingFilter
{
	public static class LinqExtensions
	{
		public static IFilteredEnumerable<T> Filter<T>(this IEnumerable<T> collection, IFilter<T> filter, string query)
		{
			return filter.Apply(collection, query);
		}
	}
}
