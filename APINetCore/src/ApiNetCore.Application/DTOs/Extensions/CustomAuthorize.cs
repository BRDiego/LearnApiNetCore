using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ApiNetCore.Application.DTOs.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidateUserClaims(HttpContext context, string claimTypeName, string claimValue)
        {
            return context.User.Identity!.IsAuthenticated &&
                context.User.Claims.Any(cl => cl.Type == claimTypeName && cl.Value.Contains(claimValue));
        }
    }

    public class ClaimsAuthorization : TypeFilterAttribute
    {
        public ClaimsAuthorization(string claimTypeName, string claimValue) : base(typeof(RequirementClaimFilter))
        {
            Arguments = new object[] { new Claim(claimTypeName, claimValue) };
        }
    }

    public class RequirementClaimFilter : IAuthorizationFilter
    {
        private readonly Claim claim;

        public RequirementClaimFilter(Claim claim)
        {
            this.claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomAuthorization.ValidateUserClaims(context.HttpContext, claim.Type, claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
