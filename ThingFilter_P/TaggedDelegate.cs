using System;

namespace ThingFilter
{
	internal class TaggedDelegate
	{
		public Delegate Delegate { get; set; }
		public string Tag { get; set; }
		public bool RequireTag { get; set; }
		public int Weight { get; set; }
	}
}