using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        string url = ConfigurationManager.AppSettings["ApiUrl"];
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


    }
}
