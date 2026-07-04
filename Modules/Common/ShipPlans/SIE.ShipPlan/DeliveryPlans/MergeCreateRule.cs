using SIE.Core.Enums;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 合并创单规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("合并创单规则")]
    public partial class MergeCreateRule : DataEntity
    {
        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<MergeCreateRule>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 同发货日期 IsSameDeliveryDate
        /// <summary>
        /// 同发货日期
        /// </summary>
        [Label("同发货日期")]
        public static readonly Property<bool> IsSameDeliveryDateProperty = P<MergeCreateRule>.Register(e => e.IsSameDeliveryDate);

        /// <summary>
        /// 同发货日期
        /// </summary>
        public bool IsSameDeliveryDate
        {
            get { return GetProperty(IsSameDeliveryDateProperty); }
            set { SetProperty(IsSameDeliveryDateProperty, value); }
        }
        #endregion

        #region 同单号 IsSameNo
        /// <summary>
        /// 同单号
        /// </summary>
        [Label("同单号")]
        public static readonly Property<bool> IsSameNoProperty = P<MergeCreateRule>.Register(e => e.IsSameNo);

        /// <summary>
        /// 同单号
        /// </summary>
        public bool IsSameNo
        {
            get { return GetProperty(IsSameNoProperty); }
            set { SetProperty(IsSameNoProperty, value); }
        }
        #endregion

        #region 同相关单号 IsSameOrderNo
        /// <summary>
        /// 同相关单号
        /// </summary>
        [Label("同相关单号")]
        public static readonly Property<bool> IsSameOrderNoProperty = P<MergeCreateRule>.Register(e => e.IsSameOrderNo);

        /// <summary>
        /// 同相关单号
        /// </summary>
        public bool IsSameOrderNo
        {
            get { return GetProperty(IsSameOrderNoProperty); }
            set { SetProperty(IsSameOrderNoProperty, value); }
        }
        #endregion

        #region 默认满足条件 DefaultDesc
        /// <summary>
        /// 默认满足条件
        /// </summary>
        [Label("默认满足条件")]
        public static readonly Property<string> DefaultDescProperty = P<MergeCreateRule>.Register(e => e.DefaultDesc);

        /// <summary>
        /// 默认满足条件
        /// </summary>
        public string DefaultDesc
        {
            get { return GetProperty(DefaultDescProperty); }
            set { SetProperty(DefaultDescProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 分配仓库规则 实体配置
    /// </summary>
    internal class MergeCreateRuleConfig : EntityConfig<MergeCreateRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MERGE_CREATE_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
