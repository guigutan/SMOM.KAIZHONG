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
    /// 总控与SAP接口日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("总控与SAP接口日志查询实体")]
    public class InfNcDataLogGroupCriteria : Criteria
    {
        #region 批次号(Guid) BatchNo
        /// <summary>
        /// 批次号(Guid)
        /// </summary>
        [Label("批次号(Guid)")]
        public static readonly Property<string> BatchNoProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号(Guid)
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 接口名 InfType

        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<InfType?> InfTypeProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType? InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }

        #endregion 接口名 InfType

        #region 工厂组织 InvOrg
        /// <summary>
        /// 工厂组织
        /// </summary>
        [Label("工厂组织")]
        public static readonly Property<string> InvOrgProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.InvOrg);

        /// <summary>
        /// 工厂组织
        /// </summary>
        public string InvOrg
        {
            get { return this.GetProperty(InvOrgProperty); }
            set { this.SetProperty(InvOrgProperty, value); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 执行结果 CallResult

        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<CallResult?> CallResultProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.CallResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public CallResult? CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }

        #endregion 执行结果 CallResult

        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateRange> BeginDateProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateRange BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }

        #endregion 开始时间 BeginDate

        #region 推送状态 SendState
        /// <summary>
        /// 推送状态
        /// </summary>
        [Label("推送状态")]
        public static readonly Property<SendState?> SendStateProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.SendState);

        /// <summary>
        /// 推送状态
        /// </summary>
        public SendState? SendState
        {
            get { return this.GetProperty(SendStateProperty); }
            set { this.SetProperty(SendStateProperty, value); }
        }
        #endregion

        #region 请求数据 DataJsons
        /// <summary>
        /// 请求数据
        /// </summary>
        [Label("请求数据")]
        public static readonly Property<string> DataJsonsProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.DataJsons);

        /// <summary>
        /// 请求数据
        /// </summary>
        public string DataJsons
        {
            get { return this.GetProperty(DataJsonsProperty); }
            set { this.SetProperty(DataJsonsProperty, value); }
        }
        #endregion 主数据 DataJsons 

        #region 成功数据 SuccessJson
        /// <summary>
        /// 成功数据
        /// </summary>
        [Label("成功数据")]
        public static readonly Property<string> SuccessJsonProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.SuccessJson);

        /// <summary>
        /// 成功数据
        /// </summary>
        public string SuccessJson
        {
            get { return this.GetProperty(SuccessJsonProperty); }
            set { this.SetProperty(SuccessJsonProperty, value); }
        }
        #endregion

        #region 响应信息 ResponseContent
        /// <summary>
        /// 响应信息
        /// </summary>
        [Label("响应信息")]
        public static readonly Property<string> ResponseContentProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应信息
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }
        #endregion

        #region 异常信息 ErrorMsg

        /// <summary>
        /// 异常信息
        /// </summary>
        [Label("异常信息")]
        public static readonly Property<string> ErrorMsgProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        #region 工厂错误信息 FactoryErrorMsg
        /// <summary>
        /// 工厂错误信息
        /// </summary>
        [Label("工厂错误信息")]
        public static readonly Property<string> FactoryErrorMsgProperty = P<InfNcDataLogGroupCriteria>.Register(e => e.FactoryErrorMsg);

        /// <summary>
        /// 工厂错误信息
        /// </summary>
        public string FactoryErrorMsg
        {
            get { return this.GetProperty(FactoryErrorMsgProperty); }
            set { this.SetProperty(FactoryErrorMsgProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<LogGroupController>().CriteriaInfNcDataLogGroup(this);
        }
    }
}
