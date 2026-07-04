using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.WIP;
using System;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class RepairsListPanelCtr : BaseUserControl
    {
        public RepairDefectViewModel repairDefectViewModel { get; set; }
        public RepairsListPanelCtr()
        {
            InitializeComponent();
        }

        private void LoadItemPanelCtr_Load(object sender, EventArgs e)
        {
            this.xpTextBox3.AText = "数量".L10N() + ":1";
            InitData();
        }

        private void InitData()
        {
            if (repairDefectViewModel != null)
            {
                this.xpTextBox1.AText = repairDefectViewModel.DefectCode + "(" + repairDefectViewModel.DefectDesc + ")";
                this.xpTextBox2.AText = repairDefectViewModel.ProcessName;
                this.label4.Text = repairDefectViewModel.InspItemName;
                string measure = string.Empty;
                repairDefectViewModel.MeasureList.ForEach(p => { measure += p.Name + ";"; });

                this.label5.Text = measure;
                this.label7.Text = repairDefectViewModel.ScrapQty.ToString("0");
                this.lbScrap.Text = repairDefectViewModel.ScrapReason;
                this.label11.Text = repairDefectViewModel.Remark;
                string responsibility = string.Empty;
                repairDefectViewModel.ResponsibilityList.ForEach(p => { responsibility += p.Description + ";"; });
                this.label9.Text = responsibility;
            }
        }

        private void xpButton1_Click(object sender, EventArgs e)
        {
            var inputForm = new RepairsInputForm(repairDefectViewModel);
            var result = inputForm.ShowDialog();
            //刷新列表
            if (result == DialogResult.OK)
            {
                this.repairDefectViewModel = inputForm.RepairDefectViewModel;
                InitData();

            }
        }
    }
}
