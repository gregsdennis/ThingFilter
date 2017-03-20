using System.Collections.Generic;

namespace ThingFilter
{
	public interface IFilteredEnumerable<out T> : IEnumerable<T>
	{
		IEnumerable<T> Results { get; }
		IEnumerable<string> Warnings { get; }
	}
}
