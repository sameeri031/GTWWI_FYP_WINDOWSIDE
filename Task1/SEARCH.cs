using ImageMagick;
using Newtonsoft.Json;
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
    public partial class SEARCH : Form
    {
        string connectionString = "Server=.;Database=GEO PHOTO TAGGING;User Id=sa;Password=123;TrustServerCertificate=True;";
        private List<SearchResult> matchedMetadata = new List<SearchResult>();

        private System.Timers.Timer searchDelayTimer;
        string query = "";

        private Dictionary<string, dynamic> metadataCache = new();

        private List<string> allImages = new List<string>();

        public SEARCH(List<string> allImages, Dictionary<string, dynamic> metadataCache)
        {
            InitializeComponent();
            this.Text = "GEO PHOTO TAGGING";

            // 🔹 Initialize timer for delay
            searchDelayTimer = new System.Timers.Timer(500);
            searchDelayTimer.AutoReset = false;
            searchDelayTimer.Elapsed += async (s, ev) =>
            {
                // Use BeginInvoke to safely call UI code
                this.BeginInvoke(new Action(async () => await PerformSearch()));
            };
            this.allImages = allImages;
            this.metadataCache = metadataCache;
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
        }
        private async Task PerformSearch()
        {
            query = textBox1.Text.Trim().ToLower();
            matchedMetadata.Clear();
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SQL query jo saare linked names ko join karke lata hai
                string sql = @"
            SELECT i.path, 
                   ISNULL(STRING_AGG(p.name, ','), '') as PeopleNames,
                   ISNULL(STRING_AGG(e.name, ','), '') as EventNames,
                   ISNULL(STRING_AGG(l.name, ','), '') as LocationNames
            FROM Image i
            LEFT JOIN ImagePerson ip ON i.ImageID = ip.ImageID
            LEFT JOIN Person p ON ip.pid = p.pid
            LEFT JOIN ImageEvent ie ON i.ImageID = ie.ImageID
            LEFT JOIN Event e ON ie.eid = e.eid
            LEFT JOIN ImageLocation il ON i.ImageID = il.ImageID
            LEFT JOIN Location l ON il.lid = l.lid
            WHERE p.name LIKE @q OR e.name LIKE @q OR l.name LIKE @q OR i.path LIKE @q
            GROUP BY i.ImageID, i.path";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@q", "%" + query + "%");

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        matchedMetadata.Add(new SearchResult
                        {
                            Url = reader["path"].ToString(),
                            People = reader["PeopleNames"].ToString(),
                            Events = reader["EventNames"].ToString(),
                            Locations = reader["LocationNames"].ToString()
                        });
                    }
                }
            }

            // Pehli baar mein saare results dikhao
            DisplayImages(matchedMetadata);
        }
        //private DataTable GetSearchResultsFromDb(string searchTxt)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        // SQL Query jo sab tables ko join karti hai aur concatenated names laati hai
        //        string sql = @"
        //    SELECT i.path, i.date, 
        //           ISNULL(STRING_AGG(p.name, ', '), '') as People,
        //           ISNULL(STRING_AGG(e.name, ', '), '') as Events,
        //           ISNULL(STRING_AGG(l.name, ', '), '') as Locations
        //    FROM Image i
        //    LEFT JOIN ImagePerson ip ON i.ImageID = ip.ImageID
        //    LEFT JOIN Person p ON ip.pid = p.pid
        //    LEFT JOIN ImageEvent ie ON i.ImageID = ie.ImageID
        //    LEFT JOIN Event e ON ie.eid = e.eid
        //    LEFT JOIN ImageLocation il ON i.ImageID = il.ImageID
        //    LEFT JOIN Location l ON il.lid = l.lid
        //    WHERE p.name LIKE @q OR e.name LIKE @q OR l.name LIKE @q OR i.path LIKE @q
        //    GROUP BY i.ImageID, i.path, i.date";

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@q", "%" + searchTxt + "%");

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);
        //    }
        //    return dt;
        //}
        private void DisplayImages(List<SearchResult> results)
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var item in results)
            {
                Panel imgPanel = new Panel
                {
                    Width = 190,
                    Height = 250, // Size thoda barhaya taake 3 lines aa saken
                    Margin = new Padding(12),
                    BackColor = ColorTranslator.FromHtml("#0A4E6E"),
                    Padding = new Padding(8)
                };

                PictureBox pic = new PictureBox
                {
                    Width = 174,
                    Height = 140,
                    Location = new Point(8, 8),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(30, 0, 0, 0) // Light contrast
                };

                // Label mein Person, Event aur Location teeno ka data
                Label lbl = new Label
                {
                    Text = $"👤 {item.People}\n🎭 {item.Events}\n📍 {item.Locations}",
                    ForeColor = Color.White,
                    Width = 174,
                    Height = 80, // Height barha di taake teeno lines nazar aayein
                    Location = new Point(8, 155),
                    Font = new Font("Segoe UI Semibold", 8.5f),
                    AutoEllipsis = true
                };

                // File loading logic
                if (File.Exists(item.Url))
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            using (var fs = new FileStream(item.Url, FileMode.Open, FileAccess.Read))
                            {
                                var bmp = new Bitmap(Image.FromStream(fs));
                                pic.Invoke((Action)(() => pic.Image = bmp));
                            }
                        }
                        catch { /* skip if error */ }
                    });
                }

                pic.Click += (s, e) => new picturedetail("SEARCH", item.Url).ShowDialog();

                imgPanel.Controls.Add(pic);
                imgPanel.Controls.Add(lbl);
                flowLayoutPanel1.Controls.Add(imgPanel);
            }
        }
        private async void button7_Click(object sender, EventArgs e)
        {
            string query = textBox1.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Please enter something to search!");
                return;
            }

        }
        private void PEOPLE_Click(object sender, EventArgs e)
        {
            label7.Text = "PEOPLE RELATED SEARCH RESULT";
            FilterByField("Person");
        }
        private void EVENT_Click(object sender, EventArgs e)
        {
            label7.Text = "EVENT RELATED SEARCH RESULT"; FilterByField("Event");
        }
        private void location_Click(object sender, EventArgs e)
        {
            label7.Text = "LOCATION RELATED SEARCH RESULT"; FilterByField("Location");
        }
        private void FilterByField(string fieldType)
        {
            flowLayoutPanel1.Controls.Clear();
            string filterText = textBox1.Text.ToLower();

            List<SearchResult> filteredList;

            // Field ke mutabiq filter logic
            if (fieldType == "Person")
                filteredList = matchedMetadata.Where(x => x.People.ToLower().Contains(filterText)).ToList();
            else if (fieldType == "Event")
                filteredList = matchedMetadata.Where(x => x.Events.ToLower().Contains(filterText)).ToList();
            else if (fieldType == "Location")
                filteredList = matchedMetadata.Where(x => x.Locations.ToLower().Contains(filterText)).ToList();
            else
                filteredList = matchedMetadata;

            if (filteredList.Count == 0)
            {
                MessageBox.Show($"No images found for this {fieldType} in current results.");
                return;
            }

            DisplayImages(filteredList);
        }

        private void SHOWALL_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)

        {
            var f = new ADVANCE_SEARCH(allImages, metadataCache);
            f.Show();




        }
    }
    public class SearchResult
    {
        public string Url { get; set; }
        public string People { get; set; }    // Comma separated names from DB
        public string Events { get; set; }    // Comma separated names from DB
        public string Locations { get; set; } // Comma separated names from DB
    }

}
/*using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task1
{
    public partial class SEARCH : Form
    {
        string baseUrl = "http://127.0.0.1:8000/photos/";
        private List<(string Url, dynamic Meta)> matchedMetadata = new List<(string, dynamic)>();
        private List<string> currentFiltered = new List<string>();
        private System.Timers.Timer searchDelayTimer;
        string query = "";

        private Dictionary<string, dynamic> metadataCache = new();

        private List<string> allImages = new List<string>();

        public SEARCH(List<string> allImages, Dictionary<string, dynamic> metadataCache)
        {
            InitializeComponent();
            this.Text = "GEO PHOTO TAGGING";

            // 🔹 Initialize timer for delay
            searchDelayTimer = new System.Timers.Timer(500);
            searchDelayTimer.AutoReset = false;
            searchDelayTimer.Elapsed += async (s, ev) =>
            {
                // Use BeginInvoke to safely call UI code
                this.BeginInvoke(new Action(async () => await PerformSearch()));
            };
            this.allImages = allImages;
            this.metadataCache = metadataCache;
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            searchDelayTimer.Stop();
            searchDelayTimer.Start();
            //string query = textBox1.Text.Trim().ToLower();
            //if (string.IsNullOrWhiteSpace(query))
            //{
            //    MessageBox.Show("Please enter something to search!");
            //    return;
            //}

            //flowLayoutPanel1.Controls.Clear();
            //matchedMetadata.Clear();

            //var allImages = await GetServerImagesAsync();
            //if (allImages.Count == 0)
            //{
            //    MessageBox.Show("No images found on server!");
            //    return;
            //}

            //foreach (var filename in allImages)
            //{
            //    string imageUrl = baseUrl + filename;
            //    string metaJson = await GetMetadataAsync(imageUrl);

            //    if (!string.IsNullOrWhiteSpace(metaJson))
            //    {
            //        dynamic meta = JsonConvert.DeserializeObject(metaJson);
            //        string searchable = $"{meta.Person} {meta.Event} {meta.Location} {meta.Date}".ToLower();

            //        if (searchable.Contains(query))
            //            matchedMetadata.Add((imageUrl, meta));
            //    }
            //}

            //if (matchedMetadata.Count == 0)
            //{
            //    MessageBox.Show("No matching images found!");
            //    return;
            //}

            //currentFiltered = matchedMetadata.Select(x => x.Url).ToList();
            //DisplayImages(currentFiltered);
        }
        private async Task PerformSearch()
        {
            query = textBox1.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                flowLayoutPanel1.Controls.Clear();
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            matchedMetadata.Clear();


            if (allImages.Count == 0)
            {
                MessageBox.Show("No images found on server!");
                return;
            }

            foreach (var filename in allImages)
            {
                string imageUrl = baseUrl + filename;
                //********************
                if (metadataCache.ContainsKey(imageUrl))
                {
                    dynamic cachedMeta = metadataCache[imageUrl];
                    try
                    {
                        string searchable = $"{cachedMeta.Person} {cachedMeta.Event} {cachedMeta.Location} {cachedMeta.Date}".ToLower();

                        if (searchable.Contains(query))
                            matchedMetadata.Add((imageUrl, cachedMeta));

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {


                    //*********************88

                    string metaJson = await GetMetadataAsync(imageUrl);
                    if (!string.IsNullOrWhiteSpace(metaJson))
                    {
                        dynamic meta = JsonConvert.DeserializeObject(metaJson);
                        string searchable = $"{meta.Person} {meta.Event} {meta.Location} {meta.Date}".ToLower();

                        if (searchable.Contains(query))
                            matchedMetadata.Add((imageUrl, meta));
                        metadataCache[imageUrl] = meta;
                    }
                }
            }

            if (matchedMetadata.Count == 0)
            {
                MessageBox.Show("No matching images found!");
                return;
            }

            currentFiltered = matchedMetadata.Select(x => x.Url).ToList();
            DisplayImages(currentFiltered);
        }
        private async void button7_Click(object sender, EventArgs e)
        {
            string query = textBox1.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Please enter something to search!");
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            matchedMetadata.Clear();

            // var allImages = await GetServerImagesAsync();
            if (allImages.Count == 0)
            {
                MessageBox.Show("No images found on server!");
                return;
            }

            foreach (var filename in allImages)
            {
                string imageUrl = baseUrl + filename;
                string metaJson = await GetMetadataAsync(imageUrl);

                if (!string.IsNullOrWhiteSpace(metaJson))
                {
                    dynamic meta = JsonConvert.DeserializeObject(metaJson);
                    string searchable = $"{meta.Person} {meta.Event} {meta.Location} {meta.Date}".ToLower();

                    if (searchable.Contains(query))
                        matchedMetadata.Add((imageUrl, meta));
                }
            }

            if (matchedMetadata.Count == 0)
            {
                MessageBox.Show("No matching images found!");
                return;
            }

            currentFiltered = matchedMetadata.Select(x => x.Url).ToList();
            DisplayImages(currentFiltered);
        }
        private async Task<List<string>> GetServerImagesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("http://127.0.0.1:8000/get_all_photos");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    List<string> photos = new List<string>();
                    foreach (var p in data.photos)
                        photos.Add((string)p);
                    return photos;
                }
                return new List<string>();
            }
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
        //private async Task<string> GetMetadataAsync(string imageUrl)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        using (var stream = await client.GetStreamAsync(imageUrl))
        //        using (var img = new MagickImage(stream))
        //        {
        //            var profile = img.GetProfile("xmp");
        //            if (profile == null) return null;

        //            string xmpXml = Encoding.UTF8.GetString(profile.ToByteArray());
        //            if (xmpXml.Contains("<custom:PhotoInfo>"))
        //            {
        //                int start = xmpXml.IndexOf("<custom:PhotoInfo>") + "<custom:PhotoInfo>".Length;
        //                int end = xmpXml.IndexOf("</custom:PhotoInfo>");
        //                string json = xmpXml.Substring(start, end - start);
        //                string decodedJson = System.Net.WebUtility.HtmlDecode(json);
        //                return decodedJson;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // silently ignore broken images
        //    }
        //    return null;
        //}
        private async void DisplayImages(List<string> imageUrls)
        {
            foreach (string imageUrl in imageUrls)
            {
                Panel imgPanel = new Panel
                {
                    Width = 150,
                    Height = 150,
                    Margin = new Padding(10, 10, 10, 10),

                    BackColor = ColorTranslator.FromHtml("#0A4E6E"),
                    BorderStyle = BorderStyle.Fixed3D
                };

                PictureBox pic = new PictureBox
                {
                    Width = 140,
                    Height = 140,
                    Location = new Point(5, 5),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                try
                {
                    using (HttpClient client = new HttpClient())
                    using (var stream = await client.GetStreamAsync(imageUrl))
                    {
                        pic.Image = Image.FromStream(stream);
                    }
                }
                catch
                {
                    // skip broken images
                }


                imgPanel.Controls.Add(pic);

                pic.Click += (s, e) =>
                {
                    new picturedetail("SEARCH", imageUrl).ShowDialog();
                };
                flowLayoutPanel1.Controls.Add(imgPanel);
            }
        }
        private void PEOPLE_Click(object sender, EventArgs e)
        {
            label7.Text = "PEOPLE RELATED SEARCH RESULT";
            FilterByField("Person");
        }
        private void EVENT_Click(object sender, EventArgs e)
        {
            label7.Text = "EVENT RELATED SEARCH RESULT"; FilterByField("Event");
        }
        private void location_Click(object sender, EventArgs e)
        {
            label7.Text = "LOCATION RELATED SEARCH RESULT"; FilterByField("Location");
        }
        private void FilterByField(string field)
        {
            flowLayoutPanel1.Controls.Clear();
            var filtered = matchedMetadata
                .Where(x => (x.Meta[field] ?? "").ToString().ToLower().Contains(textBox1.Text.ToLower()))
                .Select(x => x.Url)
                .ToList();

            if (filtered.Count == 0)
                MessageBox.Show($"No related {field} on server!");
            DisplayImages(filtered);
        }
        //private void PEOPLE_Click(object sender, EventArgs e)
        //{

        //    flowLayoutPanel1.Controls.Clear();
        //    var filtered = matchedMetadata
        //        .Where(x => x.Meta.Person != null && x.Meta.Person.ToString().ToLower().Contains(textBox1.Text.ToLower()))
        //        .Select(x => x.Url)
        //        .ToList();
        //    if (filtered.Count == 0)
        //        MessageBox.Show("No  related person on server!");
        //    DisplayImages(filtered);
        //}

        //private void EVENT_Click(object sender, EventArgs e)
        //{
        //    flowLayoutPanel1.Controls.Clear();
        //    var filtered = matchedMetadata
        //        .Where(x => x.Meta.Event != null && x.Meta.Event.ToString().ToLower().Contains(textBox1.Text.ToLower()))
        //        .Select(x => x.Url)
        //        .ToList();
        //    if (filtered.Count == 0)
        //        MessageBox.Show("No related event on server!");
        //    DisplayImages(filtered);

        //}

        //private void location_Click(object sender, EventArgs e)
        //{
        //    flowLayoutPanel1.Controls.Clear();
        //    var filtered = matchedMetadata
        //        .Where(x => x.Meta.Location != null && x.Meta.Location.ToString().ToLower().Contains(textBox1.Text.ToLower()))
        //        .Select(x => x.Url)
        //        .ToList();
        //    if (filtered.Count == 0)
        //        MessageBox.Show("No related location on server!");
        //    DisplayImages(filtered);
        //}

        private void SHOWALL_Click(object sender, EventArgs e)
        {
            label7.Text = "ALL SEARCH RESULT";
            flowLayoutPanel1.Controls.Clear();
            currentFiltered = matchedMetadata.Select(x => x.Url).ToList();
            DisplayImages(currentFiltered);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        
        {
            var f = new ADVANCE_SEARCH(allImages, metadataCache);
            f.Show();




        }
}
}*/