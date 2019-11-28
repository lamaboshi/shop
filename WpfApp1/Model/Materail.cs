using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
  public  class Materail:BaseModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int TypeId { get; set; }
        public Type type { get; set; }
        public ICollection<BillMaterail>  billMaterails { get; set; }
    }
}
