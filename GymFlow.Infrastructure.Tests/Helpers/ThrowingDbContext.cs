using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Tests.Helpers
{
    public class ThrowingDbContext : TestDbContext
    {
        public ThrowingDbContext(
            DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }


        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            throw new Exception("Database error");
        }
    }
}
