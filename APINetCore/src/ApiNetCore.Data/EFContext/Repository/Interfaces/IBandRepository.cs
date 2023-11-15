using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IBandRepository<TEntity> : IEntityRepository<Band>
    {
        public List<Band> GetBandsByMusician(int musicianId);
    }
}