using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
   public class Type:BaseModel
    {
        public string Name { get; set; }
        public ICollection<Materail> Materails { get; set; }
    }
}
