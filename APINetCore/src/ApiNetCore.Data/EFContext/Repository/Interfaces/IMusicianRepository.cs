using System.Linq.Expressions;
using ApiNetCore.Business.Models;

namespace ApiNetCore.Data.EFContext.Repository.Interfaces
{
    public interface IMusicianRepository : IEntityRepository<Musician>
    {
        public Task<IEnumerable<Musician>> GetMusiciansByBand(ushort bandId);
    }
}