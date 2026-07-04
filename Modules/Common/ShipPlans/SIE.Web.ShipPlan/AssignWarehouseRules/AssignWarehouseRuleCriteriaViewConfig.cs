using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using SIE.ShipPlan;
using SIE.Web.Inventory;

namespace SIE.Web.ShipPlan.Commands
{
    /// <summary>
    /// 上架规则
    /// </summary>
    internal class AssignWarehouseRuleCriteriaViewConfig : WebViewConfig<AssignWarehouseRuleCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
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
                    p.ValuesList.Add((int)OrderType.PurchaseIn);
                    p.ValuesList.Add((int)OrderType.Finished);
                    p.ValuesList.Add((int)OrderType.PartedIn);
                    p.ValuesList.Add((int)OrderType.VMIIN);
                    p.ValuesList.Add((int)OrderType.CustomerIn);
                    p.ValuesList.Add((int)OrderType.MaterialReturn);
                    p.ValuesList.Add((int)OrderType.SaleReturn);
                    p.ValuesList.Add((int)OrderType.OtherIn);
                    p.ValuesList.Add((int)OrderType.AutoJoinLineWarehouse);
                }).Show();
                View.Property(p => p.ItemType).Show();
                View.Property(p => p.ItemCategory).UseDataSource((e, c, r) =>
                {
                    var criteria = e as AssignWarehouseRuleCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<ItemCategory>();
                    }
                    return RT.Service.Resolve<ItemController>().GetItemCategoryByType(SIE.Items.Items.CategoryType.Item, null, c);
                }).Show();
            }
        }
    }
}
