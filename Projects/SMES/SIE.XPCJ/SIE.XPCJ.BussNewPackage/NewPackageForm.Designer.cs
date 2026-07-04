
namespace SIE.XPCJ.BussNewPackage
{
    partial class NewPackageForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeGrid1 = new SIE.XPCJ.Common.Controls.TreeGrid.UCDataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.xpDataGridViewRule = new SIE.XPCJ.Common.Controls.XPDataGridView();
            this.colPackageUnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.xpCollectionRecordsGrid1 = new SIE.XPCJ.Common.Controls.XPCollectionRecordsGrid();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.xpMessageList1 = new SIE.XPCJ.Common.Controls.XPMessageList();
            this.xpTabControlHeader1 = new SIE.XPCJ.Common.Controls.XPTabControlHeader();
            this.panel4 = new System.Windows.Forms.Panel();
            this.xpWorkOrder1 = new SIE.XPCJ.Common.Controls.XPWorkOrderSimple();
            this.panel1 = new System.Windows.Forms.Panel();
            this.xpScanBarcode1 = new SIE.XPCJ.Common.Controls.XPScanBarcode();
            this.xpTitle1 = new SIE.XPCJ.Common.Controls.XPTitle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.xpButtonPackage = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnReset = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSwitchWo = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnSetting = new SIE.XPCJ.Common.Controls.XPButton();
            this.bottomBarCtr1 = new SIE.XPCJ.Common.Controls.XPBottomBar();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xpDataGridViewRule)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
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
            this.panel2.Location = new System.Drawing.Point(0, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 381);
            this.panel2.TabIndex = 16;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 44);
            this.tabControl1.Location = new System.Drawing.Point(0, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1024, 333);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 48);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1016, 281);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeGrid1
            // 
            this.treeGrid1.BackColor = System.Drawing.Color.White;
            this.treeGrid1.Columns = null;
            this.treeGrid1.DataSource = null;
            this.treeGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGrid1.HeadFont = new System.Drawing.Font("微软雅黑", 12F);
            this.treeGrid1.HeadHeight = 40;
            this.treeGrid1.HeadPadingLeft = 0;
            this.treeGrid1.HeadTextColor = System.Drawing.Color.Black;
            this.treeGrid1.IsShowCheckBox = false;
            this.treeGrid1.IsShowChildRowCheckBox = false;
            this.treeGrid1.IsShowHead = true;
            this.treeGrid1.Location = new System.Drawing.Point(3, 3);
            this.treeGrid1.Name = "treeGrid1";
            this.treeGrid1.Padding = new System.Windows.Forms.Padding(0, 40, 0, 0);
            this.treeGrid1.RowHeight = 40;
            this.treeGrid1.RowType = typeof(SIE.XPCJ.Common.Controls.TreeGrid.UCDataGridViewRow);
            this.treeGrid1.Size = new System.Drawing.Size(1010, 275);
            this.treeGrid1.TabIndex = 0;
            this.treeGrid1.ARowCheckedChanged += new SIE.XPCJ.Common.Controls.TreeGrid.DataGridViewEventHandler(this.treeGrid1_ARowCheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.xpDataGridViewRule);
            this.tabPage2.Location = new System.Drawing.Point(4, 48);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1016, 281);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // xpDataGridViewRule
            // 
            this.xpDataGridViewRule.AllowUserToAddRows = false;
            this.xpDataGridViewRule.AllowUserToDeleteRows = false;
            this.xpDataGridViewRule.AllowUserToResizeColumns = false;
            this.xpDataGridViewRule.AllowUserToResizeRows = false;
            this.xpDataGridViewRule.BackgroundColor = System.Drawing.Color.White;
            this.xpDataGridViewRule.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xpDataGridViewRule.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.xpDataGridViewRule.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(5)))), ((int)(((byte)(61)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.xpDataGridViewRule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.xpDataGridViewRule.ColumnHeadersHeight = 45;
            this.xpDataGridViewRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.xpDataGridViewRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPackageUnitName,
            this.colQty,
            this.colWeight,
            this.colHeight,
            this.colVolume,
            this.colLength,
            this.colWidth,
            this.colTemplateName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(5)))), ((int)(((byte)(61)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.xpDataGridViewRule.DefaultCellStyle = dataGridViewCellStyle2;
            this.xpDataGridViewRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpDataGridViewRule.EnableHeadersVisualStyles = false;
            this.xpDataGridViewRule.GridColor = System.Drawing.Color.White;
            this.xpDataGridViewRule.Location = new System.Drawing.Point(3, 3);
            this.xpDataGridViewRule.MultiSelect = false;
            this.xpDataGridViewRule.Name = "xpDataGridViewRule";
            this.xpDataGridViewRule.ReadOnly = true;
            this.xpDataGridViewRule.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.xpDataGridViewRule.RowHeadersVisible = false;
            this.xpDataGridViewRule.RowTemplate.Height = 50;
            this.xpDataGridViewRule.RowTemplate.ReadOnly = true;
            this.xpDataGridViewRule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.xpDataGridViewRule.Size = new System.Drawing.Size(1010, 275);
            this.xpDataGridViewRule.TabIndex = 1;
            this.xpDataGridViewRule.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.xpDataGridViewRule_CellFormatting);
            // 
            // colPackageUnitName
            // 
            this.colPackageUnitName.DataPropertyName = "PackageUnitName";
            this.colPackageUnitName.HeaderText = "包装单位";
            this.colPackageUnitName.Name = "colPackageUnitName";
            this.colPackageUnitName.ReadOnly = true;
            this.colPackageUnitName.Width = 120;
            // 
            // colQty
            // 
            this.colQty.DataPropertyName = "Qty";
            this.colQty.HeaderText = "产品数";
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            this.colQty.Width = 120;
            // 
            // colWeight
            // 
            this.colWeight.DataPropertyName = "Weight";
            this.colWeight.HeaderText = "重量";
            this.colWeight.Name = "colWeight";
            this.colWeight.ReadOnly = true;
            this.colWeight.Width = 120;
            // 
            // colHeight
            // 
            this.colHeight.DataPropertyName = "Height";
            this.colHeight.HeaderText = "高度";
            this.colHeight.Name = "colHeight";
            this.colHeight.ReadOnly = true;
            this.colHeight.Width = 120;
            // 
            // colVolume
            // 
            this.colVolume.DataPropertyName = "Volume";
            this.colVolume.HeaderText = "体积";
            this.colVolume.Name = "colVolume";
            this.colVolume.ReadOnly = true;
            this.colVolume.Width = 120;
            // 
            // colLength
            // 
            this.colLength.DataPropertyName = "Length";
            this.colLength.HeaderText = "长";
            this.colLength.Name = "colLength";
            this.colLength.ReadOnly = true;
            this.colLength.Width = 120;
            // 
            // colWidth
            // 
            this.colWidth.DataPropertyName = "Width";
            this.colWidth.HeaderText = "宽";
            this.colWidth.Name = "colWidth";
            this.colWidth.ReadOnly = true;
            this.colWidth.Width = 120;
            // 
            // colTemplateName
            // 
            this.colTemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTemplateName.DataPropertyName = "TemplateName";
            this.colTemplateName.HeaderText = "打印模板";
            this.colTemplateName.Name = "colTemplateName";
            this.colTemplateName.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.xpCollectionRecordsGrid1);
            this.tabPage3.Location = new System.Drawing.Point(4, 48);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1016, 281);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // xpCollectionRecordsGrid1
            // 
            this.xpCollectionRecordsGrid1.AMaxRowCount = 1000;
            this.xpCollectionRecordsGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpCollectionRecordsGrid1.Location = new System.Drawing.Point(0, 0);
            this.xpCollectionRecordsGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.xpCollectionRecordsGrid1.Name = "xpCollectionRecordsGrid1";
            this.xpCollectionRecordsGrid1.Size = new System.Drawing.Size(1016, 281);
            this.xpCollectionRecordsGrid1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.xpMessageList1);
            this.tabPage4.Location = new System.Drawing.Point(4, 48);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1016, 281);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // xpMessageList1
            // 
            this.xpMessageList1.AMaxRowCount = 30;
            this.xpMessageList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpMessageList1.Location = new System.Drawing.Point(0, 0);
            this.xpMessageList1.Margin = new System.Windows.Forms.Padding(4);
            this.xpMessageList1.Name = "xpMessageList1";
            this.xpMessageList1.Size = new System.Drawing.Size(1016, 281);
            this.xpMessageList1.TabIndex = 0;
            // 
            // xpTabControlHeader1
            // 
            this.xpTabControlHeader1.AButton1Text = "条码明细";
            this.xpTabControlHeader1.AButton2Text = "包装规则";
            this.xpTabControlHeader1.AButton3Text = "采集记录";
            this.xpTabControlHeader1.AButton4Text = "消息列表";
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
            this.panel4.Size = new System.Drawing.Size(1024, 54);
            this.panel4.TabIndex = 15;
            // 
            // xpWorkOrder1
            // 
            this.xpWorkOrder1.AColumn = 3;
            this.xpWorkOrder1.AExistMoreInfo = true;
            this.xpWorkOrder1.ALabel1Text = "产品编码";
            this.xpWorkOrder1.ALabel2Text = "工单号";
            this.xpWorkOrder1.ALabel3Text = "产品名称";
            this.xpWorkOrder1.ATextBox1Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox1Text = "";
            this.xpWorkOrder1.ATextBox2Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox2Text = "";
            this.xpWorkOrder1.ATextBox3Font = new System.Drawing.Font("宋体", 12F);
            this.xpWorkOrder1.ATextBox3Text = "";
            this.xpWorkOrder1.ATextBoxBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xpWorkOrder1.BackColor = System.Drawing.SystemColors.Control;
            this.xpWorkOrder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpWorkOrder1.Location = new System.Drawing.Point(4, 0);
            this.xpWorkOrder1.Name = "xpWorkOrder1";
            this.xpWorkOrder1.Size = new System.Drawing.Size(1016, 50);
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
            this.xpScanBarcode1.ALeftSwitchLeftText = "上料";
            this.xpScanBarcode1.ALeftSwitchMiddleText = "中间";
            this.xpScanBarcode1.ALeftSwitchRightText = "采集";
            this.xpScanBarcode1.ALeftSwitchVisible = false;
            this.xpScanBarcode1.ALeftSwitchWidth = 162;
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
            this.xpScanBarcode1.ABarcodeChanged += new System.EventHandler(this.xpScanBarcode1_ABarcodeChanged);
            // 
            // xpTitle1
            // 
            this.xpTitle1.AInvOrg = "    库存组织";
            this.xpTitle1.AProcessType = SIE.XPCJ.Models.Enums.ProcessType.Assembly;
            this.xpTitle1.ATileFont = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Bold);
            this.xpTitle1.ATitle = "新包装采集";
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
            this.panel3.Controls.Add(this.xpButtonPackage);
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Controls.Add(this.btnSwitchWo);
            this.panel3.Controls.Add(this.btnSetting);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 641);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1024, 56);
            this.panel3.TabIndex = 10;
            // 
            // xpButtonPackage
            // 
            this.xpButtonPackage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.xpButtonPackage.Enabled = false;
            this.xpButtonPackage.FlatAppearance.BorderSize = 0;
            this.xpButtonPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonPackage.ForeColor = System.Drawing.Color.Silver;
            this.xpButtonPackage.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButtonPackage.ForeColorEnable = System.Drawing.Color.White;
            this.xpButtonPackage.IsPrivilegeAllow = true;
            this.xpButtonPackage.Location = new System.Drawing.Point(7, 4);
            this.xpButtonPackage.Name = "xpButtonPackage";
            this.xpButtonPackage.PrivilegeName = null;
            this.xpButtonPackage.Size = new System.Drawing.Size(120, 49);
            this.xpButtonPackage.TabIndex = 4;
            this.xpButtonPackage.Text = "打包";
            this.xpButtonPackage.UseVisualStyleBackColor = false;
            this.xpButtonPackage.Click += new System.EventHandler(this.xpButtonPackage_Click);
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
            // NewPackageForm
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
            this.Name = "NewPackageForm";
            this.Text = "新包装采集";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.NewPackageForm_Load);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xpDataGridViewRule)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
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
        private Common.Controls.XPWorkOrderSimple xpWorkOrder1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private Common.Controls.XPTabControlHeader xpTabControlHeader1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Common.Controls.XPMessageList xpMessageList1;
        private Common.Controls.XPCollectionRecordsGrid xpCollectionRecordsGrid1;
        private Common.Controls.XPDataGridView xpDataGridViewRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageUnitName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemplateName;
        private Common.Controls.TreeGrid.UCDataGridView treeGrid1;
        private Common.Controls.XPButton xpButtonPackage;
    }
}