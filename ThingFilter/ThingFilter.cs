using System;
using System.Collections.Generic;
using System.Linq;

namespace ThingFilter
{
	public class ThingFilter<T> : IFilter<T>
	{
		private readonly List<TaggedDelegate> _allTargets = new List<TaggedDelegate>();
		private readonly TaggedValueComparer _comparer = new TaggedValueComparer();
		private bool _sort;

		public IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc, string tag = null, bool requireTag = false)
		{
			tag = tag?.Trim();
			var tagIsEmpty = string.IsNullOrEmpty(tag);
			if (requireTag && tagIsEmpty)
				throw new ArgumentException($"Parameter '{nameof(tag)}' must be provided if parameter '{nameof(requireTag)}' is true.");

			_allTargets.Add(new TaggedDelegate
				{
					Delegate = valueFunc,
					Tag = tag,
					RequireTag = requireTag
				});
			return this;
		}

		public IFilter<T> CaseSensitive()
		{
			_comparer.IsCaseSensitive = true;
			return this;
		}

		public IFilter<T> SortByRelevance()
		{
			_sort = true;
			return this;
		}

		public IFilteredEnumerable<T> Apply(IEnumerable<T> collection, string text)
		{
			var tokens = text.GetTokens().ToList();

			var values = collection.Select(i => new
				{
					Item = i,
					Values = _GetValues(i).ToList()
				});

			var results = values.Select(i => new
				{
					Item = i.Item,
					Score = i.Values.Intersect(tokens, _comparer).Count()
				});

			IEnumerable<T> filteredResults;

			if (_sort)
				filteredResults = results.GroupBy(i => i.Score)
				                         .OrderByDescending(g => g.Key)
				                         .Where(g => g.Key != 0)
				                         .SelectMany(g => g.Select(i => i.Item))
				                         .Distinct();
			else
				filteredResults = results.Where(i => i.Score != 0)
				                         .Select(g => g.Item)
				                         .Distinct();

			// TODO: Create warnings
			return new FilteredEnumerable<T>
				{
					Results = filteredResults
				};
		}

		private IEnumerable<TaggedValue> _GetValues<TValue>(TValue value)
		{
			var values = _allTargets.Select(e => new TaggedValue
				{
					Value = e.Delegate.DynamicInvoke(value),
					Tag = e.Tag,
					RequiresTag = e.RequireTag
				});

			return values;
		}
	}
}
