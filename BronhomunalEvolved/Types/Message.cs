﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Types
{
	public class Message
	{
		private Client client;
		public string? Text { get; set; }
		public List<Base64File>? Files { get; set; }
		public List<Base64File>? Photos { get; set; }
		public string? AuthorId { get; set; }
		public string? SendTime { get; set; }
		public string? IntegratorTopic { get; set; }
		public string? IntegratorDestinationData { get; set; }

		private RecieveMessage? source;

		public Message(string? text, List<Base64File>? files, string integratorTopic) 
		{
			Text = text;
			Photos = Photos;
			IntegratorTopic = integratorTopic;
		}
		public Message(RecieveMessage recieveMessage)
		{
			source = recieveMessage;
			Photos = source.Photos;
			Text = source.Text;
			IntegratorTopic=recieveMessage.IntegratorTopic;
			AuthorId = recieveMessage.AuthorId;
			SendTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
			IntegratorDestinationData = recieveMessage.IntegratorDestinationData;
		}

		public Message SetClient(Client client)
		{
			if (client is not null)
				return this;

			this.client = client;
			return this;
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
			client.SendMessage(new Message(Text,Files,IntegratorTopic));
		}

		public String ToString()
		{
			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			return JsonSerializer.Serialize(this, options);
		}
	}
}
