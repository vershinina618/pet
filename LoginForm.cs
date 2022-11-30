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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PetProducts
{
    public partial class LoginForm : Form
    {
        string str_connect = @"Data Source=DESKTOP-QCM9LHG;Initial Catalog=Trade2;Integrated Security=True";
        private string text = String.Empty;
        int errorCount = 0;
        Timer timer1 = new Timer();
        int i = 0;
        
        private void log_in_Click(object sender, EventArgs e)
        {
            string login, password;
            string sql_str = "SELECT * FROM [User]";
            login = textBoxLogin.Text;
            password = textBoxPassword.Text;

            sql_str += "WHERE UserLogin = '" + login + "' AND UserPassword = '" + password + "'";

            SqlConnection connection = new SqlConnection(str_connect);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql_str;
            cmd.ExecuteNonQuery();

            SqlDataReader data = cmd.ExecuteReader();
            string res = string.Empty;
            string role = string.Empty;
            string userName = string.Empty;

            while (data.Read())
            {
                role += data["UserRole"];
                userName += data["UserSurname"];
                userName += " ";
                userName += data["UserName"];
                userName += " ";
                userName += data["UserPatronymic"];
                
            }
            data.Close();
            connection.Close();

            if (role != "" && errorCount == 0)
            {
                if (role == "1")
                {
                    AdminForm admin = new AdminForm(userName);
                    admin.Show();
                    Hide();
                    
                }
                else if (role == "2")
                {
                    ManagerForm manager = new ManagerForm(userName);
                    manager.Show();
                    Hide();
                }
                else if (role == "3")
                {
                    ClientForm clientForm = new ClientForm(userName);
                    clientForm.Show();
                    Hide();
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
                errorCount += 1;
                pictureBoxCaptcha.Visible = true;
                textBoxCaptcha.Visible = true;
                pictureBoxCaptcha.Image = this.CreateImage(pictureBoxCaptcha.Width, pictureBoxCaptcha.Height);
                buttonLogin.Visible = false;
                buttonLogin2.Visible = true;

            };

        }

        public LoginForm()
        {
            InitializeComponent();
            buttonLogin2.Visible = false;
            pictureBoxCaptcha.Visible = false;
            textBoxCaptcha.Visible = false;
            timer1.Tick += new EventHandler(timer2_Tick);
            timer1.Interval = 10000;
        }

        private void ButtonBack(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);

            int Xpos = rnd.Next(10, 50);
            int Ypos = rnd.Next(15, 30);

            Brush[] colors = { Brushes.Black,Brushes.Red, Brushes.Black };
            Graphics g = Graphics.FromImage(result);
            g.Clear(Color.LightGreen);

            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Segoe Print", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            return result;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            buttonLogin2.Enabled = true;
            i = 0;
            timer1.Stop();
        }

        private void buttonLogin2_Click(object sender, EventArgs e)
        {
            string login, password;
            string sql_str = "SELECT * FROM [User]";
            login = textBoxLogin.Text;
            password = textBoxPassword.Text;

            sql_str += "WHERE UserLogin = '" + login + "' AND UserPassword = '" + password + "'";

            SqlConnection connection = new SqlConnection(str_connect);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql_str;
            cmd.ExecuteNonQuery();

            SqlDataReader data = cmd.ExecuteReader();
            string res = string.Empty;
            string role = string.Empty;
            string userName = string.Empty;

            while (data.Read())
            {
                role += data["UserRole"];
                userName += data["UserSurname"];
                userName += " ";
                userName += data["UserName"];
                userName += " ";
                userName += data["UserPatronymic"];
            }
            data.Close();
            connection.Close();

            if (role != "" && textBoxCaptcha.Text == this.text)
            {
                if (role == "1")
                {
                    AdminForm admin = new AdminForm(userName);
                    admin.Show();
                    Hide();

                }
                else if (role == "2")
                {
                    ManagerForm manager = new ManagerForm(userName);
                    manager.Show();
                    Hide();
                }
                else if (role == "3")
                {
                    ClientForm clientForm = new ClientForm(userName);
                    clientForm.Show();
                    Hide();
                }
            }
            else
            {
                buttonLogin2.Enabled = false;
                errorCount = 0;
                timer1.Start();
            };
           
        }
    }
}
