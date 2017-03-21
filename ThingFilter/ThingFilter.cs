using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThingFilter
{
	public class ThingFilter<T> : IFilter<T>
	{
		private readonly List<TaggedDelegate> _allTargets = new List<TaggedDelegate>();
		private readonly TaggedValueComparer _comparer = new TaggedValueComparer();
		private bool _includeUnmatched;

		public IFilter<T> MatchOn<TProp>(Func<T, TProp> valueFunc, string tag = null, bool requireTag = false)
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
					RequireTag = requireTag
				});
			return this;
		}

		public IFilter<T> CaseSensitive()
		{
			_comparer.IsCaseSensitive = true;
			return this;
		}

		public IFilter<T> IncludeUnmatched()
		{
			_includeUnmatched = true;
			return this;
		}

		public IFilter<T> AddEvaluator(IMatchEvaluator evaluator)
		{
			if (evaluator == null)
				throw new ArgumentNullException();

			_comparer.Evaluators.Add(evaluator);
			return this;
		}

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

		public IEnumerable<IFilterResult<T>> Apply(IEnumerable<T> collection, string text)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			var tokens = _GetTokens(text).ToList();

			var values = collection.Select(i => new
				{
					Item = i,
					Values = _GetValues(i).ToList()
				});

			var results = values.Select(i => new FilterResult<T>
				{
					Item = i.Item,
					Score = i.Values.Intersect(tokens, _comparer).Count()
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
					RequiresTag = e.RequireTag
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
