using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
     public class BillStore:BaseModel
    {
        public int Count { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
