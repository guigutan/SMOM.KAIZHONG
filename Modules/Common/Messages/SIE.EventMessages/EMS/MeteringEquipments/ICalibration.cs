using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.MeteringEquipments
{
    /// <summary>
    /// 计量设备定检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultCalibration))]
    public interface ICalibration
    {
        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        List<CalibrationObject> PurchaseGetCalibrationNo(PagingInfo pagingInfo, string keyword);

        /// <summary>
        /// 关闭未完结的计量设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        void CloseCalibrationByEquipAccountIds(IList<double> equipAccountIds);
    }

    /// <summary>
    /// 计量设备定检接口默认实现
    /// </summary>
    public class DefaultCalibration : ICalibration
    {
        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        public List<CalibrationObject> PurchaseGetCalibrationNo(PagingInfo pagingInfo, string keyword)
        {
            return new List<CalibrationObject> { };
        }

        /// <summary>
        /// 关闭未完结的计量设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseCalibrationByEquipAccountIds(IList<double> equipAccountIds)
        {
            Logging.LogManager.Logger.Warn("关闭未完结的计量设备定检任务接口未有具体实现!".L10N());
        }
    }

    /// <summary>
    /// 计量设备定检对象
    /// </summary>
    public class CalibrationObject
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string InspectionNo { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>

        public string PlanNmae { get; set; }
    }
}
