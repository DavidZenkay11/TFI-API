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
        public List<Producto> nuevoProducto { get; private set; }
        public ConexionAPI conexionApi;
        public FormCrear(List<Producto> existingProducts)
        {
            InitializeComponent();
            this.nuevoProducto = existingProducts;
        }

        public FormCrear()
        {
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ConexionAPI connecectionApi = new ConexionAPI();
            Producto nuevoProducto = new Producto()
            {
                Title = txtTitulo.Text,
                Price = decimal.Parse(txtPrecio.Text),
                Category = txtCategoria.Text,
                Description = txtDescripcion.Text
            };
            

            try
            {
                var productosActualizados = conexionApi.PostProducts((List<Producto>)dgvProductos.DataSource, nuevoProducto);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el producto: {ex.Message}");
            }
        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            ConexionAPI conexionApi = new ConexionAPI();
            string title = txtTitulo.Text;
            string priceText = txtPrecio.Text;
            decimal price = Convert.ToDecimal(priceText);

            /*if (!ValidateFields())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            int id = int.Parse(txtId.Text);

            Producto product = new Producto
            {
                Id = id,
                Title = title,
                Price = price,
                Description = txtDescripcion.Text,
                Category = txtCategoria.Text
            };



            //MessageBox.Show(conexionApi.PostProducts(nuevoProducto, product));

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
        private void FormCrear_Load(object sender, EventArgs e)
        {
            txtId.Text = GetNextProductId().ToString();
        }
        private int GetNextProductId()
        {
            if (nuevoProducto != null && nuevoProducto.Count > 0)
            {
                return nuevoProducto.Max(p => p.Id) + 1;
            }
            return 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }/*private ErrorProvider _errorProvider = new ErrorProvider();
        public List<ApiProducts> newProducts { get; private set; }

        public FrmNew(List<ApiProducts> existingProducts)
        {
            InitializeComponent();
            this.newProducts = existingProducts;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmNew_Load(object sender, EventArgs e)
        {
            txtBoxId.Text = GetNextProductId().ToString();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            ConnecectionApi connecectionApi = new ConnecectionApi();
            string title = txtBoxTitle.Text;
            string priceText = txtBoxPrice.Text;
            decimal price = Convert.ToDecimal(priceText);

            if (!ValidateFields())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = int.Parse(txtBoxId.Text);

            ApiProducts product = new ApiProducts
            {
                Id = id,
                Title = title,
                Price = price,
                Description = txtBoxDescription.Text,
                Category = txtBoxCategory.Text
            };

            

            MessageBox.Show(connecectionApi.PostProducts(newProducts, product));

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private bool ValidateFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtBoxTitle.Text))
            {
                _errorProvider.SetError(txtBoxTitle, "El campo Title es obligatorio.");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtBoxTitle, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(txtBoxPrice.Text) || !decimal.TryParse(txtBoxPrice.Text, out decimal price) || price <= 0)
            {
                _errorProvider.SetError(txtBoxPrice, "El campo Price debe ser un número válido mayor que cero.");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtBoxPrice, string.Empty);
            }

            return isValid;
        }

    }*/
}
