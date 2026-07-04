using SIE.MES.Report.BatchTracebacks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class KeyComponentPartDtlViewConfig : WebViewConfig<KeyComponentPartDtl>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ProcessCode).Show().Readonly();
            View.Property(p => p.SourceSn).Show().Readonly();
            View.Property(p => p.ItemLabelLot).Show().Readonly();
            View.Property(p => p.DeductedQty).Show().Readonly();
            View.Property(p => p.ShortDescription).Show().Readonly();
            View.Property(p => p.ItemCode).Show().Readonly();
            View.Property(p => p.ItemName).Show().Readonly();
            View.Property(p => p.Unit).Show().Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
