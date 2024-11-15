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
using TFI_API.Presentación;

namespace TFI_API.Presentación
{
    public partial class FormCrear : Form
    {
        string url = ConfigurationManager.AppSettings["ApiUrl"];
        private ErrorProvider _errorProvider = new ErrorProvider();
        public Producto productoNuevo;


        public FormCrear()
        {
            InitializeComponent();
            btnAgregar.Enabled = true;
            ConseguirId();
        }

        private void ConseguirId()
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("products", Method.Get);
                var response = client.Execute<List<Producto>>(request);
                if (response.IsSuccessful && response.Data != null)
                {
                    int nuevoId = response.Data.Max(p => p.Id) + 1;
                    txtId.Text = nuevoId.ToString();
                }
                else
                {
                    MessageBox.Show("Error al obtener el Id, intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al obtener el Id: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool ValidarCampos()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                _errorProvider.SetError(txtTitulo, "El campo Title es obligatorio.");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTitulo, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal price) || price <= 0)
            {
                _errorProvider.SetError(txtPrecio, "El campo Price debe ser un número válido mayor que cero.");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtPrecio, string.Empty);
            }

            return isValid;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var conexionApi = new ConexionAPI(url);
            if (!ValidarCampos())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string title = txtTitulo.Text;
            string priceText = txtPrecio.Text;
            decimal price = Convert.ToDecimal(priceText);
            int id = int.Parse(txtId.Text);



            Producto product = new Producto()
            {
                Id = int.Parse(txtId.Text),
                Title = txtTitulo.Text,
                Price = decimal.Parse(txtPrecio.Text),
                Category = txtCategoria.Text,
                Description = txtDescripcion.Text
            };

            productoNuevo = new ConexionAPI(url).PostProducts(product, url);
           
            MessageBox.Show("Producto agregado exitosamente");

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void FormCrear_Load(object sender, EventArgs e)
        {

        }
    }
}
