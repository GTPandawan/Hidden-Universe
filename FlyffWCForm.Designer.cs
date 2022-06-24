namespace HiddenUniverse_WebClient
{
    partial class FlyffWCForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlyffWCForm));
            this.autoHealBox = new System.Windows.Forms.CheckBox();
            this.autoHealTime = new System.Windows.Forms.ComboBox();
            this.autoBuffBox = new System.Windows.Forms.CheckBox();
            this.autoBuffList = new System.Windows.Forms.CheckedListBox();
            this.autoFollowBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // autoHealBox
            // 
            this.autoHealBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoHealBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.autoHealBox.AutoSize = true;
            this.autoHealBox.BackColor = System.Drawing.Color.Gray;
            this.autoHealBox.Enabled = false;
            this.autoHealBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.autoHealBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 21.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHealBox.Location = new System.Drawing.Point(1558, 346);
            this.autoHealBox.Name = "autoHealBox";
            this.autoHealBox.Size = new System.Drawing.Size(326, 75);
            this.autoHealBox.TabIndex = 0;
            this.autoHealBox.Text = "Auto Heal (C)";
            this.autoHealBox.UseVisualStyleBackColor = false;
            this.autoHealBox.Visible = false;
            this.autoHealBox.CheckStateChanged += new System.EventHandler(this.autoHealBox_CheckStateChanged);
            // 
            // autoHealTime
            // 
            this.autoHealTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoHealTime.BackColor = System.Drawing.Color.Gray;
            this.autoHealTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoHealTime.Enabled = false;
            this.autoHealTime.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 14.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHealTime.FormattingEnabled = true;
            this.autoHealTime.Items.AddRange(new object[] {
            "Every 1 second",
            "Every 2 seconds",
            "Every 3 seconds",
            "Every 4 seconds",
            "Every 5 seconds",
            "Every 6 seconds",
            "Every 7 seconds",
            "Every 8 seconds",
            "Every 9 seconds",
            "Every 10 seconds"});
            this.autoHealTime.Location = new System.Drawing.Point(1726, 427);
            this.autoHealTime.Name = "autoHealTime";
            this.autoHealTime.Size = new System.Drawing.Size(158, 46);
            this.autoHealTime.TabIndex = 1;
            this.autoHealTime.Visible = false;
            this.autoHealTime.SelectedIndexChanged += new System.EventHandler(this.autoHealTime_SelectedIndexChanged);
            // 
            // autoBuffBox
            // 
            this.autoBuffBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoBuffBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.autoBuffBox.AutoSize = true;
            this.autoBuffBox.BackColor = System.Drawing.Color.Gray;
            this.autoBuffBox.Enabled = false;
            this.autoBuffBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.autoBuffBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 21.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoBuffBox.Location = new System.Drawing.Point(1640, 479);
            this.autoBuffBox.Name = "autoBuffBox";
            this.autoBuffBox.Size = new System.Drawing.Size(244, 75);
            this.autoBuffBox.TabIndex = 2;
            this.autoBuffBox.Text = "Auto Buff";
            this.autoBuffBox.UseVisualStyleBackColor = false;
            this.autoBuffBox.Visible = false;
            this.autoBuffBox.Click += new System.EventHandler(this.autoBuffBox_Click);
            // 
            // autoBuffList
            // 
            this.autoBuffList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoBuffList.BackColor = System.Drawing.Color.PeachPuff;
            this.autoBuffList.CheckOnClick = true;
            this.autoBuffList.Enabled = false;
            this.autoBuffList.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoBuffList.FormattingEnabled = true;
            this.autoBuffList.Items.AddRange(new object[] {
            "[Taskbar Slot 0]",
            "[Taskbar Slot 1]",
            "[Taskbar Slot 2]",
            "[Taskbar Slot 3]",
            "[Taskbar Slot 4]",
            "[Taskbar Slot 5]",
            "[Taskbar Slot 6]",
            "[Taskbar Slot 7]",
            "[Taskbar Slot 8]",
            "[Taskbar Slot 9]"});
            this.autoBuffList.Location = new System.Drawing.Point(1693, 560);
            this.autoBuffList.Name = "autoBuffList";
            this.autoBuffList.Size = new System.Drawing.Size(191, 326);
            this.autoBuffList.TabIndex = 3;
            this.autoBuffList.TabStop = false;
            this.autoBuffList.UseCompatibleTextRendering = true;
            this.autoBuffList.UseTabStops = false;
            this.autoBuffList.Visible = false;
            this.autoBuffList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.autoBuffList_ItemCheck);
            // 
            // autoFollowBox
            // 
            this.autoFollowBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFollowBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.autoFollowBox.AutoSize = true;
            this.autoFollowBox.BackColor = System.Drawing.Color.Gray;
            this.autoFollowBox.Enabled = false;
            this.autoFollowBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.autoFollowBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 21.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoFollowBox.Location = new System.Drawing.Point(1521, 892);
            this.autoFollowBox.Name = "autoFollowBox";
            this.autoFollowBox.Size = new System.Drawing.Size(363, 75);
            this.autoFollowBox.TabIndex = 4;
            this.autoFollowBox.Text = "Auto Follow (Z)";
            this.autoFollowBox.UseVisualStyleBackColor = false;
            this.autoFollowBox.Visible = false;
            this.autoFollowBox.CheckStateChanged += new System.EventHandler(this.autoFollowBox_CheckStateChanged);
            // 
            // FlyffWCForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1896, 1016);
            this.Controls.Add(this.autoFollowBox);
            this.Controls.Add(this.autoBuffList);
            this.Controls.Add(this.autoBuffBox);
            this.Controls.Add(this.autoHealTime);
            this.Controls.Add(this.autoHealBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FlyffWCForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Flyff Universe Webclient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoHealBox;
        private System.Windows.Forms.ComboBox autoHealTime;
        private System.Windows.Forms.CheckBox autoBuffBox;
        private System.Windows.Forms.CheckedListBox autoBuffList;
        private System.Windows.Forms.CheckBox autoFollowBox;
    }
}

