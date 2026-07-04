using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.AlertPlugs
{
    /// <summary>
    /// 设备维修单超时提醒
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备维修单超时提醒")]
    public class EmsRepairAlertPlugConfig : AlertConfig
    {
        #region 车间 Enterprise
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<EmsRepairAlertPlugConfig>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<EmsRepairAlertPlugConfig>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<EmsRepairAlertPlugConfig>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EmsRepairAlertPlugConfig>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 早班开始时间 EarlyStartDate
        /// <summary>
        /// 早班开始时间
        /// </summary>
        [Label("早班开始时间")]
        public static readonly Property<DateTime> EarlyStartDateProperty = P<EmsRepairAlertPlugConfig>.Register(e => e.EarlyStartDate, new PropertyMetadata<DateTime>
        {
            DateTimePart = ObjectModel.DateTimePart.Time
        });

        /// <summary>
        /// 早班开始时间
        /// </summary>
        public DateTime EarlyStartDate
        {
            get { return GetProperty(EarlyStartDateProperty); }
            set { SetProperty(EarlyStartDateProperty, value); }
        }
        #endregion

        #region 晚班开始时间 NightStartDate
        /// <summary>
        /// 晚班开始时间
        /// </summary>
        [Label("晚班开始时间")]
        public static readonly Property<DateTime> NightStartDateProperty = P<EmsRepairAlertPlugConfig>.Register(e => e.NightStartDate, new PropertyMetadata<DateTime>
        {
            DateTimePart = ObjectModel.DateTimePart.Time
        });
        /// <summary>
        /// 晚班开始时间
        /// </summary>
        public DateTime NightStartDate
        {
            get { return GetProperty(NightStartDateProperty); }
            set { SetProperty(NightStartDateProperty, value); }
        }
        #endregion

        #region 超时时间(分钟) TimeOut
        /// <summary>
        /// 超时时间(分钟)
        /// </summary>
        [Label("超时时间(分钟)")]
        public static readonly Property<int> TimeOutProperty = P<EmsRepairAlertPlugConfig>.Register(e => e.TimeOut);

        /// <summary>
        /// 超时时间(分钟)
        /// </summary>
        public int TimeOut
        {
            get { return GetProperty(TimeOutProperty); }
            set { SetProperty(TimeOutProperty, value); }
        }
        #endregion

        #region 备注 Note
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> NoteProperty = P<EmsRepairAlertPlugConfig>.Register(e => e.Note);

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(256)]
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">值</param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return;

            var config = JsonConvert.DeserializeObject<EmsRepairAlertPlugConfig>(value);
            this.ProcessId = config.ProcessId;
            this.EnterpriseId = config.EnterpriseId;
            this.EarlyStartDate = config.EarlyStartDate;
            this.NightStartDate = config.NightStartDate;
            this.TimeOut = config.TimeOut;
            this.Note = config.Note;
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var o = new
            {
                ProcessId = this.ProcessId,
                EnterpriseId = this.EnterpriseId,
                EarlyStartDate = this.EarlyStartDate,
                NightStartDate = this.NightStartDate,
                TimeOut = this.TimeOut,
                Note = this.Note,
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
