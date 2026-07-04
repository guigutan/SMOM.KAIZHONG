using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Report.BatchWipProducts;
using SIE.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchWipProducts
{
    internal class BatchWipProductVersionReportViewConfig : WebViewConfig<BatchWipProductVersionReport>
    {
        protected override void ConfigListView()
        {
            View.ChildrenProperty(p => p.BatchSplitMergeList).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(BatchSplitMergeRecord), (e) =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity.ToJsonObject<BatchWipProductVersionReport>();
                if (parent == null)
                {

                    return new EntityList<BatchSplitMergeRecord>();
                }
                else
                {
                    return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchSplitMergeRecord(arg.PagingInfo,parent.BatchNo);
                }
            }, ListView).HasOrderNo(40).HasLabel("批次合并拆分记录");
        }
    }
}
