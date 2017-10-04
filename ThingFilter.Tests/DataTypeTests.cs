using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThingFilter.Tests
{
	[TestFixture]
	public class DataTypeTests
	{

		[Test]
		public void NumericSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value)
				.MatchOn(s => s.Deep?.Value);
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
							Deep = new Subject {Value = 6}
						},
					new Subject
						{
							Value = 8,
						}
				};

			var filtered = filter.Apply(collection, "6.0");
			Assert.AreEqual(2, filtered.Count());
		}

		[Test]
		public void BooleanSearch()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Bool);
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
