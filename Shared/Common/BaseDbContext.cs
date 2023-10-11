using Common.Interface;
using Common.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Common
{
    public abstract class BaseDbContext : DbContext, IDatabaseContext
    {
        public BaseDbContext(DbContextOptions options)
             : base(options) { }

        public string Token { get; set; }
        public AuthenticatedAccount CurrentAccount { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }
}
