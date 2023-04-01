namespace SoundTyperWinform
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
            audioDeviceComboBox = new ComboBox();
            textBoxFilePath = new TextBox();
            buttonBrowse = new Button();
            AudioDeviceLabel = new Label();
            AudioFIleLabel = new Label();
            ActiveWindowTextBox = new Label();
            ActiveWindowLabel = new Label();
            SuspendLayout();
            // 
            // audioDeviceComboBox
            // 
            audioDeviceComboBox.FormattingEnabled = true;
            audioDeviceComboBox.Location = new Point(25, 35);
            audioDeviceComboBox.Name = "audioDeviceComboBox";
            audioDeviceComboBox.Size = new Size(376, 28);
            audioDeviceComboBox.TabIndex = 0;
            audioDeviceComboBox.SelectedIndexChanged += audioDeviceComboBox_SelectedIndexChanged;
            // 
            // textBoxFilePath
            // 
            textBoxFilePath.Location = new Point(25, 97);
            textBoxFilePath.Name = "textBoxFilePath";
            textBoxFilePath.Size = new Size(376, 27);
            textBoxFilePath.TabIndex = 1;
            textBoxFilePath.TextChanged += textBoxFilePath_TextChanged;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(407, 96);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(94, 29);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // AudioDeviceLabel
            // 
            AudioDeviceLabel.AutoSize = true;
            AudioDeviceLabel.Location = new Point(25, 8);
            AudioDeviceLabel.Name = "AudioDeviceLabel";
            AudioDeviceLabel.Size = new Size(98, 20);
            AudioDeviceLabel.TabIndex = 3;
            AudioDeviceLabel.Text = "Audio Device";
            // 
            // AudioFIleLabel
            // 
            AudioFIleLabel.AutoSize = true;
            AudioFIleLabel.Location = new Point(25, 70);
            AudioFIleLabel.Name = "AudioFIleLabel";
            AudioFIleLabel.Size = new Size(76, 20);
            AudioFIleLabel.TabIndex = 4;
            AudioFIleLabel.Text = "Audio File";
            // 
            // ActiveWindowTextBox
            // 
            ActiveWindowTextBox.Location = new Point(28, 177);
            ActiveWindowTextBox.Name = "ActiveWindowTextBox";
            ActiveWindowTextBox.Size = new Size(473, 25);
            ActiveWindowTextBox.TabIndex = 5;
            // 
            // ActiveWindowLabel
            // 
            ActiveWindowLabel.AutoSize = true;
            ActiveWindowLabel.Location = new Point(28, 148);
            ActiveWindowLabel.Name = "ActiveWindowLabel";
            ActiveWindowLabel.Size = new Size(109, 20);
            ActiveWindowLabel.TabIndex = 6;
            ActiveWindowLabel.Text = "Active Window";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(525, 244);
            Controls.Add(ActiveWindowLabel);
            Controls.Add(ActiveWindowTextBox);
            Controls.Add(AudioFIleLabel);
            Controls.Add(AudioDeviceLabel);
            Controls.Add(buttonBrowse);
            Controls.Add(textBoxFilePath);
            Controls.Add(audioDeviceComboBox);
            Name = "Form1";
            Text = "SoundTyper";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox audioDeviceComboBox;
        private TextBox textBoxFilePath;
        private Button buttonBrowse;
        private Label AudioDeviceLabel;
        private Label AudioFIleLabel;
        private Label ActiveWindowTextBox;
        private Label ActiveWindowLabel;
    }
}