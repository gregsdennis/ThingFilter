using System;

namespace ThingFilter
{
	internal class GreaterThanMatchEvaluator : IMatchEvaluator
	{
		public TokenOperator Operation => TokenOperator.GreaterThan;

		public bool Match(string query, string target, bool caseSensitive)
		{
			var comparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
			return 0 > string.Compare(query, target, comparison);
		}
		public bool Match(double query, double target)
		{
			return target > query;
		}
		public bool Match(bool query, bool target)
		{
			return false;
		}
	}
}