namespace Disable_Shutter_Sound_Tool
{
    partial class Mainform
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.チュートリアルを再表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonFetchDevices = new System.Windows.Forms.Button();
            this.buttonDetails = new System.Windows.Forms.Button();
            this.labelConnectedDevice = new System.Windows.Forms.Label();
            this.labelConnectedDeviceStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.ヘルプToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(679, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.チュートリアルを再表示ToolStripMenuItem,
            this.終了ToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(65, 26);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // チュートリアルを再表示ToolStripMenuItem
            // 
            this.チュートリアルを再表示ToolStripMenuItem.Name = "チュートリアルを再表示ToolStripMenuItem";
            this.チュートリアルを再表示ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.チュートリアルを再表示ToolStripMenuItem.Text = "チュートリアルを再表示";
            this.チュートリアルを再表示ToolStripMenuItem.Click += new System.EventHandler(this.チュートリアルを再表示ToolStripMenuItem_Click);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.終了ToolStripMenuItem.Text = "終了";
            this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.情報ToolStripMenuItem});
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(58, 26);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            // 
            // 情報ToolStripMenuItem
            // 
            this.情報ToolStripMenuItem.Name = "情報ToolStripMenuItem";
            this.情報ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.情報ToolStripMenuItem.Text = "情報";
            this.情報ToolStripMenuItem.Click += new System.EventHandler(this.情報ToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(16, 38);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 44);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(71, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "ADBコマンドを使用できません。";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLabel1.Location = new System.Drawing.Point(416, 51);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(200, 20);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "ADBをインストールする";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonFetchDevices
            // 
            this.buttonFetchDevices.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonFetchDevices.Location = new System.Drawing.Point(16, 89);
            this.buttonFetchDevices.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFetchDevices.Name = "buttonFetchDevices";
            this.buttonFetchDevices.Size = new System.Drawing.Size(217, 40);
            this.buttonFetchDevices.TabIndex = 7;
            this.buttonFetchDevices.Text = "デバイス情報を更新";
            this.buttonFetchDevices.UseVisualStyleBackColor = true;
            this.buttonFetchDevices.Click += new System.EventHandler(this.buttonFetchDevices_Click);
            // 
            // buttonDetails
            // 
            this.buttonDetails.Enabled = false;
            this.buttonDetails.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonDetails.Location = new System.Drawing.Point(16, 212);
            this.buttonDetails.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDetails.Name = "buttonDetails";
            this.buttonDetails.Size = new System.Drawing.Size(647, 48);
            this.buttonDetails.TabIndex = 9;
            this.buttonDetails.Text = "オプションを表示";
            this.buttonDetails.UseVisualStyleBackColor = true;
            this.buttonDetails.Click += new System.EventHandler(this.buttonDetails_Click);
            // 
            // labelConnectedDevice
            // 
            this.labelConnectedDevice.AutoSize = true;
            this.labelConnectedDevice.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelConnectedDevice.Location = new System.Drawing.Point(16, 145);
            this.labelConnectedDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelConnectedDevice.Name = "labelConnectedDevice";
            this.labelConnectedDevice.Size = new System.Drawing.Size(310, 24);
            this.labelConnectedDevice.TabIndex = 10;
            this.labelConnectedDevice.Text = "デバイス情報を更新してください";
            // 
            // labelConnectedDeviceStatus
            // 
            this.labelConnectedDeviceStatus.AutoSize = true;
            this.labelConnectedDeviceStatus.Font = new System.Drawing.Font("UD デジタル 教科書体 NP", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelConnectedDeviceStatus.Location = new System.Drawing.Point(16, 179);
            this.labelConnectedDeviceStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelConnectedDeviceStatus.Name = "labelConnectedDeviceStatus";
            this.labelConnectedDeviceStatus.Size = new System.Drawing.Size(310, 24);
            this.labelConnectedDeviceStatus.TabIndex = 11;
            this.labelConnectedDeviceStatus.Text = "labelConnectedDeviceStatus";
            this.labelConnectedDeviceStatus.Visible = false;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(679, 272);
            this.Controls.Add(this.labelConnectedDeviceStatus);
            this.Controls.Add(this.labelConnectedDevice);
            this.Controls.Add(this.buttonDetails);
            this.Controls.Add(this.buttonFetchDevices);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Mainform";
            this.Text = "DSST v2.0 (Galaxy端末のみ使用できます)";
            this.Load += new System.EventHandler(this.Mainform_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 情報ToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button buttonFetchDevices;
        private System.Windows.Forms.Button buttonDetails;
        private System.Windows.Forms.Label labelConnectedDevice;
        private System.Windows.Forms.Label labelConnectedDeviceStatus;
        private System.Windows.Forms.ToolStripMenuItem チュートリアルを再表示ToolStripMenuItem;
    }
}

