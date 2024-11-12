using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFI_API.Datos;
using TFI_API.Negocio;

namespace TFI_API.Presentación
{
    public partial class FormEditar : Form
    {
        string url = ConfigurationManager.AppSettings["urlApi"];
        private readonly FormProductos formProductos;
        private readonly ErrorProvider errorProvider = new ErrorProvider();
        public FormEditar(int id, FormCrear form)
        {
            InitializeComponent();
        }


        private void InitializeProductFields(Producto product)
        {
            txtId.Text = product.Id.ToString();
            txtCategoria.Text = product.Category;
            txtDescripcion.Text = product.Description;
            txtTitulo.Text = product.Title;
            txtPrecio.Text = product.Price.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (ValidarCampos())
            {
                var productoActualizado = new Producto();
                {
                    int Id = int.Parse(txtId.Text);
                    decimal Price = decimal.Parse(txtPrecio.Text);
                    string Title = txtTitulo.Text;
                    string Description = txtDescripcion.Text;
                    string Category = txtCategoria.Text;
                };
                GuardarProducto(productoActualizado);
            }
            else
            {
                DialogResult result = MessageBox.Show("Algunos campos obligatorios están vacíos o no cumplen con los requisitos. ¿Querés cancelar la edición de producto?", "Error de validación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
        private void GuardarProducto(Producto producto)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest($"products/{producto.Id}", Method.Put).AddJsonBody(producto);
                var response = client.Put(request);
                if (response.IsSuccessful)
                {
                    MessageBox.Show("Producto actualizado correctamente.");
                    formProductos.EditarProducto(producto);
                    this.Close();
                }
                else
                {
                    MostrarError("Error al actualizar el producto", response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al intentar actualizar el producto", ex.Message);
            }
        }
        private void MostrarError(string mensaje, string detalle)
        {
            MessageBox.Show($"{mensaje}: {detalle}");
            this.Close();
        }
        private bool ValidarCampos()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                errorProvider.SetError(txtTitulo, "El título es obligatorio.");
                isValid = false;
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal price) || price <= 0)
            {
                errorProvider.SetError(txtPrecio, "El precio es obligatorio y debe ser mayor que 0.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out _))
            {
                errorProvider.SetError(txtId, "El ID es obligatorio y debe ser un número válido.");
                isValid = false;
            }
            return isValid;
        }

    }
}
