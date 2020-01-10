using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Back.com.App.Controllers
{
    class StatisticsController
    {
        public static Dictionary<string, string> getDays()
        {
            var map = new Dictionary<string, string>();

            map.Add("1", "1 jour");
            map.Add("7", "7 jours");
            map.Add("30", "1 mois");
            map.Add("365", "12 mois");
            return map;
        }
    }
}
