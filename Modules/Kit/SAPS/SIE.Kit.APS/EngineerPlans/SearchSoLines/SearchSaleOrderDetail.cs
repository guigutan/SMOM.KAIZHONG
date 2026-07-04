using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.EngineerPlans.SearchSoLines
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SearchSaleOrderDetailCriteria))]
    [Label("订单明细")]
    [DisplayMember(nameof(LineNo))]
    public class SearchSaleOrderDetail : SaleOrderDetail
    {
     
        
    }

    /// <summary>
    /// 订单明细 实体配置
    /// </summary>
    internal class SearchSaleOrderDetailConfig : EntityConfig<SearchSaleOrderDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SALE_ORDER_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
        }

    }
}
