namespace ThingFilter
{
	internal interface IMatchEvaluator
	{
		TokenOperator Operation { get; }

		bool Match(string query, string target, bool caseSensitive);
		bool Match(double query, double target);
		bool Match(bool query, bool target);
	}
}
