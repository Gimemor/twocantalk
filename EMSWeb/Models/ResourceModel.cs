using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EMSWeb.Models
{

	[DataContract]
	public class AjaxResourceList
	{
		public AjaxResourceList(List<ResourceListEntry> entries)
		{
			Data = entries;
		}

		[DataMember]
		public List<ResourceListEntry> Data { get; set; }
	}
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

	[DataContract]
	public class ResourceListEntry
	{
		[DataMember]
		public string Filename { get; set; }
		[DataMember]
		public string Language { get; set; }
		[DataMember]
		public string Subject1 { get; set; }
		[DataMember]
		public string Subject2 { get; set; }
		[DataMember]
		public string Subject3 { get; set; }
		[DataMember]
		public string Mime_type { get; set; }
		[DataMember]
		public string Tags { get; set; }
		[DataMember]
		public uint Id { get; set; }
	}
}
