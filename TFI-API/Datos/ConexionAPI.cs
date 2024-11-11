using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TFI_API.Negocio;

namespace TFI_API.Datos
{
    public class ConexionAPI
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        RestClient client;
        List<string> Categories;

        public ConexionAPI(string url)
        {
            client = new RestClient(url);
        }

        public ConexionAPI()
        {
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

                    // Loguear la salida del método
                    logger.Info("Metodo GetProducts finalizado correctamente. Lista de productos actualizada.");

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
                logger.Error(ex, "Excepcion en el metodo GetProducts");

                return "Error al obtener los productos";
            }
        }
        public string GetCategories(List<string> ListCategoriesToUpdate)
        {
            try
            {
                logger.Info($"Llamada al metodo GetCategories.");

                var request = new RestRequest("products/categories", Method.Get);
                var response = client.Get(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var categories = JsonConvert.DeserializeObject<List<string>>(response.Content);

                    ListCategoriesToUpdate.Clear();
                    ListCategoriesToUpdate.AddRange(categories);

                    logger.Info($"Categorias obtenidas correctamente. Categorias: {string.Join(", ", categories)}");
                    return "Categorias obtenidas correctamente";
                }
                else
                {
                    logger.Warn($"Error al obtener las categorías. Código de estado: {response.StatusCode}");
                    return "Error al obtener las categorías";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrio un error en el metodo GetCategories.");
                return "Error al obtener las categorias";
            }
        }
        public void GetInCategory(List<Producto> ListProductsToUpdate, string category)
        {
            try
            {
                logger.Info($"Llamada al metodo GetInCategory.");

                var request = new RestRequest($"products/categories/{category}", Method.Get);
                var response = client.Get(request);

                ListProductsToUpdate.RemoveAll(p => p.Category != category);
                logger.Info($"Productos filtrados por categoria {category}. Total productos despues de filtrar: {ListProductsToUpdate.Count}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrio un error en el metodo GetInCategory.");
            }
        }
        public List<Producto> LimitResult(List<Producto> ListProductsToUpdate, int limitNumber)
        {
            try
            {
                logger.Info($"Llamada al metodo LimitResult.");

                var request = new RestRequest($"products?limit={limitNumber}", Method.Get);
                var response = client.Get(request);

                logger.Info($"Productos limitados en {limitNumber}.");
                return ListProductsToUpdate.Take(limitNumber).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrio un error en el metodo LimitResult.");
                return new List<Producto>();
            }
        }
        public void SortResults(List<Producto> listProductsToUpdate, string order)
        {
            try
            {
                logger.Info($"Llamada al metodo SortResults.");

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
                    logger.Warn($"Error al ordenar los productos. Codigo de estado: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrio un error en el metodo SortResults.");
            }
        }
        public string PostProducts(List<Producto> listProductsToUpdate, Producto newProduct)
        {
            try
            {
                logger.Info("Llamada al método PostProducts.");

                // Configuración de la solicitud
                var request = new RestRequest("products", Method.Post);
                request.AddJsonBody(newProduct);

                // Realiza la solicitud POST
                var response = client.Post(request);

                // Verificación de la respuesta
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.Created)
                {
                    listProductsToUpdate.Add(newProduct);
                    logger.Info($"Producto '{newProduct.Title}' agregado correctamente.");
                    return "Producto agregado correctamente.";
                }
                else
                {
                    logger.Warn($"Error al agregar el producto '{newProduct.Title}'. Código de estado: {response.StatusCode}. Mensaje: {response.Content}");
                    return $"No se pudo agregar el producto. Código de estado: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ocurrió un error en el método PostProducts.");
                return "Ocurrió un error al agregar el producto.";
            }
        }
        public string DeleteProducts(List<Producto> listProductsToUpdate, List<int> listIds)
        {
            try
            {
                foreach (int productId in listIds)
                {
                    logger.Info("Ejecutando metodo DeleteProducts");
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
                        logger.Warn($"Fallo al eliminar productos. Codigo de estado: {response.StatusCode}");
                        return "Error al llamar a DeleteProducts";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en el metodo DeleteProducts");
                return "Error al eliminar el producto";
            }
        }
        public List<Producto> PutProducts(List<Producto> ListProductsToUpdate, Producto productToEdit)
        {
            var request = new RestRequest($"products/{productToEdit}", Method.Put);

            //ApiProducts? response = client.Put<ApiProducts>(request);

            var product = ListProductsToUpdate.Where(item => item.Id == productToEdit.Id).First();

            ListProductsToUpdate.Remove(product);
            ListProductsToUpdate.Add(productToEdit);

            return ListProductsToUpdate;
        }
    }
}

