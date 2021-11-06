using BronhomunalEvolved.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Connections
{
	public interface IConnection
	{
		public delegate void MessagesHandler(Message message);
		event MessagesHandler GetMessage;

		public void Start();

		public void SendMessage(Message message);
	}
}
