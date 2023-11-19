using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class MusicianRepository : EntityRepository<Musician>, IMusicianRepository
    {
        public MusicianRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<IEnumerable<Musician>> ListMusiciansByBand(ushort bandId)
        {
            return await dbContext.Musicians.AsNoTracking()
                                        .Include(m => m.Bands)
                                        .Where(m => m.Bands.Where(b => b.Id == bandId).ToList().Count > 0)
                                        .ToListAsync();
        }

        public async Task<Musician> GetMusicianBands(ushort id)
        {
            return await dbContext.Musicians.AsNoTracking()
                                        .Include(m => m.Bands)
                                        .Where(m => m.Id == id)
                                        .FirstAsync();
        }
    }
}