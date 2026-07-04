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
    public partial class LoadItemPanelListCtr : UserControl
    {
        /// <summary>
        /// 上料明细
        /// </summary>
        private List<LoadItem> LoadItems { get; set; }

        public Action<LoadItem, string> ReflashLoadItemListAction { get; set; }
        public Action<LoadItem, string> ReflashUnLoadItemListAction { get; set; }
        public LoadItemPanelListCtr()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        public void SetData(List<LoadItem> loadItems)
        {
            LoadItems = loadItems;
            this.Controls.Clear();
            foreach (var item in LoadItems)
            {
                LoadItemPanelCtr loadItemPanelCtr = new LoadItemPanelCtr();
                loadItemPanelCtr.loadItem = item;
                loadItemPanelCtr.Dock = DockStyle.Top;
                loadItemPanelCtr.ReflashLoadItemListAction = ReflashLoadItemListAction;
                loadItemPanelCtr.ReflashUnLoadItemListAction = ReflashUnLoadItemListAction;
                this.Controls.Add(loadItemPanelCtr);
            }
        }
    }
}
