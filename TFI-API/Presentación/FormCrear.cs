using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private ErrorProvider _errorProvider = new ErrorProvider();
        public List<Producto> newProduct { get; private set; }

        
        public FormCrear(List<Producto> existingProducts)
        {
            InitializeComponent();
            this.newProduct = existingProducts;
        }

        /*public FormCrear()
        {
        }*/

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ConexionAPI conexionApi = new ConexionAPI();
            string title = txtTitulo.Text;
            string priceText = txtPrecio.Text;
            decimal price = Convert.ToDecimal(priceText); 
            int id = int.Parse(txtId.Text);

            if (!ValidateFields())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Producto product = new Producto()
            {
                Title = txtTitulo.Text,
                Price = decimal.Parse(txtPrecio.Text),
                Category = txtCategoria.Text,
                Description = txtDescripcion.Text
            };

            MessageBox.Show(conexionApi.PostProducts(newProduct, product));
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
        private void FormCrear_Load(object sender, EventArgs e)
        {
            txtId.Text = GetNextProductId().ToString();
        }
        private int GetNextProductId()
        {
            if (newProduct != null && newProduct.Count > 0)
            {
                return newProduct.Max(p => p.Id) + 1;
            }
            return 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool ValidateFields()
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

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            ConexionAPI conexionApi = new ConexionAPI();
            string title = txtTitulo.Text;
            string priceText = txtPrecio.Text;
            decimal price = Convert.ToDecimal(priceText);
            int id = int.Parse(txtId.Text);

            if (!ValidateFields())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Producto product = new Producto()
            {
                Title = txtTitulo.Text,
                Price = decimal.Parse(txtPrecio.Text),
                Category = txtCategoria.Text,
                Description = txtDescripcion.Text
            };

            MessageBox.Show(conexionApi.PostProducts(newProduct, product));
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
    }
}
