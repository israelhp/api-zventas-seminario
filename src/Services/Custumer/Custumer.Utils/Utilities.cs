using EASendMail;
using Custumer.Domain;
using MongoDB.Driver;
using System.Net.Mail;
using Custumer.Database;

namespace Custumer.Utils
{
    public class Utilities
    {
        public bool sendMail(string recipient, string code)
        {
            try
            {

                var mongoUri = ApplicationDbContext.mongoUri;
                var client = new MongoClient(mongoUri);
                var database = client.GetDatabase("Riisem_email");
                var Collection = database.GetCollection<Mail>("mail");
                bool exist = (Collection.Find(x => true).FirstOrDefault()?.email != null);

                if (!exist)
                {
                    return false;
                }
                Mail email = Collection.Find(x => true).FirstOrDefault();

                SmtpMail oMail = new SmtpMail("TryIt");
                oMail.From = email.email;
                oMail.To = recipient;
                oMail.Subject = "RESTABLECIMIENTO DE CONTRASEÑA";
                oMail.HtmlBody = @" <div class="" container"">
                                        <H1>RESTABLECIMIENTO DE CONTRASEÑA</H1>
                                        <H2>Codigo de seguridad </H2>
                                        <h2>Usa el siguiente código de seguridad para la cuenta de " + recipient + @"</h2>
                                        <h2>Codigo de seguridad: " + code + @"</h2>
                                        <p>Si no ha solicitado un reestablecimiento de contraseña, por favor reportelo con su proveedor</p>
                                        <p>Gracias</p>
                                        <p>Equipo de cuentas RIISEM</p>
                                        <p>© 2020 - 2028 RIISEM</p>
                                    </div>
                                    <style>
                                        .container { border: solid;  padding: 10px; font-family: Arial, Helvetica, sans-serif; background-color: rgb(215, 215, 215); }
                                        h1 { color: rgb(0, 0, 0);}
                                        h2 { color: rgb(39, 39, 39); }
                                    </style>";

                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                oServer.User = email.email;
                oServer.Password = email.password;
                oServer.Port = 465;
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                oSmtp.SendMail(oServer, oMail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public UpdateDefinition<User> createProfileUpdate(dynamic user)
        {

            var builder = Builders<User>.Update;
            var nameUpdates = new List<UpdateDefinition<User>>();

            if (user?.profile?.name != null)
            {
                if (user?.profile?.name?.firstName != null) { nameUpdates.Add(builder.Set("profile.name.firstName", user.profile.name.firstName)); }
                if (user?.profile?.name?.middleName != null) { nameUpdates.Add(builder.Set("profile.name.middleName", user.profile.name.middleName)); }
                if (user?.profile?.name?.lastName != null) { nameUpdates.Add(builder.Set("profile.name.lastName", user.profile.name.lastName)); }
                if (user?.profile?.name?.lastName2 != null) { nameUpdates.Add(builder.Set("profile.name.lastName2", user.profile.name.lastName2)); }
            }

            if (user?.profile?.nit != null) { nameUpdates.Add(builder.Set("profile.nit", user.profile.nit)); }

            if (user?.profile?.image != null)
            {
                if (user?.profile?.image?.id != null) { nameUpdates.Add(builder.Set("profile.image.id", user.profile.image.id)); }
                if (user?.profile?.image?.name != null) { nameUpdates.Add(builder.Set("profile.image.name", user.profile.image.name)); }
                if (user?.profile?.image?.extension != null) { nameUpdates.Add(builder.Set("profile.image.extension", user.profile.image.extension)); }
                if (user?.profile?.image?.url != null) { nameUpdates.Add(builder.Set("profile.image.url", user.profile.image.url)); }
                if (user?.profile?.image?.imageType != null) { nameUpdates.Add(builder.Set("profile.image.imageType", user.profile.image.imageType)); }
            }

            if (user?.profile?.address != null)
            {
                if (user?.profile?.address?.address != null) { nameUpdates.Add(builder.Set("profile.address.address", user.profile.address.address)); }
                if (user?.profile?.address?.complement != null) { nameUpdates.Add(builder.Set("profile.address.complement", user.profile.address.complement)); }
                if (user?.profile?.address?.country != null) { nameUpdates.Add(builder.Set("profile.address.country", user.profile.address.country)); }
                if (user?.profile?.address?.county != null) { nameUpdates.Add(builder.Set("profile.address.county", user.profile.address.county)); }
                if (user?.profile?.address?.city != null) { nameUpdates.Add(builder.Set("profile.address.city", user.profile.address.city)); }
                if (user?.profile?.address?.postalCode != null) { nameUpdates.Add(builder.Set("profile.address.postalCode", user.profile.address.postalCode)); }
            }
            UpdateDefinition<User> Updates = builder.Combine(nameUpdates);
            return Updates;
        }
    }
}