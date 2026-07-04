using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 选择物料
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialPrepareItemCriteria))]
    [Label("选择物料")]
    public class MaterialPrepareItemViewModel : ViewModel
    {
        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<MaterialPrepareItemViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialPrepareItemViewModel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<MaterialPrepareItemViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 消耗方式 ConsumeMode
        /// <summary>
        /// 消耗方式
        /// </summary>
        [Label("消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<MaterialPrepareItemViewModel>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MaterialPrepareItemViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 是否启用扩展属性 EnableExtendProperty
        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        [Label("是否启用扩展属性")]
        public static readonly Property<bool> EnableExtendPropertyProperty = P<MaterialPrepareItemViewModel>.Register(e => e.EnableExtendProperty);

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool EnableExtendProperty
        {
            get { return this.GetProperty(EnableExtendPropertyProperty); }
            set { this.SetProperty(EnableExtendPropertyProperty, value); }
        }
        #endregion

    }
}
