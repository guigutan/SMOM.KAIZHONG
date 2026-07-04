using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// MOM与其它系统接口日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("MOM与其它系统接口日志查询实体")]
    public class InfDataLogCriteria : Criteria
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public InfDataLogCriteria()
        {
            BeginDate = new DateRange() { DateRangeType = DateRangeType.Today };
        }

        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateRange> BeginDateProperty = P<InfDataLogCriteria>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateRange BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 接口名 InfType
        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<InfType?> InfTypeProperty = P<InfDataLogCriteria>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType? InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }
        #endregion

        #region 接口方向 CallDirection
        /// <summary>
        /// 接口方向
        /// </summary>
        [Label("任务方向")]
        public static readonly Property<CallDirection?> CallDirectionProperty = P<InfDataLogCriteria>.Register(e => e.CallDirection);

        /// <summary>
        /// 任务方向
        /// </summary>
        public CallDirection? CallDirection
        {
            get { return GetProperty(CallDirectionProperty); }
            set { SetProperty(CallDirectionProperty, value); }
        }
        #endregion

        #region 执行结果 CallResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<CallResult?> CallResultProperty = P<InfDataLogCriteria>.Register(e => e.CallResult);

        /// <summary>
        /// 任务方向
        /// </summary>
        public CallResult? CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<InfDataLogCriteria>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 异常信息 ErrorMsg
        /// <summary>
        /// 异常信息
        /// </summary>
        [Label("异常信息")]
        public static readonly Property<string> ErrorMsgProperty = P<InfDataLogCriteria>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }
        #endregion

        #region 其它信息 TipMsg
        /// <summary>
        /// 其它信息
        /// </summary>
        [Label("其它信息")]
        public static readonly Property<string> TipMsgProperty = P<InfDataLogCriteria>.Register(e => e.TipMsg);

        /// <summary>
        /// 其它信息
        /// </summary>
        public string TipMsg
        {
            get { return GetProperty(TipMsgProperty); }
            set { SetProperty(TipMsgProperty, value); }
        }
        #endregion

        #region 请求内容 RequestContent
        /// <summary>
        /// 请求内容
        /// </summary>
        [Label("请求内容")]
        public static readonly Property<string> RequestContentProperty = P<InfDataLogCriteria>.Register(e => e.RequestContent);

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
        public static readonly Property<string> ResponseContentProperty = P<InfDataLogCriteria>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InfDataLogController>().GetInfDataLog(this);
        }

    }
}
