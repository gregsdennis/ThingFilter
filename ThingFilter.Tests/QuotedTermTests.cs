using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThingFilter.Tests
{
	[TestClass]
	public class QuotedTermTests
	{
		[TestMethod]
		public void QuotedStringMatch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1);
			var collection = new List<Subject>
				{
					new Subject
						{
							Prop1 = "Prop1",
							Value = 5,
							Exclude = "Prop2"
						},
					new Subject
						{
							Prop1 = "Prop1 and Prop2",
							Value = 6,
							Exclude = "Prop2"
						},
					new Subject
						{
							Prop1 = "Prop3",
							Exclude = "Prop1",
							Deep = new Subject {Value = 6}
						},
					new Subject
						{
							Prop1 = "Prop4",
							Value = 8,
							Exclude = "Prop1"
						}
				};

			var results = filter.Apply(collection, "\"prop1 and\" prop4");
			Assert.AreEqual(2, results.Count());
			Assert.AreSame(collection[1], results.First().Item);
		}
	}
}
