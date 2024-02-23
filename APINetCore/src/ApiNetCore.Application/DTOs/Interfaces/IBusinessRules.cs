namespace ApiNetCore.Application.DTOs.Interfaces
{
    public interface IBusinessRules
    {
        #region "Musician"
        void ValidateMusicianAge(int age);
        void ValidateMusicianSurname(string surname);
        void ValidateMusicianNickname(string nickname);
        #endregion

        #region "Band"
        void ValidateBandName(string name);
        #endregion
    }
}
