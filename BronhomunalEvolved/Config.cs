using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved
{
	public class Config
	{
		public string VkToken { get; set; } = String.Empty;
		public string DiscordToken { get; set; } = String.Empty;
		public string ConnectionType { get; set; } = "builtin";

		public Config() { }

		public void Save()
		{
			var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
			{
				WriteIndented = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
			try
			{
				File.WriteAllText(Globals.ConfigPath, json);
			}
			catch(Exception ex)
			{
				Logger.Error(ex);
			}
		}

		public static Config? Load()
		{
			Config? config = null;
			try
			{
				config = JsonSerializer.Deserialize<Config>(Globals.ConfigPath);
			}catch(Exception ex) 
			{ 
				Logger.Error(ex);
				config = new Config();
			}
			if (config.ConnectionType.Equals("builtin"))
			{
				CheckVkToken(config);
				CheckDiscordToken(config);
			}
			config.Save();
			return config;
		}

		private static void CheckDiscordToken(Config config)
		{
			if (config.VkToken.Equals(String.Empty))
			{
				string discordToken;
				Logger.Log("Enter Discord token to start:");
				discordToken = Console.ReadLine();
				while (discordToken == null || discordToken.Equals(String.Empty))
				{
					Logger.Log("Invalid Discord token. Try again:");
					discordToken = Console.ReadLine();
				}
				config.VkToken = discordToken;
			}
		}

		private static void CheckVkToken(Config config)
		{
			if (config.VkToken.Equals(String.Empty))
			{
				string vkToken;
				Logger.Log("Enter VK token to start:");
				vkToken = Console.ReadLine();
				while (vkToken == null || vkToken.Equals(String.Empty))
				{
					Logger.Log("Invalid VK token. Try again:");
					vkToken = Console.ReadLine();
				}
				config.VkToken = vkToken;
			}
		}
	}
}
