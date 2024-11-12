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

namespace TFI_API.Presentación
{
    public partial class FormEditar : Form
    {
        private readonly ErrorProvider _errorProvider = new ErrorProvider();
        private readonly ConexionAPI _connecectionApi;
        private Producto _product;
        public List<Producto> EditedProducts { get; private set; }
        public List<string> newCategory { get; private set; }
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

        private void btnAcceptEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                MessageBox.Show("Por favor, corrija los errores antes de continuar.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(txtCategoria.Text))
            {
                if (!newCategory.Contains(txtCategoria.Text))
                {
                    newCategory.Add(txtCategoria.Text);
                }
            }

            var updatedProduct = GetUpdatedProductFromFields();
            //MessageBox.Show(_connecectionApi.PutProducts(EditedProducts, updatedProduct));
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private Producto GetUpdatedProductFromFields()
        {
            return new Producto
            {
                Id = int.Parse(txtId.Text),
                Title = txtTitulo.Text,
                Price = decimal.Parse(txtPrecio.Text),
                Description = txtDescripcion.Text,
                Category = txtCategoria.Text
            };
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

            if (!ValidatePrice())
            {
                isValid = false;
            }

            return isValid;
        }

        private bool ValidatePrice()
        {
            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal price) || price <= 0)
            {
                _errorProvider.SetError(txtPrecio, "El campo Precio debe ser un número válido mayor que cero.");
                return false;
            }
            else
            {
                _errorProvider.SetError(txtPrecio, string.Empty);
                return true;
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
