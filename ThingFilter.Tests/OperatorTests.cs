using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThingFilter.Tests
{
	[TestFixture]
	public class OperatorTests
	{
		[Test]
		public void EqualToString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "prop1a"}
				};

			var filtered = filter.Apply(collection, "prop=prop1");
			Assert.AreEqual(1, filtered.Count());
		}

		[Test]
		public void EqualToNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 6},
				};

			var filtered = filter.Apply(collection, "prop=5");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void EqualToBooleanTrue()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Bool, "prop");
			var collection = new List<Subject>
				{
					new Subject {Bool = true},
					new Subject {Bool = false},
				};

			var filtered = filter.Apply(collection, "prop=true");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void EqualToBooleanFalse()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Bool, "prop");
			var collection = new List<Subject>
				{
					new Subject {Bool = true},
					new Subject {Bool = false},
				};

			var filtered = filter.Apply(collection, "prop=false");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void EqualToUntagged()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop")
				.MatchOn(s => s.Exclude);
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1", Exclude = "temp"},
					new Subject {Prop1 = "prop1a", Exclude = "temp"},
					new Subject {Prop1 = "temp", Exclude = "prop1a"},
					new Subject {Prop1 = "temp", Exclude = "prop1"}
				};

			var filtered = filter.Apply(collection, "=prop1");
			Assert.AreEqual(2, filtered.Count());
		}

		[Test]
		public void NotEqualToString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "prop1a"}
				};

			var filtered = filter.Apply(collection, "prop<>prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void NotEqualToNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 6},
				};

			var filtered = filter.Apply(collection, "prop<>6");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void NotEqualToBooleanTrue()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Bool, "prop");
			var collection = new List<Subject>
				{
					new Subject {Bool = true},
					new Subject {Bool = false},
				};

			var filtered = filter.Apply(collection, "prop<>true");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void NotEqualToBooleanFalse()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Bool, "prop");
			var collection = new List<Subject>
				{
					new Subject {Bool = true},
					new Subject {Bool = false},
				};

			var filtered = filter.Apply(collection, "prop<>false");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void LessThanString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "abc"},
					new Subject {Prop1 = "xyz"}
				};

			var filtered = filter.Apply(collection, "prop<prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void LessThanNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 15}
				};

			var filtered = filter.Apply(collection, "prop<10");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void LessThanEqualString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "abc"},
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "xyz"}
				};

			var filtered = filter.Apply(collection, "prop<=prop1");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void LessThanEqualNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 10},
					new Subject {Value = 15}
				};

			var filtered = filter.Apply(collection, "prop<=10");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreSame(collection[0], filtered.First().Item);
		}

		[Test]
		public void GreaterThanString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "abc"},
					new Subject {Prop1 = "xyz"}
				};

			var filtered = filter.Apply(collection, "prop>prop1");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void GreaterThanNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 15}
				};

			var filtered = filter.Apply(collection, "prop>10");
			Assert.AreEqual(1, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void GreaterThanEqualString()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Prop1, "prop");
			var collection = new List<Subject>
				{
					new Subject {Prop1 = "abc"},
					new Subject {Prop1 = "prop1"},
					new Subject {Prop1 = "xyz"}
				};

			var filtered = filter.Apply(collection, "prop>=prop1");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}

		[Test]
		public void GreaterThanEqualNumber()
		{
			var filter = new ThingFilter<Subject>()
				.MatchOn(s => s.Value, "prop");
			var collection = new List<Subject>
				{
					new Subject {Value = 5},
					new Subject {Value = 10},
					new Subject {Value = 15}
				};

			var filtered = filter.Apply(collection, "prop>=10");
			Assert.AreEqual(2, filtered.Count());
			Assert.AreSame(collection[1], filtered.First().Item);
		}
	}
}
