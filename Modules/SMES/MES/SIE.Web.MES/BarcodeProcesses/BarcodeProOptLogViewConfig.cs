using SIE.MES.BarcodeProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派操作日志
    /// </summary>
    public class BarcodeProOptLogViewConfig : WebViewConfig<BarcodeProOptLog>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using(View.OrderProperties())
            {
                View.Property(p => p.OptTime).Readonly().ShowInList(150);
                View.Property(p => p.Opter).Readonly().ShowInList(150);
                View.Property(p => p.Content).Readonly().ShowInList(300);
            }
        }
    }
}
