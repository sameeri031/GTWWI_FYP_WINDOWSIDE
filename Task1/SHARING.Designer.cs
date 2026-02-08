namespace Task1
{
    partial class SHARING
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
            label5 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button10 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(10, 78, 110);
            panel1.Controls.Add(label5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1825, 88);
            panel1.TabIndex = 34;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(23, 28);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(143, 38);
            label5.TabIndex = 0;
            label5.Text = "SHARING";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Silver;
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            flowLayoutPanel1.ForeColor = Color.FromArgb(148, 163, 184);
            flowLayoutPanel1.Location = new Point(95, 168);
            flowLayoutPanel1.Margin = new Padding(30, 30, 0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(2);
            flowLayoutPanel1.Size = new Size(1707, 683);
            flowLayoutPanel1.TabIndex = 35;
            // 
            // button10
            // 
            button10.BackColor = Color.FromArgb(10, 78, 110);
            button10.ForeColor = SystemColors.ButtonFace;
            button10.Location = new Point(958, 864);
            button10.Name = "button10";
            button10.Size = new Size(137, 61);
            button10.TabIndex = 36;
            button10.Text = "SHARE";
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // SHARING
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(193, 196, 197);
            ClientSize = new Size(1825, 1017);
            Controls.Add(button10);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Name = "SHARING";
            Text = "SHARING";
            WindowState = FormWindowState.Maximized;
            Load += SHARING_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label5;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button10;
    }
}