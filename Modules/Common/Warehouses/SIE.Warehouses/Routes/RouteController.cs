using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses
{
    /// <summary>
    /// 路径控制器
    /// </summary>
    public partial class RouteController : DomainController
    {
        /// <summary>
        /// 获取路径ByCode
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>路径</returns>
        public virtual Route GetRouteByCode(string code)
        {
            return Query<Route>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="ids">Id</param>
        /// <returns>路径</returns>
        public virtual EntityList<Route> GetRoutes(List<double> ids)
        {
            return ids.SplitContains(pIds =>
            {
                return Query<Route>().Where(p => pIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取可用路径
        /// </summary>
        /// <param name="srcWhId">启示仓库ID</param>
        /// <returns>路径</returns>
        public virtual EntityList<Route> GetRoutes(double srcWhId)
        {
            var query = Query<Route>()
                .Join<Warehouse>((r, w) => r.SrcWhCode == w.Code && w.Id == srcWhId);
            query.Where(p => p.State == State.Enable);
            return query.ToList();
        }

        /// <summary>
        /// 获取路径查询数据
        /// </summary>
        /// <param name="criteria">路径查询条件</param>
        /// <returns>返回路径数据</returns>
        public virtual EntityList<Route> GetRouteData(RouteCriteria criteria)
        {
            var q = Query<Route>();
            ////仓库权限关联查询

            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    q.Where(p => p.Code.Contains(criteria.Code));
                if (criteria.DesWhCode.IsNotEmpty())
                    q.Where(p => p.DesWhCode.Contains(criteria.DesWhCode));
                if (criteria.SrcWhCode.IsNotEmpty())
                    q.Where(p => p.SrcWhCode.Contains(criteria.SrcWhCode));
                if (criteria.SrcAdd.IsNotEmpty())
                    q.Where(p => p.SrcAdd.Contains(criteria.SrcAdd));
                if (criteria.DesAdd.IsNotEmpty())
                    q.Where(p => p.DesAdd.Contains(criteria.DesAdd));
                if (criteria.Docks.IsNotEmpty())
                    q.Where(p => p.Docks.Contains(criteria.Docks));
                if (criteria.State.HasValue)
                    q.Where(p => p.State == criteria.State.Value);
            }

            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="srcAdd">来源地址</param>
        /// <param name="desAdd">目的地址</param>
        /// <returns>路径</returns>
        public virtual int GetRouteCount(string srcAdd, string desAdd)
        {
            var query = Query<Route>().Where(p => p.State == State.Enable);
            query.WhereIf(srcAdd.IsNotEmpty(), p => p.SrcAdd == srcAdd);
            query.WhereIf(desAdd.IsNotEmpty(), p => p.DesAdd == desAdd);
            return query.Count();
        }

        /// <summary>
        /// 检查验证路径
        /// </summary>
        /// <param name="srcAdd">来源地址</param>
        /// <param name="desAdd">目的地址</param>
        /// <exception cref="ValidationException">路径验证：起始地址[{0}]-> 终点地址[{1}]不存在可用路径</exception>
        public virtual void CheckRoute(string srcAdd, string desAdd)
        {
            if (srcAdd == null)
            {
                throw new ArgumentNullException(nameof(srcAdd));
            }
            if (desAdd == null)
            {
                throw new ArgumentNullException(nameof(desAdd));
            }

            ////Cell:立库库位  StationGroup:站台组 Station/Entrance:站台 ,都不是前面的类型，着为平库库位
            if (srcAdd.IndexOf("Cell") > -1 || srcAdd.IndexOf("StationGroup") > -1 || srcAdd.IndexOf("Station") > -1 || srcAdd.IndexOf("Entrance") > -1)
            {
                var count = GetRouteCount(srcAdd, desAdd);
                if (count <= 0)
                {
                    throw new ValidationException("路径验证：起始地址[{0}]-> 终点地址[{1}]不存在可用路径".L10nFormat(srcAdd, desAdd));
                }
            }
        }
    }
}
