using Nest;
using SIE.Core;
using SIE.Core.Common.Service;
using SIE.Dock.DockRunMts.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.DockRunMts.Service
{
    /// <summary>
    /// 月台运行维护服务
    /// </summary>
    public class DockRunMtService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 月台运行维护数据访问
        /// </summary>
        private readonly DockRunMtDao _dockRunmtDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DockRunMtService(DockRunMtDao dockRunmtDao)
        {
            _dockRunmtDao = dockRunmtDao;
        }
        #endregion

        /// <summary>
        /// 获取月台运行维护数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>月台运行维护数据</returns>
        public virtual DockRunMt GetDockRunMt(double id, EagerLoadOptions elo)
        {
            return _dockRunmtDao.GetDockRunMt(id, elo);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<DockRunMt> GetDockRunMts(DockRunMtCriteria criteria)
        {
            return _dockRunmtDao.GetDockRunMts(criteria);
        }

        /// <summary>
        /// 根据月台ID获取月台运行数据
        /// </summary>
        /// <param name="dockIds">月台ID集合</param>
        /// <returns>月台运行数据</returns>
        public virtual EntityList<DockRunMt> GetDockRunMtByDockIds(List<double> dockIds)
        {
            return _dockRunmtDao.GetDockRunMtByDockIds(dockIds);
        }

        /// <summary>
        /// 获取工作时段列表
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>工作时段列表</returns>
        public virtual EntityList<WorkTime> GetWorkTimeList(double billId)
        {
            return DB.Query<WorkTime>().Where(p => p.DockRunMtId == billId).ToList();
        }

        /// <summary>
        /// 获取例外时段列表
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>例外时段列表</returns>
        public virtual EntityList<ExcepTime> GetExcepTimeList(double billId)
        {
            return DB.Query<ExcepTime>().Where(p => p.DockRunMtId == billId).ToList();
        }

        /// <summary>
        /// 验证月台运行维护数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void ValidateDockRunMtData(DockRunMt data)
        {
            if (data.WorkTimeList.Count > 1)
            {
                DateTime? tmpEndTime = null;
                foreach (var workTime in data.WorkTimeList.OrderBy(p => p.BeginTime).ThenBy(p => p.EndTime))
                {
                    if (tmpEndTime.HasValue && DateTime.Parse(workTime.BeginTime.ToString(DateTimeFormat.HHmm)) < DateTime.Parse(tmpEndTime.Value.ToString(DateTimeFormat.HHmm)))
                    {
                        throw new ValidationException("月台编码:[{0}]的工作时段不能存在交集的两个时间段".L10nFormat(data.DockMaintain?.Code));
                    }

                    tmpEndTime = workTime.EndTime;
                }
            }

            if (data.ExcepTimeList.Count > 1)
            {
                DateTime? tmpEndTime = null;
                foreach (var excepTime in data.ExcepTimeList.OrderBy(p => p.BeginTime).ThenBy(p => p.EndTime))
                {
                    if (tmpEndTime.HasValue && excepTime.BeginTime < tmpEndTime.Value)
                    {
                        throw new ValidationException("月台编码:[{0}]的例外时段不能存在交集的两个时间段".L10nFormat(data.DockMaintain?.Code));
                    }

                    tmpEndTime = excepTime.EndTime;
                }
            }
        }
    }
}
