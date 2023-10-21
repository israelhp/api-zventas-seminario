using Catalog.Database;
using Catalog.Domain;
using Catalog.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;

namespace Catalog.Behaviour
{
    public class ProductBehaviour : Utilities
    {
        public Response Insert(Product producto)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                bool exist = (app.Collection.Find(x => x.code == producto.code).FirstOrDefault() != null);

                if (exist)
                {
                    return new Response() { code = HttpStatusCode.BadRequest, message = "Codigo de producto ya en uso" };
                }
                producto._id = ObjectId.GenerateNewId();
                app.Collection.InsertOne(producto);
                return new Response() { code = HttpStatusCode.OK, message = "Producto ingresado con exito" };
            }
        }

        public Response GetByCode(string code)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {

                Product producto = app.Collection.Find(x => x.code == code).SortBy(x => x.code).FirstOrDefault();

                return new Response() { code = HttpStatusCode.OK, message = (producto == null) ? "No se encontraron registros." : "Consulta realizada con exito, total de registros: 1", data = producto };
            }
        }

        public Response Get(ProductFilters filtros)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                List<Product> Listado;

                if (filtros?.filter == false || filtros?.filter == null)
                {
                    Listado = app.Collection.Find(x => true).ToList();
                }
                else
                {
                    var filter = createProductFilter(filtros);
                    if (filtros.ItemsPerPage != 0 && filtros.page != 0)
                    {
                        Listado = app.Collection.Find(filter).Skip((filtros.page - 1) * filtros.ItemsPerPage).Limit(filtros.ItemsPerPage).SortBy(x => x.code).ToList();
                    }
                    else
                    {
                        Listado = app.Collection.Find(filter).SortBy(x => x.code).ToList();
                    }

                }

                return new Response() { code = HttpStatusCode.OK, message = (Listado.Count == 0) ? "No se encontraron registros." : "Consulta realizada con exito, total de registros: " + Listado.Count, data = Listado };
            }

        }

        public Response Update(dynamic producto, string code)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                string codigo = code;
                if (codigo == null) { return new Response() { code = HttpStatusCode.BadRequest, message = "Codigo no definido en el objeto" }; }
                bool exist = (app.Collection.Find(x => x.code == codigo).FirstOrDefault() != null);

                var filter = Builders<Product>
                   .Filter
                   .Eq("code", code);

                var update = createProductUpdate(producto);

                if (!exist)
                {
                    return new Response() { code = HttpStatusCode.BadRequest, message = "Producto no existe." };
                }
                else
                {
                    var updateResult = app.Collection.UpdateOne(filter, update);
                    return new Response() { code = HttpStatusCode.OK, message = "Registros actualizados con exito: " + updateResult.ModifiedCount };
                }
            }
        }

        public Response Delete(string code)
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                bool exist = (app.Collection.Find(x => x.code == code).FirstOrDefault() != null);

                if (!exist)
                {
                    return new Response() { code = HttpStatusCode.BadRequest, message = "Producto no registrado." };
                }
                else
                {
                    app.Collection.DeleteOne(x => x.code == code);
                    return new Response() { code = HttpStatusCode.OK, message = "Producto eliminado con exito" };
                }
            }
        }

        public Response Categories()
        {
            using (ApplicationDbContext app = new ApplicationDbContext())
            {
                var filter = Builders<Product>.Filter.Empty;

                var projection = Builders<Product>.Projection.Exclude("_id").Include("category.name");

                var Listado = app.Collection.Find(filter).Project(projection).ToList();

                var valoresUnicos = Listado
                    .Select(doc => doc["category"]["name"].AsString)
                    .Where(value => !string.IsNullOrEmpty(value))
                    .Distinct()
                    .ToList();

                return new Response() { code = HttpStatusCode.OK, message = (valoresUnicos.Count == 0) ? "No se encontraron registros." : "Consulta realizada con exito, total de registros: " + valoresUnicos.Count, data = valoresUnicos };
            }

        }
    }
}