
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Dispatchs
{
    /// <summary>
    /// KZ工序报工接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultITaskReportKZ))]
    public interface ITaskReportKZ
    {
        /// <summary>
        /// 热处理工序报工
        /// </summary>
        /// <param name="reportInfos"></param>
        void HeatTreatmentReport(List<ReportInfo> reportInfos);

        /// <summary>
        /// 耐压采集报工
        /// </summary>
        /// <param name="reportInfos"></param>
        void PressureReport(List<ReportInfo> reportInfos);

        /// <summary>
        /// 包装采集报工
        /// </summary>
        /// <param name="packingReportInfos"></param>
        void PackingReport(List<ReportInfo> packingReportInfos, bool IsTaskFinish = true);

        /// <summary>
        /// 校验SN前置工序任务是否已报工("电性能测试"工序除外)
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="process">工序编码/名称</param>
        object ValidatePrepareProcessHasReport(string sn, string process);
        /// <summary>
        /// 校验标签是否已报工
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        bool ValidateProcessHasReport(string batchNo, string process);
    }

    /// <summary>
    /// 接口默认实现
    /// </summary>
    public class DefaultITaskReportKZ : ITaskReportKZ
    {
        /// <summary>
        /// 热处理工序报工
        /// </summary>
        /// <param name="reportInfos"></param>
        public void HeatTreatmentReport(List<ReportInfo> reportInfos)
        {
        }

        /// <summary>
        /// 耐压采集报工
        /// </summary>
        /// <param name="reportInfos"></param>
        public void PressureReport(List<ReportInfo> reportInfos)
        {
        }

        /// <summary>
        /// 包装采集报工
        /// </summary>
        /// <param name="packingReportInfos"></param>
        public void PackingReport(List<ReportInfo> packingReportInfos, bool IsTaskFinish = true)
        {
        }

        /// <summary>
        /// 校验前置工序任务是否已报工
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="process">工序编码/名称</param>
        public object ValidatePrepareProcessHasReport(string sn, string process) { return null; }

        /// <summary>
        /// 校验标签是否已报工
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public bool ValidateProcessHasReport(string batchNo, string process) { return false; }
    }

}
