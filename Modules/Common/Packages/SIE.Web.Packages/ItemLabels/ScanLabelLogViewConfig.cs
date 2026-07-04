using SIE.MetaModel.View;
using SIE.Packages.ItemLabels;

namespace SIE.Web.Packages.ItemLabels
{
    internal class ScanLabelLogViewConfig : WebViewConfig<ScanLabelLog>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();

            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.HighestNo).ShowInList(width: 150).FixColumn();
                View.Property(p => p.No).ShowInList(width: 150).FixColumn();
                View.Property(p => p.Qty);
                View.Property(p => p.LabelOpType);
                View.Property(p => p.BillNo);
                View.Property(p => p.LineNo);
                View.Property(p => p.PackingTypeName);
                View.Property(p => p.ItemPackageRuleCode).HasLabel("物料包装规则");
                View.Property(p => p.ItemPackageRuleName).HasLabel("物料包装规则名称");
                View.Property(p => p.ItemCode).HasLabel("物料编码").ShowInList(150);
                View.Property(p => p.ItemName).HasLabel("物料名称").ShowInList(150);
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.LotCode);
                View.Property(p => p.WarehouseId).Readonly();
                View.Property(p => p.IsSequence).Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No);
                View.Property(p => p.BillNo);
                View.Property(p => p.LabelOpType);
                View.Property(p => p.WarehouseId);
                View.Property(p => p.ItemId);
                View.Property(p => p.LotCode);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                });
            }
        }
    }
}
