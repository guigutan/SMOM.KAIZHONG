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
    /// 主数据NC接口日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("主数据NC接口日志查询实体")]
    public class InfNcDataLogCriteria : Criteria
    {
        #region 接口名 InfType

        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<InfType?> InfTypeProperty = P<InfNcDataLogCriteria>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType? InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }

        #endregion 接口名 InfType

        #region 主数据类型 InfCode
        /// <summary>
        /// 主数据类型
        /// </summary>
        [Label("主数据类型")]
        public static readonly Property<string> InfCodeProperty = P<InfNcDataLogCriteria>.Register(e => e.InfCode);

        /// <summary>
        /// 主数据类型
        /// </summary>
        public string InfCode
        {
            get { return this.GetProperty(InfCodeProperty); }
            set { this.SetProperty(InfCodeProperty, value); }
        }
        #endregion 主数据类型 InfCode

        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<string> OperationTypeProperty = P<InfNcDataLogCriteria>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion 操作类型 OperationType

        #region 执行结果 CallResult

        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<CallResult?> CallResultProperty = P<InfNcDataLogCriteria>.Register(e => e.CallResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public CallResult? CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }

        #endregion 执行结果 CallResult

        #region 主数据 DataJsons
        /// <summary>
        /// 主数据
        /// </summary>
        [Label("主数据")]
        public static readonly Property<string> DataJsonsProperty = P<InfNcDataLogCriteria>.Register(e => e.DataJsons);

        /// <summary>
        /// 主数据
        /// </summary>
        public string DataJsons
        {
            get { return this.GetProperty(DataJsonsProperty); }
            set { this.SetProperty(DataJsonsProperty, value); }
        }
        #endregion 主数据 DataJsons 

        #region 异常信息 ErrorMsg

        /// <summary>
        /// 异常信息
        /// </summary>
        [Label("异常信息")]
        public static readonly Property<string> ErrorMsgProperty = P<InfNcDataLogCriteria>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        #region 总控Guid GroupGuid
        /// <summary>
        /// 总控Guid
        /// </summary>
        [Label("总控Guid")]
        public static readonly Property<string> GroupGuidProperty = P<InfNcDataLogCriteria>.Register(e => e.GroupGuid);

        /// <summary>
        /// 总控Guid
        /// </summary>
        public string GroupGuid
        {
            get { return this.GetProperty(GroupGuidProperty); }
            set { this.SetProperty(GroupGuidProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InfNcDataLogController>().CriteriaInfNcDataLog(this);
        }
    }
}
