using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Warehouses;
using System;

namespace SIE.Wpf.Packages.Packings
{
    /// <summary>
    /// 工作站信息
    /// </summary>
    [RootEntity, Serializable]
    public class Workstation : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public Workstation()
        {
        }
        #endregion

        #region User 员工
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<Workstation>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty =
            P<Workstation>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public Employee User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region Warehouse 仓库

        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<Workstation>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<Workstation>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 货区ID
        /// </summary>
        [Label("货区")]
        public static readonly IRefIdProperty StorageAreaIdProperty =
            P<Workstation>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 货区ID
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)this.GetRefNullableId(StorageAreaIdProperty); }
            set { this.SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty =
            P<Workstation>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return this.GetRefEntity(StorageAreaProperty); }
            set { this.SetRefEntity(StorageAreaProperty, value); }
        }

        /// <summary>
        /// 货位ID
        /// </summary>
        [Label("货位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
             P<Workstation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 货位id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 货位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<Workstation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 货位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
    }

    /// <summary>
    /// 工作站配置 
    /// </summary>
    class WorkstationConfig : EntityConfig<Workstation>
    {
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="rules">验证规则规</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(Workstation.UserProperty, new RequiredRule());
            rules.Add(Workstation.StorageLocationProperty, new RequiredRule());
            rules.Add(Workstation.StorageAreaProperty, new RequiredRule());
            rules.Add(Workstation.WarehouseProperty, new RequiredRule());
        }
    }
}
