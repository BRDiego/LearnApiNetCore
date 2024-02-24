using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class MusicianRepository : EntityRepository<Musician>, IMusicianRepository
    {
        public MusicianRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<Musician?> GetMusicianWithBands(ushort id)
        {
            return await dbContext.Musician.AsNoTracking()
                                        .Include(m => m.Bands)
                                        .Where(m => m.Id == id)
                                        .FirstOrDefaultAsync();
        }
    }
}