using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
   public class ContactContext:DbContext
    {
        public ContactContext() : base("Name=ConnectionStringLama")
        {

        }
        public DbSet<Type> Types { get; set; }
        public DbSet<Materail> Materails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillMaterail> billMaterails { get; set; }

    }
}
