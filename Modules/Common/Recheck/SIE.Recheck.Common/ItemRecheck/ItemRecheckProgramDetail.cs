using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// 物料复检方案明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料复检方案明细")]
    public class ItemRecheckProgramDetail : DataEntity
    {
        #region 复检次序 RecheckSort
        /// <summary>
        /// 复检次序
        /// </summary>
        [Label("复检次序")]
        public static readonly Property<string> RecheckSortProperty = P<ItemRecheckProgramDetail>.Register(e => e.RecheckSort);

        /// <summary>
        /// 复检次序
        /// </summary>
        public string RecheckSort
        {
            get { return this.GetProperty(RecheckSortProperty); }
            set { this.SetProperty(RecheckSortProperty, value); }
        }
        #endregion

        #region 次序 Sort
        /// <summary>
        /// 次序
        /// </summary>
        [Label("次序")]
        [MinValue(1)]
        public static readonly Property<int> SortProperty = P<ItemRecheckProgramDetail>.Register(e => e.Sort);

        /// <summary>
        /// 次序
        /// </summary>
        public int Sort
        {
            get { return this.GetProperty(SortProperty); }
            set { this.SetProperty(SortProperty, value); }
        }
        #endregion

        #region 复检延长保质期(天) LimitDays
        /// <summary>
        /// 复检延长保质期(天)
        /// </summary>
        [Label("复检延长保质期(天)")]
        [MinValue(1)]
        public static readonly Property<decimal> LimitDaysProperty = P<ItemRecheckProgramDetail>.Register(e => e.LimitDays);

        /// <summary>
        /// 复检延长保质期(天)
        /// </summary>
        public decimal LimitDays
        {
            get { return this.GetProperty(LimitDaysProperty); }
            set { this.SetProperty(LimitDaysProperty, value); }
        }
        #endregion

        #region 物料复检方案 ItemRecheckProgram
        /// <summary>
        /// 物料复检方案Id
        /// </summary>
        public static readonly IRefIdProperty ItemRecheckProgramIdProperty = P<ItemRecheckProgramDetail>.RegisterRefId(e => e.ItemRecheckProgramId, ReferenceType.Parent);

        /// <summary>
        /// 物料复检方案Id
        /// </summary>
        public double ItemRecheckProgramId
        {
            get { return (double)GetRefId(ItemRecheckProgramIdProperty); }
            set { SetRefId(ItemRecheckProgramIdProperty, value); }
        }

        /// <summary>
        /// 物料复检方案
        /// </summary>
        public static readonly RefEntityProperty<ItemRecheckProgram> ItemRecheckProgramProperty = P<ItemRecheckProgramDetail>.RegisterRef(e => e.ItemRecheckProgram, ItemRecheckProgramIdProperty);

        /// <summary>
        /// 物料复检方案
        /// </summary>
        public ItemRecheckProgram ItemRecheckProgram
        {
            get { return GetRefEntity(ItemRecheckProgramProperty); }
            set { SetRefEntity(ItemRecheckProgramProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料批次方案 实体配置
    /// </summary>
    internal class ItemRecheckProgramDetailConfig : EntityConfig<ItemRecheckProgramDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_RECHECK_PROGRAM_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }
    }
}
