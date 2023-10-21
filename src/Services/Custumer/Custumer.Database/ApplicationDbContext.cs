using Custumer.Domain;
using MongoDB.Driver;

namespace Custumer.Database
{
    public class ApplicationDbContext: IDisposable
    {
        //public static string mongoUri = "mongodb+srv://root:seminario123@cluster0.5itlf.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoUri = "mongodb+srv://rajuarezy:$RIISEM2023@riisem.u1ccpp0.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoDataBase = "Riisem_Identity";
        public static string mongoCollection = "users";
        public static MongoClient client = new MongoClient(mongoUri);
        public static IMongoDatabase database = client.GetDatabase(mongoDataBase);
        public IMongoCollection<User> Collection = database.GetCollection<User>(mongoCollection);
        public ApplicationDbContext() { }
        public void Dispose() { }
    }
}