namespace ApiNetCore.Application.DTOs.Authentication
{
    public class UserAuthorizationDTO
    {
        public string AccessToken { get; set; } = "";
        public double SecondsToExpire { get; set; } = 0;
        public UserDataDTO UserData { get; set; } = new UserDataDTO();
    }

    public class UserDataDTO
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public IEnumerable<ClaimDTO>? Claims { get; set; }
    }

    public class ClaimDTO
    {
        public string TypeName { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
