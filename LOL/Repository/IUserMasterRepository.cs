using LOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOL.Repository
{
    interface IUserMasterRepository
    {
        IEnumerable<Film> GetAll();
       Film Get(int id);
       Film Add(Film item);
        bool Update(Film item);
        bool Delete(int id);
    }
}
