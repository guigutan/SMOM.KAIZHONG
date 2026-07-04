using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 工程节假日维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class HolidaySettingCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<HolidaySettingCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<HolidaySettingCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 开始日期 StartDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateRange> StartDateProperty = P<HolidaySettingCriteria>.Register(e => e.StartDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateRange StartDate
        {
            get { return GetProperty(StartDateProperty); }
            set { SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        public static readonly Property<DateRange> EndDateProperty = P<HolidaySettingCriteria>.Register(e => e.EndDate);

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateRange EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 备注 Remerk
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemerkProperty = P<HolidaySettingCriteria>.Register(e => e.Remerk);

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
            return RT.Service.Resolve<HolidaySettingController>().GetHolidaySettingList(this);
        }
    }
}
