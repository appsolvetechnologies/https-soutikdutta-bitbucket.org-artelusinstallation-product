using System.Data.Entity;

namespace Artelus.Model
{
    class ArtelusDbContext : DbContext
    {
        static ArtelusDbContext()
        {
            Database.SetInitializer<ArtelusDbContext>(null);
        }
        public ArtelusDbContext() : base("name=ConnectionString") { }
    }
}
