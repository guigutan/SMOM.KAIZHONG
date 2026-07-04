using Newtonsoft.Json;
using SIE.Common;
using SIE.Defects;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Statistics.Fpy;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集数统计管理
    /// </summary>
    public class WipCollectedManager
    {
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private WipCollectedManager() { }

        /// <summary>
        /// 定时器，定时加载异常统计数据
        /// </summary>
        private static Timer _timer = new Timer();

        /// <summary>
        /// 单例
        /// </summary>
        public static WipCollectedManager Instance { get; } = new WipCollectedManager();

        /// <summary>
        /// 静态对象，用于加锁
        /// </summary>
        private static object o = new object();

        /// <summary>
        /// 采集消息队列
        /// </summary>
        private static Queue<WipCollected> _wipCollectedEvent = new Queue<WipCollected>();

        /// <summary>
        /// 程序退出标识
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// 是否启用统计
        /// </summary>
        bool isStart;

        /// <summary>
        /// 已分组采集数据，统计数据来源
        /// </summary>
        readonly Dictionary<string, WipData> _wipData = new Dictionary<string, WipData>();

        /// <summary>
        /// 添加采集记录
        /// </summary>
        /// <param name="data">采集成功事件</param>
        internal void AddWipCollectedData(WipCollectedEvent data)
        {
            var wipCollected = InitWipCollected(data.Data);
            _wipCollectedEvent.Enqueue(wipCollected);
            if (!isStart)
            {
                isStart = true;
                _timer.Interval = 1000 * 60 * 10;  //10分钟执行一次
                _timer.Elapsed += Timer_Elapsed;
                _timer.Enabled = true;
                _timer.Start();
                ////预统计启动时将上一次统计失败的采集数据重新做统计
                ReloadFailData();
                Start();
            }
        }

        /// <summary>
        /// 定时器定时重新加载统计错误数据
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">参数</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReloadFailData();
        }

        /// <summary>
        /// 重新加载统计失败数据
        /// </summary>
        void ReloadFailData()
        {
            AddFailToWipCollected();
            AddFailToWipData();
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        /// <param name="collectData">采集原始数据</param>
        /// <returns>采集数据</returns>
        WipCollected InitWipCollected(CollectEventData collectData)
        {
            var product = collectData.Product;
            var workOrder = product.WorkOrder;
            var workcell = collectData.Workcell;
            var wipCollected = new WipCollected()
            {
                InvOrgId = RT.InvOrg,
                CollectDate = collectData.CollectDate,
                WorkOrderId = collectData.Product.WorkOrderId,
                WorkOrderNo = workOrder.No,
                CustomerName = workOrder.Customer?.Name,
                CustomerId = workOrder.CustomerId,
                EmployeeId = workcell.EmployeeId,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                EquipmentId = workcell.EquipmentId,
                ProductId = collectData.Product.ItemId,
                ProductName = workOrder.Product.Name,
                ModelId = workOrder.Product.ModelId,
                ModelName = workOrder.Product.Model?.Name,
                IsScrap = collectData.CollectData.Grade == ProductGrade.Scrap,
                Barcode = collectData.Barcodes[0].Code,
                Type = collectData.Barcodes[0].Type,
                Result = collectData.CollectData.Result,
                IsNg = product.IsNg,
                BatchQty = collectData.CollectData.OutputBatch != null ? collectData.CollectData.OutputBatch.Qty : 0,
                Qty = product.Qty,
                IsStart = product.Routing.Current.IsStart,
                IsEnd = product.Routing.Current.IsEnd,
                ProcessType = product.Routing.Current.Type,
                ScrapQty = collectData.CollectData.ScrapQty,
                ProcessIndex = product.Routing.Current.Index
            };
            wipCollected.DefectList.AddRange(collectData.CollectData.Defects);
            return wipCollected;
        }

        /// <summary>
        /// 开始统计
        /// </summary>
        void Start()
        {
            while (true)
            {
                try
                {
                    lock (o)
                    {
                        CollectWipData();
                    }

                    if (Flag)
                    {
                        _timer?.Stop();
                        if (_wipCollectedEvent.Count < 1 && _wipData.Count < 1)
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Logging.LogManager.GetLogger("error_logger")?.Error($"采集统计异常 库存组织[{RT.InvOrg}]".L10N(), exc);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 采集过站数据收集
        /// </summary>
        private void CollectWipData()
        {
            WipData wip = GetWipCollectedData();
            if (wip == null)
                return;
            try
            {
                RT.InvOrg = wip.InvOrgId;
                RT.Service.Resolve<WipStatisticsController>().ProductCollected(wip);
            }
            catch (Exception exc)
            {
                //采集预统计数据报错异常
                Logging.LogManager.GetLogger("error_logger")?.Error($"采集统计异常 库存组织[{RT.InvOrg}]".L10N(), exc);
                RT.Service.Resolve<WipStatisticsController>().SaveFailWipData(wip);
            }
        }

        /// <summary>
        /// 获取生产采集数据
        /// </summary>
        /// <returns>采集数据</returns>
        WipData GetWipCollectedData()
        {
            //合并统计队列存在数据，先消费          
            if (_wipData.Count > 0)
            {
                return GetValue();
            }
            else
            {
                if (_wipCollectedEvent.Count > 0)
                {
                    InitWipCollectData();
                    if (_wipData.Count > 0)
                        return GetValue();
                }

                return null;
            }
        }

        /// <summary>
        /// 获取采集数据
        /// </summary>
        /// <returns>采集数据</returns>
        WipData GetValue()
        {
            var data = _wipData.FirstOrDefault();
            WipData wip = data.Value;
            string key = data.Key;
            _wipData.Remove(key);
            return wip;
        }

        /// <summary>
        /// 初始化采集数据，将相同的采集记录合并处理
        /// </summary>
        private void InitWipCollectData()
        {
            var count = _wipCollectedEvent.Count;
            List<WipCollected> wipCollectedEvent = new List<WipCollected>();
            for (int i = 0; i < count; i++)
            {
                wipCollectedEvent.Add(_wipCollectedEvent.Dequeue());
            }

            DataChange(wipCollectedEvent);
        }

        /// <summary>
        /// 获取失败的记录并加入到List中
        /// </summary>  
        private void AddFailToWipCollected()
        {
            var contoller = RT.Service.Resolve<WipStatisticsController>();
            var failList = contoller.GetWipCollectedFail();
            failList.ForEach(fail =>
            {
                var wipCollected = new WipCollected()
                {
                    Barcode = fail.Barcode,
                    BatchQty = fail.BatchQty,
                    CollectDate = fail.CollectDate,
                    CustomerName = fail.CustomerName,
                    EmployeeId = fail.EmployeeId,
                    EquipmentId = fail.EquipmentId,
                    InvOrgId = fail.SourceInvOrgId,
                    IsEnd = fail.IsEnd,
                    IsNg = fail.IsNg,
                    IsScrap = fail.IsScrap,
                    IsStart = fail.IsStart,
                    ModelId = fail.ModelId,
                    ModelName = fail.ModelName,
                    ProcessId = fail.ProcessId,
                    ProcessType = fail.ProcessType,
                    ProductId = fail.ProductId,
                    ProductName = fail.ProductName,
                    Qty = fail.Qty,
                    ResourceId = fail.ResourceId,
                    Result = fail.Result,
                    ScrapQty = fail.ScrapQty,
                    StationId = fail.StationId,
                    WorkOrderId = fail.WorkOrderId,
                    WorkOrderNo = fail.WorkOrderNo
                };
                string[] defects = fail.Defect.Split(new char[] { '|' });
                defects.ForEach(defect =>
                {
                    if (!string.IsNullOrEmpty(defect) && defect.ToLower() != "null")
                    {
                        wipCollected.DefectList.Add(JsonConvert.DeserializeObject<DefectData>(defect));
                    }
                });
                _wipCollectedEvent.Enqueue(wipCollected);
                contoller.DelWipCollectedFail(fail.Id);
            });
        }

        /// <summary>
        /// 获取失败wipData加入到字典中处理
        /// </summary>
        private void AddFailToWipData()
        {
            var wipList = RT.Service.Resolve<WipStatisticsController>().GetWipDataFail();
            wipList.ForEach(p =>
            {
                WipStation station = new WipStation()
                {
                    QtyPass = p.S_QtyPass,
                    QtyTimes = p.S_QtyTimes,
                    QtyFailed = p.S_QtyFailed,
                    QtyFailedTimes = p.S_QtyFailedTimes,
                    QtyMove = p.S_QtyMove
                };
                WipProcess process = new WipProcess()
                {
                    QtyPass = p.P_QtyPass,
                    QtyFailed = p.P_QtyFailed,
                    QtyMove = p.P_QtyMove
                };
                WipData data = new WipData()
                {
                    ProcessIndex = p.ProcessIndex,
                    Barcode = p.Barcode,
                    WorkOrderId = p.WorkOrderId,
                    WorkOrderNo = p.WorkOrderNo,
                    CustomerId = p.CustomerId,
                    CustomerName = p.CustomerName,
                    WorkShopId = p.WorkShopId,
                    WorkShopName = p.WorkShopName,
                    ResourceId = p.ResourceId,
                    ResourceName = p.ResourceName,
                    ProcessId = p.ProcessId,
                    ProcessName = p.ProcessName,
                    StationId = p.StationId,
                    StationName = p.StationName,
                    EquipmentId = p.EquipmentId,
                    EquipmentName = p.EquipmentName,
                    OperatorId = p.OperatorId,
                    OperatorName = p.OperatorName,
                    ShiftId = p.ShiftId,
                    ShiftName = p.ShiftName,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ModelId = p.ModelId,
                    ModelName = p.ModelName,
                    CollectDate = p.CollectDate,
                    ShiftDate = p.ShiftDate,
                    Hour = p.Hour,
                    QtyPass = p.QtyPass,
                    QtyFailed = p.QtyFailed,
                    QtyOnline = p.QtyOnline,
                    IsScrap = p.IsScrap,
                    InvOrgId = p.SourceInvOrgId,
                    WipStation = station,
                    WipProcess = process
                };
                CombineCollectedData(data);
                RT.Service.Resolve<WipStatisticsController>().DelWipDataFail(p.Id);
            });
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="wipCollectedEvent">采集事件列表</param>
        private void DataChange(List<WipCollected> wipCollectedEvent)
        {
            foreach (var item in wipCollectedEvent.GroupBy(p => p.InvOrgId))
            {
                var datas = item.ToList();
                RT.InvOrg = datas[0]?.InvOrgId;   //多组织数据汇总，处理数据是需要根据数据切换组织
                foreach (WipCollected wipCollected in datas)
                {
                    try
                    {
                        var data = PrepareData(wipCollected);
                        CombineCollectedData(data);
                    }
                    catch (Exception exc)
                    {
                        Logging.LogManager.GetLogger("error_logger")?.Error($"采集统计异常 库存组织[{RT.InvOrg}]".L10N(), exc);
                        ////数据准备时报错，记录准备前数据
                        RT.Service.Resolve<WipStatisticsController>().SaveFailWipCollected(wipCollected);
                    }
                }
            }
        }

        /// <summary>
        /// 合并采集数据
        /// </summary>
        /// <param name="data">采集数据</param>
        private void CombineCollectedData(WipData data)
        {
            string key = string.Format("{0}{1}{2}{3}{4}", data.WorkOrderId, data.StationId, data.ShiftId, data.ShiftDate.ToString("F"), data.OperatorId);
            if (!_wipData.ContainsKey(key))
            {
                _wipData.Add(key, data);
            }
            else
            {
                WipData value = _wipData[key];
                value.QtyPass += data.QtyPass;
                value.QtyFailed += data.QtyFailed;
                value.QtyOnline += data.QtyOnline;
                value.WipProcess.QtyPass += data.WipProcess.QtyPass;
                value.WipProcess.QtyFailed += data.WipProcess.QtyFailed;
                value.WipProcess.QtyMove += data.WipProcess.QtyMove;
                value.WipStation.QtyPass += data.WipStation.QtyPass;
                value.WipStation.QtyFailed += data.WipStation.QtyFailed;
                value.WipStation.QtyTimes += data.WipStation.QtyTimes;
                value.WipStation.QtyFailedTimes += data.WipStation.QtyFailedTimes;
                value.WipStation.QtyMove += data.WipStation.QtyMove;
            }
        }

        /// <summary>
        /// 数据准备
        /// </summary>
        /// <param name="item">采集事件</param>
        /// <returns>采集数据</returns>
        private WipData PrepareData(WipCollected item)
        {
            var collectDate = item.CollectDate;
            var controller = RT.Service.Resolve<WipStatisticsController>();
            var data = new WipData();
            data.ProcessIndex = item.ProcessIndex;
            data.InvOrgId = item.InvOrgId;
            data.WorkOrderId = item.WorkOrderId;
            data.WorkOrderNo = item.WorkOrderNo;
            data.CustomerName = item.CustomerName;
            data.OperatorId = item.EmployeeId;
            data.ResourceId = item.ResourceId;
            data.ResourceName = controller.GetResourceName(data);
            data.WorkShopId = controller.GetWorkShopId(item.ResourceId);
            data.WorkShopName = controller.GetWorkShopName(data);
            data.ProcessId = item.ProcessId;
            data.ProcessName = controller.GetProcessName(data);
            data.StationId = item.StationId;
            data.EquipmentId = item.EquipmentId;
            data.ProductId = item.ProductId;
            data.ProductName = item.ProductName;
            data.ModelId = item.ModelId;
            data.ModelName = item.ModelName;
            data.IsScrap = item.IsScrap;
            data.Barcode = item.Barcode;
            data.Type = item.Type;
            data.CollectDate = collectDate.Date;
            data.Hour = collectDate.Hour;
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(data.ResourceId, collectDate);
            DateTime shiftDate = RT.Service.Resolve<Resources.ShiftTypes.ShiftTypeController>().GetShiftDate(shift, collectDate);
            data.ShiftId = shift.Id;
            data.ShiftName = shift.Name;
            data.ShiftDate = shiftDate;
            var rule = RT.Service.Resolve<ItemController>().GetRetrospectType(item.ProductId);
            IsFinish(item, data, rule);
            IsRepeat(item, data, rule);
            return data;
        }

        /// <summary>
        /// 是否重复过站
        /// </summary>
        /// <param name="item">采集完成事件</param>
        /// <param name="data">采集数据</param>
        /// <param name="retrospectType">物料追溯方式</param>
        private void IsRepeat(WipCollected item, WipData data, SIE.Core.Items.RetrospectType retrospectType)
        {
            if (retrospectType == SIE.Core.Items.RetrospectType.Batch)
            {
                ////良品数，工序过站非失败，且批次无不良记录
                decimal passQty = item.Result != ResultType.Fail && !item.IsNg ? item.BatchQty : 0m;
                decimal failQty = item.Result == ResultType.Fail ? item.BatchQty : 0m;
                MoveCollected(data, item.Result, item.IsStart, item.BatchQty);
                FpyDefectStatistics(item, data, item.Result, passQty, failQty);
                return;
            }
            ////是否重复过站，重复过站工位统计累加成功或失败台数，工序和工单统计不处理
            var wipProcess = RT.Service.Resolve<WipProductVersionController>().GetWipProductProcess(data.Barcode, data.Type, data.ResourceId, data.ProcessId);
            if (wipProcess.Count > 1)
            {
                RepeatMoveCollected(data, item.Result, item.Qty, wipProcess);
            }
            else
            {
                decimal passQty = item.Result != ResultType.Fail ? item.Qty : 0m;
                decimal failQty = item.Result == ResultType.Fail ? item.Qty : 0m;
                MoveCollected(data, item.Result, item.IsStart, item.Qty);
                FpyDefectStatistics(item, data, item.Result, passQty, failQty);
            }
        }

        /// <summary>
        /// 重复过站统计
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="result">采集结果</param>
        /// <param name="qty">过站数量</param>
        /// <param name="wipProcess">工序过站记录</param>
        private void RepeatMoveCollected(WipData data, ResultType result, decimal qty, EntityList<WipProductProcess> wipProcess)
        {
            ////上一次过站记录结果 
            var currentResult = wipProcess.OrderByDescending(p => p.CreateDate).FirstOrDefault();
            var lastResult = wipProcess.Where(p => p.Id != currentResult.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault().Result;
            if (lastResult == ResultType.Pass)
            {
                if (result == ResultType.Pass)  ////上一次成功，本次成功
                    data.WipStation.QtyTimes += qty;
                else  ////上一次成功，本次失败
                {
                    data.WipStation.QtyPass -= qty;
                    data.WipStation.QtyFailed += qty;
                    data.WipStation.QtyFailedTimes += qty;
                }
            }
            else
            {
                if (result == ResultType.Pass)  ////上一次失败，本次成功
                {
                    data.WipStation.QtyPass += qty;
                    data.WipStation.QtyFailed -= qty;
                    data.WipStation.QtyTimes += qty;
                }
                else  ////上一次失败，本次失败
                {
                    data.WipStation.QtyFailedTimes += qty;
                }
            }
        }

        /// <summary>
        /// 过站统计
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="result">采集结果</param>
        /// <param name="isStart">是否开始工序</param>
        /// <param name="qty">过站数量</param>
        private void MoveCollected(WipData data, ResultType result, bool isStart, decimal qty)
        {
            if (result != ResultType.Fail)
            {
                data.WipStation.QtyPass += qty;
                data.WipProcess.QtyPass += qty;
            }
            else
            {
                data.WipStation.QtyFailed += qty;
                data.WipProcess.QtyFailed += qty;
            }

            data.WipStation.QtyMove += qty;
            data.WipProcess.QtyMove += qty;
            if (isStart)
            {
                data.QtyOnline += qty;
            }
        }

        /// <summary>
        /// 是否完工下线
        /// </summary>
        /// <param name="item">采集完成事件</param>
        /// <param name="data">采集数据</param>
        /// <param name="retrospectType">物料追溯方式</param>
        private void IsFinish(WipCollected item, WipData data, SIE.Core.Items.RetrospectType retrospectType)
        {
            if (retrospectType == SIE.Core.Items.RetrospectType.Single)
            {
                //是否完工下线
                bool isFinish = false;
                var wipProduct = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(new CollectBarcode { Code = data.Barcode, Type = data.Type });
                if (wipProduct != null)
                    isFinish = wipProduct.IsFinish;
                if (isFinish)
                {
                    if (data.IsScrap)
                    {
                        data.QtyFailed += item.Qty;
                    }
                    else
                    {
                        data.QtyPass += item.Qty;
                    }
                }
            }
            else
            {
                if (item.IsEnd && item.Result != ResultType.Fail)
                {
                    ////产品完工下线记录工单成功数 
                    data.QtyPass += item.BatchQty;
                }

                if (item.ProcessType == ProcessType.BatchFix)
                {
                    ////批次维修产生报废
                    data.QtyFailed += item.ScrapQty;
                }
            }
        }

        /// <summary>
        /// 直通不良统计
        /// </summary>
        /// <param name="item">采集数据</param> 
        /// <param name="data">汇总后采集数据</param> 
        /// <param name="result">结果类型</param>
        /// <param name="passQty">数量</param>
        /// <param name="failQty">失败数量</param>
        private void FpyDefectStatistics(WipCollected item, WipData data, ResultType result, decimal passQty, decimal failQty)
        {
            if (result == ResultType.Fail)
                DefectStatics(item, data, failQty);
            var process = RF.GetById<Process>(data.ProcessId);
            if (process.Type == ProcessType.Fix || process.Type == ProcessType.BatchFix)
                return;
            ////工序直通率统计
            ProcessFpyStatistics(data, passQty, failQty);
            ////产品直通率统计
            ProductFpyStatistics(item, data, passQty, failQty);
        }

        /// <summary>
        /// 不良缺陷统计
        /// </summary>
        /// <param name="item">采集数据</param>
        /// <param name="data">汇总后采集数据</param>
        /// <param name="qty">数量</param>
        private void DefectStatics(WipCollected item, WipData data, decimal qty)
        {
            RT.Service.Resolve<FpyController>().CreateOrUpdateDefectStatics(data, qty, item.DefectList);
        }

        /// <summary>
        /// 工序直通率统计
        /// </summary>
        /// <param name="data">采集数据</param> 
        /// <param name="passQty">一次通过数</param>
        /// <param name="failQty">一次失败数</param>
        private void ProcessFpyStatistics(WipData data, decimal passQty, decimal failQty)
        {
            RT.Service.Resolve<FpyController>().CreateOrUpdateProcessFpy(data, passQty, failQty);
        }

        /// <summary>
        /// 产品直通率统计
        /// </summary>
        /// <param name="item">采集数据</param>
        /// <param name="data">汇总后采集数据</param>
        /// <param name="passQty">一次通过数</param>
        /// <param name="failQty">一次失败数</param>
        private void ProductFpyStatistics(WipCollected item, WipData data, decimal passQty, decimal failQty)
        {
            if (item.IsStart)
            {
                //产品上线时记录投入数
                RT.Service.Resolve<FpyController>().CreateOrUpdateProductFpy(data, passQty + failQty, 0, 0);
            }

            if (item.IsEnd && item.Result != ResultType.Fail)
            {
                //产品下线时记录一次通过数据
                RT.Service.Resolve<FpyController>().CreateOrUpdateProductFpy(data, 0, passQty, failQty);
            }
        }
    }
}