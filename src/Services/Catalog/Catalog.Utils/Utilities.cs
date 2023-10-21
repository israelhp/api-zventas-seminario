using System.Net.Http.Headers;
using System.Net;
using Catalog.Domain;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Catalog.Utils
{
    public class Utilities
    {
        public static string urlAutenticate = "http://localhost:18839/identity/api/v1/users/auth";
        public static bool Auth(string token)
        {
            string apiUrl = urlAutenticate;

            string authToken = token;

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                try
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                        request.Method = "GET";
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string responseText = reader.ReadToEnd();
                                    AuthInfo responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthInfo>(responseText);
                                    return (bool)responseData.status;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                }
                catch (HttpRequestException ex)
                {
                    throw new Exception("Error en conexion al api IDENTITY.API");
                }

            }
        }

        public static AuthInfo AuthInfoGet(string token)
        {
            string apiUrl = urlAutenticate;

            string authToken = token;

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                try
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                        request.Method = "GET";
                        request.Headers.Add("Authorization", $"Bearer {authToken}");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string responseText = reader.ReadToEnd();
                                    AuthInfo responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthInfo>(responseText);
                                    return responseData;
                                }
                            }
                        }
                        else
                        {
                            return new AuthInfo
                            {
                                status = false,
                                username = "",
                                role = ""
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new AuthInfo
                        {
                            status = false,
                            username = "",
                            role = ""
                        };
                    }

                }
                catch (HttpRequestException ex)
                {
                    throw new Exception("Error en conexion al api IDENTITY.API");
                }

            }
        }

        public FilterDefinition<Product> createProductFilter(ProductFilters productFilters)
        {
            var builder = Builders<Product>.Filter;
            List<FilterDefinition<Product>> ListaFiltros = new List<FilterDefinition<Product>>();
            FilterDefinition<Product> filter = builder.Gte("price", productFilters.Price_gt);

            if (productFilters.search != null)
            {
                ListaFiltros.Add(builder.Or(
                    Builders<Product>.Filter.Regex(x => x.code, new BsonRegularExpression("/.*" + productFilters.search + ".*/i")),
                    Builders<Product>.Filter.Regex(x => x.description, new BsonRegularExpression("/.*" + productFilters.search + ".*/i")),
                    Builders<Product>.Filter.Regex(x => x.name, new BsonRegularExpression("/.*" + productFilters.search + ".*/i"))
                ));
            }
            if (productFilters.brand_eq != null)
            {
                ListaFiltros.Add(builder.Regex(x => x.brand.name, new BsonRegularExpression($"/^" + productFilters.brand_eq + "$/i")));
            }
            if (productFilters.category_eq != null)
            {
                ListaFiltros.Add(builder.Regex(x => x.category.name, new BsonRegularExpression($"/^" + productFilters.category_eq + "$/i")));
            }
            if (productFilters.Price_lt != 0)
            {
                ListaFiltros.Add(builder.Lte("price", productFilters.Price_lt));
            }

            foreach (FilterDefinition<Product> element in ListaFiltros)
            {
                filter = filter & element;
            }

            return filter;
        }
        public UpdateDefinition<Product> createProductUpdate(dynamic producto)
        {
            var builder = Builders<Product>.Update;
            var nameUpdates = new List<UpdateDefinition<Product>>();

            if (producto?.name != null)
            { nameUpdates.Add(builder.Set("name", producto.name)); }
            if (producto?.description != null)
            { nameUpdates.Add(builder.Set("description", producto.description)); }
            if (producto?.quantity != null)
            { nameUpdates.Add(builder.Set("quantity", producto.quantity)); }
            if (producto?.discount != null)
            { nameUpdates.Add(builder.Set("discount", producto.discount)); }
            if (producto?.price != null)
            { nameUpdates.Add(builder.Set("price", producto.price)); }
            if (producto?.tax != null)
            { nameUpdates.Add(builder.Set("tax", producto.tax)); }


            if (producto?.category != null)
            {
                if (producto?.category?.id != null) { nameUpdates.Add(builder.Set("category.id", producto.category.id)); }
                if (producto?.category?.name != null) { nameUpdates.Add(builder.Set("category.name", producto.category.name)); }
            }
            if (producto?.brand != null)
            {
                if (producto?.brand?.id != null) { nameUpdates.Add(builder.Set("brand.id", producto.brand.id)); }
                if (producto?.brand?.name != null) { nameUpdates.Add(builder.Set("brand.name", producto.brand.name)); }
            }
            if (producto?.image != null)
            {
                if (producto?.image?.id != null) { nameUpdates.Add(builder.Set("image.id", producto.image.id)); }
                if (producto?.image?.name != null) { nameUpdates.Add(builder.Set("image.name", producto.image.name)); }
                if (producto?.image?.extension != null) { nameUpdates.Add(builder.Set("image.extension", producto.image.extension)); }
                if (producto?.image?.url != null) { nameUpdates.Add(builder.Set("image.url", producto.image.url)); }
                if (producto?.image?.imageType != null) { nameUpdates.Add(builder.Set("image.imageType", producto.image.imageType)); }
            }

            if (producto?.carrete != null && producto?.carrete?.Count > 0)
            { nameUpdates.Add(builder.Set("carrete", JsonConvert.DeserializeObject<List<Image>>((producto.carrete).ToString()))); }

            UpdateDefinition<Product> Updates = builder.Combine(nameUpdates);
            return Updates;
        }

    }
}