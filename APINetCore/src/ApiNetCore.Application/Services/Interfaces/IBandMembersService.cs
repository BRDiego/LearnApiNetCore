using ApiNetCore.Application.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNetCore.Application.Services.Interfaces
{
    public interface IBandMembersService
    {
        Task<BandMembersDTO> GetBandWithMembers(ushort id);
        Task<IEnumerable<BandMembersDTO>> ListByMusiciansAgeAsync(int? minimumMusicianAge, int? maximumMusicianAge);
    }
}
