using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("推送升级机制")]
    public partial class PushUpgradeRule : DataEntity
    {
        #region 时间 Time
        /// <summary>
        /// 时间
        /// </summary>
        [Label("时间")]
        public static readonly Property<double> TimeProperty = P<PushUpgradeRule>.Register(e => e.Time);

        /// <summary>
        /// 时间
        /// </summary>
        public double Time
        {
            get { return GetProperty(TimeProperty); }
            set { SetProperty(TimeProperty, value); }
        }
        #endregion

        #region 单位 UnitType
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<UnitType> UnitTypeProperty = P<PushUpgradeRule>.Register(e => e.UnitType);

        /// <summary>
        /// 单位
        /// </summary>
        public UnitType UnitType
        {
            get { return GetProperty(UnitTypeProperty); }
            set { SetProperty(UnitTypeProperty, value); }
        }
        #endregion

        #region 节点 AbnormalNode
        /// <summary>
        /// 节点
        /// </summary>
        [Label("节点")]
        public static readonly Property<TaskStateEnum> AbnormalNodeProperty = P<PushUpgradeRule>.Register(e => e.AbnormalNode);

        /// <summary>
        /// 节点
        /// </summary>
        public TaskStateEnum AbnormalNode
        {
            get { return GetProperty(AbnormalNodeProperty); }
            set { SetProperty(AbnormalNodeProperty, value); }
        }
        #endregion

        #region 推送对象 TargetList
        /// <summary>
        /// 推送对象
        /// </summary>
        public static readonly ListProperty<EntityList<PushTarget>> TargetListProperty = P<PushUpgradeRule>.RegisterList(e => e.TargetList);
        /// <summary>
        /// 推送对象
        /// </summary>
        public EntityList<PushTarget> TargetList
        {
            get { return this.GetLazyList(TargetListProperty); }
        }
        #endregion

        #region 推送方式 Pusher
        /// <summary>
        /// 推送方式Id
        /// </summary>
        public static readonly IRefIdProperty PusherIdProperty = P<PushUpgradeRule>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

        /// <summary>
        /// 推送方式Id
        /// </summary>
        public double PusherId
        {
            get { return (double)GetRefId(PusherIdProperty); }
            set { SetRefId(PusherIdProperty, value); }
        }

        /// <summary>
        /// 推送方式
        /// </summary>
        public static readonly RefEntityProperty<Pusher> PusherProperty = P<PushUpgradeRule>.RegisterRef(e => e.Pusher, PusherIdProperty);

        /// <summary>
        /// 推送方式
        /// </summary>
        public Pusher Pusher
        {
            get { return GetRefEntity(PusherProperty); }
            set { SetRefEntity(PusherProperty, value); }
        }
        #endregion

        #region 异常预警定义 AbnormalWarnDefine
        /// <summary>
        /// 推送升级机制Id
        /// </summary>
        public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<PushUpgradeRule>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Parent);

        /// <summary>
        /// 推送升级机制Id
        /// </summary>
        public double AbnormalWarnDefineId
        {
            get { return (double)GetRefId(AbnormalWarnDefineIdProperty); }
            set { SetRefId(AbnormalWarnDefineIdProperty, value); }
        }

        /// <summary>
        /// 推送升级机制
        /// </summary>
        public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<PushUpgradeRule>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

        /// <summary>
        /// 推送升级机制
        /// </summary>
        public AbnormalWarnDefine AbnormalWarnDefine
        {
            get { return GetRefEntity(AbnormalWarnDefineProperty); }
            set { SetRefEntity(AbnormalWarnDefineProperty, value); }
        }
        #endregion

        #region 只读属性

        #region 间隔时间(分钟) IntervalTime
        /// <summary>
        /// 间隔时间(分钟)
        /// </summary>
        [Label("间隔时间(分钟)")]
        public static readonly Property<double> IntervalTimeProperty = P<PushUpgradeRule>.RegisterReadOnly(
            e => e.IntervalTime, e => e.GetIntervalTime(), UnitTypeProperty, TimeProperty);
        /// <summary>
        /// 间隔时间(分钟)
        /// </summary>

        public double IntervalTime
        {
            get { return this.GetProperty(IntervalTimeProperty); }
        }
        private double GetIntervalTime()
        {
            double intervalTime = 0;
            switch (UnitType)
            {
                case UnitType.Days:
                    intervalTime = Time * 24 * 60;
                    break;
                case UnitType.Hours:
                    intervalTime = Time * 60;
                    break;
                case UnitType.Minute:
                    intervalTime = Time * 1;
                    break;
            }
            return intervalTime;
        }
        #endregion

        #endregion

        #region 不映射数据库







        #region 距离上次处理的间隔时间 IntervalDateTimeForLastProcess
        /// <summary>
        /// 距离上次处理的间隔时间
        /// </summary>

        public static readonly Property<DateTime?> IntervalDateTimeForLastProcessProperty = P<PushUpgradeRule>.Register(e => e.IntervalDateTimeForLastProcess);

        /// <summary>
        /// 距离上次处理的间隔时间
        /// </summary>
        public DateTime? IntervalDateTimeForLastProcess
        {
            get { return GetProperty(IntervalDateTimeForLastProcessProperty); }
            set { SetProperty(IntervalDateTimeForLastProcessProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class PushUpgradeRuleConfig : EntityConfig<PushUpgradeRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ABNORMAL_UPGRADE_RULE").MapAllPropertiesExcept(PushUpgradeRule.IntervalDateTimeForLastProcessProperty);
            Meta.EnablePhantoms();
        }
    }
}