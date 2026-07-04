
namespace SIE.XPCJ.BussRework
{
    partial class ReworkKeyItemComfrimForm
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
            this.xpDialogTitle1 = new SIE.XPCJ.Common.Controls.XPDialogTitle();
            this.xpSwitch1 = new SIE.XPCJ.Common.Controls.XPSwitch();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelWarehouse = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelWarehouse.SuspendLayout();
            this.SuspendLayout();
            // 
            // xpDialogTitle1
            // 
            this.xpDialogTitle1.ATileFont = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpDialogTitle1.ATitle = "请确定";
            this.xpDialogTitle1.BackColor = System.Drawing.Color.White;
            this.xpDialogTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpDialogTitle1.Location = new System.Drawing.Point(0, 0);
            this.xpDialogTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.xpDialogTitle1.Name = "xpDialogTitle1";
            this.xpDialogTitle1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.xpDialogTitle1.Size = new System.Drawing.Size(575, 58);
            this.xpDialogTitle1.TabIndex = 0;
            this.xpDialogTitle1.AOkClick += new System.EventHandler(this.xpDialogTitle1_AOkClick);
            // 
            // xpSwitch1
            // 
            this.xpSwitch1.AChecked = false;
            this.xpSwitch1.ACheckedIndex = 0;
            this.xpSwitch1.ALeftText = "置换后作废";
            this.xpSwitch1.AMiddleText = "置换后正常下料";
            this.xpSwitch1.ARightText = "置换后不良下料";
            this.xpSwitch1.Location = new System.Drawing.Point(12, 99);
            this.xpSwitch1.Name = "xpSwitch1";
            this.xpSwitch1.Size = new System.Drawing.Size(537, 41);
            this.xpSwitch1.TabIndex = 1;
            this.xpSwitch1.ACheckedChanged += new System.EventHandler(this.xpSwitch1_ACheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 16F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(517, 29);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "下料目标线边仓";
            // 
            // panelWarehouse
            // 
            this.panelWarehouse.Controls.Add(this.comboBox1);
            this.panelWarehouse.Controls.Add(this.label1);
            this.panelWarehouse.Location = new System.Drawing.Point(12, 146);
            this.panelWarehouse.Name = "panelWarehouse";
            this.panelWarehouse.Size = new System.Drawing.Size(537, 100);
            this.panelWarehouse.TabIndex = 4;
            this.panelWarehouse.Visible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(575, 34);
            this.label2.TabIndex = 5;
            this.label2.Text = "物料置换后处理方式";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReworkKeyItemComfrimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(575, 373);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelWarehouse);
            this.Controls.Add(this.xpSwitch1);
            this.Controls.Add(this.xpDialogTitle1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReworkKeyItemComfrimForm";
            this.ShowInTaskbar = false;
            this.Text = "ReworkKeyItemComfrimForm";
            this.panelWarehouse.ResumeLayout(false);
            this.panelWarehouse.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.XPDialogTitle xpDialogTitle1;
        private Common.Controls.XPSwitch xpSwitch1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelWarehouse;
        private System.Windows.Forms.Label label2;
    }
}