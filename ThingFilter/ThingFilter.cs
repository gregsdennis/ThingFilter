using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ThingFilter
{
	public class ThingFilter<T>
	{
		private class ContainsComparer : IEqualityComparer<string>
		{
			private readonly CompareOptions _comparison;

			public static ContainsComparer CaseSensitive { get; }
			public static ContainsComparer CaseInsensitive { get; }

			static ContainsComparer()
			{
				CaseSensitive = new ContainsComparer(CompareOptions.None);
				CaseInsensitive = new ContainsComparer(CompareOptions.IgnoreCase);
			}
			private ContainsComparer(CompareOptions comparison)
			{
				_comparison = comparison;
			}


			public bool Equals(string x, string y)
			{
				var contains = y.Contains(x, _comparison);
				return contains;
			}
			public int GetHashCode(string obj)
			{
				return 0;
			}
		}

		private readonly List<Delegate> _expressions = new List<Delegate>();
		private ContainsComparer _comparer = ContainsComparer.CaseInsensitive;
		private bool _sort;

		public ThingFilter<T> MatchOn<TProp>(Func<T, TProp> propertyExpression)
		{
			_expressions.Add(propertyExpression);
			return this;
		}

		public ThingFilter<T> CaseSensitive()
		{
			_comparer = ContainsComparer.CaseSensitive;
			return this;
		}

		public ThingFilter<T> SortByRelevance()
		{
			_sort = true;
			return this;
		}

		public IEnumerable<T> Apply(IEnumerable<T> collection, string text)
		{
			var words = text.GetTermsAdvanced().ToList();

			var results = collection.Select(i => new
				{
					Item = i,
					ExpressionValues = _GetExprValueStrings(i)
				});

			var filteredResults = results.Select(i => new
				{
					Item = i.Item,
					Score = i.ExpressionValues.Intersect(words, _comparer).Count()
				});

			if (_sort)
				return filteredResults.GroupBy(i => i.Score)
				                      .OrderByDescending(g => g.Key)
				                      .Where(g => g.Key != 0)
				                      .SelectMany(g => g.Select(i => i.Item))
				                      .Distinct();

			return filteredResults.Where(i => i.Score != 0)
			                      .Select(g => g.Item)
			                      .Distinct();
		}

		private IEnumerable<string> _GetExprValueStrings<TValue>(TValue value)
		{
			var values = _expressions.Select(e => e.DynamicInvoke(value)).Select(p => p?.ToString());

			return values;
		}
	}
}
