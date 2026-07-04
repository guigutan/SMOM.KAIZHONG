using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.Strategy;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common.Commands;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 分配规则 视图配置
    /// </summary>
    internal class AssignRuleViewConfig : WebViewConfig<AssignRule>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(AddAssignRuleCommand).FullName, typeof(EditAssignRuleCommand).FullName, typeof(DeleteAssignRuleCommand).FullName);
            View.UseCommands(typeof(SetIsDefaultAssignRuleCommand).FullName, typeof(SaveAssignRuleCommand).FullName, WebCommandNames.ExportXls, typeof(InitAssignRuleCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(DisableAssignRuleCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}编码", "单号生成规则", "分配规则") + ",新增状态可编辑"; }).ShowInList(150);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.CustomerId).UseDataSource((o, c, r) =>
            {
                var rule = o as AssignRule;
                if (rule == null)
                    return new EntityList<Customer>();
                return RT.Service.Resolve<CustomerController>().GetEnableCustomers(c, r);
            }).Readonly(p => p.Code == AssignRule.Default);
            View.Property(p => p.WarehouseId).UseDataSource((o, c, r) =>
            {
                var rule = o as AssignRule;
                if (rule == null)
                    return new EntityList<Warehouse>();
                return RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(c, r);
            }).Readonly(p => p.Code == AssignRule.Default);
            View.Property(p => p.OrderType).UseSelectEnumEditor(p =>
            {
                p.ValuesList.Add((int)OrderType.SaleOut);
                p.ValuesList.Add((int)OrderType.WorkFeed);
                p.ValuesList.Add((int)OrderType.OutWorkFeed);
                p.ValuesList.Add((int)OrderType.OutWorkFeedUse);
                p.ValuesList.Add((int)OrderType.OutAllotReturn);
                p.ValuesList.Add((int)OrderType.OtherOut);
                p.ValuesList.Add((int)OrderType.SupplierReturn);
                p.ValuesList.Add((int)OrderType.DirectAllocate);
                p.ValuesList.Add((int)OrderType.TwoAllocate);
                p.ValuesList.Add((int)OrderType.WoFinishReturn);
            }).Readonly(p => p.Code == AssignRule.Default);
            View.Property(p => p.RelationType);
            View.Property(p => p.OnhandState).ShowInList(130);
            View.Property(p => p.LotCountUL).UseSpinEditor(p => { p.MinValue = 1; });
            View.Property(p => p.ProductBatchUL).UseSpinEditor(p => { p.MinValue = 1; });
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsDefault).Readonly();
            View.ChildrenProperty(p => p.AssignRuleDetailList).HasLabel("明细");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
