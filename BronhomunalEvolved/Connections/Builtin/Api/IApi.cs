using BronhomunalEvolved.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Connections.Builtin.Api
{
	internal interface IApi
	{

		public delegate void MessagesHandler(RecieveMessage message);
		event MessagesHandler GetMessage;
	}
}
