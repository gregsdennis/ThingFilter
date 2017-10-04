using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThingFilter.Tests
{
	[TestClass]
	public class AndQueryTests
	{
		[TestMethod]
		public void AndQuery()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, weight: 2)
				.MatchOn(s => s.Exclude);
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1", Exclude = "prop2"},
					new Subject {Prop1 = "prop1", Exclude = "no"},
					new Subject {Prop1 = "no", Exclude = "prop2"},
					new Subject {Exclude = "and"},
				};

			var filtered = filter.Apply(collection, "prop1 AND prop2");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
			Assert.AreEqual(3, filtered.First().Score);
		}
		[TestMethod]
		public void NotAnAndQuery()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.MatchOn(s => s.Exclude);
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1", Exclude = "prop2"},
					new Subject {Exclude = "and"},
					new Subject {Prop1 = "no", Exclude = "prop2"},
				};

			var filtered = filter.Apply(collection, "prop1 and prop2");
			Assert.AreEqual(3, filtered.Count());
		}
	}
}