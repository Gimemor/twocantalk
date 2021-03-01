using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class SubjectService : ISubjectService
	{
		private string _connectionString { get; set; }
		public SubjectService(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public async Task<List<Subject>> GetList()
		{
			var subjects = new List<Subject>();
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand("SELECT id, name FROM subjects"))
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
						subjects.Add(new Subject
						{
							Id = d["id"].ToString(),
							Name = d["name"].ToString()
						});
					}
				}
				finally
				{
					await connection.CloseAsync();
				}
				return subjects;
			}
		}
	}
}
