using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JobApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly JwtOptions _jwtOptions;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> identityOptions,
            IOptions<JwtOptions> jwtOptions,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityOptions = identityOptions;
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
        }

       

        [AllowAnonymous]
        [HttpPost("~/api/auth/login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Ensure the username and password is valid.
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new
                {
                    error = "", //OpenIdConnectConstants.Errors.InvalidGrant,
                    error_description = "The username or password is invalid."
                });
            }

            // Ensure the email is confirmed.
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new
                {
                    error = "email_not_confirmed",
                    error_description = "You must have a confirmed email to log in."
                });
            }
            if (await _userManager.IsInRoleAsync(user, "disabled"))
            {
                return BadRequest(new
                {
                    error = "User Is Disabled",
                    error_description = "User Is Disabled"
                });
            }

            // Generate and issue a JWT token
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
              };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _jwtOptions.issuer,
              audience: _jwtOptions.issuer,
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        [HttpDelete("~/api/auth/{id}")]
        public async Task<IActionResult> Disable(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (await _userManager.IsInRoleAsync(user, "agent") && !await _userManager.IsInRoleAsync(user, "disabled"))
            {
                var otherresult = await _userManager.AddToRoleAsync(user, "disabled");

            }

            return Ok();
        }

        [HttpDelete("~/api/auth/{id}/enable")]
        public async Task<IActionResult> Enable(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (await _userManager.IsInRoleAsync(user, "agent") && await _userManager.IsInRoleAsync(user, "disabled"))
            {
                var otherresult = await _userManager.RemoveFromRoleAsync(user, "disabled");

            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("~/api/auth/register")]
        public async Task<IActionResult> Register([FromBody]NewUser model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.UserName, EmailConfirmed = true };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
                    //var otherresult = await _userManager.AddToRoleAsync(user, "agent");

                    return Ok();
                }
                else
                {
                    return BadRequest(new { general = result.Errors.Select(x => x.Description) });
                }
            }
            catch (Exception e)
            {
                int x = 0;
            }
            return null;
        }

     
    }
}
