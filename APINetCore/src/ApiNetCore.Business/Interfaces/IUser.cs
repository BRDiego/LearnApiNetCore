using System.Runtime.InteropServices;
using System.Security.Claims;

namespace ApiNetCore.Business.Interfaces
{
    public interface IUser
    {
        string Name { get; }
        public int GetId();
        public string GetEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaims();
    }
}
