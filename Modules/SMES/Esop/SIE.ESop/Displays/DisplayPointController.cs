using SIE.Dashboard.Definitions;
using SIE.Dashboard.Modules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.ESop.Displays
{
    /// <summary>
    /// 显示点控制器
    /// </summary>
    public class DisplayPointController : DomainController
    {
        const string ESOPKEY = "ESOP";

        /// <summary>
        /// 初始化ESOP控制台等
        /// </summary>
        public virtual void InitEsop()
        {

            DashboardDefinition dashboardDefinition = Query<DashboardDefinition>().Where(m => m.Code == ESOPKEY).FirstOrDefault();
            DashboardModule dashboardModule = Query<DashboardModule>().Where(m => m.KeyLabel == ESOPKEY).FirstOrDefault();
            if (dashboardDefinition != null && dashboardModule != null)
            {
                throw new ValidationException("系统已初始化过ESOP功能，请勿重复初始化".L10N());
            }
            if (dashboardDefinition == null)
            {
                dashboardDefinition = new DashboardDefinition()
                {
                    Code = ESOPKEY,
                    Name = ESOPKEY,
                    LayoutScale = 1,
                    Type = DashboardType.Dashboard,
                    PersistenceStatus = PersistenceStatus.New
                };
                dashboardDefinition.GenerateId();
            }
            if (dashboardModule == null)
            {
                dashboardModule = new DashboardModule()
                {
                    IsFullScreen = false,
                    DashboardDefinitionId = dashboardDefinition.Id,
                    KeyLabel = ESOPKEY,
                    Label = ESOPKEY,
                    PersistenceStatus = PersistenceStatus.New
                };
                dashboardModule.GenerateId();
            }
            using (var trans = DB.TransactionScope(ESopEntityDataProvider.ConnectionStringName))
            {
                RF.Save(dashboardDefinition);
                RF.Save(dashboardModule);
                trans.Complete();
            }
        }



        /// <summary>
        /// 查询显示点
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>查询点列表</returns>
        public virtual EntityList<DisplayPoint> GetDisplayPointList(DisplayPointCriteria criteria)
        {
            var q = Query<DisplayPoint>();
            if (criteria.Code.IsNotEmpty())
                q.Where(f => f.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                q.Where(f => f.Name.Contains(criteria.Name));
            if (criteria.ResourceId.HasValue)
                q.Where(f => f.ResourceId == criteria.ResourceId);
            if (criteria.ProcessId.HasValue)
                q.Exists<DisplayPointProcess>((d, p) => p.Where(f => f.DisplayPointId == d.Id && f.ProcessId == criteria.ProcessId));
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWith(DisplayPointProcess.ProcessProperty).LoadWith(DisplayPointProcess.DisplayPointProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 获取显示点数据
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="keyword">搜索</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>返回显示点数据</returns>
        public virtual EntityList<DisplayPoint> GetDisplayPointList(double resourceId, string keyword, PagingInfo pagingInfo)
        {
            var q = Query<DisplayPoint>()
                .Exists<DisplayPointProcess>((x, y) => y.Where(z => z.DisplayPointId == x.Id));
            return q.Where(p => p.ResourceId == resourceId).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 资源是否关联显示点
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <param name="displayPointId">显示点ID</param>
        /// <returns>bool</returns>
        public virtual bool HasResourceDisplayPoint(double resourceId, double displayPointId)
        {
            return Query<DisplayPointProcess>().Where(f => f.DisplayPointId == displayPointId && f.DisplayPoint.ResourceId == resourceId).Count() > 0;
        }
    }
}