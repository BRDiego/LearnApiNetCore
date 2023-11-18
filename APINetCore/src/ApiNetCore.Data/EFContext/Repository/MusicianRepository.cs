using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class MusicianRepository : EntityRepository<Musician>, IMusicianRepository
    {
        public MusicianRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<IEnumerable<Musician>> GetMusiciansByBand(ushort bandId)
        {
            return await DbContext.Musicians.AsNoTracking()
                                        .Include(m => m.Bands)
                                        .Where(m => m.Bands.Where(b => b.Id == bandId).ToList().Count > 0)
                                        .ToListAsync();
        }
    }
}