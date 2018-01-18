using System.Data.Entity;

namespace GameRoulette.Models
{
    public class AppIDContext : DbContext
    {
        public AppIDContext() : 
            base("GameDropDB")
        {}

        public DbSet<AppID> AppID { get; set; }
    }
}