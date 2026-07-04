using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using System;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class ResponsibilitySelectedButton : UserControl
    {
        /// <summary>
        /// 点击动作
        /// </summary>
        public Action ClickAction { get; set; }

        public DefectResponsibility DefectResponsibility { get; set; }
        public ResponsibilitySelectedButton()
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
        /// 设置缺陷责任
        /// </summary>
        /// <param name="defectResponsibility"></param>
        public void SetDefectResponsibility(DefectResponsibility  defectResponsibility)
        {
            this.DefectResponsibility = defectResponsibility;
            this.label3.Text = defectResponsibility.Description;
            this.Tag = defectResponsibility;
        }
    }
}
