using System;

namespace ThingFilter
{
	internal class NotEqualToMatchEvaluator : IMatchEvaluator
	{
		public TokenOperator Operation => TokenOperator.NotEqualTo;

		public bool Match(string query, string target, bool caseSensitive)
		{
			var comparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
			return !string.Equals(query, target, comparison);
		}
		public bool Match(double query, double target)
		{
			return query != target;
		}
		public bool Match(bool query, bool target)
		{
			return query != target;
		}
	}
}