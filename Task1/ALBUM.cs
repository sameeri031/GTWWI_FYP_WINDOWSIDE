using ImageMagick;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;



namespace Task1
{

    public partial class ALBUM : Form
    {
        string connectionString = "Server=.;Database=GEO PHOTO TAGGING;User Id=sa;Password=123;TrustServerCertificate=True;";
        string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain

        private bool selectionMode = false;
        private Panel selectedPanel = null;
        private System.Windows.Forms.Timer holdTimer = new System.Windows.Forms.Timer();
        // Add this with your other fields
        private bool selectionTriggeredByHold = false;

        private Panel holdTargetPanel = null;



        bool groupByMonth = true;
        bool isLoaded = false;

        private List<string> allImages = new List<string>();
        private Dictionary<string, dynamic> metadataCache = new();

        public ALBUM()
        {
            InitializeComponent();
            label1.Text = "PEOPLE";
            this.Text = "GEO PHOTO TAGGING";
            this.WindowState = FormWindowState.Maximized;
            flowLayoutPanel1.Controls.Clear();
            this.Load += ALBUM_Load;
            holdTimer.Interval = 500; // half second hold
            holdTimer.Tick += HoldTimer_Tick;
        }
        private async void ALBUM_Load(object sender, EventArgs e)
        {
            try
            {
                if (isLoaded) return;
                isLoaded = true;
                //allImages = await GetServerImagesAsync();
                LoadPeopleGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading images: " + ex.Message);
            }
        }

        private async Task LoadAlbumsFromFoldersAsync(string folderType)
        {
            var grouped = new Dictionary<string, List<string>>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "";

                if (folderType.Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    sql = @"
                        SELECT DISTINCT f.folder_id, f.folder_name, i.path
                        FROM Folder f
                        INNER JOIN FolderImage fi ON f.folder_id = fi.folder_id
                        INNER JOIN Image i ON fi.ImageID = i.ImageID
                        WHERE f.parent_folder_id = (
                            SELECT TOP 1 folder_id 
                            FROM Folder 
                            WHERE folder_name = 'Dates' AND folder_type = 'category'
                        ) 
                        AND f.folder_type = 'date'
                        ORDER BY f.folder_name DESC";
                }
                else if (folderType.Equals("person", StringComparison.OrdinalIgnoreCase))
                {
                    // ✅ SPECIAL HANDLING FOR PERSON - Use ref_id (PID)
                    sql = @"
        WITH FolderHierarchy AS (
            SELECT folder_id, folder_name, ref_id, folder_id as root_folder_id
            FROM Folder
            WHERE folder_type = @type 
              AND parent_folder_id IS NOT NULL
              AND ref_id IS NOT NULL

            UNION ALL

            SELECT f.folder_id, fh.folder_name, fh.ref_id, fh.root_folder_id
            FROM Folder f
            INNER JOIN FolderHierarchy fh ON f.parent_folder_id = fh.folder_id
        )
        SELECT fh.ref_id, fh.folder_name, i.path
        FROM FolderHierarchy fh
        INNER JOIN FolderImage fi ON fh.folder_id = fi.folder_id
        INNER JOIN Image i ON fi.ImageID = i.ImageID
        ORDER BY fh.folder_name;";
                }
                else
                {
                    // For Location/Event - same as before
                    sql = @"
        WITH FolderHierarchy AS (
            SELECT folder_id, folder_name, folder_id as root_folder_id
            FROM Folder
            WHERE folder_type = @type 
              AND parent_folder_id IS NOT NULL

            UNION ALL

            SELECT f.folder_id, fh.folder_name, fh.root_folder_id
            FROM Folder f
            INNER JOIN FolderHierarchy fh ON f.parent_folder_id = fh.folder_id
        )
        SELECT fh.root_folder_id, fh.folder_name, i.path
        FROM FolderHierarchy fh
        INNER JOIN FolderImage fi ON fh.folder_id = fi.folder_id
        INNER JOIN Image i ON fi.ImageID = i.ImageID
        ORDER BY fh.folder_name;";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@type", folderType);

                    if (conn.State != ConnectionState.Open)
                        await conn.OpenAsync();

                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int uniqueId = reader.GetInt32(0);  // folder_id ya ref_id
                        string folderName = reader.GetString(1);
                        string path = reader.IsDBNull(2) ? null : reader.GetString(2);

                        // ✅ Unique key: ID_Name format
                        string key = $"{folderName} (ID: {uniqueId})";

                        if (!grouped.ContainsKey(key))
                            grouped[key] = new List<string>();

                        if (!string.IsNullOrEmpty(path))
                            grouped[key].Add(path);
                    }
                }
            }

            RenderAlbums(grouped);
        }
        private void RenderAlbums(Dictionary<string, List<string>> grouped)
        {
            flowLayoutPanel1.Controls.Clear();
            var keys = grouped.Keys.OrderBy(k => k).ToList();

            foreach (var key in keys)
            {
                string displayName = key;
                var images = grouped[key];
                string coverUrl = images.FirstOrDefault();

                // --- 1. ALBUM CARD (Panel) ---
                // Size ko thoda barhaya hai taake label aur image ko saans lene ki jagah mile
                Panel albumPanel = new Panel
                {
                    Width = 200,
                    Height = 220,
                    Margin = new Padding(15),
                    BackColor = ColorTranslator.FromHtml("#0A4E6E"),
                    BorderStyle = BorderStyle.None, // Fixed3D ki jagah None use karke custom border feel di hai
                    Cursor = Cursors.Hand,
                    Padding = new Padding(10)
                };

                albumPanel.Tag = images;

                // --- 2. IMAGE BOX (PictureBox) ---
                PictureBox cover = new PictureBox
                {
                    Width = 170, // Panel width (200) - Padding (10*2)
                    Height = 150,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(10, 10),
                    BackColor = Color.FromArgb(20, 0, 0, 0), // Image ke peeche halka shade
                };

                // --- 3. NAME LABEL (Label) ---
                string[] nameParts = displayName.Split("(");
                Label nameLabel = new Label
                {
                    Text = nameParts[0],
                    AutoSize = false,
                    Width = 180,
                    Height = 60, // Height barha di taake 2 lines tak naam aa sake
                    TextAlign = System.Drawing.ContentAlignment.TopCenter,
                    Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(10, 180),
                    AutoEllipsis = true // Agar naam phir bhi bada ho toh "..." dikhayega bajaye katne ke
                };

                // --- ASYNC IMAGE LOADING LOGIC (Same as before) ---
                string fullPath = coverUrl;
                if (!string.IsNullOrEmpty(fullPath))
                {
                    if (!Path.IsPathRooted(fullPath))
                        fullPath = Path.Combine(imageBasePath, coverUrl);

                    if (File.Exists(fullPath))
                    {
                        _ = Task.Run(() =>
                        {
                            try
                            {
                                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                                using (var tempImg = System.Drawing.Image.FromStream(fs))
                                {
                                    var safeImg = new Bitmap(tempImg);
                                    cover.Invoke((Action)(() => cover.Image = safeImg));
                                }
                            }
                            catch { /* Handle error or set default icon */ }
                        });
                    }
                }

                // --- EVENT HANDLERS (Same as before) ---
                EventHandler openAlbum = (s, e) =>
                {
                    if (selectionTriggeredByHold)
                    {
                        selectionTriggeredByHold = false;
                        return;
                    }
                    if (albumPanel.BackColor == Color.Black)
                        albumPanel.BackColor = ColorTranslator.FromHtml("#0A4E6E");
                    else
                        new AlbumView(displayName, images).ShowDialog();
                };

                // Attach Click & Mouse Events
                Control[] controls = { albumPanel, cover, nameLabel };
                foreach (var ctrl in controls)
                {
                    ctrl.Click += openAlbum;
                    if (ctrl is Panel p)
                    {
                        p.MouseDown += Panel_MouseDown; p.MouseUp += Panel_MouseUp; p.MouseLeave += Panel_MouseUp;
                    }
                    else if (ctrl is PictureBox pb)
                    {
                        pb.MouseDown += PictureBox_MouseDown; pb.MouseUp += PictureBox_MouseUp; pb.MouseLeave += PictureBox_MouseUp;
                    }
                    else
                    {
                        ctrl.MouseDown += Label_MouseDown; ctrl.MouseUp += Label_MouseUp; ctrl.MouseLeave += Label_MouseUp;
                    }
                }

                albumPanel.Controls.Add(cover);
                albumPanel.Controls.Add(nameLabel);
                flowLayoutPanel1.Controls.Add(albumPanel);
            }
        }
        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            var ctrl = sender as Control;
            holdTargetPanel = ctrl?.Parent as Panel;
            if (holdTargetPanel != null)
                holdTimer.Start();
        }

        private void Label_MouseUp(object sender, EventArgs e)
        {
            holdTimer.Stop();
            holdTargetPanel = null;
        }
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var ctrl = sender as Control;
            holdTargetPanel = ctrl?.Parent as Panel;
            if (holdTargetPanel != null)
                holdTimer.Start();
        }

        private void PictureBox_MouseUp(object sender, EventArgs e)
        {
            holdTimer.Stop();
            holdTargetPanel = null;
        }
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            holdTargetPanel = (sender as Panel);
            if (holdTargetPanel != null)
                holdTimer.Start();
        }

        private void Panel_MouseUp(object sender, EventArgs e)
        {
            holdTimer.Stop();
            holdTargetPanel = null;
        }

        private void HoldTimer_Tick(object sender, EventArgs e)
        {
            holdTimer.Stop();
            if (holdTargetPanel != null)
            {
                EnterSelectionMode(holdTargetPanel);
            }
        }
        private void EnterSelectionMode(Panel panel)
        {
            if (panel == null) return;

            selectionMode = true;
            selectionTriggeredByHold = true; // used to suppress immediate click action
            selectedPanel = panel;

            //  if already selected, unselect; else select
            if (panel.BackColor == Color.Black)
            {
                panel.Tag = null;
                panel.BackColor = ColorTranslator.FromHtml("#0A4E6E"); // restore original
            }
            else
            {

                panel.BackColor = Color.Black; // highlight
            }

            button9.Visible = true;
        }

        private async void LoadPeopleGroups()
        {

            await LoadAlbumsFromFoldersAsync("person");
        }






        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async Task<string> GetMetadataAsync(string imageUrl)
        {
            try
            {

                string filename = Path.GetFileName(imageUrl); // extract only filename

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://127.0.0.1:8000/get_metadata?filename={filename}";
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Metadata fetch error: {ex.Message}");
            }
            return null;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private async void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "PEOPLE";
            groupBox1.Visible = false;
            await LoadAlbumsFromFoldersAsync("person");
            // await LoadAlbumsAsync("People", meta => (string)meta.Person ?? "");
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "LOCATION";
            groupBox1.Visible = false;
            await LoadAlbumsFromFoldersAsync("location");
            //await LoadAlbumsAsync("Location", meta => (string)meta.Location ?? "");
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            label1.Text = "EVENT";
            groupBox1.Visible = false;
            await LoadAlbumsFromFoldersAsync("event");
            // await LoadAlbumsAsync("Event", meta => (string)meta.Event ?? "");
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "DATE";
            groupBox1.Visible = true;
            await LoadAlbumsFromFoldersAsync("date");
            // await LoadAlbumsAsync("Date", meta => (string)meta.Date ?? "");
        }


        private void button8_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            label1.Text = "SEARCH";
            var f = new SEARCH(allImages, metadataCache);
            f.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            if (radioButton2.Checked)
                groupByMonth = true;
            else if (radioButton1.Checked)
                groupByMonth = false;

            var grouped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "";

                // Yahan hum subquery se "Dates" root ki ID khud nikal rahe hain
                sql = @"
                SELECT DISTINCT f.folder_name, i.path
                FROM Folder f
                INNER JOIN FolderImage fi ON f.folder_id = fi.folder_id
                INNER JOIN Image i ON fi.ImageID = i.ImageID
                WHERE f.parent_folder_id = (
                    SELECT TOP 1 folder_id 
                    FROM Folder 
                    WHERE folder_name = 'Dates' AND folder_type = 'category'
                ) 
                AND f.folder_type = 'date'
                ORDER BY f.folder_name DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@type", "date");
                    if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string folderName = reader.GetString(0);
                        string path = reader.IsDBNull(1) ? null : reader.GetString(1);
                        string finalGroupName = folderName; // Default
                        if (DateTime.TryParse(folderName, out DateTime dt))
                        {
                            if (groupByMonth)
                                finalGroupName = dt.ToString("MMMM yyyy"); // "January 2026"
                            else
                                finalGroupName = dt.ToString("yyyy");      // "2026"
                        }

                        if (!grouped.ContainsKey(finalGroupName))
                            grouped[finalGroupName] = new List<string>();

                        if (!string.IsNullOrEmpty(path))
                            grouped[finalGroupName].Add(path);
                    }
                }
            }

            RenderAlbums(grouped);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var F = new Form1();
            F.Show();
        }

        private async void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        private async void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        public async Task<List<string>> GetDeletedPhotosFromServer()
        {
            string url = "http://127.0.0.1:8000/deleted_photos";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<List<string>>(response);
                }
                catch { return new List<string>(); }
            }
        }
        public async Task<List<string>> GetChangedPhotosFromServer()
        {
            string url = "http://127.0.0.1:8000/changed_photos"; // apna server base url
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<string>>(response);
            }
        }

        public async Task ClearDeletedPhotosFromServer()
        {
            string url = "http://127.0.0.1:8000/clear_deleted_photos";
            using (HttpClient client = new HttpClient())
            {
                await client.PostAsync(url, null);
            }
        }


        public async Task ClearChangedPhotosFromServer()
        {
            string url = "http://127.0.0.1:8000/clear_changed_photos";
            using (HttpClient client = new HttpClient())
            {

                await client.PostAsync(url, null);
            }
        }
        // edit meta data of panel
        private void button9_Click(object sender, EventArgs e)
        {
            List<string> selectedImages = new List<string>();

            foreach (Panel p in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                if (p.Tag != null && p.Tag is List<string> imgs && p.BackColor == Color.Black)
                {
                    selectedImages.AddRange(imgs);
                }
            }

            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No albums selected!");
                return;
            }

            // open upload window
            EDITFOLDER f = new EDITFOLDER(selectedImages);
            f.Show();
        }
        //SHARE
        private void button10_Click(object sender, EventArgs e)
        {
            List<string> selectedImages = new List<string>();

            foreach (Panel p in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                if (p.Tag != null && p.Tag is List<string> imgs && p.BackColor == Color.Black)
                {
                    selectedImages.AddRange(imgs);
                }
            }

            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No albums selected!");
                return;
            }
            SHARING f = new SHARING(selectedImages);
            f.Show();


        }

        private void button11_Click(object sender, EventArgs e)
        {
            var F = new CREATE_ACCOUNT();
            F.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            List<string> selectedImages = new List<string>();

            foreach (Panel p in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                if (p.Tag != null && p.Tag is List<string> imgs && p.BackColor == Color.Black)
                {
                    selectedImages.AddRange(imgs);
                }
            }

            if (selectedImages.Count == 0)
            {
                MessageBox.Show("No albums selected!");
                return;
            }
            var f = new ACCOUNT_ACCESS(selectedImages);
            f.Show();
        }
    }
    //private async Task LoadAlbumsAsync(string category, Func<dynamic, string> getTagFunc)
    //{
    //    if (allImages.Count == 0)
    //    {
    //        MessageBox.Show("No images found on server!");
    //        return;
    //    }
    //    /*Create a dictionary where key= string(album name)
    //            value = list of image URLs and
    //           ignore letter casing when comparing keys.”*/
    //    var grouped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
    //    string baseUrl = "http://127.0.0.1:8000/photos/";

    //    foreach (var filename in allImages)
    //    {
    //        string imageUrl = baseUrl + filename;
    //        // gettagfunc take input dynamic and return string
    //        string tagValue = await ExtractTagAsync(imageUrl, getTagFunc);
    //        if (string.IsNullOrWhiteSpace(tagValue))
    //            tagValue = "Unknown";

    //        foreach (var tag in tagValue.Split(',', ';').Select(t => t.Trim()).Where(t => t != ""))
    //        {
    //            //grouped["Ali"].Add(image1);


    //            if (!grouped.ContainsKey(tag))//If album doesn’t exist create it
    //                grouped[tag] = new List<string>();
    //            grouped[tag].Add(imageUrl);//If album  exist add image
    //        }
    //    }

    //    RenderAlbums(grouped);
    //}
    //private async Task<string> ExtractTagAsync(string imageUrl, Func<dynamic, string> getTagFunc)
    //{
    //    try
    //    {
    //        // Return from cache if available
    //        if (metadataCache.ContainsKey(imageUrl))
    //        {
    //            dynamic cachedMeta = metadataCache[imageUrl];
    //            try
    //            {
    //                string tag = getTagFunc(cachedMeta);
    //                return tag ?? ""; //return string agr koi error h to null  return
    //            }
    //            catch { return ""; }
    //        }

    //        // Fetch JSON from server
    //        string json = await GetMetadataAsync(imageUrl);
    //        if (string.IsNullOrWhiteSpace(json))
    //            return "";

    //        dynamic meta = null;
    //        try
    //        {
    //            meta = JsonConvert.DeserializeObject(json);
    //        }
    //        catch
    //        {
    //            return "";
    //        }

    //        // Save to cache for future use
    //        metadataCache[imageUrl] = meta;

    //        // Apply user-provided function to extract tag
    //        try
    //        {
    //            string tag = getTagFunc(meta);
    //            return tag ?? "";
    //        }
    //        catch
    //        {
    //            return "";
    //        }
    //    }
    //    catch
    //    {
    //        return "";
    //    }
    //}
    //***********************************************   

    //        private async Task LoadAlbumsFromFoldersAsync(string folderType)
    //        {
    //            // ✅ Use int key to avoid duplicates
    //            var grouped = new Dictionary<int, (string displayName, List<string> paths)>();

    //            using (SqlConnection conn = new SqlConnection(connectionString))
    //            {
    //                await conn.OpenAsync();
    //                string sql = "";

    //                if (folderType.Equals("date", StringComparison.OrdinalIgnoreCase))
    //                {
    //                    sql = @"
    //                SELECT DISTINCT f.folder_id, f.folder_name, i.path
    //                FROM Folder f
    //                INNER JOIN FolderImage fi ON f.folder_id = fi.folder_id
    //                INNER JOIN Image i ON fi.ImageID = i.ImageID
    //                WHERE f.parent_folder_id = (
    //                    SELECT TOP 1 folder_id 
    //                    FROM Folder 
    //                    WHERE folder_name = 'Dates' AND folder_type = 'category'
    //                ) 
    //                AND f.folder_type = 'date'
    //                ORDER BY f.folder_name DESC";
    //                }
    //                else if (folderType.Equals("person", StringComparison.OrdinalIgnoreCase))
    //                {
    //                    // ✅ For Person - use ref_id (PID)
    //                    sql = @"
    //WITH FolderHierarchy AS (
    //    SELECT folder_id, folder_name, ref_id, folder_id as root_folder_id
    //    FROM Folder
    //    WHERE folder_type = @type 
    //      AND parent_folder_id IS NOT NULL
    //      AND ref_id IS NOT NULL

    //    UNION ALL

    //    SELECT f.folder_id, fh.folder_name, fh.ref_id, fh.root_folder_id
    //    FROM Folder f
    //    INNER JOIN FolderHierarchy fh ON f.parent_folder_id = fh.folder_id
    //)
    //SELECT fh.ref_id, fh.folder_name, i.path
    //FROM FolderHierarchy fh
    //INNER JOIN FolderImage fi ON fh.folder_id = fi.folder_id
    //INNER JOIN Image i ON fi.ImageID = i.ImageID
    //ORDER BY fh.folder_name;";
    //                }
    //                else
    //                {
    //                    // For Location/Event
    //                    sql = @"
    //WITH FolderHierarchy AS (
    //    SELECT folder_id, folder_name, folder_id as root_folder_id
    //    FROM Folder
    //    WHERE folder_type = @type 
    //      AND parent_folder_id IS NOT NULL

    //    UNION ALL

    //    SELECT f.folder_id, fh.folder_name, fh.root_folder_id
    //    FROM Folder f
    //    INNER JOIN FolderHierarchy fh ON f.parent_folder_id = fh.folder_id
    //)
    //SELECT fh.root_folder_id, fh.folder_name, i.path
    //FROM FolderHierarchy fh
    //INNER JOIN FolderImage fi ON fh.folder_id = fi.folder_id
    //INNER JOIN Image i ON fi.ImageID = i.ImageID
    //ORDER BY fh.folder_name;";
    //                }

    //                using (SqlCommand cmd = new SqlCommand(sql, conn))
    //                {
    //                    cmd.CommandTimeout = 120;
    //                    cmd.Parameters.AddWithValue("@type", folderType);

    //                    if (conn.State != ConnectionState.Open)
    //                        await conn.OpenAsync();

    //                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();

    //                    while (await reader.ReadAsync())
    //                    {
    //                        int uniqueId = reader.GetInt32(0);  // folder_id or ref_id (PID)
    //                        string folderName = reader.GetString(1);
    //                        string path = reader.IsDBNull(2) ? null : reader.GetString(2);

    //                        // ✅ Use ID as key (no duplicates possible)
    //                        if (!grouped.ContainsKey(uniqueId))
    //                            grouped[uniqueId] = (folderName, new List<string>());

    //                        if (!string.IsNullOrEmpty(path))
    //                            grouped[uniqueId].paths.Add(path);
    //                    }
    //                }
    //            }

    //            // ✅ FIX: Handle duplicate names properly
    //            var albumData = new Dictionary<string, List<string>>();
    //            var nameCounter = new Dictionary<string, int>();

    //            foreach (var kvp in grouped.OrderBy(x => x.Value.displayName))
    //            {
    //                string displayName = kvp.Value.displayName;
    //                List<string> paths = kvp.Value.paths;

    //                // Check if this name already exists in albumData
    //                if (albumData.ContainsKey(displayName))
    //                {
    //                    // First duplicate - rename the first occurrence
    //                    if (!nameCounter.ContainsKey(displayName))
    //                    {
    //                        // Move first entry to numbered version
    //                        var firstPaths = albumData[displayName];
    //                        albumData.Remove(displayName);
    //                        albumData[$"{displayName} (1)"] = firstPaths;
    //                        nameCounter[displayName] = 1;
    //                    }

    //                    // Add current entry with next number
    //                    nameCounter[displayName]++;
    //                    displayName = $"{displayName} ({nameCounter[displayName]})";
    //                }

    //                albumData[displayName] = paths;
    //            }

    //            RenderAlbums(albumData);
    //        }
    //        }
    //```

    //**Yeh approach behtar hai:**
    //```
    //Database:
    //- SAMEER(PID= 5) → 10 images
    //- SAMEER(PID= 12) → 5 images

    //Display(automatically numbered):
    //📁 SAMEER(1)    ← 10 images
    //📁 SAMEER(2)    ← 5 images




    //private void RenderAlbums(Dictionary<string, List<string>> grouped)
    //{
    //    flowLayoutPanel1.Controls.Clear();
    //    //grouped["Ali"].Add(image1);
    //    // sort keys for stable order (optional)
    //    var keys = grouped.Keys.OrderBy(k => k).ToList();

    //    foreach (var key in keys)
    //    {
    //        string displayName = key;
    //        var images = grouped[key];
    //        string coverUrl = images.FirstOrDefault();

    //        // Album card
    //        Panel albumPanel = new Panel
    //        {
    //            Width = 180,
    //            Height = 190,
    //            Margin = new Padding(20, 20, 20, 20),
    //            BackColor = ColorTranslator.FromHtml("#0A4E6E"),
    //            BorderStyle = BorderStyle.Fixed3D,
    //            Cursor = Cursors.Hand,
    //            Padding = new Padding(5)

    //        };


    //        albumPanel.Tag = images;   // store album images list



    //        PictureBox cover = new PictureBox
    //        {
    //            Width = 180,
    //            Height = 140,
    //            SizeMode = PictureBoxSizeMode.Zoom,
    //            Location = new Point(5, 5),
    //            Margin = new Padding(5)
    //        };

    //        Label nameLabel = new Label
    //        {
    //            Text = displayName,
    //            AutoSize = false,
    //            Width = 180,
    //            Height = 30,
    //            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
    //            Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
    //            ForeColor = Color.White,
    //            Location = new Point(5, 150)
    //        };

    //        //// Load album cover async — don't block UI thread
    //        //if (!string.IsNullOrWhiteSpace(coverUrl))
    //        //{
    //        //    // fire-and-forget task to set image (wrap exceptions)
    //        //    _ = Task.Run(async () =>
    //        //    {
    //        //        try
    //        //        {
    //        //            using (HttpClient client = new HttpClient())
    //        //            using (var stream = await client.GetStreamAsync(coverUrl))
    //        //            {
    //        //                var img = System.Drawing.Image.FromStream(stream);
    //        //                // Because this is background thread, marshal to UI thread
    //        //                cover.Invoke((Action)(() =>
    //        //                {
    //        //                    cover.Image = img;
    //        //                }));
    //        //            }
    //        //        }
    //        //        catch
    //        //        {

    //        //        }
    //        //    });
    //        //}
    //        string fullPath = coverUrl;

    //        // agar relative path hai → absolute banao
    //        if (!Path.IsPathRooted(fullPath))
    //        {
    //            fullPath = Path.Combine(imageBasePath, coverUrl);
    //        }

    //        if (File.Exists(fullPath))
    //        {
    //            _ = Task.Run(() =>
    //            {
    //                try
    //                {
    //                    using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
    //                    using (var tempImg = System.Drawing.Image.FromStream(fs))
    //                    {
    //                        var safeImg = new Bitmap(tempImg);

    //                        cover.Invoke((Action)(() =>
    //                        {
    //                            cover.Image = safeImg;
    //                        }));
    //                    }
    //                }
    //                catch { }
    //            });
    //        }

    //        // Open album on click
    //        EventHandler openAlbum = (s, e) =>
    //        {
    //            // if we just triggered selection by hold, suppress this click open.
    //            if (selectionTriggeredByHold)
    //            {
    //                // reset the flag so next clicks behave normally
    //                selectionTriggeredByHold = false;
    //                return;
    //            }
    //            // ensure closure captures current values
    //            if (albumPanel.BackColor == Color.Black)
    //                albumPanel.BackColor = ColorTranslator.FromHtml("#0A4E6E");
    //            else
    //            {
    //                string name = displayName;
    //                var imgs = images;
    //                new AlbumView(name, imgs).ShowDialog();
    //            }
    //        };

    //        // Attach handlers to all clickable parts
    //        albumPanel.Click += openAlbum;
    //        cover.Click += openAlbum;
    //        nameLabel.Click += openAlbum;

    //        albumPanel.MouseDown += Panel_MouseDown;
    //        albumPanel.MouseUp += Panel_MouseUp;
    //        albumPanel.MouseLeave += Panel_MouseUp;

    //        cover.MouseDown += PictureBox_MouseDown;
    //        cover.MouseUp += PictureBox_MouseUp;
    //        cover.MouseLeave += PictureBox_MouseUp;

    //        nameLabel.MouseDown += Label_MouseDown;
    //        nameLabel.MouseUp += Label_MouseUp;
    //        nameLabel.MouseLeave += Label_MouseUp;


    //        albumPanel.Controls.Add(cover);
    //        albumPanel.Controls.Add(nameLabel);
    //        flowLayoutPanel1.Controls.Add(albumPanel);
    //    }
    //}

}
