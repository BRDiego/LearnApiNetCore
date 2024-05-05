using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IBandRepository : IEntityRepository<Band>
    {
        public Task<BandMembers> GetBandWithMembers(ushort id);
        public Task<IEnumerable<BandMembers>> ListByMembersDataAsync(DateTime maxBirthDate, DateTime minBirthDate);
    }
}