using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 设备报警记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipAlarmRecordCriteria))]
    [DisplayMember(nameof(Code))]
    [Label("设备报警记录")]
    public class EquipAlarmRecord : DataEntity
    {
        #region MDC传过来的Key MdcUid
        /// <summary>
        /// MDC传过来的Key
        /// </summary>
        [Label("MDC报警UID")]
        [MaxLength(500)]
        public static readonly Property<string> MdcUidProperty = P<EquipAlarmRecord>.Register(e => e.MdcUid);

        /// <summary>
        /// MDC传过来的Key
        /// </summary>
        public string MdcUid
        {
            get { return GetProperty(MdcUidProperty); }
            set { SetProperty(MdcUidProperty, value); }
        }
        #endregion

        #region 报警编码 Code
        /// <summary>
        /// 报警编码 EDO生成
        /// </summary>
        [Label("报警编码")]
        [MaxLength(500)]
        public static readonly Property<string> CodeProperty = P<EquipAlarmRecord>.Register(e => e.Code);

        /// <summary>
        /// 报警编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 报警ID AlarmId
        /// <summary>
        /// 报警ID  MDC传过来
        /// </summary>
        [Label("报警ID")]
        public static readonly Property<double?> AlarmIdProperty = P<EquipAlarmRecord>.Register(e => e.AlarmId);

        /// <summary>
        /// 报警ID
        /// </summary>
        public double? AlarmId
        {
            get { return GetProperty(AlarmIdProperty); }
            set { SetProperty(AlarmIdProperty, value); }
        }
        #endregion

        #region 报警路径  LinkAlarmPath
        /// <summary>
        /// 报警路径
        /// </summary>
        [Label("报警路径")]
        [MaxLength(500)]
        public static readonly Property<string> LinkAlarmPathProperty = P<EquipAlarmRecord>.Register(e => e.LinkAlarmPath);

        /// <summary>
        /// 报警路径
        /// </summary>
        public string LinkAlarmPath
        {
            get { return GetProperty(LinkAlarmPathProperty); }
            set { SetProperty(LinkAlarmPathProperty, value); }
        }
        #endregion

        #region 报警名称 AlarmName
        /// <summary>
        /// 报警名称
        /// </summary>
        [Label("报警名称")]
        [MaxLength(500)]
        public static readonly Property<string> AlarmNameProperty = P<EquipAlarmRecord>.Register(e => e.AlarmName);

        /// <summary>
        /// 报警名称
        /// </summary>
        public string AlarmName
        {
            get { return GetProperty(AlarmNameProperty); }
            set { SetProperty(AlarmNameProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Required]
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipAlarmRecord>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<EquipAlarmRecord>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region Tag全称  LinkTagFullName
        /// <summary>
        /// Tag全称
        /// </summary>
        [Label("MDC变量全称")]
        [MaxLength(500)]
        public static readonly Property<string> LinkTagFullNameProperty = P<EquipAlarmRecord>.Register(e => e.LinkTagFullName);

        /// <summary>
        /// Tag全称
        /// </summary>
        public string LinkTagFullName
        {
            get { return GetProperty(LinkTagFullNameProperty); }
            set { SetProperty(LinkTagFullNameProperty, value); }
        }
        #endregion

        #region Tag简称  TagName
        /// <summary>
        /// Tag简称
        /// </summary>
        [Label("MDC变量简称")]
        [MaxLength(500)]
        public static readonly Property<string> TagNameProperty = P<EquipAlarmRecord>.Register(e => e.TagName);

        /// <summary>
        /// Tag简称
        /// </summary>
        public string TagName
        {
            get { return GetProperty(TagNameProperty); }
            set { SetProperty(TagNameProperty, value); }
        }
        #endregion

        #region 报警原因  AlarmReason
        /// <summary>
        /// 报警原因
        /// </summary>
        [Label("报警原因")]
        [MaxLength(1000)]
        public static readonly Property<string> AlarmReasonProperty = P<EquipAlarmRecord>.Register(e => e.AlarmReason);

        /// <summary>
        /// 报警原因
        /// </summary>
        public string AlarmReason
        {
            get { return GetProperty(AlarmReasonProperty); }
            set { SetProperty(AlarmReasonProperty, value); }
        }
        #endregion

        #region 报警类型 AlarmType
        /// <summary>
        /// 报警类型
        /// </summary>
        [Required]
        [Label("报警类型")]
        public static readonly Property<string> AlarmTypeProperty = P<EquipAlarmRecord>.Register(e => e.AlarmType);

        /// <summary>
        /// 报警类型
        /// </summary>
        public string AlarmType
        {
            get { return this.GetProperty(AlarmTypeProperty); }
            set { this.SetProperty(AlarmTypeProperty, value); }
        }
        #endregion

        #region 报警主题  AlarmContent
        /// <summary>
        /// 报警主题
        /// </summary>
        [Label("报警主题")]
        [MaxLength(1000)]
        public static readonly Property<string> AlarmContentProperty = P<EquipAlarmRecord>.Register(e => e.AlarmContent);

        /// <summary>
        /// 报警主题
        /// </summary>
        public string AlarmContent
        {
            get { return GetProperty(AlarmContentProperty); }
            set { SetProperty(AlarmContentProperty, value); }
        }
        #endregion

        #region 报警描述  AlarmDescription
        /// <summary>
        /// 报警描述
        /// </summary>
        [Label("报警描述")]
        [MaxLength(1000)]
        public static readonly Property<string> AlarmDescriptionProperty = P<EquipAlarmRecord>.Register(e => e.AlarmDescription);

        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmDescription
        {
            get { return GetProperty(AlarmDescriptionProperty); }
            set { SetProperty(AlarmDescriptionProperty, value); }
        }
        #endregion

        #region 报警备注  AlarmRemark
        /// <summary>
        /// 报警备注
        /// </summary>
        [Label("报警备注")]
        [MaxLength(1000)]
        public static readonly Property<string> AlarmRemarkProperty = P<EquipAlarmRecord>.Register(e => e.AlarmRemark);

        /// <summary>
        /// 报警备注
        /// </summary>
        public string AlarmRemark
        {
            get { return GetProperty(AlarmRemarkProperty); }
            set { SetProperty(AlarmRemarkProperty, value); }
        }
        #endregion

        #region 报警级别 AlarmLevel
        /// <summary>
        /// 报警级别
        /// </summary>
        [Required]
        [Label("报警级别")]
        public static readonly Property<AlarmLevel> AlarmLevelProperty = P<EquipAlarmRecord>.Register(e => e.AlarmLevel);

        /// <summary>
        /// 报警级别
        /// </summary>
        public AlarmLevel AlarmLevel
        {
            get { return this.GetProperty(AlarmLevelProperty); }
            set { this.SetProperty(AlarmLevelProperty, value); }
        }
        #endregion

        #region 限制值  LimitValue
        /// <summary>
        /// 限制值
        /// </summary>
        [Required]
        [Label("限制值")]
        public static readonly Property<double> LimitValueProperty = P<EquipAlarmRecord>.Register(e => e.LimitValue);

        /// <summary>
        /// 限制值
        /// </summary>
        public double LimitValue
        {
            get { return GetProperty(LimitValueProperty); }
            set { SetProperty(LimitValueProperty, value); }
        }
        #endregion

        #region 报警状态 AlarmState
        /// <summary>
        /// 报警状态
        /// </summary>
        [Required]
        [Label("报警状态")]
        public static readonly Property<AlarmState> AlarmStateProperty = P<EquipAlarmRecord>.Register(e => e.AlarmState);

        /// <summary>
        /// 报警状态
        /// </summary>
        public AlarmState AlarmState
        {
            get { return this.GetProperty(AlarmStateProperty); }
            set { this.SetProperty(AlarmStateProperty, value); }
        }
        #endregion

        #region 报警时间  AlarmTime
        /// <summary>
        /// 报警时间
        /// </summary>
        [Required]
        [Label("报警时间")]
        public static readonly Property<DateTime> AlarmTimeProperty = P<EquipAlarmRecord>.Register(e => e.AlarmTime);

        /// <summary>
        /// 报警时间
        /// </summary>
        public DateTime AlarmTime
        {
            get { return GetProperty(AlarmTimeProperty); }
            set { SetProperty(AlarmTimeProperty, value); }
        }
        #endregion

        #region 关闭时间  CloseTime
        /// <summary>
        /// 关闭时间
        /// </summary>
        [Label("关闭时间")]
        public static readonly Property<DateTime?> CloseTimeProperty = P<EquipAlarmRecord>.Register(e => e.CloseTime);

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime
        {
            get { return GetProperty(CloseTimeProperty); }
            set { SetProperty(CloseTimeProperty, value); }
        }
        #endregion

        #region 报警持续时间  Duration
        /// <summary>
        /// 报警持续时间
        /// </summary>
        [Label("报警持续时间")]
        public static readonly Property<string> DurationProperty = P<EquipAlarmRecord>.Register(e => e.Duration);

        /// <summary>
        /// 报警持续时间
        /// </summary>
        public string Duration
        {
            get { return GetProperty(DurationProperty); }
            set { SetProperty(DurationProperty, value); }
        }
        #endregion

        #region TAG值清单 AlarmRecordValueList
        /// <summary>
        /// TAG值清单
        /// </summary>
        [Label("TAG值清单")]
        public static readonly ListProperty<EntityList<AlarmRecordValue>> AlarmRecordValueListProperty = P<EquipAlarmRecord>.RegisterList(e => e.AlarmRecordValueList);

        /// <summary>
        /// TAG值清单
        /// </summary>
        public EntityList<AlarmRecordValue> AlarmRecordValueList
        {
            get { return this.GetLazyList(AlarmRecordValueListProperty); }
        }
        #endregion

        #region 维修单 EquipRepairBill
        /// <summary>
        /// 维修单Id
        /// </summary>
        [Label("维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty = P<EquipAlarmRecord>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Normal);

        /// <summary>
        /// 维修单Id
        /// </summary>
        public double? EquipRepairBillId
        {
            get { return (double?)GetRefNullableId(EquipRepairBillIdProperty); }
            set { SetRefNullableId(EquipRepairBillIdProperty, value); }
        }

        /// <summary>
        /// 维修单
        /// </summary>
        public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty = P<EquipAlarmRecord>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return GetRefEntity(EquipRepairBillProperty); }
            set { SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 设备类型编码 EquipTypeeCode 
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipTypeeCodeProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipTypeeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeeCode
        {
            get { return this.GetProperty(EquipTypeeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 EquipTypeeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型名称")]
        public static readonly Property<string> EquipTypeeNameProperty = P<EquipAlarmRecord>.RegisterView(e => e.EquipTypeeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeeName
        {
            get { return this.GetProperty(EquipTypeeNameProperty); }
        }
        #endregion

        #region 维修状态 RepairState
        /// <summary>
        /// 维修状态
        /// </summary>
        [Label("维修状态")]
        public static readonly Property<EquipRepairState?> RepairStateProperty = P<EquipAlarmRecord>.RegisterView(e => e.RepairState, e => e.EquipRepairBill.RepairState);

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState? RepairState
        {
            get { return this.GetProperty(RepairStateProperty); }
            set { this.SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备报警记录 实体配置
    /// </summary>
    internal class EquipAlarmRecordConfig : EntityConfig<EquipAlarmRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ALARM_RECORD").MapAllProperties();
            Meta.Property(EquipAlarmRecord.MdcUidProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.CodeProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.AlarmNameProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.LinkAlarmPathProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.LinkTagFullNameProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.TagNameProperty).ColumnMeta.HasLength(500);
            Meta.Property(EquipAlarmRecord.AlarmReasonProperty).ColumnMeta.HasLength(1000);
            Meta.Property(EquipAlarmRecord.AlarmContentProperty).ColumnMeta.HasLength(1000);
            Meta.Property(EquipAlarmRecord.AlarmRemarkProperty).ColumnMeta.HasLength(1000);
            Meta.Property(EquipAlarmRecord.AlarmDescriptionProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<EquipAlarmRecord>();
                    if (para.AlarmTime == new DateTime(2000,1,1)) 
                    {
                        para.AlarmTime = DateTime.Now;
                    }
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}