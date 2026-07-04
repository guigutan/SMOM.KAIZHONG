using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标目标定义查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class QuotaTargetSettingCriteria : Criteria
    {
        #region 编码

        /// <summary>
        /// 编码
        /// </summary>
        public static readonly Property<string> CodeProperty = P<QuotaTargetSettingCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }

        #endregion

        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly Property<string> NameProperty = P<QuotaTargetSettingCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }

        #endregion

        #region 周期类型 DataType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<DateType> DataTypeProperty = P<QuotaTargetSettingCriteria>.Register(e => e.DataType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public DateType DataType
        {
            get { return GetProperty(DataTypeProperty); }
            set { SetProperty(DataTypeProperty, value); }
        }
        #endregion

        #region 指标维度 Dimension
        /// <summary>
        /// 指标维度
        /// </summary>
        [Label("指标维度")]
        [Required]
        public static readonly Property<KPIDimension?> DimensionProperty = P<QuotaTargetSettingCriteria>.Register(e => e.Dimension);

        /// <summary>
        /// 指标维度
        /// </summary>
        public KPIDimension? Dimension
        {
            get { return this.GetProperty(DimensionProperty); }
            set { this.SetProperty(DimensionProperty, value); }
        }
        #endregion

        #region 层级类型 EntType
        /// <summary>
        /// 层级类型
        /// </summary>
        [Label("层级类型")]
        public static readonly Property<EnterpriseType?> EntTypeProperty = P<QuotaTargetSettingCriteria>.Register(e => e.EntType);

        /// <summary>
        /// 层级类型
        /// </summary>
        public EnterpriseType? EntType
        {
            get { return this.GetProperty(EntTypeProperty); }
            set { this.SetProperty(EntTypeProperty, value); }
        }
        #endregion

        #region 企业模型 Enterprise
        /// <summary>
        /// 企业模型Id
        /// </summary>
        [Label("企业模型")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<QuotaTargetSettingCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 企业模型Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)this.GetRefNullableId(EnterpriseIdProperty); }
            set { this.SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 企业模型
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<QuotaTargetSettingCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 企业模型
        /// </summary>
        public Enterprise Enterprise
        {
            get { return this.GetRefEntity(EnterpriseProperty); }
            set { this.SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>返回查询数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<QuotaTargetSettingController>().GetQuota(this);
        }
    }
}
