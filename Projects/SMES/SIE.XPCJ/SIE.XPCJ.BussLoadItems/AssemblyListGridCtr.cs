using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussLoadItems
{
    public partial class AssemblyListGridCtr : UserControl
    {
        public AssemblyListGridCtr()
        {
            InitializeComponent();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238);
                }
            }
        }
    }
}
