using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// SAP工单接口查询与重传
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InfNcDataLogSOCriteria))]
    [Label("SAP工单接口查询与重传")]
    public class InfNcDataLogSO : DataEntity
    {
        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<InfNcDataLogSO>.Register(e => e.BeginDate);

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int> InvOrgIdProperty = P<InfNcDataLogSO>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }

        #endregion 开始时间 BeginDate

        #region 结束时间 EndDate

        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndDateProperty = P<InfNcDataLogSO>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }

        #endregion 结束时间 EndDate

        #region 接口名 InfType

        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<InfType?> InfTypeProperty = P<InfNcDataLogSO>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType? InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }

        #endregion 接口名 InfType

        #region 接口方向 CallDirection

        /// <summary>
        /// 接口方向
        /// </summary>
        [Label("接口方向")]
        public static readonly Property<CallDirection> CallDirectionProperty = P<InfNcDataLogSO>.Register(e => e.CallDirection);

        /// <summary>
        /// 接口方向
        /// </summary>
        public CallDirection CallDirection
        {
            get { return GetProperty(CallDirectionProperty); }
            set { SetProperty(CallDirectionProperty, value); }
        }

        #endregion 接口方向 CallDirection

        #region 执行结果 CallResult

        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<CallResult> CallResultProperty = P<InfNcDataLogSO>.Register(e => e.CallResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public CallResult CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }

        #endregion 执行结果 CallResult

        #region 总控Guid GroupGuid
        /// <summary>
        /// 总控Guid
        /// </summary>
        [Label("总控Guid")]
        public static readonly Property<string> GroupGuidProperty = P<InfNcDataLogSO>.Register(e => e.GroupGuid);

        /// <summary>
        /// 总控Guid
        /// </summary>
        public string GroupGuid
        {
            get { return this.GetProperty(GroupGuidProperty); }
            set { this.SetProperty(GroupGuidProperty, value); }
        }
        #endregion


        #region 接口的四个参数
        #region 第三方系统编码 SystemCode
        /// <summary>
        /// 系统编码
        /// </summary>
        [Label("系统编码")]
        public static readonly Property<string> SystemCodeProperty = P<InfNcDataLogSO>.Register(e => e.SystemCode);

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode
        {
            get { return this.GetProperty(SystemCodeProperty); }
            set { this.SetProperty(SystemCodeProperty, value); }
        }
        #endregion 系统编码 SystemCode

        #region 主数据类型 InfCode
        /// <summary>
        /// 主数据类型
        /// </summary>
        [Label("主数据类型")]
        public static readonly Property<string> InfCodeProperty = P<InfNcDataLogSO>.Register(e => e.InfCode);

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
        public static readonly Property<string> OperationTypeProperty = P<InfNcDataLogSO>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion 操作类型 OperationType

        #region 主数据 DataJsons
        /// <summary>
        /// 主数据
        /// </summary>
        [Label("主数据")]
        public static readonly Property<string> DataJsonsProperty = P<InfNcDataLogSO>.Register(e => e.DataJsons);

        /// <summary>
        /// 主数据
        /// </summary>
        public string DataJsons
        {
            get { return this.GetProperty(DataJsonsProperty); }
            set { this.SetProperty(DataJsonsProperty, value); }
        }
        #endregion 主数据 DataJsons 
        #endregion

        #region 响应内容 ResponseContent
        /// <summary>
        /// 响应内容
        /// </summary>
        [Label("响应内容")]
        public static readonly Property<string> ResponseContentProperty = P<InfNcDataLogSO>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }

        #endregion 响应内容 ResponseContent


        #region 备注 Remark

        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<InfNcDataLogSO>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }

        #endregion 备注 Remark

        #region 异常信息 ErrorMsg

        /// <summary>
        /// 异常信息
        /// </summary>
        [Label("异常信息")]
        public static readonly Property<string> ErrorMsgProperty = P<InfNcDataLogSO>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        #region 工单 WO

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WOProperty = P<InfNcDataLogSO>.Register(e => e.WO);

        /// <summary>
        /// 工单
        /// </summary>
        public string WO
        {
            get { return GetProperty(WOProperty); }
            set { SetProperty(WOProperty, value); }
        }

        #endregion 工单 WO

    }

    /// <summary>
    /// 主数据NC接口日志 实体配置
    /// </summary>
    internal class InfNcDataLogSOConfig : EntityConfig<InfNcDataLogSO>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            // 映射到视图，而不是原始表
            Meta.MapView("V_INF_NC_LOG_WITH_WO").MapAllProperties();

            // 设置工单字段映射到视图的 WO_NUMBER 列
            Meta.Property(InfNcDataLogSO.WOProperty).ColumnMeta.ColumnName = "WO_NUMBER";
            Meta.Property(InfNcDataLogSO.DataJsonsProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogSO.ResponseContentProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogSO.RemarkProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogSO.ErrorMsgProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogSO.GroupGuidProperty).ColumnMeta.HasLength(2000);
            Meta.Property(InfNcDataLogSO.WOProperty).ColumnMeta.HasLength(100);
            
            Meta.DisablePhantoms();

                 // 设置只读，因为视图通常不可更新
            //Meta.SetReadOnly();
        }
    }
}