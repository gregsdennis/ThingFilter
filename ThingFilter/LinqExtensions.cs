using System.Collections.Generic;

namespace ThingFilter
{
	/// <summary>
	/// Provides Linq-style extensions for <see cref="IFilter{T}"/>.
	/// </summary>
	public static class LinqExtensions
	{
		/// <summary>
		/// Applies a filter to an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of object in the collection.</typeparam>
		/// <param name="collection">The collection of <typeparamref name="T"/>.</param>
		/// <param name="filter">The filter to be applied.</param>
		/// <param name="query">The query to filter against.</param>
		/// <returns>A collection of <see cref="IFilterResult{T}"/> that contains the matches.</returns>
		public static IEnumerable<IFilterResult<T>> Filter<T>(this IEnumerable<T> collection, IFilter<T> filter, string query)
		{
			return filter.Apply(collection, query);
		}
	}
}
