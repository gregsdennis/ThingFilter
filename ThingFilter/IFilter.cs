using System;
using System.Collections.Generic;

namespace ThingFilter
{
	public interface IFilter<T>
	{
		IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc,
		                          string tag = null,
		                          bool requireTag = false,
		                          int weight = 1);
		IFilter<T> CaseSensitive();
		IFilter<T> IncludeUnmatched();
		IFilter<T> AddEvaluator(IMatchEvaluator evaluator);
		IFilter<T> RemoveEvaluator(string @operator);

		IEnumerable<IFilterResult<T>> Apply(IEnumerable<T> collection, string query);
	}
}
