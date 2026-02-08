namespace Task1
{
    partial class SEARCH
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
            textBox1 = new TextBox();
            button7 = new Button();
            PEOPLE = new Button();
            EVENT = new Button();
            location = new Button();
            button1 = new Button();
            panel1 = new Panel();
            button2 = new Button();
            linkLabel1 = new LinkLabel();
            label1 = new Label();
            label2 = new Label();
            label7 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            flowLayoutPanel1.Location = new Point(181, 189);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1707, 683);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(679, 28);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(255, 45);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(10, 78, 110);
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ForeColor = SystemColors.ButtonFace;
            button7.Location = new Point(969, 27);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(112, 46);
            button7.TabIndex = 19;
            button7.Text = "SEARCH";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // PEOPLE
            // 
            PEOPLE.BackColor = Color.FromArgb(10, 78, 110);
            PEOPLE.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PEOPLE.ForeColor = SystemColors.ButtonFace;
            PEOPLE.Location = new Point(560, 137);
            PEOPLE.Margin = new Padding(3, 2, 3, 2);
            PEOPLE.Name = "PEOPLE";
            PEOPLE.Size = new Size(112, 46);
            PEOPLE.TabIndex = 20;
            PEOPLE.Text = "PEOPLE";
            PEOPLE.UseVisualStyleBackColor = false;
            PEOPLE.Click += PEOPLE_Click;
            // 
            // EVENT
            // 
            EVENT.BackColor = Color.FromArgb(10, 78, 110);
            EVENT.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            EVENT.ForeColor = SystemColors.ButtonFace;
            EVENT.Location = new Point(745, 137);
            EVENT.Margin = new Padding(3, 2, 3, 2);
            EVENT.Name = "EVENT";
            EVENT.Size = new Size(112, 46);
            EVENT.TabIndex = 21;
            EVENT.Text = "EVENT";
            EVENT.UseVisualStyleBackColor = false;
            EVENT.Click += EVENT_Click;
            // 
            // location
            // 
            location.BackColor = Color.FromArgb(10, 78, 110);
            location.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            location.ForeColor = SystemColors.ButtonFace;
            location.Location = new Point(938, 137);
            location.Margin = new Padding(3, 2, 3, 2);
            location.Name = "location";
            location.Size = new Size(143, 46);
            location.TabIndex = 22;
            location.Text = "LOCATION";
            location.UseVisualStyleBackColor = false;
            location.Click += location_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 78, 110);
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(23, 223);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(112, 46);
            button1.TabIndex = 24;
            button1.Text = "BACK";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button7);
            panel1.Controls.Add(textBox1);
            panel1.Dock = DockStyle.Top;
            panel1.ForeColor = SystemColors.ButtonFace;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1924, 88);
            panel1.TabIndex = 44;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(10, 78, 110);
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = SystemColors.ButtonFace;
            button2.Location = new Point(1142, 26);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(221, 46);
            button2.TabIndex = 29;
            button2.Text = "ADVANCE SEARCH";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.Chartreuse;
            linkLabel1.Location = new Point(1727, 36);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(63, 28);
            linkLabel1.TabIndex = 28;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "SYNC";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(0, 28);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(124, 38);
            label1.TabIndex = 0;
            label1.Text = "SEARCH";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.DarkGreen;
            label2.Location = new Point(408, 139);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(102, 38);
            label2.TabIndex = 20;
            label2.Text = "FILTER";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(696, 92);
            label7.Name = "label7";
            label7.Size = new Size(206, 25);
            label7.TabIndex = 45;
            label7.Text = "SHOWING ALL RESULT";
            label7.Click += label7_Click;
            // 
            // SEARCH
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1924, 927);
            Controls.Add(label7);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(button1);
            Controls.Add(location);
            Controls.Add(EVENT);
            Controls.Add(PEOPLE);
            Controls.Add(flowLayoutPanel1);
            Name = "SEARCH";
            Text = "SEARCH";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox1;
        private Button button7;
        private Button PEOPLE;
        private Button EVENT;
        private Button location;
        private Button button1;
        private Panel panel1;
        private Label label1;
        //private Button button8;
        //private Button button2;
        //private Button button6;
        //private Button button5;
        //private Button button3;
        private Label label2;
        private LinkLabel linkLabel1;
        private Label label7;
        private Button button2;
    }
}