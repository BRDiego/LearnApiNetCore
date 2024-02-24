using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IBandRepository : IEntityRepository<Band>
    {
        public Task<Band?> GetBandWithMembers(ushort id);
    }
}