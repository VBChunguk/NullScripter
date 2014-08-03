namespace NullScripter.GUI
{
    partial class Setting
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
            this.FontName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FontSize = new System.Windows.Forms.TextBox();
            this.FontSetting = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancle = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FontName
            // 
            this.FontName.Location = new System.Drawing.Point(6, 20);
            this.FontName.Name = "FontName";
            this.FontName.ReadOnly = true;
            this.FontName.Size = new System.Drawing.Size(181, 21);
            this.FontName.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FontSize);
            this.groupBox1.Controls.Add(this.FontSetting);
            this.groupBox1.Controls.Add(this.FontName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font";
            // 
            // FontSize
            // 
            this.FontSize.Location = new System.Drawing.Point(6, 47);
            this.FontSize.Name = "FontSize";
            this.FontSize.ReadOnly = true;
            this.FontSize.Size = new System.Drawing.Size(93, 21);
            this.FontSize.TabIndex = 2;
            // 
            // FontSetting
            // 
            this.FontSetting.Location = new System.Drawing.Point(105, 46);
            this.FontSetting.Name = "FontSetting";
            this.FontSetting.Size = new System.Drawing.Size(82, 21);
            this.FontSetting.TabIndex = 1;
            this.FontSetting.Text = "Setting";
            this.FontSetting.UseVisualStyleBackColor = true;
            this.FontSetting.Click += new System.EventHandler(this.FontSetting_Click);
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(18, 104);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(82, 21);
            this.Confirm.TabIndex = 2;
            this.Confirm.Text = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Cancle
            // 
            this.Cancle.Location = new System.Drawing.Point(117, 104);
            this.Cancle.Name = "Cancle";
            this.Cancle.Size = new System.Drawing.Size(82, 21);
            this.Cancle.TabIndex = 3;
            this.Cancle.Text = "Cancle";
            this.Cancle.UseVisualStyleBackColor = true;
            this.Cancle.Click += new System.EventHandler(this.Cancle_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 137);
            this.Controls.Add(this.Cancle);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Setting";
            this.Text = "Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox FontName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox FontSize;
        private System.Windows.Forms.Button FontSetting;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancle;
    }
}