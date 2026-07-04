
using SIE.XPCJ.Common.Controls;

namespace SIE.XPCJ.BussLoadItems
{
    partial class LoadItemsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.xpTitle1 = new SIE.XPCJ.Common.Controls.XPTitle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.xpbtnOneUnloaditem = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnReset = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSwitchWo = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSetting = new SIE.XPCJ.Common.Controls.XPButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bottomBarCtr1 = new SIE.XPCJ.Common.Controls.XPBottomBar();
            this.panel5 = new System.Windows.Forms.Panel();
            this.xpScanBarcode1 = new SIE.XPCJ.Common.Controls.XPScanBarcode();
            this.panel6 = new System.Windows.Forms.Panel();
            this.xpWorkOrder1 = new SIE.XPCJ.Common.Controls.XPWorkOrder();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.assemblyListGridCtr1 = new SIE.XPCJ.BussLoadItems.AssemblyListGridCtr();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.loadItemPanelListCtr1 = new SIE.XPCJ.Common.Controls.LoadItemPanelListCtr();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.unloadItemsListCtr1 = new SIE.XPCJ.Common.Controls.UnloadItemsListCtr();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.collectionRecordsGridCtr1 = new SIE.XPCJ.Common.Controls.XPCollectionRecordsGrid();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tasksListCtr1 = new SIE.XPCJ.Common.Controls.XPTasksList();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.messageListCtr1 = new SIE.XPCJ.Common.Controls.XPMessageList();
            this.xpTabControlHeader1 = new SIE.XPCJ.Common.Controls.XPTabControlHeader();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.xpTitle1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 88);
            this.panel1.TabIndex = 1;
            // 
            // xpTitle1
            // 
            this.xpTitle1.AInvOrg = "    库存组织";
            this.xpTitle1.AProcessType = SIE.XPCJ.Models.Enums.ProcessType.Assembly;
            this.xpTitle1.ATileFont = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Bold);
            this.xpTitle1.ATitle = "上料采集";
            this.xpTitle1.AType = SIE.XPCJ.Common.Controls.EnumXPTitleType.WorkerCell;
            this.xpTitle1.AUserInfo = "当前用户";
            this.xpTitle1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpTitle1.Location = new System.Drawing.Point(0, 0);
            this.xpTitle1.Name = "xpTitle1";
            this.xpTitle1.Size = new System.Drawing.Size(1008, 88);
            this.xpTitle1.TabIndex = 0;
            this.xpTitle1.AExitClick += new System.EventHandler(this.xpTitle1_AExitClick);
            this.xpTitle1.AWorkCellChanged += new System.EventHandler(this.xpTitle1_AWorkCellChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.xpbtnOneUnloaditem);
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Controls.Add(this.btnSwitchWo);
            this.panel3.Controls.Add(this.btnSetting);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 645);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 56);
            this.panel3.TabIndex = 10;
            // 
            // xpbtnOneUnloaditem
            // 
            this.xpbtnOneUnloaditem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.xpbtnOneUnloaditem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.xpbtnOneUnloaditem.FlatAppearance.BorderSize = 0;
            this.xpbtnOneUnloaditem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpbtnOneUnloaditem.ForeColor = System.Drawing.Color.White;
            this.xpbtnOneUnloaditem.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpbtnOneUnloaditem.ForeColorEnable = System.Drawing.Color.White;
            this.xpbtnOneUnloaditem.IsPrivilegeAllow = true;
            this.xpbtnOneUnloaditem.Location = new System.Drawing.Point(504, 5);
            this.xpbtnOneUnloaditem.Name = "xpbtnOneUnloaditem";
            this.xpbtnOneUnloaditem.PrivilegeName = null;
            this.xpbtnOneUnloaditem.Size = new System.Drawing.Size(120, 49);
            this.xpbtnOneUnloaditem.TabIndex = 4;
            this.xpbtnOneUnloaditem.Text = "一键下料";
            this.xpbtnOneUnloaditem.UseVisualStyleBackColor = false;
            this.xpbtnOneUnloaditem.Visible = false;
            this.xpbtnOneUnloaditem.Click += new System.EventHandler(this.xpButton2_Click_1);
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
            this.btnReset.Location = new System.Drawing.Point(632, 4);
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
            this.btnSwitchWo.Location = new System.Drawing.Point(755, 4);
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
            this.btnSetting.Location = new System.Drawing.Point(897, 4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.PrivilegeName = null;
            this.btnSetting.Size = new System.Drawing.Size(102, 49);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "配置项";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PaleTurquoise;
            this.panel2.Controls.Add(this.bottomBarCtr1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 701);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 28);
            this.panel2.TabIndex = 9;
            // 
            // bottomBarCtr1
            // 
            this.bottomBarCtr1.BackColor = System.Drawing.SystemColors.Control;
            this.bottomBarCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomBarCtr1.Location = new System.Drawing.Point(0, 0);
            this.bottomBarCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.bottomBarCtr1.Name = "bottomBarCtr1";
            this.bottomBarCtr1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.bottomBarCtr1.Size = new System.Drawing.Size(1008, 28);
            this.bottomBarCtr1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.xpScanBarcode1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 88);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(5, 5, 10, 5);
            this.panel5.Size = new System.Drawing.Size(1008, 127);
            this.panel5.TabIndex = 11;
            // 
            // xpScanBarcode1
            // 
            this.xpScanBarcode1.ABarcode = "";
            this.xpScanBarcode1.ALeftSwitchChecked = false;
            this.xpScanBarcode1.ALeftSwitchCheckedIndex = 0;
            this.xpScanBarcode1.ALeftSwitchLeftText = "装配";
            this.xpScanBarcode1.ALeftSwitchMiddleText = "";
            this.xpScanBarcode1.ALeftSwitchRightText = "上料";
            this.xpScanBarcode1.ALeftSwitchVisible = true;
            this.xpScanBarcode1.ALeftSwitchWidth = 162;
            this.xpScanBarcode1.ARightSwitchChecked = false;
            this.xpScanBarcode1.ARightSwitchCheckedIndex = 0;
            this.xpScanBarcode1.ARightSwitchLeftText = "合格";
            this.xpScanBarcode1.ARightSwitchMiddleText = "";
            this.xpScanBarcode1.ARightSwitchRightText = "不合格";
            this.xpScanBarcode1.ARightSwitchVisible = false;
            this.xpScanBarcode1.ARightSwitchWidth = 190;
            this.xpScanBarcode1.ATips = "请扫描条码";
            this.xpScanBarcode1.ATipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(203)))), ((int)(((byte)(106)))));
            this.xpScanBarcode1.ATipsFont = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpScanBarcode1.BackColor = System.Drawing.Color.White;
            this.xpScanBarcode1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpScanBarcode1.Location = new System.Drawing.Point(5, 5);
            this.xpScanBarcode1.Name = "xpScanBarcode1";
            this.xpScanBarcode1.Padding = new System.Windows.Forms.Padding(4);
            this.xpScanBarcode1.Size = new System.Drawing.Size(993, 117);
            this.xpScanBarcode1.TabIndex = 0;
            this.xpScanBarcode1.TipsOverFontSize = 18;
            this.xpScanBarcode1.ABarcodeChanged += new System.EventHandler(this.scanBracodeCtr1_ABarCodeChanged);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.xpWorkOrder1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 215);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panel6.Size = new System.Drawing.Size(1008, 73);
            this.panel6.TabIndex = 14;
            // 
            // xpWorkOrder1
            // 
            this.xpWorkOrder1.AColumn = 2;
            this.xpWorkOrder1.AExistMoreInfo = true;
            this.xpWorkOrder1.ALabel1Text = "工单号";
            this.xpWorkOrder1.ALabel2Text = "产品编码";
            this.xpWorkOrder1.ALabel3Text = "当班采集数";
            this.xpWorkOrder1.ALabel4Text = "当班不良数";
            this.xpWorkOrder1.ATextBox1Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox1Text = "";
            this.xpWorkOrder1.ATextBox2Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox2Text = "";
            this.xpWorkOrder1.ATextBox3Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold);
            this.xpWorkOrder1.ATextBox3Text = "0";
            this.xpWorkOrder1.ATextBox4Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold);
            this.xpWorkOrder1.ATextBox4Text = "0";
            this.xpWorkOrder1.ATextBoxBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xpWorkOrder1.BackColor = System.Drawing.Color.White;
            this.xpWorkOrder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpWorkOrder1.Location = new System.Drawing.Point(10, 5);
            this.xpWorkOrder1.Name = "xpWorkOrder1";
            this.xpWorkOrder1.Size = new System.Drawing.Size(988, 63);
            this.xpWorkOrder1.TabIndex = 11;
            this.xpWorkOrder1.AMoreInfoClick += new System.EventHandler(this.xpWorkOrder1_AMoreInfoClick);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tabControl1);
            this.panel7.Controls.Add(this.xpTabControlHeader1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 288);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panel7.Size = new System.Drawing.Size(1008, 357);
            this.panel7.TabIndex = 15;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 44);
            this.tabControl1.Location = new System.Drawing.Point(10, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(988, 299);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.assemblyListGridCtr1);
            this.tabPage1.Location = new System.Drawing.Point(4, 48);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(980, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "装配清单";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // assemblyListGridCtr1
            // 
            this.assemblyListGridCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assemblyListGridCtr1.Location = new System.Drawing.Point(3, 3);
            this.assemblyListGridCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.assemblyListGridCtr1.Name = "assemblyListGridCtr1";
            this.assemblyListGridCtr1.Padding = new System.Windows.Forms.Padding(11);
            this.assemblyListGridCtr1.Size = new System.Drawing.Size(974, 241);
            this.assemblyListGridCtr1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.loadItemPanelListCtr1);
            this.tabPage2.Location = new System.Drawing.Point(4, 48);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(980, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "上料明细";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // loadItemPanelListCtr1
            // 
            this.loadItemPanelListCtr1.AutoScroll = true;
            this.loadItemPanelListCtr1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.loadItemPanelListCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadItemPanelListCtr1.Location = new System.Drawing.Point(3, 3);
            this.loadItemPanelListCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.loadItemPanelListCtr1.Name = "loadItemPanelListCtr1";
            this.loadItemPanelListCtr1.ReflashLoadItemListAction = null;
            this.loadItemPanelListCtr1.ReflashUnLoadItemListAction = null;
            this.loadItemPanelListCtr1.Size = new System.Drawing.Size(974, 241);
            this.loadItemPanelListCtr1.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.unloadItemsListCtr1);
            this.tabPage3.Location = new System.Drawing.Point(4, 48);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(980, 247);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "下料明细";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // unloadItemsListCtr1
            // 
            this.unloadItemsListCtr1.AutoScroll = true;
            this.unloadItemsListCtr1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.unloadItemsListCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unloadItemsListCtr1.Location = new System.Drawing.Point(0, 0);
            this.unloadItemsListCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.unloadItemsListCtr1.Name = "unloadItemsListCtr1";
            this.unloadItemsListCtr1.Size = new System.Drawing.Size(980, 247);
            this.unloadItemsListCtr1.TabIndex = 4;
            this.unloadItemsListCtr1.UnloadItems = null;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.collectionRecordsGridCtr1);
            this.tabPage4.Location = new System.Drawing.Point(4, 48);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(980, 247);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "采集记录";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // collectionRecordsGridCtr1
            // 
            this.collectionRecordsGridCtr1.AMaxRowCount = 1000;
            this.collectionRecordsGridCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionRecordsGridCtr1.Location = new System.Drawing.Point(0, 0);
            this.collectionRecordsGridCtr1.Margin = new System.Windows.Forms.Padding(7);
            this.collectionRecordsGridCtr1.Name = "collectionRecordsGridCtr1";
            this.collectionRecordsGridCtr1.Padding = new System.Windows.Forms.Padding(13);
            this.collectionRecordsGridCtr1.Size = new System.Drawing.Size(980, 247);
            this.collectionRecordsGridCtr1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tasksListCtr1);
            this.tabPage5.Location = new System.Drawing.Point(4, 48);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(980, 247);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "任务列表";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tasksListCtr1
            // 
            this.tasksListCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tasksListCtr1.Location = new System.Drawing.Point(0, 0);
            this.tasksListCtr1.Margin = new System.Windows.Forms.Padding(4);
            this.tasksListCtr1.Name = "tasksListCtr1";
            this.tasksListCtr1.Size = new System.Drawing.Size(980, 247);
            this.tasksListCtr1.TabIndex = 2;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.messageListCtr1);
            this.tabPage6.Location = new System.Drawing.Point(4, 48);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(980, 247);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "消息列表";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // messageListCtr1
            // 
            this.messageListCtr1.AMaxRowCount = 80;
            this.messageListCtr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageListCtr1.Location = new System.Drawing.Point(0, 0);
            this.messageListCtr1.Margin = new System.Windows.Forms.Padding(5);
            this.messageListCtr1.Name = "messageListCtr1";
            this.messageListCtr1.Padding = new System.Windows.Forms.Padding(11);
            this.messageListCtr1.Size = new System.Drawing.Size(980, 247);
            this.messageListCtr1.TabIndex = 1;
            // 
            // xpTabControlHeader1
            // 
            this.xpTabControlHeader1.AButton1Text = "装配清单";
            this.xpTabControlHeader1.AButton2Text = "上料明细";
            this.xpTabControlHeader1.AButton3Text = "下料明细";
            this.xpTabControlHeader1.AButton4Text = "采集记录";
            this.xpTabControlHeader1.AButton5Text = "任务列表";
            this.xpTabControlHeader1.AButton6Text = "消息列表";
            this.xpTabControlHeader1.AButton7Text = "";
            this.xpTabControlHeader1.AButton8Text = "";
            this.xpTabControlHeader1.AButton9Text = "";
            this.xpTabControlHeader1.ASelectedIndex = 0;
            this.xpTabControlHeader1.ATabControl = this.tabControl1;
            this.xpTabControlHeader1.BackColor = System.Drawing.Color.White;
            this.xpTabControlHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpTabControlHeader1.Location = new System.Drawing.Point(10, 5);
            this.xpTabControlHeader1.Margin = new System.Windows.Forms.Padding(4);
            this.xpTabControlHeader1.Name = "xpTabControlHeader1";
            this.xpTabControlHeader1.Size = new System.Drawing.Size(988, 48);
            this.xpTabControlHeader1.TabIndex = 17;
            this.xpTabControlHeader1.ASelectIndexChanged += new System.EventHandler(this.xpTabControlHeader1_ASelectIndexChanged);
            // 
            // LoadItemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.ControlBox = false;
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadItemsForm";
            this.Text = "上料采集";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MoveForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private SIE.XPCJ.Common.Controls.XPButton btnSwitchWo;
        private SIE.XPCJ.Common.Controls.XPButton btnSetting;
        private SIE.XPCJ.Common.Controls.XPButton btnReset;
        private System.Windows.Forms.Panel panel2;
        private SIE.XPCJ.Common.Controls.XPBottomBar bottomBarCtr1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private XPTasksList tasksListCtr1;
        private SIE.XPCJ.Common.Controls.XPMessageList messageListCtr1;
        private SIE.XPCJ.Common.Controls.XPCollectionRecordsGrid collectionRecordsGridCtr1;
        private SIE.XPCJ.BussLoadItems.AssemblyListGridCtr assemblyListGridCtr1;
        private SIE.XPCJ.Common.Controls.UnloadItemsListCtr unloadItemsListCtr1;
        private SIE.XPCJ.Common.Controls.LoadItemPanelListCtr loadItemPanelListCtr1;
        private SIE.XPCJ.Common.Controls.XPButton xpbtnOneUnloaditem;
        private Common.Controls.XPTitle xpTitle1;
        private XPScanBarcode xpScanBarcode1;
        private XPWorkOrder xpWorkOrder1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private XPTabControlHeader xpTabControlHeader1;
    }
}