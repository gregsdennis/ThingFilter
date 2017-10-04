using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThingFilter.Tests
{
	[TestFixture]
	public class CaseSensitivityTests
	{
		[Test]
		public void CaseSensitive()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.CaseSensitive();
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "Prop1a"}
				};

			var filtered = filter.Apply(collection, "prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}
	}
}