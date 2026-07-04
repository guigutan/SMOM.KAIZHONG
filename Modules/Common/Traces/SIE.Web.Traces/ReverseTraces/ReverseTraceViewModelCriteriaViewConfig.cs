using SIE.Items;
using SIE.Traces.ReverseTraces;

namespace SIE.Web.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯查询实体视图配置
    /// </summary>
    internal class ReverseTraceViewModelCriteriaViewConfig : WebViewConfig<ReverseTraceViewModelCriteria>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.UseDefaultCommands();
                View.Property(p => p.ProductId).UseDataSource((e, p, keyword) =>
                {
                    List<int> itemTypeList = new List<int>();
                    itemTypeList.Add((int)ItemType.Product);
                    itemTypeList.Add((int)ItemType.SemiFinished);
                    return RT.Service.Resolve<ItemController>().GetItems(itemTypeList, p, keyword);
                }).Show(ShowInWhere.All);
                View.Property(p => p.ProductSn).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.ProductionDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.All; }).Show(ShowInWhere.All).Readonly(false);
            }
        }
    }
}