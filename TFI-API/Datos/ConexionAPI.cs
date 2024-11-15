using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFI_API.Negocio;

namespace TFI_API.Datos
{
    public class ConexionAPI
    {
        string url = ConfigurationManager.AppSettings["ApiUrl"];
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        RestClient client;
        List<string> Categories;


        public ConexionAPI(string url)
        {
            client = new RestClient(url);
        }

        public string GetProducts(List<Producto> listProductsToUpdate)
        {
            try
            {
                var request = new RestRequest("products", Method.Get);
                var response = client.Get(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var products = JsonConvert.DeserializeObject<List<Producto>>(response.Content);

                    logger.Info($"Productos obtenidos: {products.Count}");

                    listProductsToUpdate.Clear();
                    listProductsToUpdate.AddRange(products);

                    logger.Info("Método GetProducts finalizado correctamente. Lista de productos actualizada.");

                    return "Conexión Exitosa";
                }
                else
                {
                    logger.Error($"Error al obtener los productos. StatusCode: {response.StatusCode}");

                    return "Error al obtener los productos";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió una excepción en el método GetProducts");

                return "Error al obtener los productos";
            }
        }
        public string GetCategories(List<string> ListCategoriesToUpdate)
        {
            try
            {
                logger.Info($"Llamada al método GetCategories.");

                var request = new RestRequest("products/categories", Method.Get);
                var response = client.Get(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var categories = JsonConvert.DeserializeObject<List<string>>(response.Content);

                    ListCategoriesToUpdate.Clear();
                    ListCategoriesToUpdate.AddRange(categories);

                    logger.Info($"Categorías obtenidas correctamente. Categorías: {string.Join(", ", categories)}");
                    return "Categorías obtenidas correctamente";
                }
                else
                {
                    logger.Warn($"Error al obtener las categorías. Código de estado: {response.StatusCode}");
                    return "Error al obtener las categorías";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió un error en el método GetCategories.");
                return "Ocurrió un error al obtener las categorías";
            }
        }
        public void GetInCategory(List<Producto> ListProductsToUpdate, string category)
        {
            try
            {
                logger.Info($"Llamada al método GetInCategory.");

                var request = new RestRequest($"products/categories/{category}", Method.Get);
                var response = client.Get(request);

                ListProductsToUpdate.RemoveAll(p => p.Category != category);
                logger.Info($"Productos filtrados por categoría {category}. Total productos después de filtrar: {ListProductsToUpdate.Count}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió un error en el método GetInCategory.");
            }
        }
        public List<Producto> LimitProduct(List<Producto> ListProductsToUpdate, int limitNumber)
        {
            try
            {
                logger.Info($"Llamada al método LimitProduct.");

                var request = new RestRequest($"products?limit={limitNumber}", Method.Get);
                var response = client.Get(request);

                logger.Info($"Productos limitados en {limitNumber}.");
                return ListProductsToUpdate.Take(limitNumber).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió un error en el método LimitProduct.");
                return new List<Producto>();
            }
        }
        public void SortResults(List<Producto> listProductsToUpdate, string order)
        {
            try
            {
                logger.Info($"Llamada al método SortResults.");

                var request = new RestRequest("products/products?sort=desc", Method.Get);
                var response = client.Get(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (order == "Ascendente")
                    {
                        listProductsToUpdate.Sort((p1, p2) => p1.Id.CompareTo(p2.Id));
                        logger.Info($"Productos ordenados de forma ascendente");
                    }
                    else
                    {
                        listProductsToUpdate.Sort((p1, p2) => p2.Id.CompareTo(p1.Id));
                        logger.Info($"Productos ordenados de forma descendente");
                    }
                }
                else
                {
                    logger.Warn($"Error al ordenar los productos. Código de estado: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió un error en el método SortResults.");
            }
        }
        public Producto PostProducts( Producto productoNuevo, string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products", Method.Post);
                request.AddJsonBody(productoNuevo);
                var response = client.Execute<Producto>(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Error: {response.ErrorMessage}");
                }
                return productoNuevo;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en el método PostProducts.", url, productoNuevo);
                return null;
            }
        }
        public string DeleteProducts(List<Producto> listProductsToUpdate, List<int> listIds)
        {
            try
            {
                foreach (int productId in listIds)
                {
                    logger.Info("Ejecutando método DeleteProducts");
                    var request = new RestRequest($"products/{productId}", Method.Delete);
                    var response = client.Delete(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        listProductsToUpdate.RemoveAll(item => listIds.Contains(item.Id));
                        logger.Info($"Producto(s) eliminado(s) correctamente: {listIds.Count}");
                        return "Productos eliminados correctamente";
                    }
                    else
                    {
                        logger.Warn($"Fallo al eliminar productos. Código de estado: {response.StatusCode}");
                        return "Error al llamar a DeleteProducts";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en el método DeleteProducts");
                return "Error al eliminar el producto";
            }
        }
        public List<Producto> PutProducts(List<Producto> ListProductsToUpdate, Producto productToEdit)
        {
            try
            {
                var request = new RestRequest($"products/{productToEdit.Id}", Method.Put);


                request.AddJsonBody(productToEdit);

                var client = new RestClient(url);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Error: {response.ErrorMessage}");
                }

                var product = ListProductsToUpdate.Where(item => item.Id == productToEdit.Id).FirstOrDefault();
                if (product != null)
                {
                    ListProductsToUpdate.Remove(product);
                    ListProductsToUpdate.Add(productToEdit);
                }

                return ListProductsToUpdate;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en el método PutProducts.", url, productToEdit);
                return ListProductsToUpdate;
            }
        }

    }
}

