namespace ChessPro.GUI.EngineOptionControls
{
    partial class EngineButtonOption
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.setOptionButton = new System.Windows.Forms.Button();
            this.optionNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // setOptionButton
            // 
            this.setOptionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setOptionButton.Location = new System.Drawing.Point(3, 24);
            this.setOptionButton.Name = "setOptionButton";
            this.setOptionButton.Size = new System.Drawing.Size(122, 23);
            this.setOptionButton.TabIndex = 0;
            this.setOptionButton.Text = "Set";
            this.setOptionButton.UseVisualStyleBackColor = true;
            this.setOptionButton.Click += new System.EventHandler(this.SetOptionButtonClick);
            // 
            // optionNameLabel
            // 
            this.optionNameLabel.AutoSize = true;
            this.optionNameLabel.Location = new System.Drawing.Point(3, 8);
            this.optionNameLabel.Name = "optionNameLabel";
            this.optionNameLabel.Size = new System.Drawing.Size(38, 13);
            this.optionNameLabel.TabIndex = 1;
            this.optionNameLabel.Text = "NAME";
            // 
            // EngineButtonOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionNameLabel);
            this.Controls.Add(this.setOptionButton);
            this.Name = "EngineButtonOption";
            this.Size = new System.Drawing.Size(128, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setOptionButton;
        private System.Windows.Forms.Label optionNameLabel;
    }
}
