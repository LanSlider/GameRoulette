using System.Data.Entity;

namespace GameRoulette.Models
{
    public class ErrorsContext : DbContext
    {
        public ErrorsContext() : 
            base("GameDropDB")
        {}

        public DbSet<Error> Errors { get; set; }
    }
}