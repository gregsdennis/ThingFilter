using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThingFilter
{
	public static class StringExtensions
	{
		private static readonly Regex TermSplitPattern = new Regex(@"(""(?<search>(?:[^""\\]|\\.)*)"")|\w+");

		public static bool Contains(this string source, string search, CompareOptions comparison, CultureInfo culture = null)
		{
			if (source == null) return false;

			culture = culture ?? CultureInfo.CurrentCulture;

			return culture.CompareInfo.IndexOf(source, search, comparison) >= 0;
		}

		public static IEnumerable<string> GetTerms(this string source)
		{
			// splits on all whitespace
			return source.Split(null);
		}

		public static IEnumerable<string> GetTermsAdvanced(this string source)
		{
			var matches = TermSplitPattern.Matches(source);
			return matches.Cast<Match>().Select(m =>
				{
					var groupMatch = m.Groups["search"]?.Value;
					return string.IsNullOrEmpty(groupMatch) ? m.Value : groupMatch;
				});
		}
	}
}
