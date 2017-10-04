using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThingFilter.Tests
{
	[TestClass]
	public class UnmatchedTests
	{
		[TestMethod]
		public void Unmatched()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.IncludeUnmatched();
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "don't match"}
				};

			var filtered = filter.Apply(collection, "prop1");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreEqual(0, filtered.Last().Score);
		}
	}
}
