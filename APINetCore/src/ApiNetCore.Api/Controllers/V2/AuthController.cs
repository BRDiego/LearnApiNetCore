using ApiNetCore.Api.Controllers;
using ApiNetCore.Application.DTOs.Authentication;
using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APINetCore.Api.Controllers.V2
{
    [AllowAnonymous]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly AppHandshakeSettings appHandshakeSettings;

        public AuthController(IAlertManager alertManager,
                            SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IOptions<AppHandshakeSettings> appHandshakeSettingsOptions,
                            IUser user)
                            : base(alertManager, user)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            appHandshakeSettings = appHandshakeSettingsOptions.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = new IdentityUser()
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return CustomResponse(await BuildAuthorization(registerUser.Email));
            }
            foreach (var error in result.Errors)
            {
                alertManager.AddAlert(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUser)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await signInManager.PasswordSignInAsync(loginUser.Email,
                                                                loginUser.Password,
                                                                false,
                                                                true);

            if (result.Succeeded)
                return CustomResponse(await BuildAuthorization(loginUser.Email));

            if (result.IsLockedOut)
                alertManager.AddAlert("user temporarily locked for many attempts");
            else
                alertManager.AddAlert("username, email or password are incorrect");

            return CustomResponse(loginUser);
        }


        private async Task<UserAuthorizationDTO> BuildAuthorization(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            var identityClaims = await GetIdentityClaims(user!);

            string encodedToken = BuildToken(identityClaims);

            var authorization = new UserAuthorizationDTO()
            {
                AccessToken = encodedToken,
                SecondsToExpire = TimeSpan.FromHours(appHandshakeSettings.HoursToExpire).TotalSeconds,
                UserData = new UserDataDTO()
                {
                    Id = user!.Id,
                    Email = user.Email!,
                    Claims = identityClaims.Claims.Select(
                                                    cl => new ClaimDTO() { TypeName = cl.Type, Value = cl.Value }
                                                    )
                }
            };

            return authorization;
        }

        private string BuildToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appHandshakeSettings.Secret);

            var token = tokenHandler.CreateToken(
                new SecurityTokenDescriptor
                {
                    Issuer = appHandshakeSettings.Sender,
                    Audience = appHandshakeSettings.ValidSpectator,
                    Subject = identityClaims,
                    Expires = DateTime.UtcNow.AddHours(appHandshakeSettings.HoursToExpire),
                    SigningCredentials = new SigningCredentials(
                                            new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha256Signature
                                            )
                }
            );

            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetIdentityClaims(IdentityUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user!.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user!.Email!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ConvertToEpochDate(DateTime.Now).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ConvertToEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private static long ConvertToEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
