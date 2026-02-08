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
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Task1
{
    public partial class CREATE_ACCOUNT : Form
    {
        public CREATE_ACCOUNT()
        {
            InitializeComponent();
        }
        string cs = "Server=.;Database=GEO PHOTO TAGGING;User Id=sa;Password=123;TrustServerCertificate=True;";
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            string name = tbname.Text.Trim();
            string password = textBox1.Text;
            string email = tbemail.Text.Trim();

            // 🔹 REGEX PATTERNS
            string namePattern = @"^[A-Za-z ]+$";
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d).{6,}$";

            // 🔹 EMPTY CHECK
            if (name == "" || password == "" || email == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            // 🔹 NAME REGEX
            if (!Regex.IsMatch(name, namePattern))
            {
                MessageBox.Show("Name can contain only letters");
                return;
            }

            // 🔹 EMAIL REGEX
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Invalid email format");
                return;
            }

            // 🔹 PASSWORD REGEX
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Password must be at least 6 characters and contain letters and numbers");
                return;
            }

            try
            {

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Account WHERE Username = @name OR Email = @email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@name", name);
                    checkCmd.Parameters.AddWithValue("@email", email);

                    con.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Username or email already exists
                        MessageBox.Show("Username or Email already exists!");
                        return; // or throw an exception
                    }
                    string query = "INSERT INTO Account (Username, Password, Email) VALUES (@name, @pass, @email)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@email", email);


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Account Created Successfully ✔");

                tbname.Clear();
                tbemail.Clear();
                textBox1.Clear();
                textBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MANAGEACCES M=new MANAGEACCES();
            M.Show();
        }
    }
}
