using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件基础数据
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartCriteria))]
    [DisplayMember(nameof(SparePartCode))]
    [Label("备件基础数据")]
    public partial class SparePart : DataEntity, IStateEntity
    {
        #region 编码 SparePartCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> SparePartCodeProperty = P<SparePart>.Register(e => e.SparePartCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode
        {
            get { return GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 名称 SparePartName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        public static readonly Property<string> SparePartNameProperty = P<SparePart>.Register(e => e.SparePartName);

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName
        {
            get { return GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<SparePart>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 原厂料号 OriginalItemCode
        /// <summary>
        /// 原厂料号
        /// </summary>
        [Label("原厂料号")]
        public static readonly Property<string> OriginalItemCodeProperty = P<SparePart>.Register(e => e.OriginalItemCode);

        /// <summary>
        /// 原厂料号
        /// </summary>
        public string OriginalItemCode
        {
            get { return GetProperty(OriginalItemCodeProperty); }
            set { SetProperty(OriginalItemCodeProperty, value); }
        }
        #endregion

        #region 以旧换新 IsReplacement
        /// <summary>
        /// 以旧换新
        /// </summary>
        [Label("以旧换新")]
        public static readonly Property<bool> IsReplacementProperty = P<SparePart>.Register(e => e.IsReplacement);

        /// <summary>
        /// 以旧换新
        /// </summary>
        public bool IsReplacement
        {
            get { return GetProperty(IsReplacementProperty); }
            set { SetProperty(IsReplacementProperty, value); }
        }
        #endregion

        #region 制造商 Manufacturer
        /// <summary>
        /// 制造商
        /// </summary>
        [Label("制造商")]
        public static readonly Property<string> ManufacturerProperty = P<SparePart>.Register(e => e.Manufacturer);

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer
        {
            get { return GetProperty(ManufacturerProperty); }
            set { SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierProperty = P<SparePart>.Register(e => e.Supplier);

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier
        {
            get { return GetProperty(SupplierProperty); }
            set { SetProperty(SupplierProperty, value); }
        }
        #endregion

        #region 安全库存 SafeStock
        /// <summary>
        /// 安全库存
        /// </summary>
        [MinValue(0)]
        [Label("安全库存")]
        public static readonly Property<int> SafeStockProperty = P<SparePart>.Register(e => e.SafeStock);

        /// <summary>
        /// 安全库存
        /// </summary>
        public int SafeStock
        {
            get { return GetProperty(SafeStockProperty); }
            set { SetProperty(SafeStockProperty, value); }
        }
        #endregion

        #region 存放仓库 WareHouse
        /// <summary>
        /// 存放仓库
        /// </summary>
        [Label("存放仓库")]
        public static readonly Property<string> WareHouseProperty = P<SparePart>.Register(e => e.WareHouse);

        /// <summary>
        /// 存放仓库
        /// </summary>
        public string WareHouse
        {
            get { return GetProperty(WareHouseProperty); }
            set { SetProperty(WareHouseProperty, value); }
        }
        #endregion

        #region 库位 StorageArea
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageAreaProperty = P<SparePart>.Register(e => e.StorageArea);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageArea
        {
            get { return GetProperty(StorageAreaProperty); }
            set { SetProperty(StorageAreaProperty, value); }
        }
        #endregion

        #region 更换周期(天) LifeTime
        /// <summary>
        /// 更换周期(天)
        /// </summary>
        [Label("更换周期(天)")]
        public static readonly Property<int?> LifeTimeProperty = P<SparePart>.Register(e => e.LifeTime);

        /// <summary>
        /// 更换周期(天)
        /// </summary>
        public int? LifeTime
        {
            get { return GetProperty(LifeTimeProperty); }
            set { SetProperty(LifeTimeProperty, value); }
        }
        #endregion

        #region 可用时间(小时) UseTime
        /// <summary>
        /// 可用时间(小时)
        /// </summary>
        [Label("可用时间(小时)")]
        public static readonly Property<int?> UseTimeProperty = P<SparePart>.Register(e => e.UseTime);

        /// <summary>
        /// 可用时间(小时)
        /// </summary>
        public int? UseTime
        {
            get { return this.GetProperty(UseTimeProperty); }
            set { this.SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 单价 UnitPrice
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal?> UnitPriceProperty = P<SparePart>.Register(e => e.UnitPrice);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<SparePart>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<SparePart>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 图片列表 PictureAttachmentList
        /// <summary>
        /// 图片列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartPictureAttachment>> PictureAttachmentListProperty = P<SparePart>.RegisterList(e => e.PictureAttachmentList);
        /// <summary>
        /// 图片列表
        /// </summary>
        public EntityList<SparePartPictureAttachment> PictureAttachmentList
        {
            get { return this.GetLazyList(PictureAttachmentListProperty); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAttachment>> AttachmentListProperty = P<SparePart>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<SparePartAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<SparePart>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否同步物料 IsImportItem
        /// <summary>
        /// 是否同步物料
        /// </summary>
        [Label("是否同步物料")]
        public static readonly Property<bool?> IsImportItemProperty = P<SparePart>.Register(e => e.IsImportItem);

        /// <summary>
        /// 是否同步物料
        /// </summary>
        public bool? IsImportItem
        {
            get { return this.GetProperty(IsImportItemProperty); }
            set { this.SetProperty(IsImportItemProperty, value); }
        }
        #endregion

        #region 分类层级 ItemCategory
        /// <summary>
        /// 分类层级Id
        /// </summary>
        //[Required]
        [Label("分类层级")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<SparePart>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类层级Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(ItemCategoryIdProperty); }
            set { this.SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类层级
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty =
            P<SparePart>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类层级
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<SparePart>.Register(e => e.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return GetProperty(SpartTypeProperty); }
            set { SetProperty(SpartTypeProperty, value); }
        }
        #endregion

        #region 设备类型 SpartEquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty SpartEquipTypeIdProperty =
            P<SparePart>.RegisterRefId(e => e.SpartEquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? SpartEquipTypeId
        {
            get { return (double?)this.GetRefNullableId(SpartEquipTypeIdProperty); }
            set { this.SetRefNullableId(SpartEquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> SpartEquipTypeProperty =
            P<SparePart>.RegisterRef(e => e.SpartEquipType, SpartEquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType SpartEquipType
        {
            get { return this.GetRefEntity(SpartEquipTypeProperty); }
            set { this.SetRefEntity(SpartEquipTypeProperty, value); }
        }
        #endregion

        #region 设备型号 SpartEquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty SpartEquipModelIdProperty =
            P<SparePart>.RegisterRefId(e => e.SpartEquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? SpartEquipModelId
        {
            get { return (double?)this.GetRefNullableId(SpartEquipModelIdProperty); }
            set { this.SetRefNullableId(SpartEquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> SpartEquipModelProperty =
            P<SparePart>.RegisterRef(e => e.SpartEquipModel, SpartEquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel SpartEquipModel
        {
            get { return this.GetRefEntity(SpartEquipModelProperty); }
            set { this.SetRefEntity(SpartEquipModelProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Required]
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePart>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return GetProperty(ControlMethodProperty); }
            set { SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 物料 SparePartItem
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty SparePartItemIdProperty =
            P<SparePart>.RegisterRefId(e => e.SparePartItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? SparePartItemId
        {
            get { return (double?)this.GetRefNullableId(SparePartItemIdProperty); }
            set { this.SetRefNullableId(SparePartItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> SparePartItemProperty =
            P<SparePart>.RegisterRef(e => e.SparePartItem, SparePartItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item SparePartItem
        {
            get { return this.GetRefEntity(SparePartItemProperty); }
            set { this.SetRefEntity(SparePartItemProperty, value); }
        }
        #endregion

        #region 免检 ExemptionInspect
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> ExemptionInspectProperty = P<SparePart>.Register(e => e.ExemptionInspect);

        /// <summary>
        /// 免检
        /// </summary>
        public bool ExemptionInspect
        {
            get { return this.GetProperty(ExemptionInspectProperty); }
            set { this.SetProperty(ExemptionInspectProperty, value); }
        }
        #endregion

        #region 良品库存 GoodNumber
        /// <summary>
        /// 良品库存
        /// </summary>
        [Label("良品库存")]
        public static readonly Property<int> GoodNumberProperty = P<SparePart>.Register(e => e.GoodNumber);

        /// <summary>
        /// 良品库存
        /// </summary>
        public int GoodNumber
        {
            get { return this.GetProperty(GoodNumberProperty); }
            set { this.SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 不良品库存 RotNumber
        /// <summary>
        /// 不良品库存
        /// </summary>
        [Label("不良品库存")]
        public static readonly Property<int> RotNumberProperty = P<SparePart>.Register(e => e.RotNumber);

        /// <summary>
        /// 不良品库存
        /// </summary>
        public int RotNumber
        {
            get { return this.GetProperty(RotNumberProperty); }
            set { this.SetProperty(RotNumberProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 备件类型名称 SparePartTypeName
        /// <summary>
        /// 备件类型名称
        /// </summary>
        [Label("备件类型名称")]
        public static readonly Property<string> SparePartTypeNameProperty = P<SparePart>.RegisterView(e => e.SparePartTypeName, p => p.SpartEquipType.TypeName);

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName
        {
            get { return this.GetProperty(SparePartTypeNameProperty); }
        }
        #endregion

        #region 备件类型编码 SparePartTypeCode
        /// <summary>
        /// 备件类型编码
        /// </summary>
        [Label("备件类型编码")]
        public static readonly Property<string> SparePartTypeCodeProperty = P<SparePart>.RegisterView(e => e.SparePartTypeCode, p => p.ItemCategory.Code);

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode
        {
            get { return this.GetProperty(SparePartTypeCodeProperty); }
        }
        #endregion

        #region 设备型号 EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty = P<SparePart>.RegisterView(e => e.EquipModelCode, p => p.SpartEquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelNameProperty = P<SparePart>.RegisterView(e => e.EquipModelName, p => p.SpartEquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 单位编码 UnitCode
        /// <summary>
        /// 单位编码
        /// </summary>
        [Label("单位编码")]
        public static readonly Property<string> UnitCodeProperty = P<SparePart>.RegisterView(e => e.UnitCode, p => p.Unit.Code);

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SparePart>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 分类名称 ItemCategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [Label("分类名称")]
        public static readonly Property<string> ItemCategoryNameProperty = P<SparePart>.RegisterView(e => e.ItemCategoryName, p => p.ItemCategory.Name);

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
        #endregion

        #region 设备类型 EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<SparePart>.RegisterView(e => e.EquipTypeCode, p => p.SpartEquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 备件基础数据 实体配置
    /// </summary>
    internal class SparePartConfig : EntityConfig<SparePart>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART").MapAllProperties();
            Meta.Property(SparePart.GoodNumberProperty).DontMapColumn();
            Meta.Property(SparePart.RotNumberProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }

		/// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
		{
			rules.AddRule(new HandlerRule()
			{
				Handler = (o, e) =>
				{
					var para = o.CastTo<SparePart>();
                    StringBuilder sb = new StringBuilder();

                    if (para.ItemCategory == null && ((para.IsImportItem != true) || (para.IsImportItem == true && para.PersistenceStatus == PersistenceStatus.Modified)))
                    {
                        sb.AppendLine("【分类层级】不能为空！".L10N());
                    }
                    if (para.SpartType == SparePartType.Special && para.SpartEquipType == null && para.SpartEquipModel == null && ((para.IsImportItem != true) || (para.IsImportItem == true && para.PersistenceStatus == PersistenceStatus.Modified)))
                    {
                        sb.AppendLine("专用备件时，【设备类型】和【设备型号】需至少填写一个！".L10N());
                    }
                    if (para.SpartType != SparePartType.Special && (para.SpartEquipType != null || para.SpartEquipModel != null) && ((para.IsImportItem != true) || (para.IsImportItem == true && para.PersistenceStatus == PersistenceStatus.Modified)))
                    {
                        sb.AppendLine("【{0}】为非专用备件，【设备类型】和【设备型号】无需填写！".L10nFormat(para.SparePartCode));
                    }
                    if (para.LifeTime != null && para.LifeTime <= 0)
                    {
                        sb.AppendLine("【更换周期(天)】的值需大于0！".L10N());
                    }
                    if (para.UseTime != null && para.UseTime <= 0)
                    {
                        sb.AppendLine("【可用时间(小时)】的值需大于0！".L10N());
                    }
                    if (para.UnitPrice != null && para.UnitPrice <= 0)
                    {
                        sb.AppendLine("【单价】的值需大于0！".L10N());
                    }
                    e.BrokenDescription = sb.ToString();
                }
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}