namespace ThingFilter
{
	internal class TaggedValue
	{
		public object Value { get; set; }
		public string Tag { get; set; }
		public bool RequiresTag { get; set; }
	}
}