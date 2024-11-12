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

namespace TFI_API
{
    public partial class FormProductos : Form
    {
        public ConexionAPI conexionApi;
        public List<Producto> Products;
        public List<string> Categorias;
        public List<Producto> ProductoFiltrado;
        public FormProductos()
        {
            Products = new List<Producto>();
            Categorias = new List<string>();
            InitializeComponent();
            conexionApi = new ConexionAPI("https://fakestoreapi.com/");
            //var url = ConfigurationManager.AppSettings["urlApi"].ToString();
            //MessageBox.Show(url)
            //CargarProductos();
        }
        


        public void btnEliminar_Click(object sender, EventArgs e)
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



        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            using (FormCrear fCrear = new FormCrear())
            {
                if (fCrear.ShowDialog() == DialogResult.OK)
                {
                    var product = dgvProductos.DataSource as List<Producto> ?? new List<Producto>();
                    product.Add(fCrear.productoNuevo);
                    dgvProductos.DataSource = null;
                    dgvProductos.DataSource = product;
                }
                else
                {
                    MessageBox.Show("Error al agregar el producto");
                }
            }
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            MessageBox.Show(conexionApi.GetProducts(Products));
            ProductoFiltrado = new List<Producto>(Products);
            conexionApi.GetCategories(Categorias);
            dgvProductos.DataSource = Products;
            Categorias.Insert(0, "All");
            cmbCategoria.DataSource = Categorias;
            cmbCategoria.SelectedIndex = 0;
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = cmbCategoria.SelectedItem.ToString();

            ProductoFiltrado = new List<Producto>(Products);

            if (int.TryParse(txtLimite.Text, out int limit) && limit > 0)
            {
                if (selectedCategory == "All")
                {
                    conexionApi.LimitResult(Products, limit);
                }
                else
                {
                    conexionApi.GetInCategory(ProductoFiltrado, selectedCategory);
                    conexionApi.LimitResult(ProductoFiltrado, limit);
                }
            }
            else
            {
                if (selectedCategory == "All")
                {
                    dgvProductos.DataSource = Products;
                }
                else
                {
                    conexionApi.GetInCategory(ProductoFiltrado, selectedCategory);
                }
            }
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = ProductoFiltrado;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCategoria_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var filteredProducts = (List<Producto>)dgvProductos.DataSource;
            var selectedProduct = filteredProducts[e.RowIndex];

        }

        private void btnAscDesc_Click(object sender, EventArgs e)
        {
            string selectedCategory = cmbCategoria.SelectedItem?.ToString();

            if (selectedCategory != "All")
            {
                conexionApi.SortResults(Products, btnAscDesc.Text);
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = Products
                    .Where(p => p.Category != null && p.Category.Equals(selectedCategory))
                    .ToList();
            }
            else
            {
                conexionApi.SortResults(Products, btnAscDesc.Text);
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = Products;
            }

            if (btnAscDesc.Text == "Descendente")
            {
                btnAscDesc.Text = "Ascendente";
            }
            else
            {
                btnAscDesc.Text = "Descendente";
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                var selectedIds = new List<int>();
                foreach (DataGridViewRow row in dgvProductos.SelectedRows)
                {
                    int selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                    selectedIds.Add(selectedId);
                }

                string resultMessage = conexionApi.DeleteProducts(Products, selectedIds);
                MessageBox.Show(resultMessage);

                dgvProductos.DataSource = null;  // Desvincula el DataSource temporalmente
                dgvProductos.DataSource = Products;  // Vuelve a asignar la lista actualizada
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila.");
            }
        }
    }
}
