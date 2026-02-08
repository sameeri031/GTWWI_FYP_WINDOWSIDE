using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1
{
    public partial class SHARING : Form
    {
        string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain

        List<string> selectedImages = new List<string>();
        public SHARING(List<string> selectedImages)
        {
            InitializeComponent();
            this.selectedImages = selectedImages;
            loadAlbum();

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

        private void SHARING_Load(object sender, EventArgs e)
        {

        }
        //sharing

        private async void button10_Click(object sender, EventArgs e)
        {
            List<string> finalImages = GetCheckedImages();

            if (finalImages.Count == 0)
            {
                MessageBox.Show("Please select at least one image.");
                return;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = Path.Combine(desktopPath, "SharedImages");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Task.Run use kar rahe hain taake UI hang na ho agar bohot saari pics hon
            await Task.Run(() =>
            {
                foreach (var sourcePath in finalImages)
                {
                    try
                    {
                        if (File.Exists(sourcePath))
                        {
                            string fileName = Path.GetFileName(sourcePath);
                            string destPath = Path.Combine(folderPath, fileName);

                            // File copy karne ka sabse fast tareeka
                            File.Copy(sourcePath, destPath, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // UI thread par error dikhane ke liye Invoke zaroori hai
                        this.Invoke((Action)(() => MessageBox.Show($"Failed to copy {sourcePath}: {ex.Message}")));
                    }
                }
            });

            MessageBox.Show($"Done! {finalImages.Count} images copied to Desktop\\SharedImages");
        }
    }
}
