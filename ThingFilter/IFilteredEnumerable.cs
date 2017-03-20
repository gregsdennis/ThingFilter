using System.Collections.Generic;

namespace ThingFilter
{
	public interface IFilterResult<out T>
	{
		T Item { get; }
		IEnumerable<string> Matches { get; }
		IEnumerable<string> Warnings { get; }
		int Score { get; }
	}
}
