namespace ChessPro.GUI
{
    partial class MainWindow
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
            this.boardPictureBox = new System.Windows.Forms.PictureBox();
            this.gamePropertiesTabControl = new System.Windows.Forms.TabControl();
            this.gameTabPage = new System.Windows.Forms.TabPage();
            this.gameGroupBox = new System.Windows.Forms.GroupBox();
            this.moveNowButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.getHintButton = new System.Windows.Forms.Button();
            this.newGameButton = new System.Windows.Forms.Button();
            this.boardGroupBox = new System.Windows.Forms.GroupBox();
            this.reverseBoardCheckBox = new System.Windows.Forms.CheckBox();
            this.whiteFieldColorPanelPreview = new System.Windows.Forms.Panel();
            this.blackFieldColorPanelPreview = new System.Windows.Forms.Panel();
            this.whiteFieldColorLabel = new System.Windows.Forms.Label();
            this.blackFieldColorLabel = new System.Windows.Forms.Label();
            this.engineGroupBox = new System.Windows.Forms.GroupBox();
            this.engineIdRichTextBox = new System.Windows.Forms.RichTextBox();
            this.showEngineOutput = new System.Windows.Forms.Button();
            this.engineStatusLabel = new System.Windows.Forms.Label();
            this.loadEngineButton = new System.Windows.Forms.Button();
            this.selectEngineComboBox = new System.Windows.Forms.ComboBox();
            this.playAsGroupBox = new System.Windows.Forms.GroupBox();
            this.whiteRadioButton = new System.Windows.Forms.RadioButton();
            this.blackRadioButton = new System.Windows.Forms.RadioButton();
            this.engineOptionsTabPage = new System.Windows.Forms.TabPage();
            this.searchingTabPage = new System.Windows.Forms.TabPage();
            this.saveSearchProperties = new System.Windows.Forms.Button();
            this.depthTextBox = new System.Windows.Forms.TextBox();
            this.depthLabel = new System.Windows.Forms.Label();
            this.nodesTextBox = new System.Windows.Forms.TextBox();
            this.nodesLabel = new System.Windows.Forms.Label();
            this.searchTimeTextBox = new System.Windows.Forms.TextBox();
            this.searchTimeLabel = new System.Windows.Forms.Label();
            this.searchInfinityCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.boardPictureBox)).BeginInit();
            this.gamePropertiesTabControl.SuspendLayout();
            this.gameTabPage.SuspendLayout();
            this.gameGroupBox.SuspendLayout();
            this.boardGroupBox.SuspendLayout();
            this.engineGroupBox.SuspendLayout();
            this.playAsGroupBox.SuspendLayout();
            this.searchingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // boardPictureBox
            // 
            this.boardPictureBox.Location = new System.Drawing.Point(12, 12);
            this.boardPictureBox.Name = "boardPictureBox";
            this.boardPictureBox.Size = new System.Drawing.Size(480, 480);
            this.boardPictureBox.TabIndex = 0;
            this.boardPictureBox.TabStop = false;
            // 
            // gamePropertiesTabControl
            // 
            this.gamePropertiesTabControl.Controls.Add(this.gameTabPage);
            this.gamePropertiesTabControl.Controls.Add(this.engineOptionsTabPage);
            this.gamePropertiesTabControl.Controls.Add(this.searchingTabPage);
            this.gamePropertiesTabControl.Location = new System.Drawing.Point(498, 12);
            this.gamePropertiesTabControl.Name = "gamePropertiesTabControl";
            this.gamePropertiesTabControl.SelectedIndex = 0;
            this.gamePropertiesTabControl.Size = new System.Drawing.Size(195, 480);
            this.gamePropertiesTabControl.TabIndex = 2;
            // 
            // gameTabPage
            // 
            this.gameTabPage.Controls.Add(this.gameGroupBox);
            this.gameTabPage.Controls.Add(this.boardGroupBox);
            this.gameTabPage.Controls.Add(this.engineGroupBox);
            this.gameTabPage.Controls.Add(this.playAsGroupBox);
            this.gameTabPage.Location = new System.Drawing.Point(4, 22);
            this.gameTabPage.Name = "gameTabPage";
            this.gameTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.gameTabPage.Size = new System.Drawing.Size(187, 454);
            this.gameTabPage.TabIndex = 0;
            this.gameTabPage.Text = "Game";
            this.gameTabPage.UseVisualStyleBackColor = true;
            // 
            // gameGroupBox
            // 
            this.gameGroupBox.Controls.Add(this.moveNowButton);
            this.gameGroupBox.Controls.Add(this.infoLabel);
            this.gameGroupBox.Controls.Add(this.getHintButton);
            this.gameGroupBox.Controls.Add(this.newGameButton);
            this.gameGroupBox.Location = new System.Drawing.Point(6, 351);
            this.gameGroupBox.Name = "gameGroupBox";
            this.gameGroupBox.Size = new System.Drawing.Size(175, 97);
            this.gameGroupBox.TabIndex = 4;
            this.gameGroupBox.TabStop = false;
            this.gameGroupBox.Text = "Game";
            // 
            // moveNowButton
            // 
            this.moveNowButton.Location = new System.Drawing.Point(9, 32);
            this.moveNowButton.Name = "moveNowButton";
            this.moveNowButton.Size = new System.Drawing.Size(98, 23);
            this.moveNowButton.TabIndex = 3;
            this.moveNowButton.Text = "Move now !";
            this.moveNowButton.UseVisualStyleBackColor = true;
            this.moveNowButton.Click += new System.EventHandler(this.MoveNowButtonClick);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(6, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(62, 13);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Information:";
            // 
            // getHintButton
            // 
            this.getHintButton.Location = new System.Drawing.Point(113, 32);
            this.getHintButton.Name = "getHintButton";
            this.getHintButton.Size = new System.Drawing.Size(59, 23);
            this.getHintButton.TabIndex = 1;
            this.getHintButton.Text = "Get hint";
            this.getHintButton.UseVisualStyleBackColor = true;
            this.getHintButton.Click += new System.EventHandler(this.GetHint);
            // 
            // newGameButton
            // 
            this.newGameButton.Location = new System.Drawing.Point(6, 61);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(163, 31);
            this.newGameButton.TabIndex = 0;
            this.newGameButton.Text = "New Game";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.NewGameButtonClick);
            // 
            // boardGroupBox
            // 
            this.boardGroupBox.Controls.Add(this.reverseBoardCheckBox);
            this.boardGroupBox.Controls.Add(this.whiteFieldColorPanelPreview);
            this.boardGroupBox.Controls.Add(this.blackFieldColorPanelPreview);
            this.boardGroupBox.Controls.Add(this.whiteFieldColorLabel);
            this.boardGroupBox.Controls.Add(this.blackFieldColorLabel);
            this.boardGroupBox.Location = new System.Drawing.Point(6, 240);
            this.boardGroupBox.Name = "boardGroupBox";
            this.boardGroupBox.Size = new System.Drawing.Size(175, 105);
            this.boardGroupBox.TabIndex = 3;
            this.boardGroupBox.TabStop = false;
            this.boardGroupBox.Text = "Board";
            // 
            // reverseBoardCheckBox
            // 
            this.reverseBoardCheckBox.AutoSize = true;
            this.reverseBoardCheckBox.Location = new System.Drawing.Point(6, 83);
            this.reverseBoardCheckBox.Name = "reverseBoardCheckBox";
            this.reverseBoardCheckBox.Size = new System.Drawing.Size(97, 17);
            this.reverseBoardCheckBox.TabIndex = 4;
            this.reverseBoardCheckBox.Text = "Reverse Board";
            this.reverseBoardCheckBox.UseVisualStyleBackColor = true;
            this.reverseBoardCheckBox.CheckedChanged += new System.EventHandler(this.ReverseBoardCheckBoxChanged);
            // 
            // whiteFieldColorPanelPreview
            // 
            this.whiteFieldColorPanelPreview.Location = new System.Drawing.Point(94, 50);
            this.whiteFieldColorPanelPreview.Name = "whiteFieldColorPanelPreview";
            this.whiteFieldColorPanelPreview.Size = new System.Drawing.Size(75, 25);
            this.whiteFieldColorPanelPreview.TabIndex = 3;
            this.whiteFieldColorPanelPreview.Click += new System.EventHandler(this.WhiteFieldColorPanelClicked);
            // 
            // blackFieldColorPanelPreview
            // 
            this.blackFieldColorPanelPreview.Location = new System.Drawing.Point(94, 19);
            this.blackFieldColorPanelPreview.Name = "blackFieldColorPanelPreview";
            this.blackFieldColorPanelPreview.Size = new System.Drawing.Size(75, 25);
            this.blackFieldColorPanelPreview.TabIndex = 2;
            this.blackFieldColorPanelPreview.Click += new System.EventHandler(this.BlackFieldColorPanelClicked);
            // 
            // whiteFieldColorLabel
            // 
            this.whiteFieldColorLabel.AutoSize = true;
            this.whiteFieldColorLabel.Location = new System.Drawing.Point(6, 53);
            this.whiteFieldColorLabel.Name = "whiteFieldColorLabel";
            this.whiteFieldColorLabel.Size = new System.Drawing.Size(83, 13);
            this.whiteFieldColorLabel.TabIndex = 1;
            this.whiteFieldColorLabel.Text = "White field color";
            // 
            // blackFieldColorLabel
            // 
            this.blackFieldColorLabel.AutoSize = true;
            this.blackFieldColorLabel.Location = new System.Drawing.Point(6, 25);
            this.blackFieldColorLabel.Name = "blackFieldColorLabel";
            this.blackFieldColorLabel.Size = new System.Drawing.Size(82, 13);
            this.blackFieldColorLabel.TabIndex = 0;
            this.blackFieldColorLabel.Text = "Black field color";
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.engineIdRichTextBox);
            this.engineGroupBox.Controls.Add(this.showEngineOutput);
            this.engineGroupBox.Controls.Add(this.engineStatusLabel);
            this.engineGroupBox.Controls.Add(this.loadEngineButton);
            this.engineGroupBox.Controls.Add(this.selectEngineComboBox);
            this.engineGroupBox.Location = new System.Drawing.Point(6, 80);
            this.engineGroupBox.Name = "engineGroupBox";
            this.engineGroupBox.Size = new System.Drawing.Size(175, 154);
            this.engineGroupBox.TabIndex = 2;
            this.engineGroupBox.TabStop = false;
            this.engineGroupBox.Text = "Engine";
            // 
            // engineIdRichTextBox
            // 
            this.engineIdRichTextBox.Location = new System.Drawing.Point(6, 99);
            this.engineIdRichTextBox.Name = "engineIdRichTextBox";
            this.engineIdRichTextBox.ReadOnly = true;
            this.engineIdRichTextBox.Size = new System.Drawing.Size(163, 49);
            this.engineIdRichTextBox.TabIndex = 4;
            this.engineIdRichTextBox.Text = "";
            // 
            // showEngineOutput
            // 
            this.showEngineOutput.Location = new System.Drawing.Point(6, 74);
            this.showEngineOutput.Name = "showEngineOutput";
            this.showEngineOutput.Size = new System.Drawing.Size(163, 23);
            this.showEngineOutput.TabIndex = 3;
            this.showEngineOutput.Text = "Show engine communication";
            this.showEngineOutput.UseVisualStyleBackColor = true;
            this.showEngineOutput.Click += new System.EventHandler(this.ShowEngineOutput);
            // 
            // engineStatusLabel
            // 
            this.engineStatusLabel.AutoSize = true;
            this.engineStatusLabel.Location = new System.Drawing.Point(3, 51);
            this.engineStatusLabel.Name = "engineStatusLabel";
            this.engineStatusLabel.Size = new System.Drawing.Size(43, 13);
            this.engineStatusLabel.TabIndex = 2;
            this.engineStatusLabel.Text = "Status: ";
            // 
            // loadEngineButton
            // 
            this.loadEngineButton.Location = new System.Drawing.Point(94, 46);
            this.loadEngineButton.Name = "loadEngineButton";
            this.loadEngineButton.Size = new System.Drawing.Size(75, 23);
            this.loadEngineButton.TabIndex = 1;
            this.loadEngineButton.Text = "Load";
            this.loadEngineButton.UseVisualStyleBackColor = true;
            this.loadEngineButton.Click += new System.EventHandler(this.LoadEngine);
            // 
            // selectEngineComboBox
            // 
            this.selectEngineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectEngineComboBox.FormattingEnabled = true;
            this.selectEngineComboBox.Location = new System.Drawing.Point(6, 19);
            this.selectEngineComboBox.Name = "selectEngineComboBox";
            this.selectEngineComboBox.Size = new System.Drawing.Size(166, 21);
            this.selectEngineComboBox.TabIndex = 0;
            // 
            // playAsGroupBox
            // 
            this.playAsGroupBox.Controls.Add(this.whiteRadioButton);
            this.playAsGroupBox.Controls.Add(this.blackRadioButton);
            this.playAsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.playAsGroupBox.Name = "playAsGroupBox";
            this.playAsGroupBox.Size = new System.Drawing.Size(175, 68);
            this.playAsGroupBox.TabIndex = 1;
            this.playAsGroupBox.TabStop = false;
            this.playAsGroupBox.Text = "Play as";
            // 
            // whiteRadioButton
            // 
            this.whiteRadioButton.AutoSize = true;
            this.whiteRadioButton.Checked = true;
            this.whiteRadioButton.Location = new System.Drawing.Point(6, 19);
            this.whiteRadioButton.Name = "whiteRadioButton";
            this.whiteRadioButton.Size = new System.Drawing.Size(53, 17);
            this.whiteRadioButton.TabIndex = 1;
            this.whiteRadioButton.TabStop = true;
            this.whiteRadioButton.Text = "White";
            this.whiteRadioButton.UseVisualStyleBackColor = true;
            // 
            // blackRadioButton
            // 
            this.blackRadioButton.AutoSize = true;
            this.blackRadioButton.Location = new System.Drawing.Point(6, 42);
            this.blackRadioButton.Name = "blackRadioButton";
            this.blackRadioButton.Size = new System.Drawing.Size(52, 17);
            this.blackRadioButton.TabIndex = 0;
            this.blackRadioButton.Text = "Black";
            this.blackRadioButton.UseVisualStyleBackColor = true;
            // 
            // engineOptionsTabPage
            // 
            this.engineOptionsTabPage.AutoScroll = true;
            this.engineOptionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.engineOptionsTabPage.Name = "engineOptionsTabPage";
            this.engineOptionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.engineOptionsTabPage.Size = new System.Drawing.Size(187, 454);
            this.engineOptionsTabPage.TabIndex = 1;
            this.engineOptionsTabPage.Text = "Engine Options";
            this.engineOptionsTabPage.UseVisualStyleBackColor = true;
            // 
            // searchingTabPage
            // 
            this.searchingTabPage.Controls.Add(this.saveSearchProperties);
            this.searchingTabPage.Controls.Add(this.depthTextBox);
            this.searchingTabPage.Controls.Add(this.depthLabel);
            this.searchingTabPage.Controls.Add(this.nodesTextBox);
            this.searchingTabPage.Controls.Add(this.nodesLabel);
            this.searchingTabPage.Controls.Add(this.searchTimeTextBox);
            this.searchingTabPage.Controls.Add(this.searchTimeLabel);
            this.searchingTabPage.Controls.Add(this.searchInfinityCheckBox);
            this.searchingTabPage.Location = new System.Drawing.Point(4, 22);
            this.searchingTabPage.Name = "searchingTabPage";
            this.searchingTabPage.Size = new System.Drawing.Size(187, 454);
            this.searchingTabPage.TabIndex = 2;
            this.searchingTabPage.Text = "Searching";
            this.searchingTabPage.UseVisualStyleBackColor = true;
            // 
            // saveSearchProperties
            // 
            this.saveSearchProperties.Location = new System.Drawing.Point(81, 118);
            this.saveSearchProperties.Name = "saveSearchProperties";
            this.saveSearchProperties.Size = new System.Drawing.Size(103, 23);
            this.saveSearchProperties.TabIndex = 7;
            this.saveSearchProperties.Text = "Save Properties";
            this.saveSearchProperties.UseVisualStyleBackColor = true;
            this.saveSearchProperties.Click += new System.EventHandler(this.SaveSearchProperties);
            // 
            // depthTextBox
            // 
            this.depthTextBox.Location = new System.Drawing.Point(91, 62);
            this.depthTextBox.Name = "depthTextBox";
            this.depthTextBox.Size = new System.Drawing.Size(93, 20);
            this.depthTextBox.TabIndex = 6;
            this.depthTextBox.TextChanged += new System.EventHandler(this.SearchPropertiesTextBoxTextChanged);
            // 
            // depthLabel
            // 
            this.depthLabel.AutoSize = true;
            this.depthLabel.Location = new System.Drawing.Point(3, 65);
            this.depthLabel.Name = "depthLabel";
            this.depthLabel.Size = new System.Drawing.Size(39, 13);
            this.depthLabel.TabIndex = 5;
            this.depthLabel.Text = "Depth:";
            // 
            // nodesTextBox
            // 
            this.nodesTextBox.Location = new System.Drawing.Point(91, 36);
            this.nodesTextBox.Name = "nodesTextBox";
            this.nodesTextBox.Size = new System.Drawing.Size(93, 20);
            this.nodesTextBox.TabIndex = 4;
            this.nodesTextBox.TextChanged += new System.EventHandler(this.SearchPropertiesTextBoxTextChanged);
            // 
            // nodesLabel
            // 
            this.nodesLabel.AutoSize = true;
            this.nodesLabel.Location = new System.Drawing.Point(3, 39);
            this.nodesLabel.Name = "nodesLabel";
            this.nodesLabel.Size = new System.Drawing.Size(41, 13);
            this.nodesLabel.TabIndex = 3;
            this.nodesLabel.Text = "Nodes:";
            // 
            // searchTimeTextBox
            // 
            this.searchTimeTextBox.Location = new System.Drawing.Point(91, 10);
            this.searchTimeTextBox.Name = "searchTimeTextBox";
            this.searchTimeTextBox.Size = new System.Drawing.Size(93, 20);
            this.searchTimeTextBox.TabIndex = 2;
            this.searchTimeTextBox.TextChanged += new System.EventHandler(this.SearchPropertiesTextBoxTextChanged);
            // 
            // searchTimeLabel
            // 
            this.searchTimeLabel.AutoSize = true;
            this.searchTimeLabel.Location = new System.Drawing.Point(3, 13);
            this.searchTimeLabel.Name = "searchTimeLabel";
            this.searchTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.searchTimeLabel.TabIndex = 1;
            this.searchTimeLabel.Text = "Search time (ms):";
            // 
            // searchInfinityCheckBox
            // 
            this.searchInfinityCheckBox.AutoSize = true;
            this.searchInfinityCheckBox.Location = new System.Drawing.Point(6, 94);
            this.searchInfinityCheckBox.Name = "searchInfinityCheckBox";
            this.searchInfinityCheckBox.Size = new System.Drawing.Size(93, 17);
            this.searchInfinityCheckBox.TabIndex = 0;
            this.searchInfinityCheckBox.Text = "Search Infinity";
            this.searchInfinityCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 505);
            this.Controls.Add(this.gamePropertiesTabControl);
            this.Controls.Add(this.boardPictureBox);
            this.MaximumSize = new System.Drawing.Size(716, 544);
            this.MinimumSize = new System.Drawing.Size(716, 544);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.boardPictureBox)).EndInit();
            this.gamePropertiesTabControl.ResumeLayout(false);
            this.gameTabPage.ResumeLayout(false);
            this.gameGroupBox.ResumeLayout(false);
            this.gameGroupBox.PerformLayout();
            this.boardGroupBox.ResumeLayout(false);
            this.boardGroupBox.PerformLayout();
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.playAsGroupBox.ResumeLayout(false);
            this.playAsGroupBox.PerformLayout();
            this.searchingTabPage.ResumeLayout(false);
            this.searchingTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox boardPictureBox;
        private System.Windows.Forms.TabControl gamePropertiesTabControl;
        private System.Windows.Forms.TabPage gameTabPage;
        private System.Windows.Forms.TabPage engineOptionsTabPage;
        private System.Windows.Forms.TabPage searchingTabPage;
        private System.Windows.Forms.GroupBox gameGroupBox;
        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.GroupBox boardGroupBox;
        private System.Windows.Forms.CheckBox reverseBoardCheckBox;
        private System.Windows.Forms.Panel whiteFieldColorPanelPreview;
        private System.Windows.Forms.Panel blackFieldColorPanelPreview;
        private System.Windows.Forms.Label whiteFieldColorLabel;
        private System.Windows.Forms.Label blackFieldColorLabel;
        private System.Windows.Forms.GroupBox engineGroupBox;
        private System.Windows.Forms.Label engineStatusLabel;
        private System.Windows.Forms.Button loadEngineButton;
        private System.Windows.Forms.ComboBox selectEngineComboBox;
        private System.Windows.Forms.GroupBox playAsGroupBox;
        private System.Windows.Forms.RadioButton whiteRadioButton;
        private System.Windows.Forms.RadioButton blackRadioButton;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button getHintButton;
        private System.Windows.Forms.Button showEngineOutput;
        private System.Windows.Forms.RichTextBox engineIdRichTextBox;
        private System.Windows.Forms.TextBox depthTextBox;
        private System.Windows.Forms.Label depthLabel;
        private System.Windows.Forms.TextBox nodesTextBox;
        private System.Windows.Forms.Label nodesLabel;
        private System.Windows.Forms.TextBox searchTimeTextBox;
        private System.Windows.Forms.Label searchTimeLabel;
        private System.Windows.Forms.CheckBox searchInfinityCheckBox;
        private System.Windows.Forms.Button saveSearchProperties;
        private System.Windows.Forms.Button moveNowButton;
    }
}