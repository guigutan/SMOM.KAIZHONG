using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.WIP.Repairs;
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
    public partial class RepirsCompleteForm : XPFormBaseDialog
    {
        private UplineViewModel m_UplineViewModel { get; set; }

        /// <summary>
        /// 提交委托
        /// </summary>
        public Func<UplineViewModel, bool> FunctionSubmit { get; set; }
        /// <summary>
        /// 维修完成页面
        /// </summary>
        /// <param name="uplineViewModel"></param>
        public RepirsCompleteForm(UplineViewModel uplineViewModel)
        {
            InitializeComponent();
            m_UplineViewModel = uplineViewModel;
        }

        private void RepirsCompleteForm_Load(object sender, EventArgs e)
        {
            xpComboBox1.DataSource = m_UplineViewModel.ProcessList;
            //加载默认工序
            var index = m_UplineViewModel.ProcessList.FindIndex(m => m.IsDefault);
            if (index >= 0)
            {
                this.xpComboBox1.SelectedIndex = index;
            }
        }

        public UplineViewModel GetUplineViewModel()
        {
            return m_UplineViewModel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FunctionSubmit.Invoke(m_UplineViewModel))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void SetTitle(string title)
        {
            this.label1.Text = title;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingDisplayLabel"></param>
        public void SetRoutingDisplay(string routingDisplayLabel)
        {
            this.label3.Text = routingDisplayLabel;
        }
        private void xpComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.xpComboBox1.SelectedItem != null)
            {
                var gotoProcessViewModel = this.xpComboBox1.SelectedItem as GotoProcessViewModel;
                if (gotoProcessViewModel != null)
                {
                    m_UplineViewModel.UplineProcess = gotoProcessViewModel;
                    m_UplineViewModel.UplineProcessId = gotoProcessViewModel.RoutingProcessId.ToString();
                    this.xpTextBoxNextProcess.AText = gotoProcessViewModel.PathDescription;
                }
            }
        }
    }
}
