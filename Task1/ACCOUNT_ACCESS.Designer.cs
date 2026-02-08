namespace Task1
{
    partial class ACCOUNT_ACCESS
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
            groupBox1 = new GroupBox();
            button9 = new Button();
            label3 = new Label();
            comboBox1 = new ComboBox();
            label6 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1 = new Panel();
            linkLabel1 = new LinkLabel();
            label1 = new Label();
            button1 = new Button();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Silver;
            groupBox1.Controls.Add(button9);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(458, 168);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(760, 218);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Enter += groupBox1_Enter;
            // 
            // button9
            // 
            button9.BackColor = Color.FromArgb(10, 78, 110);
            button9.ForeColor = SystemColors.ButtonFace;
            button9.Location = new Point(305, 162);
            button9.Name = "button9";
            button9.Size = new Size(151, 38);
            button9.TabIndex = 50;
            button9.Text = "CREATE NEW";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(34, 98);
            label3.Name = "label3";
            label3.Size = new Size(217, 32);
            label3.TabIndex = 49;
            label3.Text = "SELECT ACCOUNT: ";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(284, 98);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(446, 33);
            comboBox1.TabIndex = 48;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.DarkGreen;
            label6.Location = new Point(145, 27);
            label6.Name = "label6";
            label6.Size = new Size(455, 25);
            label6.TabIndex = 30;
            label6.Text = "IF YOU WANT TO ADD COMMON LOCATION OR EVENT";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.Location = new Point(126, 406);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1602, 452);
            flowLayoutPanel1.TabIndex = 29;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1818, 88);
            panel1.TabIndex = 30;
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
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(0, 28);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(218, 38);
            label1.TabIndex = 0;
            label1.Text = "Account Access";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 78, 110);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(1646, 893);
            button1.Name = "button1";
            button1.Size = new Size(119, 38);
            button1.TabIndex = 51;
            button1.Text = "DONE";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // ACCOUNT_ACCESS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1818, 943);
            Controls.Add(button1);
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(groupBox1);
            Name = "ACCOUNT_ACCESS";
            Text = "ACCOUNT_ACCESS";
            WindowState = FormWindowState.Maximized;
            Load += ACCOUNT_ACCESS_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private LinkLabel linkLabel1;
        private Label label1;
        private Label label6;
        private ComboBox comboBox1;
        private Label label3;
        private Button button9;
        private Button button1;
    }
}