using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThingFilter.Tests
{
	[TestFixture]
	public class MatchAllTests
	{
		[Test]
		public void StringSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOnAll();
			var collection = new List<Subject>
			{
				new Subject {Prop1 = "prop1"},
				new Subject {Prop1 = "don't match"}
			};

			var filtered = filter.Apply(collection, "prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreEqual(1, filtered.Last().Score);
		}

		[Test]
		public void NumericSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOnAll();
			var collection = new List<Subject>
			{
				new Subject
				{
					Value = 5,
				},
				new Subject
				{
					Value = 6,
				},
				new Subject
				{
					Value = 8,
				}
			};

			var filtered = filter.Apply(collection, "6.0");
			Assert.AreEqual(1, filtered.Count());
		}

		[Test]
		public void BooleanSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOnAll();
			var collection = new List<Subject>
			{
				new Subject {Bool = false},
				new Subject {Bool = true},
				new Subject {Bool = true},
				new Subject {Bool = false}
			};

			var filtered = filter.Apply(collection, "true");
			Assert.AreEqual(2, filtered.Count());
		}
	}
}