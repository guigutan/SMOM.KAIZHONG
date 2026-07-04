using SIE.CSM.Customers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Kit.APS.EngineerPlans.Settings;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.EngineerPlan.Settings
{
    /// <summary>
    /// 客户等级设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CustLevelSettingCriteria))]
    [EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("客户等级设置")]
    //[DisplayMember(nameof(LevelName))]
    public partial class CustLevelSetting : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<CustLevelSetting>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id 
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<CustLevelSetting>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户编码")]
        public static readonly IRefIdProperty CustomerIdProperty = P<CustLevelSetting>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        [Required]
        [NotDuplicate]
        public double CustomerId
        {
            get { return (double)GetRefId(CustomerIdProperty); }
            set { SetRefId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<CustLevelSetting>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        [NotDuplicate]
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 客户等级 CustLevel
        /// <summary>
        /// 客户等级Id
        /// </summary>
        [Label("客户等级")]
        public static readonly IRefIdProperty CustLevelIdProperty = P<CustLevelSetting>.RegisterRefId(e => e.CustLevelId, ReferenceType.Normal);

        /// <summary>
        /// 客户等级Id
        /// </summary>
        [Label("客户等级")]
        public double CustLevelId
        {
            get { return (double)GetRefId(CustLevelIdProperty); }
            set { SetRefId(CustLevelIdProperty, value); }
        }

        /// <summary>
        /// 等级
        /// </summary>
        [Label("等级")]
        public static readonly RefEntityProperty<CustLevel> CustLevelProperty = P<CustLevelSetting>.RegisterRef(e => e.CustLevel, CustLevelIdProperty);

        /// <summary>
        /// 等级
        /// </summary>
        [Label("等级")]
        public CustLevel CustLevel
        {
            get { return GetRefEntity(CustLevelProperty); }
            set { SetRefEntity(CustLevelProperty, value); }
        }
        #endregion

        #region 备注 Remerk
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemerkProperty = P<CustLevelSetting>.Register(e => e.Remerk);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remerk
        {
            get { return GetProperty(RemerkProperty); }
            set { SetProperty(RemerkProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 时效性  ViewCustLevelHour
        /// <summary>
        /// 时效性
        /// </summary>
        [Label("时效性(H)")]
        public static readonly Property<int> ViewCustLevelHourProperty = P<CustLevelSetting>.RegisterView(e => e.ViewCustLevelHour, p => p.CustLevel.Hour);

        /// <summary>
        /// 时效性
        /// </summary>
        public int ViewCustLevelHour
        {
            get { return this.GetProperty(ViewCustLevelHourProperty); }
        }
        #endregion

        #region 客户名称  ViewCustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> ViewCustomerNameProperty = P<CustLevelSetting>.RegisterView(e => e.ViewCustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string ViewCustomerName
        {
            get { return this.GetProperty(ViewCustomerNameProperty); }
            set { this.SetProperty(ViewCustomerNameProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
	///  实体配置
	/// </summary>
	internal class CustLevelSettingEntityConfig : EntityConfig<CustLevelSetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_MSO_MI_PLAN_CLSet").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

