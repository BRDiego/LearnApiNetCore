using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IBandRepository : IEntityRepository<Band>
    {
        public Task<IEnumerable<Band>> GetBandsByMusician(ushort musicianId);
    }
}