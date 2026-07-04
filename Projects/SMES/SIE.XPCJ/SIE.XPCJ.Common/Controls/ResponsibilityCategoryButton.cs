using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class ResponsibilityCategoryButton : UserControl
    {
        public Action ClickAction { get; set; }

        public ResponsibilityCategoryItem CurrentResponsibilityCategory { get; set; }
        public ResponsibilityCategoryButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 设置缺陷项
        /// </summary>
        /// <param name="defectCategoryItem"></param>
        public void SetDefectCategoryItem(ResponsibilityCategoryItem responsibilityCategoryItem)
        {
            this.xpButton1.Text = responsibilityCategoryItem.Category.Description;
            CurrentResponsibilityCategory = responsibilityCategoryItem;
        }

        private void DefectCategoryButton_Click(object sender, EventArgs e)
        {
            ClickAction?.Invoke();
        }
    }
}
