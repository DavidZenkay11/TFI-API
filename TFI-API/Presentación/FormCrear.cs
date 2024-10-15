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
        public ConexionAPI conexionApi;
        public FormCrear()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
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
    }
}
