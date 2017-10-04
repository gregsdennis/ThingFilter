using System;
using System.Collections.Generic;

namespace ThingFilter
{
	/// <summary>
	/// Defines a object that filters collections.
	/// </summary>
	/// <typeparam name="T">The content type of the collection to be filtered.</typeparam>
	public interface IFilter<T>
	{
		/// <summary>
		/// Defines a value on which to match.
		/// </summary>
		/// <typeparam name="TProp">The type of the value that will be matched.</typeparam>
		/// <param name="valueFunc">A function which takes an item from the collection and returns the value to match.</param>
		/// <param name="tag">(Optional) A tag to be used in the query string to isolate this value.</param>
		/// <param name="requireTag">(Optional) <c>true</c> if the tag should be required to match this value; <c>false</c> otherwise. The default is <c>false</c>.</param>
		/// <param name="weight">(Optional) A custom weighting for this value.  The default is 1.</param>
		/// <returns>The filter instance (this).</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="valueFunc"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="tag"/> is null but <paramref name="requireTag"/> is <c>true</c>.</exception>
		IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc,
		                          string tag = null,
		                          bool requireTag = false,
		                          int weight = 1);
		/// <summary>
		/// Indicates that this filter should consider case.
		/// </summary>
		/// <returns>The filter instance (this).</returns>
		IFilter<T> CaseSensitive();
		/// <summary>
		/// Indicates that this filter will include unmatched items in the result set.  By default, they are excluded.
		/// </summary>
		/// <returns>The filter instance (this).</returns>
		IFilter<T> IncludeUnmatched();
		/// <summary>
		/// Adds a custom match evaluator.
		/// </summary>
		/// <param name="evaluator">The match evaluator instance.</param>
		/// <returns>The filter instance (this).</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="evaluator"/> is null.</exception>
		IFilter<T> AddEvaluator(IMatchEvaluator evaluator);
		/// <summary>
		/// Removes a match evaluator.
		/// </summary>
		/// <param name="operator">The operator that identifies the match evaluator.</param>
		/// <returns>The filter instance (this).</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="operator"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="operator"/> is empty or whitespace.</exception>
		IFilter<T> RemoveEvaluator(string @operator);

		/// <summary>
		/// Applies the filter to a collection.
		/// </summary>
		/// <param name="collection">The collection of <typeparamref name="T"/>.</param>
		/// <param name="query">The query to filter against.</param>
		/// <returns>A collection of <see cref="IFilterResult{T}"/> that contains the matches.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
		IEnumerable<IFilterResult<T>> Apply(IEnumerable<T> collection, string query);
	}
}
