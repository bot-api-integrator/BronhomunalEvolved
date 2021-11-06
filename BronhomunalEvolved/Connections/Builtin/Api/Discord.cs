using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Connections.Builtin.Api
{
	internal class Discord : IApi
	{
		public event IApi.MessagesHandler GetMessage;
	}
}
