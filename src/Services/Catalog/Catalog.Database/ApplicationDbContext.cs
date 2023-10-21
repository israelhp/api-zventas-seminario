using Catalog.Domain;
using MongoDB.Driver;


namespace Catalog.Database
{
    public class ApplicationDbContext : IDisposable
    {
        //public static string mongoUri = "mongodb+srv://root:seminario123@cluster0.5itlf.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoUri = "mongodb+srv://rajuarezy:$RIISEM2023@riisem.u1ccpp0.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoDataBase = "Riisem_Catalog";
        public static string mongoCollection = "products";
        public static MongoClient client = new MongoClient(mongoUri);
        public static IMongoDatabase database = client.GetDatabase(mongoDataBase);
        public IMongoCollection<Product> Collection = database.GetCollection<Product>(mongoCollection);
        public ApplicationDbContext() { }
        public void Dispose() { }
    }
}