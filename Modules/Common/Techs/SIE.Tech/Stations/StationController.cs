using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using SIE.Api;
using SIE.Core;
using SIE.Domain;
using SIE.EventMessages.MES.Dispatchs;
using SIE.EventMessages.Tech.Stations;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位控制器
    /// </summary>
    public class StationController : DomainController, IStation
    {
        /// <summary>
        /// 根据生产资源更新工位
        /// </summary>
        /// <param name="wipResourceIds"></param>
        public virtual void UpdateStationByWipResource()
        {
            //找出编码不相等或者名称不相等
            var updateStations = Query<Station>().Join<WipResource>((x, y) => x.Code == y.Code && x.Name != y.Name).ToList();

            List<DispatchTaskDetailInfo> taskDetailInfos = new List<DispatchTaskDetailInfo>();

            var wipCodes = updateStations.Select(p => p.Code).Distinct().ToList();
            var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResourceByCodes(wipCodes);

            foreach (var updateStation in updateStations)
            {
                var wipResource = wipResources.FirstOrDefault(p => p.Code == updateStation.Code);
                if (wipResource != null)
                {
                    updateStation.Name = wipResource.Name;
                    updateStation.PersistenceStatus = PersistenceStatus.Modified;
                    DispatchTaskDetailInfo info = new DispatchTaskDetailInfo
                    {
                        StationId = updateStation.Id,
                        StationName = updateStation.Name,
                    };
                    taskDetailInfos.Add(info);
                }
            }

            if (updateStations.Count > 0)
                RF.Save(updateStations);

            if (taskDetailInfos.Any())
                RT.Service.Resolve<IDispatchs>().UpateDispatchTaskDetail(taskDetailInfos);
        }

        /// <summary>
        /// 创建工位
        /// </summary>
        /// <param name="code"></param>
        /// <param name="resourceId"></param>
        /// <param name="processIds"></param>
        public virtual Station CreateStation(string code, string name, double resourceId, List<double?> processIds)
        {
            Station station = new Station();
            station.Code = code;
            station.Name = name;
            station.ResourceId = resourceId;
            station.SourceWipResourceId = resourceId;
            foreach (var processId in processIds)
            {
                if (processId != null)
                    station.StationProcessList.Add(new StationProcess()
                    {
                        ProcessId = processId.Value,
                        PersistenceStatus = PersistenceStatus.New
                    });
            }

            station.PersistenceStatus = PersistenceStatus.New;
            RF.Save(station);
            return station;
        }

        /// <summary>
        /// 根据编码获取工位工序关系
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<StationProcess> GetStationProcessByStationCode(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<StationProcess>().Where(p => c.Contains(p.Station.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据编码获取工位
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<Station> GetStations(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<Station>().Where(p => c.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">模糊查询</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStations(double processId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Station>()
                .Exists<StationProcess>((s, p) => p.Where(x => s.Id == x.StationId && x.ProcessId == processId));

            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">模糊查询</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStations(PagingInfo pagingInfo, string keyword = null)
        {
            var query = Query<Station>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="lineId">资源ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工位列表</returns>
        [ApiService("获取工位")]
        [return: ApiReturn("工位信息. 参数类型: Station")]
        public virtual EntityList<Station> GetStations(double? lineId, double? processId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Station>();

            if (lineId != 0)
                query.Where(p => p.ResourceId == lineId);

            if (processId.HasValue && processId != 0)
                query.Exists<StationProcess>((s, p) => p.Where(x => s.Id == x.StationId && x.ProcessId == processId));

            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据资源编号获取工位及工位物料列表
        /// </summary>
        /// <param name="lineId">资源Id</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStations(double lineId)
        {
            EagerLoadOptions eagerLoad = new EagerLoadOptions();
            return Query<Station>().Where(p => p.ResourceId == lineId).ToList(null, eagerLoad);
        }

        /// <summary>
        /// 根据资源获取工位列表 
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页添加</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStations(double resourceId, string keyword, PagingInfo pagingInfo = null)
        {
            var query = Query<Station>().Where(p => p.ResourceId == resourceId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStations()
        {
            return Query<Station>().ToList();
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="id">工位ID</param>
        /// <returns>工位</returns>
        public virtual Station GetStation(double id)
        {
            return Query<Station>().Where(p => p.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>工位</returns>
        public virtual Station GetStation(string code)
        {
            return Query<Station>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取工位
        /// </summary>
        /// <param name="name">名称</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>工位</returns>
        public virtual Station GetStationByName(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            return Query<Station>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取需上料工位
        /// （批次）上料、（批次）维修
        /// </summary>
        /// <param name="keyWord">查询条件，编码/名称</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetLoadItemStations(string keyWord, PagingInfo pagingInfo = null)
        {
            int[] types = new int[] { (int)ProcessType.Assembly, (int)ProcessType.BatchAssembly, (int)ProcessType.BatchFix, (int)ProcessType.Fix };
            return Query<Station>()
                .Join<StationProcess>((s, p) => s.Id == p.StationId)
                .Join<StationProcess, Process>((x, y) => x.ProcessId == y.Id && types.Contains((int)y.Type))
                .WhereIf(keyWord.IsNotEmpty(), p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord))
                .Distinct()
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产线和工序获取工位列表
        /// </summary>
        /// <param name="reourceId">产线ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStationsByResourceId(double reourceId, double processId, PagingInfo pagingInfo = null)
        {
            return Query<Station>().Join<StationProcess>((s, p) => s.Id == p.StationId && p.ProcessId == processId).Where(p => p.ResourceId == reourceId).ToList(pagingInfo);
        }

        /// <summary>
        /// 是否存在某资源某工序的工位
        /// </summary>
        /// <param name="id">工位Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="processId">工序Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistStation(double id, double resourceId, double processId)
        {
            return Query<Station>().Join<StationProcess>((s, p) => s.Id == p.StationId && p.ProcessId == processId).Where(p => p.Id == id && p.ResourceId == resourceId).Count() > 0;
        }

        /// <summary>
        /// 获取工位物料
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>bool</returns>
        public virtual bool IsExistsStationItem(double productId, double stationId)
        {
            return Query<StationItem>().Where(p => p.ItemId == productId && p.StationId == stationId).FirstOrDefault() != null;
        }

        /// <summary>
        /// 获取工位工序
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>bool</returns>
        public virtual bool IsExistsStationProcess(double processId, double stationId)
        {
            return Query<StationProcess>().Where(p => p.ProcessId == processId && p.StationId == stationId).FirstOrDefault() != null;
        }

        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null) return true;
            return Query<Station>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 获取有有工位关联的资源
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="pageInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>资源</returns>
        public virtual EntityList<WipResource> GetWipResourceByStation(double processId, PagingInfo pageInfo, string keyword)
        {
            var query = Query<WipResource>().Exists<EmployeeResource>((x, y) => y.Where(e => e.ResourceId == x.Id && e.EmployeeId == RT.IdentityId))
                .Where(p => p.SourceType == SyncSourceType.Enterprise && (p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Stop || p.ResourceState == ResourceState.Unused));
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.Exists<Station>((x, y) => y.Join<StationProcess>((s, p) => s.Id == p.StationId && p.ProcessId == processId).Where(e => e.ResourceId == x.Id)).ToList(pageInfo);
        }

        /// <summary>
        /// 获取工位物料信息
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>工位物料信息</returns>
        public virtual StationItem GetStationItem(double itemId, double stationId)
        {
            return Query<StationItem>().Where(p => p.ItemId == itemId && p.StationId == stationId).FirstOrDefault();
        }

        /// <summary>
        /// 获取工位下的工序列表
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="elo"></param>
        /// <returns>工位下的工序列表</returns>
        public virtual EntityList<StationProcess> GetStationProcess(double stationId, EagerLoadOptions elo = null)
        {
            return Query<StationProcess>().Where(p => p.StationId == stationId).ToList(null, elo);
        }

        /// <summary>
        /// 根据工序Id获取工位工序列表
        /// </summary>
        /// <param name="processId">工位ID</param>
        /// <param name="elo"></param>
        /// <returns>工位下的工序列表</returns>
        public virtual EntityList<StationProcess> GetStationProcessByProcessId(double processId, EagerLoadOptions elo = null)
        {
            return Query<StationProcess>().Where(p => p.ProcessId == processId).ToList(null, elo);
        }


        /// <summary>
        /// 获取工位下的设备列表
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <param name="elo"></param>
        /// <returns>工位下的设备列表</returns>
        public virtual EntityList<StationEquipment> GetStationEquipments(double stationId, EagerLoadOptions elo = null)
        {
            return Query<StationEquipment>().Where(p => p.StationId == stationId).ToList(null, elo);
        }

        /// <summary>
        /// 设置主设备
        /// </summary>
        /// <param name="stationEquipment"></param>
        public virtual void SetEquipmentMaster(StationEquipment stationEquipment)
        {
            using (var trans = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                DB.Update<StationEquipment>().Set(p => p.IsMaster, false).Where(p => p.StationId == stationEquipment.StationId && p.IsMaster).Execute();
                DB.Update<StationEquipment>().Set(p => p.IsMaster, true).Where(p => p.Id == stationEquipment.Id).Execute();
                trans.Complete();
            }
        }
    }
}