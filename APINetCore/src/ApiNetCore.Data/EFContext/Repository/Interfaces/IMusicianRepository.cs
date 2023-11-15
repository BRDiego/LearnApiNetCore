using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IMusicianRepository<TEntity> : IEntityRepository<Band>
    {
        public List<Musician> GetMusicians(int bandId);
    }
}