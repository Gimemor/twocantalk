using EMSWeb.BusinessServices.Helpers;
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
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

		public async Task Update(ResourceModel model)
		{
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
