namespace ChessPro.GUI.EngineOptionControls
{
    partial class EngineComboOption
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
            this.comboBoxOptions = new System.Windows.Forms.ComboBox();
            this.optionNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxOptions
            // 
            this.comboBoxOptions.FormattingEnabled = true;
            this.comboBoxOptions.Location = new System.Drawing.Point(3, 27);
            this.comboBoxOptions.Name = "comboBoxOptions";
            this.comboBoxOptions.Size = new System.Drawing.Size(105, 21);
            this.comboBoxOptions.TabIndex = 0;
            this.comboBoxOptions.SelectedValueChanged += new System.EventHandler(this.SelectedItemChanged);
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
            // EngineComboOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionNameLabel);
            this.Controls.Add(this.comboBoxOptions);
            this.Name = "EngineComboOption";
            this.Size = new System.Drawing.Size(116, 54);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOptions;
        private System.Windows.Forms.Label optionNameLabel;
    }
}
