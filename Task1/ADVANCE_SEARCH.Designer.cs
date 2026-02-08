namespace Task1
{
    partial class ADVANCE_SEARCH
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
            linkLabel1 = new LinkLabel();
            label5 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label4 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            checkedListBox1 = new CheckedListBox();
            button7 = new Button();
            dateTimePicker2 = new DateTimePicker();
            label6 = new Label();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            label7 = new Label();
            personPopup = new ListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(label5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1877, 88);
            panel1.TabIndex = 33;
            panel1.Paint += panel1_Paint;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.Chartreuse;
            linkLabel1.Location = new Point(1485, 36);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(63, 28);
            linkLabel1.TabIndex = 28;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "SYNC";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(0, 28);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(265, 38);
            label5.TabIndex = 0;
            label5.Text = "ADVANCE SEARCH";
            label5.Click += label5_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.Location = new Point(820, 126);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1007, 755);
            flowLayoutPanel1.TabIndex = 34;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(69, 639);
            label4.Name = "label4";
            label4.Size = new Size(141, 32);
            label4.TabIndex = 40;
            label4.Text = "DATE FROM";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CalendarMonthBackground = SystemColors.ScrollBar;
            dateTimePicker1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateTimePicker1.Location = new Point(274, 639);
            dateTimePicker1.Margin = new Padding(3, 2, 3, 2);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(446, 39);
            dateTimePicker1.TabIndex = 39;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(85, 453);
            label3.Name = "label3";
            label3.Size = new Size(84, 32);
            label3.TabIndex = 38;
            label3.Text = "EVENT";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(85, 552);
            label2.Name = "label2";
            label2.Size = new Size(125, 32);
            label2.TabIndex = 37;
            label2.Text = "LOCATION";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(72, 181);
            label1.Name = "label1";
            label1.Size = new Size(102, 32);
            label1.TabIndex = 35;
            label1.Text = "PERSON";
            // 
            // checkedListBox1
            // 
            checkedListBox1.BackColor = Color.Silver;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(338, 288);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(196, 144);
            checkedListBox1.TabIndex = 43;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(10, 78, 110);
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ForeColor = SystemColors.ButtonFace;
            button7.Location = new Point(477, 835);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(112, 46);
            button7.TabIndex = 44;
            button7.Text = "SEARCH";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.CalendarMonthBackground = SystemColors.ScrollBar;
            dateTimePicker2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateTimePicker2.Location = new Point(274, 734);
            dateTimePicker2.Margin = new Padding(3, 2, 3, 2);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(446, 39);
            dateTimePicker2.TabIndex = 45;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(69, 734);
            label6.Name = "label6";
            label6.Size = new Size(105, 32);
            label6.TabIndex = 46;
            label6.Text = "DATE TO";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(274, 456);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(446, 33);
            comboBox1.TabIndex = 47;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.TextChanged += comboBox1_TextChanged;
            // 
            // comboBox2
            // 
            comboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.ItemHeight = 50;
            comboBox2.Location = new Point(286, 167);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(446, 56);
            comboBox2.TabIndex = 48;
            comboBox2.DrawItem += comboBox2_DrawItem;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox2.Click += comboBox2_Click;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(274, 551);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(446, 33);
            comboBox3.TabIndex = 49;
            comboBox3.TextChanged += comboBox3_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(72, 288);
            label7.Name = "label7";
            label7.Size = new Size(219, 32);
            label7.TabIndex = 50;
            label7.Text = "PERSON SELECTED:";
            // 
            // personPopup
            // 
            personPopup.FormattingEnabled = true;
            personPopup.ItemHeight = 25;
            personPopup.Location = new Point(286, 167);
            personPopup.Name = "personPopup";
            personPopup.Size = new Size(443, 79);
            personPopup.TabIndex = 51;
            personPopup.Visible = false;
            personPopup.Click += personPopup_Click;
            personPopup.SelectedIndexChanged += personPopup_SelectedIndexChanged;
            // 
            // ADVANCE_SEARCH
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1877, 1050);
            Controls.Add(comboBox2);
            Controls.Add(personPopup);
            Controls.Add(label7);
            Controls.Add(comboBox3);
            Controls.Add(comboBox1);
            Controls.Add(label6);
            Controls.Add(dateTimePicker2);
            Controls.Add(button7);
            Controls.Add(checkedListBox1);
            Controls.Add(label4);
            Controls.Add(dateTimePicker1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Name = "ADVANCE_SEARCH";
            Text = "ADVANCE_SEARCH";
            WindowState = FormWindowState.Maximized;
            Load += ADVANCE_SEARCH_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private LinkLabel linkLabel1;
        private Label label5;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label4;
        private DateTimePicker dateTimePicker1;
        private Label label3;
        private Label label2;
        private Label label1;
        private CheckedListBox checkedListBox1;
        private Button button7;
        private DateTimePicker dateTimePicker2;
        private Label label6;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Label label7;
        private ListBox personPopup;
    }
}