using EMSWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IResourceLibService
	{
		public Task<ResourceModel> Get(int id);
		public Task Update(ResourceModel model);
		public Task Create(ResourceModel model);
		public Task Delete(int id);
		public Task<byte[]> DownloadFile(string fileName);
		Task<List<ResourceModel>> GetByLanguage(int languageId);
	}
}
