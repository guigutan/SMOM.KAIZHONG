using SIE.Common;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SIE.EventMessages.Inspection
{
    /// <summary>
    /// 成品报检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultProductInsp))]
    public interface IProductInsp
    {
        /// <summary>
        /// 成品报检
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        void ProductInsp(ProductInspEvent inspEvent);

        /// <summary>
        /// 成品报检（任务单）
        /// </summary>
        /// <param name="inspEvent"></param>
        bool GenerateTaskProductInsp(List<ProductInspEvent> inspEvent);

        /// <summary>
        /// 根据成品检验单结果更新成品报检结果
        /// </summary>
        /// <param name="billEvents">成品检验提交后事件集合</param>
        void UpdateInspResult(List<ShippingBillSubmittedEvent> billEvents);

        /// <summary>
        /// 根据成品不合格审核更新成品报检审核结果
        /// </summary>
        /// <param name="billEvents">成品不合格审核提交后事件集合</param>
        void UpdateAuditResult(List<ShippingBillSubmittedEvent> billEvents);
    }

    /// <summary>
    /// 成品报检接口默认实现
    /// </summary>
    public class DefaultProductInsp : IProductInsp
    {
        /// <summary>
        /// 成品报检
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        public void ProductInsp(ProductInspEvent inspEvent)
        {
            // 
        }

        /// <summary>
        /// 成品报检（任务单）
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        public bool GenerateTaskProductInsp(List<ProductInspEvent> inspEvent)
        {
            return false;
        }

        /// <summary>
        /// 根据成品检验单结果更新成品报检结果
        /// </summary>
        /// <param name="billEvents">成品检验提交后事件集合</param>
        public void UpdateAuditResult(List<ShippingBillSubmittedEvent> billEvents)
        {
            // 
        }

        /// <summary>
        /// 根据成品不合格审核更新成品报检审核结果
        /// </summary>
        /// <param name="billEvents">成品不合格审核提交后事件集合</param>
        public void UpdateInspResult(List<ShippingBillSubmittedEvent> billEvents)
        {
            // 
        }
    }

    /// <summary>
    /// 成品报检创建成品检验单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultCreateShippingBill))]
    public interface ICreateShippingBill
    {
        /// <summary>
        /// 生成成品检验单，返回QMS检验单号
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        string GenerateShippingBill(InspBillEvent billEvent);
    }

    /// <summary>
    /// 成品报检创建成品检验单接口默认实现
    /// </summary>
    public class DefaultCreateShippingBill : ICreateShippingBill
    {
        /// <summary>
        /// 生成成品检验单
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        public string GenerateShippingBill(InspBillEvent billEvent)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 成品报检事件
    /// </summary>
    [Serializable]
    public class ProductInspEvent
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double ShopId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 报检条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 任务单ID
        /// </summary>
        public double? DispatchTaskId { get; set; }

        /// <summary>
        /// 报工记录ID
        /// </summary>
        public double? ReportRecordId { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionDate { get; set; }

        /// <summary>
        /// 是否结束工序
        /// </summary>
        public bool IsEndProcess { get; set; }

        /// <summary>
        /// 是否开始工序
        /// </summary>
        public bool IsStartProcess { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty { get; set; }
        /// <summary>
        /// 上下文
        /// </summary>
        public HybridDictionary Context { get; set; }
    }

    /// <summary>
    /// 报检参数
    /// </summary>
    [Serializable]
    public class InspBillEvent
    {
        /// <summary>
        /// 成品报检单号
        /// </summary>
        public string InspNo { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double ShopId { get; set; }

        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 报检数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 报检日志ID
        /// </summary>
        public double InspLogId { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime? CollectionDate { get; set; }

        /// <summary>
        /// 流程卡号
        /// </summary>
        public string FlowCardNo { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<ProductBarcodeInfo> Barcodes { get; set; } = new List<ProductBarcodeInfo>();
    }

    /// <summary>
    /// 条码明细
    /// </summary>
    [Serializable]
    public class ProductBarcodeInfo
    {
        /// <summary>
        /// SN号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 成品检验提交后事件
    /// </summary>
    [Serializable]
    public class ShippingBillSubmittedEvent
    {
        /// <summary>
        /// 报检日志ID
        /// </summary>
        public double InspLogId { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult Result { get; set; }

        /// <summary>
        /// 检验人Id
        /// </summary>
        public double? InspectorId { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime InspectDate { get; set; }

        /// <summary>
        /// 成品检验单号
        /// </summary>
        public string CheckNo { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public string ProcessMode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 缺陷代码ID集合
        /// </summary>
        public List<double> DefectIds { get; set; }

        /// <summary>
        /// 抽样明细列表
        /// </summary>
        public List<ProductSample> SampleList { get; set; }
    }

    /// <summary>
    /// 抽样明细
    /// </summary>
    [Serializable]
    public class ProductSample
    {
        /// <summary>
        /// SN号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 检验数量
        /// </summary>
        public decimal InspQty { get; set; }

        /// <summary>
        /// 检验结论
        /// </summary>
        public InspectionResult InspectionResult { get; set; }

        /// <summary>
        /// 不良检验项目
        /// </summary>
        public string FailedInspectionName { get; set; }

        /// <summary>
        /// 缺陷代码Id集合
        /// </summary>
        public List<double> DefectIdList { get; set; }
    }
}