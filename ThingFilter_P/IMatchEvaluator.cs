namespace ThingFilter
{
	/// <summary>
	/// Evaluates query tokens and values to determine if a match has been found.
	/// </summary>
	public interface IMatchEvaluator
	{
		/// <summary>
		/// Gets the operator for this evaluator.  This value will be used between the tag and the query.
		/// </summary>
		string Operation { get; }

		/// <summary>
		/// Determines whether a match is found for string values.
		/// </summary>
		/// <param name="query">The query token.</param>
		/// <param name="target">The target value to match.</param>
		/// <param name="caseSensitive"><c>true</c> if the comparison should consider case; <c>false</c> otherwise.</param>
		/// <returns><c>true</c> if a match is found; <c>false</c> otherwise.</returns>
		bool Match(string query, string target, bool caseSensitive);
		/// <summary>
		/// Determines whether a match is found for numeric values.
		/// </summary>
		/// <param name="query">The query token.</param>
		/// <param name="target">The target value to match.</param>
		/// <returns><c>true</c> if a match is found; <c>false</c> otherwise.</returns>
		bool Match(double query, double target);
		/// <summary>
		/// Determines whether a match is found for boolean values.
		/// </summary>
		/// <param name="query">The query token.</param>
		/// <param name="target">The target value to match.</param>
		/// <returns><c>true</c> if a match is found; <c>false</c> otherwise.</returns>
		bool Match(bool query, bool target);
	}
}
