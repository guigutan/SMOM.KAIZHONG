using SIE.Core;
using SIE.Core.Common.Service;
using SIE.Dock.DockMaintains;
using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Dock.YardZones.Service
{
    /// <summary>
    /// 园片区维护服务
    /// </summary>
    public partial class YardZoneService : DomainService
    {
        #region 属性 + 构造方法

        /// <summary>
        /// 园片区维护数据访问
        /// </summary>
        private readonly YardZoneDao _yardzoneDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        public YardZoneService(YardZoneDao yardzoneDao)
        {
            _yardzoneDao = yardzoneDao;
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回查询结果</returns>
        public virtual EntityList<YardZone> GetYardZones(YardZoneCriteria criteria)
        {
            return _yardzoneDao.GetYardZones(criteria);
        }

        /// <summary>
        /// 获取可用园片区维护数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园片区维护数据</returns>
        public virtual EntityList<YardZone> GetEnableYardZones(string keyword, PagingInfo pagingInfo)
        {
            return _yardzoneDao.GetEnableYardZones(keyword, pagingInfo);
        }

        /// <summary>
        /// 获取可用园片区有收货月台的数据
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>园区维护数据</returns>
        public virtual EntityList<YardZone> GetEnableYardZoneByReceiveDocks(string keyword, PagingInfo pagingInfo)
        {
            var query = DB.Query<YardZone>().Exists<DockMaintain>((x, y) => y.Where(p => p.YardZoneId == x.Id && p.IsReceive))
                        .Where(p => p.State == State.Enable);

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取月台装卸能力列表
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>月台装卸能力列表</returns>
        public virtual EntityList<DockHandling> GetDockHandlingList(double billId)
        {
            return DB.Query<DockHandling>().Where(p => p.YardZoneId == billId).ToList();
        }

        /// <summary>
        /// 验证园片区维护数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void ValidateYardZoneData(YardZone data)
        {
            if (data.DockHandlingList.Count > 1)
            {
                string tmpEndTime = "";
                foreach (var dockhandling in data.DockHandlingList.OrderBy(p => p.BeginTime).ThenBy(p => p.EndTime))
                {
                    if (tmpEndTime.IsNotEmpty() && DateTime.Parse(dockhandling.BeginTime) < DateTime.Parse(tmpEndTime))
                    {
                        throw new ValidationException("园片区编码:[{0}]的月台装卸能力不能存在交集的两个时间段".L10nFormat(data.Code));
                    }

                    tmpEndTime = dockhandling.EndTime;
                }
            }
        }
    }
}
