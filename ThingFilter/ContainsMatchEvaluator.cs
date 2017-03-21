using System.Globalization;

namespace ThingFilter
{
	internal class ContainsMatchEvaluator : IMatchEvaluator
	{
		public string Operation => ":";

		public bool Match(string query, string target, bool caseSensitive)
		{
			var comparison = caseSensitive ? CompareOptions.None : CompareOptions.IgnoreCase;
			return target.Contains(query, comparison);
		}
		public bool Match(double query, double target)
		{
			return query == target;
		}
		public bool Match(bool query, bool target)
		{
			return query == target;
		}
	}
}