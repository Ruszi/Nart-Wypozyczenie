using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wypożyczalnia_Nart
{
    class Rental
    {
        public string CustomerName { get; set; }
        public string EquipmentName { get; set; }
        public DateTime Date { get; set; }

       public override string ToString()
{
         return $"{CustomerName} - {EquipmentName}";
}

    }
}
