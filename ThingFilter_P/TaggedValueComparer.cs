using System;
using System.Collections.Generic;
using System.Linq;

namespace ThingFilter
{
	internal class TaggedValueComparer : IEqualityComparer<TaggedValue>
	{
		public List<IMatchEvaluator> Evaluators { get; } =
			new List<IMatchEvaluator>
				{
					new ContainsMatchEvaluator(),
					new EqualToMatchEvaluator(),
					new NotEqualToMatchEvaluator(),
					new LessThanMatchEvaluator(),
					new LessThanEqualMatchEvaluator(),
					new GreaterThanMatchEvaluator(),
					new GreaterThanEqualMatchEvaluator(),
				};

		public bool IsCaseSensitive { get; set; }

		public int GetScore(List<List<TaggedValue>> queries, List<TaggedValue> targets)
		{
			return queries.Sum(query =>
				{
					var local = query.Sum(q => targets.Sum(t => (Equals(q, t) ? 1 : 0) * t.Weight));
					var matches = query.All(q => targets.Any(t => Equals(q, t)));
					return matches ? local : 0;
				});
		}

		public bool Equals(TaggedValue query, TaggedValue target)
		{
			if (target.Value == null) return false;
			if (query.Tag == null && target.Tag != null && target.RequiresTag) return false;
			if (query.Tag != null && query.Tag != target.Tag) return false;

			var value = query.Value as string;
			if (value == null) return false;

			var matches = _PerformComparison(value, target.Value, query.Operator);
			return matches;
		}

		public int GetHashCode(TaggedValue obj)
		{
			return 0;
		}

		private bool _PerformComparison(string query, object target, string operation)
		{
			var evaluator = string.IsNullOrEmpty(operation)
				                ? Evaluators.FirstOrDefault()
				                : Evaluators.FirstOrDefault(e => e.Operation == operation);
			if (evaluator == null) return false;

			if (target is bool)
			{
				bool boolQuery;
				return bool.TryParse(query, out boolQuery) && evaluator.Match(boolQuery, (bool) target);
			}
			if (target is sbyte || target is byte || target is char ||
			    target is short || target is ushort ||
			    target is int || target is uint ||
			    target is long || target is ulong ||
			    target is float || target is double ||
			    target is decimal)
			{
				double doubleQuery;
				return double.TryParse(query, out doubleQuery) && evaluator.Match(doubleQuery, Convert.ToDouble(target));
			}

			return evaluator.Match(query, target.ToString(), IsCaseSensitive);
		}
	}
}