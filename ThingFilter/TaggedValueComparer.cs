using System;
using System.Collections.Generic;
using System.Linq;

namespace ThingFilter
{
	internal class TaggedValueComparer : IEqualityComparer<TaggedValue>
	{
		private static readonly IEnumerable<IMatchEvaluator> Evaluators =
			new List<IMatchEvaluator>
				{
					new ContainsMatchEvaluator(),
					new EqualToMatchEvaluator(),
					new NotEqualToMatchEvaluator(),
					new LessThanMatchEvaluator(),
					new LessThanEqualMatchEvaluator(),
					new GreaterThanMatchEvaluator(),
					new GreaterThanEqualMatchEvaluator()
				};

		public bool IsCaseSensitive { get; set; }

		public bool Equals(TaggedValue query, TaggedValue target)
		{
			if (target.Value == null) return false;
			if (query.Tag == null && target.Tag != null && target.RequiresTag) return false;
			if (query.Tag != null && query.Tag != target.Tag) return false;

			var contains = _PerformComparison((string) query.Value, target.Value, query.Operator);
			return contains;
		}
		public int GetHashCode(TaggedValue obj)
		{
			return 0;
		}

		private bool _PerformComparison(string query, object target, TokenOperator operation)
		{
			var evaluator = Evaluators.FirstOrDefault(e => e.Operation == operation);
			if (evaluator == null)
				throw new ArgumentOutOfRangeException(nameof(operation));

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