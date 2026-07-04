using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Inventory.Strategy.Commands;

namespace SIE.Web.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则视图配置
    /// </summary>
    public class TaskAllotRuleViewConfig : WebViewConfig<TaskAllotRule>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddTaskAllotRuleCommand).FullName, WebCommandNames.Edit, typeof(DeleteTaskAllotRuleCommand).FullName, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(width: 150).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New);
                View.Property(p => p.Name).ShowInList(width: 150);
                View.Property(p => p.Description);
                View.Property(p => p.OperationType).UseSelectEnumEditor(p =>
                {
                    p.AllowBlank = true;
                    p.ValuesList.Add((int)OperationType.PutOn);
                    p.ValuesList.Add((int)OperationType.PullOff);
                    p.ValuesList.Add((int)OperationType.Move);
                    p.ValuesList.Add((int)OperationType.Allot);
                    p.ValuesList.Add((int)OperationType.Replenish);
                    p.ValuesList.Add((int)OperationType.Check);
                });
                View.Property(p => p.StorerCode).UseSelectStorerCodeEditor(p =>
                {
                    p.DisplayField = Customer.CodeProperty.Name;
                    p.ValueField = Customer.CodeProperty.Name;
                    p.ReloadDataOnPopping = true;
                });
                View.Property(p => p.Warehouse).UseDataSource((o, c, r) =>
                {
                    var rule = o as TaskAllotRule;
                    if (rule == null)
                    {
                        return new EntityList<Warehouse>();
                    }

                    return RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(c, r);
                }).Cascade(p => p.LogicAreaId, null);
                View.Property(p => p.LogicArea).UseDataSource((o, c, r) =>
                {
                    var rule = o as TaskAllotRule;
                    if (rule == null || !rule.WarehouseId.HasValue)
                    {
                        return new EntityList<LogicArea>();
                    }

                    return RT.Service.Resolve<WarehouseController>().GetLogicAreas(rule.WarehouseId.Value, r, c);
                });
                View.Property(p => p.ItemCategory).UseDataSource((e, p, s) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemCategoryByType(CategoryType.Item, null, p);
                });
                View.Property(p => p.Priority);
                View.Property(p => p.State);
            }

            View.ChildrenProperty(p => p.EmployeeList);
        }
    }
}