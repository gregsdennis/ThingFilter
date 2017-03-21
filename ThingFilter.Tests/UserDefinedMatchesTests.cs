using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThingFilter;

namespace FilterTests
{
	[TestClass]
	public class UserDefinedMatchesTests
	{
		private class StartsWithMatchEvaluator : IMatchEvaluator
		{
			public string Operation => "%";

			public bool Match(string query, string target, bool caseSensitive)
			{
				return target.StartsWith(query, !caseSensitive, CultureInfo.CurrentCulture);
			}
			public bool Match(double query, double target)
			{
				return false;
			}
			public bool Match(bool query, bool target)
			{
				return false;
			}
		}

		[TestMethod]
		public void CanAddCustomMatchEvaluator()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.AddEvaluator(new StartsWithMatchEvaluator());
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "secondProp"},
					new Subject {Prop1 = "something else"},
				};

			var filtered = filter.Apply(collection, "%prop");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[TestMethod]
		public void CanRemoveMatchEvaluator()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1)
				.RemoveEvaluator("=");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "Propane"},
					new Subject {Prop1 = "Gas"},
				};

			var filtered = filter.Apply(collection, "=prop1");
			Assert.AreEqual(0, filtered.Count());
		}
	}
}
