using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class DefectButton : UserControl
    {
        public Action ClickAction { get; set; }

        public DefectItem CurrentDefectItem { get; set; }

        public DefectResponsibility CurrentDefectResponsibility { get; set; }
        public DefectButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        public void SetDefectItem(DefectItem defectItem)
        {
            this.xpButton1.Text = defectItem.Defect.Description;
            CurrentDefectItem = defectItem;
        }

        /// <summary>
        /// 设置缺陷责任
        /// </summary>
        /// <param name="defectResponsibility"></param>
        public void SetDefectResponsibility(DefectResponsibility  defectResponsibility)
        {
            this.xpButton1.Text = defectResponsibility.Description;
            CurrentDefectResponsibility = defectResponsibility;
        }

        private void DefectButton_Click(object sender, EventArgs e)
        {
            ClickAction?.Invoke();
        }
    }
}
