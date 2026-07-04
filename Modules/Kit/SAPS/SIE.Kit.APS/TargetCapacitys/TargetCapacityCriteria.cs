using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("目标产能查询")]
    public class TargetCapacityCriteria : Criteria
    {

        #region 工厂 Enterprise
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<TargetCapacityCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<TargetCapacityCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Label("年份")]
        public static readonly Property<string> YearProperty = P<TargetCapacityCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public string Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<TargetCapacityController>().GetTargetCapacityList(this);
        }
    }
}
