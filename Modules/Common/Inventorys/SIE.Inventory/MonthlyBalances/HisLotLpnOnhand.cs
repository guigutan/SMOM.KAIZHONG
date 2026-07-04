using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.Onhands;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using SIE.Inventory.Commom;

namespace SIE.Inventory.MonthlyBalances
{
    /// <summary>
    /// 历史LPN库存记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("历史LPN库存记录")]
    public partial class HisLotLpnOnhand : SIE.Inventory.Onhands.BaseOnhand
    {
        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<HisLotLpnOnhand>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次Id
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty = P<HisLotLpnOnhand>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次Id
        /// </summary>
        public double LotId
        {
            get { return (double)GetRefId(LotIdProperty); }
            set { SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty = P<HisLotLpnOnhand>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return GetRefEntity(LotProperty); }
            set { SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<HisLotLpnOnhand>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<HisLotLpnOnhand>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return GetProperty(LpnProperty); }
            set { SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<HisLotLpnOnhand>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<HisLotLpnOnhand>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<HisLotLpnOnhand>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropNameProperty = P<HisLotLpnOnhand>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 库存状态 State
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> StateProperty = P<HisLotLpnOnhand>.Register(e => e.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 生成日期 GenerationDate
        /// <summary>
        /// 生成日期
        /// </summary>
        [Label("生成日期")]
        public static readonly Property<DateTime> GenerationDateProperty = P<HisLotLpnOnhand>.Register(e => e.GenerationDate);

        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime GenerationDate
        {
            get { return this.GetProperty(GenerationDateProperty); }
            set { this.SetProperty(GenerationDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 历史LPN库存记录 实体配置
    /// </summary>
    internal class HisLotLpnOnhandConfig : EntityConfig<HisLotLpnOnhand>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_HIS_LOT_LPN_ONHAND").MapAllProperties();
            Meta.Property(HisLotLpnOnhand.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}