using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EMSWeb.Models
{
	[DataContract]
	public class User
	{
		[DataMember]
		public uint Id { get; set; }

		[DataMember]
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[DataMember]
		public string OrganizationName { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public string PasswordPlain { get; set; }

		[DataMember]
		public string SecondaryPassword { get; set; }

		[DataMember]
		public bool PermAnyBrowser { get; set; }

		[DataMember]
		public bool PermVault { get; set; }

		[DataMember]
		public bool PermTextTutor { get; set; }

		[DataMember]
		public bool PermTalkingTutor { get; set; }


		[DataMember]
		public bool PermTwoCanTalk { get; set; }

		[DataMember]
		public bool PermPhrasebook { get; set; }

		[DataMember]
		public uint? PhrasebookId { get; set; }

		[DataMember]
		public bool PermSecondaryLogin { get; set; }

		[DataMember]
		public DateTime? LicenseStartDate { get; set; }

		[DataMember]
		public DateTime? LicenseEndDate { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public string MembershipNumber { get; set; }

		[DataMember]
		public bool Active { get; set; }

		[DataMember]
		public uint? ResellerId { get; set; }

		[DataMember]
		public uint? ConcurrentSessionLimit { get; set; }

		[DataMember]
		public string IpAddressWhitelist { get; set; }

		[DataMember]
		public string ContactForename { get; set; }

		[DataMember]
		public string ContactSurname { get; set; }

		[DataMember]
		public string AddressLine1 { get; set; }

		[DataMember]
		public string AddressLine2 { get; set; }

		[DataMember]
		public string AddressLine3 { get; set; }

		[DataMember]
		public string AddressLine4 { get; set; }

		[DataMember]
		public string AddressPostcode { get; set; }

		[DataMember]
		public DateTime? TimestampCreated { get; set; }

		[DataMember]
		public bool EnableChangePassword { get; set; }
		
	}
}

// id, username, organisation_name, password, password_plain,
// secondary_password, perm_any_browser, perm_vault, perm_text_tutor, 
// perm_talking_tutor, perm_twocan_talk, perm_phrasebook, phrasebook_id, 
// perm_secondary_login, license_start_date, license_end_date, type, membership_number,
// active, reseller_id, concurrent_sessions_limit, ip_address_whitelist, contact_forename, c
// contact_surname, address_line_1, address_line_2, address_line_3, address_line_4, address_postcode, timestamp_created, enableChangePassword