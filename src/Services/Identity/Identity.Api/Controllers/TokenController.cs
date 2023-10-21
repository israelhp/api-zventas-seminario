using Identity.Database;
using Identity.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Identity.Api.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        [Route("token")]
        public IActionResult ObtenerToken([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
        {
            if (ValidarCredenciales(username, password, grant_type, out string role))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ClaveMuySeguraSeminario2023OctubreX"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim("username", username),
                };

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new {
                    access_token = tokenString,
                    token_type = "bearer",
                    expires_in = 86399
                });
            }
            else
            {
                return BadRequest(new
                {
                    error = "invalid_credentials",
                    error_description = "Usuario o contraseña incorrecta, por favor probar nuevamente."
                });
            }
        }

        private bool ValidarCredenciales(string username, string password, string grant_type, out string role)
        {
            role = "0";
            if (grant_type != "password")
            { return false;  }

            using (ApplicationDbContext con = new ApplicationDbContext())
            {
                User usuario = con.Collection.Find(a => a.username == username).FirstOrDefault();

                bool existe = usuario != null;

                if (!existe) { return false; }

                bool validPassword = BCrypt.Net.BCrypt.Verify(password, usuario.password);
                usuario.role = (usuario.role == null) ? "0" : usuario.role;

                if (validPassword)
                {
                    role = usuario.role;
                    return true;
                }
                return false;
            }

        }
    }
}
