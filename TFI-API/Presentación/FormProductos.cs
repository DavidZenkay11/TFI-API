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

namespace TFI_API
{
    public partial class FormProductos : Form
    {
        private ConexionAPI conexionApi;
        public FormProductos()
        {
            InitializeComponent();
            conexionApi = new ConexionAPI("https://fakestoreapi.com/");
            CargarProductos();
        }
        
        private void CargarProductos()
        {
            try
            {
                var products = conexionApi.GetProducts();
                dgvProductos.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                List<int> idsAEliminar = new List<int>();

                foreach (DataGridViewRow row in dgvProductos.SelectedRows)
                {
                    int idProducto = (int)row.Cells["Id"].Value;
                    idsAEliminar.Add(idProducto);
                }

                try
                {
                    var products = conexionApi.DeleteProducts((List<Producto>)dgvProductos.DataSource, idsAEliminar);
                    dgvProductos.DataSource = products; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el producto: {ex.Message}");
                }
            }
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
                dgvProductos.DataSource = productosActualizados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el producto: {ex.Message}");
            }
        }
    }
}
