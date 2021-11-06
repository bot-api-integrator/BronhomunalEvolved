using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved
{
	public static class Globals
	{
		public static Config? CONFIG { get; set; }
		public static IConnection? Connection { get; set; }

		public const string ConfigPath = "config.cfg";

	}
}
