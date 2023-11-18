using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class BandRepository : EntityRepository<Band>, IBandRepository
    {
        public BandRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<IEnumerable<Band>> GetBandsByMusician(ushort musicianId)
        {
            return await DbContext.Bands.AsNoTracking()
                                        .Include(b => b.Musicians)
                                        .Where(b => b.Musicians.Where(m => m.Id == musicianId).ToList().Count > 0)
                                        .ToListAsync();
        }
    }
}