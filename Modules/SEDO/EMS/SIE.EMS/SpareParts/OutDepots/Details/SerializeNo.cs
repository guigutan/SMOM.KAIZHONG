using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 序列号
    /// </summary>
    [RootEntity, Serializable]    
    [Label("序列号")]
    public class SerializeNo : DataEntity
    {
        #region 设备出库单 OutDepot
        /// <summary>
        /// 设备出库单Id
        /// </summary>
        [Label("设备出库单")]
        public static readonly IRefIdProperty OutDepotIdProperty =
            P<SerializeNo>.RegisterRefId(e => e.OutDepotId, ReferenceType.Parent);

        /// <summary>
        /// 设备出库单Id
        /// </summary>
        public double OutDepotId
        {
            get { return (double)this.GetRefId(OutDepotIdProperty); }
            set { this.SetRefId(OutDepotIdProperty, value); }
        }

        /// <summary>
        /// 设备出库单
        /// </summary>
        public static readonly RefEntityProperty<OutDepot> OutDepotProperty =
            P<SerializeNo>.RegisterRef(e => e.OutDepot, OutDepotIdProperty);

        /// <summary>
        /// 设备出库单
        /// </summary>
        public OutDepot OutDepot
        {
            get { return this.GetRefEntity(OutDepotProperty); }
            set { this.SetRefEntity(OutDepotProperty, value); }
        }
        #endregion

        #region 序列号编码 Code
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Label("序列号编码")]
        [Required]
        public static readonly Property<string> CodeProperty = P<SerializeNo>.Register(e => e.Code);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 数量 Count
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<string> CountProperty = P<SerializeNo>.Register(e => e.Count);

        /// <summary>
        /// 数量
        /// </summary>
        public string Count
        {
            get { return this.GetProperty(CountProperty); }
            set { this.SetProperty(CountProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<SerializeNo>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion


    }
    internal class SerializeNoConfig : EntityConfig<SerializeNo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SERIALIE_NO").MapAllProperties();
            Meta.EnablePhantoms();

        }
    }
}
