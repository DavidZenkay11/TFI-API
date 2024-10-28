using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFI_API.Negocio
{
    public class Producto
    {
        public Producto()
        {
        }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public Producto(string title, decimal price, string description, string category)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        //GET PRODUCTOS
        public List<Producto> GetProductos(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products", Method.Get);
                List<Producto> productos = client.Get<List<Producto>>(request);

                return productos;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener productos desde la URL: {url}", url); // Loguea el error
                return new List<Producto>(); // Retorna una lista vacía en caso de error
            }
        }


        //METODO GET BY ID

        public Producto GetProductoByID(string url, int productId)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products/{productId}", Method.Get);
                Producto producto = client.Get<Producto>(request);
                return producto;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener el producto con ID: {productId} desde la URL: {url}", productId, url);
                return null;

            }
        }





        // METODO POST

        public Producto PostProducto(Producto nuevoProducto, string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products", Method.Post);
                request.AddJsonBody(nuevoProducto);

                // Ejecutar la solicitud y obtener la respuesta
                var response = client.Execute<Producto>(request);

                // Retorna el producto creado o null si la respuesta no fue exitosa
                return response.Data; // Esto podría ser null si la respuesta no fue exitosa
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al crear el producto: {producto}", nuevoProducto); // Loguea el error
                return null; // Retorna null en caso de error
            }
        }

        // METODO PUT

        public string ModificarProducto(Producto producto, string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products/{producto.Id}", Method.Put);

                request.AddJsonBody(producto); // Agrega el producto como JSON en el cuerpo de la solicitud

                // Ejecutar la solicitud y obtener la respuesta
                var response = client.Execute<Producto>(request);

                // Retorna un mensaje de éxito o el mensaje de error
                return response.IsSuccessful ? "Producto modificado exitosamente." : response.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al modificar el producto: {producto}", producto); // Loguea el error
                return "Ocurrió un error al modificar el producto."; // Mensaje genérico en caso de error

            }
        }




        //METODO DELETE
        public string EliminarProducto(Producto producto, string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products/{producto.Id}", Method.Delete);

                // Ejecutar la solicitud y obtener la respuesta
                var response = client.Execute(request);

                // Retornar un mensaje basado en el resultado de la solicitud
                return response.IsSuccessful ? "Producto eliminado exitosamente." : throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al eliminar el producto: {productoId}", producto.Id); // Loguea el error
                return $"Ocurrió un error al intentar eliminar el producto: {ex.Message}"; // Mensaje de error
            }
        }


        // METODO GET ALL CATEGORY


        public List<string> GetCategorias(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products/categories", Method.Get);

                // Llamada a la API para obtener las categorías
                List<string> categorias = client.Get<List<string>>(request);
                return categorias;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener categorías"); // Loguea el error
                return null; // Devolver null en caso de erro
            }
        }


        // METODO GET IN CATEGORY

        public List<Producto> GetProductosPorCategoria(string url, string categoria)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products/category/{categoria}", Method.Get);

                // Llamada a la API para obtener productos por categoría
                List<Producto> productos = client.Get<List<Producto>>(request);
                return productos;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al obtener productos por categoría: {categoria}"); // Loguea el error
                return null; // Devolver null en caso de error
            }
        }

        //METODO SORT RESULT DESCENDENTE

        public List<Producto> ObtenerProductosOrdenadosDesc(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products?sort=desc", Method.Get);
                var response = client.Execute<List<Producto>>(request);

                // Devolver lista de productos si la solicitud fue exitosa
                return response.Data;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener productos ordenados de manera descendente"); // Loguea la excepción
                return null; // Devolver null en caso de error
            }
        }

        //METODO SORT RESULT ASCENDENTE

        public List<Producto> ObtenerProductosOrdenadosAsc(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products?sort=asc", Method.Get);
                var response = client.Execute<List<Producto>>(request);

                // Devolver lista de productos si la solicitud fue exitosa
                return response.Data;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener productos ordenados de manera ascendente"); // Loguea la excepción
                return null;  // Devolver null en caso de error
            }
        }

        // METODO LIMIT RESULT
        public List<Producto> GetProductosConLimite(string url, int limite)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products?limit={limite}", Method.Get);

                // Ejecutar la solicitud y obtener la respuesta
                List<Producto> productos = client.Get<List<Producto>>(request);

                return productos;  // Devolver lista de productos
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener productos con límite"); // Loguea la excepción
                return null;  // Devolver null en caso de error
            }
        }
    }
}
