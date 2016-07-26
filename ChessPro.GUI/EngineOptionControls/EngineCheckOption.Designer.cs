namespace ChessPro.GUI.EngineOptionControls
{
    partial class EngineCheckOption
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
            this.optionCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // optionCheckBox
            // 
            this.optionCheckBox.AutoSize = true;
            this.optionCheckBox.Location = new System.Drawing.Point(3, 3);
            this.optionCheckBox.Name = "optionCheckBox";
            this.optionCheckBox.Size = new System.Drawing.Size(80, 17);
            this.optionCheckBox.TabIndex = 0;
            this.optionCheckBox.Text = "checkBox1";
            this.optionCheckBox.UseVisualStyleBackColor = true;
            this.optionCheckBox.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // EngineCheckOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionCheckBox);
            this.Name = "EngineCheckOption";
            this.Size = new System.Drawing.Size(109, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox optionCheckBox;
    }
}
