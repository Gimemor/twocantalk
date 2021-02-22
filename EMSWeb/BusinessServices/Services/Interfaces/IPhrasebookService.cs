
using EMSWeb.Models;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IPhrasebookService
	{
		public Task<PhrasebookList> GetPhrasebook();
	}
}
