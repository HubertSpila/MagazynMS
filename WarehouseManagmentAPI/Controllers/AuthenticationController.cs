using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseManagmentAPI.Controllers.PostModels;
using WarehouseManagmentAPI.Database.DatabaseControllers;

namespace WarehouseManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        //Pobiera informacje z pliku appsettings.json do _configuration
        private IConfiguration _configuration { get; set; }
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        public ActionResult<string> Authenticate(AuthenticationPostModel form)
        {
            //Sprawdzenie poprawności danych logowania
            if(!UserDbC.IsOkUser(form)) return Unauthorized();

            //Wygenerowanie tokena jeśli user jest poprawny
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Wpisanie do tokena UserName
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", form.UserName));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            
            return Ok(tokenToReturn);
        }
    }
}
