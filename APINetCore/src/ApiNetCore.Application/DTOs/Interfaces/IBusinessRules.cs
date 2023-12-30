using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNetCore.Application.DTOs.Interfaces
{
    public interface IBusinessRules
    {
        #region "Musician"
        bool IsValidMusicianAge(int age);
        bool IsValidMusicianSurname(string surname);
        bool IsValidMusicianNickname(string nickname);
        #endregion

        #region "Band"
        bool IsValidBandName(string name);
        #endregion
    }
}
