using SIE.Common.Import;
using SIE.Core.Boxs;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台控制器
    /// </summary>
    public partial class StationController : DomainController
    {
        /// <summary>
        /// 获取站台
        /// </summary>
        /// <param name="code">路径Id</param>    
        /// <param name="whId">仓库Id</param>
        /// <returns>站台</returns>
        public virtual Station GetStationByCode(double whId, string code)
        {
            return Query<Station>().Where(p => p.Code == code && p.WarehouseId == whId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询站台
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<Station> GetStations(StationCriteria criteria)
        {
            var query = Query<Station>();
            ////增加仓库权限关联查询
            RT.Service.Resolve<WarehouseController>().ExistWarehouseEmplyee(query, Station.WarehouseIdProperty);

            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State.Value);
            if (criteria.StationType.HasValue)
                query.Where(p => p.StationType == criteria.StationType.Value);
            if (criteria.WarehouseId.HasValue)
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            if (criteria.RoutewayId.HasValue)
                query.Where(p => p.RoutewayId == criteria.RoutewayId.Value);
            if (criteria.LedId.HasValue)
                query.Where(p => p.LedId == criteria.LedId.Value);

            if (criteria.IsInStation)
                query.Where(p => p.StationType == StationType.InStation || p.StationType == StationType.InAndOutStation);

            if (criteria.IsOutStation)
                query.Where(p => p.StationType == StationType.OutStation || p.StationType == StationType.InAndOutStation);

            if (criteria.IsPickStation)
                query.Where(p => p.StationType == StationType.PickingStation || p.StationType == StationType.InventoryStation);

            if (criteria.IsCountStation)
                query.Where(p => p.StationType == StationType.PickingStation || p.StationType == StationType.InventoryStation);

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询站台
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>站台</returns>
        public virtual EntityList<Station> GetStations(double wareHouseId, string keyword, PagingInfo info)
        {
            var query = Query<Station>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询站台
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="queryAction">查询委托</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>站台</returns>
        public virtual EntityList<Station> GetStations(double wareHouseId, Action<IEntityQueryer<Station>> queryAction, EagerLoadOptions elo = null)
        {
            var query = Query<Station>().Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            queryAction?.Invoke(query);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 查询上架策略特定类型站台（入库站台、双向出入站台、关键节点站台、缓存站台）
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <param name="isFrom">是否来源站台</param>
        /// <returns>站台</returns>
        public virtual EntityList<Station> GetStrategyStations(double wareHouseId, string keyword, PagingInfo info, bool isFrom)
        {
            var query = Query<Station>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            List<StationType> stationTypes = new List<StationType>()
            {
                 StationType.InStation,
                 StationType.InAndOutStation,
                 StationType.KeyNodeStation,
                 StationType.CacheStation,
            };
            if (isFrom)
            {
                stationTypes.Add(StationType.InventoryStation);
                stationTypes.Add(StationType.PickingStation);
            }
            query.Where(p => stationTypes.Contains(p.StationType));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取站台
        /// </summary>
        /// <param name="whId">仓库ID</param>
        /// <param name="codes">站台编码</param>
        /// <returns>站台</returns>
        public virtual EntityList<Station> GetStationCountByCodes(double whId, List<string> codes)
        {
            return codes.SplitContains(tempCodes =>
            {
                var query = Query<Station>().Where(p => p.WarehouseId == whId);
                query.Where(p => tempCodes.Contains(p.Code));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取所有站台数据
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <returns>站台数据</returns>
        public virtual EntityList<Station> GetAllStations(double whId)
        {
            return Query<Station>().Where(p => p.State == State.Enable && p.WarehouseId == whId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取出库站台数据
        /// </summary>
        /// <param name="whId">仓库Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>站台数据</returns>
        public virtual EntityList<Station> GetOutStorageStations(double whId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Station>().Where(p => p.State == State.Enable && p.WarehouseId == whId && (p.StationType == StationType.OutStation || p.StationType == StationType.InAndOutStation));

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取仓库下的站台数据
        /// </summary>
        /// <param name="whIds">仓库Id集合</param>
        /// <param name="type">类型</param>
        /// <returns>站台数据</returns>
        public virtual EntityList<Station> GetStationByWarehouse(List<double> whIds, StationType? type = null)
        {
            var query = Query<Station>().Where(p => p.State == State.Enable);
            if (whIds.Any())
            {
                query.Where(p => whIds.Contains(p.WarehouseId));
            }

            if (type.HasValue)
            {
                query.Where(p => p.StationType == type.Value);
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据站台配置属性获取站台（不包含当前站台ID）
        /// </summary>
        /// <param name="curStationId">当前站台ID</param>
        /// <param name="opcScannerNo">条码枪OPC编号</param>
        /// <param name="opcSerialNo">站台OPC编号</param>
        /// <param name="opcSubTrayScannerNo">子托盘OPC编号</param>
        /// <returns></returns>
        public virtual int GetStationCoutByPropery(double curStationId, int? opcScannerNo, int? opcSerialNo, int? opcSubTrayScannerNo)
        {
            var query = Query<Station>().Where(p => p.Id != curStationId);
            query.WhereIf(opcScannerNo.HasValue, p => p.OpcScannerNo == opcScannerNo);
            query.WhereIf(opcSerialNo.HasValue, p => p.OpcSerialNo == opcSerialNo);
            query.WhereIf(opcSubTrayScannerNo.HasValue, p => p.OpcSubTrayScannerNo == opcSubTrayScannerNo);
            return query.Count();
        }

        /// <summary>
        /// 获取站台
        /// </summary>
        /// <param name="code">编码</param>       
        /// <param name="warehouseId">仓库</param>
        /// <returns>站台</returns>
        public virtual StationGroup GetStationGroupByCode(string code, double warehouseId)
        {
            return Query<StationGroup>().Where(p => p.Code == code && p.WarehouseId == warehouseId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取站台组数据
        /// </summary>
        /// <param name="warehouseCode">仓库编码</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>站台组数据</returns>
        public virtual EntityList<StationGroup> GetStationGroupByTypes(string warehouseCode, PagingInfo pagingInfo, string keyWord)
        {
            var query = Query<StationGroup>().Where(p => p.State == State.Enable);
            query.Where(p => p.StationGroupType == StationGroupType.FlatLibrary || p.StationGroupType == StationGroupType.RollerLine);
            if (warehouseCode.IsNotEmpty())
            {
                query.Exists<Warehouse>((p, w) => w.Where(t => t.Id == p.WarehouseId && t.Code.Contains(warehouseCode)));
            }

            if (keyWord.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取站台组数据
        /// </summary>
        /// <param name="whIds">仓库Id集合</param>
        /// <param name="models">型号集合</param>
        /// <returns>站台组数据</returns>
        public virtual EntityList<StationGroup> GetOutAndPickingStationGroups(List<double> whIds, List<string> models)
        {
            var query = Query<StationGroup>().Where(p => p.State == State.Enable && (p.StationGroupType == StationGroupType.Out || p.StationGroupType == StationGroupType.Picking));
            if (whIds.Any())
            {
                query.Where(p => whIds.Contains(p.WarehouseId));
            }

            if (models.Any())
            {
                query.Exists<TurnoverBoxModel>((p, w) => w.Where(t => t.Id == p.TurnoverBoxModelId && models.Contains(t.Code)));
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取站台组数据
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>站台组数据</returns>
        public virtual EntityList<StationGroup> GetStationGroups(StationGroupCriteria criteria)
        {
            var query = Query<StationGroup>();
            if (criteria.WarehouseCode.IsNotEmpty())
            {
                query.Where(p => p.Warehouse.Code.Contains(criteria.WarehouseCode));
            }
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.Location.IsNotEmpty())
            {
                query.Where(p => p.Location.Contains(criteria.Location));
            }
            if (criteria.IsCount)
            {
                query.Where(p => p.StationGroupType == StationGroupType.FlatLibrary || p.StationGroupType == StationGroupType.RollerLine);
            }
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询站台组
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="queryAction">查询委托</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>站台</returns>
        public virtual EntityList<StationGroup> GetStationGroups(double wareHouseId, Action<IEntityQueryer<StationGroup>> queryAction, EagerLoadOptions elo = null)
        {
            var query = Query<StationGroup>().Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            queryAction?.Invoke(query);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取站台组的数据
        /// </summary>
        /// <param name="wareHouseId">仓库Id</param>
        /// <param name="model">托盘型号编码</param>
        /// <param name="stationGroupType">站台组类型</param>
        /// <returns>站台组数据</returns>
        public virtual EntityList<StationGroup> GetStationGroups(double wareHouseId, string model, StationGroupType? stationGroupType)
        {
            var query = Query<StationGroup>().Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);

            if (!string.IsNullOrEmpty(model))
            {
                query.Exists<TurnoverBoxModel>((p, w) => w.Where(t => t.Id == p.TurnoverBoxModelId && t.Code == model));
            }
            if (stationGroupType.HasValue)
            {
                query.Where(p => p.StationGroupType == stationGroupType);
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询站台组
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="stationGroupType">站台组类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页</param>
        /// <returns>站台</returns>
        public virtual EntityList<StationGroup> GetStationGroups(double wareHouseId, StationGroupType? stationGroupType, string keyword, PagingInfo info)
        {
            var query = Query<StationGroup>();
            query.Where(p => p.WarehouseId == wareHouseId && p.State == State.Enable);
            if (stationGroupType.HasValue)
            {
                query.Where(p => p.StationGroupType == stationGroupType);
            }
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取站台组
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="codes">编码</param>
        /// <param name="model">托盘型号</param>
        /// <returns>站台组</returns>
        public virtual EntityList<StationGroup> GetStationGroups(double warehouseId, List<string> codes, string model = "", string keyword = "")
        {
            var query = Query<StationGroup>().Where(p => codes.Contains(p.Code) && p.WarehouseId == warehouseId);
            if (model.IsNotEmpty())
            {
                query.Where(p => p.TurnoverBoxModel.Code == model);
            }
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains("%" + keyword + "%") || p.Name.Contains("%" + keyword + "%"));
            }
            return query.ToList();
        }

        /// <summary>
        /// 这里是重新定义了框架默认的导入保存的方法（为了支持导入的数据只要有失败就回滚全部）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual List<ImportMessageResult> ImportSave(IList<RowData> data)
        {
            List<ImportMessageResult> list = new List<ImportMessageResult>();
            EntityRepository entityRepository = null;
            double num = 0.0;
            double num2 = 0.0;
            try
            {   //增加事务回滚数据
                using (var tran = DB.TransactionScope(WareHouseEntityDataProvider.ConnectionStringName))
                {
                    bool err = false;//记录是否有错误
                    foreach (RowData datum in data)
                    {
                        try
                        {
                            if (entityRepository == null)
                            {
                                entityRepository = (datum.Entity.GetRepository() as EntityRepository);
                                num = entityRepository.GetDoubleKeyNextId();
                            }

                            if (datum.Entity.PersistenceStatus == PersistenceStatus.New && !datum.Entity.HasId)
                            {
                                if (num2 > 9999.0)
                                {
                                    num2 = 0.0;
                                    num = entityRepository.GetDoubleKeyNextId();
                                }

                                datum.Entity.SetId(num + num2 / 100000.0);
                                num2 += 1.0;
                            }
                            var station = datum.Entity as Station;
                            station.State = State.Enable;//这里同时要写可用
                            entityRepository.Save(station);
                            list.Add(new ImportMessageResult
                            {
                                RowNum = datum.RowIndex + 1,
                                MsgType = ImportMessageType.SaveSucess,
                                Message = "保存成功！".L10N()
                            });
                        }
                        catch (Exception ex)
                        {
                            list.Add(new ImportMessageResult
                            {
                                RowNum = datum.RowIndex + 1,
                                MsgType = ImportMessageType.SaveFail,
                                Message = ex.GetBaseException().Message
                            });
                            err = true;
                        }
                    }
                    if (err)
                    {//这里用途是为了回滚数据
                        throw new ValidationException("error");
                    }

                    tran.Complete();
                }
            }
            catch
            {//吃掉错误error，不用报出到前端

            }
            return list;
        }
    }
}
