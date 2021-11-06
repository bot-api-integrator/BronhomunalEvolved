using CLAP;

namespace Memator
{
	public static class Program
	{
		private static bool _help = false;
		private static Client _client;
		
		public static void Main(string[] args)
		{
			Logger.DEBUG = false;
			#if DEBUG
				Logger.DEBUG = true;
			#endif
			LoadConfig();

			ReadArguments(args);
			if (_help)
			{
				return;
			}

			_client
		}



		/// <summary>
		/// Загрузка и применение конфигурации мематора
		/// </summary>
		private static void LoadConfig()
		{
			try
			{
				Globals.CONFIG = Config.Load();
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				Globals.CONFIG = new Config();
			}

			SetConnectionType(Globals.CONFIG.ConnectionType);

			
		}



		/// <summary>
		/// Устанавливает тип соединения с внешним миром.
		/// builtin - встроенное подключение, используемое для тестирования функционала клиента.
		/// kafka - для связи с внешним миром используется уже работающая кафка.
		/// </summary>
		/// <param name="type">Предпочитаемый тип подключения</param>
		/// <exception cref="ArgumentException">Происходит в случае, если указан неизвестный тип подключения</exception>
		private static void SetConnectionType(string type)
		{
			if (type == "builtin")
			{
				Globals.Connection = new BronhomunalEvolved.Connections.Builtin.BuiltinConnection();
			}
			else if (type == "kafka")
			{
				Globals.Connection = new BronhomunalEvolved.Connections.Kafka.KafkaConnection();
			}
			else
			{
				throw new ArgumentException("Unknown connection type. Connection type must be 'builtin' or 'kafka'");
			}
			Globals.CONFIG.ConnectionType = type;
		}



		/// <summary>
		/// Чтение и обработка параметров запуска
		/// d - включение отладки
		/// </summary>
		/// <param name="args"></param>
		private static void ReadArguments(string[] args)
		{
			Parser.Run<Options>(args);
		}


		/// <summary>
		/// Класс для работы с параметрами запуска
		/// </summary>
		private class Options
		{
			[Help]
			public static void Help(string help)
			{
				Logger.Log(help);
				Program._help = true;
			}

			[Global(Aliases = "d", Description = "Show debug messages")]
			public static void Debug()
			{
				Logger.DEBUG = true;
			}

			[Verb(Aliases = "c", Description = "Specify connection type: 'builtin' (default) or 'kafka'")]
			public static void Connection(string type)
			{
				SetConnectionType(type);
			}
		}
	}
}