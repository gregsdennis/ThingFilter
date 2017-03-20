using System;
using System.Collections.Generic;

namespace ThingFilter
{
	public interface IFilter<T>
	{
		IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc, string tag = null, bool requireTag = false);
		IFilter<T> CaseSensitive();
		IFilter<T> SortByRelevance();
		IEnumerable<T> Apply(IEnumerable<T> collection, string query);
	}
}
