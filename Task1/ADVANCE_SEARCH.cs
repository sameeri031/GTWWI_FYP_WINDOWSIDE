using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1
{


    public partial class ADVANCE_SEARCH : Form
    {
         private List<(string Url, dynamic Meta)> matchedMetadata = new List<(string, dynamic)>();
        public List<string> AllImages;
        public Dictionary<string, dynamic> MetadataCache;
        string baseUrl = "http://127.0.0.1:8000/photos/";
        private List<string> currentFiltered = new List<string>();
        public List<string> ResultImages = new();
        List<string> allPersons = new List<string>();
        List<string> allEvent = new List<string>();
        List<string> allLocation = new List<string>();

        public ADVANCE_SEARCH(List<string> allImages, Dictionary<string, dynamic> metadataCache)
        {
            InitializeComponent();
            AllImages = allImages;
            MetadataCache = metadataCache;

            LoadPeopleToCombo();
            LoadEventFromServer();
            LoadLocationFromServer();

            // ✅ personPopup ko SABSE PEHLE initialize karo
            personPopup.DrawMode = DrawMode.OwnerDrawFixed;
            personPopup.ItemHeight = 80;
            personPopup.BorderStyle = BorderStyle.FixedSingle;
            personPopup.Visible = false;
            personPopup.IntegralHeight = false;
            personPopup.DrawItem += personPopup_DrawItem;
            personPopup.Click += personPopup_Click;
            personPopup.ScrollAlwaysVisible = true;
            personPopup.TabStop = false;

            // 🔥 IMPORTANT: Form me add karo aur Z-index control karo
            this.Controls.Add(personPopup);
            this.Controls.SetChildIndex(personPopup, 0); // ⭐ SABSE TOP PE

            // Hide popup when clicking elsewhere
            this.Click += (s, e) => personPopup.Visible = false;
            flowLayoutPanel1.Click += (s, e) => personPopup.Visible = false;
        }
        List<PersonItem> allPersonItems = new List<PersonItem>();


        private void LoadPeopleToCombo()
        {
            comboBox2.Items.Clear();
            allPersonItems.Clear();

            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                string sql = @"
        SELECT p.pid, p.name, MIN(i.path) as sample_path
        FROM Person p
        LEFT JOIN ImagePerson ip ON p.pid = ip.pid
        LEFT JOIN Image i ON ip.ImageID = i.ImageID
        GROUP BY p.pid, p.name";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PersonItem item = new PersonItem
                    {
                        Pid = (int)rdr["pid"],
                        Name = rdr["name"].ToString(),
                        Photo = SystemIcons.Question.ToBitmap()
                    };

                    string path = rdr["sample_path"]?.ToString();
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                        item.Photo = Image.FromFile(path);

                    allPersonItems.Add(item);
                    comboBox2.Items.Add(item);
                }
            }
        }
        private async void LoadEventFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("http://127.0.0.1:8000/event/");
                    var events = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (var p in events)
                    {

                        allEvent.Add(p);
                    }
                    foreach (var p in events)
                    {
                        comboBox1.Items.Add(p);

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
        private async void LoadLocationFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("http://127.0.0.1:8000/location/");
                    var loc = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (var p in loc)
                    {

                        allLocation.Add(p);
                    }
                    foreach (var p in loc)
                    {
                        comboBox3.Items.Add(p);

                        comboBox3.AutoCompleteMode = AutoCompleteMode.None;
                        comboBox3.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error loading persons from server!");
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tbevent_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //SB SELECTED PERSON HONY CHIYE
        /*private async void button7_Click(object sender, EventArgs e)
{
    flowLayoutPanel1.Controls.Clear();
    
    string eventQuery = comboBox1.Text.Trim();
    string locationQuery = comboBox3.Text.Trim();
    
    var selectedPersons = checkedListBox1.CheckedItems.Cast<string>().ToList();
    
    DateTime fromDate = dateTimePicker1.Value.Date;
    DateTime toDate = dateTimePicker2.Value.Date;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        StringBuilder sql = new StringBuilder(@"
            SELECT i.ImageID, i.path, i.date
            FROM Image i
        ");

        List<string> conditions = new List<string>();
        
        // Date filter
        conditions.Add("i.date >= @fromDate AND i.date <= @toDate");

        // Event filter
        if (!string.IsNullOrWhiteSpace(eventQuery))
        {
            sql.Append(" INNER JOIN ImageEvent ie ON i.ImageID = ie.ImageID INNER JOIN Event e ON ie.eid = e.eid ");
            conditions.Add("e.name LIKE @event");
        }

        // Location filter
        if (!string.IsNullOrWhiteSpace(locationQuery))
        {
            sql.Append(" INNER JOIN ImageLocation il ON i.ImageID = il.ImageID INNER JOIN Location l ON il.lid = l.lid ");
            conditions.Add("l.name LIKE @location");
        }

        // Person filter - SABHI persons hone chahiye (AND condition)
        if (selectedPersons.Count > 0)
        {
            sql.Append(@"
                WHERE i.ImageID IN (
                    SELECT ip.ImageID
                    FROM ImagePerson ip
                    INNER JOIN Person p ON ip.pid = p.pid
                    WHERE p.name IN ({PERSON_PARAMS})
                    GROUP BY ip.ImageID
                    HAVING COUNT(DISTINCT p.pid) = @personCount
                )
            ".Replace("{PERSON_PARAMS}", 
                string.Join(",", selectedPersons.Select((p, idx) => $"@person{idx}"))
            ));
            
            if (conditions.Count > 0)
                sql.Append(" AND " + string.Join(" AND ", conditions));
        }
        else if (conditions.Count > 0)
        {
            sql.Append(" WHERE " + string.Join(" AND ", conditions));
        }

        SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
        
        // Parameters
        for (int i = 0; i < selectedPersons.Count; i++)
            cmd.Parameters.AddWithValue($"@person{i}", selectedPersons[i]);
        
        if (selectedPersons.Count > 0)
            cmd.Parameters.AddWithValue("@personCount", selectedPersons.Count);
        
        if (!string.IsNullOrWhiteSpace(eventQuery))
            cmd.Parameters.AddWithValue("@event", $"%{eventQuery}%");
        
        if (!string.IsNullOrWhiteSpace(locationQuery))
            cmd.Parameters.AddWithValue("@location", $"%{locationQuery}%");
        
        cmd.Parameters.AddWithValue("@fromDate", fromDate);
        cmd.Parameters.AddWithValue("@toDate", toDate);

        try
        {
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            
            List<string> imagePaths = new List<string>();
            while (rdr.Read())
                imagePaths.Add(rdr["path"].ToString());
            
            rdr.Close();

            if (imagePaths.Count == 0)
            {
                MessageBox.Show("No matching images found!");
                return;
            }

            await DisplayImagesFromPaths(imagePaths);
            MessageBox.Show($"Found {imagePaths.Count} images!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}\n\nSQL: {sql}");
        }
    }
}*/
        private async void button7_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear(); // Pehle clear karo
            matchedMetadata.Clear();

            // Get selected filters
            string eventQuery = comboBox1.Text.Trim();
            string locationQuery = comboBox3.Text.Trim();

            var selectedPersons = checkedListBox1.CheckedItems
                .Cast<string>()
                .ToList();

            DateTime fromDate = dateTimePicker1.Value.Date;
            DateTime toDate = dateTimePicker2.Value.Date;

            // Build SQL query
            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(@"
            SELECT DISTINCT i.ImageID, i.path, i.date
            FROM Image i
        ");

                // Joins based on filters
                bool needPersonJoin = selectedPersons.Count > 0;
                bool needEventJoin = !string.IsNullOrWhiteSpace(eventQuery);
                bool needLocationJoin = !string.IsNullOrWhiteSpace(locationQuery);

                if (needPersonJoin)
                    sql.Append(" INNER JOIN ImagePerson ip ON i.ImageID = ip.ImageID INNER JOIN Person p ON ip.pid = p.pid ");

                if (needEventJoin)
                    sql.Append(" INNER JOIN ImageEvent ie ON i.ImageID = ie.ImageID INNER JOIN Event e ON ie.eid = e.eid ");

                if (needLocationJoin)
                    sql.Append(" INNER JOIN ImageLocation il ON i.ImageID = il.ImageID INNER JOIN Location l ON il.lid = l.lid ");

                // WHERE clause
                List<string> conditions = new List<string>();

                if (needPersonJoin)
                    conditions.Add($"p.name IN ({string.Join(",", selectedPersons.Select((p, idx) => $"@person{idx}"))})");

                if (needEventJoin)
                    conditions.Add("e.name LIKE @event");

                if (needLocationJoin)
                    conditions.Add("l.name LIKE @location");

                // Date range
                conditions.Add("i.date >= @fromDate AND i.date <= @toDate");

                if (conditions.Count > 0)
                    sql.Append(" WHERE " + string.Join(" AND ", conditions));

                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

                // Add parameters
                for (int i = 0; i < selectedPersons.Count; i++)
                    cmd.Parameters.AddWithValue($"@person{i}", selectedPersons[i]);

                if (needEventJoin)
                    cmd.Parameters.AddWithValue("@event", $"%{eventQuery}%");

                if (needLocationJoin)
                    cmd.Parameters.AddWithValue("@location", $"%{locationQuery}%");

                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    List<string> imagePaths = new List<string>();

                    while (rdr.Read())
                    {
                        string path = rdr["path"].ToString();
                        imagePaths.Add(path);
                    }

                    rdr.Close();

                    if (imagePaths.Count == 0)
                    {
                        MessageBox.Show("No matching images found!");
                        return;
                    }

                    // Display images
                    await DisplayImagesFromPaths(imagePaths);

                    MessageBox.Show($"Found {imagePaths.Count} images!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private async Task DisplayImagesFromPaths(List<string> imagePaths)
        {
            foreach (string path in imagePaths)
            {
                if (!File.Exists(path))
                    continue;

                Panel imgPanel = new Panel
                {
                    Width = 200,
                    Height = 200,
                    Margin = new Padding(10),
                    BackColor = ColorTranslator.FromHtml("#0A4E6E"),
                    BorderStyle = BorderStyle.Fixed3D
                };

                PictureBox pic = new PictureBox
                {
                    Width = 200,
                    Height = 200,
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                try
                {
                    // Load from local file
                    pic.Image = Image.FromFile(path);
                }
                catch
                {
                    continue; // Skip broken images
                }

                imgPanel.Controls.Add(pic);

                pic.Click += (s, e) =>
                {
                    // Convert local path to URL for detail view
                    string filename = Path.GetFileName(path);
                    string imageUrl = baseUrl + filename;
                    new picturedetail("SEARCH", imageUrl).ShowDialog();
                };

                flowLayoutPanel1.Controls.Add(imgPanel);

                // UI responsive rakho
                await Task.Delay(1);
            }
        }





        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                comboBox1.Items.AddRange(allEvent.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allEvent
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

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

            string text = comboBox3.Text;
            int selectionStart = comboBox3.SelectionStart;

            comboBox3.TextChanged -= comboBox3_TextChanged;

            // Clear old items
            comboBox3.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
            {
                // 👉 User ne textbox clear kiya → full list restore
                comboBox3.Items.AddRange(allLocation.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allLocation
                               .Where(p => p.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                               .ToList();

                comboBox3.Items.AddRange(filtered.ToArray());
            }

            comboBox3.DroppedDown = true;
            comboBox3.Text = text;                      // text restore
            comboBox3.SelectionStart = text.Length;     // cursor end pe
            comboBox3.SelectionLength = 0;
            // Cursor ko end pe rakhna zaroori
            comboBox3.SelectionStart = comboBox3.Text.Length;

            comboBox3.TextChanged += comboBox3_TextChanged;

        }
        private void comboBox2_Click(object sender, EventArgs e)
        {
            personPopup.Items.Clear();
            foreach (var p in allPersonItems)
                personPopup.Items.Add(p);

            // Screen coordinates me convert karo
            Point comboScreenPos = comboBox2.PointToScreen(Point.Empty);
            Point formScreenPos = this.PointToScreen(Point.Empty);

            int maxItems = 4;
            int visibleCount = Math.Min(personPopup.Items.Count, maxItems);
            int popupWidth = comboBox2.Width;
            int popupHeight = visibleCount * personPopup.ItemHeight + 2;

            Rectangle screen = Screen.FromControl(this).WorkingArea;

            // ⭐ Form-relative position calculate karo (screen nahi)
            int popupX = comboScreenPos.X - formScreenPos.X;
            int popupY = comboScreenPos.Y - formScreenPos.Y + comboBox2.Height;

            // Check if enough space below
            if (comboScreenPos.Y + comboBox2.Height + popupHeight > screen.Bottom)
            {
                popupY = comboScreenPos.Y - formScreenPos.Y - popupHeight;
            }

            personPopup.Size = new Size(popupWidth, popupHeight);
            personPopup.Location = new Point(popupX, popupY);
            personPopup.MaximumSize = new Size(popupWidth, screen.Height / 2);

            // ✅ Z-order reset karo har baar
            this.Controls.SetChildIndex(personPopup, 0);
            personPopup.Visible = true;
            personPopup.BringToFront();
            personPopup.Focus();
        }
        private void personPopup_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = (PersonItem)personPopup.Items[e.Index];
            e.DrawBackground();

            int imgSize = 64;
            int topPadding = 8;

            if (item.Photo != null)
            {
                e.Graphics.DrawImage(
                    item.Photo,
                    new Rectangle(
                        e.Bounds.Left + 10,
                        e.Bounds.Top + topPadding,
                        imgSize,
                        imgSize
                    )
                );
            }

            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? Brushes.White
                : Brushes.Black;

            e.Graphics.DrawString(
                item.Name,
                e.Font,
                textBrush,
                e.Bounds.Left + 90,
                e.Bounds.Top + (e.Bounds.Height / 2) - 8
            );

            e.DrawFocusRectangle();
        }

        private void personPopup_Click(object sender, EventArgs e)
        {
            if (personPopup.SelectedItem is PersonItem p)
            {
                if (!checkedListBox1.Items.Contains(p.Name))
                    checkedListBox1.Items.Add(p.Name, true);
                else
                {
                    int i = checkedListBox1.Items.IndexOf(p.Name);
                    checkedListBox1.SetItemChecked(i, true);
                }

                personPopup.Visible = false;

                comboBox2.Text = "";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem is PersonItem selectedPerson)
            {
                string name = selectedPerson.Name;
                if (!checkedListBox1.Items.Contains(name))
                    checkedListBox1.Items.Add(name, true);
                else
                {
                    int i = checkedListBox1.Items.IndexOf(name);
                    checkedListBox1.SetItemChecked(i, true);
                }
                comboBox2.Text = "";
            }
        }
        private void ADVANCE_SEARCH_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            var item = (PersonItem)comboBox2.Items[e.Index];

            int imgSize = 64; // 🔥 image size
            int padding = 5;

            // Image draw
            if (item.Photo != null)
            {
                e.Graphics.DrawImage(
                    item.Photo,
                    new Rectangle(
                        e.Bounds.Left + padding,
                        e.Bounds.Top + (e.Bounds.Height - imgSize) / 2,
                        imgSize,
                        imgSize
                    )
                );
            }

            // Text brush
            Brush textBrush =
                (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? Brushes.White
                : Brushes.Black;

            // Text draw
            e.Graphics.DrawString(
                item.Name,
                e.Font,
                textBrush,
                new Rectangle(
                    e.Bounds.Left + imgSize + (padding * 3),
                    e.Bounds.Top,
                    e.Bounds.Width - imgSize,
                    e.Bounds.Height
                ),
                new StringFormat
                {
                    LineAlignment = StringAlignment.Center
                }
            );

            e.DrawFocusRectangle();
        }

        public class PersonItem
        {
            public string Name { get; set; }
            public Image Photo { get; set; }
            public int Pid { get; set; }

            public override string ToString() => Name; // ComboBox search ke liye
        }

        private void personPopup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}


/*using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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


    public partial class ADVANCE_SEARCH : Form
    {

        private List<(string Url, dynamic Meta)> matchedMetadata = new List<(string, dynamic)>();
        public List<string> AllImages;
        public Dictionary<string, dynamic> MetadataCache;
        string baseUrl = "http://127.0.0.1:8000/photos/";
        private List<string> currentFiltered = new List<string>();
        public List<string> ResultImages = new();
        List<string> allPersons = new List<string>();
        List<string> allEvent = new List<string>();
        List<string> allLocation = new List<string>();

        public ADVANCE_SEARCH(List<string> allImages, Dictionary<string, dynamic> metadataCache)
        {
            InitializeComponent();
            AllImages = allImages;
            MetadataCache = metadataCache;

            LoadPersonsFromServer();
            LoadEventFromServer();
            LoadLocationFromServer();
        }
        private async void LoadPersonsFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("http://127.0.0.1:8000/person/");
                    var persons = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (var p in persons)
                    {

                        allPersons.Add(p);
                    }
                    foreach (var p in persons)
                    {
                        comboBox2.Items.Add(p);

                        comboBox2.AutoCompleteMode = AutoCompleteMode.None;
                        comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error loading persons from server!");
            }
        }
        private async void LoadEventFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("http://127.0.0.1:8000/event/");
                    var events = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (var p in events)
                    {

                        allEvent.Add(p);
                    }
                    foreach (var p in events)
                    {
                        comboBox1.Items.Add(p);

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
        private async void LoadLocationFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = await client.GetStringAsync("http://127.0.0.1:8000/location/");
                    var loc = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (var p in loc)
                    {

                        allLocation.Add(p);
                    }
                    foreach (var p in loc)
                    {
                        comboBox3.Items.Add(p);

                        comboBox3.AutoCompleteMode = AutoCompleteMode.None;
                        comboBox3.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error loading persons from server!");
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tbevent_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button7_Click(object sender, EventArgs e)
        {
            string eventQuery = comboBox1.SelectedItem?.ToString() ?? "";

            string locationQuery = comboBox3.Text.Trim().ToLower() ?? "";
            var selectedPersons = checkedListBox1.CheckedItems
        .Cast<string>()
        .Select(p => p.ToLower())
        .ToList();


            DateTime fromDate = dateTimePicker1.Value.Date;
            DateTime toDate = dateTimePicker2.Value.Date;

            // Selected persons

            ResultImages.Clear();

            string eventQueryLC = eventQuery.ToLower();
            string locationQueryLC = locationQuery.ToLower();

            foreach (var filename in AllImages)
            {
                string imageUrl = baseUrl + filename;
                if (MetadataCache.ContainsKey(imageUrl))
                {
                    dynamic cachedMeta = MetadataCache[imageUrl];
                    try
                    {
                        string searchable = $"{cachedMeta.Person} {cachedMeta.Event} {cachedMeta.Location} {cachedMeta.Date}".ToLower();

                        if (searchable.Contains(eventQueryLC) && searchable.Contains(locationQueryLC))
                            for (int i = 0; i < selectedPersons.Count; i++)
                            {
                                if (searchable.Contains(selectedPersons[i].ToLower()))
                                    matchedMetadata.Add((imageUrl, cachedMeta));

                            }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {
                    string metaJson = await GetMetadataAsync(imageUrl);

                    if (!string.IsNullOrWhiteSpace(metaJson))
                    {
                        dynamic meta = JsonConvert.DeserializeObject(metaJson);


                        string searchable = $"{meta.Person} {meta.Event} {meta.Location} {meta.Date}".ToLower();

                        if (searchable.Contains(eventQueryLC) && searchable.Contains(locationQueryLC))
                            for (int i = 0; i < selectedPersons.Count; i++)
                            {
                                if (searchable.Contains(selectedPersons[i].ToLower()))
                                    matchedMetadata.Add((imageUrl, meta));

                            }
                        MetadataCache[imageUrl] = meta;

                    }
                }
            }

            if (matchedMetadata.Count == 0)
            {
                MessageBox.Show("No matching images found!");
                return;
            }

            currentFiltered = matchedMetadata.Select(x => x.Url).ToList();
            await DisplayImages(currentFiltered);

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

        private async Task DisplayImages(List<string> imageUrls)
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





        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

            string text = comboBox2.Text;
            int selectionStart = comboBox2.SelectionStart;

            comboBox2.TextChanged -= comboBox2_TextChanged;

            // Clear old items
            comboBox2.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
            {
                // 👉 User ne textbox clear kiya → full list restore
                comboBox2.Items.AddRange(allPersons.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allPersons
                               .Where(p => p.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                               .ToList();

                comboBox2.Items.AddRange(filtered.ToArray());
            }

            comboBox2.DroppedDown = true;
            comboBox2.Text = text;                      // text restore
            comboBox2.SelectionStart = text.Length;     // cursor end pe
            comboBox2.SelectionLength = 0;
            // Cursor ko end pe rakhna zaroori
            comboBox2.SelectionStart = comboBox2.Text.Length;

            comboBox2.TextChanged += comboBox2_TextChanged;


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
                comboBox1.Items.AddRange(allEvent.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allEvent
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

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

            string text = comboBox3.Text;
            int selectionStart = comboBox3.SelectionStart;

            comboBox3.TextChanged -= comboBox3_TextChanged;

            // Clear old items
            comboBox3.Items.Clear();

            if (string.IsNullOrWhiteSpace(text))
            {
                // 👉 User ne textbox clear kiya → full list restore
                comboBox3.Items.AddRange(allLocation.ToArray());
            }
            else
            {
                // 👉 Filter logic
                var filtered = allLocation
                               .Where(p => p.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                               .ToList();

                comboBox3.Items.AddRange(filtered.ToArray());
            }

            comboBox3.DroppedDown = true;
            comboBox3.Text = text;                      // text restore
            comboBox3.SelectionStart = text.Length;     // cursor end pe
            comboBox3.SelectionLength = 0;
            // Cursor ko end pe rakhna zaroori
            comboBox3.SelectionStart = comboBox3.Text.Length;

            comboBox3.TextChanged += comboBox3_TextChanged;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string person = comboBox2.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(person))
                return;

            // Duplicate add na ho
            if (!checkedListBox1.Items.Contains(person))
            {
                checkedListBox1.Items.Add(person, true);   // true = Add + Check
            }
            else
            {
                // If already exists, ensure it's checked
                int index = checkedListBox1.Items.IndexOf(person);
                checkedListBox1.SetItemChecked(index, true);
            }
        }

        private void ADVANCE_SEARCH_Load(object sender, EventArgs e)
        {

        }
    }
    public class PersonItem
    {
        public string Name { get; set; }
        public Image Photo { get; set; }
        public int Pid { get; set; }

        public override string ToString() => Name; // ComboBox search ke liye
    }

}*/