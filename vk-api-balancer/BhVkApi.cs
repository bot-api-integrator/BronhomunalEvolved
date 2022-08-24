using BronhomunalEvolved;
using BronhomunalEvolved.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace vk_api_balancer
{
	internal class BhVkApi
	{
		private static Config _config;
		private static UInt64 _groupId;
		private static UInt64 _peerId;
		private static Client _client;

		public static VkApi API;
		public static List<String> Files = new List<string>();
		public static event EventHandler<BronhomunalEvolved.Types.Message> MessageReceived = (s,m) => { };
		public static void StartVkApi(Config config, Client client)
		{
			_config = config;

			_groupId = config.GroupId;

			_client = client;

			Console.WriteLine("Авторизация вк с токеном: " + config.VkApiToken);
			API = new VkApi();

			API.Authorize(new ApiAuthParams()
			{
				AccessToken = config.VkApiToken
			});

			new Thread(() => {
				Upd();
			}).Start();
			//Console.ReadLine();
		}

		static void Upd()
		{
			while (true) // Бесконечный цикл, получение обновлений
			{
				try
				{
					var s = API.Groups.GetLongPollServer(_groupId);
					var poll = API.Groups.GetBotsLongPollHistory(
					   new BotsLongPollHistoryParams()
					   { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });
					if (poll?.Updates == null) continue; // Проверка на новые события


					foreach (var a in poll.Updates)
					{
						if (a.Type == GroupUpdateType.MessageNew)
						{
							string userMessage = a.MessageNew.Message.Text ?? "NULL";//a.Message.Body?.ToLower() ?? "NULL";
							long? userID = a.MessageNew.Message.UserId;
							Console.WriteLine("Всосал сообщеньку: " + userMessage);
							if (userMessage.StartsWith("!"))
							{
								var msg = new RecieveMessage()
								{
									Text = userMessage,
									AuthorId = a.MessageNew.Message.FromId.ToString(),
									SendTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
									IntegratorTopic = _config.SubscribeTopic,
									IntegratorDestinationData = a.MessageNew.Message.PeerId.ToString()
								};
								
								MessageReceived(a,new BronhomunalEvolved.Types.Message(msg).SetClient(_client));
								
								//HandleCommand(userMessage, a.MessageNew.Message.PeerId - 2000000000);
							}
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

			}
		}

		public static void SendMeme(string path, long? chatId)
		{
			Console.WriteLine("Sending meme " + path);
			Random rnd = new Random();
			API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { Photo(path) }
			});
		}


		public static void SendMessage()
		{

		}


		public static void SendDoc(string path, long? chatId)
		{
			Console.WriteLine("Sending doc " + path);
			Random rnd = new Random();
			API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Attachments = new List<MediaAttachment> { Document(path) }
			});
		}

		public static long SendMessage(string message, long? chatId)
		{
			Random rnd = new Random();
			return API.Messages.Send(new MessagesSendParams()
			{
				RandomId = rnd.Next(),
				ChatId = chatId,
				Message = message
			});
		}


		public static MediaAttachment PhotoFromUrl(string url)
		{
			Console.WriteLine("Getting photo from " + url);
			var wc = new WebClient();
			wc.DownloadFile(url, "temp.png");
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "temp.png"));
			var photo = API.Photo.SaveMessagesPhoto(result);//.SaveMessagesPhoto(url);
			wc.Dispose();

			return photo.FirstOrDefault();
		}

		public static MediaAttachment PhotoFromUrlJpg(string url)
		{
			Console.WriteLine("Getting photo from " + url);
			var wc = new WebClient();
			wc.DownloadFile(url, "temp.jpg");
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "temp.jpg"));
			var photo = API.Photo.SaveMessagesPhoto(result);//.SaveMessagesPhoto(url);
			wc.Dispose();
			return photo.FirstOrDefault();
		}

		public static MediaAttachment Photo(string path)
		{
			Console.WriteLine("Getting photo from " + path);
			var uploadServer = API.Photo.GetMessagesUploadServer(88798690);
			var wc = new WebClient();
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, path));
			var photo = API.Photo.SaveMessagesPhoto(result);
			wc.Dispose();
			return photo.FirstOrDefault();
		}

		public static MediaAttachment Document(string path)
		{
			Console.WriteLine("Getting document from " + path);
			var uploadServer = API.Docs.GetMessagesUploadServer(88798690);
			var wc = new WebClient();
			var result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, path));
			var doc = API.Docs.Save(result, Path.GetFileNameWithoutExtension(path), tags: null);
			wc.Dispose();
			List<VkNet.Model.Attachments.MediaAttachment> _attachments = new List<VkNet.Model.Attachments.MediaAttachment>();
			foreach (var a in doc)
				_attachments.Add(a.Instance);
			return _attachments.FirstOrDefault();
		}
	}
}
