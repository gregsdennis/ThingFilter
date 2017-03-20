using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThingFilter;

namespace FilterTests
{
	[TestClass]
	public class CaseSensitivityTests
	{
		[TestMethod]
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
			Assert.AreSame(collection[0], filtered.First());
		}
	}
}