using SIE.Common;
using SIE.EventMessages.Common.SnModels;
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Inspection
{
    /// <summary>
    /// 首检报检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultFirstInsp))]
    public interface IFirstInsp
    {
        /// <summary>
        /// 首检报检
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        void GenerateFirstInsp(FirstInspEvent inspEvent);

        /// <summary>
        /// 首检报检（任务单）
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        void GenerateTaskFirstInsp(FirstInspEvent inspEvent);

        /// <summary>
        /// 验证是否完成首检（任务单）
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        void CheckTaskFinishInsp(FirstInspEvent inspEvent);

        /// <summary>
        /// 根据首检检验单结果更新首检报检结果
        /// </summary>
        /// <param name="billEvent">首检检验提交后事件集合</param>
        void UpdateFirstInspResult(FirstInspBillSubmittedEvent billEvent);

        /// <summary>
        /// 根据首检不合格审核更新首检报检审核结果
        /// </summary>
        /// <param name="billEvent">首检不合格审核提交后事件集合</param>
        void UpdateFirstAuditResult(FirstInspBillSubmittedEvent billEvent);


        /// <summary>
        /// 验证Sn是否属于单据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        ValidateResultInfo ValidateSnInBill(ValidateRequestInfo info);

        /// <summary>
        /// 获取报工记录对应成品检验单
        /// </summary>
        /// <param name="recordIds">报工记录</param>
        /// <returns></returns>
        Dictionary<double, string> GetRecordInspLogByIds(List<double> recordIds);
    }

    /// <summary>
    /// 首检报检接口默认实现
    /// </summary>
    public class DefaultFirstInsp : IFirstInsp
    {
        /// <summary>
        /// 首检报检
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        public void GenerateFirstInsp(FirstInspEvent inspEvent)
        {
            //重写接口
        }

        /// <summary>
        /// 首检报检（任务单）
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        public void GenerateTaskFirstInsp(FirstInspEvent inspEvent)
        {
            //重写接口
        }

        /// <summary>
        /// 验证是否完成首检（任务单）
        /// </summary>
        /// <param name="inspEvent">首检报检事件</param>
        public void CheckTaskFinishInsp(FirstInspEvent inspEvent)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// 根据首检检验单结果更新首检报检结果
        /// </summary>
        /// <param name="billEvent">首检检验提交后事件集合</param>
        public void UpdateFirstInspResult(FirstInspBillSubmittedEvent billEvent)
        {
            //重写接口
        }

        /// <summary>
        /// 根据首检不合格审核更新首检报检审核结果
        /// </summary>
        /// <param name="billEvent">首检不合格审核提交后事件集合</param>
        public void UpdateFirstAuditResult(FirstInspBillSubmittedEvent billEvent)
        {
            //重写接口
        }

        /// <summary>
        /// 验证Sn是否属于单据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ValidateResultInfo ValidateSnInBill(ValidateRequestInfo info)
        {
            return null;
        }

        /// <summary>
        /// 获取报工记录对应成品检验单
        /// </summary>
        /// <param name="recordIds"></param>
        /// <returns></returns>
        public virtual Dictionary<double, string> GetRecordInspLogByIds(List<double> recordIds)
        {
            return new Dictionary<double, string>();
        }
    }

    /// <summary>
    /// 首检报检创建成品检验单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultCreateFirstInspBill))]
    public interface ICreateFirstInspBill
    {
        /// <summary>
        /// 生成首检检验单
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        void GenerateFirstInspBill(FirstInspBillEvent billEvent);
    }

    /// <summary>
    /// 首检报检创建成品检验单接口默认实现
    /// </summary>
    public class DefaultCreateFirstInspBill : ICreateFirstInspBill
    {
        /// <summary>
        /// 生成首检检验单
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        public void GenerateFirstInspBill(FirstInspBillEvent billEvent)
        {
            //重写接口
        }
    }

    /// <summary>
    /// 成品报检事件
    /// </summary>
    [Serializable]
    public class FirstInspEvent
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
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double ShopId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string DispatchTaskNo { get; set; }

        /// <summary>
        /// 报检数量
        /// </summary>
        public decimal FirstInspQty { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

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
    }

    /// <summary>
    /// 报检参数
    /// </summary>
    [Serializable]
    public class FirstInspBillEvent
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double ShopId { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 班组ID
        /// </summary>
        public double? WorkGroupId { get; set; }

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime CollectionDate { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> Barcodes { get; set; } = new List<string>();

        /// <summary>
        /// 报检日志ID
        /// </summary>
        public double InspLogId { get; set; }

        /// <summary>
        /// 报检单号
        /// </summary>
        public string InspNo { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public List<double> ProcessIdList { get; set; }

        /// <summary>
        /// 是否胶纸板检验
        /// </summary>
        public bool IsGlueInsp { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public double? FactoryId { get; set; }
    }

    /// <summary>
    /// 首件检验提交后事件
    /// </summary>
    [Serializable]
    public class FirstInspBillSubmittedEvent
    {
        /// <summary>
        /// 首件检验单号
        /// </summary>
        public string CheckNo { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 检验员
        /// </summary>
        public double? InspectorId { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult Result { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime InspectDate { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public ProcessMode ProcessMode { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public double AuditManId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// 报检日志ID
        /// </summary>
        public double InspLogId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public List<string> Barcodes { get; set; }

        /// <summary>
        /// 缺陷代码Id集合
        /// </summary>
        public List<double> DefectIdList { get; set; }
    }
}