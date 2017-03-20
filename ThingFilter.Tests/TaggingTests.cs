using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThingFilter;

namespace FilterTests
{
	[TestClass]
	public class TaggingTests
	{

		[TestMethod]
		public void TaggedSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.MatchOn(s => s.Deep?.Prop1, "nest");
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
							Prop1 = "Prop1a",
							Value = 6,
							Exclude = "Prop2"
						},
					new Subject
						{
							Prop1 = "Prop3",
							Exclude = "Prop1",
							Deep = new Subject {Prop1 = "test"}
						},
					new Subject
						{
							Prop1 = "test",
							Value = 8,
							Exclude = "Prop1"
						}
				};

			var results = filter.Apply(collection, "prop1 nest:te");
			Assert.AreEqual(3, results.Count());
			Assert.IsTrue(results.Contains(collection[2]));
			Assert.IsFalse(results.Contains(collection[3]));
		}

		[TestMethod]
		public void RequiredTag()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop", true)
				.MatchOn(s => s.Exclude);
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Exclude = "Prop1a"}
				};

			var filtered = filter.Apply(collection, "prop:prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First());
		}
	}
}
