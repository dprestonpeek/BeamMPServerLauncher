﻿namespace BeamMPServerLauncher
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            StartButton = new Button();
            MapSelector = new ComboBox();
            MapPreview = new PictureBox();
            NextPreviewButton = new Button();
            ServerDescBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            MaxPlayersBox = new TextBox();
            label3 = new Label();
            MaxCarsBox = new TextBox();
            label4 = new Label();
            label5 = new Label();
            ServerNameBox = new TextBox();
            PrivateCheckbox = new CheckBox();
            SourceButton = new Button();
            SaveConfigButton = new Button();
            OfflineCheckbox = new CheckBox();
            StopServerButton = new Button();
            RestartButton = new Button();
            ReimportMapButton = new Button();
            RefreshButton = new Button();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)MapPreview).BeginInit();
            SuspendLayout();
            // 
            // StartButton
            // 
            StartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            StartButton.Enabled = false;
            StartButton.Location = new Point(455, 248);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(81, 49);
            StartButton.TabIndex = 6;
            StartButton.Text = "Start Server";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // MapSelector
            // 
            MapSelector.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MapSelector.FormattingEnabled = true;
            MapSelector.Location = new Point(12, 263);
            MapSelector.MaxDropDownItems = 99;
            MapSelector.Name = "MapSelector";
            MapSelector.Size = new Size(254, 23);
            MapSelector.TabIndex = 0;
            MapSelector.SelectedIndexChanged += MapSelector_SelectedIndexChanged;
            // 
            // MapPreview
            // 
            MapPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MapPreview.Location = new Point(12, 12);
            MapPreview.Name = "MapPreview";
            MapPreview.Size = new Size(304, 230);
            MapPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            MapPreview.TabIndex = 2;
            MapPreview.TabStop = false;
            // 
            // NextPreviewButton
            // 
            NextPreviewButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            NextPreviewButton.Location = new Point(272, 259);
            NextPreviewButton.Name = "NextPreviewButton";
            NextPreviewButton.Size = new Size(44, 29);
            NextPreviewButton.TabIndex = 8;
            NextPreviewButton.Text = ">>";
            NextPreviewButton.UseVisualStyleBackColor = true;
            NextPreviewButton.Click += NextPreviewButton_Click;
            // 
            // ServerDescBox
            // 
            ServerDescBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ServerDescBox.Location = new Point(322, 83);
            ServerDescBox.Name = "ServerDescBox";
            ServerDescBox.PlaceholderText = "What players read while considering";
            ServerDescBox.Size = new Size(214, 23);
            ServerDescBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(322, 65);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 5;
            label1.Text = "Server Description:";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(322, 118);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 7;
            label2.Text = "Max Players:";
            // 
            // MaxPlayersBox
            // 
            MaxPlayersBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MaxPlayersBox.Location = new Point(322, 136);
            MaxPlayersBox.Name = "MaxPlayersBox";
            MaxPlayersBox.PlaceholderText = "Number of players including AI";
            MaxPlayersBox.Size = new Size(214, 23);
            MaxPlayersBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(322, 171);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 9;
            label3.Text = "Max Cars:";
            // 
            // MaxCarsBox
            // 
            MaxCarsBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MaxCarsBox.Location = new Point(322, 189);
            MaxCarsBox.Name = "MaxCarsBox";
            MaxCarsBox.PlaceholderText = "Number of cars per player";
            MaxCarsBox.Size = new Size(214, 23);
            MaxCarsBox.TabIndex = 4;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(12, 245);
            label4.Name = "label4";
            label4.Size = new Size(34, 15);
            label4.TabIndex = 10;
            label4.Text = "Map:";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(322, 12);
            label5.Name = "label5";
            label5.Size = new Size(77, 15);
            label5.TabIndex = 12;
            label5.Text = "Server Name:";
            // 
            // ServerNameBox
            // 
            ServerNameBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ServerNameBox.Location = new Point(322, 30);
            ServerNameBox.Name = "ServerNameBox";
            ServerNameBox.PlaceholderText = "What players see before joining";
            ServerNameBox.Size = new Size(214, 23);
            ServerNameBox.TabIndex = 1;
            // 
            // PrivateCheckbox
            // 
            PrivateCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PrivateCheckbox.AutoSize = true;
            PrivateCheckbox.Location = new Point(322, 223);
            PrivateCheckbox.Name = "PrivateCheckbox";
            PrivateCheckbox.Size = new Size(97, 19);
            PrivateCheckbox.TabIndex = 5;
            PrivateCheckbox.Text = "Private Server";
            PrivateCheckbox.UseVisualStyleBackColor = true;
            // 
            // SourceButton
            // 
            SourceButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SourceButton.Location = new Point(12, 303);
            SourceButton.Name = "SourceButton";
            SourceButton.Size = new Size(67, 25);
            SourceButton.TabIndex = 7;
            SourceButton.Text = "Source";
            SourceButton.UseVisualStyleBackColor = true;
            SourceButton.Click += SourceButton_Click;
            // 
            // SaveConfigButton
            // 
            SaveConfigButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveConfigButton.Enabled = false;
            SaveConfigButton.Location = new Point(368, 248);
            SaveConfigButton.Name = "SaveConfigButton";
            SaveConfigButton.Size = new Size(81, 49);
            SaveConfigButton.TabIndex = 13;
            SaveConfigButton.Text = "Save Config";
            SaveConfigButton.UseVisualStyleBackColor = true;
            SaveConfigButton.Click += SaveConfigButton_Click;
            // 
            // OfflineCheckbox
            // 
            OfflineCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OfflineCheckbox.AutoSize = true;
            OfflineCheckbox.Location = new Point(425, 223);
            OfflineCheckbox.Name = "OfflineCheckbox";
            OfflineCheckbox.Size = new Size(97, 19);
            OfflineCheckbox.TabIndex = 14;
            OfflineCheckbox.Text = "Offline Server";
            OfflineCheckbox.UseVisualStyleBackColor = true;
            OfflineCheckbox.Visible = false;
            // 
            // StopServerButton
            // 
            StopServerButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            StopServerButton.Enabled = false;
            StopServerButton.ForeColor = Color.Red;
            StopServerButton.Location = new Point(368, 303);
            StopServerButton.Name = "StopServerButton";
            StopServerButton.Size = new Size(81, 25);
            StopServerButton.TabIndex = 15;
            StopServerButton.Text = "Stop Server";
            StopServerButton.UseVisualStyleBackColor = true;
            StopServerButton.Click += StopServerButton_Click;
            // 
            // RestartButton
            // 
            RestartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            RestartButton.Enabled = false;
            RestartButton.ForeColor = Color.Red;
            RestartButton.Location = new Point(455, 303);
            RestartButton.Name = "RestartButton";
            RestartButton.Size = new Size(81, 25);
            RestartButton.TabIndex = 16;
            RestartButton.Text = "Restart";
            RestartButton.UseVisualStyleBackColor = true;
            RestartButton.Click += RestartButton_Click;
            // 
            // ReimportMapButton
            // 
            ReimportMapButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ReimportMapButton.ForeColor = Color.Red;
            ReimportMapButton.Location = new Point(216, 303);
            ReimportMapButton.Name = "ReimportMapButton";
            ReimportMapButton.Size = new Size(100, 25);
            ReimportMapButton.TabIndex = 17;
            ReimportMapButton.Text = "Reimport Map";
            ReimportMapButton.UseVisualStyleBackColor = true;
            ReimportMapButton.Click += ReimportMapButton_Click;
            // 
            // RefreshButton
            // 
            RefreshButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            RefreshButton.Location = new Point(85, 303);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(67, 25);
            RefreshButton.TabIndex = 18;
            RefreshButton.Text = "Refresh";
            RefreshButton.UseVisualStyleBackColor = true;
            RefreshButton.Click += RefreshButton_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(111, 118);
            label6.Name = "label6";
            label6.Size = new Size(111, 15);
            label6.TabIndex = 19;
            label6.Text = "No image available.";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(548, 340);
            Controls.Add(RefreshButton);
            Controls.Add(ReimportMapButton);
            Controls.Add(RestartButton);
            Controls.Add(StopServerButton);
            Controls.Add(OfflineCheckbox);
            Controls.Add(SaveConfigButton);
            Controls.Add(SourceButton);
            Controls.Add(PrivateCheckbox);
            Controls.Add(label5);
            Controls.Add(ServerNameBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(MaxCarsBox);
            Controls.Add(label2);
            Controls.Add(MaxPlayersBox);
            Controls.Add(label1);
            Controls.Add(ServerDescBox);
            Controls.Add(NextPreviewButton);
            Controls.Add(MapPreview);
            Controls.Add(MapSelector);
            Controls.Add(StartButton);
            Controls.Add(label6);
            Enabled = false;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            Text = "Beam-MP Server Configurator - D. Preston Peek (Version 1.07)";
            WindowState = FormWindowState.Minimized;
            Shown += Main_Shown;
            Enter += Main_Enter;
            ((System.ComponentModel.ISupportInitialize)MapPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartButton;
        private ComboBox MapSelector;
        private PictureBox MapPreview;
        private Button NextPreviewButton;
        private TextBox ServerDescBox;
        private Label label1;
        private Label label2;
        private TextBox MaxPlayersBox;
        private Label label3;
        private TextBox MaxCarsBox;
        private Label label4;
        private Label label5;
        private TextBox ServerNameBox;
        private CheckBox PrivateCheckbox;
        private Button SourceButton;
        private Button SaveConfigButton;
        private CheckBox OfflineCheckbox;
        private Button StopServerButton;
        private Button RestartButton;
        private Button ReimportMapButton;
        private Button RefreshButton;
        private Label label6;
    }
}
