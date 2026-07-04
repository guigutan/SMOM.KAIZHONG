using SIE.XPCJ.Models;
using System;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class DefectSelectedButton : UserControl
    {
        /// <summary>
        /// 点击动作
        /// </summary>
        public Action ClickAction { get; set; }

        public DefectItem DefectItem { get; set; }
        public DefectSelectedButton()
        {
            InitializeComponent();
            this.Click += DefectSelectedButton_Click;
            DoubleBuffered = true;
        }

        private void DefectSelectedButton_Click(object sender, EventArgs e)
        {
            ClickAction?.Invoke();
        }

        /// <summary>
        /// 设置缺陷
        /// </summary>
        /// <param name="defectItem"></param>
        public void SetDefectItem(DefectItem defectItem)
        {
            this.DefectItem = defectItem;
            this.label3.Text = defectItem.Defect.Description;
            this.label1.Text = defectItem.Qty == 0 ? "1" : defectItem.Qty.ToString();
            this.Tag = defectItem;
        }
    }
}
