namespace Task1
{
    partial class ALBUM
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
            button12 = new Button();
            button10 = new Button();
            button9 = new Button();
            linkLabel1 = new LinkLabel();
            groupBox1 = new GroupBox();
            radioButton1 = new RadioButton();
            label2 = new Label();
            radioButton2 = new RadioButton();
            button2 = new Button();
            label1 = new Label();
            button8 = new Button();
            button1 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button11 = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            flowLayoutPanel1.ForeColor = Color.FromArgb(148, 163, 184);
            flowLayoutPanel1.Location = new Point(192, 122);
            flowLayoutPanel1.Margin = new Padding(30, 30, 0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.Size = new Size(1707, 837);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(10, 78, 110);
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ForeColor = Color.Snow;
            button7.Location = new Point(12, 759);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(135, 46);
            button7.TabIndex = 18;
            button7.Text = "BACK";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(button12);
            panel1.Controls.Add(button10);
            panel1.Controls.Add(button9);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1827, 88);
            panel1.TabIndex = 19;
            // 
            // button12
            // 
            button12.BackColor = Color.FromArgb(10, 78, 110);
            button12.ForeColor = SystemColors.ButtonFace;
            button12.Location = new Point(1521, 33);
            button12.Name = "button12";
            button12.Size = new Size(105, 37);
            button12.TabIndex = 30;
            button12.Text = "ACCESS";
            button12.UseVisualStyleBackColor = false;
            button12.Click += button12_Click;
            // 
            // button10
            // 
            button10.BackColor = Color.FromArgb(10, 78, 110);
            button10.ForeColor = SystemColors.ButtonFace;
            button10.Location = new Point(1384, 33);
            button10.Name = "button10";
            button10.Size = new Size(105, 37);
            button10.TabIndex = 29;
            button10.Text = "SHARE";
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.FromArgb(10, 78, 110);
            button9.ForeColor = SystemColors.ButtonFace;
            button9.Location = new Point(1170, 32);
            button9.Name = "button9";
            button9.Size = new Size(193, 38);
            button9.TabIndex = 28;
            button9.Text = "GIVE COMMON TAG";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.Chartreuse;
            linkLabel1.Location = new Point(1680, 35);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(63, 28);
            linkLabel1.TabIndex = 27;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "SYNC";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked_1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(button2);
            groupBox1.Location = new Point(477, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(630, 73);
            groupBox1.TabIndex = 26;
            groupBox1.TabStop = false;
            groupBox1.Visible = false;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.ForeColor = SystemColors.ButtonFace;
            radioButton1.Location = new Point(156, 24);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(79, 29);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "YEAR";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(23, 19);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(102, 38);
            label2.TabIndex = 1;
            label2.Text = "FILTER";
            label2.Visible = false;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.ForeColor = SystemColors.ButtonFace;
            radioButton2.Location = new Point(271, 25);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(102, 29);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "MONTH";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(10, 78, 110);
            button2.ForeColor = SystemColors.ButtonFace;
            button2.Location = new Point(450, 19);
            button2.Name = "button2";
            button2.Size = new Size(105, 38);
            button2.TabIndex = 2;
            button2.Text = "LOAD";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(0, 28);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(116, 38);
            label1.TabIndex = 0;
            label1.Text = "PEOPLE";
            // 
            // button8
            // 
            button8.BackColor = Color.FromArgb(10, 78, 110);
            button8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button8.ForeColor = Color.Snow;
            button8.Location = new Point(12, 512);
            button8.Margin = new Padding(3, 2, 3, 2);
            button8.Name = "button8";
            button8.Size = new Size(135, 46);
            button8.TabIndex = 25;
            button8.Text = "SEARCH";
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 78, 110);
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Snow;
            button1.Location = new Point(12, 429);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(135, 46);
            button1.TabIndex = 24;
            button1.Text = "DATE";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(10, 78, 110);
            button6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button6.ForeColor = Color.Snow;
            button6.Location = new Point(12, 337);
            button6.Margin = new Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new Size(135, 46);
            button6.TabIndex = 23;
            button6.Text = "EVENT";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(10, 78, 110);
            button5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button5.ForeColor = Color.Snow;
            button5.Location = new Point(12, 245);
            button5.Margin = new Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new Size(135, 46);
            button5.TabIndex = 22;
            button5.Text = "LOCATION";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(10, 78, 110);
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button4.ForeColor = Color.Snow;
            button4.Location = new Point(12, 156);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(135, 46);
            button4.TabIndex = 21;
            button4.Text = "PERSON";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(10, 78, 110);
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.Snow;
            button3.Location = new Point(12, 597);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(135, 46);
            button3.TabIndex = 26;
            button3.Text = "UPLOAD";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button11
            // 
            button11.BackColor = Color.FromArgb(10, 78, 110);
            button11.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button11.ForeColor = Color.Snow;
            button11.Location = new Point(12, 679);
            button11.Margin = new Padding(3, 2, 3, 2);
            button11.Name = "button11";
            button11.Size = new Size(135, 46);
            button11.TabIndex = 27;
            button11.Text = "ACCOUNTS";
            button11.UseVisualStyleBackColor = false;
            button11.Click += button11_Click;
            // 
            // ALBUM
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1827, 980);
            Controls.Add(button11);
            Controls.Add(button3);
            Controls.Add(button8);
            Controls.Add(button1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(panel1);
            Controls.Add(button7);
            Controls.Add(flowLayoutPanel1);
            Name = "ALBUM";
            Text = "people";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button button7;
        private Panel panel1;
        private Label label1;
        private Button button8;
        private Button button1;
        private Button button6;
        private Button button5;
        private Button button4;
        private RadioButton radioButton1;
        private Button button2;
        private RadioButton radioButton2;
        private GroupBox groupBox1;
        private Label label2;
        private Button button3;
        private LinkLabel linkLabel1;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
    }
}