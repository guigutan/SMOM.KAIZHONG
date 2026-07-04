using Newtonsoft.Json;
using SIE.MES.Report.BatchTracebacks;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackDefetctLabelDtlViewConfig : WebViewConfig<BatchTracebackDefetctLabelDtl>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.AttachChildrenProperty(typeof(BatchTracebackDefetctDtl), (o) =>
                {
                    //var parent = o.Parent as BatchTracebackDefetctLabelDtl;
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackDefetctLabelDtl>(args.ParentEntity);
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackDefetctDtlsById(parent.SuspectProductLabelId, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("缺陷代码");
            }
        }
    }
}
