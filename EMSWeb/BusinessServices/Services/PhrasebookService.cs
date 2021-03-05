
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class PhrasebookService : IPhrasebookService
	{
        private string _connectionString ;

        public PhrasebookService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<PhrasebookList> GetPhrasebook()
		{
            var list = new PhrasebookList() { 
                Id = 0,
                ParentId = 0,
                Name = "Root"
            };
            await GetPhrasesForList(list);
            await GetChildren(list);
            return list;
        }

        public async Task Delete(DeletePhrasebookDto[] toDelete) 
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString)) 
            {
                await con.OpenAsync();
                var phrasesIds = toDelete.Where(x => !x.IsList).Select(x => x.Id).ToList();
                if(phrasesIds.Any()) 
                {
                    var phrasesIdsString = phrasesIds.Select(x => x.ToString()).Aggregate((value, acc) => acc + ", " + value);
                    phrasesIdsString = phrasesIdsString.Trim();
                    if (phrasesIdsString[^1] == ',')
                    {
                        phrasesIdsString = phrasesIdsString.Substring(0, phrasesIdsString.Length - 1);
                    }
                    var commandText = $"UPDATE phrases SET deleted = 1 WHERE id IN({phrasesIdsString})";
                    using (var cmd = new MySqlCommand(commandText, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                var listsIds = toDelete.Where(x => x.IsList).Select(x => x.Id).ToList();
                if(listsIds.Any()) 
                {
                    var listsIdsString = listsIds.Select(x => x.ToString()).Aggregate((acc, value) => acc + ", " + value);
                    listsIdsString = listsIdsString.Trim();
                    if (listsIdsString[^1] == ',')
                    {
                        listsIdsString = listsIdsString.Substring(0, listsIdsString.Length - 1);
                    }
                    var commandText = $"UPDATE phrase_lists SET deleted = 1 WHERE id IN({listsIdsString})";
                    using (var cmd = new MySqlCommand(commandText, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await con.CloseAsync();
            }
        }

        public async Task<int> CreatePhrase(CreatePhraseDto createPhraseDto) 
        {
            int id = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var commandText =
                    $"INSERT INTO phrases(phrase_list_id, content, sort_order, deleted, created_by) " +
                    $"VALUES({createPhraseDto.ListId}, '{createPhraseDto.Text}', 1, 0, 1)";
                using (var cmd = new MySqlCommand(commandText, con))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS ID;", con))
                {
                    var d = await cmd.ExecuteReaderAsync();
                    if (d.HasRows &&  d.Read())
                    {
                        id = Convert.ToInt32((ulong)d["ID"]);
                    }
                }
                await con.CloseAsync();
            }
            return id;
        }
        public async Task<int> CreateList(CreateListDto createListDto)
        {
            int id = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var commandText =
                    $"INSERT INTO phrase_lists(created_by, name, sort_order, org_name, deleted, parent_id)" +
                    $"VALUES(1, '{createListDto.Name}', 1, 'DEFAULT', 0, {createListDto.ParentId})";
                using (var cmd = new MySqlCommand(commandText, con))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS ID;", con))
                {
                    var d = await cmd.ExecuteReaderAsync();
                    if (d.HasRows && d.Read())
                    {
                        id = Convert.ToInt32((ulong)d["ID"]);
                    }
                }
                await con.CloseAsync();
            }
            return id;
        }


        public async Task Modify(ModifyNodeDto modifyNodeDto)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var commandText = modifyNodeDto.IsList ?
                    $"UPDATE phrase_lists SET name = '{modifyNodeDto.Name}' WHERE id = '{modifyNodeDto.Id}'" :
                    $"UPDATE phrases SET content = '{modifyNodeDto.Name}' WHERE id = '{modifyNodeDto.Id}'";
                using (var cmd = new MySqlCommand(commandText, con))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                await con.CloseAsync();
            }
        }

        private async Task GetChildren(PhrasebookList parent) 
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(
                    $"SELECT id, name FROM phrase_lists WHERE parent_id = {parent.Id} AND deleted = 0 ORDER BY name, sort_order"
                ))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                await con.OpenAsync();

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
                        await GetChildren(child);
                        await GetPhrasesForList(child);
                        parent.ChildLists.Add(child);
                    }
                }
                await con.CloseAsync();
                parent.ChildLists.OrderBy(x => x.ChildLists.Count > 0 || x.Phrases.Count > 0);
            }
        }

        private async Task GetPhrasesForList(PhrasebookList parent) 
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(
                $"SELECT id, phrase_list_id, content FROM phrases WHERE deleted = 0 AND phrase_list_id = {parent.Id} ORDER BY content, sort_order"
            ))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                await con.OpenAsync();
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
                await con.CloseAsync();
            }
        }

    }
}
