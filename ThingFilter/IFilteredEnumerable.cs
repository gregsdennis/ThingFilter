using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingFilter
{
	public interface IFilteredEnumerable<T> : IEnumerable<T>
	{
		IEnumerable<T> Results { get; }
		IEnumerable<string> Warnings { get; }
	}
}
