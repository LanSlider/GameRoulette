using System.Data.Entity;

namespace GameRoulette.Models
{
    public class UserContext : DbContext
    {
        public UserContext() :
            base("GameDropDB")
        { }

        public DbSet<User> Users { get; set; }
    }
}