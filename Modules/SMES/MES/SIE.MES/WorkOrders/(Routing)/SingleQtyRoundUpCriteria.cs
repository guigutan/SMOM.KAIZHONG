using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders._Routing_
{
    /// <summary>
    /// 单位耗用量向上取整配置表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("单位耗用量向上取整配置表查询实体")]
    public class SingleQtyRoundUpCriteria : Criteria
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SingleQtyRoundUpCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SingleQtyRoundUpCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<SingleQtyRoundUpCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType? ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
            set { this.SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WorkOrderController>().CriteriaSingleQtyRoundUp(this);
        }
    }
}
