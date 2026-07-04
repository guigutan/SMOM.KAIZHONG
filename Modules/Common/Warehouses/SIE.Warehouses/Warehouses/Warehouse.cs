using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS.Inventory;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses.Configs;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WarehouseCriteria))]
    [EntityWithConfig(typeof(WarehousesCodeConfig))]
    [Label("仓库")]
    [DisplayMember(nameof(Name))]
    public partial class Warehouse : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Warehouse() { State = State.Enable; }

        /// <summary>
        /// 快码类型：分类
        /// </summary>
        public const string CatalogCategory = "WAREHOUSE_TYPE";

        /// <summary>
        /// 默认库区、库位的编码
        /// </summary>
        public const string STAGE = "STAGE";

        /// <summary>
        /// 默认库区、库位的编码
        /// </summary>
        public const string PICKTO = "PICKTO";

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Warehouse>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Warehouse>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 分类 Category
        /// <summary>
        /// 分类
        /// </summary>       
        [Label("分类")]
        [MaxLength(80)]
        public static readonly Property<string> CategoryProperty = P<Warehouse>.Register(e => e.Category);

        /// <summary>
        /// 分类
        /// </summary>
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 简码 SimpleCode
        /// <summary>
        /// 简码
        /// </summary>
        [Label("简码")]
        [MaxLength(80)]
        public static readonly Property<string> SimpleCodeProperty = P<Warehouse>.Register(e => e.SimpleCode);

        /// <summary>
        /// 简码
        /// </summary>
        public string SimpleCode
        {
            get { return GetProperty(SimpleCodeProperty); }
            set { SetProperty(SimpleCodeProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool> IsFrozenProperty = P<Warehouse>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<Warehouse>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 仓库地址列表 WarehouseAddressList
        /// <summary>
        /// 仓库地址列表
        /// </summary>
        public static readonly ListProperty<EntityList<WarehouseAddress>> WarehouseAddressListProperty = P<Warehouse>.RegisterList(e => e.WarehouseAddressList);

        /// <summary>
        /// 仓库地址列表
        /// </summary>
        public EntityList<WarehouseAddress> WarehouseAddressList
        {
            get { return this.GetLazyList(WarehouseAddressListProperty); }
        }
        #endregion

        #region 仓库与员工关系 EmployeeList
        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public static readonly ListProperty<EntityList<WarehouseEmployee>> EmployeeListProperty = P<Warehouse>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 仓库与员工关系
        /// </summary>
        public EntityList<WarehouseEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

        #region 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Warehouse>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<Warehouse>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 是否线边仓 IsLineWarehouse
        /// <summary>
        /// 是否线边仓
        /// </summary>
        [Label("是否线边仓")]
        public static readonly Property<bool> IsLineWarehouseProperty = P<Warehouse>.Register(e => e.IsLineWarehouse);

        /// <summary>
        /// 是否线边仓
        /// </summary>
        public bool IsLineWarehouse
        {
            get { return this.GetProperty(IsLineWarehouseProperty); }
            set { this.SetProperty(IsLineWarehouseProperty, value); }
        }
        #endregion

        #region 库存组织名称 InvOrgName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<string> InvOrgNameProperty = P<Warehouse>.Register(e => e.InvOrgName);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName
        {
            get { return this.GetProperty(InvOrgNameProperty); }
            set { this.SetProperty(InvOrgNameProperty, value); }
        }
        #endregion

        #region 库存组织编码 InvOrgCode
        /// <summary>
        /// 库存组织Id
        /// </summary>
        [Label("库存组织编码")]
        public static readonly Property<int?> InvOrgCodeProperty = P<Warehouse>.Register(e => e.InvOrgCode);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int? InvOrgCode
        {
            get { return this.GetProperty(InvOrgCodeProperty); }
            set { this.SetProperty(InvOrgCodeProperty, value); }
        }
        #endregion

        #region 不管库存 IngoreOnhand
        /// <summary>
        /// 不管库存
        /// </summary>
        [Label("不管库存")]
        public static readonly Property<bool> IngoreOnhandProperty = P<Warehouse>.Register(e => e.IngoreOnhand);

        /// <summary>
        /// 不管库存
        /// </summary>
        public bool IngoreOnhand
        {
            get { return this.GetProperty(IngoreOnhandProperty); }
            set { this.SetProperty(IngoreOnhandProperty, value); }
        }
        #endregion

     
    }

    /// <summary>
    /// 仓库 实体配置
    /// </summary>
    internal class WarehouseConfig : EntityConfig<Warehouse>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new HandlerRule()
            {
                Handler = (o,e) =>
                {
                    var wareHouse = o.CastTo<Warehouse>();
                    var oldWareHouse = RF.GetById<Warehouse>(wareHouse.Id);
                    if (oldWareHouse != null)
                    {
                        if (wareHouse.IsLineWarehouse != oldWareHouse.IsLineWarehouse)
                        {
                            //说明已经更改了线边仓选项 判断该仓库是否有库存
                            var flag = RT.Service.Resolve<IGetLotLpnOnhand>().IsWareHouseHasOnHand(wareHouse.Id);
                            if (flag)
                            {
                                e.BrokenDescription = "当前仓库[{0}]已存在现有量大于0的库存数据，无法更改其线边仓属性".L10nFormat(wareHouse.Code);
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH").MapAllProperties();
            Meta.Property(Warehouse.InvOrgNameProperty).DontMapColumn();
            Meta.Property(Warehouse.InvOrgCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
            //  Meta.EnableSort();
        }
    }

    /// <summary>
    /// 扩展仓库基本资料
    /// </summary>
    [CompiledPropertyDeclarer]
    public class WarehouseInfoDetailProperty
    {
        /// <summary>
        /// 扩展仓库资料
        /// </summary>
        public static readonly Property<WarehouseInfo> WarehouseInfoProperty =
            P<Warehouse>.RegisterExtension<WarehouseInfo>("WarehouseInfoTem", typeof(WarehouseInfoDetailProperty));

        /// <summary>
        /// 获取仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <returns>返回仓库资料对象</returns>
        public static WarehouseInfo GetWarehouseInfoDetail(Warehouse me)
        {
            return me.GetProperty(WarehouseInfoProperty);
        }

        /// <summary>
        /// 设置仓库资料对象
        /// </summary>
        /// <param name="me">仓库对象</param>
        /// <param name="value">需要设置的仓库对象</param>
        public static void SetWarehouseInfoDetail(Warehouse me, WarehouseInfo value)
        {
            me.SetProperty(WarehouseInfoProperty, value);
        }

        /// <summary>
        /// 扩展仓库资料 实体配置
        /// </summary>
        internal class WarehouseInfoDetailPropertyConfig : EntityConfig<Warehouse>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(WarehouseInfoDetailProperty.WarehouseInfoProperty).DontMapColumn();
            }
        }
    }

    /// <summary>
    /// 仓库扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("仓库扩展")]
    public partial class WarehouseJoin : Warehouse//!为了自定义查询块与列表块，多个join使用
    {
    }
}