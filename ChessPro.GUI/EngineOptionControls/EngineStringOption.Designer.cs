namespace ChessPro.GUI.EngineOptionControls
{
    partial class EngineStringOption
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
            this.optionValueTextBox = new System.Windows.Forms.TextBox();
            this.optionName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // optionValueTextBox
            // 
            this.optionValueTextBox.Location = new System.Drawing.Point(6, 21);
            this.optionValueTextBox.Name = "optionValueTextBox";
            this.optionValueTextBox.Size = new System.Drawing.Size(126, 20);
            this.optionValueTextBox.TabIndex = 0;
            // 
            // optionName
            // 
            this.optionName.AutoSize = true;
            this.optionName.Location = new System.Drawing.Point(3, 4);
            this.optionName.Name = "optionName";
            this.optionName.Size = new System.Drawing.Size(64, 13);
            this.optionName.TabIndex = 1;
            this.optionName.Text = "optionName";
            // 
            // EngineStringOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionName);
            this.Controls.Add(this.optionValueTextBox);
            this.Name = "EngineStringOption";
            this.Size = new System.Drawing.Size(135, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox optionValueTextBox;
        private System.Windows.Forms.Label optionName;
    }
}
