using SIE.Core.Enums;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备台账查询实体")]
    public partial class EquipAccountSelectCriteria : Criteria
    {
        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<EquipAccountSelectCriteria>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<EquipAccountSelectCriteria>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        public static readonly IRefIdProperty WorkShopIdProperty = P<EquipAccountSelectCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<EquipAccountSelectCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty
            = P<EquipAccountSelectCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty
            = P<EquipAccountSelectCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<EquipAccountSelectCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EquipAccountSelectCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 设备类型 TypeCategory
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> TypeCategoryProperty = P<EquipAccountSelectCriteria>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string TypeCategory
        {
            get { return GetProperty(TypeCategoryProperty); }
            set { SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<EquipAccountSelectCriteria>.Register(e => e.ModelCode);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<EquipAccountSelectCriteria>.Register(e => e.ModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 设备状态 State
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<AccountState?> StateProperty = P<EquipAccountSelectCriteria>.Register(e => e.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 管理状态 AccountUseState
        /// <summary>
        /// 管理状态（原使用状态）
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> AccountUseStateProperty = P<EquipAccountSelectCriteria>.Register(e => e.AccountUseState);

        /// <summary>
        /// 使用状态
        /// </summary>
        public AccountUseState? AccountUseState
        {
            get { return GetProperty(AccountUseStateProperty); }
            set { SetProperty(AccountUseStateProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipAccountSelectCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 忽略权限过滤 IsLoadAll
        /// <summary>
        /// 忽略权限过滤
        /// </summary>
        [Label("忽略权限过滤")]
        public static readonly Property<bool> IsLoadAllProperty = P<EquipAccountSelectCriteria>.Register(e => e.IsLoadAll);

        /// <summary>
        /// 忽略权限过滤
        /// </summary>
        public bool IsLoadAll
        {
            get { return this.GetProperty(IsLoadAllProperty); }
            set { this.SetProperty(IsLoadAllProperty, value); }
        }
        #endregion

        #region 可用状态 EnableUseState
        /// <summary>
        /// 可用状态
        /// </summary>
        [Label("可用状态")]
        public static readonly Property<string> EnableUseStateProperty = P<EquipAccountSelectCriteria>.Register(e => e.EnableUseState);

        /// <summary>
        /// 可用状态
        /// </summary>
        public string EnableUseState
        {
            get { return this.GetProperty(EnableUseStateProperty); }
            set { this.SetProperty(EnableUseStateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipAccountSelectController>().GetEquipAccountsByCriteria(this);
        }
    }
}
