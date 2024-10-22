using DAT250_REST.Models;
using Microsoft.EntityFrameworkCore;

namespace DAT250_REST.Data
{
    public class AppDBContext: DbContext
    {
        protected readonly IConfiguration Configuration;
        public AppDBContext(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            Options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteOption> VoteOptions { get; set; }


    }
}
