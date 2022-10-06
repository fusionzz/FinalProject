using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class FinalProjectContext : DbContext
    {
        public FinalProjectContext (DbContextOptions<FinalProjectContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.User> User { get; set; } = default!;

        public DbSet<FinalProject.Models.Account> Account { get; set; }

        public DbSet<FinalProject.Models.Transaction> Transaction { get; set; }
    }
}
