using BronhomunalEvolved.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Connections.Kafka
{
	public class KafkaConnection : IConnection
	{
		public event IConnection.MessagesHandler GetMessage;

		public void SendMessage(Message message)
		{
			throw new NotImplementedException();
		}

		public void Start()
		{
			throw new NotImplementedException();
		}
	}
}
