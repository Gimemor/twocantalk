using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Repositories
{
    public class BaseRepository
    {
        public virtual DataTable ExecuteReader()
        {
            return new DataTable();
        }
    }
}
