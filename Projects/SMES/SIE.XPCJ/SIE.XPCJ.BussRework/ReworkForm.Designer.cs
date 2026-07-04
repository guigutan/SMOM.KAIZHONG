
namespace SIE.XPCJ.BussRework
{
    partial class ReworkForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.xpCardListPanel1 = new SIE.XPCJ.Common.Controls.XPCardListPanel();
            this.keyItemCard1 = new SIE.XPCJ.BussRework.KeyItemCard();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.xpCollectionRecordsGrid1 = new SIE.XPCJ.Common.Controls.XPCollectionRecordsGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.xpMessageList1 = new SIE.XPCJ.Common.Controls.XPMessageList();
            this.xpTabControlHeader1 = new SIE.XPCJ.Common.Controls.XPTabControlHeader();
            this.panel4 = new System.Windows.Forms.Panel();
            this.xpWorkOrder1 = new SIE.XPCJ.Common.Controls.XPWorkOrder();
            this.panel1 = new System.Windows.Forms.Panel();
            this.xpScanBarcode1 = new SIE.XPCJ.Common.Controls.XPScanBarcode();
            this.xpTitle1 = new SIE.XPCJ.Common.Controls.XPTitle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.xpButtonSubmit = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnReset = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSwitchWo = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSetting = new SIE.XPCJ.Common.Controls.XPButton();
            this.bottomBarCtr1 = new SIE.XPCJ.Common.Controls.XPBottomBar();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.xpCardListPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Controls.Add(this.xpTabControlHeader1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 275);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 366);
            this.panel2.TabIndex = 16;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 44);
            this.tabControl1.Location = new System.Drawing.Point(0, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1024, 318);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.xpCardListPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 48);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1016, 266);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "关键件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // xpCardListPanel1
            // 
            this.xpCardListPanel1.ACard = this.keyItemCard1;
            this.xpCardListPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.xpCardListPanel1.Controls.Add(this.keyItemCard1);
            this.xpCardListPanel1.DataSource = null;
            this.xpCardListPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpCardListPanel1.Location = new System.Drawing.Point(3, 3);
            this.xpCardListPanel1.Name = "xpCardListPanel1";
            this.xpCardListPanel1.Size = new System.Drawing.Size(1010, 260);
            this.xpCardListPanel1.TabIndex = 0;
            // 
            // keyItemCard1
            // 
            this.keyItemCard1.Data = null;
            this.keyItemCard1.Dock = System.Windows.Forms.DockStyle.Top;
            this.keyItemCard1.Location = new System.Drawing.Point(0, 0);
            this.keyItemCard1.Name = "keyItemCard1";
            this.keyItemCard1.Size = new System.Drawing.Size(1010, 133);
            this.keyItemCard1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.xpCollectionRecordsGrid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 48);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1016, 266);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "采集记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // xpCollectionRecordsGrid1
            // 
            this.xpCollectionRecordsGrid1.AMaxRowCount = 1000;
            this.xpCollectionRecordsGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpCollectionRecordsGrid1.Location = new System.Drawing.Point(0, 0);
            this.xpCollectionRecordsGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.xpCollectionRecordsGrid1.Name = "xpCollectionRecordsGrid1";
            this.xpCollectionRecordsGrid1.Size = new System.Drawing.Size(1016, 266);
            this.xpCollectionRecordsGrid1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.xpMessageList1);
            this.tabPage3.Location = new System.Drawing.Point(4, 48);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1016, 266);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "消息列表";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // xpMessageList1
            // 
            this.xpMessageList1.AMaxRowCount = 80;
            this.xpMessageList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpMessageList1.Location = new System.Drawing.Point(0, 0);
            this.xpMessageList1.Margin = new System.Windows.Forms.Padding(5);
            this.xpMessageList1.Name = "xpMessageList1";
            this.xpMessageList1.Size = new System.Drawing.Size(1016, 266);
            this.xpMessageList1.TabIndex = 1;
            // 
            // xpTabControlHeader1
            // 
            this.xpTabControlHeader1.AButton1Text = "关键件";
            this.xpTabControlHeader1.AButton2Text = "采集记录";
            this.xpTabControlHeader1.AButton3Text = "消息列表";
            this.xpTabControlHeader1.AButton4Text = "";
            this.xpTabControlHeader1.AButton5Text = "";
            this.xpTabControlHeader1.AButton6Text = "";
            this.xpTabControlHeader1.AButton7Text = "";
            this.xpTabControlHeader1.AButton8Text = "";
            this.xpTabControlHeader1.AButton9Text = "";
            this.xpTabControlHeader1.ASelectedIndex = 0;
            this.xpTabControlHeader1.ATabControl = this.tabControl1;
            this.xpTabControlHeader1.BackColor = System.Drawing.Color.White;
            this.xpTabControlHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpTabControlHeader1.Location = new System.Drawing.Point(0, 0);
            this.xpTabControlHeader1.Margin = new System.Windows.Forms.Padding(4);
            this.xpTabControlHeader1.Name = "xpTabControlHeader1";
            this.xpTabControlHeader1.Size = new System.Drawing.Size(1024, 48);
            this.xpTabControlHeader1.TabIndex = 1;
            this.xpTabControlHeader1.ASelectIndexChanged += new System.EventHandler(this.xpTabControlHeader1_ASelectIndexChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.xpWorkOrder1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 206);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.panel4.Size = new System.Drawing.Size(1024, 69);
            this.panel4.TabIndex = 15;
            // 
            // xpWorkOrder1
            // 
            this.xpWorkOrder1.AColumn = 3;
            this.xpWorkOrder1.AExistMoreInfo = true;
            this.xpWorkOrder1.ALabel1Text = "工单号";
            this.xpWorkOrder1.ALabel2Text = "产品编码";
            this.xpWorkOrder1.ALabel3Text = "当班采集数";
            this.xpWorkOrder1.ALabel4Text = "工位采集数";
            this.xpWorkOrder1.ATextBox1Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox1Text = "";
            this.xpWorkOrder1.ATextBox2Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox2Text = "";
            this.xpWorkOrder1.ATextBox3Font = new System.Drawing.Font("宋体", 32F, System.Drawing.FontStyle.Bold);
            this.xpWorkOrder1.ATextBox3Text = "0";
            this.xpWorkOrder1.ATextBox4Font = new System.Drawing.Font("宋体", 32F, System.Drawing.FontStyle.Bold);
            this.xpWorkOrder1.ATextBox4Text = "0";
            this.xpWorkOrder1.ATextBoxBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xpWorkOrder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpWorkOrder1.Location = new System.Drawing.Point(4, 0);
            this.xpWorkOrder1.Name = "xpWorkOrder1";
            this.xpWorkOrder1.Size = new System.Drawing.Size(1016, 65);
            this.xpWorkOrder1.TabIndex = 0;
            this.xpWorkOrder1.AMoreInfoClick += new System.EventHandler(this.xpWorkOrder1_AMoreInfoClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.xpScanBarcode1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 118);
            this.panel1.TabIndex = 13;
            // 
            // xpScanBarcode1
            // 
            this.xpScanBarcode1.ABarcode = "";
            this.xpScanBarcode1.ALeftSwitchChecked = false;
            this.xpScanBarcode1.ALeftSwitchCheckedIndex = 0;
            this.xpScanBarcode1.ALeftSwitchLeftText = "条码置换";
            this.xpScanBarcode1.ALeftSwitchMiddleText = "条码解绑关键件";
            this.xpScanBarcode1.ALeftSwitchRightText = "关键键解绑";
            this.xpScanBarcode1.ALeftSwitchVisible = true;
            this.xpScanBarcode1.ALeftSwitchWidth = 400;
            this.xpScanBarcode1.ARightSwitchChecked = false;
            this.xpScanBarcode1.ARightSwitchCheckedIndex = 0;
            this.xpScanBarcode1.ARightSwitchLeftText = "合格";
            this.xpScanBarcode1.ARightSwitchMiddleText = "中间";
            this.xpScanBarcode1.ARightSwitchRightText = "不合格";
            this.xpScanBarcode1.ARightSwitchVisible = false;
            this.xpScanBarcode1.ARightSwitchWidth = 190;
            this.xpScanBarcode1.ATips = "请扫描条码";
            this.xpScanBarcode1.ATipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(203)))), ((int)(((byte)(106)))));
            this.xpScanBarcode1.ATipsFont = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpScanBarcode1.BackColor = System.Drawing.SystemColors.Control;
            this.xpScanBarcode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpScanBarcode1.Location = new System.Drawing.Point(0, 0);
            this.xpScanBarcode1.Name = "xpScanBarcode1";
            this.xpScanBarcode1.Padding = new System.Windows.Forms.Padding(4);
            this.xpScanBarcode1.Size = new System.Drawing.Size(1024, 118);
            this.xpScanBarcode1.TabIndex = 0;
            this.xpScanBarcode1.ALeftSwitchCheckedChanged += new System.EventHandler(this.xpScanBarcode1_ALeftSwitchCheckedChanged);
            this.xpScanBarcode1.ABarcodeChanged += new System.EventHandler(this.xpScanBarcode1_ABarcodeChanged);
            // 
            // xpTitle1
            // 
            this.xpTitle1.AInvOrg = "    库存组织";
            this.xpTitle1.AProcessType = SIE.XPCJ.Models.Enums.ProcessType.Rework;
            this.xpTitle1.ATileFont = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Bold);
            this.xpTitle1.ATitle = "返工采集";
            this.xpTitle1.AType = SIE.XPCJ.Common.Controls.EnumXPTitleType.WorkerCell;
            this.xpTitle1.AUserInfo = "当前用户";
            this.xpTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpTitle1.Location = new System.Drawing.Point(0, 0);
            this.xpTitle1.Name = "xpTitle1";
            this.xpTitle1.Size = new System.Drawing.Size(1024, 88);
            this.xpTitle1.TabIndex = 11;
            this.xpTitle1.AWorkCellChanged += new System.EventHandler(this.xpTitle1_AWorkCellChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.checkBoxAll);
            this.panel3.Controls.Add(this.xpButtonSubmit);
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Controls.Add(this.btnSwitchWo);
            this.panel3.Controls.Add(this.btnSetting);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 641);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1024, 56);
            this.panel3.TabIndex = 10;
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Font = new System.Drawing.Font("宋体", 14F);
            this.checkBoxAll.Location = new System.Drawing.Point(12, 17);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(66, 23);
            this.checkBoxAll.TabIndex = 5;
            this.checkBoxAll.Text = "全选";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // xpButtonSubmit
            // 
            this.xpButtonSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.xpButtonSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.xpButtonSubmit.FlatAppearance.BorderSize = 0;
            this.xpButtonSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonSubmit.ForeColor = System.Drawing.Color.White;
            this.xpButtonSubmit.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButtonSubmit.ForeColorEnable = System.Drawing.Color.White;
            this.xpButtonSubmit.IsPrivilegeAllow = true;
            this.xpButtonSubmit.Location = new System.Drawing.Point(521, 4);
            this.xpButtonSubmit.Name = "xpButtonSubmit";
            this.xpButtonSubmit.PrivilegeName = null;
            this.xpButtonSubmit.Size = new System.Drawing.Size(120, 49);
            this.xpButtonSubmit.TabIndex = 4;
            this.xpButtonSubmit.Text = "提交";
            this.xpButtonSubmit.UseVisualStyleBackColor = false;
            this.xpButtonSubmit.Click += new System.EventHandler(this.xpButtonSubmit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.ForeColorDisable = System.Drawing.Color.Silver;
            this.btnReset.ForeColorEnable = System.Drawing.Color.White;
            this.btnReset.IsPrivilegeAllow = true;
            this.btnReset.Location = new System.Drawing.Point(648, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.PrivilegeName = null;
            this.btnReset.Size = new System.Drawing.Size(116, 49);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重新开始";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSwitchWo
            // 
            this.btnSwitchWo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitchWo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.btnSwitchWo.FlatAppearance.BorderSize = 0;
            this.btnSwitchWo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchWo.ForeColor = System.Drawing.Color.White;
            this.btnSwitchWo.ForeColorDisable = System.Drawing.Color.Silver;
            this.btnSwitchWo.ForeColorEnable = System.Drawing.Color.White;
            this.btnSwitchWo.IsPrivilegeAllow = true;
            this.btnSwitchWo.Location = new System.Drawing.Point(771, 4);
            this.btnSwitchWo.Name = "btnSwitchWo";
            this.btnSwitchWo.PrivilegeName = null;
            this.btnSwitchWo.Size = new System.Drawing.Size(134, 49);
            this.btnSwitchWo.TabIndex = 1;
            this.btnSwitchWo.Text = "切换在制工单";
            this.btnSwitchWo.UseVisualStyleBackColor = false;
            this.btnSwitchWo.Click += new System.EventHandler(this.btnSwitchWo_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.ForeColor = System.Drawing.Color.White;
            this.btnSetting.ForeColorDisable = System.Drawing.Color.Silver;
            this.btnSetting.ForeColorEnable = System.Drawing.Color.White;
            this.btnSetting.IsPrivilegeAllow = true;
            this.btnSetting.Location = new System.Drawing.Point(913, 4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.PrivilegeName = null;
            this.btnSetting.Size = new System.Drawing.Size(102, 49);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "配置项";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // bottomBarCtr1
            // 
            this.bottomBarCtr1.BackColor = System.Drawing.SystemColors.Control;
            this.bottomBarCtr1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomBarCtr1.Location = new System.Drawing.Point(0, 697);
            this.bottomBarCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.bottomBarCtr1.Name = "bottomBarCtr1";
            this.bottomBarCtr1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.bottomBarCtr1.Size = new System.Drawing.Size(1024, 32);
            this.bottomBarCtr1.TabIndex = 12;
            // 
            // ReworkForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1024, 729);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.xpTitle1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.bottomBarCtr1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReworkForm";
            this.Text = "新包装采集";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReworkForm_Load);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.xpCardListPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private SIE.XPCJ.Common.Controls.XPButton btnSwitchWo;
        private SIE.XPCJ.Common.Controls.XPButton btnSetting;
        private SIE.XPCJ.Common.Controls.XPButton btnReset;
        private Common.Controls.XPTitle xpTitle1;
        private Common.Controls.XPBottomBar bottomBarCtr1;
        private System.Windows.Forms.Panel panel1;
        private Common.Controls.XPScanBarcode xpScanBarcode1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Common.Controls.XPTabControlHeader xpTabControlHeader1;
        private System.Windows.Forms.TabPage tabPage2;
        private Common.Controls.XPCollectionRecordsGrid xpCollectionRecordsGrid1;
        private Common.Controls.XPButton xpButtonSubmit;
        private Common.Controls.XPWorkOrder xpWorkOrder1;
        private Common.Controls.XPCardListPanel xpCardListPanel1;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.TabPage tabPage3;
        private Common.Controls.XPMessageList xpMessageList1;
        private KeyItemCard keyItemCard1;
    }
}