using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webowka1.Models;

namespace webowka1.Data
{
    public class webowka1Context : DbContext
    {
        public webowka1Context (DbContextOptions<webowka1Context> options)
            : base(options)
        {
        }

        public DbSet<webowka1.Models.Co> OsadzeniTablica { get; set; } = default!;
    }
}
