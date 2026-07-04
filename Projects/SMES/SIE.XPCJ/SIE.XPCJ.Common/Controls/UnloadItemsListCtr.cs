using SIE.XPCJ.Models.WIP.Entity;
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
    public partial class UnloadItemsListCtr : UserControl
    {

        public List<UnloadItem> UnloadItems { get; set; }
        public UnloadItemsListCtr()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void SetData(List<UnloadItem> unloadItems)
        {
            this.Controls.Clear();
            this.UnloadItems = unloadItems;
            foreach (var unloadItem in this.UnloadItems)
            {
                UnloadItemsCtr unloadItemsCtr = new UnloadItemsCtr();
                unloadItemsCtr.unloadItem = unloadItem;
                unloadItemsCtr.Dock = DockStyle.Top;
                this.Controls.Add(unloadItemsCtr);
            }
        }
    }
}
