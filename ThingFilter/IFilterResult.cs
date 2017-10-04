﻿using System.Collections.Generic;

namespace ThingFilter
{
	/// <summary>
	/// Contains the matched item as well as the details regarding the match.
	/// </summary>
	/// <typeparam name="T">The type of object which was matched.</typeparam>
	public interface IFilterResult<out T>
	{
		/// <summary>
		/// Gets the item which was matched.
		/// </summary>
		T Item { get; }
		/// <summary>
		/// Gets the tokens that matched the item.
		/// </summary>
		IEnumerable<string> Matches { get; }
		/// <summary>
		/// Gets any warnings generated by the match (under construction).
		/// </summary>
		IEnumerable<string> Warnings { get; }
		/// <summary>
		/// Gets the score for the match.
		/// </summary>
		int Score { get; }
	}
}
