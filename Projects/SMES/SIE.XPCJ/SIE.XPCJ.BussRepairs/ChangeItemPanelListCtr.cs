using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class ChangeItemPanelListCtr : UserControl
    {
        /// <summary>
        /// 装配记录
        /// </summary>
        private List<ProductAssemblyDetailViewModel> ProductAssemblyDetails { get; set; } 

        public ChangeItemPanelListCtr()
        {
            InitializeComponent();
            ProductAssemblyDetails = new List<ProductAssemblyDetailViewModel>();
        }
        public void SetData(List<ProductAssemblyDetailViewModel> productAssemblyDetails)
        {
            ProductAssemblyDetails = productAssemblyDetails;
            this.Controls.Clear();
            foreach (var item in ProductAssemblyDetails)
            {
                ChangeItemPanelCtr changeItemPanelCtr = new ChangeItemPanelCtr();
                changeItemPanelCtr.SetData(item);
                changeItemPanelCtr.Dock = DockStyle.Top;
                changeItemPanelCtr.SetProductAssemblyDetails(productAssemblyDetails);
                this.Controls.Add(changeItemPanelCtr);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.Controls.Clear();
            ProductAssemblyDetails.Clear();
        }

        /// <summary>
        /// 获取最新的
        /// </summary>
        /// <returns></returns>
        public List<ProductAssemblyDetailViewModel> GetNewProductAssemblyDetails()
        {
            return ProductAssemblyDetails;
        }
    }
}
