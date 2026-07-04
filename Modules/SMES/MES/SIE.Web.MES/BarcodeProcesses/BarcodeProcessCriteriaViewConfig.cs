using SIE.MES.BarcodeProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Barcodes.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派查询视图配置
    /// </summary>
    public class BarcodeProcessCriteriaViewConfig : WebViewConfig<BarcodeProcessCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.Sn).Show();
                View.Property(p => p.AssignState).Show();
                View.Property(p => p.CrtTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today).Show();
            }
        }
    }
}
