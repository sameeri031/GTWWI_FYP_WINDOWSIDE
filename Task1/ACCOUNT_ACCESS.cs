using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1
{
    public partial class ACCOUNT_ACCESS : Form
    { // 👈 jahan images actually stored hain
       
        List<string> allAccount = new List<string>();
        List<string> selectedImages = new List<string>();
        public ACCOUNT_ACCESS(List<string> selectedImages)
        {
            InitializeComponent();
            this.selectedImages = selectedImages;
            loadaccountfromdatabase();

            loadAlbum();
        }
        private List<string> GetCheckedImages()
        {
            List<string> filtered = new List<string>();

            foreach (GroupBox group in flowLayoutPanel1.Controls)
            {
                CheckBox cb = group.Controls.OfType<CheckBox>().FirstOrDefault();
                PictureBox pb = group.Controls.OfType<PictureBox>().FirstOrDefault();

                if (cb != null && pb != null && cb.Checked == true)
                {
                    string path = pb.Tag.ToString();
                    filtered.Add(path);
                }
            }

            return filtered;
        }

        public async void loadAlbum()
        {
            foreach (var imgPath in selectedImages)
            {
                // GroupBox banaya container ki jaga
                GroupBox group = new GroupBox();
                group.Width = 170;
                group.Height = 190;    // thoda upar space taake title overlap na ho
                group.Text = "";       // groupbox title blank
                group.Margin = new Padding(10);

                // PictureBox
                PictureBox pb = new PictureBox();

                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Dock = DockStyle.Fill;

                string fulpath = imgPath;
                pb.Tag = fulpath;
                if (!Path.IsPathRooted(fulpath))
                {
                    fulpath = Path.Combine(Program.imageBasePath, imgPath);
                }

                if (File.Exists(imgPath))
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            using (var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                            using (var tempImg = System.Drawing.Image.FromStream(fs))
                            {
                                var safeImg = new Bitmap(tempImg);

                                pb.Invoke((Action)(() =>
                                {
                                    pb.Image = safeImg;
                                }));
                            }
                        }
                        catch { }
                    });
                }

                // CheckBox
                CheckBox cb = new CheckBox();
                cb.Text = "";
                cb.Width = 20;
                cb.Height = 20;
                cb.Checked = true;
                cb.BackColor = Color.Transparent;
                cb.Location = new Point(group.Width - 35, 5);  // top-right corner

                // Pehle checkbox add karo, phir picturebox
                group.Controls.Add(pb);
                group.Controls.Add(cb);

                flowLayoutPanel1.Controls.Add(group);
            }
        }


        private async void loadaccountfromdatabase()
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Program.connectionString))
                {
                    string checkQuery = "SELECT Username FROM Account ";
                    SqlCommand cmd = new SqlCommand(checkQuery, con);


                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string username = rdr["Username"].ToString();

                        comboBox1.Items.Add(username);

                        comboBox1.AutoCompleteMode = AutoCompleteMode.None;
                        comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
                    }



                }




            }
            catch
            {
                MessageBox.Show("Error loading persons from server!");
            }
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

            string text = comboBox1.Text;
            int selectionStart = comboBox1.SelectionStart;

            comboBox1.TextChanged -= comboBox1_TextChanged;

            // Clear old items
            comboBox1.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
            {
                // 👉 User ne textbox clear kiya → full list restore
                comboBox1.Items.AddRange(allAccount.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allAccount
                               .Where(p => p.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                               .ToList();

                comboBox1.Items.AddRange(filtered.ToArray());
            }

            comboBox1.DroppedDown = true;
            comboBox1.Text = text;                      // text restore
            comboBox1.SelectionStart = text.Length;     // cursor end pe
            comboBox1.SelectionLength = 0;
            // Cursor ko end pe rakhna zaroori
            comboBox1.SelectionStart = comboBox1.Text.Length;

            comboBox1.TextChanged += comboBox1_TextChanged;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var F = new CREATE_ACCOUNT();
            F.Show();
        }

        private void ACCOUNT_ACCESS_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // giving acces process 
            try
            {
                string selectedUsername = comboBox1.Text;

                if (string.IsNullOrEmpty(selectedUsername))
                {
                    MessageBox.Show("Please select a user");
                    return;
                }
                List<string> checkedImages = GetCheckedImages();

                if (checkedImages.Count == 0)
                {
                    MessageBox.Show("Please select at least one image");
                    return;
                }
                int uid = 0;

                using (SqlConnection con = new SqlConnection(Program.connectionString))
                {
                    string q = "SELECT UID FROM Account WHERE Username = @u";
                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@u", selectedUsername);

                    con.Open();
                    uid = Convert.ToInt32(cmd.ExecuteScalar());
                }
                List<int> imageIds = new List<int>();

                using (SqlConnection con = new SqlConnection(Program.connectionString))
                {
                    con.Open();

                    foreach (string imgPath in checkedImages)
                    {
                        string q = "SELECT ImageID FROM Image WHERE path = @p";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.Parameters.AddWithValue("@p", imgPath);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            imageIds.Add(Convert.ToInt32(result));
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(Program.connectionString))
                {
                    con.Open();

                    foreach (int imgId in imageIds)
                    {
                        string q = @"
        IF NOT EXISTS (
            SELECT 1 FROM Account_Image 
            WHERE UID = @uid AND ImageID = @img
        )
        INSERT INTO Account_Image (UID, ImageID)
        VALUES (@uid, @img)";

                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.Parameters.AddWithValue("@uid", uid);
                        cmd.Parameters.AddWithValue("@img", imgId);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Access granted successfully ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
