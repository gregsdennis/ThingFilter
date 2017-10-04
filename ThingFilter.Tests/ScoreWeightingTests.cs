using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThingFilter.Tests
{
	[TestClass]
	public class ScoreWeightingTests
	{
		[TestMethod]
		public void WeightingApplies()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, weight: 2)
				.MatchOn(s => s.Exclude);
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Exclude = "prop1"}
				};

			var filtered = filter.Apply(collection, "prop");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreSame(collection[0], filtered.OrderByDescending(s => s.Score).First().Item);
			Assert.AreEqual(2, filtered.OrderByDescending(s => s.Score).First().Score);
		}
	}
}
