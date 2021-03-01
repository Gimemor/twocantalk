using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class KnowledgeSharedService : IKnowledgeService
	{
        private string _connectionString;

        public KnowledgeSharedService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<List<KnowledgeSharebyCountry>> GetList()
		{
            var list = new List<KnowledgeSharebyCountry>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT id,Name FROM countries Order By name"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                await con.OpenAsync();
                var d = await cmd.ExecuteReaderAsync();
                if (d.HasRows)
                {
                    while (await d.ReadAsync())
                    {
                        list.Add(new KnowledgeSharebyCountry { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                    }
                }
                await con.CloseAsync();
            }
            return list;
        }
	}
}
