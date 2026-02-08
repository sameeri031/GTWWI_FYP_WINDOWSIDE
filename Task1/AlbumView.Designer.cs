
namespace Task1
{
    partial class AlbumView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            button7 = new Button();
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            button10 = new Button();
            linkLabel1 = new LinkLabel();
            label1 = new Label();
            flat = new Button();
            EVENT = new Button();
            LOCATION = new Button();
            PERSON = new Button();
            date = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            flowLayoutPanel1.Location = new Point(204, 105);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1648, 683);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(10, 78, 110);
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ForeColor = Color.WhiteSmoke;
            button7.Location = new Point(21, 258);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(151, 46);
            button7.TabIndex = 26;
            button7.Text = "BACK";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click_1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button10);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.ForeColor = Color.WhiteSmoke;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1603, 88);
            panel1.TabIndex = 31;
            panel1.Paint += panel1_Paint;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(10, 78, 110);
            button2.ForeColor = SystemColors.ButtonFace;
            button2.Location = new Point(882, 33);
            button2.Name = "button2";
            button2.Size = new Size(174, 37);
            button2.TabIndex = 32;
            button2.Text = "ACCESS";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 78, 110);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(1093, 32);
            button1.Name = "button1";
            button1.Size = new Size(174, 37);
            button1.TabIndex = 31;
            button1.Text = "SET COMON TAG";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // button10
            // 
            button10.BackColor = Color.FromArgb(10, 78, 110);
            button10.ForeColor = SystemColors.ButtonFace;
            button10.Location = new Point(1303, 33);
            button10.Name = "button10";
            button10.Size = new Size(105, 37);
            button10.TabIndex = 30;
            button10.Text = "SHARE";
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.Chartreuse;
            linkLabel1.Location = new Point(1646, 38);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(63, 28);
            linkLabel1.TabIndex = 28;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "SYNC";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(0, 28);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(145, 38);
            label1.TabIndex = 0;
            label1.Text = "PICTURES";
            label1.Click += label1_Click;
            // 
            // flat
            // 
            flat.BackColor = Color.FromArgb(10, 78, 110);
            flat.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            flat.ForeColor = Color.WhiteSmoke;
            flat.Location = new Point(21, 353);
            flat.Margin = new Padding(3, 2, 3, 2);
            flat.Name = "flat";
            flat.Size = new Size(151, 46);
            flat.TabIndex = 32;
            flat.Text = "FLAT";
            flat.UseVisualStyleBackColor = false;
            flat.Click += flat_Click;
            // 
            // EVENT
            // 
            EVENT.BackColor = Color.FromArgb(10, 78, 110);
            EVENT.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            EVENT.ForeColor = Color.WhiteSmoke;
            EVENT.Location = new Point(21, 442);
            EVENT.Margin = new Padding(3, 2, 3, 2);
            EVENT.Name = "EVENT";
            EVENT.Size = new Size(151, 46);
            EVENT.TabIndex = 33;
            EVENT.Text = "EVENT";
            EVENT.UseVisualStyleBackColor = false;
            EVENT.Click += EVENT_Click;
            // 
            // LOCATION
            // 
            LOCATION.BackColor = Color.FromArgb(10, 78, 110);
            LOCATION.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LOCATION.ForeColor = Color.WhiteSmoke;
            LOCATION.Location = new Point(21, 525);
            LOCATION.Margin = new Padding(3, 2, 3, 2);
            LOCATION.Name = "LOCATION";
            LOCATION.Size = new Size(151, 46);
            LOCATION.TabIndex = 34;
            LOCATION.Text = "LOCATION";
            LOCATION.UseVisualStyleBackColor = false;
            LOCATION.Click += LOCATION_Click;
            // 
            // PERSON
            // 
            PERSON.BackColor = Color.FromArgb(10, 78, 110);
            PERSON.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PERSON.ForeColor = Color.WhiteSmoke;
            PERSON.Location = new Point(21, 598);
            PERSON.Margin = new Padding(3, 2, 3, 2);
            PERSON.Name = "PERSON";
            PERSON.Size = new Size(151, 46);
            PERSON.TabIndex = 35;
            PERSON.Text = "PERSON";
            PERSON.UseVisualStyleBackColor = false;
            PERSON.Click += PERSON_Click;
            // 
            // date
            // 
            date.BackColor = Color.FromArgb(10, 78, 110);
            date.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            date.ForeColor = Color.WhiteSmoke;
            date.Location = new Point(21, 687);
            date.Margin = new Padding(3, 2, 3, 2);
            date.Name = "date";
            date.Size = new Size(151, 46);
            date.TabIndex = 36;
            date.Text = "DATE";
            date.UseVisualStyleBackColor = false;
            date.Click += date_Click;
            // 
            // AlbumView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1603, 760);
            Controls.Add(date);
            Controls.Add(PERSON);
            Controls.Add(LOCATION);
            Controls.Add(EVENT);
            Controls.Add(flat);
            Controls.Add(panel1);
            Controls.Add(button7);
            Controls.Add(flowLayoutPanel1);
            ForeColor = Color.Black;
            Name = "AlbumView";
            Text = "AlbumView";
            Load += AlbumView_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new ALBUM();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        //private Button button8;
        //private Button button1;
        //private Button button6;
        //private Button button5;
        //private Button button4;
        private Button button7;
        private Panel panel1;
        private Label label1;
        private LinkLabel linkLabel1;
        private Button button10;
        private Button button1;
        private Button button2;
        private Button flat;
        private Button EVENT;
        private Button LOCATION;
        private Button PERSON;
        private Button date;
    }
}