using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
   public class Store:BaseModel
    {
        public int Number { get; set; }
        public double price { get; set; }
        public DateTime DateIn { get; set; }
         public int MaterialId { get; set; }
        public Materail Materail { get; set; }
        public ICollection<BillStore>  BillStores { get; set; }

    }
}
