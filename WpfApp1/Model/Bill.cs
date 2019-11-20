using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
     public class Bill:BaseModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime DateOut { get; set; }
        public ICollection<BillStore> BillStores { get; set; }
    }
}
