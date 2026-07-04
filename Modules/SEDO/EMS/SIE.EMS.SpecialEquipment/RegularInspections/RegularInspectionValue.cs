using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 定检检验数据
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("定检检验数据")]
    public partial class RegularInspectionValue : DataEntity
    {
        #region 数据 Value
        /// <summary>
        /// 数据
        /// </summary>
        [Label("数据")]
        public static readonly Property<decimal?> ValueProperty = P<RegularInspectionValue>.Register(e => e.Value);

        /// <summary>
        /// 数据
        /// </summary>
        public decimal? Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 序号 Index
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<int> IndexProperty = P<RegularInspectionValue>.Register(e => e.Index);

        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return GetProperty(IndexProperty); }
            set { SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 检验明细 RegularInspectionDetail
        /// <summary>
        /// 检验明细Id
        /// </summary>
        public static readonly IRefIdProperty RegularInspectionDetailIdProperty = P<RegularInspectionValue>.RegisterRefId(e => e.RegularInspectionDetailId, ReferenceType.Parent);

        /// <summary>
        /// 检验明细Id
        /// </summary>
        public double RegularInspectionDetailId
        {
            get { return (double)GetRefId(RegularInspectionDetailIdProperty); }
            set { SetRefId(RegularInspectionDetailIdProperty, value); }
        }

        /// <summary>
        /// 检验明细
        /// </summary>
        public static readonly RefEntityProperty<RegularInspectionDetail> RegularInspectionDetailProperty = P<RegularInspectionValue>.RegisterRef(e => e.RegularInspectionDetail, RegularInspectionDetailIdProperty);

        /// <summary>
        /// 检验明细
        /// </summary>
        public RegularInspectionDetail RegularInspectionDetail
        {
            get { return GetRefEntity(RegularInspectionDetailProperty); }
            set { SetRefEntity(RegularInspectionDetailProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 定检检验数据 实体配置
    /// </summary>
    internal class RegularInspectionValueConfig : EntityConfig<RegularInspectionValue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_REG_INS_VAL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}