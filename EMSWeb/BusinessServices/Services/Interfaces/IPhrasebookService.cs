
using EMSWeb.Models;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IPhrasebookService
	{
		public Task<PhrasebookList> GetPhrasebook();
		public Task Delete(DeletePhrasebookDto[] phrases);
		public Task<int> CreatePhrase(CreatePhraseDto createPhrase);
		public Task<int> CreateList(CreateListDto createPhrase);
		public Task Modify(ModifyNodeDto node);
	}
}
