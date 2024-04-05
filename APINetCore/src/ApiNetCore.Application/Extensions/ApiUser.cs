using ApiNetCore.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApiNetCore.Application.Extensions
{
    public class ApiUser : IUser
    {
        private readonly IHttpContextAccessor accessor;
        private readonly ClaimsPrincipal httpUser; 

        public ApiUser(IHttpContextAccessor htppAccessor)
        {
            this.accessor = htppAccessor;
            httpUser = accessor.HttpContext.User;
        }

        public string Name => httpUser.Identity!.Name!;

        public int GetId() => httpUser.GetUserId();
        public string GetEmail() => httpUser.GetUserEmail();

        public IEnumerable<Claim> GetClaims() => httpUser.Claims;

        public bool IsAuthenticated() => httpUser.Identity!.IsAuthenticated;

        public bool IsInRole(string role) => httpUser.IsInRole(role);
    }

    public static class ClaimsPrincipalExtension
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            CheckPrincipal(principal);

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value!;
        }
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            CheckPrincipal(principal);

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(claim?.Value);
        }

        private static void CheckPrincipal(ClaimsPrincipal principal)
        {
            if (principal is null)
                throw new ArgumentException(nameof(principal));
        }
    }
}
