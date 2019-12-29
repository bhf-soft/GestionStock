using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Back.com.App.Controllers
{
    class UsersController
    {
        public static StockDATAEntities StkInfo = new StockDATAEntities();

        public static Dictionary<string, string> GetUsers()
        {
            var map = new Dictionary<string, string>();
            var q = (from a in StkInfo.Users
                     select new
                     {
                         id = a.Id,
                         name = a.FullName
                     }).Distinct().ToList();

            for (int i = 0; i < q.Count; i++)
            {
                map.Add(q[i].id.ToString(), q[i].name);
            }
            return map;
        }
    }
}
