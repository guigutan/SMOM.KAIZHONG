using SIE.Items;
using SIE.Traces.ForwardTraces;

namespace SIE.Web.Traces.ForwardTraces
{
    /// <summary>
    /// 正向追溯查询实体视图配置
    /// </summary>
    internal class ForwardTraceViewModelCriteriaViewConfig : WebViewConfig<ForwardTraceViewModelCriteria>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.UseDefaultCommands();
                View.Property(p => p.Item).UseDataSource((e, p, keyword) =>
                {
                    List<int> itemTypeList = new List<int>();
                    //itemTypeList.Add((int)ItemType.Product);
                    itemTypeList.Add((int)ItemType.SemiFinished);
                    itemTypeList.Add((int)ItemType.Material);
                    itemTypeList.Add((int)ItemType.SparePart);
                    itemTypeList.Add((int)ItemType.Other);
                    return RT.Service.Resolve<ItemController>().GetItems(itemTypeList, p, keyword);
                }).Show(ShowInWhere.All);
                View.Property(p => p.ItemExtPropName).Show(ShowInWhere.All);
                View.Property(p => p.Lot).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
                View.Property(p => p.Supplier).Show(ShowInWhere.All);
                View.Property(p => p.ProductionDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.All; }).Show(ShowInWhere.All).Readonly(false);
                View.Property(p => p.ReceiptDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show(ShowInWhere.All).Readonly(false);
            }
        }
    }
}