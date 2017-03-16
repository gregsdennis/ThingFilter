using System.Globalization;

namespace Filter
{
	public static class StringExtensions
	{
		public static bool Contains(this string source, string search, CompareOptions comparison, CultureInfo culture = null)
		{
			if (source == null) return false;

			culture = culture ?? CultureInfo.CurrentCulture;

			return culture.CompareInfo.IndexOf(source, search, comparison) >= 0;
		}
	}
}
