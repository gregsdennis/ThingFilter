using System.Collections.Generic;

namespace ThingFilter
{
	internal class TaggedValue
	{
		public object Value { get; set; }
		public string Tag { get; set; }
		public bool RequiresTag { get; set; }
		public TokenOperator Operator { get; set; }
		public List<string> Warnings { get; } = new List<string>();
	}
}