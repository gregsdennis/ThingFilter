using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTests
{
	class Subject
	{
		public string Prop1 { get; set; }
		public int Value { get; set; }
		public string Exclude { get; set; }
		public Subject Deep { get; set; }
		public bool Bool { get; set; }
	}
}
