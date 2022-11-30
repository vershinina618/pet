using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PetProducts
{
    public partial class ClientForm : Form
    {
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string str_connect = @"Data Source=DESKTOP-QCM9LHG;Initial Catalog=Trade2;Integrated Security=True";
        string sql;
        string sql1 = "select count(ProductArticleNumber) from Product";
        string sqlSearch = "";
        string sqlFiltr = "";

        public ClientForm(string userName)
        {
            InitializeComponent();
            UserName.Text = userName;


            sql = "SELECT * From Product";
            using (SqlConnection connection = new SqlConnection(str_connect))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                SqlCommand command = new SqlCommand(sql1, connection);
                labelCount.Text = command.ExecuteScalar().ToString();
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridProd.DataSource = ds.Tables[0];

            }
        }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(str_connect);

            sql = "SELECT * From Product";
            if ((comboBoxFilter.Text != "" && comboBoxFilter.Text != "Все типы") || textBoxSearch.Text != "")
                sql += " WHERE ";
            sql += sqlFiltr;
            if (comboBoxFilter.Text != "" && comboBoxFilter.Text != "Все типы" && textBoxSearch.Text != "")
                sql += " AND ";
            sql += sqlSearch;

            adapter = new SqlDataAdapter(sql, connection);
            ds = new DataSet();
            connection.Open();
            adapter.Fill(ds);

            dataGridProd.DataSource = ds.Tables[0];

            connection.Close();
        }
       
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            sqlSearch = String.Format("[ProductName] like '{0}%'", textBoxSearch.Text);
            //sqlSearchMax = String.Format("[Наименование материала] like '{0}%'", textBoxSearch.Text);
            if (textBoxSearch.Text == "")
            {
                sqlSearch = "";
                //sqlSearchMax = "";
            }
            //Max();
            Create();
        }
        private void ButtonBack(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
    }
}
