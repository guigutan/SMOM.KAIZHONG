using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(TargetCapacityCriteria))]
   // [EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(EnterpriseId), false)]
    [Label("目标产能")]
    public partial class TargetCapacity : DataEntity
    {
        #region 工厂 Enterprise
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<TargetCapacity>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<TargetCapacity>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

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

        #region 工厂名称 EnterpriseName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<TargetCapacity>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
            set { SetProperty(EnterpriseNameProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Label("年份")]
        public static readonly Property<string> YearProperty = P<TargetCapacity>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public string Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 1月份 M1
        /// <summary>
        /// 1月份
        /// </summary>
        [Label("1月份")]
        public static readonly Property<decimal> M1Property = P<TargetCapacity>.Register(e => e.M1);

        /// <summary>
        /// 1月份
        /// </summary>
        public decimal M1
        {
            get { return GetProperty(M1Property); }
            set { SetProperty(M1Property, value); }
        }
        #endregion

        #region 2月份 M2
        /// <summary>
        /// 2月份
        /// </summary>
        [Label("2月份")]
        public static readonly Property<decimal> M2Property = P<TargetCapacity>.Register(e => e.M2);

        /// <summary>
        /// 2月份
        /// </summary>
        public decimal M2
        {
            get { return GetProperty(M2Property); }
            set { SetProperty(M2Property, value); }
        }
        #endregion

        #region 3月份 M3
        /// <summary>
        /// 3月份
        /// </summary>
        [Label("3月份")]
        public static readonly Property<decimal> M3Property = P<TargetCapacity>.Register(e => e.M3);

        /// <summary>
        /// 3月份
        /// </summary>
        public decimal M3
        {
            get { return GetProperty(M3Property); }
            set { SetProperty(M3Property, value); }
        }
        #endregion

        #region 4月份 M4
        /// <summary>
        /// 4月份
        /// </summary>
        [Label("4月份")]
        public static readonly Property<decimal> M4Property = P<TargetCapacity>.Register(e => e.M4);

        /// <summary>
        /// 4月份
        /// </summary>
        public decimal M4
        {
            get { return GetProperty(M4Property); }
            set { SetProperty(M4Property, value); }
        }
        #endregion

        #region 5月份 M5
        /// <summary>
        /// 5月份
        /// </summary>
        [Label("5月份")]
        public static readonly Property<decimal> M5Property = P<TargetCapacity>.Register(e => e.M5);

        /// <summary>
        /// 5月份
        /// </summary>
        public decimal M5
        {
            get { return GetProperty(M5Property); }
            set { SetProperty(M5Property, value); }
        }
        #endregion

        #region 6月份 M6
        /// <summary>
        /// 6月份
        /// </summary>
        [Label("6月份")]
        public static readonly Property<decimal> M6Property = P<TargetCapacity>.Register(e => e.M6);

        /// <summary>
        /// 6月份
        /// </summary>
        public decimal M6
        {
            get { return GetProperty(M6Property); }
            set { SetProperty(M6Property, value); }
        }
        #endregion

        #region 7月份 M7
        /// <summary>
        /// 7月份
        /// </summary>
        [Label("7月份")]
        public static readonly Property<decimal> M7Property = P<TargetCapacity>.Register(e => e.M7);

        /// <summary>
        /// 7月份
        /// </summary>
        public decimal M7
        {
            get { return GetProperty(M7Property); }
            set { SetProperty(M7Property, value); }
        }
        #endregion

        #region 8月份 M8
        /// <summary>
        /// 8月份
        /// </summary>
        [Label("8月份")]
        public static readonly Property<decimal> M8Property = P<TargetCapacity>.Register(e => e.M8);

        /// <summary>
        /// 8月份
        /// </summary>
        public decimal M8
        {
            get { return GetProperty(M8Property); }
            set { SetProperty(M8Property, value); }
        }
        #endregion

        #region 9月份 M9
        /// <summary>
        /// 9月份
        /// </summary>
        [Label("9月份")]
        public static readonly Property<decimal> M9Property = P<TargetCapacity>.Register(e => e.M9);

        /// <summary>
        /// 9月份
        /// </summary>
        public decimal M9
        {
            get { return GetProperty(M9Property); }
            set { SetProperty(M9Property, value); }
        }
        #endregion

        #region 10月份 M10
        /// <summary>
        /// 10月份
        /// </summary>
        [Label("10月份")]
        public static readonly Property<decimal> M10Property = P<TargetCapacity>.Register(e => e.M10);

        /// <summary>
        /// 10月份
        /// </summary>
        public decimal M10
        {
            get { return GetProperty(M10Property); }
            set { SetProperty(M10Property, value); }
        }
        #endregion

        #region 11月份 M11
        /// <summary>
        /// 11月份
        /// </summary>
        [Label("11月份")]
        public static readonly Property<decimal> M11Property = P<TargetCapacity>.Register(e => e.M11);

        /// <summary>
        /// 11月份
        /// </summary>
        public decimal M11
        {
            get { return GetProperty(M11Property); }
            set { SetProperty(M11Property, value); }
        }
        #endregion

        #region 12月份 M12
        /// <summary>
        /// 12月份
        /// </summary>
        [Label("12月份")]
        public static readonly Property<decimal> M12Property = P<TargetCapacity>.Register(e => e.M12);

        /// <summary>
        /// 12月份
        /// </summary>
        public decimal M12
        {
            get { return GetProperty(M12Property); }
            set { SetProperty(M12Property, value); }
        }
        #endregion
    }

    /// <summary>
    /// 目标产能 实体配置
    /// </summary>
    internal class TargetCapacityConfig : EntityConfig<TargetCapacity>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_TARGET_CAPACITY").MapAllProperties();
            Meta.Property(TargetCapacity.YearProperty).ColumnMeta.HasLength(100);
            Meta.EnablePhantoms();
        }
    }
}