
using EMSWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IKnowledgeService
	{
		public Task<List<KnowledgeSharebyCountry>> GetList();
	}
}
