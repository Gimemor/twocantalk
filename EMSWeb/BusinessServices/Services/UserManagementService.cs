
using EMSWeb.BusinessServices.Helpers;
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
	public class UserManagementService : IUserManagementService
	{
		//
		// 
		// FROM users;
		// 
		private string _connectionSettings { get; set; }
		public UserManagementService(IConfiguration configuration)
		{
			_connectionSettings = configuration.GetConnectionString("DefaultConnection");
		}

		public User BindUser(MySqlDataReader d)
		{
			return new User
			{
				Id = (uint)d["id"],
				Username = d["username"].ToString(),
				OrganizationName = d["organisation_name"].ToString(),
				Password = d["password"].ToString(),
				PasswordPlain = d["password_plain"].ToString(),
				SecondaryPassword = d["secondary_password"].ToString(),
				PermAnyBrowser = ((bool)d["perm_any_browser"]),
				PermPhrasebook = ((bool)d["perm_phrasebook"]),
				PermSecondaryLogin = ((byte)d["perm_secondary_login"]) > 0,
				PermTalkingTutor = ((bool)d["perm_talking_tutor"]),
				PermTextTutor = ((bool)d["perm_text_tutor"]),
				PermTwoCanTalk = ((bool)d["perm_twocan_talk"]),
				PermVault = ((bool)d["perm_vault"]),
				Active = ((bool)d["active"]),
				AddressLine1 = d["address_line_1"].ToString(),
				AddressLine2 = d["address_line_2"].ToString(),
				AddressLine3 = d["address_line_3"].ToString(),
				AddressLine4 = d["address_line_4"].ToString(),
				AddressPostcode = d["address_postcode"].ToString(),
				ConcurrentSessionLimit = (d["reseller_id"] == DBNull.Value) ? 0 : (uint?)d["concurrent_sessions_limit"],
				ContactForename = d["contact_forename"].ToString(),
				ContactSurname = d["contact_surname"].ToString(),
				EnableChangePassword = ((bool)d["enableChangePassword"]),
				IpAddressWhitelist = d["ip_address_whitelist"].ToString(),
				LicenseEndDate = (d["license_end_date"] == DBNull.Value) ? DateTime.MinValue : (DateTime?)d["license_end_date"],
				LicenseStartDate = (d["license_start_date"] == DBNull.Value) ? DateTime.MinValue : (DateTime?)d["license_start_date"],
				MembershipNumber = d["membership_number"].ToString(),
				PhrasebookId = (d["phrasebook_id"] == DBNull.Value) ? 0 : (uint?)d["phrasebook_id"],
				ResellerId = (d["reseller_id"] == DBNull.Value) ? 0 : (uint?)d["reseller_id"],
				TimestampCreated = (d["timestamp_created"] == DBNull.Value) ? DateTime.MinValue : (DateTime?)d["timestamp_created"],
				Type = d["type"].ToString()
			};
		}
		public async Task<PagingResult<User>> GetList(Paging paging)
		{
			var commandText = @"
				SELECT id, username, organisation_name, password, password_plain,  
					secondary_password, perm_any_browser, perm_vault, perm_text_tutor, perm_talking_tutor,  
					perm_twocan_talk, perm_phrasebook, phrasebook_id, perm_secondary_login, license_start_date, license_end_date, 
					type, membership_number, active, reseller_id, concurrent_sessions_limit, ip_address_whitelist, contact_forename,  
					contact_surname, address_line_1, address_line_2, address_line_3, address_line_4, address_postcode, timestamp_created, enableChangePassword
				FROM users";
			 
			var list = new List<User>();
			using (var connection = new MySqlConnection(_connectionSettings))
			using (var command = new MySqlCommand(commandText, connection))
			{
				command.CommandType = CommandType.Text;
				await connection.OpenAsync();
				var d = await command.ExecuteReaderAsync();
				if (d.HasRows)
				{
					while (await d.ReadAsync())
					{
						list.Add(BindUser(d));
					}
				}
				await connection.CloseAsync();
			}
			return new PagingResult<User>()
			{
				Data = list.Skip(paging.PageSize * (paging.PageIndex - 1)).Take(paging.PageSize).ToList(),
				ItemsCount = list.Count
			};
		}


		public async Task<User> Get(int id)
		{
			var commandText = "SELECT id, username, organisation_name, password, password_plain, " +
					"secondary_password, perm_any_browser, perm_vault, perm_text_tutor, perm_talking_tutor, " +
					"perm_twocan_talk, perm_phrasebook, phrasebook_id, perm_secondary_login, license_start_date, license_end_date, " +
					"type, membership_number, active, reseller_id, concurrent_sessions_limit, ip_address_whitelist, contact_forename, " +
					"contact_surname, address_line_1, address_line_2, address_line_3, address_line_4, address_postcode, timestamp_created, enableChangePassword " +
				$"FROM users WHERE id = {id}";
			User user = null;
			using (var connection = new MySqlConnection(_connectionSettings))
			using (var command = new MySqlCommand(commandText, connection))
			{
				command.CommandType = CommandType.Text;
				await connection.OpenAsync();
				var d = await command.ExecuteReaderAsync();
				if (d.HasRows)
				{
					await d.ReadAsync();
					user = BindUser(d);
				}
				await connection.CloseAsync();
			}
			return user;
		}




		public async Task Update(User user)
		{
			var commandText = @$"
			UPDATE users SET 
				 username = {user.Username.ToSqlValue()}, 
				 organisation_name = {user.OrganizationName.ToSqlValue()},
				 password = {user.Password.ToSqlValue()}, 
				 password_plain = {user.PasswordPlain.ToSqlValue()}, 
				 secondary_password = {user.SecondaryPassword.ToSqlValue()}, 
				 perm_any_browser = {user.PermAnyBrowser.ToSqlValue()}, 
				 perm_vault = {user.PermVault.ToSqlValue()}, 
				 perm_text_tutor = {user.PermTextTutor.ToSqlValue()}, 
				 perm_talking_tutor = {user.PermTalkingTutor.ToSqlValue()}, 
				 perm_twocan_talk = {user.PermTwoCanTalk.ToSqlValue()}, 
				 perm_phrasebook = {user.PermPhrasebook.ToSqlValue()}, 
				 phrasebook_id = {user.PhrasebookId.ToSqlValue()}, 
				 perm_secondary_login = {user.PermSecondaryLogin.ToSqlValue()}, 
				 license_start_date = {user.LicenseStartDate.ToSqlValue()}, 
				 license_end_date = {user.LicenseEndDate.ToSqlValue()}, 
				 type = {user.Type.ToSqlValue()}, 
				 membership_number = {user.MembershipNumber.ToSqlValue()}, 
				 active = {user.Active.ToSqlValue()}, 
				 reseller_id = {user.ResellerId.ToSqlValue()}, 
				 concurrent_sessions_limit = {user.ConcurrentSessionLimit.ToSqlValue()}, 
				 ip_address_whitelist = {user.IpAddressWhitelist.ToSqlValue()}, 
				 contact_forename = {user.ContactForename.ToSqlValue()},  
				 contact_surname = {user.ContactSurname.ToSqlValue()},
				 address_line_1 = {user.AddressLine1.ToSqlValue()}, 
				 address_line_2 = {user.AddressLine2.ToSqlValue()}, 
				 address_line_3 = {user.AddressLine3.ToSqlValue()}, 
				 address_line_4 = {user.AddressLine4.ToSqlValue()}, 
				 address_postcode = {user.AddressPostcode.ToSqlValue()}, 
				 enableChangePassword = {user.EnableChangePassword.ToSqlValue()}               
			WHERE id = {user.Id}
			";
			using (var connection = new MySqlConnection(_connectionSettings))
			using (var command = new MySqlCommand(commandText, connection))
			{
				command.CommandType = CommandType.Text;
				await connection.OpenAsync();
				var d = await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}

		public async Task Insert(User user)
		{
			var commandText = @$"
				INSERT INTO users ( 
                             username,
                             organisation_name,
                             password,
                             password_plain,
                             secondary_password,
                             perm_any_browser,
                             perm_vault,
                             perm_text_tutor,
                             perm_talking_tutor,
                             perm_twocan_talk,
                             perm_phrasebook,
                             phrasebook_id,
                             perm_secondary_login,
                             license_start_date,
                             license_end_date,
                             type,
                             membership_number,
                             active,
                             reseller_id,
                             concurrent_sessions_limit,
                             ip_address_whitelist,
                             contact_forename,
                             contact_surname,
                             address_line_1,
                             address_line_2,
                             address_line_3,
                             address_line_4,
                             address_postcode,
                             enableChangePassword)
					 VALUES (
                             {user.Username.ToSqlValue()},
                             {user.OrganizationName.ToSqlValue()},
                             {user.Password.ToSqlValue()},
                             {user.PasswordPlain.ToSqlValue()},
                             {user.SecondaryPassword.ToSqlValue()},
                             {user.PermAnyBrowser.ToSqlValue()},
                             {user.PermVault.ToSqlValue()},
                             {user.PermTextTutor.ToSqlValue()},
                             {user.PermPhrasebook.ToSqlValue()},
                             {user.PermTwoCanTalk.ToSqlValue()},
                             {user.PermPhrasebook.ToSqlValue()},
                             {user.PhrasebookId.ToSqlValue()},
                             {user.PermSecondaryLogin.ToSqlValue()},
                             {user.LicenseStartDate.ToSqlValue()},
                             {user.LicenseEndDate.ToSqlValue()},
                             {user.Type.ToSqlValue()},
                             {user.MembershipNumber.ToSqlValue()},
                             {user.Active.ToSqlValue()},
                             {user.ResellerId.ToSqlValue()},
                             {user.ConcurrentSessionLimit.ToSqlValue()},
                             {user.IpAddressWhitelist.ToSqlValue()},
                             {user.ContactForename.ToSqlValue()},
                             {user.ContactSurname.ToSqlValue()},
                             {user.AddressLine1.ToSqlValue()},
                             {user.AddressLine2.ToSqlValue()},
                             {user.AddressLine3.ToSqlValue()},
                             {user.AddressLine4.ToSqlValue()},
                             {user.AddressPostcode.ToSqlValue()},
                             {user.EnableChangePassword.ToSqlValue()}
				)
			";
			using (var connection = new MySqlConnection(_connectionSettings))
			using (var command = new MySqlCommand(commandText, connection))
			{
				command.CommandType = CommandType.Text;
				await connection.OpenAsync();
				var d = await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}

		public async Task Delete(int id)
		{
			var commandText = $"DELETE FROM users WHERE id = {id}";
			using (var connection = new MySqlConnection(_connectionSettings))
			using (var command = new MySqlCommand(commandText, connection))
			{
				command.CommandType = CommandType.Text;
				await connection.OpenAsync();
				var d = await command.ExecuteNonQueryAsync();
				await connection.CloseAsync();
			}
		}
	}
}
