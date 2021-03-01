using EMSWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface ITeacherSupportDocumentService
	{
		public Task<List<TeachersSupportDocuments>> GetList();

	}
}
