
namespace SIE.XPCJ.Common.Controls
{
    partial class XPTitle
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.xpButtonExit = new SIE.XPCJ.Common.Controls.XPButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelWorkCell = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.xpButtonChangeWorkCell = new SIE.XPCJ.Common.Controls.XPButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelWorkerCellLine = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelWorkCellProcess = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelWorkCellPosition = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelDefault = new System.Windows.Forms.Panel();
            this.labelUserInfo = new System.Windows.Forms.Label();
            this.xpButtonInvOrg = new SIE.XPCJ.Common.Controls.XPButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelWorkCell.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelDefault.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.xpButtonExit, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 88);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(203, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(110, 88);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xpButtonExit
            // 
            this.xpButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.xpButtonExit.BackgroundImage = global::SIE.XPCJ.Common.Properties.Resources.退出_;
            this.xpButtonExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.xpButtonExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpButtonExit.FlatAppearance.BorderSize = 0;
            this.xpButtonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.xpButtonExit.IsPrivilegeAllow = true;
            this.xpButtonExit.Location = new System.Drawing.Point(946, 3);
            this.xpButtonExit.Name = "xpButtonExit";
            this.xpButtonExit.PrivilegeName = null;
            this.xpButtonExit.Size = new System.Drawing.Size(75, 82);
            this.xpButtonExit.TabIndex = 6;
            this.xpButtonExit.UseVisualStyleBackColor = false;
            this.xpButtonExit.Click += new System.EventHandler(this.xpButtonExit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::SIE.XPCJ.Common.Properties.Resources.SMOM_logo_1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(194, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelWorkCell);
            this.panel1.Controls.Add(this.panelDefault);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(319, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(621, 82);
            this.panel1.TabIndex = 8;
            // 
            // panelWorkCell
            // 
            this.panelWorkCell.Controls.Add(this.tableLayoutPanel2);
            this.panelWorkCell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkCell.Location = new System.Drawing.Point(0, 0);
            this.panelWorkCell.Name = "panelWorkCell";
            this.panelWorkCell.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.panelWorkCell.Size = new System.Drawing.Size(621, 82);
            this.panelWorkCell.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Controls.Add(this.xpButtonChangeWorkCell, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 10);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(621, 62);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // xpButtonChangeWorkCell
            // 
            this.xpButtonChangeWorkCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.xpButtonChangeWorkCell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpButtonChangeWorkCell.FlatAppearance.BorderSize = 0;
            this.xpButtonChangeWorkCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonChangeWorkCell.Image = global::SIE.XPCJ.Common.Properties.Resources.切换;
            this.xpButtonChangeWorkCell.IsPrivilegeAllow = true;
            this.xpButtonChangeWorkCell.Location = new System.Drawing.Point(560, 0);
            this.xpButtonChangeWorkCell.Margin = new System.Windows.Forms.Padding(0);
            this.xpButtonChangeWorkCell.Name = "xpButtonChangeWorkCell";
            this.xpButtonChangeWorkCell.PrivilegeName = null;
            this.xpButtonChangeWorkCell.Size = new System.Drawing.Size(61, 62);
            this.xpButtonChangeWorkCell.TabIndex = 4;
            this.xpButtonChangeWorkCell.UseVisualStyleBackColor = false;
            this.xpButtonChangeWorkCell.Click += new System.EventHandler(this.xpButtonChangeWorkCell_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelWorkerCellLine);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(180, 56);
            this.panel2.TabIndex = 7;
            // 
            // labelWorkerCellLine
            // 
            this.labelWorkerCellLine.AutoEllipsis = true;
            this.labelWorkerCellLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWorkerCellLine.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWorkerCellLine.ForeColor = System.Drawing.Color.White;
            this.labelWorkerCellLine.Location = new System.Drawing.Point(33, 0);
            this.labelWorkerCellLine.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkerCellLine.Name = "labelWorkerCellLine";
            this.labelWorkerCellLine.Size = new System.Drawing.Size(147, 56);
            this.labelWorkerCellLine.TabIndex = 4;
            this.labelWorkerCellLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelWorkerCellLine.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label1.Image = global::SIE.XPCJ.Common.Properties.Resources.产线;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 56);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelWorkCellProcess);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(189, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(181, 56);
            this.panel3.TabIndex = 8;
            // 
            // labelWorkCellProcess
            // 
            this.labelWorkCellProcess.AutoEllipsis = true;
            this.labelWorkCellProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWorkCellProcess.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWorkCellProcess.ForeColor = System.Drawing.Color.White;
            this.labelWorkCellProcess.Location = new System.Drawing.Point(33, 0);
            this.labelWorkCellProcess.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkCellProcess.Name = "labelWorkCellProcess";
            this.labelWorkCellProcess.Size = new System.Drawing.Size(148, 56);
            this.labelWorkCellProcess.TabIndex = 5;
            this.labelWorkCellProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelWorkCellProcess.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label2.Image = global::SIE.XPCJ.Common.Properties.Resources.工序;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(4);
            this.label2.Size = new System.Drawing.Size(33, 56);
            this.label2.TabIndex = 3;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelWorkCellPosition);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(376, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(181, 56);
            this.panel4.TabIndex = 9;
            // 
            // labelWorkCellPosition
            // 
            this.labelWorkCellPosition.AutoEllipsis = true;
            this.labelWorkCellPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWorkCellPosition.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWorkCellPosition.ForeColor = System.Drawing.Color.White;
            this.labelWorkCellPosition.Location = new System.Drawing.Point(33, 0);
            this.labelWorkCellPosition.Margin = new System.Windows.Forms.Padding(0);
            this.labelWorkCellPosition.Name = "labelWorkCellPosition";
            this.labelWorkCellPosition.Size = new System.Drawing.Size(148, 56);
            this.labelWorkCellPosition.TabIndex = 5;
            this.labelWorkCellPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelWorkCellPosition.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(28)))), ((int)(((byte)(60)))));
            this.label3.Image = global::SIE.XPCJ.Common.Properties.Resources.工位;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 56);
            this.label3.TabIndex = 4;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Click += new System.EventHandler(this.labelWorkCell_Click);
            // 
            // panelDefault
            // 
            this.panelDefault.Controls.Add(this.labelUserInfo);
            this.panelDefault.Controls.Add(this.xpButtonInvOrg);
            this.panelDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefault.Location = new System.Drawing.Point(0, 0);
            this.panelDefault.Name = "panelDefault";
            this.panelDefault.Size = new System.Drawing.Size(621, 82);
            this.panelDefault.TabIndex = 3;
            // 
            // labelUserInfo
            // 
            this.labelUserInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelUserInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.labelUserInfo.ForeColor = System.Drawing.Color.White;
            this.labelUserInfo.Location = new System.Drawing.Point(189, 0);
            this.labelUserInfo.Name = "labelUserInfo";
            this.labelUserInfo.Size = new System.Drawing.Size(300, 82);
            this.labelUserInfo.TabIndex = 3;
            this.labelUserInfo.Text = "当前用户";
            this.labelUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // xpButtonInvOrg
            // 
            this.xpButtonInvOrg.AutoSize = true;
            this.xpButtonInvOrg.Dock = System.Windows.Forms.DockStyle.Right;
            this.xpButtonInvOrg.FlatAppearance.BorderSize = 0;
            this.xpButtonInvOrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonInvOrg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.xpButtonInvOrg.ForeColor = System.Drawing.Color.White;
            this.xpButtonInvOrg.Image = global::SIE.XPCJ.Common.Properties.Resources.组织;
            this.xpButtonInvOrg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.xpButtonInvOrg.IsPrivilegeAllow = true;
            this.xpButtonInvOrg.Location = new System.Drawing.Point(489, 0);
            this.xpButtonInvOrg.Name = "xpButtonInvOrg";
            this.xpButtonInvOrg.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.xpButtonInvOrg.PrivilegeName = null;
            this.xpButtonInvOrg.Size = new System.Drawing.Size(132, 82);
            this.xpButtonInvOrg.TabIndex = 2;
            this.xpButtonInvOrg.Text = "    库存组织";
            this.xpButtonInvOrg.UseVisualStyleBackColor = true;
            this.xpButtonInvOrg.Click += new System.EventHandler(this.xpButtonInvOrg_Click);
            // 
            // XPTitle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "XPTitle";
            this.Size = new System.Drawing.Size(1024, 88);
            this.Load += new System.EventHandler(this.XPTitle_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panelWorkCell.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panelDefault.ResumeLayout(false);
            this.panelDefault.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelTitle;
        private XPButton xpButtonExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelWorkCell;
        private System.Windows.Forms.Panel panelDefault;
        private System.Windows.Forms.Label labelUserInfo;
        private XPButton xpButtonInvOrg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private XPButton xpButtonChangeWorkCell;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelWorkerCellLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelWorkCellProcess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelWorkCellPosition;
        private System.Windows.Forms.Label label3;
    }
}
