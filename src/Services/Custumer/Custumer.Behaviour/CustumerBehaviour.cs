using Custumer.Database;
using Custumer.Domain;
using Custumer.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;

namespace Custumer.Behaviour;

public class CustumerBehaviour : Utilities
{
    private static Random random = new Random();

    public Response Insert(User usuario)
    {

        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == usuario.username).FirstOrDefault()?.username != null);
            bool passwordEmpty = usuario?.password == null || usuario?.password == "" || usuario?.password.Length < 5;
            if (exist)
            {
                return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario ya en uso" };
            }
            if (passwordEmpty == true)
            {
                return new Response() { code = HttpStatusCode.BadRequest, message = "Longitud de contraseña invalida, longitud minima 5 caracteres" };
            }

            var user = new User
            {
                _id = MongoDB.Bson.ObjectId.GenerateNewId(),
                ID = Guid.NewGuid().ToString().ToUpper(),
                username = usuario.username,
                email = usuario.email,
                password = BCrypt.Net.BCrypt.HashPassword(usuario.password),
                role = usuario.role
            };

            con.Collection.InsertOne(user);
            return new Response() { code = HttpStatusCode.OK, message = "Usuario ingresado con exito" };
        }



    }

    public Response changePassword(dynamic passwords, string username)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == username).FirstOrDefault()?.username != null);
            if (!exist) { return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no encontrado" }; }

            User user = con.Collection.Find(x => x.username == username).FirstOrDefault();
            string oldPassword = passwords?.oldPassword;
            string newPassword = passwords?.newPassword;
            if (oldPassword == null || newPassword == null) { return new Response() { code = HttpStatusCode.BadRequest, message = "Formato de peticion mal definido" }; }

            bool login = BCrypt.Net.BCrypt.Verify(oldPassword, user.password);
            bool passwordEmpty = newPassword == null || newPassword == "" || newPassword.Length < 5;
            if (login == false) { return new Response() { code = HttpStatusCode.BadRequest, message = "Contraseña original incorrecta." }; }
            if (oldPassword == newPassword) { return new Response() { code = HttpStatusCode.BadRequest, message = "La nueva contraseña no puede ser igual a la actual." }; }
            if (passwordEmpty == true) { return new Response() { code = HttpStatusCode.BadRequest, message = "Longitud de contraseña invalida, longitud minima 5 caracteres" }; }

            string password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var filter = Builders<User>
               .Filter
               .Eq(x => x.username, username);
            var resetPassword = Builders<User>
                .Update
                .Set(x => x.password, password);
            var updatePassword = con.Collection.UpdateOne(filter, resetPassword);
            if (!updatePassword.IsAcknowledged)
            {
                return new Response() { code = HttpStatusCode.InternalServerError, message = "Ocurrio un error al actualizar la contraseña, por favor intentar de nuevo." };
            }
            else
            {
                return new Response() { code = HttpStatusCode.OK, message = "Cambio de contraseña exitoso" };
            }
        }
    }

    public Response info(string username)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == username).FirstOrDefault()?.username != null);

            if (!exist) { return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no registrado en el sistema" }; }

            User user = con.Collection.Find(x => x.username == username).FirstOrDefault();
            user.password = null;

            return new Response() { code = HttpStatusCode.OK, message = "Usuario solicitado con exito.", data = user };
        }
    }

    public Response delete(string username)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == username).FirstOrDefault()?.username != null);
            if (!exist) { return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no registrado en el sistema" }; }

            var filter = Builders<User>
              .Filter
              .Eq(x => x.username, username);

            var delete = con.Collection.DeleteOne(filter);
            return new Response() { code = HttpStatusCode.OK, message = "Usuarios eliminados con exito. Count: " + delete.DeletedCount };
        }
    }

    public Response ResetRequest(User usuario)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == usuario.username).FirstOrDefault()?.username != null);

            if (!exist)
            {
                return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no encontrado" };
            }
            User user = con.Collection.Find(x => x.username == usuario.username).FirstOrDefault();

            var filter = Builders<User>
                .Filter
                .Eq(x => x.username, usuario.username);

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string codigo = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var values = Builders<User>
                .Update
                .Set(x => x.resetCode, codigo);

            var update = con.Collection.UpdateOne(filter, values);

            if (!update.IsAcknowledged)
            {
                return new Response() { code = HttpStatusCode.InternalServerError, message = "Ocurrio un error al generar el codigo de recuperacion" };
            }

            if (sendMail(user.email, codigo))
            {
                return new Response() { code = HttpStatusCode.OK, message = "Correo enviado con exito" };
            }
            else
            {
                return new Response() { code = HttpStatusCode.InternalServerError, message = "Error al enviar correo" };
            }
        }
    }

    public Response Reset(User usuario)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == usuario.username).FirstOrDefault()?.username != null);

            if (!exist)
            {
                return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no encontrado" };
            }
            User user = con.Collection.Find(x => x.username == usuario.username).FirstOrDefault();

            bool ValidCode = user.resetCode == usuario.resetCode;

            var filter = Builders<User>
                .Filter
                .Eq(x => x.username, usuario.username);

            string codigo = null;

            var values = Builders<User>
                .Update
                .Set(x => x.resetCode, codigo);

            var resetPassword = Builders<User>
                .Update
                .Set(x => x.password, BCrypt.Net.BCrypt.HashPassword(usuario.password));

            var update = con.Collection.UpdateOne(filter, values);

            if (!update.IsAcknowledged)
            {
                return new Response() { code = HttpStatusCode.InternalServerError, message = "Ocurrio un error al actualizar la contraseña, por favor generar nuevo codigo e intentar de nuevo." };
            }
            else
            {
                if (!ValidCode)
                {
                    return new Response() { code = HttpStatusCode.InternalServerError, message = "Codigo de verificacion invalido." };
                }
                var updatePassword = con.Collection.UpdateOne(filter, resetPassword);
                if (!update.IsAcknowledged)
                {
                    return new Response() { code = HttpStatusCode.InternalServerError, message = "Ocurrio un error al actualizar la contraseña, por favor generar nuevo codigo e intentar de nuevo." };
                }
                else
                {
                    return new Response() { code = HttpStatusCode.OK, message = "Restablecimiento de contraseña exitoso" };
                }
            }
        }

    }

    public Response UpdateProfile(dynamic usuario, string username)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            bool exist = (con.Collection.Find(x => x.username == username).FirstOrDefault()?.username != null);

            if (!exist) { return new Response() { code = HttpStatusCode.BadRequest, message = "Usuario no encontrado" }; }
            User user = con.Collection.Find(x => x.username == username).FirstOrDefault();

            var filter = Builders<User>
                .Filter
                .Eq(x => x.username, username);

            var values = createProfileUpdate(usuario);


            //  var values = Builders<User>.Update.Set("username", user.username);

            var update = con.Collection.UpdateOne(filter, values);


            if (!update.IsAcknowledged)
            {
                return new Response() { code = HttpStatusCode.InternalServerError, message = "Ocurrio un error al actualizar la contraseña, por favor generar nuevo codigo e intentar de nuevo." };
            }
            else
            {
                return new Response() { code = HttpStatusCode.OK, message = "Actualizacion de perfil exitoso: " + update.ModifiedCount };
            }
        }
    }

    public Response saveEmail(Mail email)
    {
        using (ApplicationDbContext con = new ApplicationDbContext())
        {
            var Collection = con.Collection.Database.Client.GetDatabase("Riisem_email").GetCollection<Mail>("mail");
            email._id = ObjectId.GenerateNewId();
            Collection.InsertOne(email);
            return new Response() { code = HttpStatusCode.OK, message = "email ingresado con exito" };
        }
        
    }
}
