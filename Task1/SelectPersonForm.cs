using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Task1
{
    public partial class SelectPersonForm : Form
    {
        public int? SelectedPid { get; private set; } = null;

        public SelectPersonForm(List<(int pid, string name, int? imageId, string imagePath)> persons)
        {
            InitializeComponent();

            // ===== FORM SETTINGS =====
            this.Text = "Select Person";
            this.Width = 700;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.WhiteSmoke;

            // ===== LABEL =====
            var lblTitle = new Label
            {
                Text = "Select a Person:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Left = 20,
                Top = 15
            };

            // ===== LISTVIEW =====
            var listView = new ListView
            {
                Left = 20,
                Top = 50,
                Width = 640,
                Height = 340,
                View = View.LargeIcon,
                MultiSelect = false,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // ===== IMAGELIST (Bari tasweeren) =====
            var imageList = new ImageList
            {
                ImageSize = new Size(120, 120),  // Pehle 64x64 thi, ab 120x120
                ColorDepth = ColorDepth.Depth32Bit
            };
            listView.LargeImageList = imageList;

            // ===== LOAD DATA =====
            int imgIndex = 0;
            foreach (var p in persons)
            {
                Image img;
                if (!string.IsNullOrEmpty(p.imagePath) && File.Exists(p.imagePath))
                {
                    // Load image safely aur resize karo
                    using (var temp = Image.FromFile(p.imagePath))
                    {
                        img = new Bitmap(temp, new Size(120, 120));
                    }
                }
                else
                {
                    // Fallback image ko bhi resize karo
                    var icon = SystemIcons.Question.ToBitmap();
                    img = new Bitmap(icon, new Size(120, 120));
                }

                imageList.Images.Add(img);

                var item = new ListViewItem
                {
                    Text = $"{p.name}\n(ID: {p.pid})",
                    ImageIndex = imgIndex,
                    Tag = p.pid,
                    Font = new Font("Segoe UI", 9)
                };
                listView.Items.Add(item);
                imgIndex++;
            }

            // ===== BUTTONS =====
            var okBtn = new Button
            {
                Text = "Use This Person",  // ✅ Changed
                Width = 140,
                Height = 35,
                Left = 200,
                Top = 410,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            okBtn.FlatAppearance.BorderSize = 0;

            var cancelBtn = new Button
            {
                Text = "Create New Person",  // ✅ Changed
                Width = 140,
                Height = 35,
                Left = 360,
                Top = 410,
                BackColor = Color.FromArgb(220, 80, 50),  // ✅ Orange color
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            cancelBtn.FlatAppearance.BorderSize = 0;

            // ===== EVENT HANDLERS =====
            okBtn.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    SelectedPid = (int)listView.SelectedItems[0].Tag;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select a person or click Cancel.",
                                    "No Selection",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            };

            cancelBtn.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Double-click se bhi select ho jaye
            listView.DoubleClick += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    SelectedPid = (int)listView.SelectedItems[0].Tag;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            // ===== ADD CONTROLS =====
            this.Controls.Add(lblTitle);
            this.Controls.Add(listView);
            this.Controls.Add(okBtn);
            this.Controls.Add(cancelBtn);
        }
    }
}