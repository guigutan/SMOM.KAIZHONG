
namespace SIE.XPCJ.Common.Controls
{
    partial class XPBottomBar
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelCompanyInfo = new System.Windows.Forms.Label();
            this.labelNetStatus = new System.Windows.Forms.Label();
            this.labelLocalIP = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelDate = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCompanyInfo
            // 
            this.labelCompanyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCompanyInfo.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCompanyInfo.Location = new System.Drawing.Point(520, 0);
            this.labelCompanyInfo.Name = "labelCompanyInfo";
            this.labelCompanyInfo.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.labelCompanyInfo.Size = new System.Drawing.Size(501, 24);
            this.labelCompanyInfo.TabIndex = 0;
            this.labelCompanyInfo.Text = "V1.0.0.1 Copyright © 2024 广州赛意信息科技股份有限公司";
            this.labelCompanyInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNetStatus
            // 
            this.labelNetStatus.AutoSize = true;
            this.labelNetStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNetStatus.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelNetStatus.Location = new System.Drawing.Point(230, 0);
            this.labelNetStatus.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelNetStatus.Name = "labelNetStatus";
            this.labelNetStatus.Size = new System.Drawing.Size(105, 24);
            this.labelNetStatus.TabIndex = 4;
            this.labelNetStatus.Text = "网络连接不可用";
            this.labelNetStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLocalIP
            // 
            this.labelLocalIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalIP.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLocalIP.Location = new System.Drawing.Point(374, 0);
            this.labelLocalIP.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelLocalIP.Name = "labelLocalIP";
            this.labelLocalIP.Size = new System.Drawing.Size(140, 24);
            this.labelLocalIP.TabIndex = 7;
            this.labelLocalIP.Text = "IP:192.168.177.166";
            this.labelLocalIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelDate
            // 
            this.labelDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDate.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDate.Location = new System.Drawing.Point(36, 0);
            this.labelDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(82, 24);
            this.labelDate.TabIndex = 10;
            this.labelDate.Text = "2024-05-06";
            this.labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTime
            // 
            this.labelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTime.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTime.Location = new System.Drawing.Point(121, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(70, 24);
            this.labelTime.TabIndex = 12;
            this.labelTime.Text = "15:44:40";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelLocalIP, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelCompanyInfo, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelNetStatus, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTime, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 24);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame3;
            this.pictureBox3.Location = new System.Drawing.Point(341, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(30, 18);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame2;
            this.pictureBox2.Location = new System.Drawing.Point(197, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 18);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // XPBottomBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "XPBottomBar";
            this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.Size = new System.Drawing.Size(1024, 32);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCompanyInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelNetStatus;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelLocalIP;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
