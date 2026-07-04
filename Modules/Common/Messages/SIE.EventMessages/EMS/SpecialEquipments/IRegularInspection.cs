using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.SpecialEquipments
{
    /// <summary>
    /// 特种设备定检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultRegularInspection))]
    public interface IRegularInspection
    {
        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        List<string> PurchaseGetRegularInspectionNo(PagingInfo pagingInfo, string keyword);

        /// <summary>
        /// 关闭未完结的特种设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        void CloseRegularInspectionByEquipAccountIds(IList<double> equipAccountIds);
    }

    /// <summary>
    /// 特种设备定检接口默认实现
    /// </summary>
    public class DefaultRegularInspection : IRegularInspection
    {
        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        public List<string> PurchaseGetRegularInspectionNo(PagingInfo pagingInfo, string keyword)
        {
            return new List<string> { };
        }

        /// <summary>
        /// 关闭未完结的特种设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseRegularInspectionByEquipAccountIds(IList<double> equipAccountIds)
        {
            Logging.LogManager.Logger.Warn("关闭未完结的特种设备定检任务接口未有具体实现!".L10N());
        }
    }
}
