using SIE.Core.Enums;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Equipments
{
    /// <summary>
    /// 设备型号维护
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [Label("设备型号维护")]
    public class EquipModel : DataEntity
    {

        #region 型号编码 Code
        /// <summary>
        /// 型号编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("型号编码")]
        public static readonly Property<string> CodeProperty = P<EquipModel>.Register(e => e.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 型号名称 Name
        /// <summary>
        /// 型号名称
        /// </summary>
        [Required]
        [Label("型号名称")]
        public static readonly Property<string> NameProperty = P<EquipModel>.Register(e => e.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        [Required]
        public static readonly Property<string> TypeCategoryProperty = P<EquipModel>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return this.GetProperty(TypeCategoryProperty); }
            set { this.SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<EquipModel>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)this.GetRefNullableId(EquipTypeIdProperty); }
            set { this.SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<EquipModel>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        [MaxLength(800)]
        public static readonly Property<string> SpecificationsProperty = P<EquipModel>.Register(e => e.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return GetProperty(SpecificationsProperty); }
            set { SetProperty(SpecificationsProperty, value); }
        }
        #endregion

        #region 生产厂商 Manufacturers
        /// <summary>
        /// 生产厂商
        /// </summary>
        [Label("生产厂商")]
        [MaxLength(800)]
        public static readonly Property<string> ManufacturersProperty = P<EquipModel>.Register(e => e.Manufacturers);

        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturers
        {
            get { return GetProperty(ManufacturersProperty); }
            set { SetProperty(ManufacturersProperty, value); }
        }
        #endregion

        #region 行业属性 IndustryCategory
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        [Required]
        public static readonly Property<IndustryCategory> IndustryCategoryProperty = P<EquipModel>.Register(e => e.IndustryCategory);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryCategory IndustryCategory
        {
            get { return GetProperty(IndustryCategoryProperty); }
            set { SetProperty(IndustryCategoryProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备类型编码 TypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> TypeCodeProperty = P<EquipModel>.RegisterView(e => e.TypeCode, p => p.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string TypeCode
        {
            get { return this.GetProperty(TypeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 TypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型名称")]
        public static readonly Property<string> TypeNameProperty = P<EquipModel>.RegisterView(e => e.TypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string TypeName
        {
            get { return this.GetProperty(TypeNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class EquipModelConfig : EntityConfig<EquipModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MODEL").MapAllProperties();
            Meta.Property(EquipModel.CodeProperty).ColumnMeta.HasLength(200);
            Meta.EnablePhantoms();
        }
    }
}