using MongoDB.Bson;
using MongoDB.Driver;
using Order.Database;
using Order.Domain;
using Order.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Order.Behaviour
{
    public class OrderBehaviour : Utilities
    {
        public Response Insert(Domain.Order orden, string username)
        {
            try
            {
                using (ApplicationDbContext app = new ApplicationDbContext())
                {
                    using (var session = app.Collection.Database.Client.StartSession())
                    {
                        try
                        {
                            var ProductsCollection = app.Collection.Database.Client.GetDatabase("Riisem_Catalog").GetCollection<BsonDocument>("products");
                            session.StartTransaction();

                            foreach (OrderLine linea in orden.lines)
                            {
                                var filterP = Builders<BsonDocument>.Filter.Eq("code", linea.code);
                                BsonDocument producto = ProductsCollection.Find(filterP).FirstOrDefault();

                                var NegativeInventory = producto.GetValue("quantity").AsDouble < linea.quantity;

                                if (NegativeInventory) { throw new Exception("El inventario recae en un inventario negativo para el articulo: " + linea.code + " Cantidad disponible: " + producto.GetValue("quantity").AsDouble); }

                                var filterProduct = Builders<BsonDocument>
                                  .Filter
                                .Eq("code", linea.code);

                                var updateProduct = Builders<BsonDocument>
                                    .Update
                                    .Inc("quantity", -linea.quantity);

                                ProductsCollection.UpdateOne(session, filterProduct, updateProduct);
                            }
                            orden._id = ObjectId.GenerateNewId();
                            orden.userId = username;
                            orden.date = DateTime.Now;
                            app.Collection.InsertOne(session, orden);
                            session.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            session.AbortTransaction();
                            return new Response() { code = HttpStatusCode.OK, message = ex.Message, data = ex };
                        }
                    }
                    return new Response() { code = HttpStatusCode.OK, message = "Orden ingresada con exito" };
                }
            }
            catch (Exception ex)
            {
                return new Response() { code = HttpStatusCode.OK, message = ex.Message, data = ex };
            }

        }

        public Response get(string username)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                var Listado = app.Collection.Find(x => x.userId == username).SortByDescending(x => x.date).ToList();
                return new Response() { code = HttpStatusCode.OK, message = (Listado.Count == 0) ? "No se encontraron registros." : "Consulta realizada con exito, total de registros: " + Listado.Count, data = Listado };
            }
        }

        public Response cancel(string orden, string username)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                var ordenObj = app.Collection.Find(x => x._id == new MongoDB.Bson.ObjectId(orden)).FirstOrDefault();
                bool valida = (ordenObj.userId == username);
                if (!valida) { return new Response() { code = HttpStatusCode.BadRequest, message = "Orden no pertenece a este usuario" }; }
                if (ordenObj.status == "C") { return new Response() { code = HttpStatusCode.BadRequest, message = "Esta orden ya habia sido cancelada" }; }
                var ProductsCollection = app.Collection.Database.Client.GetDatabase("Riisem_Catalog").GetCollection<BsonDocument>("products");

                using (var session = app.Collection.Database.Client.StartSession())
                {
                    try
                    {
                        session.StartTransaction();

                        foreach (OrderLine linea in ordenObj.lines)
                        {
                            var filterP = Builders<BsonDocument>.Filter.Eq("code", linea.code);
                            var producto = ProductsCollection.Find(filterP).FirstOrDefault();

                            var filterProduct = Builders<BsonDocument>
                              .Filter
                            .Eq("code", linea.code);

                            var updateProduct = Builders<BsonDocument>
                                .Update
                                .Inc("quantity", linea.quantity);

                            ProductsCollection.UpdateOne(session, filterProduct, updateProduct);
                        }


                        var filter = Builders<Domain.Order>
                           .Filter
                           .Eq(x => x._id, new ObjectId(orden));
                        var update = Builders<Domain.Order>
                            .Update
                            .Set(x => x.status, "C");

                        var updated = app.Collection.UpdateOne(filter, update);
                        session.CommitTransaction();
                        return new Response() { code = HttpStatusCode.OK, message = "Orden cancelada con exito, registros cancelados: " + updated.ModifiedCount };
                    }
                    catch (Exception ex)
                    {
                        session.AbortTransaction();
                        return new Response() { code = HttpStatusCode.OK, message = ex.Message, data = ex };
                    }
                }
            }
        }
    }
}
