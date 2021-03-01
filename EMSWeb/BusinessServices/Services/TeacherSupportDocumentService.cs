using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class TeacherSupportDocumentService : ITeacherSupportDocumentService
	{
        private string _connectionString;

        public TeacherSupportDocumentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<TeachersSupportDocuments>> GetList()
		{
            var list = new List<TeachersSupportDocuments>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT Id, filename AS name FROM files_teachers_support_documents WHERE deleted = 0 Order By filename;"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                await con.OpenAsync();
                var d = await cmd.ExecuteReaderAsync();
                if (d.HasRows)
                {
                    while (await d.ReadAsync())
                    {
                        list.Add(new TeachersSupportDocuments { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                    }
                }
                await con.CloseAsync();
            }
            return list;
        }
    }
}
