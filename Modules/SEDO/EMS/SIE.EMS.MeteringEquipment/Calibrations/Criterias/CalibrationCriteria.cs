using SIE.Common;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations.Criterias
{
    /// <summary>
    /// 检验规程查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class CalibrationCriteria : Criteria
    {
        #region 检验单号 InspectionNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> InspectionNoProperty = P<CalibrationCriteria>.Register(e => e.InspectionNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo
        {
            get { return GetProperty(InspectionNoProperty); }
            set { SetProperty(InspectionNoProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus?> InspectionStatusProperty = P<CalibrationCriteria>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus? InspectionStatus
        {
            get { return GetProperty(InspectionStatusProperty); }
            set { SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<CalibrationCriteria>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 检验机构 Agency
        /// <summary>
        /// 检验机构Id
        /// </summary>
        public static readonly IRefIdProperty AgencyIdProperty = P<CalibrationCriteria>.RegisterRefId(e => e.AgencyId, ReferenceType.Normal);

        /// <summary>
        /// 检验机构Id
        /// </summary>
        public double? AgencyId
        {
            get { return (double?)GetRefNullableId(AgencyIdProperty); }
            set { SetRefNullableId(AgencyIdProperty, value); }
        }

        /// <summary>
        /// 检验机构
        /// </summary>
        public static readonly RefEntityProperty<Supplier> AgencyProperty = P<CalibrationCriteria>.RegisterRef(e => e.Agency, AgencyIdProperty);

        /// <summary>
        /// 检验机构
        /// </summary>
        public Supplier Agency
        {
            get { return GetRefEntity(AgencyProperty); }
            set { SetRefEntity(AgencyProperty, value); }
        }
        #endregion

        #region 计划检验日期 PlanInspectionDate
        /// <summary>
        /// 计划检验日期
        /// </summary>
        [Label("计划检验日期")]
        public static readonly Property<DateRange> PlanInspectionDateProperty = P<CalibrationCriteria>.Register(e => e.PlanInspectionDate);

        /// <summary>
        /// 计划检验日期
        /// </summary>
        public DateRange PlanInspectionDate
        {
            get { return GetProperty(PlanInspectionDateProperty); }
            set { SetProperty(PlanInspectionDateProperty, value); }
        }
        #endregion

        #region 实际检验日期 ActualInspectionDate
        /// <summary>
        /// 实际检验日期
        /// </summary>
        [Label("实际检验日期")]
        public static readonly Property<DateRange> ActualInspectionDateProperty = P<CalibrationCriteria>.Register(e => e.ActualInspectionDate);

        /// <summary>
        /// 实际检验日期
        /// </summary>
        public DateRange ActualInspectionDate
        {
            get { return GetProperty(ActualInspectionDateProperty); }
            set { SetProperty(ActualInspectionDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>检验规程列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CalibrationController>().GetCalibrationList(this);
        }
    }
}
