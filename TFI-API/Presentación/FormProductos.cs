using NLog;
using RestSharp;
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
                    conexionApi.LimitProduct(Products, limit);
                }
                else
                {
                    conexionApi.GetInCategory(ProductoFiltrado, selectedCategory);
                    conexionApi.LimitProduct(ProductoFiltrado, limit);
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

                dgvProductos.DataSource = null;  
                dgvProductos.DataSource = Products;  
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                var selectedRow = dgvProductos.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value != DBNull.Value)
                {
                    int id = (int)selectedRow.Cells["Id"].Value;
                    FormEditar fEditar = new FormEditar(id, this);
                    if (fEditar.ShowDialog() == DialogResult.OK)
                    {
                        var productoEditado = fEditar.ProductoEditado;
                        if (productoEditado != null)
                        {
                            var productList = dgvProductos.DataSource as List<Producto>;
                            int index = productList.FindIndex(p => p.Id == productoEditado.Id);
                            if (index != -1)
                            {
                                productList[index] = productoEditado;
                                dgvProductos.DataSource = null;
                                dgvProductos.DataSource = productList;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("La fila seleccionada no tiene un ID válido.");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para editar.");
            }
        }
        public void EditarProducto(Producto productoEditado)
        {
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if ((int)row.Cells["Id"].Value == productoEditado.Id)
                {
                    row.Cells["Price"].Value = productoEditado.Price;
                    row.Cells["Title"].Value = productoEditado.Title;
                    row.Cells["Description"].Value = productoEditado.Description;
                    row.Cells["Category"].Value = productoEditado.Category;
                    break;
                }
            }
        }
       
    }
}
