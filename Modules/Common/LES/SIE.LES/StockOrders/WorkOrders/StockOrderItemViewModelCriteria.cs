using SIE.Domain;
using SIE.Items;
using SIE.LES;
using SIE.LES.StockOrders.Service;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 备料单物料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备料单物料查询实体")]
    public class StockOrderItemViewModelCriteria : Criteria
    {
        #region 物料编码 Code
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<StockOrderItemViewModelCriteria>.Register(e => e.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料名称 Name
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<StockOrderItemViewModelCriteria>.Register(e => e.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 物料消耗方式 ConsumeMode
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode?> ConsumeModeProperty = P<StockOrderItemViewModelCriteria>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode? ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 工单Id WoId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WoIdProperty = P<StockOrderItemViewModelCriteria>.Register(e => e.WoId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WoId
        {
            get { return this.GetProperty(WoIdProperty); }
            set { this.SetProperty(WoIdProperty, value); }
        }
        #endregion

        #region 备料模式 StockType
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType?> StockTypeProperty = P<StockOrderItemViewModelCriteria>.Register(e => e.StockType);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType? StockType
        {
            get { return this.GetProperty(StockTypeProperty); }
            set { this.SetProperty(StockTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StockOrderService>().GetStockOrderItemViewModels(this);
        }
    }
}
