using SIE.Items;
using SIE.Items.Items;
using SIE.LES;
using SIE.LES.Commons;
using SIE.Resources.WipResources;
using SIE.Web.Items._Extentions_;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES
{
    /// <summary>
    /// 备料单查询实体配置视图
    /// </summary>
    public class PrepareItemPushCriteriaViewConfig : WebViewConfig<PrepareItemPushCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResourceId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(null, c, r);
                }).HasLabel("资源编号").Show();
                View.Property(p => p.ItemCategoryId).UseDataSource((e, c, r) =>
                {
                    var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(CategoryType.Item, null, r, c);
                    list.ForEach(p => p.TreePId = null);
                    return list;
                }).Show();
                View.Property(p => p.ItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemList(keyword, pagingInfo);
                }).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.TriggerType).UseSelectEnumBoxEditor(p =>
                {
                    p.ValuesList.Add((int)TriggerMode.XHoursBefore);
                    p.ValuesList.Add((int)TriggerMode.InvBelowSafeLevelToBeat);
                }).Show();
                View.Property(p => p.DemandType).UseSelectEnumBoxEditor(p =>
                {
                    p.ValuesList.Add((int)DemandMode.WoSurplusQty);
                    p.ValuesList.Add((int)DemandMode.StockToSafeLevelForBeat);
                    p.ValuesList.Add((int)DemandMode.StockIsSafeLevelForBeat);
                    p.ValuesList.Add((int)DemandMode.FixedQuantity);
                }).Show();
            }
        }
    }
}