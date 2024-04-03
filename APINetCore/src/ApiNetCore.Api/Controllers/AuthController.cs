using ApiNetCore.Api.Controllers;
using ApiNetCore.Application.CustomExceptions;
using ApiNetCore.Application.DTOs.Authentication;
using ApiNetCore.Application.DTOs.Extensions;
using ApiNetCore.Business.AlertsManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APINetCore.Api.Controllers
{
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly AppHandshakeSettings appHandshakeSettings;

        public AuthController(IAlertManager alertManager,
                            SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IOptions<AppHandshakeSettings> appHandshakeSettingsOptions)
                            : base(alertManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.appHandshakeSettings = appHandshakeSettingsOptions.Value;
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
                return CustomResponse(await GenerateToken(registerUser.Email));
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
                return CustomResponse(await GenerateToken(loginUser.Email));

            if (result.IsLockedOut)
                alertManager.AddAlert("user temporarily locked for many attempts");
            else
                alertManager.AddAlert("username, email or password are incorrect");

            return CustomResponse(loginUser);
        }


        private async Task<string> GenerateToken(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                alertManager.AddAlert("could not load user for authentication");
                ShowAlertsException.Throw();
            }

            var claims = await userManager.GetClaimsAsync(user!);
            var roles = await userManager.GetRolesAsync(user!);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user!.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user!.Email!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ConvertToEpochDate(DateTime.Now).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ConvertToEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

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

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        private static long ConvertToEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
