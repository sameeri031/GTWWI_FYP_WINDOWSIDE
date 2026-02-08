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

namespace Task1
{
    public partial class MANAGEACCES : Form
    {
        private int selectedUserId = -1;
        string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain
        string cs = "Password=123;\r\nPersist Security Info=True;\r\nUser ID=sa;\r\nInitial Catalog=GEO PHOTO TAGGING;\r\nData Source=.";

        List<string> selectedImages = new List<string>();
        public MANAGEACCES()
        {
            InitializeComponent();
            loadaccountfromdatabase();
        }

        private void MANAGEACCES_Load(object sender, EventArgs e)
        {

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
                    fulpath = Path.Combine(imageBasePath, imgPath);
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

                using (SqlConnection con = new SqlConnection(cs))
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

        private void CHECK_ACCESS_Click(object sender, EventArgs e)
        {
            string username = comboBox1.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT UID FROM Account WHERE Username = @u", con);
                cmd.Parameters.AddWithValue("@u", username);

                selectedUserId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            LoadImagesByAccount(selectedUserId);

        }
        private void LoadImagesByAccount(int userId)
        {
            flowLayoutPanel1.Controls.Clear();
            selectedImages.Clear();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
            SELECT i.ImageID, i.path
            FROM Image i
            INNER JOIN Account_Image ai ON i.ImageID = ai.ImageID
            WHERE ai.UID = @uid
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@uid", userId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string path = rdr["path"].ToString();
                    int imageId = Convert.ToInt32(rdr["ImageID"]);

                    AddImageCard(path, imageId);
                }
            }
        }
        private void AddImageCard(string imgPath, int imageId)
        {
            GroupBox group = new GroupBox();
            group.Width = 170;
            group.Height = 190;
            group.Margin = new Padding(10);

            PictureBox pb = new PictureBox();
            pb.Dock = DockStyle.Fill;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Tag = imageId;   // 🔥 IMPORTANT

            string fullPath = Path.Combine(imageBasePath, imgPath);
            if (File.Exists(fullPath))
                pb.Image = Image.FromFile(fullPath);

            CheckBox cb = new CheckBox();
            cb.Checked = true;
            cb.Location = new Point(group.Width - 30, 10);
            cb.Tag = imageId;   // 🔥 IMPORTANT

            group.Controls.Add(pb);
            group.Controls.Add(cb);
            flowLayoutPanel1.Controls.Add(group);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                foreach (GroupBox group in flowLayoutPanel1.Controls)
                {
                    CheckBox cb = group.Controls.OfType<CheckBox>().FirstOrDefault();
                    if (cb != null && cb.Checked == false)
                    {
                        int imageId = (int)cb.Tag;

                        SqlCommand cmd = new SqlCommand(
                            "DELETE FROM Account_Image WHERE UID=@u AND ImageID=@i", con);

                        cmd.Parameters.AddWithValue("@u", selectedUserId);
                        cmd.Parameters.AddWithValue("@i", imageId);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            MessageBox.Show("Access updated successfully");
        }
    }
}