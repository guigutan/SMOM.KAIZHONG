using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class DefectCategoryButton : UserControl
    {
        public Action ClickAction { get; set; }

        public DefectCategoryItem CurrentDefectItem { get; set; }
        public DefectCategoryButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 设置缺陷项
        /// </summary>
        /// <param name="defectCategoryItem"></param>
        public void SetDefectCategoryItem(DefectCategoryItem defectCategoryItem)
        {
            this.xpButton1.Text = defectCategoryItem.Category.Description;
            CurrentDefectItem = defectCategoryItem;
        }

        private void DefectCategoryButton_Click(object sender, EventArgs e)
        {
            ClickAction?.Invoke();
        }
    }
}
