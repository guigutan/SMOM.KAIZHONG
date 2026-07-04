using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class RepairsListPanelListCtr : UserControl
    {
        /// <summary>
        /// 维修缺陷记录
        /// </summary>
        private List<RepairDefectViewModel> RepairDefectItems { get; set; }

        public RepairsListPanelListCtr()
        {
            InitializeComponent();
        }
        public void SetData(List<RepairDefectViewModel> repairDefectItems)
        {
            RepairDefectItems = repairDefectItems;
            this.Controls.Clear();
            foreach (var item in RepairDefectItems)
            {
                RepairsListPanelCtr repairsListPanelCtr = new RepairsListPanelCtr();
                repairsListPanelCtr.repairDefectViewModel = item;
                repairsListPanelCtr.Dock = DockStyle.Top;
                //repairsListPanelCtr.ReflashLoadItemListAction = ReflashRepairsListAction;
                this.Controls.Add(repairsListPanelCtr);
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>

        public void ClearData()
        {
            RepairDefectItems = new List<RepairDefectViewModel>();
            this.Controls.Clear();
        }

    }
}
