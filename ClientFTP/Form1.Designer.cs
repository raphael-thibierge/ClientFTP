namespace ClientFTP
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Host_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Port_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserID_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.Connect_button = new System.Windows.Forms.Button();
            this.ConnexionState_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Directories_listBox = new System.Windows.Forms.ListBox();
            this.Files_listBox = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Host_textBox
            // 
            this.Host_textBox.Location = new System.Drawing.Point(12, 25);
            this.Host_textBox.Name = "Host_textBox";
            this.Host_textBox.Size = new System.Drawing.Size(100, 20);
            this.Host_textBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port";
            // 
            // Port_textBox
            // 
            this.Port_textBox.Location = new System.Drawing.Point(12, 64);
            this.Port_textBox.Name = "Port_textBox";
            this.Port_textBox.Size = new System.Drawing.Size(100, 20);
            this.Port_textBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User";
            // 
            // UserID_textBox
            // 
            this.UserID_textBox.Location = new System.Drawing.Point(12, 104);
            this.UserID_textBox.Name = "UserID_textBox";
            this.UserID_textBox.Size = new System.Drawing.Size(100, 20);
            this.UserID_textBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password";
            // 
            // Password_textBox
            // 
            this.Password_textBox.Location = new System.Drawing.Point(12, 140);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.Size = new System.Drawing.Size(100, 20);
            this.Password_textBox.TabIndex = 6;
            // 
            // Connect_button
            // 
            this.Connect_button.Location = new System.Drawing.Point(23, 166);
            this.Connect_button.Name = "Connect_button";
            this.Connect_button.Size = new System.Drawing.Size(75, 23);
            this.Connect_button.TabIndex = 8;
            this.Connect_button.Text = "Connect";
            this.Connect_button.UseVisualStyleBackColor = true;
            this.Connect_button.Click += new System.EventHandler(this.Connect_button_Click);
            // 
            // ConnexionState_label
            // 
            this.ConnexionState_label.AutoSize = true;
            this.ConnexionState_label.Location = new System.Drawing.Point(9, 194);
            this.ConnexionState_label.Name = "ConnexionState_label";
            this.ConnexionState_label.Size = new System.Drawing.Size(0, 13);
            this.ConnexionState_label.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(157, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Directories list";
            // 
            // Directories_listBox
            // 
            this.Directories_listBox.FormattingEnabled = true;
            this.Directories_listBox.Location = new System.Drawing.Point(160, 25);
            this.Directories_listBox.Name = "Directories_listBox";
            this.Directories_listBox.Size = new System.Drawing.Size(280, 95);
            this.Directories_listBox.TabIndex = 11;
            this.Directories_listBox.SelectedIndexChanged += new System.EventHandler(this.Directories_listBox_SelectedIndexChanged);
            // 
            // Files_listBox
            // 
            this.Files_listBox.FormattingEnabled = true;
            this.Files_listBox.Location = new System.Drawing.Point(160, 140);
            this.Files_listBox.Name = "Files_listBox";
            this.Files_listBox.Size = new System.Drawing.Size(280, 95);
            this.Files_listBox.TabIndex = 13;
            this.Files_listBox.SelectedIndexChanged += new System.EventHandler(this.Files_listBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(157, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Files list";
            // 
            // Refresh_button
            // 
            this.Refresh_button.Location = new System.Drawing.Point(23, 212);
            this.Refresh_button.Name = "Refresh_button";
            this.Refresh_button.Size = new System.Drawing.Size(75, 23);
            this.Refresh_button.TabIndex = 14;
            this.Refresh_button.Text = "Refresh";
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 255);
            this.Controls.Add(this.Refresh_button);
            this.Controls.Add(this.Files_listBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Directories_listBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ConnexionState_label);
            this.Controls.Add(this.Connect_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Password_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UserID_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Port_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Host_textBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Host_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Port_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UserID_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Button Connect_button;
        private System.Windows.Forms.Label ConnexionState_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox Directories_listBox;
        private System.Windows.Forms.ListBox Files_listBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Refresh_button;
    }
}

