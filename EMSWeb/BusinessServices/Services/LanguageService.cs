
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class LanguageService : ILanguageService
	{
		protected string _connectionString;
		public LanguageService(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task<List<Language>> GetList()
		{
			var languages = new List<Language>();
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand("SELECT id, name FROM languages WHERE active = 1 ORDER BY name"))
			{
				await connection.OpenAsync();
				try
				{
					command.Connection = connection;
					var d = await command.ExecuteReaderAsync();
					if (!d.HasRows)
					{
						return null;
					}
					while (await d.ReadAsync())
					{
						languages.Add(new Language
						{
							Id = d["id"].ToString(),
							Name = d["name"].ToString()
						});
					}
				}
				finally {
					await connection.CloseAsync();
				}
				return languages;
			}
		}
	}
}
