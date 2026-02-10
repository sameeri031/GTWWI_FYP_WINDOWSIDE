using ImageMagick;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task1
{

    public partial class Form1 : Form
    {
        int imageId = 0;
        bool auto = false;
        List<string> selectedImages = new List<string>();
        List<face> faceList = new List<face>();
        string imgpath = "";
        string oldnames = "";
        string newnames = "";
        bool DUP;
        int currentIndex = 0;
        int totalImages = 0;
        private bool? customTaggingChoice = null;

        List<string> idlist = new List<string>();

        private int? _peopleRootId;
        private int? _locationsRootId;
        private int? _eventsRootId;
        private int? _datesRootId;
        public Form1()
        {
            InitializeComponent();
            this.Text = "GEO PHOTO TAGGING";
            LoadRootFolderIds();

        }
        private async Task LoadRootFolderIds()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Program.connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"
                    SELECT folder_name, folder_id 
                    FROM Folder 
                    WHERE folder_type = 'category' 
                      AND parent_folder_id = (
                          SELECT folder_id FROM Folder WHERE folder_type = 'root' AND parent_folder_id IS NULL
                      )
                ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string name = reader.GetString(0);
                            int id = reader.GetInt32(1);

                            switch (name.ToLower())
                            {
                                case "people":
                                    _peopleRootId = id;
                                    break;
                                case "locations":
                                    _locationsRootId = id;
                                    break;
                                case "events":
                                    _eventsRootId = id;
                                    break;
                                case "dates":
                                    _datesRootId = id;
                                    break;
                            }
                        }
                    }
                }
                // Validate
                if (!_peopleRootId.HasValue || !_locationsRootId.HasValue ||
                    !_eventsRootId.HasValue || !_datesRootId.HasValue)
                {
                    MessageBox.Show(
                        "Database is missing required root folders:\n\n" +
                        $"People: {(_peopleRootId.HasValue ? "✓" : "✗")}\n" +
                        $"Locations: {(_locationsRootId.HasValue ? "✓" : "✗")}\n" +
                        $"Events: {(_eventsRootId.HasValue ? "✓" : "✗")}\n" +
                        $"Dates: {(_datesRootId.HasValue ? "✓" : "✗")}\n\n" +
                        "Please run the database setup script.",
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading database configuration: {ex.Message}");
                Application.Exit();
            }
        }
        //private async void LoadRootFolderIds()
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        await conn.OpenAsync();

        //        string sql = @"
        //        SELECT folder_name, folder_id 
        //        FROM Folder 
        //        WHERE folder_type = 'category' 
        //          AND parent_folder_id = (SELECT folder_id FROM Folder WHERE folder_type = 'root')
        //    ";

        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    string name = reader.GetString(0);
        //                    int id = reader.GetInt32(1);

        //                    switch (name)
        //                    {
        //                        case "People":
        //                            _peopleRootId = id;
        //                            break;
        //                        case "Locations":
        //                            _locationsRootId = id;
        //                            break;
        //                        case "Events":
        //                            _eventsRootId = id;
        //                            break;
        //                        case "Dates":
        //                            _datesRootId = id;
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Validate all roots were found
        //if (!_peopleRootId.HasValue || !_locationsRootId.HasValue ||
        //    !_eventsRootId.HasValue || !_datesRootId.HasValue)
        //{
        //    MessageBox.Show("ERROR: Database is missing required root folders! Please run setup script.");
        //    this.Close();


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imgpath))
            {
                MessageBox.Show("Please select photos first.");
                return;
            }
            button1.Enabled = false;
            label7.Text = "Saving...";
            label7.Visible = true;
            // --- Step 1: Duplicate check ---
            // --- Step 1: Duplicate check ---
            try
            {
                using (var client = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        var imageContent = new ByteArrayContent(File.ReadAllBytes(imgpath));
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        form.Add(imageContent, "file", Path.GetFileName(imgpath));

                        // ✅ Send title as well (required by FastAPI)
                        string title = Path.GetFileNameWithoutExtension(imgpath);
                        form.Add(new StringContent(title), "title");

                        var response = await client.PostAsync(Program.BASE_URL+"/DUPLICATE", form);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(jsonString);
                        DUP = result.duplicate;

                        if (DUP)
                        {
                            MessageBox.Show($" Skipping {Path.GetFileName(imgpath)} — already on server.");
                            currentIndex++;
                            await LoadNextImage();
                            return;
                        }
                    }
                }
                //***************************************************************************************************************

                // ✅ FIX: Create dictionary to store all person choices
                var personPidMap = new Dictionary<string, int>();

                // Split names and check each one
                string[] personNames = tbperson.Text.Split(',')
                                                    .Select(x => x.Trim())
                                                    .Where(x => !string.IsNullOrEmpty(x))
                                                    .ToArray();

                foreach (string personName in personNames)
                {
                    var matches = await GetPersonsWithSampleImage(personName);

                    if (matches.Count > 0)
                    {
                        // Show selection dialog
                        var selForm = new SelectPersonForm(matches);
                        var dr = selForm.ShowDialog();

                        if (dr == DialogResult.OK && selForm.SelectedPid.HasValue)
                        {
                            // ✅ User ne existing person select kiya
                            personPidMap[personName] = selForm.SelectedPid.Value;
                            Console.WriteLine($"✅ Selected existing PID {selForm.SelectedPid.Value} for '{personName}'");
                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            // ✅ User ne Cancel kiya - personPidMap mein NAHI dalo
                            // ProcessCategory automatically naya person banayega
                            Console.WriteLine($"⚠️ User cancelled - will create NEW person for '{personName}'");
                        }
                    }
                    // else: No matches found, will create new automatically
                }

                // ✅ FIX: Store mapping AFTER loop completes
                if (personPidMap.Count > 0)
                {
                    tbperson.Tag = personPidMap;
                    Console.WriteLine($"📌 Stored {personPidMap.Count} person mappings");
                }
                else
                {
                    tbperson.Tag = null;
                    Console.WriteLine("📌 No mappings - all new persons will be created");
                }
                //*****************************************************************************************************************


                // --- Step 2: Upload with metadata ---
                using (var client = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        var fileBytes = File.ReadAllBytes(imgpath);
                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        form.Add(fileContent, "file", Path.GetFileName(imgpath));

                        var metadata = new
                        {
                            Person = tbperson.Text,
                            Event = tbevent.Text,
                            Location = tblocation.Text,
                            Date = dateTimePicker1.Value.ToString("yyyy-MM-dd")
                        };
                        string metadataJson = JsonConvert.SerializeObject(metadata);
                        form.Add(new StringContent(metadataJson, Encoding.UTF8, "application/json"), "metadata");

                        var response = await client.PostAsync(Program.BASE_URL+"/upload_photo", form);
                        string serverResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            MessageBox.Show($" Uploaded {Path.GetFileName(imgpath)} successfully!");
                        else
                            MessageBox.Show($" Upload failed for {Path.GetFileName(imgpath)}: {serverResponse}");
                    }
                }
                await databaseinsertion(imgpath);

                currentIndex++;
                await LoadNextImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                button1.Enabled = true;
                label7.Visible = false;
            }

            // --- Step 4: Handle any name updates automatically ---
            newnames = tbperson.Text;

            string[] oldArray = oldnames.Split(',').Select(x => x.Trim()).Where(x => x != "").ToArray();
            string[] newArray = newnames.Split(',').Select(x => x.Trim()).Where(x => x != "").ToArray();

            var addedNames = newArray.Except(oldArray).ToList();
            var removedNames = oldArray.Except(newArray).ToList();

            if (addedNames.Count > 0 && removedNames.Count > 0)
            {
                string updatename = addedNames.First();
                string purana = removedNames.First();
                string updateid = faceList.FirstOrDefault(f => f.name == purana)?.id ?? "";

                if (int.TryParse(updateid, out int idValue))
                    await SendUpdates(idValue, updatename);
            }



        }





        public async Task<List<(int pid, string name, int? imageId, string? imagePath)>> GetPersonsWithSampleImage(string personName)
        {
            var list = new List<(int, string, int?, string?)>();
            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                await conn.OpenAsync();
                string sql = @"
           SELECT 
    p.pid,
    p.name,
    i.ImageID,
    i.path
FROM Person p
OUTER APPLY (
    SELECT TOP 1 i.ImageID, i.path
    FROM ImagePerson ip
    JOIN Image i ON ip.ImageID = i.ImageID
    WHERE ip.pid = p.pid
    ORDER BY i.ImageID   -- ya date, jo chaho
) i
WHERE p.name = @name
ORDER BY p.pid;
        ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", personName);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int pid = reader.GetInt32(reader.GetOrdinal("pid"));
                            string name = reader.GetString(reader.GetOrdinal("name"));
                            int? imageId = reader.IsDBNull(reader.GetOrdinal("ImageID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ImageID"));
                            string imagePath = reader.IsDBNull(reader.GetOrdinal("path")) ? null : reader.GetString(reader.GetOrdinal("path"));
                            list.Add((pid, name, imageId, imagePath));
                        }
                    }
                }
            }
            return list;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            tbperson.Text = "";
            tblocation.Text = string.Empty;
            tbevent.Text = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedImages = ofd.FileNames.ToList();
                if (selectedImages.Count == 0) return;

                totalImages = selectedImages.Count;
                currentIndex = 0;

                // Ask user only once
                if (customTaggingChoice == null)
                {
                    DialogResult result = MessageBox.Show(
                        "Do you want to give custom tags manually?",
                        "Custom Tagging",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    customTaggingChoice = (result == DialogResult.Yes);
                }

                if (customTaggingChoice == true)
                {
                    label7.Visible = true;
                    await LoadNextImage(); // manual tagging path
                }
                else
                {
                    var f = new AUTOUPLOAD(selectedImages);
                    f.Show();
                    //MessageBox.Show("UPLOADING IT WILL TAKE YOUR FEW SECONDS");
                    //auto = true;
                    //await AutoUploadAllAsync(); // automatic path
                }
            }
        }

        private async Task LoadNextImage()
        {
            if (currentIndex >= totalImages)
            {
                // MessageBox.Show(" All photos processed!");
                currentIndex = 0;
                selectedImages.Clear();
                return;
            }

            imgpath = selectedImages[currentIndex];

            this.Text = $"Editing photo {currentIndex + 1}/{totalImages}";

            using (var stream = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                pictureBox1.Image = Image.FromStream(stream);
            }
            tbevent.Text = string.Empty;
            tbevent.Text = string.Empty;
            tblocation.Text = string.Empty;
            tbperson.Text = string.Empty;
            DateTime fileDate = File.GetCreationTime(imgpath);
            dateTimePicker1.Value = fileDate;

            try
            {
                string personName = await IdentifyFaceAsync(imgpath);
                label7.Visible = false;
                tbperson.Text = personName;
                oldnames = personName;
            }
            catch
            {
                tbperson.Text = "";
            }

            DUP = false; // reset flag for this photo
        }
        //private async Task AutoUploadAllAsync()
        //{
        //    var uploadTasks = selectedImages.Select(imagePath => UploadSinglePhotoAsync(imagePath)).ToList();
        //    await Task.WhenAll(uploadTasks);

        //    // MessageBox.Show(" All photos auto-uploaded successfully!");
        //}
        //private static readonly HttpClient client = new HttpClient();

        //private async Task UploadSinglePhotoAsync(string imagePath)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        using (var form = new MultipartFormDataContent())
        //        {
        //            var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        //            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        //            form.Add(imageContent, "file", Path.GetFileName(imagePath));
        //            string title = Path.GetFileNameWithoutExtension(imagePath);
        //            form.Add(new StringContent(title), "title");

        //            var response = await client.PostAsync("http://127.0.0.1:8000/DUPLICATE", form);
        //            var jsonString = await response.Content.ReadAsStringAsync();
        //            dynamic result = JsonConvert.DeserializeObject(jsonString);
        //            DUP = result.duplicate;

        //            if (!DUP)
        //            {
        //                using (var formupload = new MultipartFormDataContent())
        //                {

        //                    string titleT = Path.GetFileNameWithoutExtension(imagePath);
        //                    string date;
        //                    if (auto)
        //                        date = File.GetCreationTime(imagePath).ToString("yyyy-MM-dd");
        //                    else
        //                        date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        //                    string path = "C:\\Users\\Dogesh\\Desktop\\PHOTO_SERVER\\" + Path.GetFileName(imagePath);
        //                    var fileBytes = await File.ReadAllBytesAsync(imagePath);
        //                    var fileContent = new ByteArrayContent(fileBytes);
        //                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        //                    formupload.Add(fileContent, "file", Path.GetFileName(imagePath));

        //                    var metadata = new { Person = "", Event = "", Location = "", Date = File.GetCreationTime(imagePath).ToString("yyyy-MM-dd") };
        //                    // ✅ Correct: Allow default Content-Type for form string (will be text/plain)
        //                    formupload.Add(new StringContent(JsonConvert.SerializeObject(metadata)), "metadata");
        //                    formupload.Add(new StringContent(titleT), "title");
        //                    formupload.Add(new StringContent(date), "date");
        //                    formupload.Add(new StringContent(path), "path");

        //                    try
        //                    {
        //                        var responseupload = await client.PostAsync("http://127.0.0.1:8000/upload_photo_MODEL", formupload);
        //                        responseupload.EnsureSuccessStatusCode();
        //                        Console.WriteLine($"✅ Uploaded {Path.GetFileName(imagePath)}");
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine($"⚠️ Error uploading {Path.GetFileName(imagePath)}: {ex.Message}");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show($"⚠️ Skipping {Path.GetFileName(imgpath)} — already on server.");


        //            }

        //        }
        //    }

        //}

        ////private async Task UploadSinglePhotoAsync(string imagePath)
        //{
        //    using (var client = new HttpClient())
        //    using (var form = new MultipartFormDataContent())
        //    {
        //        var fileBytes = await File.ReadAllBytesAsync(imagePath);
        //        var fileContent = new ByteArrayContent(fileBytes);
        //        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        //        form.Add(fileContent, "file", Path.GetFileName(imagePath));

        //        DateTime fileDate = File.GetCreationTime(imagePath);

        //        var metadata = new
        //        {
        //            Person = "",
        //            Event = "",
        //            Location = "",
        //            Date = fileDate.ToString("yyyy-MM-dd")
        //        };

        //        string metadataJson = JsonConvert.SerializeObject(metadata);
        //        form.Add(new StringContent(metadataJson, Encoding.UTF8, "application/json"), "metadata");

        //        try
        //        {
        //            var response = await client.PostAsync("http://127.0.0.1:8000/upload_photo_MODEL", form);
        //            string result = await response.Content.ReadAsStringAsync();

        //            if (response.IsSuccessStatusCode)
        //                Console.WriteLine($"✅ Uploaded {Path.GetFileName(imagePath)} successfully.");
        //            else
        //                Console.WriteLine($"❌ Upload failed for {Path.GetFileName(imagePath)}: {result}");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"⚠️ Error uploading {Path.GetFileName(imagePath)}: {ex.Message}");
        //        }
        //    }
        //}

        public async Task<string> IdentifyFaceAsync(string imagePath)
        {
            faceList.Clear();

            using (var client = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(imageContent, "file", Path.GetFileName(imagePath));

                    // FastAPI endpoint
                    var response = await client.PostAsync(Program.BASE_URL+"/identify", form);
                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(jsonString);

                    string allPersons = "";
                    //foreach (var r in result.results)
                    //{
                    //    allPersons += r.name.ToString() + ", ";
                    //    faceList.Add(new face { name = r.name.ToString(),id=r.id.ToString() });

                    //}

                    foreach (var r in result.results)
                    {
                        allPersons += r.name.ToString() + ", ";
                        string idVal = r.id.ToString();
                        string nameVal = r.name.ToString();
                        // MessageBox.Show($"Got face: ID={idVal}, Name={nameVal}");
                        faceList.Add(new face { name = nameVal, id = idVal });
                    }
                    oldnames = allPersons;

                    return allPersons;
                }
            }
        }


        public async Task SendUpdates(int id, string name)
        {

            var data = new
            {
                id = id,
                name = name
            };

            string json = JsonConvert.SerializeObject(data);

            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Program.BASE_URL+"/update_detected_person", content);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Server Response:\n" + result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async Task databaseinsertion(string imagePath)
        {
            string title = Path.GetFileName(imagePath);
            string date;
            if (auto)
                date = File.GetCreationTime(imagePath).ToString("yyyy-MM-dd");
            else
                date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string path = "C:\\Users\\Dogesh\\Desktop\\PHOTO_SERVER\\" + Path.GetFileName(imagePath);
            string persons = tbperson.Text.Trim();
            string locations = "", events = "";
            if (tblocation.Text != null)
                locations = tblocation.Text.Trim();
            if (tbevent.Text != null)
                events = tbevent.Text.Trim();

            //using (var client = new HttpClient())
            //{
            //    var form = new MultipartFormDataContent();

            //    form.Add(new StringContent(title), "title");
            //    form.Add(new StringContent(date), "date");
            //    form.Add(new StringContent(path), "path");
            //    form.Add(new StringContent(persons), "persons");
            //    form.Add(new StringContent(locations), "locations");
            //    form.Add(new StringContent(events), "events");

            //    try
            //    {
            //        var response = await client.PostAsync("http://127.0.0.1:8000/insert_into_database", form);
            //        string result = await response.Content.ReadAsStringAsync();

            //        if (response.IsSuccessStatusCode) { }
            //        //MessageBox.Show($" Metadata saved IN DATABASE {Path.GetFileName(imagePath)}");
            //        else
            //            MessageBox.Show($" Metadata  NOT saved IN DATABASE  {Path.GetFileName(imagePath)}: {result}");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Error saving metadata for {Path.GetFileName(imagePath)}: {ex.Message}");
            //    }
            //}
            var personPidMap = tbperson.Tag as Dictionary<string, int>;
            await SaveImageData(title, date, path, persons, locations, events, personPidMap);
        }

        public async Task SaveImageData(string title, string date, string path, string persons, string locations, string events, Dictionary<string, int> personPidMap = null)
        {
            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Insert Image and get ID
                    string imgSql = "INSERT INTO Image (path, date) OUTPUT INSERTED.ImageID VALUES (@path, @date)";
                    SqlCommand imgCmd = new SqlCommand(imgSql, conn, transaction);
                    imgCmd.Parameters.AddWithValue("@path", path);
                    imgCmd.Parameters.AddWithValue("@date", DateTime.Parse(date));
                    imageId = (int)await imgCmd.ExecuteScalarAsync();

                    // 2️⃣ Process Categories
                    await ProcessCategory(conn, transaction, imageId, persons, "Person", "name", "pid", _peopleRootId.Value, "person", date, personPidMap);
                    await ProcessCategory(conn, transaction, imageId, locations, "Location", "name", "lid", _locationsRootId.Value, "location", date);
                    await ProcessCategory(conn, transaction, imageId, events, "Event", "name", "eid", _eventsRootId.Value, "event", date);

                    // 3️⃣ Date Root Tree
                    int dateRootFolderId = await GetOrCreateFolder(conn, transaction, date, "date", _datesRootId.Value, null);
                    await LinkFolderImage(conn, transaction, dateRootFolderId, imageId);

                    transaction.Commit();
                    using (var client = new HttpClient())
                    {
                        var form = new MultipartFormDataContent();

                        form.Add(new StringContent(title), "title");
                        form.Add(new StringContent(imageId.ToString()), "image_id");
                        form.Add(new StringContent(path), "file_path");


                        try
                        {
                            var response = await client.PostAsync(Program.BASE_URL+"/insert_photo_into_duptable", form);
                            string result = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode) { }
                            //MessageBox.Show($" Metadata saved IN DATABASE {Path.GetFileName(imagePath)}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error duplication for: {ex.Message}");
                        }




                        Console.WriteLine("Success: Image ID " + imageId);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw new Exception("Error saving to database: " + ex.Message);
                }
                finally
                {
                    tbperson.Tag = null; // Always clear
                }
            }
        }

        private async Task ProcessCategory(SqlConnection conn, SqlTransaction trans, int imageId, string itemsStr,
    string table, string col, string idCol, int rootId, string folderType, string dateStr,
    Dictionary<string, int> personPidMap = null)
        {
            if (string.IsNullOrWhiteSpace(itemsStr)) return;

            var items = itemsStr.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

            foreach (var item in items)
            {
                int itemId;

                if (table.Equals("Person", StringComparison.OrdinalIgnoreCase)
                     && personPidMap != null && personPidMap.ContainsKey(item))
                {
                    // ✅ User ne existing person SELECT kiya tha
                    itemId = personPidMap[item];
                    Console.WriteLine($"✅ Using selected PID {itemId} for '{item}'");
                }
                else if (table.Equals("Person", StringComparison.OrdinalIgnoreCase))
                {
                    // ✅ User ne Cancel kiya ya no match - FORCE CREATE NEW
                    itemId = await ForceCreateNewPerson(conn, trans, item);
                    Console.WriteLine($"🆕 Created NEW PID {itemId} for '{item}'");
                }
                else
                {
                    // ✅ Normal GetOrCreate for Location/Event
                    itemId = await GetOrCreate(conn, trans, table, col, idCol, item);
                }

                // Link in Many-to-Many table
                string linkSql = $@"
            IF NOT EXISTS (SELECT 1 FROM Image{table} WHERE ImageID = @imgId AND {idCol} = @itemId)
            INSERT INTO Image{table} (ImageID, {idCol}) VALUES (@imgId, @itemId);
        ";
                using (SqlCommand linkCmd = new SqlCommand(linkSql, conn, trans))
                {
                    linkCmd.Parameters.AddWithValue("@imgId", imageId);
                    linkCmd.Parameters.AddWithValue("@itemId", itemId);
                    await linkCmd.ExecuteNonQueryAsync();
                }

                // Create Folder Structure
                int catFolderId = await GetOrCreateFolder(conn, trans, item, folderType, rootId, itemId);
                int dateFolderId = await GetOrCreateFolder(conn, trans, dateStr, "date", catFolderId, null);

                // Link Folder to Image
                await LinkFolderImage(conn, trans, dateFolderId, imageId);
            }
        }

        // ✅ NEW METHOD: Force create new person (ignore duplicates)
        private async Task<int> ForceCreateNewPerson(SqlConnection conn, SqlTransaction trans, string name)
        {
            string insertSql = "INSERT INTO Person (name) OUTPUT INSERTED.pid VALUES (@val)";
            SqlCommand cmd = new SqlCommand(insertSql, conn, trans);
            cmd.Parameters.AddWithValue("@val", name);
            return (int)await cmd.ExecuteScalarAsync();
        }
        private async Task<int> GetOrCreate(SqlConnection conn, SqlTransaction trans, string table, string col, string idCol, string value)
        {
            string selectSql = $"SELECT {idCol} FROM {table} WHERE {col} = @val";
            SqlCommand cmd = new SqlCommand(selectSql, conn, trans);
            cmd.Parameters.AddWithValue("@val", value);
            object result = await cmd.ExecuteScalarAsync();

            if (result != null) return (int)result;

            string insertSql = $"INSERT INTO {table} ({col}) OUTPUT INSERTED.{idCol} VALUES (@val)";
            cmd.CommandText = insertSql;
            return (int)await cmd.ExecuteScalarAsync();
        }
        private async Task<int> GetOrCreateFolder(SqlConnection conn, SqlTransaction trans, string name, string type, int? parentId, int? refId)
        {
            string selectSql = @"
              SELECT folder_id 
               FROM Folder 
               WHERE folder_name = @name
                AND folder_type = @type
                   AND parent_folder_id " + (parentId == null ? "IS NULL" : "= @pId") + @"
                  AND ref_id " + (refId == null ? "IS NULL" : "= @refId");

            SqlCommand cmd = new SqlCommand(selectSql, conn, trans);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);

            if (parentId != null)
                cmd.Parameters.AddWithValue("@pId", parentId);

            if (refId != null)
                cmd.Parameters.AddWithValue("@refId", refId);

            object result = await cmd.ExecuteScalarAsync();
            if (result != null)
                return (int)result;

            string insertSql = @"
              INSERT INTO Folder (folder_name, folder_type, parent_folder_id, ref_id)
              OUTPUT INSERTED.folder_id
              VALUES (@name, @type, @pId, @refId)";

            cmd = new SqlCommand(insertSql, conn, trans);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@pId", (object)parentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@refId", (object)refId ?? DBNull.Value);

            return (int)await cmd.ExecuteScalarAsync();

        }

        private async Task LinkFolderImage(SqlConnection conn, SqlTransaction trans, int folderId, int imageId)
        {
            string sql = "IF NOT EXISTS (SELECT 1 FROM FolderImage WHERE folder_id = @fId AND ImageID = @iId) " +
                         "INSERT INTO FolderImage (folder_id, ImageID) VALUES (@fId, @iId)";
            SqlCommand cmd = new SqlCommand(sql, conn, trans);
            cmd.Parameters.AddWithValue("@fId", folderId);
            cmd.Parameters.AddWithValue("@iId", imageId);
            await cmd.ExecuteNonQueryAsync();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(imgpath) || !File.Exists(imgpath))
            //{
            //    MessageBox.Show("No modified image found. Please save first.");
            //    return;
            //}

            //try
            //{
            //    using (var image = new MagickImage(imgpath))
            //    {
            //        var profile = image.GetProfile("xmp");
            //        if (profile == null)
            //        {
            //            MessageBox.Show("❌ No XMP metadata found!");
            //            return;
            //        }

            //        string xmpXml = Encoding.UTF8.GetString(profile.ToByteArray());

            //        if (xmpXml.Contains("<custom:PhotoInfo>"))
            //        {
            //            int start = xmpXml.IndexOf("<custom:PhotoInfo>") + "<custom:PhotoInfo>".Length;
            //            int end = xmpXml.IndexOf("</custom:PhotoInfo>");
            //            string json = xmpXml.Substring(start, end - start);

            //            // Decode and deserialize
            //            string decodedJson = System.Net.WebUtility.HtmlDecode(json);
            //            try
            //            {
            //                dynamic meta = JsonConvert.DeserializeObject(decodedJson);
            //                tbperson.Text = meta.Person ?? "";
            //                tbevent.Text = meta.Event ?? "";
            //                tblocation.Text = meta.Location ?? "";
            //                dateTimePicker1.Value = meta.Date ?? DateTime.Now;
            //            }
            //            catch
            //            {
            //                MessageBox.Show("Corrupted or invalid metadata JSON!");
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("No PhotoInfo found inside XMP!");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error reading metadata: " + ex.Message);
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tblocation_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    var form = new Event();
        //    form.Show();
        //}

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    var form = new DATE();
        //    form.Show();
        //}

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    var f = new SEARCH();
        //    f.Show();
        //}
    }
    public class face
    {
        public string name { get; set; }
        public string id { get; set; }


    }


}
