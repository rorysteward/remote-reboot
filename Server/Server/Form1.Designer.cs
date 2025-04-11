namespace Server
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
            close_button = new Button();
            server_ip_label = new Label();
            reset_button = new Button();
            shutdown_button = new Button();
            include_this_checkbox = new CheckBox();
            server_ip_text = new TextBox();
            start_stop_button = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            list_of_pc = new DataGridView();
            PCName = new DataGridViewTextBoxColumn();
            IP = new DataGridViewTextBoxColumn();
            save_button = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)list_of_pc).BeginInit();
            SuspendLayout();
            // 
            // close_button
            // 
            close_button.Location = new Point(251, 243);
            close_button.Name = "close_button";
            close_button.Size = new Size(112, 34);
            close_button.TabIndex = 0;
            close_button.Text = "Close";
            close_button.UseVisualStyleBackColor = true;
            close_button.Click += close_button_Click;
            // 
            // server_ip_label
            // 
            server_ip_label.AutoSize = true;
            server_ip_label.Location = new Point(0, 19);
            server_ip_label.Name = "server_ip_label";
            server_ip_label.Size = new Size(81, 25);
            server_ip_label.TabIndex = 2;
            server_ip_label.Text = "Server IP";
            // 
            // reset_button
            // 
            reset_button.Location = new Point(9, 3);
            reset_button.Name = "reset_button";
            reset_button.Size = new Size(112, 76);
            reset_button.TabIndex = 6;
            reset_button.Text = "Restart";
            reset_button.UseVisualStyleBackColor = true;
            reset_button.Click += reset_button_Click;
            // 
            // shutdown_button
            // 
            shutdown_button.Location = new Point(127, 3);
            shutdown_button.Name = "shutdown_button";
            shutdown_button.Size = new Size(112, 76);
            shutdown_button.TabIndex = 7;
            shutdown_button.Text = "Shutdown";
            shutdown_button.UseVisualStyleBackColor = true;
            shutdown_button.Click += shutdown_button_Click;
            // 
            // include_this_checkbox
            // 
            include_this_checkbox.AutoSize = true;
            include_this_checkbox.Location = new Point(12, 243);
            include_this_checkbox.Name = "include_this_checkbox";
            include_this_checkbox.Size = new Size(210, 29);
            include_this_checkbox.TabIndex = 8;
            include_this_checkbox.Text = "Take action on this PC";
            include_this_checkbox.UseVisualStyleBackColor = true;
            include_this_checkbox.CheckedChanged += include_this_pc_CheckedChanged;
            // 
            // server_ip_text
            // 
            server_ip_text.Location = new Point(78, 16);
            server_ip_text.Name = "server_ip_text";
            server_ip_text.Size = new Size(167, 31);
            server_ip_text.TabIndex = 9;
            // 
            // start_stop_button
            // 
            start_stop_button.Location = new Point(251, 12);
            start_stop_button.Name = "start_stop_button";
            start_stop_button.Size = new Size(112, 34);
            start_stop_button.TabIndex = 10;
            start_stop_button.Text = "Start";
            start_stop_button.UseVisualStyleBackColor = true;
            start_stop_button.Click += start_stop_button_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(reset_button);
            panel1.Controls.Add(shutdown_button);
            panel1.Location = new Point(55, 120);
            panel1.Name = "panel1";
            panel1.Size = new Size(242, 91);
            panel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.Controls.Add(list_of_pc);
            panel2.Location = new Point(369, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(332, 233);
            panel2.TabIndex = 12;
            // 
            // list_of_pc
            // 
            list_of_pc.AllowUserToAddRows = false;
            list_of_pc.AllowUserToDeleteRows = false;
            list_of_pc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            list_of_pc.Columns.AddRange(new DataGridViewColumn[] { PCName, IP });
            list_of_pc.Location = new Point(18, 12);
            list_of_pc.Name = "list_of_pc";
            list_of_pc.ReadOnly = true;
            list_of_pc.RowHeadersVisible = false;
            list_of_pc.RowHeadersWidth = 62;
            list_of_pc.Size = new Size(387, 253);
            list_of_pc.TabIndex = 0;
            // 
            // PCName
            // 
            PCName.HeaderText = "PC Name";
            PCName.MinimumWidth = 8;
            PCName.Name = "PCName";
            PCName.ReadOnly = true;
            PCName.Resizable = DataGridViewTriState.False;
            PCName.Width = 175;
            // 
            // IP
            // 
            IP.HeaderText = "IP";
            IP.MinimumWidth = 8;
            IP.Name = "IP";
            IP.ReadOnly = true;
            IP.Resizable = DataGridViewTriState.False;
            IP.Width = 175;
            // 
            // save_button
            // 
            save_button.Location = new Point(251, 54);
            save_button.Name = "save_button";
            save_button.Size = new Size(112, 34);
            save_button.TabIndex = 13;
            save_button.Text = "Save config";
            save_button.UseVisualStyleBackColor = true;
            save_button.Click += save_button_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(703, 286);
            Controls.Add(save_button);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(start_stop_button);
            Controls.Add(server_ip_text);
            Controls.Add(include_this_checkbox);
            Controls.Add(server_ip_label);
            Controls.Add(close_button);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Server";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)list_of_pc).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button close_button;
        private Label server_ip_label;
        private Button reset_button;
        private Button shutdown_button;
        private CheckBox include_this_checkbox;
        private TextBox server_ip_text;
        private Button start_stop_button;
        private Panel panel1;
        private Panel panel2;
        private DataGridView list_of_pc;
        private Button save_button;
        private DataGridViewTextBoxColumn PCName;
        private DataGridViewTextBoxColumn IP;
    }
}
