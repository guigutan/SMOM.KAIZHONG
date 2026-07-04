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
    /// 总控与NC接口日志基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("总控与NC接口日志基类")]
    public partial class InfNcDataLogGroupBase : DataEntity
    {
        #region 工厂组织 InvOrg
        /// <summary>
        /// 工厂组织
        /// </summary>
        [Label("工厂组织")]
        public static readonly Property<string> InvOrgProperty = P<InfNcDataLogGroupBase>.Register(e => e.InvOrg);

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
        public static readonly Property<string> FactoryNameProperty = P<InfNcDataLogGroupBase>.Register(e => e.FactoryName);

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
        public static readonly Property<string> BatchNoProperty = P<InfNcDataLogGroupBase>.Register(e => e.BatchNo);

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
        public static readonly Property<InfType?> InfTypeProperty = P<InfNcDataLogGroupBase>.Register(e => e.InfType);

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
        public static readonly Property<string> DataJsonsProperty = P<InfNcDataLogGroupBase>.Register(e => e.DataJsons);

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
        public static readonly Property<string> ResponseContentProperty = P<InfNcDataLogGroupBase>.Register(e => e.ResponseContent);

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
        public static readonly Property<string> ErrorMsgProperty = P<InfNcDataLogGroupBase>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        #region 关键信息字段

        #region 关键信息1 KeyMsgone
        /// <summary>
        /// 关键信息1(默认编码)
        /// </summary>
        [Label("关键信息1(默认编码)")]
        public static readonly Property<string> KeyMsgoneProperty = P<InfNcDataLogGroupBase>.Register(e => e.KeyMsgone);

        /// <summary>
        /// 关键信息1
        /// </summary>
        public string KeyMsgone
        {
            get { return this.GetProperty(KeyMsgoneProperty); }
            set { this.SetProperty(KeyMsgoneProperty, value); }
        }
        #endregion

        #region 关键信息2 KeyMsgtwo
        /// <summary>
        /// 关键信息2
        /// </summary>
        [Label("关键信息2")]
        public static readonly Property<string> KeyMsgtwoProperty = P<InfNcDataLogGroupBase>.Register(e => e.KeyMsgtwo);

        /// <summary>
        /// 关键信息2
        /// </summary>
        public string KeyMsgtwo
        {
            get { return this.GetProperty(KeyMsgtwoProperty); }
            set { this.SetProperty(KeyMsgtwoProperty, value); }
        }
        #endregion

        #region 关键信息3 KeyMsgthree
        /// <summary>
        /// 关键信息3
        /// </summary>
        [Label("关键信息3")]
        public static readonly Property<string> KeyMsgthreeProperty = P<InfNcDataLogGroupBase>.Register(e => e.KeyMsgthree);

        /// <summary>
        /// 关键信息3
        /// </summary>
        public string KeyMsgthree
        {
            get { return this.GetProperty(KeyMsgthreeProperty); }
            set { this.SetProperty(KeyMsgthreeProperty, value); }
        }
        #endregion

        #region 关键信息4 KeyMsgfour
        /// <summary>
        /// 关键信息4
        /// </summary>
        [Label("关键信息4")]
        public static readonly Property<string> KeyMsgfourProperty = P<InfNcDataLogGroupBase>.Register(e => e.KeyMsgfour);

        /// <summary>
        /// 关键信息4
        /// </summary>
        public string KeyMsgfour
        {
            get { return this.GetProperty(KeyMsgfourProperty); }
            set { this.SetProperty(KeyMsgfourProperty, value); }
        }
        #endregion

        #region 关键信息5 KeyMsgfive
        /// <summary>
        /// 关键信息5
        /// </summary>
        [Label("关键信息5")]
        public static readonly Property<string> KeyMsgfiveProperty = P<InfNcDataLogGroupBase>.Register(e => e.KeyMsgfive);

        /// <summary>
        /// 关键信息5
        /// </summary>
        public string KeyMsgfive
        {
            get { return this.GetProperty(KeyMsgfiveProperty); }
            set { this.SetProperty(KeyMsgfiveProperty, value); }
        }
        #endregion


        #endregion

        #region 推送状态 SendState
        /// <summary>
        /// 推送状态
        /// </summary>
        [Label("推送状态")]
        public static readonly Property<SendState> SendStateProperty = P<InfNcDataLogGroupBase>.Register(e => e.SendState);

        /// <summary>
        /// 推送状态
        /// </summary>
        public SendState SendState
        {
            get { return this.GetProperty(SendStateProperty); }
            set { this.SetProperty(SendStateProperty, value); }
        }
        #endregion

        #region 第三方系统编码 NcSystemCode
        /// <summary>
        /// 第三方系统编码
        /// </summary>
        [Label("第三方系统编码")]
        public static readonly Property<string> NcSystemCodeProperty = P<InfNcDataLogGroupBase>.Register(e => e.NcSystemCode);

        /// <summary>
        /// 第三方系统编码
        /// </summary>
        public string NcSystemCode
        {
            get { return this.GetProperty(NcSystemCodeProperty); }
            set { this.SetProperty(NcSystemCodeProperty, value); }
        }
        #endregion

        #region 操作类型 NcOperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<string> NcOperationTypeProperty = P<InfNcDataLogGroupBase>.Register(e => e.NcOperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public string NcOperationType
        {
            get { return this.GetProperty(NcOperationTypeProperty); }
            set { this.SetProperty(NcOperationTypeProperty, value); }
        }
        #endregion

        #region 主数据类型 NcInfCode
        /// <summary>
        /// 主数据类型
        /// </summary>
        [Label("主数据类型")]
        public static readonly Property<string> NcInfCodeProperty = P<InfNcDataLogGroupBase>.Register(e => e.NcInfCode);

        /// <summary>
        /// 主数据类型
        /// </summary>
        public string NcInfCode
        {
            get { return this.GetProperty(NcInfCodeProperty); }
            set { this.SetProperty(NcInfCodeProperty, value); }
        }
        #endregion

    }

}
