using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products.SplitAndMerge
{

    /// <summary>
    /// 批次采集拆分记录视图配置
    /// </summary>
    public class BatchWipSplitViewModelViewConfig : WebViewConfig<BatchWipSplitViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Readonly().ShowInList(width: 150);
                View.Property(p => p.BatchSource).Readonly().ShowInList(width: 150);
                View.Property(p => p.Qty).Readonly().ShowInList(width: 150);
                View.Property(p => p.IsDefect).Readonly().ShowInList(width: 150);
            }
        }
    }
}
