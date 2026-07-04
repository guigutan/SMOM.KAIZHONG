using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.ShipPlan;
using SIE.Warehouses;
using SIE.Web.Inventory;
using SIE.Web.ShipPlan.Commands;
using SIE.Web.Warehouses;
using System.Linq;

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// 分配仓库规则视图配置
    /// </summary>
    internal class AssignWarehouseRuleViewConfig : WebViewConfig<AssignWarehouseRule>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.ShipPlan.Scripts.AddBehavior");
            View.UseCommands(typeof(AddAssignWarehouseRuleCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType).DefaultValue((int)OrderType.SaleOut).UseSelectEnumEditor(p =>
                {
                    p.AllowBlank = false;

                    p.ValuesList.Add((int)OrderType.SaleOut);
                    p.ValuesList.Add((int)OrderType.WorkFeed);                    
                    p.ValuesList.Add((int)OrderType.OutWorkFeed);
                    p.ValuesList.Add((int)OrderType.OutAllotReturn);
                    p.ValuesList.Add((int)OrderType.OutWorkFeedUse);
                    p.ValuesList.Add((int)OrderType.OtherOut);
                    p.ValuesList.Add((int)OrderType.SupplierReturn);
                    p.ValuesList.Add((int)OrderType.DirectAllocate);
                    p.ValuesList.Add((int)OrderType.TwoAllocate);
                    p.ValuesList.Add((int)OrderType.PurchaseIn);
                    p.ValuesList.Add((int)OrderType.Finished);
                    p.ValuesList.Add((int)OrderType.PartedIn);
                    p.ValuesList.Add((int)OrderType.VMIIN);
                    p.ValuesList.Add((int)OrderType.CustomerIn);
                    p.ValuesList.Add((int)OrderType.MaterialReturn);
                    p.ValuesList.Add((int)OrderType.SaleReturn);
                    p.ValuesList.Add((int)OrderType.OtherIn);
                    p.ValuesList.Add((int)OrderType.AutoJoinLineWarehouse);
                }).Cascade(p => p.CustomerId, null).Cascade(p => p.SupplierId, null).Cascade(p => p.EnterpriseId, null).Cascade(p => p.ResourceId, null);
                View.Property(p => p.ItemType).Cascade(p => p.ItemCategory, null).Readonly(m => m.ItemType == ItemType.SemiFinished && m.OrderType == OrderType.AutoJoinLineWarehouse);
                View.Property(p => p.ItemCategory).UseDataSource((e, c, r) =>
                {
                    var assignWhRule = e as AssignWarehouseRule;
                    if (assignWhRule == null)
                    {
                        return new EntityList<ItemCategory>();
                    }
                    return RT.Service.Resolve<ItemController>().GetItemCategoryByItemType(assignWhRule.ItemType, SIE.Items.Items.CategoryType.Item, r, c);
                });
                View.Property(p => p.CustomerId).UseDataSource((e, c, r) =>
                {
                    var assignWhRule = e as AssignWarehouseRule;
                    if (assignWhRule == null)
                    {
                        return new EntityList<Customer>();
                    }
                    return RT.Service.Resolve<CustomerController>().GetEnableCustomers(c, r);
                }).Readonly(p => p.OrderType != OrderType.SaleOut && p.OrderType != OrderType.SaleReturn || p.OrderType == OrderType.AutoJoinLineWarehouse);
                View.Property(p => p.SupplierId).UseDataSource((e, c, r) =>
                {
                    var assignWhRule = e as AssignWarehouseRule;
                    if (assignWhRule == null)
                    {
                        return new EntityList<Supplier>();
                    }
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(c, r);
                }).Readonly(p => p.OrderType != OrderType.OutWorkFeed&& p.OrderType != OrderType.OutWorkFeedUse && p.OrderType != OrderType.OutAllotReturn && p.OrderType != OrderType.SupplierReturn && p.OrderType != OrderType.PurchaseIn
                && p.OrderType != OrderType.VMIIN && p.OrderType != OrderType.CustomerIn || p.OrderType == OrderType.AutoJoinLineWarehouse);
                View.Property(p => p.EnterpriseId).UseDataSource((e, c, r) =>
                {
                    var assignWhRule = e as AssignWarehouseRule;
                    if (assignWhRule == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    var departmentList = RT.Service.Resolve<EnterpriseController>().GetEnterprises(null, c, r);
                    departmentList.ForEach(p => p.TreePId = null);
                    return departmentList;
                }).Readonly(p => p.OrderType != OrderType.WorkFeed && p.OrderType != OrderType.OtherOut && p.OrderType != OrderType.Finished && p.OrderType != OrderType.PartedIn
                && p.OrderType != OrderType.MaterialReturn && p.OrderType != OrderType.OtherIn || p.OrderType == OrderType.AutoJoinLineWarehouse);
                View.Property(p => p.ResourceId).DefaultValue(null);
                View.Property(p => p.Priority);
                View.Property(p => p.Warehouse).UseDataSource((source, pagingInfo, keyword) =>
                {
                    if (source == null)
                        return new EntityList<Warehouse>();

                    var entity = source as AssignWarehouseRule;
                    if (entity.OrderType == OrderType.AutoJoinLineWarehouse)
                    {
                        var results = RT.Service.Resolve<WarehouseController>().GetLineWareHouseByEmployee(pagingInfo, keyword);
                        return results;

                    }//只获取线边仓仓库
                    else
                    {
                        var results = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmployee(pagingInfo, keyword);
                        return results;
                    }
                }).UsePagingLookUpEditor();
            }
        }
    }
}