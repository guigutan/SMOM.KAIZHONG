using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 员工评分控制器
    /// </summary>
    public class EmployeeMarksController : DomainController
    {
        /// <summary>
        /// 根据员工Id，班次Id获取个人评分实体
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>个人评分实体</returns>
        public virtual EmployeeMark GetEmpMark(double employeeId, double shiftId, double resourceId)
        {
            var empMark = Query<EmployeeMark>().Where(p => p.EmployeeId == employeeId && p.ShiftId == shiftId && p.ResourceId == resourceId).FirstOrDefault();
            return empMark;
        }

        /// <summary>
        /// 根据资源获取个人评分实体的班次Id集合
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>班次Id集合</returns>
        public virtual List<double?> GetEmpMarkShiftIds(double resourceId)
        {
            List<double?> results = null;
            var qrys = Query<EmployeeMark>().Where(p => p.ResourceId == resourceId && p.ShiftId != null).ToList();
            if (qrys != null && qrys.Count > 0)
                results = qrys.OrderBy(x => x.ShiftId).Select(x => x.ShiftId).Distinct().ToList();
            /*else
                throw new ValidationException("资源Id: [{0}]的评分信息为空".L10nFormat(resourceId));*/
            return results;
        }

        /// <summary>
        /// 返回当前用户在评分中的排名
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>当前用户在评分中的排名</returns>
        public virtual int GetRankingOfMe(double employeeId, double shiftId, double resourceId)
        {
            var curRanking = 0;
            var empMarks = Query<EmployeeMark>().Where(p => p.ResourceId == resourceId && p.ShiftId == shiftId).OrderByDescending(p => p.Mark).ToList();
            var curEmpMark = empMarks.FirstOrDefault(p => p.EmployeeId == employeeId && p.ShiftId == shiftId && p.ResourceId == resourceId);
            if (curEmpMark != null)
            {
                curRanking = empMarks.IndexOf(curEmpMark) + 1;
            }

            return curRanking;
        }

        /// <summary>
        /// 根据评分值获取前X名的评分数据
        /// </summary>
        /// <param name="skipIndex">从集合获取数据的起始位置</param>
        /// <param name="tackValue">从集合获取数据的个数</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>前X名的评分数据</returns>
        public virtual List<EmployeeMark> GetTopEmpMark(int skipIndex, int tackValue, double shiftId, double resourceId)
        {
            var qry = Query<EmployeeMark>().Where(x => x.ShiftId == shiftId && x.ResourceId == resourceId).OrderByDescending(p => p.Mark).ToList();
            var top5Marks = qry.Skip(skipIndex).Take(tackValue).ToList();
            return top5Marks;
        }

        /// <summary>
        /// 保存个人评分
        /// </summary>
        /// <param name="empMark">个人评分实体</param>
        public virtual void SaveEmployeeMark(EmployeeMark empMark)
        {
            var entityRepository = empMark.GetRepository() as EntityRepository;
            var dbNowTime = entityRepository.GetDbTime();
            empMark.CalculateTime = dbNowTime;
            RF.Save(empMark);
        }
    }
}