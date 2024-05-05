using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext.Repository
{
    public class BandRepository : EntityRepository<Band>, IBandRepository
    {
        public BandRepository(ApplicationDbContext context) : base(context)  { }

        public async Task<BandMembers> GetBandWithMembers(ushort id)
        {
            var band = await dbContext.Band.AsNoTracking()
                                        .FirstAsync(b => b.Id == id);

            var members = await dbContext.BandMusician.AsNoTracking()
                                                        .Where(junction => junction.BandId == id)
                                                        .Include(junction => junction.Musician)
                                                        .ToListAsync();

            return new BandMembers()
            {
                Band = band,
                Members = members.Select(m => m.Musician).ToList()
            };
        }
        public async Task<IEnumerable<BandMembers>> ListByMembersDataAsync(DateTime maxBirthDate, DateTime minBirthDate)
        {
            var members = await dbContext.BandMusician.AsNoTracking()
                                                        .Where(junction => junction.Musician.DateOfBirth >= minBirthDate && junction.Musician.DateOfBirth <= maxBirthDate)
                                                        .Include(b => b.Band)
                                                        .Include(m => m.Musician)
                                                        .ToListAsync();

            if (members is null || members.Count == 0)
                return Enumerable.Empty<BandMembers>();

            var bandsList = members.DistinctBy(item => item.BandId).Select(r => new BandMembers() { Band = r.Band }).ToList();

            foreach (var band in bandsList)
                band.Members = members.Where(item => item.BandId == band.Band.Id).Select(item => item.Musician).ToList();

            return bandsList;
        }
    }
}