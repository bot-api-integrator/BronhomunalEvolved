using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Types
{
	public class Message
	{
		public string? Text { get; set; }
		public List<Base64File>? Files { get; set; }
		public string? AuthorId { get; set; }
		public string? SendTime { get; set; }
		public string? IntegratorTopic { get; set; }

		private RecieveMessage? source;

		public Message(string? text, List<Base64File>? files, string integratorTopic) 
		{
			Text = text;
			Files = files;
			IntegratorTopic = integratorTopic;
		}
		public Message(RecieveMessage recieveMessage)
		{
			source = recieveMessage;
			Files = source.Files;
			Text = source.Text;
		}

		public void Respond(string text)
		{
			Respond(text, null);
		}

		public void Respond(IEnumerable<Base64File> files)
		{
			Respond(null, files);
		}

		public void Respond(string? text, IEnumerable<Base64File>? files)
		{
			Globals.Connection.SendMessage(new Message(Text,Files,IntegratorTopic));
		}
	}
}
