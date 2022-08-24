using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vk_api_balancer
{
	public class Config
	{
		// СТАТИЧЕСКИЕ ПОЛЯ
		private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
		private static readonly String _defaultConfigPath = AppDomain.CurrentDomain.FriendlyName + ".json";


		// НЕСЕРИАЛИЗУЕМОЕ ПОЛЕ
		// Хранит путь к данному файлу конфигурации
		private String configPath = _defaultConfigPath;



		// СЕРИАЛИЗУЕМЫЕ СВОЙСТВА
		/// Строка подключения к серверу
		public String KafkaServer { get; set; } = "localhost:9092";

		/// Топик, из которого читаются данные
		public String SubscribeTopic { get; set; } = "VkSendMessage";

		/// Топик, в который пишутся данные
		public String ProduceTopic { get; set; } = "VkRecieveMessage";

		/// Токен Vk API. По умолчанию нюхай бебру
		public String VkApiToken { get; set; } = "1e3e8d31e3c8094f00bddac3431fa1dde34ef4845b3807ad620ff4cd5584c85808da338eba2f535313ddd";

		/// ID группы (бота) ВК. По умолчанию - бронхомунал
		public UInt64 GroupId { get; set; } = 204006161;

		/// ID пользователя для загрузки файлов на сервер ВК. По умолчанию - бронух
		public UInt64 PeerId { get; set; } = 88798690;

		/// <summary>
		/// Пытается получить значние указанной переменной среды. В случае неудачи вернёт NULL.
		/// </summary>
		/// <param name="name">Имя переменной</param>
		/// <returns></returns>
		private static String? TryGetEnvString(String name)
		{
			String? env = Environment.GetEnvironmentVariable(name);
			if (env is not null && env.Trim().Equals(String.Empty))
			{
				env = null;
			}

			return env;
		}
		private static Boolean? TryGetEnvBool(String name)
		{
			Boolean? env = null;
			try
			{
				env = Boolean.Parse(TryGetEnvString(name));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			return env;
		}

		private static UInt64? TryGetEnvInt(String name)
		{
			UInt64? env = null;
			try
			{
				env = UInt64.Parse(TryGetEnvString(name));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			return env;
		}

		public Config() { }

		/// <summary>
		/// Загружает конфигурацию из файла по указанному пути
		/// </summary>
		/// <param name="path">Путь до файла конфигурации</param>
		/// <returns></returns>
		[Obsolete(message: "Приложение конфигурируется с помощью переменных среды.")]
		public static Config Load(String path)
		{
			Config? config;
			try
			{
				config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path), _serializerOptions);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				config = new Config();
			}

			config ??= new Config();
			config.configPath = path;
			config.Save();

			return config;
		}

		/// <summary>
		/// Сохраняет конфигурацию в Env
		/// </summary>
		public void Save()
		{
			Environment.SetEnvironmentVariable("SUBSCRIBE_TOPIC", SubscribeTopic);
			Environment.SetEnvironmentVariable("KAFKA_SERVER", KafkaServer);
			Environment.SetEnvironmentVariable("PRODUCE_TOPIC", ProduceTopic);
			Environment.SetEnvironmentVariable("VK_API_TOKEN", VkApiToken);
			Environment.SetEnvironmentVariable("GROUP_ID", GroupId.ToString());
			Environment.SetEnvironmentVariable("PEER_ID", PeerId.ToString());

		}

		/// <summary>
		/// Сохраняет конфигурацию в файл по умолчанию
		/// </summary>
		[Obsolete(message: "Приложение конфигурируется с помощью переменных среды.")]
		public void SaveToFile()
		{
			SaveToFile(configPath);
		}

		/// <summary>
		/// Сохраняет конфигурацию в указанный файл
		/// </summary>
		/// <param name="path">Путь к файлу конфигурации</param>
		[Obsolete(message: "Приложение конфигурируется с помощью переменных среды.")]
		public void SaveToFile(String path)
		{
			try
			{
				File.WriteAllText(path, JsonSerializer.Serialize(this, _serializerOptions));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		/// <summary>
		/// Попытается загрузить конфигурацию из переменных среды
		/// </summary>
		/// <returns></returns>
		public static Config Load()
		{
			var config = new Config();

			config.SubscribeTopic = TryGetEnvString("SUBSCRIBE_TOPIC") ?? config.SubscribeTopic;
			config.KafkaServer = TryGetEnvString("KAFKA_SERVER") ?? config.KafkaServer;
			config.ProduceTopic = TryGetEnvString("PRODUCE_TOPIC") ?? config.ProduceTopic;
			config.VkApiToken = TryGetEnvString("VK_API_TOKEN") ?? config.VkApiToken;
			config.GroupId = TryGetEnvInt("GROUP_ID") ?? config.GroupId;
			config.PeerId = TryGetEnvInt("PEER_ID") ?? config.PeerId;

			return config;
		}
	}
}
