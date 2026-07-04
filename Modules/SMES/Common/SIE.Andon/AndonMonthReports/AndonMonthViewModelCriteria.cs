using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Andon.AndonStatisticsReports;
using SIE.Common.Organizations;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.Andon.AndonMonthReports
{

    /// <summary>
    /// 安灯统计报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯月度报表查询实体")]
    public class AndonMonthViewModelCriteria : Criteria
    {
        #region 安灯大类 AndonClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Required]
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass?> AndonClassProperty = P<AndonMonthViewModelCriteria>.Register(e => e.AndonClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass? AndonClass
        {
            get { return this.GetProperty(AndonClassProperty); }
            set { this.SetProperty(AndonClassProperty, value); }
        }
        #endregion

        #region 安灯类型 AndonType
        /// <summary>
        /// 安灯类型Id
        /// </summary>
        [Label("安灯类型")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Normal);

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double? AndonTypeId
        {
            get { return (double?)this.GetRefNullableId(AndonTypeIdProperty); }
            set { this.SetRefNullableId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion


        #region 安灯名称 AndonName
        /// <summary>
        /// 安灯名称Id
        /// </summary>
        [Label("安灯名称")]
        public static readonly IRefIdProperty AndonNameIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.AndonNameId, ReferenceType.Normal);

        /// <summary>
        /// 安灯名称Id
        /// </summary>
        public double? AndonNameId
        {
            get { return (double?)this.GetRefNullableId(AndonNameIdProperty); }
            set { this.SetRefNullableId(AndonNameIdProperty, value); }
        }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public static readonly RefEntityProperty<Andons.Andon> AndonNameProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.AndonName, AndonNameIdProperty);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public Andons.Andon AndonName
        {
            get { return this.GetRefEntity(AndonNameProperty); }
            set { this.SetRefEntity(AndonNameProperty, value); }
        }

        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 负责部门 Department
        /// <summary>
        /// 负责部门Id
        /// </summary>
        [Label("负责部门")]
        public static readonly IRefIdProperty DepartmentIdProperty =
            P<AndonMonthViewModelCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 负责部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)this.GetRefNullableId(DepartmentIdProperty); }
            set { this.SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 负责部门
        /// </summary>
        public static readonly RefEntityProperty<Organization> DepartmentProperty =
            P<AndonMonthViewModelCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 负责部门
        /// </summary>
        public Organization Department
        {
            get { return this.GetRefEntity(DepartmentProperty); }
            set { this.SetRefEntity(DepartmentProperty, value); }
        }
        #endregion


        #region 分组层级 GroupLevel
        /// <summary>
        /// 分组层级
        /// </summary>
        [Label("分组层级")]
        public static readonly Property<GroupLevel> GroupLevelProperty = P<AndonMonthViewModelCriteria>.Register(e => e.GroupLevel);

        /// <summary>
        /// 分组层级
        /// </summary>
        public GroupLevel GroupLevel
        {
            get { return this.GetProperty(GroupLevelProperty); }
            set { this.SetProperty(GroupLevelProperty, value); }
        }
        #endregion

        #region 汇总维度 SummaryDimension
        /// <summary>
        /// 汇总维度
        /// </summary>
        [Label("汇总维度")]
        public static readonly Property<SummaryDimension> SummaryDimensionProperty = P<AndonMonthViewModelCriteria>.Register(e => e.SummaryDimension);

        /// <summary>
        /// 汇总维度
        /// </summary>
        public SummaryDimension SummaryDimension
        {
            get { return this.GetProperty(SummaryDimensionProperty); }
            set { this.SetProperty(SummaryDimensionProperty, value); }
        }
        #endregion

        #region 创建日期 CreateTime
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateTime> CreateTimeProperty = P<AndonMonthViewModelCriteria>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return base.Fetch();
        }
    }
}
