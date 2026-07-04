using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.InboundOrders
{
    /// <summary>
    /// 查询视图
    /// </summary>
    public class InboundOrderCriteriaViewConfig : WebViewConfig<InboundOrderCriteria>
    {
        /// <summary>
        /// 配置查询
        /// </summary>

        protected override void ConfigQueryView()
        {
            View.Property(p => p.InboundOrderNo).Show();
            View.Property(p => p.InboundType).Show();
            View.Property(p => p.ReceiptOrderNo).Show();
            View.Property(p => p.AcceptanceOrderNo).Show();
            View.Property(p => p.InboundStatus).Show().HasLabel("状态");
            View.Property(p => p.FixtureEncode).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(keyword,pagingInfo);
            }).Show().HasLabel("工治具编码");
            View.Property(p => p.ManageMode).Show();
            View.Property(p => p.PurchaseOrderNo).Show();
            View.Property(p => p.InboundTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
        }
    }
}
