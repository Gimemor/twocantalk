using EMSWeb.BusinessServices.Repositories;
using EMSWeb.BusinessServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Services
{
    public class UserNameService : IUserNameService
    {
        public string GetUserName()
        {
            TestRepository repo = new TestRepository();
            return repo.GetUserName();
        }
    }
}
