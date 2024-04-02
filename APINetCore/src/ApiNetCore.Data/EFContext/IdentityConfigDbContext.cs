using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext
{
    public class IdentityConfigDbContext : IdentityDbContext
    {

        public IdentityConfigDbContext(DbContextOptions<IdentityConfigDbContext> options) : base(options)
        {

        }
    }
}
