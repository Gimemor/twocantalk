using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Repositories
{
    public class TestRepository:BaseRepository
    {

        public string GetUserName()
        {
            try
            {
                //Just to show you you have to put common methods in base repo
                DataTable dt = ExecuteReader();

                //Just return sting name
                return  "Paul Lee";
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
