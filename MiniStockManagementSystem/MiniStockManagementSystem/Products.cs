using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniStockManagementSystem
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            cbStatus.SelectedIndex = 0;
            LoadData();
        }
        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Sviluppo\MiniStockManagementSystem\MiniStockManagementSystem\Stocks.mdf;Integrated Security=True");
            
            // Insert Logic
            connection.Open();

            bool status = false;
            if(cbStatus.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var sqlQuery = "";

            if (IfProductExists(connection, txtProductCode.Text))
            {
                sqlQuery = @"Update Products set ProductName = '" + txtProductName.Text + "'" +
                                                ", ProductStatus = '" + status + "'" +
                                                " where ProductCode = '" + txtProductCode.Text + "'";
            }
            else
            {
                sqlQuery = @"INSERT INTO Products(ProductCode, ProductName, ProductStatus)" +
                                                "VALUES(" +
                                                "'" + txtProductCode.Text + "' " +
                                                ",'" + txtProductName.Text + "' " +
                                                ",'" + status + "')";
            }
            
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.ExecuteNonQuery();

            connection.Close();

            // reading Data
            LoadData();
        }

        private bool IfProductExists(SqlConnection connection, string productCode)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select 1 from Products where ProductCode = '" + productCode + "'", connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void LoadData()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Sviluppo\MiniStockManagementSystem\MiniStockManagementSystem\Stocks.mdf;Integrated Security=True");

            SqlDataAdapter adapter = new SqlDataAdapter("Select * from Products", connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvProducts.Rows.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                int n = dgvProducts.Rows.Add();
                dgvProducts.Rows[n].Cells[0].Value = dr["ProductCode"].ToString();
                dgvProducts.Rows[n].Cells[1].Value = dr["ProductName"].ToString();
                if ((bool)dr["ProductStatus"])
                {
                    dgvProducts.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dgvProducts.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }

        private void dgvProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtProductCode.Text = dgvProducts.SelectedRows[0].Cells[0].Value.ToString();
            txtProductName.Text = dgvProducts.SelectedRows[0].Cells[1].Value.ToString();
            if (dgvProducts.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                cbStatus.SelectedIndex = 0;
            }
            else
            {
                cbStatus.SelectedIndex = 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Sviluppo\MiniStockManagementSystem\MiniStockManagementSystem\Stocks.mdf;Integrated Security=True");

            var sqlQuery = "";

            if (IfProductExists(connection, txtProductCode.Text))
            {
                connection.Open();

                sqlQuery = @"DELETE FROM Products where ProductCode = '" + txtProductCode.Text + "'";

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.ExecuteNonQuery();

                connection.Close();

            }
            else
            {
                MessageBox.Show("Record Not Exists ... !");;
            }

            // reading Data
            LoadData();
        }
    }
}
