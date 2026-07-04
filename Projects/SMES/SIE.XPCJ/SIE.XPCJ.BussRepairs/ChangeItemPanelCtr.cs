using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class ChangeItemPanelCtr : BaseUserControl
    {
        public ProductAssemblyDetailViewModel CurrentProductAssemblyDetailViewModel { get; set; }

        /// <summary>
        /// 装配记录
        /// </summary>
        private List<ProductAssemblyDetailViewModel> ProductAssemblyDetails { get; set; }
        public ChangeItemPanelCtr()
        {
            InitializeComponent();
            ProductAssemblyDetails = new List<ProductAssemblyDetailViewModel>();
        }

        /// <summary>
        /// 设置初始化数据
        /// </summary>
        /// <param name="productAssemblyDetailViewModel"></param>
        public void SetData(ProductAssemblyDetailViewModel productAssemblyDetailViewModel)
        {
            this.xpTextBox1.AText = productAssemblyDetailViewModel.SourceCode;
            this.label4.Text = productAssemblyDetailViewModel.ProcessName;
            this.label5.Text = productAssemblyDetailViewModel.KeyItemItemCode;
            this.label9.Text = productAssemblyDetailViewModel.KeyItemItemName;
            this.lbQty.Text = productAssemblyDetailViewModel.KeyItem.Qty.ToString("0");
            this.label11.Text = productAssemblyDetailViewModel.TotalChangeQty.ToString("0");
            this.label7.Text = productAssemblyDetailViewModel.ChangeBarcode;
            CurrentProductAssemblyDetailViewModel = productAssemblyDetailViewModel;
            if (productAssemblyDetailViewModel.KeyItem.Qty <= 0)
            {
                this.xpButton1.Visible = false;
            }
        }
        public void SetProductAssemblyDetails(List<ProductAssemblyDetailViewModel> productAssemblyDetailViewModels)
        {
            ProductAssemblyDetails = productAssemblyDetailViewModels;
        }

        /// <summary>
        /// 换料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButton1_Click(object sender, EventArgs e)
        {
            ChangeMaterialForm changeMaterialForm = new ChangeMaterialForm();
            changeMaterialForm.ProductAssemblyDetails = ProductAssemblyDetails;

            var repairsForm = this.FindForm();
            if (repairsForm != null)
            {
                var parentRepairesForm = repairsForm as RepairsForm;
                if (parentRepairesForm != null)
                {
                    changeMaterialForm.LoadItemList = parentRepairesForm.LoadItemList;
                }
            }
            changeMaterialForm.SetData(CurrentProductAssemblyDetailViewModel);
            if (changeMaterialForm.ShowDialog() == DialogResult.OK)
            {
                CurrentProductAssemblyDetailViewModel = changeMaterialForm.CurrentProductAssemblyDetailViewModel;
                SetData(CurrentProductAssemblyDetailViewModel);
                //修改列表的引用数据
                var index = ProductAssemblyDetails.FindIndex(m => m.Id == CurrentProductAssemblyDetailViewModel.Id);
                if (index >= 0)
                {
                    ProductAssemblyDetails[index] = CurrentProductAssemblyDetailViewModel;
                }
            }


        }
    }
}
