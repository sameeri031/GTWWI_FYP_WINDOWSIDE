using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1
{
    public partial class EDITFOLDER : Form
    {
        string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain

        string x_person = "";
        string x_location = "";
        string x_event = "";
        DateTime x_date;

        List<string> selectedImages = new List<string>();
        public EDITFOLDER(List<string> selectedImages)
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
                if (!Path.IsPathRooted(fulpath))
                {
                    fulpath = Path.Combine(imageBasePath, imgPath);
                }

                if (System.IO.File.Exists(imgPath))
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
                pb.Tag = imgPath;

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




        private void EDITFOLDER_Load(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) &&
                                           string.IsNullOrWhiteSpace(tbevent.Text) &&
                                           string.IsNullOrWhiteSpace(tblocation.Text))
            {
                MessageBox.Show("ENTER SOME COMMON TAG");
                return;
            }
            else
            {
                List<string> finalImages = GetCheckedImages();

                MessageBox.Show("Selected Images: " + finalImages.Count);


                await AutoUploadAllAsync(finalImages);
            }
        }

private async Task AutoUploadAllAsync(List<string> imagesToUpload)
        {
            int count = 0;
            foreach (var imagePath in imagesToUpload)
            {
                count++;
                // Ek ek karke upload karein
                await UploadSinglePhotoAsync(imagePath);

                // Optional: UI update ya progress bar yahan dikha sakte hain
                Console.WriteLine($"Uploaded {count} of {imagesToUpload.Count}");
            }

            MessageBox.Show("All photos processed successfully!");
        }
        
        private static readonly HttpClient client = new HttpClient();


        private async Task UploadSinglePhotoAsync(string imagePath)
        {


            try
            {
                using (var http = new HttpClient())
                using (var form = new MultipartFormDataContent())
                {
                    // 1. GET IMAGE FROM URL
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

                    string fileName = Path.GetFileName(Uri.UnescapeDataString(new Uri(imagePath).AbsolutePath));

                    var metaOld = await readmetadata(fileName);

                    // idr abi person event location purany chk krny ha 

                    string per = string.IsNullOrWhiteSpace(textBox1.Text) ? metaOld.person : textBox1.Text;
                    string lo = string.IsNullOrWhiteSpace(tblocation.Text) ? metaOld.loc : tblocation.Text;
                    string ev = string.IsNullOrWhiteSpace(tbevent.Text) ? metaOld.ev : tbevent.Text;
                    DateTime dt = metaOld.date;


                    MessageBox.Show("uploading single phot0" + " PERSON: " + per + " LOCATION: " + lo + " EVENT: " + ev + " date " + x_date);

                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    form.Add(fileContent, "file", fileName);

                    // 2. ADD TEXTBOX METADATA
                    var metadata = new
                    {
                        Person = per,
                        Event = ev,
                        Location = lo,
                        Date = x_date.ToString("yyyy-MM-dd")
                    };
                    string metadataJson = JsonConvert.SerializeObject(metadata);
                    form.Add(new StringContent(metadataJson, Encoding.UTF8, "application/json"), "metadata");
                    // 3. SEND TO API
                    string api = "http://127.0.0.1:8000/editmetaoffolder";
                    var response = await http.PostAsync(api, form);
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic meta = JsonConvert.DeserializeObject(result);
                    x_location = ""; x_person = ""; x_event = "";

                    if (meta == null)
                    {
                        MessageBox.Show("Server returned no JSON. Raw response: " + result);
                        return;
                    }

                    if (meta.status == null)
                    {
                        MessageBox.Show("Upload failed. Server response: " + result);
                        return;
                    }

                    MessageBox.Show("UPLOAD STATUS: " + meta.status +
                                    " | DB ID: " + meta.db_id +
                                    " | Saved: " + meta.metadata_saved);



                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Upload failed: " + result);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error uploading: " + ex.Message);
            }
        }
        public async Task<(string person, string loc, string ev, DateTime date)> readmetadata(string photos)
        {
            try
            {
                string json = await GetMetadataAsync(photos);

                if (string.IsNullOrWhiteSpace(json))
                    return ("", "", "", DateTime.Now);

                dynamic meta = JsonConvert.DeserializeObject(json);

                string p = meta.custom_metadata?.Person != null ? (string)meta.custom_metadata.Person : "";
                string l = meta.custom_metadata?.Location != null ? (string)meta.custom_metadata.Location : "";
                string e = meta.custom_metadata?.Event != null ? (string)meta.custom_metadata.Event : "";
                DateTime d;

                try { d = DateTime.Parse((string)meta.custom_metadata?.Date); }
                catch { d = DateTime.Now; }

                return (p, l, e, d);
            }
            catch
            {
                return ("", "", "", DateTime.Now);
            }
        }

        //public async Task readmetadata(string photos)
        //{
        //    try
        //    {
        //        string json = await GetMetadataAsync(photos); // ✅ wait for API response
        //        if (string.IsNullOrWhiteSpace(json))
        //        {
        //            MessageBox.Show("⚠️ No metadata found on server!");
        //            return;
        //        }

        //        dynamic meta = JsonConvert.DeserializeObject(json);
        //        if (meta == null || meta.error != null)
        //        {
        //            MessageBox.Show("⚠️ No PhotoInfo found inside XMP!");
        //            return;
        //        }

        //        // ✅ Fill UI fields

        //        x_person = meta.custom_metadata?.Person != null ? (string)meta.custom_metadata.Person : "";
        //        x_event = meta.custom_metadata?.Event != null ? (string)meta.custom_metadata.Event : "";
        //        x_location = meta.custom_metadata?.Location != null ? (string)meta.custom_metadata.Location : "";
        //        try
        //        {
        //            x_date = meta.custom_metadata?.Date != null ? DateTime.Parse((string)meta.custom_metadata.Date) : DateTime.Now;

        //        }
        //        catch
        //        {
        //            x_date = DateTime.Now;

        //        }
        //        MessageBox.Show(" METADATA READ   == PERSON:   " + x_person + " LOCATION:   " + x_location + " EVENT:    " + x_event + " DATE:   " + x_date);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error reading metadata: " + ex.Message);
        //    }
        //}
        private async Task<string> GetMetadataAsync(string filename)
        {
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://127.0.0.1:8000/get_metadata_and_detail?filename={filename}";
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

        //private async Task databaseupdation(string imagePath)
        //{

        //    string persons = textBox1.Text;
        //    string locations = tblocation.Text;
        //    string events = tbevent.Text;



        //    using (var client = new HttpClient())
        //    {
        //        byte[] fileBytes = await client.GetByteArrayAsync(imagePath);

        //        string fileName = Path.GetFileName(new Uri(imagePath).LocalPath);
        //        var form = new MultipartFormDataContent();

        //        form.Add(new StringContent(fileName), "title");
        //        form.Add(new StringContent(persons), "persons");
        //        form.Add(new StringContent(locations), "locations");
        //        form.Add(new StringContent(events), "events");

        //        try
        //        {
        //            var response = await client.PostAsync("http://127.0.0.1:8000/multiple_update_database", form);
        //            string result = await response.Content.ReadAsStringAsync();

        //            if (response.IsSuccessStatusCode)
        //                Console.WriteLine($" database updated ");
        //            else
        //                Console.WriteLine($" database update failed");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($" Error saving metadata for : {ex.Message}");
        //        }
        //    }
        //}


    }



}
