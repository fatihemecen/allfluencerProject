using BlazorApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Client.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {
        
        
        }
        public virtual DbSet<TestAddress> TestAddresses { get; set; }

    }
}
