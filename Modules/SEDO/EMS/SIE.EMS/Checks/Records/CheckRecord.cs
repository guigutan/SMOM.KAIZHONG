using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Records
{
    /// <summary>
    /// 设备点检记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CheckRecordCriteria))]
    [Label("设备点检记录")]
    public class CheckRecord : CheckPlan
    {
        #region 视图属性
        #region 执行状态名称 ExeStateName
        /// <summary>
        /// 执行状态名称
        /// </summary>
        [Label("执行状态名称")]
        public static readonly Property<string> ExeStateNameProperty = P<CheckRecord>.RegisterView(e => e.ExeStateName, p => p.ExeState.ToLabel());

        /// <summary>
        /// 执行状态名称
        /// </summary>
        public string ExeStateName
        {
            get { return GetProperty(ExeStateNameProperty); }
            set { SetProperty(ExeStateNameProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class CheckPlanLogConfig : EntityConfig<CheckRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CHECK_PLAN").MapAllProperties();
            Meta.Property(CheckRecord.LastCheckSummaryProperty).DontMapColumn();            
            Meta.Property(CheckRecord.IsAbnormalInfoPushProperty).DontMapColumn();
            Meta.Property(CheckRecord.ConfirmDeptIdProperty).DontMapColumn();
            Meta.Property(CheckRecord.ConfirmDeptDisplayProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
