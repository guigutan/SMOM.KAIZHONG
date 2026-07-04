using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products.SplitAndMerge
{
    /// <summary>
    /// 批次采集记录(出站入站分离)视图配置
    /// </summary>
    public class BatchWipRecordViewConfig : WebViewConfig<BatchWipRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            //View.AddBehavior("SIE.Web.MES.BatchWip.BatchWipRecordBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.ContainerNo).ShowInList(width: 150);
                View.Property(p => p.SourceBatchNo).Show(ShowInWhere.Hide);
                View.Property(p => p.InOutType).ShowInList(width: 150);
                View.Property(p => p.Qty).ShowInList(width: 150);
                View.Property(p => p.SplitQty).UseListSetting(p => p.HelpInfo = "点击查看拆分数据".L10N()).Show(ShowInWhere.Hide);
                View.Property(p => p.DefectQty).ShowInList(width: 150);
                View.Property(p => p.ScrapQty).ShowInList(width: 150);
                View.Property(p => p.ResultType).ShowInList(width: 150);
                View.Property(p => p.Shift).ShowInList(width: 150);
                View.Property(p => p.Resource).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.Station).ShowInList(width: 150);
                View.ChildrenProperty(p => p.KeyItemList).Show( ChildShowInWhere.All);
            }
        }
    }
}
