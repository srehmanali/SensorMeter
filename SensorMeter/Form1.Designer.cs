namespace SensorMeter
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.OutputWindow = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmbCOMPort = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(197, 12);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPort.TabIndex = 0;
            this.btnOpenPort.Text = "Open Port";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // OutputWindow
            // 
            this.OutputWindow.Location = new System.Drawing.Point(12, 43);
            this.OutputWindow.Name = "OutputWindow";
            this.OutputWindow.Size = new System.Drawing.Size(260, 96);
            this.OutputWindow.TabIndex = 1;
            this.OutputWindow.Text = "";
            // 
            // timer1
            // 
            // cmbCOMPort
            // 
            this.cmbCOMPort.FormattingEnabled = true;
            this.cmbCOMPort.Location = new System.Drawing.Point(13, 13);
            this.cmbCOMPort.Name = "cmbCOMPort";
            this.cmbCOMPort.Size = new System.Drawing.Size(121, 21);
            this.cmbCOMPort.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.cmbCOMPort);
            this.Controls.Add(this.OutputWindow);
            this.Controls.Add(this.btnOpenPort);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.RichTextBox OutputWindow;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cmbCOMPort;
    }
}

