using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThingFilter
{
	internal static class StringExtensions
	{
		private static readonly Regex TermSplitPattern = new Regex(@"((?<tag>\w+)(?<operator>(:|=|<>|<|<=|>|>=)))?((""(?<quoted>(?:[^""\\]|\\.)*)"")|(?<unquoted>\w+))");

		public static bool Contains(this string source, string search, CompareOptions comparison, CultureInfo culture = null)
		{
			if (source == null) return false;

			culture = culture ?? CultureInfo.CurrentCulture;

			return culture.CompareInfo.IndexOf(source, search, comparison) >= 0;
		}

		public static IEnumerable<TaggedValue> GetTokens(this string source)
		{
			var matches = TermSplitPattern.Matches(source);
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
					Operator = _MapOperator(operatorString)
				};
		}

		private static TokenOperator _MapOperator(string operatorString)
		{
			switch (operatorString)
			{
				case null:
				case "":
				case ":":
					return TokenOperator.Contains;
				case "=":
					return TokenOperator.EqualTo;
				case "<>":
					return TokenOperator.NotEqualTo;
				case "<":
					return TokenOperator.LessThan;
				case "<=":
					return TokenOperator.LessThanEqualTo;
				case ">":
					return TokenOperator.GreaterThan;
				case ">=":
					return TokenOperator.GreaterThanEqualTo;
				default:
					throw new ArgumentOutOfRangeException(nameof(operatorString));
			}
		}
	}
}
