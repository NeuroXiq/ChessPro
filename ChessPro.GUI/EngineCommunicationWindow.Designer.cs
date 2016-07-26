namespace ChessPro.GUI
{
    partial class EngineCommunicationWindow
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
            this.engineCommunicationRichBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // engineCommunicationRichBox
            // 
            this.engineCommunicationRichBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.engineCommunicationRichBox.Location = new System.Drawing.Point(12, 12);
            this.engineCommunicationRichBox.MaxLength = 100;
            this.engineCommunicationRichBox.Name = "engineCommunicationRichBox";
            this.engineCommunicationRichBox.ReadOnly = true;
            this.engineCommunicationRichBox.Size = new System.Drawing.Size(339, 270);
            this.engineCommunicationRichBox.TabIndex = 0;
            this.engineCommunicationRichBox.Text = "";
            // 
            // EngineCommunicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 294);
            this.Controls.Add(this.engineCommunicationRichBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EngineCommunicationWindow";
            this.Text = "Engine Window";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox engineCommunicationRichBox;
    }
}