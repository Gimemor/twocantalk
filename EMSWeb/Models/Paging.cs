using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMSWeb.Models
{
	[DataContract]
	public class Paging
	{
		[DataMember]
		public int PageSize { get; set; }
		[DataMember]
		public int PageIndex { get; set; }
	}

	[DataContract]
	public class PagingResult<T>
	{ 
		[DataMember]
		public int ItemsCount { get; set; }

		[DataMember]
		public List<T> Data { get; set; }
	}
}
