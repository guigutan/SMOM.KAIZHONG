using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// MOM相关接口日志
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InfDataLogCriteria))]
    [Label("MOM相关接口日志")]
    public class InfDataLog : DataEntity, IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InfDataLog()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="infType">接口名称</param>
        /// <param name="direction">执行方向</param>
        public InfDataLog(InfType infType, CallDirection direction)
        {
            this.BeginDate = DateTime.Now;
            this.InfType = infType;
            this.CallDirection = direction;
        }

        #region 开始时间 BeginDate

        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<InfDataLog>.Register(e => e.BeginDate);

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
        public static readonly Property<DateTime?> EndDateProperty = P<InfDataLog>.Register(e => e.EndDate);

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
        public static readonly Property<InfType?> InfTypeProperty = P<InfDataLog>.Register(e => e.InfType);

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
        public static readonly Property<CallDirection> CallDirectionProperty = P<InfDataLog>.Register(e => e.CallDirection);

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
        public static readonly Property<CallResult> CallResultProperty = P<InfDataLog>.Register(e => e.CallResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public CallResult CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }

        #endregion 执行结果 CallResult

        #region 请求内容 RequestContent

        /// <summary>
        /// 请求内容
        /// </summary>
        [Label("请求内容")]
        public static readonly Property<string> RequestContentProperty = P<InfDataLog>.Register(e => e.RequestContent);

        /// <summary>
        /// 请求内容
        /// </summary>
        public string RequestContent
        {
            get { return this.GetProperty(RequestContentProperty); }
            set { this.SetProperty(RequestContentProperty, value); }
        }

        #endregion 请求内容 RequestContent

        #region 响应内容 ResponseContent

        /// <summary>
        /// 响应内容
        /// </summary>
        [Label("响应内容")]
        public static readonly Property<string> ResponseContentProperty = P<InfDataLog>.Register(e => e.ResponseContent);

        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseContent
        {
            get { return this.GetProperty(ResponseContentProperty); }
            set { this.SetProperty(ResponseContentProperty, value); }
        }

        #endregion 响应内容 ResponseContent

        #region 数据量 Qty

        /// <summary>
        /// 数据量
        /// </summary>
        [Label("数据量")]
        public static readonly Property<int> QtyProperty = P<InfDataLog>.Register(e => e.Qty);

        /// <summary>
        /// 数据量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }

        #endregion 数据量 Qty

        #region 备注 Remark

        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<InfDataLog>.Register(e => e.Remark);

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
        public static readonly Property<string> ErrorMsgProperty = P<InfDataLog>.Register(e => e.ErrorMsg);

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMsg
        {
            get { return GetProperty(ErrorMsgProperty); }
            set { SetProperty(ErrorMsgProperty, value); }
        }

        #endregion 异常信息 ErrorMsg

        #region 其它信息 TipMsg

        /// <summary>
        /// 其它信息
        /// </summary>
        [Label("其它信息")]
        public static readonly Property<string> TipMsgProperty = P<InfDataLog>.Register(e => e.TipMsg);

        /// <summary>
        /// 其它信息
        /// </summary>
        public string TipMsg
        {
            get { return GetProperty(TipMsgProperty); }
            set { SetProperty(TipMsgProperty, value); }
        }

        #endregion 其它信息 TipMsg

        #region 重传次数 ReLoadCount
        ///// <summary>
        ///// 重传次数
        ///// </summary>
        //[Label("重传次数")]
        //public static readonly Property<int?> ReLoadCountProperty = P<InfDataLog>.Register(e => e.ReLoadCount);

        ///// <summary>
        ///// 重传次数
        ///// </summary>
        //public int? ReLoadCount
        //{
        //    get { return this.GetProperty(ReLoadCountProperty); }
        //    set { this.SetProperty(ReLoadCountProperty, value); }
        //}
        #endregion


        public void Dispose()
        {
            if (!this.EndDate.HasValue)
                this.EndDate = DateTime.Now;
            try
            {
                if (this.InfType.HasValue)
                    RF.Save(this);
            }
            catch { }
        }
    }

    /// <summary>
    /// MOM相关接口日志 实体配置
    /// </summary>
    internal class InfDataLogConfig : EntityConfig<InfDataLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("Inf_Erp_Log").MapAllProperties();
            Meta.Property(InfDataLog.RequestContentProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfDataLog.ResponseContentProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfDataLog.RemarkProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfDataLog.ErrorMsgProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfDataLog.TipMsgProperty).ColumnMeta.HasLength("MAX");

            Meta.Property(InfDataLog.CreateDateProperty).ColumnMeta.HasIndex();
            Meta.DisablePhantoms();
        }
    }
}
