using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThingFilter;

namespace FilterTests
{
	[TestClass]
	public class UsageExample
	{
		[TestMethod]
		public void DoIt()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.MatchOn(s => s.Value)
				.MatchOn(s => s.Deep)
				.MatchOn(s => s.Deep?.Value);
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
							Deep = new Subject {Value = 6}
						},
					new Subject
						{
							Prop1 = "Prop4",
							Value = 8,
							Exclude = "Prop1"
						}
				};

			var filtered = filter.Apply(collection, "prop1 6");
			Assert.AreEqual(3, filtered.Count());
			collection.Add(new Subject { Prop1 = "Prop5", Value = 6 });
			Assert.AreEqual(4, filtered.Count());

			var sorted = filter.Apply(collection, "prop1 6").OrderByDescending(r => r.Score);

			Assert.AreSame(collection[1], sorted.First().Item);

			var typeFilter = filter.Apply(collection, "ubj");

			Assert.AreEqual(1, typeFilter.Count());
		}

		[TestMethod]
		public void DoItWithExtensionMethod()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.MatchOn(s => s.Value)
				.MatchOn(s => s.Deep)
				.MatchOn(s => s.Deep?.Value);
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
							Deep = new Subject {Value = 6}
						},
					new Subject
						{
							Prop1 = "Prop4",
							Value = 8,
							Exclude = "Prop1"
						}
				};

			var filtered = collection.Filter(filter, "prop1 6");
			Assert.AreEqual(3, filtered.Count());
			collection.Add(new Subject { Prop1 = "Prop5", Value = 6 });
			Assert.AreEqual(4, filtered.Count());

			var sorted = collection.Filter(filter, "prop1 6").OrderByDescending(r => r.Score);

			Assert.AreSame(collection[1], sorted.First().Item);

			var typeFilter = collection.Filter(filter, "ubj");

			Assert.AreEqual(1, typeFilter.Count());
		}
	}
}
