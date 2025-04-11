namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            close_btn = new Button();
            server_ip_text = new TextBox();
            trust = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            server_ip_label = new Label();
            statusStrip1 = new StatusStrip();
            connected_status = new ToolStripStatusLabel();
            disconnected_status = new ToolStripStatusLabel();
            daemon_checkbox = new CheckBox();
            save_config_button = new Button();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // close_btn
            // 
            close_btn.Location = new Point(524, 154);
            close_btn.Name = "close_btn";
            close_btn.Size = new Size(117, 47);
            close_btn.TabIndex = 0;
            close_btn.Text = "Close";
            close_btn.UseVisualStyleBackColor = true;
            close_btn.Click += close_btn_click;
            // 
            // server_ip_text
            // 
            server_ip_text.Location = new Point(103, 15);
            server_ip_text.Name = "server_ip_text";
            server_ip_text.Size = new Size(188, 31);
            server_ip_text.TabIndex = 1;
            // 
            // trust
            // 
            trust.Location = new Point(297, 12);
            trust.Name = "trust";
            trust.Size = new Size(138, 39);
            trust.TabIndex = 2;
            trust.Text = "Connect";
            trust.UseVisualStyleBackColor = true;
            trust.Click += trust_click;
            // 
            // server_ip_label
            // 
            server_ip_label.AutoSize = true;
            server_ip_label.Location = new Point(12, 18);
            server_ip_label.Name = "server_ip_label";
            server_ip_label.Size = new Size(85, 25);
            server_ip_label.TabIndex = 3;
            server_ip_label.Text = "Server IP:";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { connected_status, disconnected_status });
            statusStrip1.Location = new Point(0, 222);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(653, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "connection_status";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // connected_status
            // 
            connected_status.BackColor = Color.ForestGreen;
            connected_status.ImageTransparentColor = Color.Lime;
            connected_status.LinkColor = Color.Lime;
            connected_status.Name = "connected_status";
            connected_status.Size = new Size(319, 25);
            connected_status.Spring = true;
            connected_status.Text = "Connected";
            connected_status.Visible = false;
            connected_status.VisitedLinkColor = Color.Lime;
            // 
            // disconnected_status
            // 
            disconnected_status.BackColor = Color.Red;
            disconnected_status.Name = "disconnected_status";
            disconnected_status.Size = new Size(638, 25);
            disconnected_status.Spring = true;
            disconnected_status.Text = "Disconnected";
            disconnected_status.Visible = false;
            // 
            // daemon_checkbox
            // 
            daemon_checkbox.AutoSize = true;
            daemon_checkbox.Location = new Point(12, 172);
            daemon_checkbox.Name = "daemon_checkbox";
            daemon_checkbox.Size = new Size(162, 29);
            daemon_checkbox.TabIndex = 5;
            daemon_checkbox.Text = "Run as daemon";
            daemon_checkbox.UseVisualStyleBackColor = true;
            daemon_checkbox.CheckedChanged += daemon_checkbox_CheckedChanged;
            // 
            // save_config
            // 
            save_config_button.Location = new Point(529, 82);
            save_config_button.Name = "save_config_button";
            save_config_button.Size = new Size(112, 34);
            save_config_button.TabIndex = 6;
            save_config_button.Text = "Save config";
            save_config_button.UseVisualStyleBackColor = true;
            save_config_button.Click += this.save_config_button_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(653, 244);
            Controls.Add(save_config_button);
            Controls.Add(daemon_checkbox);
            Controls.Add(statusStrip1);
            Controls.Add(server_ip_label);
            Controls.Add(trust);
            Controls.Add(server_ip_text);
            Controls.Add(close_btn);
            Name = "Form1";
            Text = "Client";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button close_btn;
        private TextBox server_ip_text;
        private Button trust;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label server_ip_label;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel connected_status;
        private ToolStripStatusLabel disconnected_status;
        private CheckBox daemon_checkbox;
        private Button save_config_button;
    }
}
