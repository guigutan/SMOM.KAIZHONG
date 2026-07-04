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
    /// 总控与工厂接口日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("总控与工厂接口日志查询实体")]
    public class InfNcDataLogFactoryCriteria : Criteria
    {
        #region 工厂组织 InvOrg
        /// <summary>
        /// 工厂组织
        /// </summary>
        [Label("工厂组织")]
        public static readonly Property<string> InvOrgProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.InvOrg);

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
        public static readonly Property<string> FactoryNameProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 批次号(Guid) BatchNo
        /// <summary>
        /// 批次号(Guid)
        /// </summary>
        [Label("批次号(Guid)")]
        public static readonly Property<string> BatchNoProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.BatchNo);

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
        public static readonly Property<InfType?> InfTypeProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType? InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }

        #endregion 接口名 InfType

        #region 请求数据 DataJsons
        /// <summary>
        /// 请求数据
        /// </summary>
        [Label("请求数据")]
        public static readonly Property<string> DataJsonsProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.DataJsons);

        /// <summary>
        /// 请求数据
        /// </summary>
        public string DataJsons
        {
            get { return this.GetProperty(DataJsonsProperty); }
            set { this.SetProperty(DataJsonsProperty, value); }
        }
        #endregion 主数据 DataJsons 

        #region 响应信息 ResponseContent
        /// <summary>
        /// 响应信息
        /// </summary>
        [Label("响应信息")]
        public static readonly Property<string> ResponseContentProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应信息
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }
        #endregion

        #region 推送状态 SendState
        /// <summary>
        /// 推送状态
        /// </summary>
        [Label("推送状态")]
        public static readonly Property<SendState?> SendStateProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.SendState);

        /// <summary>
        /// 推送状态
        /// </summary>
        public SendState? SendState
        {
            get { return this.GetProperty(SendStateProperty); }
            set { this.SetProperty(SendStateProperty, value); }
        }
        #endregion

        #region 总控Guid GroupGuid
        /// <summary>
        /// 总控Guid
        /// </summary>
        [Label("总控Guid")]
        public static readonly Property<string> GroupGuidProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.GroupGuid);

        /// <summary>
        /// 总控Guid
        /// </summary>
        public string GroupGuid
        {
            get { return this.GetProperty(GroupGuidProperty); }
            set { this.SetProperty(GroupGuidProperty, value); }
        }
        #endregion

        #region 异常信息 ErrorMsg

        /// <summary>
        /// 异常信息
        /// </summary>
        [Label("异常信息")]
        public static readonly Property<string> ErrorMsgProperty = P<InfNcDataLogFactoryCriteria>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InfNcDataLogFactoryController>().CriteriaInfNcDataLogFactory(this);
        }
    }
}
