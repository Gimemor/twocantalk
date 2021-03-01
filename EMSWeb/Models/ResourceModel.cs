using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EMSWeb.Models
{
	[DataContract]
	public class ResourceModel
	{
		[DataMember]
		public uint Id { get; set; }
		[DataMember]
		public string Filename { get; set; }

		[DataMember]
		public string MimeType { get; set; }
		[DataMember]
		public string Tags { get; set; }
		[DataMember]
		public DateTime? LastUploadedTimestamp { get; set; }
		[DataMember]
		public int Language { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public uint? Subject1 { get; set; }
		[DataMember]
		public uint? Subject2 { get; set; }
		[DataMember]
		public uint? Subject3 { get; set; }

		[DataMember]
		[Required]
		[Display(Name = "File")]
		public IFormFile FormFile { get; set; }
	}
}
