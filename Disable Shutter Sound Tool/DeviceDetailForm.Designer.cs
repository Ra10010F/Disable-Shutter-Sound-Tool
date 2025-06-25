namespace Disable_Shutter_Sound_Tool
{
    partial class DeviceDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceDetailForm));
            this.labelDeviceDetails = new System.Windows.Forms.Label();
            this.labelShutterKey = new System.Windows.Forms.Label();
            this.buttonE = new System.Windows.Forms.Button();
            this.buttonD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDeviceDetails
            // 
            this.labelDeviceDetails.AutoSize = true;
            this.labelDeviceDetails.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelDeviceDetails.Location = new System.Drawing.Point(16, 15);
            this.labelDeviceDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDeviceDetails.Name = "labelDeviceDetails";
            this.labelDeviceDetails.Size = new System.Drawing.Size(228, 28);
            this.labelDeviceDetails.TabIndex = 0;
            this.labelDeviceDetails.Text = "labelDeviceDetail";
            // 
            // labelShutterKey
            // 
            this.labelShutterKey.AutoSize = true;
            this.labelShutterKey.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelShutterKey.Location = new System.Drawing.Point(16, 52);
            this.labelShutterKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelShutterKey.Name = "labelShutterKey";
            this.labelShutterKey.Size = new System.Drawing.Size(151, 28);
            this.labelShutterKey.TabIndex = 1;
            this.labelShutterKey.Text = "ShutterKey";
            // 
            // buttonE
            // 
            this.buttonE.Enabled = false;
            this.buttonE.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonE.Location = new System.Drawing.Point(21, 95);
            this.buttonE.Margin = new System.Windows.Forms.Padding(4);
            this.buttonE.Name = "buttonE";
            this.buttonE.Size = new System.Drawing.Size(292, 48);
            this.buttonE.TabIndex = 2;
            this.buttonE.Text = "有効化";
            this.buttonE.UseVisualStyleBackColor = true;
            this.buttonE.Click += new System.EventHandler(this.buttonE_Click);
            // 
            // buttonD
            // 
            this.buttonD.Enabled = false;
            this.buttonD.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonD.Location = new System.Drawing.Point(321, 95);
            this.buttonD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonD.Name = "buttonD";
            this.buttonD.Size = new System.Drawing.Size(292, 48);
            this.buttonD.TabIndex = 3;
            this.buttonD.Text = "無効化";
            this.buttonD.UseVisualStyleBackColor = true;
            this.buttonD.Click += new System.EventHandler(this.buttonD_Click);
            // 
            // DeviceDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(644, 162);
            this.Controls.Add(this.buttonD);
            this.Controls.Add(this.buttonE);
            this.Controls.Add(this.labelShutterKey);
            this.Controls.Add(this.labelDeviceDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "DeviceDetailForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDeviceDetails;
        private System.Windows.Forms.Label labelShutterKey;
        private System.Windows.Forms.Button buttonE;
        private System.Windows.Forms.Button buttonD;
    }
}