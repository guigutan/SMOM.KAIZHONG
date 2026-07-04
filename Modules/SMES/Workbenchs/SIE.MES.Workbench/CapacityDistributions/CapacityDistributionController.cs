using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.Resources.ShiftTypes;
using System;
using System.Linq;

namespace SIE.MES.Workbench.CapacityDistributions
{
    /// <summary>
    /// 工作台的工序工位实体的控制器
    /// </summary>
    public class CapacityDistributionController : DomainController
    {
        /* /// <summary>
        /// 获取产能分布数据
        /// </summary>
        /// <param name="resourceId">资源(产线)Id</param>
        /// <param name="shift">班次</param>
        /// <param name="capaDate">日期</param>
        /// <returns>产能分布数据</returns>
        public virtual EntityList<CapacityDistribution> GetCapacityDistributions(double resourceId, Shift shift, DateTime capaDate)
        {
            var capaDistrs = Query<CapacityDistribution>().Where(p => p.ResourceId == resourceId && p.ShiftId == shift.Id && p.ShiftDate == capaDate && p.CapacityTime <= shift.EndTime.Hour)
                .OrderBy(x => x.CapacityTime).ToList();
            return capaDistrs;
        }*/

        /* /// <summary>
        /// 获取产能分布数据
        /// </summary>
        /// <param name="resourceId">资源(产线)Id</param>
        /// <param name="capaDate">日期</param>
        /// <returns>产线产能分布集合</returns>
        public virtual EntityList<CapacityDistribution> GetCapacityDistributions(double resourceId, DateTime capaDate)
        {
            var nowTime = RF.Find<CapacityDistribution>().GetDbTime();
            EntityList<CapacityDistribution> capaDistrs = null;
            ////Shift shift = GetShift(resourceId, nowTime);
            Shift shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(resourceId, nowTime);
            if (shift == null)
            {
                capaDistrs = Query<CapacityDistribution>().Where(p => p.ResourceId == resourceId && p.ShiftDate == capaDate).OrderBy(x => x.CapacityTime).ToList();
            }
            else
            {
                capaDistrs = GetCapacityDistributions(resourceId, shift, capaDate);
            }

            return capaDistrs;
        }*/

        ///// <summary>
        ///// 根据资源、日期获取班次
        ///// </summary>
        ///// <param name="resourceId">资源(产线)Id</param>
        ///// <param name="curDate">日期</param>
        ///// <returns>班次</returns>
        //public virtual Shift GetShift(double resourceId, DateTime curDate)
        //{
        //    var controller = RT.Service.Resolve<CalendarController>();
        //    var calender = controller.GetResourceCalendarByDateTime(resourceId, curDate);
        //    if (calender == null || calender.CalendarDate == null || calender.ShiftType == null || calender.ShiftType.ShiftList.Count == 0)
        //        throw new ValidationException("资源日历不存在，资源ID{0}".L10nFormat(resourceId));
        //    Shift shift = controller.GetShift(calender, curDate);
        //    if (shift == null)
        //        throw new ValidationException("班次不存在，资源ID{0}".L10nFormat(resourceId));
        //    return shift;
        //}

        /// <summary>
        /// 根据产线查异常停线
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <param name="type">排除停线类型</param>
        /// <returns>异常停线列表</returns>
        public virtual EntityList<AbnormalCause> GetAbnormalCauseList(double resourceId, double shiftId, ExceptionStopType? type, DateTime nowTime)
        {
            ////var nowTime = RF.Find<AbnormalCause>().GetDbTime();
            ////Shift shift = RT.Service.Resolve<CapacityDistributionController>().GetShift((double)resourceId, nowTime);
            ////Shift shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(resourceId, nowTime);
            var curShift = RF.GetById<Shift>(shiftId);
            if (curShift == null)
                throw new EntityNotFoundException(typeof(Shift), shiftId);
            var shiftDate = RT.Service.Resolve<ShiftTypeController>().GetShiftDate(curShift, nowTime);

            var shiftBeginTimeStr = shiftDate.ToString("yyyy/MM/dd ") + curShift.BeginTime.ToString("HH:mm:ss");
            var shiftBeginTime = DateTime.Parse(shiftBeginTimeStr);

            string shiftEndTiemStr = string.Empty;
            if (curShift.IsOverDay)
                shiftEndTiemStr = shiftDate.AddDays(1).ToString("yyyy/MM/dd ") + curShift.EndTime.ToString("HH:mm:ss");
            else
                shiftEndTiemStr = shiftDate.ToString("yyyy/MM/dd ") + curShift.EndTime.ToString("HH:mm:ss");
            var shiftEndTime = DateTime.Parse(shiftEndTiemStr);

            var query = Query<AbnormalCause>().Where(p => p.ResourceId == resourceId && p.BeginDate < nowTime
                                               && p.BeginDate >= shiftBeginTime && p.BeginDate < shiftEndTime);
            if (type != null) //// 
                query.Where(p => p.ExceptionStopType != type);
            return query.OrderBy(p => p.BeginDate).ToList();
        }
    }
}
