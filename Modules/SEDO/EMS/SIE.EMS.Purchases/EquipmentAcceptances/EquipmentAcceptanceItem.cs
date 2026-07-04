using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 验收项目
    /// </summary>
    [ChildEntity, Serializable]
    [Label("验收项目")]
    public partial class EquipmentAcceptanceItem : DataEntity
    {
        #region 项目名称 ItemName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        [Required]
        public static readonly Property<string> ItemNameProperty = P<EquipmentAcceptanceItem>.Register(e => e.ItemName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 验收值 AcceptanceValue
        /// <summary>
        /// 验收值
        /// </summary>
        [Label("验收值")]
        [Required]
        public static readonly Property<string> AcceptanceValueProperty = P<EquipmentAcceptanceItem>.Register(e => e.AcceptanceValue);

        /// <summary>
        /// 验收值
        /// </summary>
        public string AcceptanceValue
        {
            get { return GetProperty(AcceptanceValueProperty); }
            set { SetProperty(AcceptanceValueProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipmentAcceptanceItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 验收结果 AcceptanceResult
        /// <summary>
        /// 验收结果
        /// </summary>
        [Label("验收结果")]
        public static readonly Property<InspectionResult> AcceptanceResultProperty = P<EquipmentAcceptanceItem>.Register(e => e.AcceptanceResult);

        /// <summary>
        /// 验收结果
        /// </summary>
        public InspectionResult AcceptanceResult
        {
            get { return GetProperty(AcceptanceResultProperty); }
            set { SetProperty(AcceptanceResultProperty, value); }
        }
        #endregion

        #region 设备开箱验收 EquipmentAcceptance
        /// <summary>
        /// 设备开箱验收Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentAcceptanceIdProperty = P<EquipmentAcceptanceItem>.RegisterRefId(e => e.EquipmentAcceptanceId, ReferenceType.Parent);

        /// <summary>
        /// 设备开箱验收Id
        /// </summary>
        public double EquipmentAcceptanceId
        {
            get { return (double)GetRefId(EquipmentAcceptanceIdProperty); }
            set { SetRefId(EquipmentAcceptanceIdProperty, value); }
        }

        /// <summary>
        /// 设备开箱验收
        /// </summary>
        public static readonly RefEntityProperty<EquipmentAcceptance> EquipmentAcceptanceProperty = P<EquipmentAcceptanceItem>.RegisterRef(e => e.EquipmentAcceptance, EquipmentAcceptanceIdProperty);

        /// <summary>
        /// 设备开箱验收
        /// </summary>
        public EquipmentAcceptance EquipmentAcceptance
        {
            get { return GetRefEntity(EquipmentAcceptanceProperty); }
            set { SetRefEntity(EquipmentAcceptanceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 验收项目 实体配置
    /// </summary>
    internal class EquipmentAcceptanceItemConfig : EntityConfig<EquipmentAcceptanceItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_ACPT_ITEM").MapAllProperties();
            Meta.Property(EquipmentAcceptanceItem.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}