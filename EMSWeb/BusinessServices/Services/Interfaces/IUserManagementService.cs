
using EMSWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services.Interfaces
{
	public interface IUserManagementService
	{
		public Task<PagingResult<User>> GetList(UserFilter filter);
		public Task<User> Get(int id);
		public Task Insert(User user);
		public Task Update(User user);
		public Task Delete(int id);
	}
}
