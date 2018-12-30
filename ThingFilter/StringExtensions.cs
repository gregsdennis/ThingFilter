using System.Globalization;

namespace ThingFilter
{
	internal static class StringExtensions
	{
		public const string TermSplitPatternFormat = @"(^|\s)((?<tag>\w+)?(?<operator>({0})))?((""(?<quoted>(?:[^""\\]|\\.)*)"")|(?<unquoted>\w+))?";

		public static bool Contains(this string source, string search, CompareOptions comparison, CultureInfo culture = null)
		{
			if (source == null) return false;

			culture = culture ?? CultureInfo.CurrentCulture;

			return culture.CompareInfo.IndexOf(source, search, comparison) >= 0;
		}
	}
}
