using Custumer.Behaviour;
using Custumer.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;

namespace Custumer.Api.Controllers
{
    [Route("[controller]/api/v1/users")]
    [ApiController]
    public class CustumerController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;

        public CustumerController()
        {
            _logger = Log.ForContext<CustumerController>();
        }

        [HttpPost]
        public Response InsertUser([FromBody] dynamic datos)
        {
            var user = JsonConvert.DeserializeObject<User>(datos.ToString());
            _logger.Information("Custumer.Api.users -- insert 1(s) user to users collection...");
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.Insert(user);
        }

        [HttpPost]
        [Route("ResetPasswordRequest")]
        public Response PasswordResetRequest([FromBody] dynamic datos)
        {
            var usuario = JsonConvert.DeserializeObject<User>(datos.ToString());
            _logger.Information($"Custumer.Api.users -- requested a reset password for {usuario.username} user from databse riisem_users...");
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.ResetRequest(usuario);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public Response PasswordReset([FromBody] dynamic datos)
        {
            var usuario = JsonConvert.DeserializeObject<User>(datos.ToString());
            _logger.Information($"Custumer.Api.users -- reset password done for {usuario.username} user from databse riisem_users...");
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.Reset(usuario);
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public Response ChangePassword([FromBody] dynamic passwords)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(passwords.ToString());

            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            _logger.Information($"Custumer.Api.users -- change password done for {username} user from databse riisem_users...");
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.changePassword(json, username);
        }

        [Authorize]
        [HttpDelete]
        public Response DeleteUser()
        {
            _logger.Information($"Custumer.Api.users -- delete 1(s) user from users collections...");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.delete(username);
        }

        [Authorize]
        [HttpGet]
        public Response InfoUser()
        {
            _logger.Information($"Custumer.Api.users -- get info from 1(s) user in users collections...");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.info(username);
        }

        [Authorize]
        [HttpGet]
        [Route("auth")]
        public dynamic Auth()
        {
            _logger.Information($"Custumer.Api.users -- Auth process...");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();
            var role = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value).FirstOrDefault();
            return
                 new
                 {
                     status = User.Identity.IsAuthenticated,
                     username = username,
                     role = role
                 };
        }


        [Authorize]
        [HttpPut]
        [Route("profile")]
        public Response UpdateProfile([FromBody] dynamic profile)
        {
            _logger.Information($"Custumer.Api.users -- updated 1 profile info from 1(s) user in users collections..");
            var identity = (ClaimsIdentity)User.Identity;
            var username = identity.Claims
                        .Where(c => c.Type == "username")
                        .Select(c => c.Value).FirstOrDefault();

            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.UpdateProfile(profile, username);
        }
        [Authorize(Roles = "1,admin")]
        [HttpPost]
        [Route("SaveEmailRIISEM")]
        public Response SaveEmail([FromBody] Mail email)
        {
            _logger.Information($"Custumer.Api.users -- Save an email for riisem company..");
            CustumerBehaviour pivot = new CustumerBehaviour();
            return pivot.saveEmail(email);
        }

    }
}
