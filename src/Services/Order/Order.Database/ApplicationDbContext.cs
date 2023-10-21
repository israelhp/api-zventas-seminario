using MongoDB.Driver;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Database
{
    public class ApplicationDbContext : IDisposable
    {
        //public static string mongoUri = "mongodb+srv://root:seminario123@cluster0.5itlf.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoUri = "mongodb+srv://rajuarezy:$RIISEM2023@riisem.u1ccpp0.mongodb.net/?retryWrites=true&w=majority";
        public static string mongoDataBase = "Riisem_Order";
        public static string mongoCollection = "orders";
        public static MongoClient client = new MongoClient(mongoUri);
        public static IMongoDatabase database = client.GetDatabase(mongoDataBase);
        public IMongoCollection<Order.Domain.Order> Collection = database.GetCollection<Order.Domain.Order>(mongoCollection);
        public ApplicationDbContext() { }
        public void Dispose() { }
    }
}
