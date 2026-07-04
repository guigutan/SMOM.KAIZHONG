using SIE.Domain;
using SIE.EAP.Common.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EAP.Common.Logs
{
    /// <summary>
    /// EAP接口日志（含立库）
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("EAP接口日志（含立库）")]
    public class EAPInterfaceLog : DataEntity
    {
        #region 描述 Desc
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescProperty = P<EAPInterfaceLog>.Register(e => e.Desc);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion

        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<EAPInterfaceLog>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<EAPInterfaceLog>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EAPInterfaceLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 任务方向 JobDirection
        /// <summary>
        /// 任务方向
        /// </summary>
        [Label("任务方向")]
        public static readonly Property<JobDirection> JobDirectionProperty = P<EAPInterfaceLog>.Register(e => e.JobDirection);

        /// <summary>
        /// 任务方向
        /// </summary>
        public JobDirection JobDirection
        {
            get { return GetProperty(JobDirectionProperty); }
            set { SetProperty(JobDirectionProperty, value); }
        }
        #endregion

        #region 请求内容 RequestContent
        /// <summary>
        /// 请求内容
        /// </summary>
        [Label("请求内容")]
        public static readonly Property<string> RequestContentProperty = P<EAPInterfaceLog>.Register(e => e.RequestContent);

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestContent
        {
            get { return this.GetProperty(RequestContentProperty); }
            set { this.SetProperty(RequestContentProperty, value); }
        }
        #endregion

        #region 响应内容 ResponseContent
        /// <summary>
        /// 响应内容
        /// </summary>
        [Label("响应内容")]
        public static readonly Property<string> ResponseContentProperty = P<EAPInterfaceLog>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class EAPInterfaceLogConfig : EntityConfig<EAPInterfaceLog>
    {

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EAP_INF_LOG").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(EAPInterfaceLog.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EAPInterfaceLog.RequestContentProperty).ColumnMeta.HasLength("Max");
            Meta.Property(EAPInterfaceLog.ResponseContentProperty).ColumnMeta.HasLength("Max");
        }
    }
}
