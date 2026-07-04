using SIE.Items;
using SIE.Items.Items;
using SIE.LES;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.Web.Items._Extentions_;
using System.Linq;

namespace SIE.Web.LES
{
    /// <summary>
    /// 备料单查询实体配置视图
    /// </summary>
    public class PrepareItemPullCriteriaViewConfig : WebViewConfig<PrepareItemPullCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
			using (View.OrderProperties())
			{
				View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
				{
					return RT.Service.Resolve<LinesideWarehouseController>().GeWarehouses(c, r);
				}).Show();
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
					p.ValuesList.Add((int)TriggerMode.BelowSafe);
				}).Show();
				View.Property(p => p.DemandType).UseSelectEnumBoxEditor(p =>
				{
					p.AllowBlank = true;
					p.ValuesList.Add((int)DemandMode.MaxStock);
					p.ValuesList.Add((int)DemandMode.FixedQuantity);
				}).Show();
			}
		}
    }
}