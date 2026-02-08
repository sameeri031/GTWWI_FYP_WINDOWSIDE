namespace Task1
{
    partial class CREATE_ACCOUNT
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
            panel1 = new Panel();
            label1 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            button2 = new Button();
            label5 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            tbemail = new TextBox();
            tbname = new TextBox();
            button1 = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1721, 88);
            panel1.TabIndex = 20;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(0, 28);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(149, 38);
            label1.TabIndex = 0;
            label1.Text = "ACCOUNT";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Silver;
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(tbemail);
            groupBox1.Controls.Add(tbname);
            groupBox1.Location = new Point(550, 300);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(777, 445);
            groupBox1.TabIndex = 28;
            groupBox1.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.DarkGreen;
            label6.Location = new Point(52, 274);
            label6.Name = "label6";
            label6.Size = new Size(578, 25);
            label6.TabIndex = 35;
            label6.Text = "Password must be at least 6 characters and contain letters and numbers";
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(10, 78, 110);
            button2.ForeColor = SystemColors.ButtonFace;
            button2.Location = new Point(335, 373);
            button2.Name = "button2";
            button2.Size = new Size(105, 38);
            button2.TabIndex = 34;
            button2.Text = "CREATE";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(52, 311);
            label5.Name = "label5";
            label5.Size = new Size(159, 32);
            label5.TabIndex = 33;
            label5.Text = "C PASSWORD";
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.ScrollBar;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(255, 311);
            textBox2.Margin = new Padding(3, 2, 3, 2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(456, 39);
            textBox2.TabIndex = 32;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(52, 224);
            label4.Name = "label4";
            label4.Size = new Size(137, 32);
            label4.TabIndex = 31;
            label4.Text = "PASSWORD";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ScrollBar;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(255, 224);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(456, 39);
            textBox1.TabIndex = 30;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(52, 150);
            label3.Name = "label3";
            label3.Size = new Size(80, 32);
            label3.TabIndex = 28;
            label3.Text = "EMAIL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(52, 75);
            label2.Name = "label2";
            label2.Size = new Size(143, 32);
            label2.TabIndex = 27;
            label2.Text = "USER NAME";
            // 
            // tbemail
            // 
            tbemail.BackColor = SystemColors.ScrollBar;
            tbemail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbemail.Location = new Point(255, 147);
            tbemail.Margin = new Padding(3, 2, 3, 2);
            tbemail.Name = "tbemail";
            tbemail.Size = new Size(456, 39);
            tbemail.TabIndex = 26;
            // 
            // tbname
            // 
            tbname.BackColor = SystemColors.ScrollBar;
            tbname.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbname.Location = new Point(255, 68);
            tbname.Margin = new Padding(3, 2, 3, 2);
            tbname.Name = "tbname";
            tbname.Size = new Size(456, 39);
            tbname.TabIndex = 25;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 78, 110);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(24, 271);
            button1.Name = "button1";
            button1.Size = new Size(168, 57);
            button1.TabIndex = 35;
            button1.Text = "MANAGE ACCES";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // CREATE_ACCOUNT
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1721, 834);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "CREATE_ACCOUNT";
            Text = "CREATE_ACCOUNT";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private GroupBox groupBox1;
        private Label label5;
        private TextBox textBox2;
        private Label label4;
        private TextBox textBox1;
        private Label label3;
        private Label label2;
        private TextBox tbemail;
        private TextBox tbname;
        private Button button2;
        private Label label6;
        private Button button1;
    }
}