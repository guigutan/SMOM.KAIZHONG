using SIE.Domain;
using SIE.MES.ItemChecker;
using SIE.MES.TaskManagement.IOT.Data;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.IOT
{
    /// <summary>
    /// IOT押出换轴记录实体查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("IOT押出换轴记录实体查询")]
    public class AxisChangeRecordCriterial:Criteria
    {
        public AxisChangeRecordCriterial() 
        {
            //CollectionTime.AllowDateRangeType = DateRangeType.All;
        }
        #region IOT实体 IotEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Label("IOT实体")]
        public static readonly Property<string> IotEntityProperty = P<AxisChangeRecordCriterial>.Register(e => e.IotEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string IotEntity
        {
            get { return this.GetProperty(IotEntityProperty); }
            set { this.SetProperty(IotEntityProperty, value); }
        }
        #endregion

        #region 换轴标识 ChangeFlag
        /// <summary>
        /// 换轴标识
        /// </summary>
        [Label("换轴标识")]
        public static readonly Property<bool?> ChangeFlagProperty = P<AxisChangeRecordCriterial>.Register(e => e.ChangeFlag);

        /// <summary>
        /// 换轴标识
        /// </summary>
        public bool? ChangeFlag
        {
            get { return this.GetProperty(ChangeFlagProperty); }
            set { this.SetProperty(ChangeFlagProperty, value); }
        }
        #endregion

        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<AxisChangeRecordCriterial>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 是否已处理 IsReport
        /// <summary>
        /// 是否已处理
        /// </summary>
        [Label("是否已处理")]
        public static readonly Property<bool?> IsReportProperty = P<AxisChangeRecordCriterial>.Register(e => e.IsReport);

        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool? IsReport
        {
            get { return this.GetProperty(IsReportProperty); }
            set { this.SetProperty(IsReportProperty, value); }
        }
        #endregion

        #region 推送时间 CollectionTime
        /// <summary>
        /// 推送时间
        /// </summary>
        [Label("推送时间")]
        public static readonly Property<DateRange?> CollectionTimeProperty = P<AxisChangeRecordCriterial>.Register(e => e.CollectionTime);

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateRange? CollectionTime
        {
            get { return this.GetProperty(CollectionTimeProperty); }
            set { this.SetProperty(CollectionTimeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<AxisChangeRecordCriterial>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<AxisChangeRecordCriterial>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AxisChangeRecordController>().GetAxisChangeRecords(this);
        }
    }
}
