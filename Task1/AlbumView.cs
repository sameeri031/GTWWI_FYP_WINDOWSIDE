using ImageMagick.Drawing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Task1
{
    public partial class AlbumView : Form
    {
        private string personName;
        private List<string> photos;
        string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain
        string connectionString = "Server=.;Database=GEO PHOTO TAGGING;User Id=sa;Password=123;TrustServerCertificate=True;";


        public AlbumView(string personName, List<string> photos)
        {
            InitializeComponent();
            this.personName = personName;
            string[] p = personName.Split("(");
            label1.Text = p[0];
            this.photos = photos;
            this.Text = $"GEO PHOTO TAGGING";
            this.WindowState = FormWindowState.Maximized;

        }
        private void AlbumView_Load(object sender, EventArgs e)
        {
            LoadAlbum();  // call your method here
        }
        private async void LoadAlbum(List<string> imagesToDisplay = null)
        {

            foreach (var imgUrl in photos)
            {
                GroupBox group = new GroupBox();
                group.Width = 170;
                group.Height = 190;    // thoda upar space taake title overlap na ho
                group.Text = "";       // groupbox title blank
                group.Margin = new Padding(10);

                // PictureBox
                PictureBox pb = new PictureBox();

                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Dock = DockStyle.Fill;
                string fulpath = imgUrl;
                if (!Path.IsPathRooted(fulpath))
                {
                    fulpath = Path.Combine(imageBasePath, imgUrl);
                }

                if (File.Exists(imgUrl))
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            using (var fs = new FileStream(imgUrl, FileMode.Open, FileAccess.Read))
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
                pb.Click += (s, e) =>
                   {
                       new picturedetail(personName, imgUrl).ShowDialog();
                   };
                pb.Tag = imgUrl;

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
            //{
            //    PictureBox pb = new PictureBox
            //    {
            //        Width = 200,
            //        Height = 200,
            //        SizeMode = PictureBoxSizeMode.Zoom,
            //        Margin = new Padding(20)
            //    };

            //    try
            //    {
            //        using (HttpClient client = new HttpClient())
            //        using (var stream = await client.GetStreamAsync(imgUrl))
            //        {
            //            pb.Image = Image.FromStream(stream);
            //        }
            //    }
            //    catch { MessageBox.Show(" image broken"); }

            //    pb.Click += (s, e) =>
            //    {
            //        new picturedetail(personName, imgUrl).ShowDialog();
            //    };

            //    flowLayoutPanel1.Controls.Add(pb);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
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
        private void button10_Click(object sender, EventArgs e)
        {
            List<string> selectedImages = GetCheckedImages();


            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No picture selected!");
                return;
            }
            SHARING f = new SHARING(selectedImages);
            f.Show();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> selectedImages = GetCheckedImages();


            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No picture selected!");
                return;
            }
            EDITFOLDER f = new EDITFOLDER(selectedImages);
            f.Show();
        }
        private void ShowFlatImages(List<string> selectedPhotos)
        {
            flowLayoutPanel1.Controls.Clear();



            foreach (var imgUrl in selectedPhotos)
            {
                GroupBox group = new GroupBox();
                group.Width = 170;
                group.Height = 190;    // thoda upar space taake title overlap na ho
                group.Text = "";       // groupbox title blank
                group.Margin = new Padding(10);

                // PictureBox
                PictureBox pb = new PictureBox();

                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Dock = DockStyle.Fill;
                string fulpath = imgUrl;
                if (!Path.IsPathRooted(fulpath))
                {
                    fulpath = Path.Combine(imageBasePath, imgUrl);
                }

                if (File.Exists(imgUrl))
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            using (var fs = new FileStream(imgUrl, FileMode.Open, FileAccess.Read))
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
                pb.Click += (s, e) =>
                {
                    new picturedetail(personName, imgUrl).ShowDialog();
                };
                pb.Tag = imgUrl;

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
        private async Task<Dictionary<string, List<string>>> GroupByMetadata(string type)
        {
            var grouped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // SQL query dynamic banayenge 'type' ke hisaab se
            string sql = "";
            if (type == "person")
            {
                sql = @"SELECT i.path, p.name 
                FROM Image i 
                JOIN ImagePerson ip ON i.ImageID = ip.ImageID 
                JOIN Person p ON ip.pid = p.pid 
                WHERE i.path IN ({0})";
            }
            else if (type == "event")
            {
                sql = @"SELECT i.path, e.name 
                FROM Image i 
                JOIN ImageEvent ie ON i.ImageID = ie.ImageID 
                JOIN Event e ON ie.eid = e.eid 
                WHERE i.path IN ({0})";
            }
            else if (type == "location")
            {
                sql = @"SELECT i.path, l.name 
                FROM Image i 
                JOIN ImageLocation il ON i.ImageID = il.ImageID 
                JOIN Location l ON il.lid = l.lid 
                WHERE i.path IN ({0})";
            }
            else if (type == "date")
            {
                sql = @"SELECT path, FORMAT(date, 'MMMM yyyy') as name 
    FROM Image 
    WHERE path IN ({0})";
            }

            // Path list ko SQL compatible string mein convert karna ('path1', 'path2')
            var formattedPaths = string.Join(",", photos.Select((p, i) => "@p" + i));
            sql = string.Format(sql, formattedPaths);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Parameters add karna SQL Injection se bachne ke liye
                    for (int i = 0; i < photos.Count; i++)
                        cmd.Parameters.AddWithValue("@p" + i, photos[i]);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string path = reader.GetString(0);
                            string metaName = reader.IsDBNull(1) ? "Unknown" : reader.GetString(1);

                            if (!grouped.ContainsKey(metaName))
                                grouped[metaName] = new List<string>();

                            grouped[metaName].Add(path);
                        }
                    }
                }
            }
            return grouped;
        }
        private async Task RenderFolderView(string type)
        {
            flowLayoutPanel1.Controls.Clear();
            var groupedData = await GroupByMetadata(type);

            if (groupedData != null)
            {
                foreach (var entry in groupedData)
                {
                    Panel albumPanel = new Panel
                    {
                        Width = 200,
                        Height = 280,
                        Margin = new Padding(15),
                        BackColor = ColorTranslator.FromHtml("#0A4E6E"),
                        BorderStyle = BorderStyle.None, // Fixed3D ki jagah None use karke custom border feel di hai
                        Cursor = Cursors.Hand,
                        Padding = new Padding(10)
                    };

                    PictureBox folderIcon = new PictureBox
                    {
                        Width = 170, // Panel width (200) - Padding (10*2)
                        Height = 150,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point(10, 10),
                        BackColor = Color.FromArgb(20, 0, 0, 0), // Image ke peeche halka shade
                    };

                    // --- Pehli image load karne ka logic ---
                    if (entry.Value.Count > 0)
                    {
                        string firstImagePath = entry.Value[0];

                        // Path check karein
                        if (!Path.IsPathRooted(firstImagePath))
                            firstImagePath = Path.Combine(imageBasePath, firstImagePath);

                        if (File.Exists(firstImagePath))
                        {
                            try
                            {
                                // Background thread par image load karein taake UI hang na ho
                                Task.Run(() =>
                                {
                                    using (var fs = new FileStream(firstImagePath, FileMode.Open, FileAccess.Read))
                                    using (var tempImg = System.Drawing.Image.FromStream(fs))
                                    {
                                        var coverImg = new Bitmap(tempImg);
                                        folderIcon.Invoke((Action)(() => folderIcon.Image = coverImg));
                                    }
                                });
                            }
                            catch { /* Error handling */ }
                        }
                    }


                    Label folderLabel = new Label
                    {
                        Text = $"{entry.Key}\n({entry.Value.Count} Photos)",
                        AutoSize = false,
                        Width = 180,
                        Height = 60, // Height barha di taake 2 lines tak naam aa sake
                        TextAlign = System.Drawing.ContentAlignment.TopCenter,
                        Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.White,
                        Location = new Point(10, 180),
                        AutoEllipsis = true // Agar naam phir bhi bada ho toh "..." dikhayega bajaye katne ke
                    };
                    folderIcon.Click += (s, e) =>
                    {
                        ShowFlatImages(entry.Value);
                    };

                    albumPanel.Controls.Add(folderIcon);
                    albumPanel.Controls.Add(folderLabel);
                    flowLayoutPanel1.Controls.Add(albumPanel);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> selectedImages = GetCheckedImages();


            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No picture selected!");
                return;
            }
            var f = new ACCOUNT_ACCESS(selectedImages);
            f.Show();
        }

        private async void EVENT_Click(object sender, EventArgs e)
        {
            await RenderFolderView("event");
        }

        private async void LOCATION_Click(object sender, EventArgs e)
        {
            await RenderFolderView("location");

        }

        private async void PERSON_Click(object sender, EventArgs e)
        {
            await RenderFolderView("person");
        }

        private async void flat_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            LoadAlbum(photos);
        }

        private async void date_Click(object sender, EventArgs e)
        {
            await RenderFolderView("date");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
