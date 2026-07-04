using SIE.EventMessages.MES.PPSNs;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Dispatchs
{
    /// <summary>
    /// 
    /// </summary>
    [Service(FallbackType = typeof(DefaultIDispatchs))]
    public interface IDispatchs
    {
        /// <summary>
        /// 更新任务单的首末工序
        /// </summary>
        /// <param name="startProcessId"></param>
        /// <param name="endProcessId"></param>
        /// <param name="workOrderId"></param>
        void UpdateTaskStartEndProcess(double? startProcessId, double? endProcessId, double workOrderId);

        /// <summary>
        /// 根据工单创建任务单
        /// </summary>
        /// <param name="workOrderIds"></param>
        void GenerateTaskByWorkOrderIds(List<double> workOrderIds);

        /// <summary>
        /// 根据任务单Id获取生产批次打印实体的轴号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetPrintAxisNumberByTaskId(double id);

        /// <summary>
        /// 根据任务单Id获取资源名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetResourceNameByTaskId(double id);

        /// <summary>
        /// 根据任务单Id获取工序的分单数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        decimal GetProcessZcodeByTaskId(double Id);

        /// <summary>
        /// 更新派工明细下的工位名称
        /// </summary>
        /// <param name="disInfoList"></param>
        void UpateDispatchTaskDetail(List<DispatchTaskDetailInfo> disInfoList);
    }

    public class DefaultIDispatchs : IDispatchs
    {

        /// <summary>
        /// 更新任务单的首末工序
        /// </summary>
        /// <param name="startProcessId"></param>
        /// <param name="endProcessId"></param>
        /// <param name="workOrderId"></param>
        public void UpdateTaskStartEndProcess(double? startProcessId, double? endProcessId, double workOrderId)
        { 
            
        }


        /// <summary>
        /// 根据工单创建任务单
        /// </summary>
        /// <param name="workOrderIds"></param>
        public void GenerateTaskByWorkOrderIds(List<double> workOrderIds)
        {
            return;
        }

        /// <summary>
        /// 根据任务单Id获取生产批次打印实体的轴号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetPrintAxisNumberByTaskId(double id)
        {
            return string.Empty;
        }

        /// <summary>
        /// 根据任务单Id获取资源名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetResourceNameByTaskId(double id)
        {
            return string.Empty;
        }

        /// <summary>
        /// 根据任务单Id获取工序的分单数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public decimal GetProcessZcodeByTaskId(double Id)
        {
            return 0;
        }

        /// <summary>
        /// 更新派工明细下的工位名称
        /// </summary>
        /// <param name="disInfoList"></param>
        public void UpateDispatchTaskDetail(List<DispatchTaskDetailInfo> disInfoList)
        {
        }
    }
}
