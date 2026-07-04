using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products.SplitAndMerge
{

    /// <summary>
    /// PDA批次合并记录
    /// </summary>
    public class BatchSplitMergeViewConfig : WebViewConfig<BatchSplitMergeRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InputBatchNo).ShowInList(width: 150);
                View.Property(p => p.InputQty).ShowInList(width: 150);
                View.Property(p => p.BatchOperationType).ShowInList(width: 150);
                View.Property(p => p.OutputBatchNo).ShowInList(width: 150);
                View.Property(p => p.OutputQty).ShowInList(width: 150);
            }

        }
    }
    }
