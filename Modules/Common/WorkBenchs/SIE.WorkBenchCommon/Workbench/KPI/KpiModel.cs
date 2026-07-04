using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 绩效目标
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("绩效目标"), DisplayMember(nameof(Name))]
    public partial class KpiModel : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<KpiModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 目标 Goal
        /// <summary>
        /// 目标
        /// </summary>
        [Label("目标")]
        public static readonly Property<decimal> GoalProperty = P<KpiModel>.Register(e => e.Goal);

        /// <summary>
        /// 目标
        /// </summary>
        public decimal Goal
        {
            get { return GetProperty(GoalProperty); }
            set { SetProperty(GoalProperty, value); }
        }
        #endregion

        #region 实际达成 Actual
        /// <summary>
        /// 实际达成
        /// </summary>
        [Label("实际达成")]
        public static readonly Property<decimal> ActualProperty = P<KpiModel>.Register(e => e.Actual);

        /// <summary>
        /// 实际达成
        /// </summary>
        public decimal Actual
        {
            get { return GetProperty(ActualProperty); }
            set { SetProperty(ActualProperty, value); }
        }
        #endregion

        #region 格式化 Format
        /// <summary>
        /// 格式化
        /// </summary>
        [Label("格式化")]
        public static readonly Property<string> FormatProperty = P<KpiModel>.Register(e => e.Format);

        /// <summary>
        /// 格式化
        /// </summary>
        public string Format
        {
            get { return GetProperty(FormatProperty); }
            set { SetProperty(FormatProperty, value); }
        }
        #endregion

        #region 绩效运算符 KpiOperators
        /// <summary>
        /// 绩效运算符
        /// </summary>
        [Label("绩效运算符")]
        public static readonly Property<KpiOperators> KpiOperatorsProperty = P<KpiModel>.Register(e => e.KpiOperators);

        /// <summary>
        /// 绩效运算符
        /// </summary>
        public KpiOperators KpiOperators
        {
            get { return GetProperty(KpiOperatorsProperty); }
            set { SetProperty(KpiOperatorsProperty, value); }
        }
        #endregion

        #region  KpiGrade
        /// <summary>
        /// 
        /// </summary>
        [Label("结果等级")]
        public static readonly Property<KpiGrade> KpiGradeProperty = P<KpiModel>.Register(e => e.KpiGrade);

        /// <summary>
        /// 
        /// </summary>
        public KpiGrade KpiGrade
        {
            get { return GetProperty(KpiGradeProperty); }
            set { SetProperty(KpiGradeProperty, value); }
        }
        #endregion

        #region 模块类型 ModuleCategory
        /// <summary>
        /// 任务类型分类
        /// </summary>
        [Label("模块类型")]
        public static readonly Property<ModuleCategory> ModuleCategoryProperty = P<KpiModel>.Register(e => e.ModuleCategory);

        /// <summary>
        /// 任务类型分类
        /// </summary>
        public ModuleCategory ModuleCategory
        {
            get { return GetProperty(ModuleCategoryProperty); }
            set { SetProperty(ModuleCategoryProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 绩效目标 实体配置
    /// </summary>
    internal class KpiModelConfig : EntityConfig<KpiModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_KPI").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}