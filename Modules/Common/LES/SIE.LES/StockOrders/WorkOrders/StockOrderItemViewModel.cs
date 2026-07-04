using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 备料单物料
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockOrderItemViewModelCriteria))]
    [Label("备料单物料")]
    public class StockOrderItemViewModel : Entity<double>
    {
        #region 物料编码 Code
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<StockOrderItemViewModel>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<StockOrderItemViewModel>.Register(e => e.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<StockOrderItemViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<StockOrderItemViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<StockOrderItemViewModel>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 物料消耗方式 ConsumeMode
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<StockOrderItemViewModel>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<StockOrderItemViewModel>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 工单需求数 WorkOrderQty
        /// <summary>
        /// 工单需求数
        /// </summary>
        [Label("工单需求数")]
        public static readonly Property<decimal> WorkOrderQtyProperty = P<StockOrderItemViewModel>.Register(e => e.WorkOrderQty);

        /// <summary>
        /// 工单需求数
        /// </summary>
        public decimal WorkOrderQty
        {
            get { return this.GetProperty(WorkOrderQtyProperty); }
            set { this.SetProperty(WorkOrderQtyProperty, value); }
        }
        #endregion

        #region 启用扩展属性 IsEnableItemExtProp
        /// <summary>
        /// 启用扩展属性
        /// </summary>
        [Label("启用扩展属性")]
        public static readonly Property<bool> IsEnableItemExtPropProperty = P<StockOrderItemViewModel>.Register(e => e.IsEnableItemExtProp);

        /// <summary>
        /// 启用扩展属性
        /// </summary>
        public bool IsEnableItemExtProp
        {
            get { return this.GetProperty(IsEnableItemExtPropProperty); }
            set { this.SetProperty(IsEnableItemExtPropProperty, value); }
        }
        #endregion

    }
}
