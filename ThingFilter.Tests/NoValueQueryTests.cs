using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThingFilter.Tests
{
	[TestFixture]
	public class NoValueQueryTests
	{
		[Test]
		public void BlankQuery()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
			{
				new Subject {Prop1 = "value"},
				new Subject {Prop1 = null}
			};

			var filtered = filter.Apply(collection, "prop=");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreEqual(1, filtered.Last().Score);
		}
		[Test]
		public void NullQuery()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
			{
				new Subject {Prop1 = "value"},
				new Subject {Prop1 = null}
			};

			var filtered = filter.Apply(collection, "prop=null");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreEqual(1, filtered.Last().Score);
		}
		[Test]
		public void QuotedNullQuery()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
			{
				new Subject {Prop1 = "value"},
				new Subject {Prop1 = null},
				new Subject {Prop1 = null},
				new Subject {Prop1 = "null"},
			};

			var filtered = filter.Apply(collection, "prop=\"null\"");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreEqual(1, filtered.Last().Score);
		}
	}
}