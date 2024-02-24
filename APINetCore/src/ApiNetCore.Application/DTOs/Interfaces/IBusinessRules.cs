namespace ApiNetCore.Application.DTOs.Interfaces
{
    public interface IBusinessRules
    {
        #region "Musician"
        void ValidateMusicianAge(ref int? age);
        void ValidateMusicianSurname(ref string? surname);
        void ValidateMusicianNickname(ref string? nickname);
        #endregion

        #region "Band"
        void ValidateBandName(ref string? name);
        #endregion
    }
}
