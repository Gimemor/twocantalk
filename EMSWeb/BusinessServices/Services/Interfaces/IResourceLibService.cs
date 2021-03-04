using EMSWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IResourceLibService
	{
		// TODO merge get list methods into a single one with filtering support
		public Task<List<ResourceListEntry>> GetListByLanguage(string id);
		public Task<List<ResourceListEntry>> GetListBySubjects(string id);
		public Task<List<ResourceListEntry>> GetListByKnowledgeShared(string id);
		public Task<List<ResourceListEntry>> GetListByTeachersDoc(string id);

		public Task<ResourceModel> Get(int id);
		public Task Update(ResourceModel model);
		public Task Create(ResourceModel model);
		public Task Delete(int id);
		public Task<byte[]> DownloadFile(string fileName);
		Task<List<ResourceModel>> GetByLanguage(int languageId);
	}
}
