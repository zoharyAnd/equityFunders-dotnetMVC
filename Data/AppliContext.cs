using cFunding.Models;
using Microsoft.EntityFrameworkCore;

namespace cFunding.Data
{
    public class AppliContext : DbContext
    {
        public AppliContext(DbContextOptions options) : base(options)
        { }
        public DbSet<user> users { get; set; }
        public DbSet<category> categories { get; set; }
        public DbSet<project> projects { get; set; }
        public DbSet<transaction> transactions { get; set; }
        public DbSet<contactus> contactsus { get; set; }
        public DbSet<question> questions { get; set; }
        public DbSet<answer> answers { get; set; }
        public DbSet<favorite> favorites { get; set; }
    }
}