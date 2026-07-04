using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Repairs
{
    /// <summary>
    /// 设备维修单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultEquipRepairBills))]
    public interface IEquipRepairBill
    {
        /// <summary>
        /// 关闭未完结的设备维修单
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        void CloseEquipRepairBillByEquipAccountIds(IList<double> equipAccountIds);

        /// <summary>
        /// 获取设备的维修工时和成本
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        List<WorkHourAndCostInfo> GetEquipRepairWorkHourAndCost(IList<double> equipAccountIds);

        /// <summary>
        /// 判断当前点检单是否有维修单
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        bool CheckPlanWithRepairBill(double equipAccountId, string sourceNo, int sourceType);
    }

    /// <summary>
    /// 设备维修单接口默认实现
    /// </summary>
    public class DefaultEquipRepairBills : IEquipRepairBill
    {
        /// <summary>
        /// 关闭未完结的设备维修单
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseEquipRepairBillByEquipAccountIds(IList<double> equipAccountIds)
        {
            Logging.LogManager.Logger.Warn("关闭未完结的设备维修单接口未有具体实现!".L10N());
        }

        /// <summary>
        /// 获取设备的维修工时和成本
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns>设备的维修工时和成本</returns>
        public virtual List<WorkHourAndCostInfo> GetEquipRepairWorkHourAndCost(IList<double> equipAccountIds)
        {
            return new List<WorkHourAndCostInfo>();
        }

        /// <summary>
        /// 判断当前点检单是否有维修单
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        public virtual bool CheckPlanWithRepairBill(double equipAccountId, string sourceNo, int sourceType)
        {
            return false;
        }
    }
}
