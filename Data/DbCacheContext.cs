using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache.Products.Data
{
    public class DbCacheContext: DbContext
    {
        public DbSet<Models.Products> Products { get; set; }
        public DbCacheContext(DbContextOptions<DbCacheContext> options)
            : base(options)
        { }
    }
}
