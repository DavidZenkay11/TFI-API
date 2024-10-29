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
        
      /* public void CargarProductos()
        {
            try
            {
                //var products = conexionApi.GetProducts();
                //dgvProductos.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}");
            }
        }*/

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

        public void btnAgregar_Click(object sender, EventArgs e)
        {
           // FormCrear formCrear = new FormCrear();
            //formCrear.Show();
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            FormCrear formCrear = new FormCrear();
            formCrear.Show();
            using (FormCrear formcrear = new FormCrear(this.Products))
            {
                if (formcrear.ShowDialog() == DialogResult.OK)
                {
                    this.Products = formcrear.newProduct;

                    dgvProductos.DataSource = null;
                    dgvProductos.DataSource = this.Products;
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
    }
}
