namespace ChessPro.GUI.EngineOptionControls
{
    partial class EngineSpinOption
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
            this.optionName = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.actualScrollBarValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // optionName
            // 
            this.optionName.AutoSize = true;
            this.optionName.Location = new System.Drawing.Point(4, 6);
            this.optionName.Name = "optionName";
            this.optionName.Size = new System.Drawing.Size(41, 13);
            this.optionName.TabIndex = 0;
            this.optionName.Text = "NAME:";
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(3, 23);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(119, 45);
            this.trackBar.TabIndex = 1;
            // 
            // actualScrollBarValue
            // 
            this.actualScrollBarValue.AutoSize = true;
            this.actualScrollBarValue.Location = new System.Drawing.Point(4, 71);
            this.actualScrollBarValue.Name = "actualScrollBarValue";
            this.actualScrollBarValue.Size = new System.Drawing.Size(37, 13);
            this.actualScrollBarValue.TabIndex = 2;
            this.actualScrollBarValue.Text = "Value:";
            // 
            // EngineSpinOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actualScrollBarValue);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.optionName);
            this.Name = "EngineSpinOption";
            this.Size = new System.Drawing.Size(125, 93);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label optionName;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label actualScrollBarValue;
    }
}
