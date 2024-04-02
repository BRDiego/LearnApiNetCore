using ApiNetCore.Api.Controllers;
using ApiNetCore.Application.DTOs.Authentication;
using ApiNetCore.Application.DTOs.Extensions;
using ApiNetCore.Business.AlertsManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                return CustomResponse(GenerateToken());
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
                return CustomResponse(GenerateToken());

            if (result.IsLockedOut)
                alertManager.AddAlert("user temporarily locked for many attempts");
            else
                alertManager.AddAlert("username, email or password are incorrect");

            return CustomResponse(loginUser);
        }


        private string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appHandshakeSettings.Secret);

            var token = tokenHandler.CreateToken(
                new SecurityTokenDescriptor
                {
                    Issuer = appHandshakeSettings.Sender,
                    Audience = appHandshakeSettings.ValidSpectator,
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

    }
}
