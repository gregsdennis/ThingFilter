﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThingFilter
{
	/// <summary>
	/// Implements <see cref="IFilter{T}"/>.
	/// </summary>
	/// <typeparam name="T">The content type of the collection to be filtered.</typeparam>
	public class ThingFilter<T> : IFilter<T>
	{
		private readonly List<TaggedDelegate> _allTargets = new List<TaggedDelegate>();
		private readonly TaggedValueComparer _comparer = new TaggedValueComparer();
		private bool _includeUnmatched;

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
		public IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc, string tag = null, bool requireTag = false, int weight = 1)
		{
			if (valueFunc == null)
				throw new ArgumentNullException(nameof(valueFunc));

			tag = tag?.Trim();
			var tagIsEmpty = string.IsNullOrEmpty(tag);
			if (requireTag && tagIsEmpty)
				throw new ArgumentException($"Parameter '{nameof(tag)}' must be provided if parameter '{nameof(requireTag)}' is true.");

			_allTargets.Add(new TaggedDelegate
				{
					Delegate = valueFunc,
					Tag = tag,
					RequireTag = requireTag,
					Weight = weight
				});
			return this;
		}

		/// <summary>
		/// Indicates that this filter should consider case.
		/// </summary>
		/// <returns>The filter instance (this).</returns>
		public IFilter<T> CaseSensitive()
		{
			_comparer.IsCaseSensitive = true;
			return this;
		}

		/// <summary>
		/// Indicates that this filter will include unmatched items in the result set.  By default, they are excluded.
		/// </summary>
		/// <returns>The filter instance (this).</returns>
		public IFilter<T> IncludeUnmatched()
		{
			_includeUnmatched = true;
			return this;
		}

		/// <summary>
		/// Adds a custom match evaluator.
		/// </summary>
		/// <param name="evaluator">The match evaluator instance.</param>
		/// <returns>The filter instance (this).</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="evaluator"/> is null.</exception>
		public IFilter<T> AddEvaluator(IMatchEvaluator evaluator)
		{
			if (evaluator == null)
				throw new ArgumentNullException();

			_comparer.Evaluators.Add(evaluator);
			return this;
		}

		/// <summary>
		/// Removes a match evaluator.
		/// </summary>
		/// <param name="operator">The operator that identifies the match evaluator.</param>
		/// <returns>The filter instance (this).</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="operator"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="operator"/> is empty or whitespace.</exception>
		public IFilter<T> RemoveEvaluator(string @operator)
		{
			if (@operator == null)
				throw new ArgumentNullException(nameof(@operator));
			if (string.IsNullOrWhiteSpace(@operator))
				throw new ArgumentException("operator must be non-empty and non-whitespace.");

			var evaluator = _comparer.Evaluators.FirstOrDefault(e => e.Operation == @operator);
			_comparer.Evaluators.Remove(evaluator);
			return this;
		}

		/// <summary>
		/// Applies the filter to a collection.
		/// </summary>
		/// <param name="collection">The collection of <typeparamref name="T"/>.</param>
		/// <param name="query">The query to filter against.</param>
		/// <returns>A collection of <see cref="IFilterResult{T}"/> that contains the matches.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
		public IEnumerable<IFilterResult<T>> Apply(IEnumerable<T> collection, string query)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			var tokens = _GetTokens(query).ToList();

			var values = collection.Select(i => new
				{
					Item = i,
					Values = _GetValues(i).ToList()
				});

			var results = values.Select(i => new FilterResult<T>
				{
					Item = i.Item,
					Score = i.Values.Intersect(tokens, _comparer).Sum(v => v.Weight)
				});

			if (!_includeUnmatched)
				results = results.Where(i => i.Score != 0);

			// TODO: Create warnings
			return results;
		}

		private IEnumerable<TaggedValue> _GetValues<TValue>(TValue value)
		{
			var values = _allTargets.Select(e => new TaggedValue
				{
					Value = e.Delegate.DynamicInvoke(value),
					Tag = e.Tag,
					RequiresTag = e.RequireTag,
					Weight = e.Weight
				});

			return values;
		}

		private IEnumerable<TaggedValue> _GetTokens(string source)
		{
			var pattern = new Regex(string.Format(StringExtensions.TermSplitPatternFormat,
			                                      string.Join("|", _comparer.Evaluators.Select(e => e.Operation))));
			var matches = pattern.Matches(source);
			return matches.Cast<Match>().Select(_ParseToken);
		}

		private static TaggedValue _ParseToken(Match m)
		{
			var quoted = m.Groups["quoted"]?.Value.Trim();
			var unquoted = m.Groups["unquoted"]?.Value.Trim();
			var term = string.IsNullOrEmpty(quoted) ? unquoted : quoted;
			var tag = m.Groups["tag"]?.Value.Trim();
			var operatorString = m.Groups["operator"]?.Value.Trim();
			return new TaggedValue
			{
				Tag = string.IsNullOrEmpty(tag) ? null : tag,
				Value = term,
				Operator = operatorString
			};
		}
	}
}
