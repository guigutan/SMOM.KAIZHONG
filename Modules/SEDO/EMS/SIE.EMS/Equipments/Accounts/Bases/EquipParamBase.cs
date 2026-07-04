using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 仪器参数
    /// </summary>
    [RootEntity, Serializable]
    [Label("仪器参数")]
    [DisplayMember(nameof(Name))]
    public class EquipParamBase : DataEntity
    {
        #region 参数项目 Name
        /// <summary>
        /// 参数项目
        /// </summary>
        [Required]
        [Label("参数项目")]
        public static readonly Property<string> NameProperty = P<EquipParamBase>.Register(e => e.Name);

        /// <summary>
        /// 参数项目
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<EquipParamBase>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<EquipParamBase>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 最大值 Max
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxProperty = P<EquipParamBase>.Register(e => e.Max);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? Max
        {
            get { return GetProperty(MaxProperty); }
            set { SetProperty(MaxProperty, value); }
        }
        #endregion

        #region 最小值 Min
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinProperty = P<EquipParamBase>.Register(e => e.Min);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? Min
        {
            get { return GetProperty(MinProperty); }
            set { SetProperty(MinProperty, value); }
        }
        #endregion

        #region 备注信息 Remark
        /// <summary>
        /// 备注信息
        /// </summary>
        [MaxLength(2000)]
        [Label("备注信息")]
        public static readonly Property<string> RemarkProperty = P<EquipParamBase>.Register(e => e.Remark);

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 仪器参数 实体配置
    /// </summary>
    internal class EquipParamBaseConfig : EntityConfig<EquipParamBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_PARAM").MapAllProperties();
            Meta.Property(EquipParamBase.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}