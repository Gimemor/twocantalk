using EMSWeb.BusinessServices.Helpers;
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
	public class ResourceLibService : IResourceLibService
	{
		private string _connectionString;
		private IWebHostEnvironment _hostingEnvironment;
		public ResourceLibService(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_hostingEnvironment = hostingEnvironment;
		}

		public ResourceListEntry BindToResourcesListEntry(MySqlDataReader d)
		{
			
			return new ResourceListEntry
			{
				Id = (uint)d["Id"],
				Filename = d["Filename"].ToString(),
				Subject1 = (string)((d["Subject1"] == DBNull.Value) ? null : d["Subject1"]),
				Subject2 = (string)((d["Subject2"] == DBNull.Value) ? null : d["Subject2"]),
				Subject3 = (string)((d["Subject3"] == DBNull.Value) ? null : d["Subject3"]),
				Language = d["Language"].ToString(),
				Mime_type = d["Mime_type"].ToString(),
				Tags = d["Tags"].ToString()
			};
		}

		public async Task<List<ResourceListEntry>> GetListByLanguage(string id)
		{
			List<ResourceListEntry> data = new List<ResourceListEntry>();
			using (MySqlConnection con = new MySqlConnection(_connectionString))
			using (MySqlCommand cmd = new MySqlCommand(@"
				SELECT 
					f.id,
					f.Filename,
					s.name as subject1,
					s2.name as subject2,
					s3.name as subject3,
					l.name as Language,
					Mime_type,
					Tags FROM files as f 
				LEFT JOIN languages as l ON f.language = l.id  
				LEFT JOIN subjects as s on f.subject1 = s.id AND f.subject1 IS NOT NULL
				LEFT JOIN subjects as s2 on f.subject2 = s2.id AND f.subject2 IS NOT NULL
				LEFT JOIN subjects as s3 on f.subject3 = s3.id AND f.subject3 IS NOT NULL
				WHERE f.deleted = 0 and f.language =" + id))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Connection = con;
				await con.OpenAsync();
				// trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
				var d = await cmd.ExecuteReaderAsync();
				if (d.HasRows)
				{
					while (await d.ReadAsync())
					{
						data.Add(BindToResourcesListEntry(d));
					}
				}
				// trvResourcesByLanguages.DataBind();
				con.Close();
			}
			return data;
		}

		public async Task<List<ResourceListEntry>> GetListBySubjects(string id = "2")
		{
			List<ResourceListEntry> data = new List<ResourceListEntry>();
			string sql = @$"
								SELECT
					   files.id,
					   files.filename,
					   files.Tags as Tags,
					   files.subject1 as subjectId1,
					   files.subject2 as subjectId2,
					   files.subject3 as subjectId3,
					   subjects.name As subject1,
					   subjects2.name AS subject2,
					   subjects3.name AS subject3,
					   languages.name AS language,
					   files.mime_type,
					   DATE_FORMAT(files.last_uploaded_timestamp, '%d/%m/%Y') As last_uploaded_date
				FROM files
					LEFT JOIN subjects AS subjects on (files.subject1 = subjects.id AND files.subject1 IS NOT NULL)
					LEFT JOIN subjects AS subjects2 on (files.subject2 = subjects2.id AND files.subject2 IS NOT NULL)
					LEFT JOIN subjects AS subjects3 on (files.subject3 = subjects3.id  AND files.subject3 IS NOT NULL)
					LEFT JOIN languages AS languages on (files.language = languages.id)
				WHERE (files.subject1={id} OR files.subject2={id} OR files.subject3={id}) AND files.deleted=0
			";
			using (MySqlConnection con = new MySqlConnection(_connectionString))
			using (MySqlCommand cmd = new MySqlCommand(sql))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Connection = con;
				await con.OpenAsync();
				// trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
				var d = await cmd.ExecuteReaderAsync();
				if (d.HasRows)
				{
					while (await d.ReadAsync())
					{
						data.Add(BindToResourcesListEntry(d));
					}
				}
				// trvResourcesByLanguages.DataBind();
				await con.CloseAsync();
			}
			return data;
		}

		public async Task<List<ResourceListEntry>> GetListByKnowledgeShared(string id = "2")
		{
			List<ResourceListEntry> data = new List<ResourceListEntry>();
			using (MySqlConnection con = new MySqlConnection(_connectionString))
			using (MySqlCommand cmd = new MySqlCommand(@$"
					SELECT 
						*,
						'' as Subject1, 
						'' as Subject2, 
						'' as Subject3,
						'English' as Language,
						'' as Tags 
					FROM country_knowledge_share_files
					WHERE 
						country_knowledge_share_files.deleted = 0 AND country_knowledge_share_files.country_id = {id} 
					ORDER BY country_knowledge_share_files.filename;"))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Connection = con;
				await con.OpenAsync();
				// trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
				var d = cmd.ExecuteReader();
				if (d.HasRows)
				{
					while (await d.ReadAsync())
					{
						data.Add(BindToResourcesListEntry(d));
					}
				}
				// trvResourcesByLanguages.DataBind();
				await con.CloseAsync();
			}
			return data;
		}

		public async Task<List<ResourceListEntry>> GetListByTeachersDoc(string id = "2")
		{
			List<ResourceListEntry> data = new List<ResourceListEntry>();
			using (MySqlConnection con = new MySqlConnection(_connectionString))
			using (MySqlCommand cmd = new MySqlCommand($@"
				SELECT 
					Id, 
					filename,
					'' Subject1,
					'' Subject2,
					'' Subject3,
					'English' Language,
					Mime_type, 
					'' as Tags  
				FROM files_teachers_support_documents WHERE deleted = 0 Order By filename;"))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Connection = con;
				await con.OpenAsync();
				// trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
				var d = await cmd.ExecuteReaderAsync();
				if (d.HasRows)
				{
					while (await d.ReadAsync())
					{
						data.Add(BindToResourcesListEntry(d));
					}
				}
				// trvResourcesByLanguages.DataBind();
				await con.CloseAsync();
			}
			return data;
		}



		public async Task<ResourceModel> Get(int id)
		{
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand(@$"
				SELECT id, filename, subject1, subject2, subject3, mime_type, last_uploaded_timestamp, tags, language, deleted FROM files WHERE id = {id}
			")) 
			{
				command.Connection = connection;
				await connection.OpenAsync();
				MySqlDataReader d = await command.ExecuteReaderAsync();
				var model = new ResourceModel();
				if (!d.HasRows)
				{
					return null;
				}
				await d.ReadAsync();
				model.Id = (uint)d["id"];
				model.Language = (int)d["Language"];
				model.Filename = (string)d["filename"];
				model.MimeType = d["mime_type"].ToString();
				model.Subject1 = (uint?)((d["Subject1"] == DBNull.Value)? null : d["Subject1"]);
				model.Subject2 = (uint?)((d["Subject2"] == DBNull.Value) ? null : d["Subject2"]);
				model.Subject3 = (uint?)((d["Subject3"] == DBNull.Value) ? null : d["Subject3"]);
				await connection.CloseAsync();
				return model;
			}
			
		}

		public async Task Create(ResourceModel model)
		{
			model.Filename = model.FormFile?.FileName ?? $"File_{DateTime.Now.Date}";
			model.MimeType = GetMime(model.Filename);
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand(@$"
				INSERT INTO files(filename, subject1, subject2, subject3, mime_type, tags, language, last_uploaded_timestamp) VALUES (
					{model.Filename.ToSqlValue()},
					{model.Subject1.ToSqlValue()},
					{model.Subject2.ToSqlValue()},
					{model.Subject3.ToSqlValue()},
					{model.MimeType.ToSqlValue()},
					{model.Tags.ToSqlValue()},
					{model.Language},
					STR_TO_DATE('{DateTime.UtcNow.ToString("MM\\/dd\\/yyyy hh:mm:ss")}', '%m/%d/%Y %r')
				)
			"))
			{
				command.Connection = connection;
				await connection.OpenAsync();
				if (model.FormFile != null)
				{
					await SaveFile(model.Filename, model.FormFile);
				}
				await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}
		public string GetMime(string fileName)
		{
			var provider = new FileExtensionContentTypeProvider();
			string contentType;
			if (!provider.TryGetContentType(fileName, out contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;
		}
		public async Task Update(ResourceModel model)
		{
			model.Filename = model.FormFile?.FileName ?? $"File_{DateTime.Now.Date}";
			model.MimeType = GetMime(model.Filename);
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand(@$"
				UPDATE files SET filename = {model.Filename.ToSqlValue()},
				subject1 = {model.Subject1.ToSqlValue()},
				subject2 = {model.Subject2.ToSqlValue()},
				subject3 = {model.Subject3.ToSqlValue()},
				mime_type = {model.MimeType.ToSqlValue()},
				tags = {model.Tags.ToSqlValue()},
				language = {model.Language} 
				WHERE id = {model.Id}
			"))
			{
				command.Connection = connection;
				await connection.OpenAsync();
				if (model.FormFile != null)
				{
					await SaveFile(model.Filename, model.FormFile);
				}
				await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}

		public async Task Delete(int id)
		{
			using (var connection = new MySqlConnection(_connectionString))
			using (var command = new MySqlCommand(@$"
				UPDATE files SET deleted = 1
				WHERE id = {id}
			"))
			{
				command.Connection = connection;
				await connection.OpenAsync();
				await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}

		public async Task SaveFile(string fileName, IFormFile file) 
		{
			string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "files");
			if (file.Length > 0)
			{
				string filePath = Path.Combine(uploads, fileName);
				using (Stream fileStream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
				}
			}
		}

		public async Task<byte[]> DownloadFile(string fileName)
		{
			string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "files");
			string filePath = Path.Combine(uploads, fileName);
			byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
			return fileBytes;
		}

		public async Task<List<ResourceModel>> GetByLanguage(int languageId)
		{
			await Task.Delay(0);
			return new List<ResourceModel>(); 
		}
	}
}
