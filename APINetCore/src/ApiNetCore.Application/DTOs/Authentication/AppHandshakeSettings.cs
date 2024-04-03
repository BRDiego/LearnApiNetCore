namespace ApiNetCore.Application.DTOs.Authentication
{
    public class AppHandshakeSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int HoursToExpire { get; set; } = 0;
        public string Sender { get; set; } = string.Empty;
        public string ValidSpectator { get; set; } = string.Empty;
    }
}
