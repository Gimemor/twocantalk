using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EMSWeb.Models
{
	[DataContract]
	public class PhrasebookPhrase 
	{
		[DataMember]
		public uint Id { get; set; }

		[DataMember]
		public int ListId { get; set; }

		[DataMember]
		public string Content { get; set; }

		[DataMember]
		public int CreatedBy { get; set; }

		[DataMember]
		public int SortOrder { get; set; }
	}

	[DataContract]
	public class PhrasebookList
	{
		public PhrasebookList() {
			ChildLists = new List<PhrasebookList>();
			Phrases = new List<PhrasebookPhrase>();
		}

		[DataMember]
		public uint Id { get; set; }

		[DataMember]
		public int CreatedBy { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int SortOrder { get; set; }
		
		[DataMember]
		public string OrgName { get; set; }
		
		[DataMember]
		public int ParentId { get; set; }

		[DataMember]
		public List<PhrasebookList> ChildLists { get; set; }

		[DataMember]
		public List<PhrasebookPhrase> Phrases { get; set; }
	}

	[DataContract]
	public class DeletePhrasebookDto 
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public bool IsList { get; set; }
	}

	[DataContract]
	public class CreatePhraseDto
	{ 
		[DataMember]
		public string Text { get; set; }

		[DataMember]
		public int ListId { get; set; }
	}

	[DataContract] 
	public class CreateListDto
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ParentId { get; set; }
	}


	[DataContract]
	public class ChangeCategoryDto 
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int ParentId { get; set; }

		[DataMember]
		public bool? IsList { get; set; }
	}

	[DataContract]
	public class ModifyNodeDto 
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public bool IsList { get; set; }
	}
}
