namespace Demos2
{
    partial class GUI
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
            this._randomButton = new System.Windows.Forms.Button();
            this._randomNumberBox = new System.Windows.Forms.TextBox();
            this.unsubscribeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _randomButton
            // 
            this._randomButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this._randomButton.Location = new System.Drawing.Point(521, 421);
            this._randomButton.Name = "_randomButton";
            this._randomButton.Size = new System.Drawing.Size(155, 83);
            this._randomButton.TabIndex = 0;
            this._randomButton.Text = "Get random";
            this._randomButton.UseVisualStyleBackColor = true;
            this._randomButton.Click += new System.EventHandler(this.OnGetRandomButtonCLick);
            // 
            // _randomNumberBox
            // 
            this._randomNumberBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this._randomNumberBox.Location = new System.Drawing.Point(705, 421);
            this._randomNumberBox.Multiline = true;
            this._randomNumberBox.Name = "_randomNumberBox";
            this._randomNumberBox.Size = new System.Drawing.Size(127, 83);
            this._randomNumberBox.TabIndex = 1;
            // 
            // unsubscribeButton
            // 
            this.unsubscribeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.unsubscribeButton.Location = new System.Drawing.Point(12, 421);
            this.unsubscribeButton.Name = "unsubscribeButton";
            this.unsubscribeButton.Size = new System.Drawing.Size(155, 83);
            this.unsubscribeButton.TabIndex = 3;
            this.unsubscribeButton.Text = "Unsubscribe";
            this.unsubscribeButton.UseVisualStyleBackColor = true;
            this.unsubscribeButton.Click += new System.EventHandler(this.OnUnsubscribeButtonClick);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 520);
            this.Controls.Add(this.unsubscribeButton);
            this.Controls.Add(this._randomNumberBox);
            this.Controls.Add(this._randomButton);
            this.Name = "GUI";
            this.Text = "GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _randomButton;
        private System.Windows.Forms.TextBox _randomNumberBox;
        private System.Windows.Forms.Button unsubscribeButton;
    }
}