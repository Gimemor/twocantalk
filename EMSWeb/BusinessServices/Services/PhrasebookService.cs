
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class PhrasebookService : IPhrasebookService
	{
		public async Task<PhrasebookList> GetPhrasebook()
		{
            var list = new PhrasebookList() { 
                Id = 0,
                ParentId = 0,
                Name = "Root"
            };
            try
            {
                using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=emsdb; UID=root; PWD=Mik70525"))
                {
                    await GetPhrasesForList(con, list);
                    await GetChildren(con, list);
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        private async Task GetChildren(MySqlConnection con, PhrasebookList parent) {
            using (MySqlCommand cmd = new MySqlCommand(
                    $"SELECT id, name FROM phrase_lists WHERE parent_id = {parent.Id} AND deleted = 0 ORDER BY sort_order;"
                ))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                var d = await cmd.ExecuteReaderAsync();
                if (d.HasRows)
                {
                    while (d.Read())
                    {
                        var child = new PhrasebookList
                        {
                            Id = (uint)d["id"],
                            Name  = d["name"].ToString(),
                        };
                        await GetChildren(con, child);
                        await GetPhrasesForList(con, child);
                        parent.ChildLists.Add(child);
                    }
                }
            }
        }

        private async Task GetPhrasesForList(MySqlConnection con, PhrasebookList parent) 
        {
            using (MySqlCommand cmd = new MySqlCommand(
                $"SELECT id, phrase_list_id, content FROM phrases WHERE deleted = 0 AND phrase_list_id = {parent.ParentId} ORDER BY sort_order"
            ))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                var d = await cmd.ExecuteReaderAsync();
                if (d.HasRows)
                {
                    while (d.Read())
                    {
                        parent.Phrases.Add(
                            new PhrasebookPhrase { 
                                Id = (uint)d["id"],
                                Content = d["content"].ToString(),
                                ListId = parent.ParentId,
                            });
                    }
                }
            }
        }
        
	}
}
