
using EMSWeb.Models;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IPhrasebookService
	{
		public Task<PhrasebookList> GetPhrasebook();
		public Task Delete(DeletePhrasebookDto[] phrases);
		public Task CreatePhrase(CreatePhraseDto createPhrase);
		public Task CreateList(CreateListDto createPhrase);
		public Task Modify(ModifyNodeDto node);
	}
}
