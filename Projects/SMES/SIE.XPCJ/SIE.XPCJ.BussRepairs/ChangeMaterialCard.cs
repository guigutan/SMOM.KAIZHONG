using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.WIP;
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
    public partial class ChangeMaterialCard : XPCJ.Common.Controls.XPCard
    {
        public bool IsCheck = false;


        /// <summary>
        /// 回刷窗体数据
        /// </summary>
        public Action ReflashForm { get; set; }
        private List<ProductAssemblyDetailViewModel> ProductAssemblyDetails { get; set; }

        public ProductAssemblyDetailViewModel CurrentProductAssemblyDetailViewModel { get; set; }

        public ChangeItemViewModel curChangeItemViewModel { get; set; }
        public ChangeMaterialCard()
        {
            InitializeComponent();
            ProductAssemblyDetails = new List<ProductAssemblyDetailViewModel>();
        }
        public void SetData(ChangeItemViewModel changeItemViewModel)
        {
            checkBox1.Text = changeItemViewModel.ChangeSn;
            xpTextBoxCanQty.AText = changeItemViewModel.LoadItemBarcodeInfo.Qty.ToString();
            xpTextBoxItemQty.AText = changeItemViewModel.ChangeQty.ToString();
            this.curChangeItemViewModel = changeItemViewModel;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IsCheck = this.checkBox1.Checked;
        }

        private void xpButtonEditQty_Click(object sender, EventArgs e)
        {
            var nowQty = xpTextBoxItemQty.AText.Length > 0 ? decimal.Parse(xpTextBoxItemQty.AText.Trim()) : 0;
            if (XPFormNumberInput.ShowInput(nowQty, out decimal newQty) == DialogResult.OK)
            {
                if (newQty <= 0)
                {
                    MessageBox.Show("换料数量必须大于0".L10N());
                }
                ProductAssemblyDetailViewModelHelper.OnChangeQtyChanged(this.curChangeItemViewModel.ChangeSn, newQty,
                    CurrentProductAssemblyDetailViewModel,
                    ProductAssemblyDetails
                    );

                xpTextBoxItemQty.AText = newQty.ToString();
                this.curChangeItemViewModel.ChangeQty = newQty;
                ReflashForm?.Invoke();
            }
        }

        public void SetProductAssemblyDetails(List<ProductAssemblyDetailViewModel> productAssemblyDetails)
        {
            ProductAssemblyDetails = productAssemblyDetails;
        }
    }
}
