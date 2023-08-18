using A1.Models;
using Microsoft.EntityFrameworkCore;

namespace A1.Data
{
    public class A1DbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public A1DbContext(DbContextOptions<A1DbContext> options) : base(options) {}
    }
}