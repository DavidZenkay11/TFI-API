using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFI_API.Negocio;

namespace TFI_API.Datos
{
    public class ConexionAPI
    {
        RestClient client;
        List<string> Categories;

        public ConexionAPI(string url)
        {
            client = new RestClient(url);
        }

        public List<Producto> GetProducts()
        {
            var request = new RestRequest("products", Method.Get);
            List<Producto> products = client.Get<List<Producto>>(request);
            return products;
        }
        public List<string> GetCategories()
        {
            var request = new RestRequest("products/categories", Method.Get);
            Categories = client.Get<List<string>>(request);
            return Categories;
        }
        public List<Producto> PostProducts(List<Producto> listProductsToUpdate, Producto nuevoProducto)
        {
            var request = new RestRequest("products", Method.Post);

            if (listProductsToUpdate == null)
            {
                listProductsToUpdate = new List<Producto>();
            }

            listProductsToUpdate.Add(nuevoProducto);

            return listProductsToUpdate;
        }
        public List<Producto> DeleteProducts(List<Producto> listProductsToUpdate, List<int> listIds)
        {
            foreach (int productId in listIds)
            {
                var request = new RestRequest($"products/{productId}", Method.Delete);
                Producto response = client.Delete<Producto>(request);
            }
            listProductsToUpdate.RemoveAll(item => listIds.Contains(item.Id));

            return listProductsToUpdate;
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

