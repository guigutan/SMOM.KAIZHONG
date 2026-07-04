using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// KPI目标设定
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(QuotaTargetSettingCriteria))]
    [Label("KPI目标设定")]
    public class QuotaTargetSetting : DataEntity
    {
        #region 指标分类 Code
        /// <summary>
        /// 指标分类
        /// </summary>
        [Label("指标分类")]
        [Required]
        public static readonly Property<string> CodeProperty = P<QuotaTargetSetting>.Register(e => e.Code);

        /// <summary>
        /// 指标分类
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 指标名称 Name
        /// <summary>
        /// 指标名称
        /// </summary>
        [Label("指标名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<QuotaTargetSetting>.Register(e => e.Name);

        /// <summary>
        /// 指标名称
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
        public static readonly Property<DateType> DataTypeProperty = P<QuotaTargetSetting>.Register(e => e.DataType);

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
        public static readonly Property<KPIDimension?> DimensionProperty = P<QuotaTargetSetting>.Register(e => e.Dimension);

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
        public static readonly Property<EnterpriseType?> EntTypeProperty = P<QuotaTargetSetting>.Register(e => e.EntType);

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
        public static readonly IRefIdProperty EnterpriseIdProperty =
            P<QuotaTargetSetting>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty =
            P<QuotaTargetSetting>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 企业模型
        /// </summary>
        public Enterprise Enterprise
        {
            get { return this.GetRefEntity(EnterpriseProperty); }
            set { this.SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 目标值格式 ValueType
        /// <summary>
        /// 目标值格式
        /// </summary>
        [Label("目标值格式")]
        public static readonly Property<KPIValueType> ValueTypeProperty = P<QuotaTargetSetting>.Register(e => e.ValueType);

        /// <summary>
        /// 目标值格式
        /// </summary>
        public KPIValueType ValueType
        {
            get { return this.GetProperty(ValueTypeProperty); }
            set { this.SetProperty(ValueTypeProperty, value); }
        }
        #endregion

        #region 目标值设定列表 QuotaTargetDetailList
        /// <summary>
        /// 目标值设定列表
        /// </summary>
        public static readonly ListProperty<EntityList<QuotaTargetDetail>> QuotaTargetDetailListProperty = P<QuotaTargetSetting>.RegisterList(e => e.QuotaTargetDetailList);

        /// <summary>
        /// 目标值设定列表
        /// </summary>
        public EntityList<QuotaTargetDetail> QuotaTargetDetailList
        {
            get { return this.GetLazyList(QuotaTargetDetailListProperty); }
        }
        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (e.Property == EntTypeProperty)
                Enterprise = null;
            base.OnPropertyChanged(e);
        }
    }

    /// <summary>
    /// KPI目标设定 实体配置
    /// </summary>
    internal class QuotaTargetSettingConfig : EntityConfig<QuotaTargetSetting>
    {
        /// <summary>
        /// 数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QUOTA_TARGET_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
