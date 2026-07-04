using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签投入工单
    /// </summary>
    public class ItemLabelWorkOrderViewConfig : WebViewConfig<ItemLabelWorkOrder>
    {
        /// <summary>
        /// 单个英文字符宽度
        /// </summary>
        private readonly int SingleCharWidth = 10;

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.WorkOrderId).ShowInList(width: SingleCharWidth * 15).Readonly();
            View.Property(p => p.Qty).ShowInList(width: SingleCharWidth * 10).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
