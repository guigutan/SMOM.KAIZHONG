using SIE.Common;
using SIE.Common.Configs;
using SIE.CSM.Suppliers.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Text.RegularExpressions;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [EntityWithConfig(typeof(SupplierIsAutoCreateConfig))]
    [EntityWithConfig(typeof(SupplierOutsourcingConfig))]
    [Label("供应商")]
    [DisplayMember(nameof(Supplier.Code))]
    public partial class Supplier : DataEntity, IStateEntity
    {
        /// <summary>
        /// 快码类型
        /// </summary>
        public static string SupperType { get; set; } = "SUPPLIER_TYPE";

        /// <summary>
        /// 快码类型：地区类型
        /// </summary>
        public static string CatalogAreaType { get; set; } = "AREA_TYPE";

        #region 构造函数
        /// <summary>
        /// 无参的构造函数
        /// </summary>
        public Supplier()
        {
            State = State.Enable;
            IsPortal = true;
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Supplier>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 全称 Name
        /// <summary>
        /// 全称
        /// </summary>
        [Required]
        [MaxLength(240)]
        [NotDuplicate]
        [Label("全称")]
        public static readonly Property<string> NameProperty = P<Supplier>.Register(e => e.Name);

        /// <summary>
        /// 全称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<Supplier>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>        
        [Label("类型")]
        public static readonly Property<string> TypeProperty = P<Supplier>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Supplier>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 简称 ShortName
        /// <summary>
        /// 简称
        /// </summary>
        [Label("简称")]
        public static readonly Property<string> ShortNameProperty = P<Supplier>.Register(e => e.ShortName);

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName
        {
            get { return GetProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }
        #endregion

        #region 英文名称 EnglishName
        /// <summary>
        /// 英文名称
        /// </summary>
        [Label("英文名称")]
        public static readonly Property<string> EnglishNameProperty = P<Supplier>.Register(e => e.EnglishName);

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName
        {
            get { return GetProperty(EnglishNameProperty); }
            set { SetProperty(EnglishNameProperty, value); }
        }
        #endregion

        #region 采购组织 PurchaseInv
        /// <summary>
        /// 采购组织
        /// </summary>
        [Label("采购组织")]
        public static readonly Property<string> PurchaseInvProperty = P<Supplier>.Register(e => e.PurchaseInv);

        /// <summary>
        /// 采购组织
        /// </summary>
        public string PurchaseInv
        {
            get { return GetProperty(PurchaseInvProperty); }
            set { SetProperty(PurchaseInvProperty, value); }
        }
        #endregion

        #region 所在区域 Region
        /// <summary>
        /// 所在区域
        /// </summary>
        [Label("所在区域")]
        public static readonly Property<string> RegionProperty = P<Supplier>.Register(e => e.Region);

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region
        {
            get { return GetProperty(RegionProperty); }
            set { SetProperty(RegionProperty, value); }
        }
        #endregion

        #region 税号 DutyParagraph
        /// <summary>
        /// 税号
        /// </summary>
        [Label("税号")]
        public static readonly Property<string> DutyParagraphProperty = P<Supplier>.Register(e => e.DutyParagraph);

        /// <summary>
        /// 税号
        /// </summary>
        public string DutyParagraph
        {
            get { return GetProperty(DutyParagraphProperty); }
            set { SetProperty(DutyParagraphProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<Supplier>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumberProperty = P<Supplier>.Register(e => e.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return GetProperty(ContactNumberProperty); }
            set { SetProperty(ContactNumberProperty, value); }
        }
        #endregion

        #region 联系地址 ContactAddress
        /// <summary>
        /// 联系地址
        /// </summary>
        [Label("联系地址")]
        public static readonly Property<string> ContactAddressProperty = P<Supplier>.Register(e => e.ContactAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress
        {
            get { return GetProperty(ContactAddressProperty); }
            set { SetProperty(ContactAddressProperty, value); }
        }
        #endregion

        #region 电子邮箱 EMail
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EMailProperty = P<Supplier>.Register(e => e.EMail);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMail
        {
            get { return GetProperty(EMailProperty); }
            set { SetProperty(EMailProperty, value); }
        }
        #endregion

        #region 邮编 ZipCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<Supplier>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<Supplier>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 是否门户 IsPortal
        /// <summary>
        /// 是否门户
        /// </summary>
        [Label("是否门户")]
        public static readonly Property<bool> IsPortalProperty = P<Supplier>.Register(e => e.IsPortal);

        /// <summary>
        /// 是否门户
        /// </summary>
        public bool IsPortal
        {
            get { return GetProperty(IsPortalProperty); }
            set { SetProperty(IsPortalProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> SourceTypeProperty = P<Supplier>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<Supplier>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 供应商与地址关系 AddressList
        /// <summary>
        /// 供应商与地址关系
        /// </summary>
        public static readonly ListProperty<EntityList<SupplierAddress>> AddressListProperty = P<Supplier>.RegisterList(e => e.AddressList);

        /// <summary>
        /// 供应商与地址关系
        /// </summary>
        public EntityList<SupplierAddress> AddressList
        {
            get { return this.GetLazyList(AddressListProperty); }
        }
        #endregion

        #region 供应商与物料关系 ItemList
        /// <summary>
        /// 供应商与物料关系
        /// </summary>
        public static readonly ListProperty<EntityList<SupplierItem>> ItemListProperty = P<Supplier>.RegisterList(e => e.ItemList);

        /// <summary>
        /// 供应商与物料关系
        /// </summary>
        public EntityList<SupplierItem> ItemList
        {
            get { return this.GetLazyList(ItemListProperty); }
        }
        #endregion

        #region 委外相关参数
        #region 委外发料调入库位 OutsourcingInLoc
        /// <summary>
        /// 委外发料调入库位ID
        /// </summary>
        [Label("委外发料调入库位")]
        public static readonly IRefIdProperty OutsourcingInLocIdProperty =
              P<Supplier>.RegisterRefId(e => e.OutsourcingInLocId, ReferenceType.Normal);

        /// <summary>
        /// 委外发料调入库位ID
        /// </summary>
        public double? OutsourcingInLocId
        {
            get { return (double?)this.GetRefNullableId(OutsourcingInLocIdProperty); }
            set { this.SetRefNullableId(OutsourcingInLocIdProperty, value); }
        }

        /// <summary>
        /// 委外发料调入库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> OutsourcingInLocProperty =
            P<Supplier>.RegisterRef(e => e.OutsourcingInLoc, OutsourcingInLocIdProperty);

        /// <summary>
        /// 委外发料调入库位
        /// </summary>
        public StorageLocation OutsourcingInLoc
        {
            get { return this.GetRefEntity(OutsourcingInLocProperty); }
            set { this.SetRefEntity(OutsourcingInLocProperty, value); }
        }
        #endregion

        #region 委外扣料库位 OutsourcingInLoc
        /// <summary>
        /// 委外扣料库位ID
        /// </summary>
        [Label("委外扣料库位")]
        public static readonly IRefIdProperty OutsourcingOutLocIdProperty =
              P<Supplier>.RegisterRefId(e => e.OutsourcingOutLocId, ReferenceType.Normal);

        /// <summary>
        /// 委外扣料库位ID
        /// </summary>
        public double? OutsourcingOutLocId
        {
            get { return (double?)this.GetRefNullableId(OutsourcingOutLocIdProperty); }
            set { this.SetRefNullableId(OutsourcingOutLocIdProperty, value); }
        }

        /// <summary>
        /// 委外扣料库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> OutsourcingOutLocProperty =
            P<Supplier>.RegisterRef(e => e.OutsourcingOutLoc, OutsourcingOutLocIdProperty);

        /// <summary>
        /// 委外扣料库位
        /// </summary>
        public StorageLocation OutsourcingOutLoc
        {
            get { return this.GetRefEntity(OutsourcingOutLocProperty); }
            set { this.SetRefEntity(OutsourcingOutLocProperty, value); }
        }
        #endregion

        #region 委外收货扣料处理 OutsourcingReceive
        /// <summary>
        /// 委外收货扣料处理
        /// </summary>
        [Label("委外收货扣料处理")]
        public static readonly Property<OutsourcingReceiveType> OutsourcingReceiveProperty = P<Supplier>.Register(e => e.OutsourcingReceive);

        /// <summary>
        /// 委外收货扣料处理
        /// </summary>
        public OutsourcingReceiveType OutsourcingReceive
        {
            get { return GetProperty(OutsourcingReceiveProperty); }
            set { SetProperty(OutsourcingReceiveProperty, value); }
        }
        #endregion

        #region 委外扣料时点 OutsourcingUseTime
        /// <summary>
        /// 委外扣料时点
        /// </summary>
        [Label("委外扣料时点")]
        public static readonly Property<OutsourcingTimeType> OutsourcingUseTimeProperty = P<Supplier>.Register(e => e.OutsourcingUseTime);

        /// <summary>
        /// 委外扣料时点
        /// </summary>
        public OutsourcingTimeType OutsourcingUseTime
        {
            get { return GetProperty(OutsourcingUseTimeProperty); }
            set { SetProperty(OutsourcingUseTimeProperty, value); }
        }
        #endregion

        #region 委外库存带货主管理 IsHasStorer
        /// <summary>
        /// 委外库存带货主管理
        /// </summary>
        [Label("委外库存带货主管理")]
        public static readonly Property<bool> IsHasStorerProperty = P<Supplier>.Register(e => e.IsHasStorer);

        /// <summary>
        /// 委外库存带货主管理
        /// </summary>
        public bool IsHasStorer
        {
            get { return this.GetProperty(IsHasStorerProperty); }
            set { this.SetProperty(IsHasStorerProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性
        #region 调入库位名称 OutsourcingInLocName
        /// <summary>
        /// 调入库位名称
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> OutsourcingInLocNameProperty = P<Supplier>.RegisterView(e => e.OutsourcingInLocName, p => p.OutsourcingInLoc.Name);

        /// <summary>
        /// 调入库位名称
        /// </summary>
        public string OutsourcingInLocName
        {
            get { return this.GetProperty(OutsourcingInLocNameProperty); }
        }
        #endregion

        #region 调入仓库 OutInWarehouseId
        /// <summary>
        /// 调入仓库
        /// </summary>
        [Label("调入仓库")]
        public static readonly Property<double?> OutInWarehouseIdProperty = P<Supplier>.RegisterView(e => e.OutInWarehouseId, p => p.OutsourcingInLoc.WarehouseId);

        /// <summary>
        /// 调入仓库
        /// </summary>
        public double? OutInWarehouseId
        {
            get { return this.GetProperty(OutInWarehouseIdProperty); }
        }
        #endregion

        #region 调入仓库名称 OutInWarehouseName
        /// <summary>
        /// 调入仓库名称
        /// </summary>
        [Label("调入仓库名称")]
        public static readonly Property<string> OutInWarehouseNameProperty = P<Supplier>.RegisterView(e => e.OutInWarehouseName, p => p.OutsourcingInLoc.Warehouse.Name);

        /// <summary>
        /// 调入仓库名称
        /// </summary>
        public string OutInWarehouseName
        {
            get { return this.GetProperty(OutInWarehouseNameProperty); }
        }
        #endregion

        #region 调入仓库类型 OutInWarehouseType
        /// <summary>
        /// 调入仓库类型
        /// </summary>
        [Label("调入仓库类型")]
        public static readonly Property<LibraryType> OutInWarehouseTypeProperty = P<Supplier>.RegisterView(e => e.OutInWarehouseType, p => p.OutsourcingInLoc.Warehouse.LibraryType);

        /// <summary>
        /// 调入仓库类型
        /// </summary>
        public LibraryType OutInWarehouseType
        {
            get { return this.GetProperty(OutInWarehouseTypeProperty); }
        }
        #endregion

        #region 委外扣料库位仓库 OutsourcingOutWhId
        /// <summary>
        /// 委外扣料库位仓库
        /// </summary>
        [Label("委外扣料库位仓库")]
        public static readonly Property<double> OutsourcingOutWhIdProperty = P<Supplier>.RegisterView(e => e.OutsourcingOutWhId, p => p.OutsourcingOutLoc.WarehouseId);

        /// <summary>
        /// 委外扣料库位仓库
        /// </summary>
        public double OutsourcingOutWhId
        {
            get { return this.GetProperty(OutsourcingOutWhIdProperty); }
        }
        #endregion

        #region 委外扣料仓库类型 OutsourcingOutWhType
        /// <summary>
        /// 委外扣料仓库类型
        /// </summary>
        [Label("委外扣料仓库类型")]
        public static readonly Property<LibraryType> OutsourcingOutWhTypeProperty = P<Supplier>.RegisterView(e => e.OutsourcingOutWhType, p => p.OutsourcingOutLoc.Warehouse.LibraryType);

        /// <summary>
        /// 委外扣料仓库类型
        /// </summary>
        public LibraryType OutsourcingOutWhType
        {
            get { return this.GetProperty(OutsourcingOutWhTypeProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 供应商 实体配置
    /// </summary>
    internal class SupplierConfig : EntityConfig<Supplier>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">电话号码验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(Supplier.ContactNumberProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$"),
                MessageBuilder = (o) =>
                {
                    return "联系电话格式不正确".L10N();
                }
            });
            rules.AddRule(Supplier.ZipCodeProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^\d{6}$"),
                MessageBuilder = (o) =>
                {
                    return "邮编格式不正确".L10N();
                }
            });
            rules.AddRule(Supplier.EMailProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "电子邮箱格式不正确".L10N();
                }
            });
        }

        /// <summary>
        /// 供应商数据库表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_SUPPLIER").MapAllProperties();
            Meta.Property(Supplier.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}