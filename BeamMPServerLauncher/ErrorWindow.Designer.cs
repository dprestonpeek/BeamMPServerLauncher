namespace BeamMPServerLauncher
{
    partial class ErrorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorWindow));
            ErrorMessage = new Label();
            ErrorOKButton = new Button();
            ErrorAltButton = new Button();
            SuspendLayout();
            // 
            // ErrorMessage
            // 
            ErrorMessage.Location = new Point(51, 50);
            ErrorMessage.Name = "ErrorMessage";
            ErrorMessage.Size = new Size(505, 139);
            ErrorMessage.TabIndex = 0;
            ErrorMessage.Text = "Couldn't find ServerConfig.toml. Please copy it to this directory and open the Server Launcher again";
            ErrorMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ErrorOKButton
            // 
            ErrorOKButton.Location = new Point(307, 192);
            ErrorOKButton.Name = "ErrorOKButton";
            ErrorOKButton.Size = new Size(135, 23);
            ErrorOKButton.TabIndex = 1;
            ErrorOKButton.Text = "Close";
            ErrorOKButton.UseVisualStyleBackColor = true;
            ErrorOKButton.Click += ErrorOKButton_Click;
            // 
            // ErrorAltButton
            // 
            ErrorAltButton.Location = new Point(166, 192);
            ErrorAltButton.Name = "ErrorAltButton";
            ErrorAltButton.Size = new Size(135, 23);
            ErrorAltButton.TabIndex = 2;
            ErrorAltButton.Text = "Open Directory";
            ErrorAltButton.UseVisualStyleBackColor = true;
            ErrorAltButton.Click += ErrorAltButton_Click;
            // 
            // ErrorWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(606, 259);
            Controls.Add(ErrorAltButton);
            Controls.Add(ErrorOKButton);
            Controls.Add(ErrorMessage);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ErrorWindow";
            Text = "Error";
            ResumeLayout(false);
        }

        #endregion

        private Label ErrorMessage;
        private Button ErrorOKButton;
        private Button ErrorAltButton;
    }
}