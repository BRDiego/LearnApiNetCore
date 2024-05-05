using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class MusicianRepository : EntityRepository<Musician>, IMusicianRepository
    {
        public MusicianRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<MusicianMemberships> GetMusicianWithBands(ushort id)
        {
            var musician = await dbContext.Musician.AsNoTracking()
                                        .Where(m => m.Id == id)
                                        .FirstAsync();

            var memberships = await dbContext.BandMusician.AsNoTracking()
                                                            .Where(junction => junction.MusicianId == id)
                                                            .Include(junction => junction.Band)
                                                            .ToListAsync();

            return new MusicianMemberships()
            {
                Musician = musician,
                Memberships = memberships.Select(b => b.Band).ToList()
            };
        }
    }
}