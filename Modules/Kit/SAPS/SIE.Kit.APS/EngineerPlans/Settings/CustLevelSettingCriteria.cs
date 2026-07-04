using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 客户分类设置查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class CustLevelSettingCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<CustLevelSettingCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<CustLevelSettingCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty CustomerIdProperty = P<CustLevelSettingCriteria>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<CustLevelSettingCriteria>.RegisterRef(e => e.Customer, CustomerIdProperty);

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

        #region 客户优先级 CustLevel
        /// <summary>
        /// 客户优先级Id
        /// </summary>
        [Label("客户优先级")]
        public static readonly IRefIdProperty CustLevelIdProperty = P<CustLevelSettingCriteria>.RegisterRefId(e => e.CustLevelId, ReferenceType.Normal);

        /// <summary>
        /// 客户优先级Id
        /// </summary>
        [Label("客户优先级")]
        public double CustLevelId
        {
            get { return (double)GetRefId(CustLevelIdProperty); }
            set { SetRefId(CustLevelIdProperty, value); }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly RefEntityProperty<CustLevel> CustLevelProperty = P<CustLevelSettingCriteria>.RegisterRef(e => e.CustLevel, CustLevelIdProperty);

        /// <summary>
        /// 客户优先级
        /// </summary>
        [Label("优先级")]
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
        public static readonly Property<string> RemerkProperty = P<CustLevelSettingCriteria>.Register(e => e.Remerk);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remerk
        {
            get { return GetProperty(RemerkProperty); }
            set { SetProperty(RemerkProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>客户编码补偿天数模型列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CustLevelSettingController>().GetCustLevelSettingList(this);
        }
    }
}
