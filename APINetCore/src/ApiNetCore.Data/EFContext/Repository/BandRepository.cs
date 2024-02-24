using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class BandRepository : EntityRepository<Band>, IBandRepository
    {
        public BandRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<Band?> GetBandWithMembers(ushort id)
        {
            return await dbContext.Band.AsNoTracking()
                                        .Include("Musicians")
                                        .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}