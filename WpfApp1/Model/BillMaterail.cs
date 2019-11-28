using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
     public class BillMaterail:BaseModel
    {
        public int Count { get; set; }
        public int MaterailId { get; set; }
        public Materail materail { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
