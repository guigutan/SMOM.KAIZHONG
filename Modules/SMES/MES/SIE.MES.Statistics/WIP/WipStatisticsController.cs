using Newtonsoft.Json;
using SIE.Common;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.EventMessages.Release;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.Statistics.Entities;
using SIE.MES.Statistics.Fpy;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 在制品统计控制器
    /// </summary>
    public partial class WipStatisticsController : DomainController, IProcessStatistics
    {
        private const string DOUBLE_PARAM_FORMAT = "{0}{1}";

        /// <summary>
        /// 生产数据
        /// </summary>
        private static Dictionary<string, string> _collectedData = new Dictionary<string, string>();

        /// <summary>
        /// 资源-车间
        /// </summary>
        private static Dictionary<double, double> _resourceDic = new Dictionary<double, double>();

        /// <summary>
        /// 生产数据采集
        /// </summary>
        /// <param name="data">采集数据</param>
        public virtual void ProductCollected(WipData data)
        {
            WipData wipData = PrepareWipData(data);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var stationRecord = GetStationStatistics(wipData);
                if (stationRecord == null)
                {
                    CreateStationStatistics(wipData); ////创建工位采集记录
                }
                else
                {
                    UpdateStationStatistics(wipData, stationRecord);  ////更新工位采集记录
                }
                ProcessRecord(wipData);   ////更新工序采集记录
                WorkOrderRecord(wipData);  ////更新工单采集记录
                tran.Complete();
            }
        }

        /// <summary>
        /// 包装采集app获取当班采集数量
        /// </summary>
        /// <param name="recourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual decimal GetCollectQty(double recourceId, double processId, double stationId, double workOrderId)
        {
            StatisticsQueryInfo info = new StatisticsQueryInfo
            {
                ResourceId = recourceId,
                ProcessId = processId,
                StationId = stationId,
                OperatorId = RT.IdentityId,
                WorkOrderId = workOrderId,
            };
            var stationCollectEvent = GetStationCollected(info);
            if (stationCollectEvent != null)
            {
                return stationCollectEvent.QtyPass + stationCollectEvent.QtyFailed;

            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 获取当前工位采集数
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual StationCollectedEvent GetStationCollectedQty(StatisticsQueryInfo info)
        {
            try
            {
                if (info.ResourceId == 0 || info.OperatorId == 0 || info.ProcessId == 0 || info.StationId == 0)
                    return new StationCollectedEvent();
                var collectDate = RF.Find<StationStatistics>().GetDbTime();
                var query = Query<StationStatistics>()
                    .Where(p => p.ResourceId == info.ResourceId
                    && p.ProcessId == info.ProcessId
                    && p.StationId == info.StationId
                    && p.OperatorId == info.OperatorId
                    && p.CollectDate == collectDate.Date)
                    .GroupBy(p => p.StationId)
                    .Select(p => new
                    {
                        p.StationId,
                        QtyTimes = p.QtyTimes.SUM(),
                        QtyFaildTimes = p.QtyFailedTimes.SUM(),
                        QtyPass = p.QtyPass.SUM(),
                        QtyFailed = p.QtyFailed.SUM()
                    });
                if (info.WorkOrderId.HasValue)
                {
                    query.Where(p => p.WorkOrderId == info.WorkOrderId);
                }
                return query.FirstOrDefault<StationCollectedEvent>();
            }
            catch (Exception exc)
            {
                Logging.LogManager.GetLogger("error_logger")?.Error($"工位采集数获取异常 {RT.InvOrg}".L10N(), exc);
                return new StationCollectedEvent();
            }
        }





        /// <summary>
        /// 获取当班统计数
        /// </summary>
        /// <param name="info">统计查询</param>
        /// <returns>统计结果</returns>
        public virtual StationCollectedEvent GetStationCollected(StatisticsQueryInfo info)
        {
            try
            {
                if (info.ResourceId == 0 || info.OperatorId == 0 || info.ProcessId == 0 || info.StationId == 0)
                    return new StationCollectedEvent();
                var collectDate = RF.Find<StationStatistics>().GetDbTime();
                var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(info.ResourceId, collectDate);
                DateTime shiftDate = RT.Service.Resolve<ShiftTypeController>().GetShiftDate(shift, collectDate);
                var query = Query<StationStatistics>()
                    .Where(p => p.ResourceId == info.ResourceId
                    && p.ProcessId == info.ProcessId
                    && p.StationId == info.StationId
                    && p.OperatorId == info.OperatorId
                    && p.ShiftId == shift.Id
                    && p.CollectDate == collectDate.Date
                    && p.ShiftDate == shiftDate)
                    .GroupBy(p => p.StationId)
                    .Select(p => new
                    {
                        p.StationId,
                        QtyTimes = p.QtyTimes.SUM(),
                        QtyFaildTimes = p.QtyFailedTimes.SUM(),
                        QtyPass = p.QtyPass.SUM(),
                        QtyFailed = p.QtyFailed.SUM()
                    });
                if (info.WorkOrderId.HasValue)
                {
                    query.Where(p => p.WorkOrderId == info.WorkOrderId);
                }
                return query.FirstOrDefault<StationCollectedEvent>();
            }
            catch (Exception exc)
            {
                Logging.LogManager.GetLogger("error_logger")?.Error($"工位当班采集数获取异常 {RT.InvOrg}".L10N(), exc);
                return new StationCollectedEvent();
            }
        }

        /// <summary>
        /// 获取工序统计
        /// </summary>
        /// <param name="info">工序统计数据查询条件</param>
        /// <returns>工位统计</returns>
        public virtual ProcessCollectedEvent GetProcessCollected(ProcessStatisticsQuery info)
        {
            try
            {
                var query = Query<StationStatistics>()
                .Where(p => p.ProcessId == info.ProcessId
                && p.ResourceId == info.ResourceId
                && p.WorkOrderId == info.WorkOrderId)
                .GroupBy(p => p.ProcessId)
                .Select(p => new
                {
                    ProcessId = p.ProcessId,
                    QtyTimes = p.QtyTimes.SUM(),
                    QtyFaildTimes = p.QtyFailedTimes.SUM(),
                    QtyPass = p.QtyPass.SUM(),
                    QtyFailed = p.QtyFailed.SUM()
                });
                return query.FirstOrDefault<ProcessCollectedEvent>();
            }
            catch (Exception exc)
            {
                Logging.LogManager.GetLogger("error_logger")?.Error($"工序采集数获取异常 {RT.InvOrg}".L10N(), exc);
                return new ProcessCollectedEvent();
            }
        }

        /// <summary>
        /// 工单采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param>
        private void WorkOrderRecord(WipData wipData)
        {
            var workOrderRecord = GetWorkOrderStatistics(wipData);
            if (workOrderRecord == null)
            {
                CreateWorkOrderStatistics(wipData);
            }
            else
            {
                UpdateWorkOrderStatistics(wipData, workOrderRecord);
            }
        }

        /// <summary>
        /// 更新工单采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <param name="workOrderRecord">工单采集记录</param> 
        private void UpdateWorkOrderStatistics(WipData wipData, WorkOrderStatisticsInfo workOrderRecord)
        {
            ////workOrderRecord.QtyPass += wipData.QtyPass;
            ////workOrderRecord.QtyFailed += wipData.QtyFailed;
            ////workOrderRecord.QtyOnline += wipData.QtyOnline;
            ////var curDateTime = RF.Find<WorkOrderStatisticsInfo>().GetDbTime();
            ////workOrderRecord.EndCollectTime = curDateTime;
            ////workOrderRecord.PersistenceStatus = PersistenceStatus.Modified;
            ////RF.Save(workOrderRecord);

            var curDateTime = RF.Find<WorkOrderStatisticsInfo>().GetDbTime();
            DB.Update<WorkOrderStatisticsInfo>()
                .Set(p => p.QtyPass, p => p.QtyPass + wipData.QtyPass)
                .Set(p => p.QtyFailed, p => p.QtyFailed + wipData.QtyFailed)
                .Set(p => p.QtyOnline, p => p.QtyOnline + wipData.QtyOnline)
                .Set(p => p.EndCollectTime, curDateTime)
                .Where(p => p.Id == workOrderRecord.Id).Execute();
        }

        /// <summary>
        /// 创建工单采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param> 
        private void CreateWorkOrderStatistics(WipData wipData)
        {
            var workOrderStatistics = new WorkOrderStatisticsInfo()
            {
                InvOrgId = wipData.InvOrgId,
                WorkOrderId = wipData.WorkOrderId,
                WorkOrderNo = wipData.WorkOrderNo,
                CustomerName = wipData.CustomerName,
                WorkShopId = wipData.WorkShopId,
                WorkShopName = wipData.WorkShopName,
                ResourceId = wipData.ResourceId,
                ResourceName = wipData.ResourceName,
                ProductId = wipData.ProductId,
                ProductName = wipData.ProductName,
                ShiftId = wipData.ShiftId,
                ShiftName = wipData.ShiftName,
                CollectDate = wipData.CollectDate,
                ShiftDate = wipData.ShiftDate,
                Hour = wipData.Hour
            };
            workOrderStatistics.QtyPass = wipData.QtyPass;
            workOrderStatistics.QtyFailed = wipData.QtyFailed;
            workOrderStatistics.QtyOnline = wipData.QtyOnline;
            var curDateTime = RF.Find<WorkOrderStatisticsInfo>().GetDbTime();
            workOrderStatistics.BeginCollectTime = curDateTime;
            workOrderStatistics.EndCollectTime = curDateTime;
            workOrderStatistics.PersistenceStatus = PersistenceStatus.New;
            RF.Save(workOrderStatistics);
        }

        /// <summary>
        /// 工序采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param> 
        private void ProcessRecord(WipData wipData)
        {
            var processRecord = GetProcessStatistics(wipData);
            if (processRecord == null)
            {
                CreateProcessStatistics(wipData);
            }
            else
            {
                UpdateProcessStatistics(wipData, processRecord);
            }
        }

        /// <summary>
        /// 更新工序采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <param name="processRecord">工序采集记录</param> 
        private void UpdateProcessStatistics(WipData wipData, ProcessStatisticsInfo processRecord)
        {
            ////processRecord.QtyPass += wipData.WipProcess.QtyPass;
            ////processRecord.QtyFailed += wipData.WipProcess.QtyFailed;
            ////processRecord.QtyMove += wipData.WipProcess.QtyMove;
            ////processRecord.PersistenceStatus = PersistenceStatus.Modified;
            ////RF.Save(processRecord);
            DB.Update<ProcessStatisticsInfo>()
                .Set(p => p.QtyPass, p => p.QtyPass + wipData.WipProcess.QtyPass)
                .Set(p => p.QtyFailed, p => p.QtyFailed + wipData.WipProcess.QtyFailed)
                .Set(p => p.QtyMove, p => p.QtyMove + wipData.WipProcess.QtyMove)
                .Where(p => p.Id == processRecord.Id).Execute();
        }

        /// <summary>
        /// 创建工序采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param> 
        private void CreateProcessStatistics(WipData wipData)
        {
            var processStatistics = new ProcessStatisticsInfo()
            {
                InvOrgId = wipData.InvOrgId,
                WorkOrderId = wipData.WorkOrderId,
                WorkOrderNo = wipData.WorkOrderNo,
                WorkShopId = wipData.WorkShopId,
                WorkShopName = wipData.WorkShopName,
                ResourceId = wipData.ResourceId,
                ResourceName = wipData.ResourceName,
                ProcessId = wipData.ProcessId,
                ProcessName = wipData.ProcessName,
                ShiftId = wipData.ShiftId,
                ShiftName = wipData.ShiftName,
                CollectDate = wipData.CollectDate,
                ShiftDate = wipData.ShiftDate,
                ProductId = wipData.ProductId,
                ProductName = wipData.ProductName,
                ProcessIndex = wipData.ProcessIndex,
            };
            processStatistics.QtyPass = wipData.WipProcess.QtyPass;
            processStatistics.QtyFailed = wipData.WipProcess.QtyFailed;
            processStatistics.QtyMove = wipData.WipProcess.QtyMove;
            processStatistics.PersistenceStatus = PersistenceStatus.New;
            RF.Save(processStatistics);
        }

        /// <summary>
        /// 更新工位采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <param name="stationStatistics">工位采集记录</param> 
        private void UpdateStationStatistics(WipData wipData, StationStatisticsInfo stationStatistics)
        {
            ////stationStatistics.QtyPass += wipData.WipStation.QtyPass;
            ////stationStatistics.QtyFailed += wipData.WipStation.QtyFailed;
            ////stationStatistics.QtyTimes += wipData.WipStation.QtyTimes;
            ////stationStatistics.QtyFailedTimes += wipData.WipStation.QtyFailedTimes;
            ////stationStatistics.QtyMove += wipData.WipStation.QtyMove;
            ////stationStatistics.PersistenceStatus = PersistenceStatus.Modified;
            ////RF.Save(stationStatistics);
            DB.Update<StationStatisticsInfo>()
                .Set(p => p.QtyPass, p => p.QtyPass + wipData.WipStation.QtyPass)
                .Set(p => p.QtyFailed, p => p.QtyFailed + wipData.WipStation.QtyFailed)
                .Set(p => p.QtyTimes, p => p.QtyTimes + wipData.WipStation.QtyTimes)
                .Set(p => p.QtyFailedTimes, p => p.QtyFailedTimes + wipData.WipStation.QtyFailedTimes)
                .Set(p => p.QtyMove, p => p.QtyMove + wipData.WipStation.QtyMove)
                .Where(p => p.Id == stationStatistics.Id).Execute();
        }

        /// <summary>
        /// 创建工位采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param> 
        private void CreateStationStatistics(WipData wipData)
        {
            var stationStatistics = new StationStatisticsInfo()
            {
                InvOrgId = wipData.InvOrgId,
                WorkOrderId = wipData.WorkOrderId,
                WorkOrderNo = wipData.WorkOrderNo,
                WorkShopId = wipData.WorkShopId,
                WorkShopName = wipData.WorkShopName,
                ResourceId = wipData.ResourceId,
                ResourceName = wipData.ResourceName,
                ProcessId = wipData.ProcessId,
                ProcessName = wipData.ProcessName,
                StationId = wipData.StationId,
                StationName = wipData.StationName,
                EquipmentId = wipData.EquipmentId,
                EquipmentName = wipData.EquipmentName,
                ProductId = wipData.ProductId,
                ProductName = wipData.ProductName,
                OperatorId = wipData.OperatorId,
                OperatorName = wipData.OperatorName,
                ShiftId = wipData.ShiftId,
                ShiftName = wipData.ShiftName,
                ShiftDate = wipData.ShiftDate,
                CollectDate = wipData.CollectDate
            };
            stationStatistics.QtyPass = wipData.WipStation.QtyPass;
            stationStatistics.QtyFailed = wipData.WipStation.QtyFailed;
            stationStatistics.QtyTimes = wipData.WipStation.QtyTimes;
            stationStatistics.QtyFailedTimes = wipData.WipStation.QtyFailedTimes;
            stationStatistics.QtyMove = wipData.WipStation.QtyMove;
            stationStatistics.PersistenceStatus = PersistenceStatus.New;
            RF.Save(stationStatistics);
        }

        /// <summary>
        /// 准备采集数据
        /// </summary>
        /// <param name="wipData">采集完成事件</param>
        /// <returns>采集数据</returns>
        private WipData PrepareWipData(WipData wipData)
        {
            wipData.OperatorName = GetOperatorName(wipData);
            wipData.ResourceName = GetResourceName(wipData);
            wipData.WorkShopName = GetWorkShopName(wipData);
            wipData.ProcessName = GetProcessName(wipData);
            wipData.StationName = GetStationName(wipData);
            wipData.EquipmentName = GetEquipmentName(wipData);
            return wipData;
        }

        /// <summary> 
        /// 获取工位统计记录
        /// 库存组织、工单、产品、车间、产线、工序、工位、设备班次、采集日期、班制日期确定唯一
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工位统计记录</returns>
        private StationStatisticsInfo GetStationStatistics(WipData wipData)
        {
            return Query<StationStatisticsInfo>()
                .Where(p => p.InvOrgId == wipData.InvOrgId)
                .Where(p => p.WorkOrderId == wipData.WorkOrderId && p.ProductId == wipData.ProductId)
                .Where(p => p.WorkShopId == wipData.WorkShopId && p.ResourceId == wipData.ResourceId)
                .Where(p => p.ProcessId == wipData.ProcessId && p.StationId == wipData.StationId && p.EquipmentId == wipData.EquipmentId)
                .Where(p => p.ShiftId == wipData.ShiftId && p.CollectDate == wipData.CollectDate && p.ShiftDate == wipData.ShiftDate)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工单采集记录
        /// 库存组织、工单、产品、车间、产线、班次、采集日期、班制日期确定唯一
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工单采集记录</returns>
        private WorkOrderStatisticsInfo GetWorkOrderStatistics(WipData wipData)
        {
            return Query<WorkOrderStatisticsInfo>()
                .Where(p => p.InvOrgId == wipData.InvOrgId)
                .Where(p => p.WorkOrderId == wipData.WorkOrderId && p.ProductId == wipData.ProductId)
                .Where(p => p.WorkShopId == wipData.WorkShopId && p.ResourceId == wipData.ResourceId)
                .Where(p => p.ShiftId == wipData.ShiftId && p.CollectDate == wipData.CollectDate && p.ShiftDate == wipData.ShiftDate && p.Hour == wipData.Hour)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工序采集记录
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工序采集记录</returns>
        private ProcessStatisticsInfo GetProcessStatistics(WipData wipData)
        {
            return Query<ProcessStatisticsInfo>()
                .Where(p => p.InvOrgId == wipData.InvOrgId
                    && p.WorkOrderId == wipData.WorkOrderId
                    && p.ProductId == wipData.ProductId
                    && p.WorkShopId == wipData.WorkShopId
                    && p.ResourceId == wipData.ResourceId
                    && p.ProcessId == wipData.ProcessId
                    && p.ProcessIndex == wipData.ProcessIndex
                    && p.ShiftId == wipData.ShiftId
                    && p.CollectDate == wipData.CollectDate
                    && p.ShiftDate == wipData.ShiftDate)
                .FirstOrDefault();
        }

        #region 生产数据操作
        /// <summary>
        /// 获取操作人
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>操作人</returns>
        [IgnoreProxy]
        private string GetOperatorName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.OperatorId), wipData.OperatorId);
            if (TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                var user = RF.GetById<Employee>(wipData.OperatorId);
                if (user == null)
                {
                    throw new EntityNotFoundException(typeof(Employee), wipData.OperatorId);
                }

                value = user.Name;
                AddValue(key, value);
            }

            return value;
        }

        /// <summary>
        /// 获取资源名称
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>资源名称</returns>
        [IgnoreProxy]
        internal virtual string GetResourceName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.ResourceId), wipData.ResourceId);
            if (TryGetValue(key, out value))
            {
                return value;
            }

            var resource = RF.GetById<WipResource>(wipData.ResourceId);
            if (resource == null)
            {
                throw new EntityNotFoundException(typeof(WipResource), wipData.ResourceId);
            }

            value = resource.Name;
            AddValue(key, value);
            if (!resource.WorkShopId.HasValue)
            {
                throw new ValidationException("未找到资源[{0}]对应车间".L10nFormat(resource.Name));
            }

            if (!_resourceDic.ContainsKey(resource.WorkShopId.Value))
            {
                _resourceDic.Add(wipData.ResourceId, resource.WorkShopId.Value);
            }

            return value;
        }

        /// <summary>
        /// 获取车间ID
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>车间ID</returns>
        [IgnoreProxy]
        internal virtual double GetWorkShopId(double resourceId)
        {
            double workShopId = 0;
            if (!_resourceDic.TryGetValue(resourceId, out workShopId))
            {
                var resource = RF.GetById<WipResource>(resourceId);
                if (resource == null)
                {
                    throw new EntityNotFoundException(typeof(WipResource), resourceId);
                }

                string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(WipData.ResourceId), resourceId);
                AddValue(key, resource.Name);
                workShopId = resource.WorkShopId.HasValue ? resource.WorkShopId.Value : 0;
                _resourceDic.Add(resourceId, workShopId);
            }

            return workShopId;
        }

        /// <summary>
        /// 获取车间名称
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>车间名称</returns>
        [IgnoreProxy]
        internal virtual string GetWorkShopName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.WorkShopId), wipData.WorkShopId);
            if (TryGetValue(key, out value))
            {
                return value;
            }
            var workShop = RF.GetById<Enterprise>(wipData.WorkShopId);
            if (workShop == null)
            {
                throw new EntityNotFoundException(typeof(Enterprise), wipData.WorkShopId);
            }

            value = workShop.Name;
            AddValue(key, value);

            return value;
        }

        /// <summary>
        /// 获取工序名称
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工序名称</returns>
        [IgnoreProxy]
        internal virtual string GetProcessName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.ProcessId), wipData.ProcessId);
            if (TryGetValue(key, out value))
            {
                return value;
            }
            var process = RF.GetById<Process>(wipData.ProcessId);
            if (process == null)
            {
                throw new EntityNotFoundException(typeof(Process), wipData.ProcessId);
            }

            value = process.Name;
            AddValue(key, value);

            return value;
        }

        /// <summary>
        /// 获取工位名称
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工位名称</returns>
        [IgnoreProxy]
        private string GetStationName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.StationId), wipData.StationId);
            if (TryGetValue(key, out value))
            {
                return value;
            }
            var station = RF.GetById<Station>(wipData.StationId);
            if (station == null)
            {
                throw new EntityNotFoundException(typeof(Station), wipData.StationId);
            }

            value = station.Name;
            AddValue(key, value);

            return value;
        }

        /// <summary>
        /// 获取工序名称
        /// </summary>
        /// <param name="wipData">采集数据</param>
        /// <returns>工序名称</returns>
        [IgnoreProxy]
        private string GetEquipmentName(WipData wipData)
        {
            string value = string.Empty;
            string key = DOUBLE_PARAM_FORMAT.FormatArgs(nameof(wipData.EquipmentId), wipData.EquipmentId);
            if (TryGetValue(key, out value))
            {
                return value;
            }
            var equipment = RF.GetById<EquipAccount>(wipData.EquipmentId);
            value = equipment?.Name;
            AddValue(key, value);

            return value;
        }

        /// <summary>
        /// 获取生产数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>存在值返回true，否则返回false</returns>
        private bool TryGetValue(string key, out string value)
        {
            return _collectedData.TryGetValue(key, out value);
        }

        /// <summary>
        /// 添加生产数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        private void AddValue(string key, string value)
        {
            if (!_collectedData.ContainsKey(key))
            {
                _collectedData.Add(key, value);
            }
        }
        #endregion

        #region 资源半小时采集统计
        /// <summary>
        /// 获取资源半小时采集统计信息
        /// </summary>
        /// <param name="resourceId">资源id</param>
        /// <param name="shiftDate">班次日期</param>
        /// <returns>当前班次资源统计信息</returns>
        private EntityList<ResourceStatisticsInfo> GetResourceStatistics(double resourceId, DateTime shiftDate)
        {
            var query = Query<ResourceStatisticsInfo>();
            query.Where(p => p.ResourceId == resourceId && p.ShiftDate == shiftDate.Date);
            query.Where(p => p.StartTime <= shiftDate && p.EndTime >= shiftDate);
            return query.ToList();
        }

        ///// <summary>
        ///// 获取资源半小时采集统计信息
        ///// </summary>
        ///// <param name="wipData">在制品采集后信息</param>
        ///// <param name="shiftDate">班次日期</param>
        ///// <returns>当前班次资源统计信息</returns>
        ////private EntityList<ResourceStatisticsInfo> GetResourceStatistics(WipCollectedEvent wipData, DateTime shiftDate)
        ////{
        ////    var query = Query<ResourceStatisticsInfo>();
        ////    query.Where(p => p.ResourceId == wipData.Data.Workcell.ResourceId && p.ShiftDate == shiftDate);
        ////    return query.ToList();
        ////}

        /// <summary>
        /// 初始化当天当前产线当前班制的统计时间段
        /// </summary>
        /// <param name="currentShift">班次</param>
        /// <param name="wipData">在制品采集后信息</param>
        /// <param name="currentDate">当前日期</param>
        /// <returns>当前班次资源统计信息</returns>
        private EntityList<ResourceStatisticsInfo> InitTimeRange(Shift currentShift, WipCollectedEvent wipData, DateTime currentDate)
        {
            DateRange dateRange = new DateRange();
            EntityList<ResourceStatisticsInfo> resStatisticsList = new EntityList<ResourceStatisticsInfo>();
            var type = currentShift.ShiftType;

            dateRange.BeginValue = DateTime.Now;
            dateRange.EndValue = DateTime.Now;
            var resourceInfo = GetResourceStatistics(wipData.Data.Workcell.ResourceId, currentDate);
            if (resourceInfo != null && resourceInfo.Count > 0)
            {
                return resourceInfo;
            }
            if (type.ShiftList.Count <= 0)
            {
                return new EntityList<ResourceStatisticsInfo>();
            }

            foreach (var shift in type.ShiftList)
            {
                DateTime begin = currentDate.Date.AddSeconds((shift.BeginTime - shift.BeginTime.Date).TotalSeconds);
                DateTime end = currentDate.Date.AddSeconds((shift.EndTime - shift.EndTime.Date).TotalSeconds);
                if (shift.IsOverDay)
                {
                    dateRange.EndValue = currentDate.Date.AddDays(1).AddSeconds((shift.EndTime - shift.EndTime.Date).TotalSeconds);
                }
                else
                {
                    if (dateRange.BeginValue > begin)
                    {
                        dateRange.BeginValue = begin;
                    }

                    if (dateRange.EndValue < end)
                    {
                        dateRange.EndValue = end;
                    }
                }
            }

            DateTime beginDate = dateRange.BeginValue.Value;
            while (beginDate <= dateRange.EndValue.Value)
            {
                resStatisticsList.Add(new ResourceStatisticsInfo()
                {
                    ResourceId = wipData.Data.Workcell.ResourceId,
                    CollectDate = currentDate,
                    OnlineQty = 0,
                    OfflineQty = 0,
                    NgQty = 0,
                    StartTime = beginDate,
                    EndTime = beginDate.AddMinutes(30)
                });
                beginDate = beginDate.AddMinutes(30).AddSeconds(1);
            }

            RF.Save(resStatisticsList);
            return resStatisticsList;
        }

        /// <summary>
        /// 更新资源半小时采集统计信息
        /// </summary>
        /// <param name="wipData">在制品采集后信息</param>
        /// <param name="list">统计信息集合</param>
        private void UpdateResourceStatistics(WipCollectedEvent wipData, EntityList<ResourceStatisticsInfo> list)
        {
            if (wipData.Data.CollectData.State == WipProductProcessState.Finish) //需要工序完成时才触发
            {
                decimal onlineQty = 0;
                decimal offlineQty = 0;
                decimal ngQty = 0;
                var record = list.FirstOrDefault(p => p.StartTime <= DateTime.Now && p.EndTime >= DateTime.Now);
                if (record == null)
                {
                    return;
                }

                if (wipData.Data.Product.Routing.Current.IsStart)
                {
                    //// 工序采集记录数量为1，则产品刚上线，上线数量+1，大于1则产品为维修跳站，上线数量不累加
                    if (RT.Service.Resolve<WipProductVersionController>().GetWipProductProcessCountBySn(wipData.Data.Barcodes[0].Code, wipData.Data.Barcodes[0].Type) == 1)
                    {
                        ////record.OnlineQty++;
                        onlineQty = onlineQty + 1;
                    }
                }

                if (wipData.Data.Product.Routing.Current.IsEnd && wipData.Data.CollectData.Result == ResultType.Pass)
                {
                    ////record.OfflineQty++;
                    offlineQty = offlineQty + 1;
                    var version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(new CollectBarcode { Code = wipData.Data.Barcodes[0].Code, Type = wipData.Data.Barcodes[0].Type });

                    if (version?.DefectList.Count > 0)
                    {
                        ///*r*/ecord.NgQty++;
                        ngQty = ngQty + 1;
                    }
                }

                ////RF.Save(record);
                DB.Update<ResourceStatisticsInfo>()
                    .Set(p => p.OnlineQty, p => p.OnlineQty + onlineQty)
                    .Set(p => p.OfflineQty, p => p.OfflineQty + offlineQty)
                    .Set(p => p.NgQty, p => p.NgQty + ngQty)
                    .Where(p => p.Id == record.Id).Execute();
            }
        }

        /// <summary>
        /// 资源半小时采集信息统计
        /// </summary>
        /// <param name="wipData">在制品采集后信息</param>
        public virtual void ResourceRecord(WipCollectedEvent wipData)
        {
            if (wipData == null)
            {
                return;
            }
            var currentDate = wipData.Data.CollectDate;
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(wipData.Data.Workcell.ResourceId, currentDate);

            var resStatisticsList = InitTimeRange(shift, wipData, currentDate);

            if (resStatisticsList != null && resStatisticsList.Count > 0)
            {
                UpdateResourceStatistics(wipData, resStatisticsList);
            }
        }
        #endregion

        #region 工作台数据统计
        /// <summary>
        /// 获取月度总量达成实际产量
        /// </summary>
        /// <param name="workshopId">车间Id</param>
        /// <param name="dbTime">数据库时间</param>
        /// <returns>月总量</returns>
        public virtual decimal GetMonthTotal(double workshopId, DateTime dbTime)
        {
            var curMonth = DateTime.Parse(dbTime.Year + "-" + dbTime.Month + "-01");
            var nexMonth = curMonth.AddMonths(1);
            DateRange dr = new DateRange() { BeginValue = curMonth, EndValue = nexMonth };
            var rst = GetMonthStatics(workshopId, dr).Sum(p => p.QtyPass);
            return rst;
        }

        /// <summary>
        /// 获取当前时间几个月的统计数据
        /// </summary>
        /// <param name="woList">工单列表Ids</param>
        /// <param name="dbTime">数据库时间</param>
        /// <param name="months">月数</param>
        /// <returns>统计数据</returns>
        public virtual EntityList<WorkOrderStatistics> GetMonthStatics(List<double> woList, DateTime dbTime, int months = 1)
        {
            var lastmonths = months - 1;
            var curMonth = DateTime.Parse(dbTime.AddMonths(-lastmonths).Year + "-" + dbTime.AddMonths(-lastmonths).Month + "-01");
            var nexMonth = curMonth.AddMonths(months);
            return Query<WorkOrderStatistics>().Where(p => p.CollectDate >= curMonth && p.CollectDate < nexMonth && woList.Contains(p.WorkOrderId)).ToList();
        }

        /// <summary>
        /// 根据时间范围获取实际产量
        /// </summary>
        /// <param name="workshopId">车间id</param>
        /// <param name="dr">时间范围</param>
        /// <returns>统计数据</returns>
        public virtual EntityList<WorkOrderStatistics> GetMonthStatics(double workshopId, DateRange dr)
        {
            return Query<WorkOrderStatistics>().Where(p => p.CollectDate >= dr.BeginValue && p.CollectDate < dr.EndValue && p.WorkShopId == workshopId).ToList();
        }

        /// <summary>
        /// 获取资源、班次、指定日期的产能
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="shiftId">班次Id</param>
        /// <param name="shiftDate">班次日期</param>
        /// <returns>工单采集集合</returns>
        public virtual EntityList<WorkOrderStatistics> GetWorkOrderStatics(double resourceId, double shiftId, DateTime shiftDate)
        {
            var querys = Query<WorkOrderStatistics>().Where(x => x.ResourceId == resourceId && x.ShiftId == shiftId && x.ShiftDate == shiftDate);
            return querys.ToList();
        }

        /// <summary>
        /// 获取工单的合格率
        /// </summary>
        /// <param name="woList">工单集合</param>
        /// <returns>统计工单合格率集合</returns>
        public virtual List<WorkOrderFpyStatistics> GetWorkOrderStatusStatics(List<double> woList)
        {
            List<WorkOrderStatistics> list = new List<WorkOrderStatistics>();
            list.AddRange(Query<WorkOrderStatistics>().Where(p => woList.Contains(p.WorkOrderId)).ToList());
            return list.GroupBy(p => new { p.WorkOrderId }).Select(
                g => new WorkOrderFpyStatistics
                {
                    WorkOrderId = g.Key.WorkOrderId,
                    Fpy = (g.Sum(p => p.QtyPass) + g.Sum(p => p.QtyFailed)) == 0 ? 0 : g.Sum(p => p.QtyPass) / (g.Sum(p => p.QtyPass) + g.Sum(p => p.QtyFailed))
                }).ToList();
        }
        #endregion

        #region 采集报错数据处理
        /// <summary>
        /// 生成预统计失败数据记录
        /// </summary>
        /// <param name="wipCollected">采集原始数据</param>
        public virtual void SaveFailWipCollected(WipCollected wipCollected)
        {
            WipCollectedFail fail = new WipCollectedFail()
            {
                Barcode = wipCollected.Barcode,
                BatchQty = wipCollected.BatchQty,
                CollectDate = wipCollected.CollectDate,
                CustomerName = wipCollected.CustomerName,
                EmployeeId = wipCollected.EmployeeId,
                ResourceId = wipCollected.ResourceId,
                StationId = wipCollected.StationId,
                EquipmentId = wipCollected.EquipmentId,
                SourceInvOrgId = wipCollected.InvOrgId,
                IsEnd = wipCollected.IsEnd,
                IsStart = wipCollected.IsStart,
                IsNg = wipCollected.IsNg,
                IsScrap = wipCollected.IsScrap,
                ModelId = wipCollected.ModelId,
                ModelName = wipCollected.ModelName,
                ProcessId = wipCollected.ProcessId,
                ProcessType = wipCollected.ProcessType,
                ProductId = wipCollected.ProductId,
                ProductName = wipCollected.ProductName,
                Qty = wipCollected.Qty,
                ScrapQty = wipCollected.ScrapQty,
                Result = wipCollected.Result,
                WorkOrderId = wipCollected.WorkOrderId,
                WorkOrderNo = wipCollected.WorkOrderNo,
            };
            int defectCount = wipCollected.DefectList.Count;
            string[] defectData = new string[defectCount];
            for (int i = 0; i < defectCount; i++)
            {
                if (wipCollected.DefectList[i] != null)
                {
                    defectData[i] = JsonConvert.SerializeObject(wipCollected.DefectList[i]);
                }
            }

            fail.Defect = string.Join("|", defectData);
            RF.Save(fail);
        }

        /// <summary>
        /// 获取采集失败的记录
        /// </summary>
        /// <returns>记录集合</returns>
        public virtual EntityList<WipCollectedFail> GetWipCollectedFail()
        {
            return Query<WipCollectedFail>().Where(p => p.SourceInvOrgId == RT.InvOrg).ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 删除失败的记录
        /// </summary>  
        /// <param name="id">记录ID</param>
        public virtual void DelWipCollectedFail(double id)
        {
            DB.Delete<WipCollectedFail>().Where(p => p.Id == id).Execute();
        }

        /// <summary>
        /// 保存生成统计数据用到的WipData
        /// </summary>
        /// <param name="data">采集预统计数据</param>
        public virtual void SaveFailWipData(WipData data)
        {
            if (data == null)
            {
                return;
            }
            WipDataFail fail = new WipDataFail()
            {
                ProcessIndex = data.ProcessIndex,
                Barcode = data.Barcode,
                WorkOrderId = data.WorkOrderId,
                WorkOrderNo = data.WorkOrderNo,
                CustomerId = data.CustomerId,
                CustomerName = data.CustomerName,
                WorkShopId = data.WorkShopId,
                WorkShopName = data.WorkShopName,
                ResourceId = data.ResourceId,
                ResourceName = data.ResourceName,
                ProcessId = data.ProcessId,
                ProcessName = data.ProcessName,
                StationId = data.StationId,
                StationName = data.StationName,
                EquipmentId = data.EquipmentId,
                EquipmentName = data.EquipmentName,
                OperatorId = data.OperatorId,
                OperatorName = data.OperatorName,
                ShiftId = data.ShiftId,
                ShiftName = data.ShiftName,
                ProductId = data.ProductId,
                ProductName = data.ProductName,
                ModelId = data.ModelId,
                ModelName = data.ModelName,
                CollectDate = data.CollectDate,
                ShiftDate = data.ShiftDate,
                Hour = data.Hour,
                QtyPass = data.QtyPass,
                QtyFailed = data.QtyFailed,
                QtyOnline = data.QtyOnline,
                IsScrap = data.IsScrap,
                S_QtyPass = data.WipStation.QtyPass,
                S_QtyFailed = data.WipStation.QtyFailed,
                S_QtyTimes = data.WipStation.QtyTimes,
                S_QtyFailedTimes = data.WipStation.QtyFailedTimes,
                S_QtyMove = data.WipStation.QtyMove,
                P_QtyPass = data.WipProcess.QtyPass,
                P_QtyFailed = data.WipProcess.QtyFailed,
                P_QtyMove = data.WipProcess.QtyMove,
                SourceInvOrgId = data.InvOrgId,
            };
            RF.Save(fail);
        }

        /// <summary>
        /// 获取采集失败的WipData
        /// </summary>
        /// <returns>记录集合</returns>
        public virtual EntityList<WipDataFail> GetWipDataFail()
        {
            return Query<WipDataFail>().Where(p => p.SourceInvOrgId == RT.InvOrg).ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 删除失败的记录
        /// </summary>
        /// <param name="id">记录ID</param>
        public virtual void DelWipDataFail(double id)
        {
            DB.Delete<WipDataFail>().Where(p => p.Id == id).Execute();
        }
        #endregion

        #region MES报工到APS调用方法
        /// <summary>
        /// 执行生产日进度
        /// </summary>
        /// <returns>true:执行成功; false:执行失败</returns>
        public virtual string RunUpdateDailySchedules()
        {
            StringBuilder rtnMessage = new StringBuilder();
            var controller = RT.Service.Resolve<ApsMesTaskController>();
            var now = RF.Find<WorkOrderStatistics>().GetDbTime();
            rtnMessage.Append("报工执行开始[{0}]\r\n".L10nFormat(now));
            var taskUnions = controller.GetToReportTasks(); //所有待报工任务
            var taskUnionIds = taskUnions.Select(p => p.Id).ToList();
            var taskUnionDetails = controller.GetToReportTaskDetails(taskUnionIds);
            if (!taskUnionDetails.Any())
                rtnMessage.Append("未获取需要报工的计划任务关联信息[{0}]".L10nFormat(now));
            else
            {
                List<MesDailySchedulePlanData> mesDailyScheduleEvents = GetDailyScheduleEvents(taskUnions, taskUnionDetails);
                if (mesDailyScheduleEvents.Any())
                {
                    List<DailySchedulePlanData> dailyScheduleEvents = new List<DailySchedulePlanData>();
                    dailyScheduleEvents.AddRange(mesDailyScheduleEvents);
                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        rtnMessage.Append("调用APS接口[{0}]\r\n".L10nFormat(now)); //10.调用APS方法更新生产日进度
                        SaveUpdateDailyScheduleListLog(dailyScheduleEvents);
                        RT.Service.Resolve<IPlanTaskDailySchedule>().UpdateDailyScheduleList(dailyScheduleEvents);
                        rtnMessage.Append("调用MES方法更新报工情况[{0}]\r\n".L10nFormat(now)); ////20.更新TaskUnionWorkOrder和WorkOrderReport
                        RT.Service.Resolve<ApsMesTaskController>().UpdateTaskUnionWorkOrderReport(mesDailyScheduleEvents, taskUnionDetails);
                        rtnMessage.Append("更新生产日进度执行成功 ! {0}".L10nFormat(now));
                        tran.Complete();
                    }
                }
                else
                {
                    rtnMessage.Append("未获取生产日进度信息 [{0}]".L10nFormat(now));
                }
            }

            return rtnMessage.ToString();
        }

        /// <summary>
        /// 保存更新生产日进度日志
        /// </summary>
        /// <param name="dailySchedulePlanDatas">进度计划数据列表</param>
        private void SaveUpdateDailyScheduleListLog(IReadOnlyList<DailySchedulePlanData> dailySchedulePlanDatas)
        {
            using (var tran = DB.AutonomousTransactionScope(StatisticsEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(dailySchedulePlanDatas);
                var inputValue = "进度计划数据列表:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IPlanTaskDailySchedule",
                    Method = "UpdateDailyScheduleList",
                    ControllerName = "WipStatisticsController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        ///// <summary>
        ///// 获取需要报工的计划任务关联工单集合
        ///// </summary>
        ///// <returns>计划任务关联工单集合</returns>
        ////private EntityList<TaskUnionDetail> GetSchedulingTaskUnionDtls()
        ////{
        ////    var taskUnionDetails = RT.Service.Resolve<ApsMesTaskController>().GetTaskUnionDetails(false, null);
        ////    return taskUnionDetails;
        ////}

        /// <summary>
        /// 获取Mes的生产日进度信息集合
        /// </summary>
        /// <param name="taskUnions">计划任务关联集合</param>
        /// <param name="taskUnionDetails">计划任务关联明细集合</param>
        /// <returns>Mes的生产日进度信息集合</returns>
        private List<MesDailySchedulePlanData> GetDailyScheduleEvents(EntityList<TaskUnion> taskUnions, EntityList<TaskUnionDetail> taskUnionDetails)
        {
            List<MesDailySchedulePlanData> mesDailyScheduleEvents = new List<MesDailySchedulePlanData>();
            List<double> woIds = taskUnionDetails.Select(p => p.WorkOrderId).ToList();
            var workOrderStaticses = RT.Service.Resolve<WipStatisticsController>().GetShiftEndWorkOrderStaticses(woIds);
            if (workOrderStaticses.Any())
            {
                var groupByWos = workOrderStaticses.GroupBy(x => new { x.WorkOrderId, x.ShiftId, x.ResourceId, x.CollectDate })
                .Select(p => (new
                {
                    KeyObj = p.Key,
                    StatisticId = p.Max(item => item.Id),
                    FinishedAmount = p.Sum(item => item.QtyPass),
                    CostHour = p.Sum(item => (item.EndCollectTime - item.BeginCollectTime).TotalHours)
                }));
                foreach (var item in groupByWos)
                {
                    double workOrderId = item.KeyObj.WorkOrderId;
                    double resourceId = item.KeyObj.ResourceId;
                    double shiftId = item.KeyObj.ShiftId;
                    DateTime collectDate = item.KeyObj.CollectDate;
                    var taskUnionDtl = taskUnionDetails.FirstOrDefault(x => x.WorkOrderId == workOrderId);
                    var taskUnion = taskUnions.FirstOrDefault(x => x.Id == taskUnionDtl.TaskUnionId);
                    var curDailySchedule = new MesDailySchedulePlanData();
                    curDailySchedule.PlanTaskId = taskUnion?.PlanTaskId;
                    curDailySchedule.ProcessTechOrderCode = taskUnionDtl?.ProcessTechOrderCode;
                    curDailySchedule.WorkOrderId = workOrderId;
                    curDailySchedule.StatisticId = item.StatisticId;
                    curDailySchedule.ShiftId = shiftId;
                    curDailySchedule.WipResourceId = resourceId;
                    curDailySchedule.FinishedAmount = item.FinishedAmount;
                    curDailySchedule.FinishedDate = collectDate;
                    curDailySchedule.CostHour = (decimal)item.CostHour;
                    curDailySchedule.DetailId = taskUnionDtl?.DetailId;
                    curDailySchedule.PlanQty = taskUnionDtl?.WorkOrder != null ? taskUnionDtl.WorkOrder.PlanQty : 0;
                    mesDailyScheduleEvents.Add(curDailySchedule);
                }
            }

            return mesDailyScheduleEvents;
        }

        /// <summary>
        /// 获取班次结束的采集数据
        /// </summary>
        /// <param name="woIds">待报工工单ID集合</param>
        /// <returns>工单采集集合</returns>
        private EntityList<WorkOrderStatistics> GetShiftEndWorkOrderStaticses(List<double> woIds)
        {
            var woStatistics = GetWorkOrderStaticses(woIds);
            var statisticsList = ProcessWorkOrderStaticses(woStatistics);
            return statisticsList;
        }

        /// <summary>
        /// 获取工单采集统计集合
        /// </summary>
        /// <param name="woIds">待报工工单ID集合</param>
        /// <returns>工单采集统计集合</returns>
        private EntityList<WorkOrderStatistics> GetWorkOrderStaticses(List<double> woIds)
        {
            //待开始报工
            var toReport = woIds.SplitContains((woIdList) =>
            {
                return Query<WorkOrderStatistics>()
                .Join<TaskUnionDetail>((w, t) => w.WorkOrderId == t.WorkOrderId && !t.IsFinish && t.TotalQty <= 0)
                .Where(p => woIdList.Contains(p.WorkOrderId) && p.QtyPass > 0)
                .ToList();
            });
            //开始报工未完成报工
            var reporting = woIds.SplitContains((woIdList) =>
            {
                return Query<WorkOrderStatistics>()
                .Join<TaskUnionDetail>((w, t) => w.WorkOrderId == t.WorkOrderId && !t.IsFinish)
                .Join<WorkOrderReport>((w, r) => w.WorkOrderId == r.WorkOrderId && w.Id > r.StatisticId)
                .Where(p => woIdList.Contains(p.WorkOrderId) && p.QtyPass > 0)
                .ToList();
            });
            var woStatistics = new EntityList<WorkOrderStatistics>();
            woStatistics.AddRange(toReport);
            woStatistics.AddRange(reporting);
            return woStatistics;
        }

        /// <summary>
        /// 获取班次结束的采集数据
        /// </summary>
        /// <param name="woStatistics">工单采集统计集合</param>
        /// <returns>班次结束的采集集合</returns>
        private EntityList<WorkOrderStatistics> ProcessWorkOrderStaticses(EntityList<WorkOrderStatistics> woStatistics)
        {
            var statisticsList = new EntityList<WorkOrderStatistics>();
            var now = RF.Find<WorkOrderStatistics>().GetDbTime();
            foreach (var item in woStatistics)
            {
                var shift = RF.GetById<Shift>(item.ShiftId);
                if (shift != null)
                {
                    var shiftEndTime = item.ShiftDate + shift.EndTime.TimeOfDay;
                    if (shift.IsOverDay)
                    {
                        shiftEndTime = item.ShiftDate.AddDays(1) + shift.EndTime.TimeOfDay;
                    }
                    if (now >= shiftEndTime)
                    {
                        statisticsList.Add(item);
                    }
                }
            }

            return statisticsList;
        }
        #endregion

        /// <summary>
        /// 获取工序采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        public virtual List<ProcessStatisticsEventInfo> GetProcessStatisticsList(double workOrderId)
        {
            SaveGetProcessStatisticsListLog(workOrderId);
            List<ProcessStatisticsEventInfo> list = new List<ProcessStatisticsEventInfo>();
            var entityList = Query<ProcessStatistics>().Where(w => w.WorkOrderId == workOrderId).ToList();
            foreach (var entity in entityList)
            {
                list.Add(new ProcessStatisticsEventInfo()
                {
                    ProcessIndex = entity.ProcessIndex,
                    ProcessId = entity.ProcessId,
                    ProcessName = entity.ProcessName,
                    InputQty = entity.QtyMove,
                    PassQty = entity.QtyPass,
                    FailedQty = entity.QtyFailed,
                    WOId = entity.WorkOrderId,
                });
            }
            return list;
        }

        /// <summary>
        /// 根据工单集合获取工序采集统计
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>

        public virtual List<ProcessStatisticsEventInfo> GetProcessStatisticsListByWorkOrderIds(List<double> workOrderIds)
        {
            //SaveGetProcessStatisticsListLog(workOrderId);
            List<ProcessStatisticsEventInfo> list = new List<ProcessStatisticsEventInfo>();
            var entityList = workOrderIds.SplitContains(ids => { return Query<ProcessStatistics>().Where(w => workOrderIds.Contains(w.WorkOrderId)).ToList(); });
            foreach (var entity in entityList)
            {
                list.Add(new ProcessStatisticsEventInfo()
                {
                    ProcessIndex = entity.ProcessIndex,
                    ProcessId = entity.ProcessId,
                    ProcessName = entity.ProcessName,
                    InputQty = entity.QtyMove,
                    PassQty = entity.QtyPass,
                    FailedQty = entity.QtyFailed,
                    WOId = entity.WorkOrderId,
                });
            }
            return list;
        }



        /// <summary>
        /// 保存工序采集信息日志
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        private void SaveGetProcessStatisticsListLog(double workOrderId)
        {
            using (var tran = DB.AutonomousTransactionScope(StatisticsEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "工单Id:{0}".L10nFormat(workOrderId);
                var log = new InterfaceLog()
                {
                    Name = "IProcessStatistics",
                    Method = "GetProcessStatisticsList",
                    ControllerName = "WipStatisticsController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workOrderId"></param>
        public virtual List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workOrderId)
        {
            SaveGetProcessFpyStatisticsListLog(workOrderId);
            List<ProcessStatisticsEventInfo> list = new List<ProcessStatisticsEventInfo>();
            var entityList = Query<ProcessFpyStatistics>().Where(w => w.WorkOrderId == workOrderId).ToList();
            foreach (var entity in entityList)
            {
                list.Add(new ProcessStatisticsEventInfo()
                {
                    ProcessId = entity.ProcessId,
                    ProcessName = entity.ProcessName,
                    InputQty = entity.InputQty,
                    PassQty = entity.PassQty,
                    FailedQty = entity.FailedQty
                });
            }
            return list;
        }

        /// <summary>
        /// 获取工序直通采集信息
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        public virtual List<ProcessStatisticsEventInfo> GetProcessFpyStatisticsList(double workShopId, DateTime startDateTime, DateTime endDateTime)
        {
            List<ProcessStatisticsEventInfo> list = new List<ProcessStatisticsEventInfo>();
            var entityList = Query<ProcessFpyStatistics>()
                .Where(w => w.WorkShopId == workShopId
                    && w.CollectedDate >= startDateTime
                    && w.CollectedDate <= endDateTime)
                .ToList();

            foreach (var entity in entityList)
            {
                list.Add(new ProcessStatisticsEventInfo()
                {
                    ProcessId = entity.ProcessId,
                    ProcessName = entity.ProcessName,
                    InputQty = entity.InputQty,
                    PassQty = entity.PassQty,
                    FailedQty = entity.FailedQty
                });
            }
            return list;
        }

        /// <summary>
        /// 保存获取工序直通采集信息日志
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        private void SaveGetProcessFpyStatisticsListLog(double workOrderId)
        {
            using (var tran = DB.AutonomousTransactionScope(StatisticsEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "工单Id:{0}".L10nFormat(workOrderId);
                var log = new InterfaceLog()
                {
                    Name = "IProcessStatistics",
                    Method = "GetProcessFpyStatisticsList",
                    ControllerName = "WipStatisticsController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
    }
}