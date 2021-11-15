using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BronhomunalEvolved
{
	public class Config
	{
		// СТАТИЧЕСКИЕ ПОЛЯ
		private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
		private static readonly String _defaultConfigPath = AppDomain.CurrentDomain.FriendlyName+".json";


		// НЕСЕРИАЛИЗУЕМОЕ ПОЛЕ
		// Хранит путь к данному файлу конфигурации
		private String configPath = _defaultConfigPath;



		// СЕРИАЛИЗУЕМЫЕ СВОЙСТВА
		// Строка подключения к серверу
		public String KafkaServer { get; set; } = "localhost:9092";

		// Топик, с которым работает данный модуль
		public String SubscribeTopic { get; set; } = "MematorModule";

		public String AddListenerTopic { get; set; } = "AddListener";

		// Разрешить применение масштабирования с учетом содержимого (жмых-эффект)
		public Boolean AllowCAS { get; set; } = true;

		//Разрешить переворачивать картинку
		public Boolean AllowFlip { get; set; } = true;


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
				config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path),_serializerOptions);
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
			Environment.SetEnvironmentVariable("SUBSCRIBE_TOPIC",SubscribeTopic);
			Environment.SetEnvironmentVariable("KAFKA_SERVER",KafkaServer);

			Environment.SetEnvironmentVariable("ALLOW_CAS", AllowCAS.ToString());
			Environment.SetEnvironmentVariable("ALLOW_FLIP",AllowFlip.ToString());

		}

		/// <summary>
		/// Сохраняет конфигурацию в файл по умолчанию
		/// </summary>
		[Obsolete(message:"Приложение конфигурируется с помощью переменных среды.")]
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

			config.AllowCAS = TryGetEnvBool("ALLOW_CAS") ?? config.AllowCAS;
			config.AllowFlip = TryGetEnvBool("ALLOW_FLIP") ?? config.AllowFlip;

			return config;
		}
	}
}
