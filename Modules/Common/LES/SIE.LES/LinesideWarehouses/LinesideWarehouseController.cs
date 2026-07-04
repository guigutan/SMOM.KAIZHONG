using DocumentFormat.OpenXml.ExtendedProperties;
using SIE.Domain;
using SIE.LES.LinesideWarehouses.Models;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using WipEnterpriseInfo = SIE.LES.LinesideWarehouses.Models.WipEnterpriseInfo;

namespace SIE.LES.LinesideWarehouses
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class LinesideWarehouseController : DomainController
    {
        /// <summary>
        /// 获取产线线边仓
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehousesForCriteria(LinesideWarehouseCriteria criteria)
        {
            var q = Query<LinesideWarehouse>();
            if (criteria.FactoryId.HasValue)
            {
                q.Where(m => m.FactoryId == criteria.FactoryId);
            }
            if (criteria.WarehouseId.HasValue)
            {
                q.Where(m => m.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.WorkShopId.HasValue)
            {
                q.Where(m => m.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResouceId.HasValue)
            {
                q.Where(m => m.WipResouceId == criteria.WipResouceId);
            }
            if (criteria.CreateTiem.BeginValue.HasValue)
            {
                q.Where(m => m.CreateDate >= criteria.CreateTiem.BeginValue);
            }
            if (criteria.CreateTiem.EndValue.HasValue)
            {
                q.Where(m => m.CreateDate <= criteria.CreateTiem.EndValue);
            }

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取同一资源下的仓库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wipResouceId"></param>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual int GetLinesideWarehouseByResource(double id, double? wipResouceId, double? workShopId)
        {
            return Query<LinesideWarehouse>().Where(m => m.Id != id && m.WipResouceId == wipResouceId && m.WorkShopId == workShopId).Count();
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>
        /// <param name="resId">生产资源ID</param>
        /// <returns>产线线边仓维护</returns>
        public virtual LinesideWarehouse GetLinesideWarehouse(double resId)
        {
            return Query<LinesideWarehouse>()
                .Where(p => p.WipResouceId == resId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>
        /// <param name="resId">生产资源ID</param>
        /// <returns>产线线边仓维护</returns>
        public virtual LinesideWarehouse GetLinesideWarehouseLoad(double resId)
        {
            return Query<LinesideWarehouse>()
                .Where(p => p.WipResouceId == resId)
                .FirstOrDefault(new EagerLoadOptions().LoadWith(LinesideWarehouse.WarehouseProperty).LoadWith(LinesideWarehouse.StorageLocationProperty));
        }

        /// <summary>
        /// 获取产线线边仓基础信息
        /// </summary>
        /// <param name="resId"></param>
        /// <returns></returns>
        public virtual LinesideWareBaseData GetBaseLinesideWarehouse(double resId)
        {
            return Query<LinesideWarehouse>().Where(p => p.WipResouceId == resId)
                .Select(p => new
                { p.Id, p.WipResouceId, p.WarehouseId, p.StorageLocationId }).FirstOrDefault<LinesideWareBaseData>();
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>
        /// <param name="resIds">生产资源ID</param>
        /// <returns>产线线边仓维护</returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehousesByIds(List<double?> resIds)
        {
            return resIds.SplitContains(tempIds =>
            {
                return Query<LinesideWarehouse>()
                    .Where(p => tempIds.Contains(p.WipResouceId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>
        /// <param name="resIds">生产资源ID</param>
        /// <returns>产线线边仓维护</returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehousesByIds(List<double> resIds)
        {
            return resIds.SplitContains(tempIds =>
            {
                return Query<LinesideWarehouse>()
                    .Where(p => p.WipResouceId != null && tempIds.Contains((double)p.WipResouceId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取产线线边仓
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns>产线线边仓</returns>
        public virtual LinesideWarehouse GetLinesideWarehouseByWarehouseId(double warehouseId)
        {
            return Query<LinesideWarehouse>().Where(p => p.WarehouseId == warehouseId).FirstOrDefault();
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>       
        /// <returns>返回产线线边仓维护列表</returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehouses()
        {
            return Query<LinesideWarehouse>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产线线边仓维护
        /// </summary>       
        /// <returns>返回产线线边仓维护列表</returns>
        public virtual EntityList<LinesideWarehouse> GetLinesideWarehouses(PagingInfo pagingInfo, string keyword = null)
        {
            var q = Query<LinesideWarehouse>();
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Factory.Code.Contains(keyword) || p.Factory.Name.Contains(keyword)
                || p.WorkShop.Code.Contains(keyword) || p.WorkShop.Name.Contains(keyword)
                || p.WipResouce.Code.Contains(keyword) || p.WipResouce.Name.Contains(keyword)
                || p.Warehouse.Code.Contains(keyword) || p.Warehouse.Name.Contains(keyword)
                || p.StorageLocation.Code.Contains(keyword) || p.StorageLocation.Name.Contains(keyword)
               );
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>       
        /// <returns>返回仓库列表</returns>
        public virtual EntityList<Warehouse> GeWarehouses(PagingInfo pagingInfo = null, string keyword = null)
        {
            return Query<Warehouse>().Exists<LinesideWarehouse>((w, lw) => lw.Where(p => p.WarehouseId == w.Id))
                .Where(w => w.State == State.Enable && !w.IsFrozen)
                .WhereIf(keyword.IsNotEmpty(), w => w.Code.Contains(keyword) || w.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用线边仓
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="isFilterVirtual"></param>
        /// <param name="whIds"></param>
        /// <returns></returns>
        public virtual EntityList<Warehouse> GetAvailableLinesideWarehouses(PagingInfo pagingInfo, string keyword, bool? isFilterVirtual = false, List<double> whIds = null)
        {
            var q = Query<Warehouse>();
            q.Where(p => p.State == State.Enable);
            q.Where(p => !p.IsFrozen);
            q.Where(p => p.IsLineWarehouse);
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (isFilterVirtual.HasValue && isFilterVirtual.Value)
                q.Where(p => p.LibraryType == LibraryType.Entity);
            if (whIds != null && whIds.Any())
            {
                q.Where(p => whIds.Contains(p.Id));
            }
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取同仓库的线边仓资源Id
        /// </summary>
        /// <param name="linesideWarehouse"></param>
        /// <returns></returns>
        public virtual List<double> GetSameWareResourceIds(LinesideWareBaseData linesideWarehouse)
        {
            return Query<LinesideWarehouse>().Where(p => p.WarehouseId == linesideWarehouse.WarehouseId).ToList().Select(p => (double)p.WipResouceId).Distinct().ToList();
        }

        /// <summary>
        /// 获取生产资源的工厂车间
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual WipEnterpriseInfo GetWipResourceInfo(double wipId)
        {
            var data = Query<WipResource>()
                .LeftJoin<Enterprise>("ef", (w, ef) => w.FactoryId == ef.Id)
                .LeftJoin<Enterprise>("es", (w, es) => w.WorkShopId == es.Id)
                .Where(w => w.Id == wipId)
                .Select<Enterprise, Enterprise>((w, ef, es) => new
                {
                    FactoryId = ef.Id,
                    FactoryName = ef.Name,
                    WorkShopId = es.Id,
                    WorkShopName = es.Name,
                }).ToList<WipEnterpriseInfo>().ToList();
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 递归向上获取工厂
        /// </summary>
        /// <param name="eId"></param>
        /// <returns></returns>
        private WipEnterpriseInfo GetUpFactory(double eId)
        {
            var data = Query<Enterprise>()
               .LeftJoin<EnterpriseLevel>((e, el) => e.LevelId == el.Id)
               .Where<EnterpriseLevel>((e, el) => e.Id == eId)
               .Select<EnterpriseLevel>((e, el) => new
               {
                   TreePId = e.TreePId,
                   Type = el.Type,
                   FactoryId = e.Id,
                   FactoryName = e.Name,
               }).FirstOrDefault<WipEnterpriseInfo>();
            if (data != null)
            {
                if (data.Type == EnterpriseType.Plant)
                {
                    return data;
                }
                else
                {
                    return GetUpFactory(data.TreePId);
                }
            }
            else
            {
                return new WipEnterpriseInfo();
            }
        }

        /// <summary>
        /// 获取车间的工厂
        /// </summary>
        /// <param name="eId"></param>
        /// <returns></returns>
        public virtual WipEnterpriseInfo GetWorkShopInfo(double eId)
        {
            return GetUpFactory(eId);
        }


        /// <summary>
        /// 判断是否需要自动接收
        /// </summary>
        /// <param name="enterpriseCode"></param>
        /// <returns></returns>
        public virtual bool IsAutoReceive(string enterpriseCode)
        {
            if (enterpriseCode.IsNullOrEmpty())
                return false;
            var list = Query<LinesideWarehouse>().Where(p => ((p.WorkShop.Code == enterpriseCode && p.WipResouceId == null) || p.WipResouce.Code == enterpriseCode) ).ToList();
            if (list.Count == 0)    //未维护相关数据时,默认为自动接收
                return true;
            if (list.Any(p => p.AutoReceive == true))
                return true;
            return false;
        }

        /// <summary>
        /// 根据车间产线获取维护信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public virtual LinesideWarehouse GetLinesideWarehouse(double? workShopId, double? resourceId, EntityList<LinesideWarehouse> lineWhs = null)
        {
            LinesideWarehouse lineWh = null;    //产线线边仓
            if(lineWhs == null)
                lineWhs = Query<LinesideWarehouse>().Where(p => (p.WorkShopId == workShopId && p.WipResouceId == null) || (p.WipResouceId == resourceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (lineWhs.Count == 0)    
                return lineWh;
            if (resourceId > 0)
            {
                lineWh = lineWhs.FirstOrDefault(p => p.WipResouceId == resourceId);
            }
            else if (workShopId > 0)
            {
                lineWh = lineWhs.FirstOrDefault(p => p.WorkShopId == workShopId && p.WipResouceId == null);
            }
            return lineWh;
        }
    }
}
