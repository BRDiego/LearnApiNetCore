using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class BandRepository : EntityRepository<Band>, IBandRepository
    {
        public BandRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<IEnumerable<Band>> ListBandsByMusician(ushort musicianId)
        {
            return await dbContext.Bands.AsNoTracking()
                                        .Include(b => b.Musicians)
                                        .Where(b => b.Musicians.Where(m => m.Id == musicianId).ToList().Count > 0)
                                        .ToListAsync();
        }

        public async Task<Band> GetBandMembers(ushort id)
        {
            return await dbContext.Bands.AsNoTracking()
                                        .Include(b => b.Musicians)
                                        .Where(b => b.Id == id)
                                        .FirstAsync();
        }
    }
}