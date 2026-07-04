using SIE.Core.Enums;
using SIE.MetaModel.View;
using SIE.ShipPlan;
using SIE.Web.Inventory;
using SIE.Web.ShipPlan.Commands;

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// 合并创单规则视图配置
    /// </summary>
    internal class MergeCreateRuleViewConfig : WebViewConfig<MergeCreateRule>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DeliveryPlan));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(InitMergeCreateRuleCommand).FullName, WebCommandNames.Edit, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType).Readonly();
                View.Property(p => p.IsSameDeliveryDate);
                View.Property(p => p.IsSameNo);
                View.Property(p => p.IsSameOrderNo);
                View.Property(p => p.DefaultDesc).Readonly();
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType).UseSelectEnumEditor(p =>
            {
                p.AllowBlank = true;
                p.ValuesList.Add((int)OrderType.SaleOut);
                p.ValuesList.Add((int)OrderType.WorkFeed);
                p.ValuesList.Add((int)OrderType.OutWorkFeed);
                p.ValuesList.Add((int)OrderType.OutWorkFeedUse);
                p.ValuesList.Add((int)OrderType.OutAllotReturn);
                p.ValuesList.Add((int)OrderType.OtherOut);
                p.ValuesList.Add((int)OrderType.SupplierReturn);
                p.ValuesList.Add((int)OrderType.DirectAllocate);
                p.ValuesList.Add((int)OrderType.TwoAllocate);
                p.ValuesList.Add((int)OrderType.WhTransferOut);
            }).Show();
        }
    }
}
