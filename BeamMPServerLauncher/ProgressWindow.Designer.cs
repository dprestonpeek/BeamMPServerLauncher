namespace BeamMPServerLauncher
{
    partial class ProgressWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressWindow));
            LoadingDetails = new RichTextBox();
            LoadingProgressBar = new ProgressBar();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            SuspendLayout();
            // 
            // LoadingDetails
            // 
            LoadingDetails.Location = new Point(12, 46);
            LoadingDetails.Name = "LoadingDetails";
            LoadingDetails.Size = new Size(431, 233);
            LoadingDetails.TabIndex = 0;
            LoadingDetails.Text = "";
            // 
            // LoadingProgressBar
            // 
            LoadingProgressBar.Location = new Point(12, 12);
            LoadingProgressBar.Name = "LoadingProgressBar";
            LoadingProgressBar.Size = new Size(431, 28);
            LoadingProgressBar.Style = ProgressBarStyle.Continuous;
            LoadingProgressBar.TabIndex = 1;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // ProgressWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(455, 291);
            Controls.Add(LoadingProgressBar);
            Controls.Add(LoadingDetails);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ProgressWindow";
            Text = "Importing Content...";
            TopMost = true;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox LoadingDetails;
        private ProgressBar LoadingProgressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}