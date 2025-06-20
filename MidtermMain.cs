// Required packages:
// Install-Package MySql.Data

using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InventorySystem
{
    public static class DBConnection
    {
        private static string connStr = "server=localhost;user=root;password=yourpassword;database=NormalizedDB";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
            }
            return conn;
        }
    }

    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT u.UserID, u.Username, r.RoleName FROM Users u JOIN Roles r ON u.RoleID = r.RoleID WHERE u.Username = @username AND u.Password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader["RoleName"].ToString();
                    MessageBox.Show("Login successful as " + role);
                    this.Hide();
                    MainForm mainForm = new MainForm(role);
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
        }
    }

    public partial class MainForm : Form
    {
        private string userRole;

        public MainForm(string role)
        {
            InitializeComponent();
            userRole = role;
            lblRole.Text = "Logged in as: " + userRole;
            if (userRole != "Admin")
            {
                btnManageUsers.Enabled = false;
            }
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ProductForm pf = new ProductForm();
            pf.Show();
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            SupplierForm sf = new SupplierForm();
            sf.Show();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            StockForm sf = new StockForm();
            sf.Show();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            SalesForm sf = new SalesForm();
            sf.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm rf = new ReportsForm();
            rf.Show();
        }
    }

    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Products";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProducts.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Products(Name, Category, Price) VALUES(@name, @category, @price)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@category", txtCategory.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                cmd.ExecuteNonQuery();
                LoadProducts();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null)
            {
                int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
                using (MySqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Products WHERE ProductID = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                    LoadProducts();
                }
            }
        }
    }

    public partial class SupplierForm : Form
    {
        public SupplierForm()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Suppliers";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSuppliers.DataSource = dt;
            }
        }
    }

    public partial class StockForm : Form
    {
        public StockForm()
        {
            InitializeComponent();
            LoadStock();
        }

        private void LoadStock()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT s.StockID, p.Name AS ProductName, sp.Name AS SupplierName, s.QuantityAdded, s.DateAdded FROM Stock s JOIN Products p ON s.ProductID = p.ProductID JOIN Suppliers sp ON s.SupplierID = sp.SupplierID";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStock.DataSource = dt;
            }
        }
    }

    public partial class SalesForm : Form
    {
        public SalesForm()
        {
            InitializeComponent();
            LoadSales();
        }

        private void LoadSales()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT s.SaleID, p.Name AS ProductName, s.QuantitySold, s.SaleDate, s.TotalAmount FROM Sales s JOIN Products p ON s.ProductID = p.ProductID";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSales.DataSource = dt;
            }
        }
    }

    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            LoadRevenueReport();
        }

        private void LoadRevenueReport()
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT p.Name, SUM(s.TotalAmount) AS TotalRevenue FROM Sales s JOIN Products p ON s.ProductID = p.ProductID GROUP BY p.Name";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvReports.DataSource = dt;
            }
        }
    }
}
