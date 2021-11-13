namespace Memator
{
	public static class Program
	{
		private static bool _help = false;
		private static Config _config = null;
		private static Client _client;
		
		public static void Main(string[] args)
		{
			Logger.DEBUG = false;
			#if DEBUG
				Logger.DEBUG = true;
			#endif
			LoadConfiguration();
		}



		/// <summary>
		/// Загрузка и применение конфигурации мематора
		/// </summary>
		private static void LoadConfiguration()
		{
				_config = Config.Load();
		}
	}
}