using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 报工记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("报工记录")]
    public class OutsourcingReportLog : DataEntity
    {
        #region 需求单 OutsourcingRequest
        /// <summary>
        /// 需求单Id
        /// </summary>
        [Label("需求单")]
        public static readonly IRefIdProperty OutsourcingRequestIdProperty = P<OutsourcingReportLog>.RegisterRefId(e => e.OutsourcingRequestId, ReferenceType.Parent);

        /// <summary>
        /// 需求单Id
        /// </summary>
        public double OutsourcingRequestId
        {
            get { return (double)GetRefId(OutsourcingRequestIdProperty); }
            set { SetRefId(OutsourcingRequestIdProperty, value); }
        }

        /// <summary>
        /// 需求单
        /// </summary>
        public static readonly RefEntityProperty<OutsourcingRequest> OutsourcingRequestProperty = P<OutsourcingReportLog>.RegisterRef(e => e.OutsourcingRequest, OutsourcingRequestIdProperty);

        /// <summary>
        /// 需求单
        /// </summary>
        public OutsourcingRequest OutsourcingRequest
        {
            get { return GetRefEntity(OutsourcingRequestProperty); }
            set { SetRefEntity(OutsourcingRequestProperty, value); }
        }
        #endregion

        #region 产品条码 SN
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SNProperty = P<OutsourcingReportLog>.Register(e => e.SN);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SN
        {
            get { return GetProperty(SNProperty); }
            set { SetProperty(SNProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<OutsourcingReportLog>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<OutsourcingReportLog>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 合格数 PassQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> PassQtyProperty = P<OutsourcingReportLog>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数 NgQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> NgQtyProperty = P<OutsourcingReportLog>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OutsourcingDetailState> StateProperty = P<OutsourcingReportLog>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public OutsourcingDetailState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否上传 IsUpload
        /// <summary>
        /// 是否上传
        /// </summary>
        [Label("是否上传事务交易")]
        public static readonly Property<bool?> IsUploadProperty = P<OutsourcingReportLog>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否上传
        /// </summary>
        public bool? IsUpload
        {
            get { return this.GetProperty(IsUploadProperty); }
            set { this.SetProperty(IsUploadProperty, value); }
        }
        #endregion

        #region 类型 ProcessingType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ProcessingType> ProcessingTypeProperty = P<OutsourcingReportLog>.Register(e => e.ProcessingType);

        /// <summary>
        /// 类型
        /// </summary>
        public ProcessingType ProcessingType
        {
            get { return this.GetProperty(ProcessingTypeProperty); }
            set { this.SetProperty(ProcessingTypeProperty, value); }
        }
        #endregion

        #region 重传次数 ReLoadCount
        /// <summary>
        /// 重传次数
        /// </summary>
        [Label("重传次数")]
        public static readonly Property<int?> ReLoadCountProperty = P<OutsourcingReportLog>.Register(e => e.ReLoadCount);

        /// <summary>
        /// 重传次数
        /// </summary>
        public int? ReLoadCount
        {
            get { return this.GetProperty(ReLoadCountProperty); }
            set { this.SetProperty(ReLoadCountProperty, value); }
        }
        #endregion

        #region 报工工厂 ReportFactory
        /// <summary>
        /// 报工工厂
        /// </summary>
        [Label("报工工厂")]
        public static readonly Property<string> ReportFactoryProperty = P<OutsourcingReportLog>.Register(e => e.ReportFactory);

        /// <summary>
        /// 报工工厂
        /// </summary>
        public string ReportFactory
        {
            get { return this.GetProperty(ReportFactoryProperty); }
            set { this.SetProperty(ReportFactoryProperty, value); }
        }
        #endregion


        #region 原来Id OldId
        /// <summary>
        /// 原来Id
        /// </summary>
        [Label("原来Id")]
        public static readonly Property<double?> OldIdProperty = P<OutsourcingReportLog>.Register(e => e.OldId);

        /// <summary>
        /// 原来Id
        /// </summary>
        public double? OldId
        {
            get { return this.GetProperty(OldIdProperty); }
            set { this.SetProperty(OldIdProperty, value); }
        }
        #endregion
    }

    internal class OutsourcingReportLogConfig : EntityConfig<OutsourcingReportLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("OUT_REQUEST_REPORT_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
