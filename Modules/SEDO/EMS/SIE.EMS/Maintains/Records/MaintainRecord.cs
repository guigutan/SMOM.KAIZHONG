using SIE.Domain;
using SIE.EMS.Maintains.Plans;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Maintains.Records
{
    /// <summary>
    /// 设备保养记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaintainRecordCriteria))]
    [Label("设备保养记录")]
    public class MaintainRecord : MaintainPlan
    {
        #region 视图属性       

        #region 执行状态名称 ExeStateName
        /// <summary>
        /// 执行状态名称
        /// </summary>
        [Label("执行状态名称")]
        public static readonly Property<string> ExeStateNameProperty = P<MaintainRecord>.RegisterView(e => e.ExeStateName, p => p.ExeState.ToLabel());

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
    internal class MaintainRecordConfig : EntityConfig<MaintainRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_PLAN").MapAllProperties();
            Meta.Property(MaintainRecord.ResourceIdProperty).DontMapColumn();
            Meta.Property(MaintainRecord.ResourceProperty).DontMapColumn();
            Meta.Property(MaintainRecord.SelectBeginTimeProperty).DontMapColumn();
            Meta.Property(MaintainRecord.SelectEndTimeProperty).DontMapColumn();
            Meta.Property(MaintainRecord.IsAbnormalInfoPushProperty).DontMapColumn();
            Meta.Property(MaintainRecord.ConfirmDeptIdProperty).DontMapColumn();
            Meta.Property(MaintainRecord.ConfirmDeptDisplayProperty).DontMapColumn();
            Meta.Property(MaintainRecord.MaintainCycleTypeProperty).DontMapColumn();
            Meta.Property(MaintainRecord.MaintainTypeInfoIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
