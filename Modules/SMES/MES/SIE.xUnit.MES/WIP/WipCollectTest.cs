using SIE.Common;
using SIE.Core.Common.Controllers;
using SIE.Defects;
using SIE.Domain;
using SIE.Logging;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Inspects;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.Threading;
using SIE.xUnit.Core;
using SIE.xUnit.Defects;
using SIE.xUnit.MES.WIP.Tasks;
using SIE.xUnit.Packages;
using SIE.xUnit.Resources;
using SIE.xUnit.Tech.Routings;
using SIE.xUnit.Techs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SIE.xUnit.MES.WIP
{
    public class WipCollectTest : IClassFixture<TestStarup>
    {
        static CommonController _commonController = RT.Service.Resolve<CommonController>();
        static ResTestController _resTestController = RT.Service.Resolve<ResTestController>();
        static TechTestController _techTestController = RT.Service.Resolve<TechTestController>();
        static MesTestController _mesTestController = RT.Service.Resolve<MesTestController>();
        static ContextControllerTest _contextControllerTest = RT.Service.Resolve<ContextControllerTest>();
        static ILog _logger = LogManager.GetLogger("wip");

        [Theory]
        [InlineData(50)]
        public void MoveCollect(decimal planQty)
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-单工序生产产品");
            //工序
            var types = new List<ProcessType>() { ProcessType.Assembly };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var process = processList.FirstOrDefault();
            //工艺路线
            var routing = _techTestController.CreateMoveRouting(process, (routingVm) => { });
            Assert.NotNull(routing);
            var routingVersion = routing.DefaultVersion;
            Assert.NotNull(routingVersion);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(planQty, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) => { });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var invOrg = RT.InvOrg;
            Exception exc = null;
            int moveQty = 0;
            _logger.Info($"*******工单[{workOrder.No}]正在生产...********");
            var workcell = _mesTestController.GetWorkcell(wipResource, process);
            Parallel.ForEach(barcodeList, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (barcode) =>
            {
                try
                {
                    RT.InvOrg = invOrg;
                    WipContext context = new WipContext()
                    {
                        Barcode = barcode,
                        ProcessType = process.Type.Value,
                        Workcell = workcell
                    };
                    _logger.Info($"条码：{barcode}过站...");
                    moveQty++;
                    Collect(context);
                }
                catch (Exception ex)
                {
                    exc = ex;
                    _logger.Info($"条码[{barcode}]采集失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(exc);
            _logger.Info($"*******工单[{workOrder.No}]生产完成...********");
            var snCount = barcodeList.Count;
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            //验证工单数据  
            _logger.Info($"工单状态[{wo.State.ToLabel()}]，上线数[{wo.OnlineQty}],完工数[{wo.FinishQty}],报废数[{wo.ScrapQty}],");
            if (wo.FinishQty != snCount)
            {
                IEnumerable<string> notMoveSnList = _mesTestController.GetNotMoveSnList(barcodeList, wo);
                _logger.Info($"出现异常未生产条码：{string.Join("，", notMoveSnList)}");
            }
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Finish, wo.State);
            Assert.Equal(snCount, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证生产产品版本
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(snCount, versionList.Count);
            Assert.All(versionList, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            Exception vExc = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    //验证采集工序
                    var processList = version.ProcessList;
                    Assert.Single(processList);
                    var wipProcess = processList.FirstOrDefault();
                    Assert.Equal(ResultType.Pass, wipProcess.Result);
                }
                catch (Exception ex)
                {
                    vExc = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc);
        }

        [Theory]
        [InlineData(10, 5)]
        public void AssemblyCollect(decimal planQty, int keyItemQty)
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-单工序生产产品");
            //if (type == ProcessType.Assembly && buckleMaterial)
            //{
            //    产品BOM
            //    var keyItmes = _mesTestController.GetOrCreateWipKeyItems(5);
            //    _mesTestController.CreateProductBom(product, keyItmes);
            //}
            //工序
            var types = new List<ProcessType>() { ProcessType.Assembly };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var process = processList.FirstOrDefault();
            //工艺路线
            var routing = _techTestController.CreateMoveRouting(process, (routingVm) => { });
            Assert.NotNull(routing);
            var routingVersion = routing.DefaultVersion;
            Assert.NotNull(routingVersion);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(planQty, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) =>
            {
                //添加工序bom
                Random r = new Random();
                var woRoutingProcess = wo.RoutingProcessList.FirstOrDefault(p => p.ProcessId == process.Id);
                var keyItmes = _mesTestController.GetOrCreateWipKeyItems(keyItemQty);
                keyItmes.ForEach(keyItem =>
                {
                    var bom = new WorkOrderProcessBom()
                    {
                        Process = process,
                        Item = keyItem,
                        SingleQty = r.Next(1, 6),
                        RoutingProcess = woRoutingProcess
                    };
                    bom.GenerateId();
                    wo.ProcessBomList.Add(bom);
                });
            });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var invOrg = RT.InvOrg;
            Exception exc = null;
            int moveQty = 0;
            _logger.Info($"*******工单[{workOrder.No}]正在生产...********");
            var workcell = _mesTestController.GetWorkcell(wipResource, process);
            //上料   
            _mesTestController.WorkOrderLoadItem(workOrder, workcell);
            foreach (var barcode in barcodeList)
            {
                try
                {
                    RT.InvOrg = invOrg;
                    WipContext context = new WipContext()
                    {
                        Barcode = barcode,
                        ProcessType = process.Type.Value,
                        Workcell = workcell,
                        IsBuckleMaterial = true
                    };
                    _logger.Info($"条码：{barcode}过站...");
                    moveQty++;
                    Collect(context);
                    Thread.Sleep(1 * 200);
                }
                catch (Exception ex)
                {
                    exc = ex;
                    _logger.Info($"条码[{barcode}]采集失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            Assert.Null(exc);
            _logger.Info($"*******工单[{workOrder.No}]生产完成...********");
            var snCount = barcodeList.Count;
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            //验证工单数据  
            _logger.Info($"工单状态[{wo.State.ToLabel()}]，上线数[{wo.OnlineQty}],完工数[{wo.FinishQty}],报废数[{wo.ScrapQty}],");
            if (wo.FinishQty != snCount)
            {
                IEnumerable<string> notMoveSnList = _mesTestController.GetNotMoveSnList(barcodeList, wo);
                _logger.Info($"出现异常未生产条码：{string.Join("，", notMoveSnList)}");
            }
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Finish, wo.State);
            Assert.Equal(snCount, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证生产产品版本
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(snCount, versionList.Count);
            Assert.All(versionList, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            var boms = workOrder.ProcessBomList;
            //验证上料明细是否正确扣料
            var loadItems = _commonController.GetDatas<LoadItem>(p => p.WorkOrderId == workOrder.Id && p.StationId == workcell.StationId);
            Assert.Equal(boms.Count, loadItems.Count);
            boms.ForEach(bom =>
            {
                var loadItem = loadItems.FirstOrDefault(p => p.ItemId == bom.ItemId);
                Assert.Equal(bom.SingleQty * workOrder.PlanQty, loadItem.LoadQty);
                Assert.Equal(0, loadItem.Qty);
            });
            //验证工位货区 
            var station = workcell.Context["Station"] as Station;
            var itemStorages = _mesTestController.GetItemStorages(station);
            Assert.Equal(boms.Count, itemStorages.Count());
            Assert.All(itemStorages, p => Assert.Equal(0, p.Qty));
            Exception vExc = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    //验证采集工序
                    var processList = version.ProcessList;
                    Assert.Single(processList);
                    var wipProcess = processList.FirstOrDefault();
                    Assert.Equal(ResultType.Pass, wipProcess.Result);
                    //验证关键件
                    boms.ForEach(bom =>
                    {
                        var keyItems = wipProcess.KeyItemList.Where(p => p.ItemId == bom.ItemId);
                        Assert.NotEmpty(keyItems);
                        Assert.Equal(bom.SingleQty, keyItems.Sum(p => p.Qty));
                    });
                }
                catch (Exception ex)
                {
                    vExc = ex;
                    _logger.Info($"条码[{version.Sn}]采集数据验证失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc);
        }

        /// <summary>
        /// 检验工序采集测试 (工艺路线：检验--成功--》结束)
        ///                                 \ 失败--》维修
        /// </summary>
        [Fact]
        public void InspectProcessCollect()
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-检验工序产品");
            //工序
            var types = new List<ProcessType>() { ProcessType.Pqc, ProcessType.Fix };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var process = processList.FirstOrDefault(p => p.Type == ProcessType.Pqc);
            _techTestController.CreateProcessDefects(process, 10);
            process = RF.GetById<Process>(process.Id);
            var fixProcess = processList.FirstOrDefault(p => p.Type == ProcessType.Fix);
            Assert.NotNull(process);
            Assert.NotNull(fixProcess);
            //工艺路线
            var info = new List<RoutingProcessInfo>();
            info.Add(new RoutingProcessInfo() { Process = process, SortOrder = 1, SortOrderBack = 0, ResultType = ResultTypeForDesign.Pass });
            info.Add(new RoutingProcessInfo() { Process = process, SortOrder = 1, SortOrderBack = 2, ResultType = ResultTypeForDesign.Fail });
            info.Add(new RoutingProcessInfo() { Process = fixProcess, SortOrder = 2, SortOrderBack = 1, ResultType = ResultTypeForDesign.Pass });
            var routing = _techTestController.CreateCustomRouting(info);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(333, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) => { });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var workcell = _mesTestController.GetWorkcell(wipResource, process);
            ThreadPool.SetMaxThreads(30, 30);
            foreach (var barcode in barcodeList)
            {
                WipContext context = new WipContext()
                {
                    Barcode = barcode,
                    ProcessType = process.Type.Value,
                    Workcell = workcell,
                    IsBuckleMaterial = false
                };
                context.CollectData = new CollectData();
                context.CollectData.CollectBarcode = new CollectBarcode { Code = barcode, Type = BarcodeType.SN };
                if (barcode.EndsWith("0"))
                {
                    context.CollectData.Result = ResultType.Fail;
                    foreach (var defectItem in process.DefectList)
                    {
                        context.CollectData.Defects.Clear();
                        var defect = defectItem.Defect;
                        context.CollectData.Defects.Add(new DefectData { DefectId = defect.Id, DefectName = defect.Description, CategoryId = defect.DefectCategoryId, CategoryName = defect.DefectCategory?.Description, Qty = 1 });
                    }
                }
                else
                {
                    context.CollectData.Result = ResultType.Pass;
                }
                while (true)
                {
                    Thread.Sleep(1000);
                    int maxWorkerThreads, workerThreads;
                    int portThreads;
                    ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                    ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                    if (maxWorkerThreads - workerThreads <= 30)
                        break;
                }
                object[] param = new object[2];
                param[0] = RT.InvOrg;
                param[1] = context;
                object obj = param;
                ThreadPool.QueueUserWorkItem(new WaitCallback(InspectCollect), obj);
            }
            while (true) //等待采集结束
            {
                Thread.Sleep(1000);
                int maxWorkerThreads, workerThreads;
                int portThreads;
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                if (maxWorkerThreads - workerThreads == 1)
                    break;
            }
            var snCount = barcodeList.Count;
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            var fixQty = barcodeList.Count(p => p.EndsWith("0"));
            Assert.NotNull(wo);
            //验证工单数据
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Producing, wo.State);
            Assert.Equal(snCount - fixQty, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证采集记录 
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(snCount, versionList.Count);
            Assert.Equal(snCount - fixQty, versionList.Count(p => p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            //验证检验失败的采集记录,验证采集失败下一个工序是否是维修工序
            var noFinishList = versionList.Where(p => !p.IsFinish).ToList();
            Assert.Equal(fixQty, noFinishList.Count);
            Assert.All(noFinishList, p => Assert.Equal(fixProcess.Id, p.NextProcessId));

            List<Task> versionTasks = new List<Task>();
            Exception exc = null;
            foreach (var version in versionList)
            {
                versionTasks.Add(Task.Run(new Action(() =>
                {
                    try
                    {
                        //验证当前版本
                        Assert.Equal(version.Id, version.Product.CurrentVersionId);
                        //验证采集记录（只有一条，验证尾号为0的条码采集失败，其他成功）
                        var processList = version.ProcessList;
                        Assert.Single(processList);
                        var firstProcess = processList.FirstOrDefault();
                        Assert.Equal(process.Id, firstProcess.ProcessId);
                        if (firstProcess.Barcode.EndsWith("0"))
                            Assert.Equal(ResultType.Fail, firstProcess.Result);
                        else
                            Assert.Equal(ResultType.Pass, firstProcess.Result);
                        //验证不良记录
                        var defectList = version.DefectList;
                        if (version.Sn.EndsWith("0"))
                            Assert.Single(defectList);
                        else
                            Assert.Empty(defectList);
                    }
                    catch (Exception ex)
                    {
                        exc = ex;
                    }
                }).WithCurrentThreadContext()));
            }
            Task.WaitAll(versionTasks.ToArray());
            Assert.Null(exc);
        }

        /// <summary>
        /// 维修工序采集测试 (工艺路线：检验--成功--》结束)
        ///                                 \ 失败--》维修
        /// </summary>
        [Fact]
        public void RepairProcessCollect()
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-维修工序产品");
            #region 工序和工艺路线设置
            //工序
            var types = new List<ProcessType>() { ProcessType.Pqc, ProcessType.Fix };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var process = processList.FirstOrDefault(p => p.Type == ProcessType.Pqc);
            _techTestController.CreateProcessDefects(process, 10);
            process = RF.GetById<Process>(process.Id);
            var fixProcess = processList.FirstOrDefault(p => p.Type == ProcessType.Fix);
            Assert.NotNull(process);
            Assert.NotNull(fixProcess);
            //工艺路线
            var info = new List<RoutingProcessInfo>();
            info.Add(new RoutingProcessInfo() { Process = process, SortOrder = 1, SortOrderBack = 0, ResultType = ResultTypeForDesign.Pass });
            info.Add(new RoutingProcessInfo() { Process = process, SortOrder = 1, SortOrderBack = 2, ResultType = ResultTypeForDesign.Fail });
            info.Add(new RoutingProcessInfo() { Process = fixProcess, SortOrder = 2, SortOrderBack = 1, ResultType = ResultTypeForDesign.Pass });
            var routing = _techTestController.CreateCustomRouting(info);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            #endregion
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(390, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) => { });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var workcell = _mesTestController.GetWorkcell(wipResource, process);
            ThreadPool.SetMaxThreads(30, 30);
            //先检验
            #region 先进行检验采集
            foreach (var barcode in barcodeList)
            {
                WipContext context = new WipContext()
                {
                    Barcode = barcode,
                    ProcessType = process.Type.Value,
                    Workcell = workcell,
                    IsBuckleMaterial = false
                };
                context.CollectData = new CollectData();
                context.CollectData.CollectBarcode = new CollectBarcode { Code = barcode, Type = BarcodeType.SN };
                if (barcode.EndsWith("0"))
                {
                    context.CollectData.Result = ResultType.Fail;
                    foreach (var defectItem in process.DefectList)
                    {
                        context.CollectData.Defects.Clear();
                        var defect = defectItem.Defect;
                        context.CollectData.Defects.Add(new DefectData { DefectId = defect.Id, DefectName = defect.Description, CategoryId = defect.DefectCategoryId, CategoryName = defect.DefectCategory?.Description, Qty = 1 });
                    }
                }
                else
                {
                    context.CollectData.Result = ResultType.Pass;
                }
                while (true)
                {
                    Thread.Sleep(1000);
                    int maxWorkerThreads, workerThreads;
                    int portThreads;
                    ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                    ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                    if (maxWorkerThreads - workerThreads <= 30)
                        break;
                }
                object[] param = new object[2];
                param[0] = RT.InvOrg;
                param[1] = context;
                object obj = param;
                ThreadPool.QueueUserWorkItem(new WaitCallback(InspectCollect), obj);
            }
            while (true) //等待采集结束
            {
                Thread.Sleep(1000);
                int maxWorkerThreads, workerThreads;
                int portThreads;
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                if (maxWorkerThreads - workerThreads == 1)
                    break;
            }
            #endregion
            #region 验证检验采集数据
            var snCount = barcodeList.Count;
            var fixQty = barcodeList.Count(p => p.EndsWith("0"));
            //验证工单数据
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Producing, wo.State);
            Assert.Equal(snCount - fixQty, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证采集记录 
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(snCount, versionList.Count);
            Assert.Equal(snCount - fixQty, versionList.Count(p => p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            //验证检验失败的采集记录,验证采集失败下一个工序是否是维修工序
            var noFinishList = versionList.Where(p => !p.IsFinish).ToList();
            Assert.Equal(fixQty, noFinishList.Count);
            Assert.All(noFinishList, p => Assert.Equal(fixProcess.Id, p.NextProcessId));
            #endregion

            //检验失败的条码进行维修
            var defectResponsibilitys = RT.Service.Resolve<DefectTestController>().GetDefectRespons(3);
            var repairMeasures = RT.Service.Resolve<DefectTestController>().GetRepairMeasures(4);
            var needFixSns = barcodeList.Where(p => p.EndsWith("0")).ToList();
            workcell = _mesTestController.GetWorkcell(wipResource, fixProcess);
            foreach (var needFixSn in needFixSns)
            {
                WipContext context = new WipContext()
                {
                    Barcode = needFixSn,
                    ProcessType = fixProcess.Type.Value,
                    Workcell = workcell,
                    IsBuckleMaterial = false
                };
                context.CollectData = new CollectData();
                context.CollectData.CollectBarcode = new CollectBarcode { Code = needFixSn, Type = BarcodeType.SN };
                context.CollectData.Result = ResultType.Pass;
                var defects = RT.Service.Resolve<RepairController>().LoadDefects(needFixSn, workcell);
                Assert.Single(defects);
                var defect = defects.FirstOrDefault();
                var repairDefect = new RepairDefect();
                repairDefect.ProductDefectId = defect.Id;
                repairDefect.IsFixed = true;
                repairDefect.Remark = "维修采集单元测试";
                repairDefect.Responsiblities = defectResponsibilitys;
                repairDefect.Measures = repairMeasures;
                context.CollectData.RepairDefects.Add(repairDefect);
                while (true)
                {
                    Thread.Sleep(1000);
                    int maxWorkerThreads, workerThreads;
                    int portThreads;
                    ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                    ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                    if (maxWorkerThreads - workerThreads <= 30)
                        break;
                }
                object[] param = new object[3];
                param[0] = RT.InvOrg;
                param[1] = context;
                param[2] = process.Id;
                object obj = param;
                ThreadPool.QueueUserWorkItem(new WaitCallback(RepairCollect), obj);
            }
            while (true) //等待采集结束
            {
                Thread.Sleep(1000);
                int maxWorkerThreads, workerThreads;
                int portThreads;
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                if (maxWorkerThreads - workerThreads == 1)
                    break;
            }
            //验证工单数据
            wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Producing, wo.State);
            Assert.Equal(snCount - fixQty, wo.FinishQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证维修条码的采集记录,验证下一工序是否为指定工序
            versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id && needFixSns.Contains(p.Sn));
            Assert.Equal(needFixSns.Count, versionList.Count);
            Assert.All(versionList, p => Assert.False(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            Assert.All(versionList, p => Assert.Equal(process.Id, p.NextProcessId));

            List<Task> versionTasks = new List<Task>();
            Exception exc = null;
            foreach (var version in versionList)
            {
                versionTasks.Add(Task.Run(new Action(() =>
                {
                    try
                    {
                        //验证当前版本
                        Assert.Equal(version.Id, version.Product.CurrentVersionId);
                        //验证采集记录(共有两条:检验失败记录，维修通过记录)
                        var processList = version.ProcessList;
                        Assert.Equal(2, processList.Count);
                        var firstProcess = processList.FirstOrDefault(p => p.ProcessId == process.Id);
                        var secondProcess = processList.FirstOrDefault(p => p.ProcessId == fixProcess.Id);
                        Assert.NotNull(firstProcess);
                        Assert.NotNull(secondProcess);
                        Assert.Equal(ResultType.Fail, firstProcess.Result);
                        Assert.Equal(ResultType.Pass, secondProcess.Result);
                        Assert.True(firstProcess.OperateTime < secondProcess.OperateTime);
                        //验证产品维修记录
                        var repaireList = version.RepaireList;
                        Assert.Single(repaireList);

                        var firstRepaire = repaireList.FirstOrDefault();

                        Assert.Equal(fixProcess.Id, firstRepaire.ProcessId);
                        Assert.Equal(RT.IdentityId, firstRepaire.ReparieById);
                        Assert.Equal(wipResource.Id, firstRepaire.ResourceId);

                        var firstRepairDefect = 
                            firstRepaire.WipProductRepairDefectList.FirstOrDefault();

                        //验证不良记录、验证缺陷责任、维修措施
                        var defectList = version.DefectList;
                        Assert.Single(defectList);
                        var firstDefect = defectList.FirstOrDefault();
                        Assert.True(firstDefect.IsFixed);
                        Assert.NotNull(firstDefect.FixedById);
                        Assert.NotNull(firstDefect.FixedDate);
                        Assert.Equal(firstDefect.Id, firstRepairDefect.WipProductDefectId);
                        Assert.Equal(4, firstDefect.MeasureList.Count);
                        Assert.Equal(3, firstDefect.ResponsibilityList.Count);
                    }
                    catch (Exception ex)
                    {
                        exc = ex;
                    }
                }).WithCurrentThreadContext()));
            }
            Task.WaitAll(versionTasks.ToArray());
            Assert.Null(exc);
        }

        [Theory]
        [InlineData(10)]
        public void NormalPackingCollect(decimal planQty)
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-单工序生产产品");
            //添加物料包装规则
            var itemPackgaeRule = RT.Service.Resolve<PkgTestController>().GetOrCreateItemPackageRule(product, "MES采集包装规则");
            //工序
            var types = new List<ProcessType>() { ProcessType.Packing };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var process = processList.FirstOrDefault();
            //工艺路线
            var routing = _techTestController.CreatePackingRouting(process, (routingVm) =>
            {
                //设置创建SKU
                var dtlVm = routingVm.ProcessDetailModelList.FirstOrDefault(p => p.ProcessType == ProcessType.Packing);
                if (dtlVm != null)
                    dtlVm.IsCreateSku = true;
            });
            Assert.NotNull(routing);
            var routingVersion = routing.DefaultVersion;
            Assert.NotNull(routingVersion);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(planQty, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) =>
            {
                //var rules = _mesTestController.CreateWorkOrderPackageRuleDetails(itemPackgaeRule);
                //wo.PackageRuleDetailList.AddRange(rules);
            });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var invOrg = RT.InvOrg;
            Exception exc = null;
            _logger.Info($"*******工单[{workOrder.No}]正在生产...********");
            var workcell = _mesTestController.GetWorkcell(wipResource, process);
            double? outRelationId = null;
            foreach (var barcode in barcodeList)
            {
                try
                {
                    RT.InvOrg = invOrg;
                    WipContext context = new WipContext()
                    {
                        Barcode = barcode,
                        ProcessType = process.Type.Value,
                        Workcell = workcell
                    };
                    _logger.Info($"条码：{barcode}过站...");
                    //context.CollectData.PackingData.OuterPackingRelationId = outRelationId;
                    Collect(context);
                    //var label = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
                    //outRelationId = label.RelationId;
                }
                catch (Exception ex)
                {
                    exc = ex;
                    _logger.Info($"条码[{barcode}]采集失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            Assert.Null(exc);
            _logger.Info($"*******工单[{workOrder.No}]生产完成...********");
            var snCount = barcodeList.Count;
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            //验证工单数据  
            _logger.Info($"工单状态[{wo.State.ToLabel()}]，上线数[{wo.OnlineQty}],完工数[{wo.FinishQty}],报废数[{wo.ScrapQty}],");
            if (wo.FinishQty != snCount)
            {
                IEnumerable<string> notMoveSnList = _mesTestController.GetNotMoveSnList(barcodeList, wo);
                _logger.Info($"出现异常未生产条码：{string.Join("，", notMoveSnList)}");
            }
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Finish, wo.State);
            Assert.Equal(snCount, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证生产产品版本
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(snCount, versionList.Count);
            Assert.All(versionList, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            Exception vExc = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    //验证采集工序
                    var processList = version.ProcessList;
                    Assert.Single(processList);
                    var wipProcess = processList.FirstOrDefault();
                    Assert.Equal(ResultType.Pass, wipProcess.Result);
                }
                catch (Exception ex)
                {
                    vExc = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc);
        }

        /// <summary>
        /// 重复过站采集测试(工艺路线：过站（可重复）--过站--》结束)
        /// </summary>
        [Fact]
        public void RepeatCollectTest()
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-重复过站产品");
            #region 工序和工艺路线设置
            //工序
            var types = new List<ProcessType>() { ProcessType.Assembly, ProcessType.Assembly };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Equal(types.Count, processList.Count);
            var firstProcess = processList.FirstOrDefault();
            var secondProcess = processList.LastOrDefault();
            Assert.NotEqual(firstProcess.Id, secondProcess.Id);
            //工艺路线
            var info = new List<RoutingProcessInfo>();
            info.Add(new RoutingProcessInfo() { Process = firstProcess, SortOrder = 1, SortOrderBack = 2, ResultType = ResultTypeForDesign.Any, IsRepeat = true });
            info.Add(new RoutingProcessInfo() { Process = secondProcess, SortOrder = 2, SortOrderBack = 0, ResultType = ResultTypeForDesign.Any });
            var routing = _techTestController.CreateCustomRouting(info);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            #endregion
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(130, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) => { });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            var workcell = _mesTestController.GetWorkcell(wipResource, firstProcess);
            var secondWorkcell = _mesTestController.GetWorkcell(wipResource, secondProcess);
            Exception exc = null;
            var invOrg = RT.InvOrg;
            Parallel.ForEach(barcodeList, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (barcode) =>
            {
                try
                {
                    RT.InvOrg = invOrg;
                    WipContext context = new WipContext()
                    {
                        Barcode = barcode,
                        ProcessType = firstProcess.Type.Value,
                        Workcell = workcell,
                        IsBuckleMaterial = false
                    };
                    context.CollectData = new CollectData();
                    context.CollectData.CollectBarcode = new CollectBarcode { Code = barcode, Type = BarcodeType.SN };
                    context.CollectData.Result = ResultType.Pass;
                    Collect(context);
                    Collect(context);
                    context.Workcell = secondWorkcell;
                    Collect(context);
                }
                catch (Exception ex)
                {
                    exc = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(exc);
            //验证工单数据
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Finish, wo.State);
            Assert.Equal(barcodeList.Count, wo.FinishQty);
            Assert.Equal(barcodeList.Count, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证采集记录 
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(barcodeList.Count, versionList.Count);
            Assert.All(versionList, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            Exception vExc = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    //验证采集记录
                    var processList = version.ProcessList;
                    Assert.Equal(3, processList.Count);
                    Assert.All(processList, p => Assert.Equal(ResultType.Pass, p.Result));
                    var firstProcessCount = processList.Count(p => p.ProcessId == firstProcess.Id);
                    var secondProcessCount = processList.Count(p => p.ProcessId == secondProcess.Id);
                    Assert.Equal(2, firstProcessCount);
                    Assert.Equal(1, secondProcessCount);
                }
                catch (Exception ex)
                {
                    vExc = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc);
        }

        /// <summary>
        /// 检验采集
        /// </summary>
        /// <param name="obj">采集数据</param>
        private void InspectCollect(object obj)
        {
            RT.InvOrg = Convert.ToInt32((obj as object[])[0]);
            WipContext context = (obj as object[])[1] as WipContext;
            Collect(context);
        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="obj">采集数据</param>
        private void RepairCollect(object obj)
        {
            RT.InvOrg = Convert.ToInt32((obj as object[])[0]);
            WipContext context = (obj as object[])[1] as WipContext;
            var uplineProcessId = Convert.ToDouble((obj as object[])[2]);
            RT.Service.Resolve<RepairController>().Collect(context.Barcode, context.CollectData, context.Workcell, uplineProcessId);
        }

        [Theory]
        [InlineData(10, 5)]
        public void WipCollect(decimal planQty, int keyItemQty)
        {
            _contextControllerTest.InitContext();
            //物料
            var product = _mesTestController.GetOrCreateWipProduct("MES-采集生产产品");
            //添加物料包装规则
            RT.Service.Resolve<PkgTestController>().GetOrCreateItemPackageRule(product, "MES全工序采集包装规则");
            //工序
            var types = new List<ProcessType>() { ProcessType.Assembly, ProcessType.Assembly, ProcessType.Fix, ProcessType.Pqc, ProcessType.Packing };
            var processList = _techTestController.CreateProcesss(types).ToArray();
            Assert.Equal(types.Count, processList.Count());
            var assemblyProcess = processList[1];
            var pqcProcess = processList.FirstOrDefault(p => p.Type == ProcessType.Pqc);
            var processDefects = _techTestController.CreateProcessDefects(pqcProcess, 5);
            //工艺路线
            var routing = _techTestController.CreateWipRouting(processList);
            Assert.NotNull(routing);
            //配置产品工艺路线设置
            _mesTestController.CreateProductRouting(product, routing, SIE.Core.WorkOrders.WorkOrderType.Mass);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //工单
            var workOrder = _mesTestController.CreateWipWorkOrder(planQty, SIE.Core.WorkOrders.WorkOrderType.Mass, wipResource, product, (wo) =>
            {
                //添加工序bom 
                Random r = new Random();
                var woRoutingProcess = wo.RoutingProcessList.FirstOrDefault(p => p.ProcessId == assemblyProcess.Id);
                var keyItmes = _mesTestController.GetOrCreateWipKeyItems(keyItemQty);
                keyItmes.ForEach(keyItem =>
                {
                    var bom = new WorkOrderProcessBom()
                    {
                        Process = assemblyProcess,
                        Item = keyItem,
                        SingleQty = r.Next(1, 6),
                        RoutingProcess = woRoutingProcess
                    };
                    bom.GenerateId();
                    wo.ProcessBomList.Add(bom);
                });
            });
            Assert.NotNull(workOrder);
            Assert.Equal(processList.Count(), workOrder.RoutingProcessList.Count);
            //打印条码
            var barcodeList = _mesTestController.PrintBarcode(workOrder);
            Exception exc = null;
            _logger.Info($"*******工单[{workOrder.No}]正在生产...********");
            Dictionary<double, Workcell> dicWorkcell = new Dictionary<double, Workcell>();
            processList.ForEach(process =>
            {
                var workcell = _mesTestController.GetWorkcell(wipResource, process);
                dicWorkcell.Add(process.Id, workcell);
            });
            _mesTestController.WorkOrderLoadItem(workOrder, dicWorkcell[assemblyProcess.Id]);
            //ConcurrentQueue<string> pqcFailBarcodes = new ConcurrentQueue<string>();
            List<string> pqcFailBarcodes = new List<string>();
            var invOrg = RT.InvOrg;
            //过站--装配--检验--包装--结束
            List<Process> wipProcessList = new List<Process>(processList);
            wipProcessList.RemoveAt(2);
            foreach (var barcode in barcodeList)
            {
                try
                {
                    for (int i = 0; i < wipProcessList.Count; i++)
                    {
                        var process = wipProcessList[i];
                        WipContext context = new WipContext()
                        {
                            Barcode = barcode,
                            ProcessType = process.Type.Value,
                            Workcell = dicWorkcell[process.Id],
                            IsBuckleMaterial = i == 1
                        };
                        if (process.Type == ProcessType.Pqc && (barcode.EndsWith("1") || barcode.EndsWith("5")))
                        {
                            context.CollectData.Result = ResultType.Fail;
                            foreach (var defectItem in processDefects)
                            {
                                var defect = defectItem.Defect;
                                context.CollectData.Defects.Add(new DefectData { DefectId = defect.Id, DefectName = defect.Description, CategoryId = defect.DefectCategoryId, CategoryName = defect.DefectCategory?.Description, Qty = 1 });
                            }
                            pqcFailBarcodes.Add(barcode);
                            //pqcFailBarcodes.Enqueue(barcode);
                        }
                        _logger.Info($"条码：{barcode} {process.Name}过站...");
                        Collect(context);
                        if (context.CollectData.Result == ResultType.Fail)  //检验失败去维修，后续处理
                            break;
                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    exc = ex;
                    _logger.Info($"条码[{barcode}]采集失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            Assert.Null(exc);
            #region 无缺陷采集断言
            _logger.Info($"*******工单[{workOrder.No}]无缺陷产品生产完成...********");
            var notDefectSnCount = barcodeList.Count - pqcFailBarcodes.Count;
            var wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            //验证工单数据  
            _logger.Info($"工单状态[{wo.State.ToLabel()}]，上线数[{wo.OnlineQty}],完工数[{wo.FinishQty}],报废数[{wo.ScrapQty}],");
            SIE.Core.WorkOrders.WorkOrderState state = pqcFailBarcodes.Count == 0 ? SIE.Core.WorkOrders.WorkOrderState.Finish : SIE.Core.WorkOrders.WorkOrderState.Producing;
            Assert.Equal(state, wo.State);
            Assert.Equal(notDefectSnCount, wo.FinishQty);
            Assert.Equal(barcodeList.Count, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证生产产品版本
            var versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(barcodeList.Count, versionList.Count);
            var finishVersion = versionList.Where(p => p.IsFinish);
            Assert.All(finishVersion, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            //验证上料明细是否正确扣料
            var boms = workOrder.ProcessBomList;
            var assemblyWorkcell = dicWorkcell[assemblyProcess.Id];
            var loadItems = _commonController.GetDatas<LoadItem>(p => p.WorkOrderId == workOrder.Id && p.StationId == assemblyWorkcell.StationId);
            Assert.Equal(boms.Count, loadItems.Count);
            boms.ForEach(bom =>
            {
                var loadItem = loadItems.FirstOrDefault(p => p.ItemId == bom.ItemId);
                Assert.Equal(bom.SingleQty * workOrder.PlanQty, loadItem.LoadQty);
                Assert.Equal(0, loadItem.Qty);
            });
            //验证工位货区 
            var station = assemblyWorkcell.Context["Station"] as Station;
            var itemStorages = _mesTestController.GetItemStorages(station);
            Assert.Equal(boms.Count, itemStorages.Count());
            Assert.All(itemStorages, p => Assert.Equal(0, p.Qty));
            Exception vExc = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    int processRecordCount = pqcFailBarcodes.Contains(version.Sn) ? 3 : processList.Length - 1;
                    //验证工序记录
                    var wipProcessList = version.ProcessList;
                    Assert.Equal(processRecordCount, wipProcessList.Count);
                    //验证关键件
                    var assemblyWipProcess = wipProcessList.FirstOrDefault(p => p.ProcessId == assemblyProcess.Id);
                    var keyItems = assemblyWipProcess.KeyItemList;
                    //验证总的装配数是否一致
                    decimal bomTotalQty = boms.Sum(p => p.SingleQty);
                    decimal totalQty = keyItems.Sum(p => p.Qty);
                    Assert.Equal(bomTotalQty, totalQty);
                    //验证bom装配数是正确
                    boms.ForEach(bom =>
                    {
                        var assemblyKeyItems = keyItems.Where(p => p.ItemId == bom.ItemId);
                        decimal itemTotalQty = assemblyKeyItems.Sum(p => p.Qty);
                        Assert.Equal(bom.SingleQty, itemTotalQty);
                    });
                }
                catch (Exception ex)
                {
                    vExc = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc);
            #endregion   
            //维修--检验--包装--结束 
            List<Process> fixProcessList = new List<Process>() { processList[2], processList[3], processList[4] };
            var defectResponsibilitys = RT.Service.Resolve<DefectTestController>().GetDefectRespons(3);
            var repairMeasures = RT.Service.Resolve<DefectTestController>().GetRepairMeasures(4);
            Exception fixExc = null;
            Parallel.ForEach(pqcFailBarcodes, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (barcode) =>
            {
                try
                {
                    for (int i = 0; i < fixProcessList.Count; i++)
                    {
                        var process = fixProcessList[i];
                        var workcell = dicWorkcell[process.Id];
                        WipContext context = new WipContext()
                        {
                            Barcode = barcode,
                            ProcessType = process.Type.Value,
                            Workcell = workcell
                        };
                        context.CollectData = new CollectData();
                        context.CollectData.CollectBarcode = new CollectBarcode { Code = barcode, Type = BarcodeType.SN };
                        context.CollectData.Result = ResultType.Pass;
                        var defects = RT.Service.Resolve<RepairController>().LoadDefects(barcode, workcell);
                        var defect = defects.FirstOrDefault();
                        var repairDefect = new RepairDefect();
                        repairDefect.ProductDefectId = defect.Id;
                        repairDefect.IsFixed = true;
                        repairDefect.Remark = "维修采集单元测试";
                        repairDefect.Responsiblities = defectResponsibilitys;
                        repairDefect.Measures = repairMeasures;
                        context.CollectData.RepairDefects.Add(repairDefect);
                        _logger.Info($"条码：{barcode} {process.Name}过站...");
                        Collect(context);
                    }
                }
                catch (Exception ex)
                {
                    fixExc = ex;
                    _logger.Info($"条码[{barcode}]采集失败：{ex}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(fixExc);
            _logger.Info($"*******工单[{workOrder.No}]生产完成...********");
            var snCount = barcodeList.Count;
            wo = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotNull(wo);
            //验证工单数据  
            _logger.Info($"工单状态[{wo.State.ToLabel()}]，上线数[{wo.OnlineQty}],完工数[{wo.FinishQty}],报废数[{wo.ScrapQty}],");
            if (wo.FinishQty != snCount)
            {
                IEnumerable<string> notMoveSnList = _mesTestController.GetNotMoveSnList(barcodeList, wo);
                _logger.Info($"出现异常未生产条码：{string.Join("，", notMoveSnList)}");
            }
            Assert.Equal(SIE.Core.WorkOrders.WorkOrderState.Finish, wo.State);
            Assert.Equal(snCount, wo.FinishQty);
            Assert.Equal(snCount, wo.OnlineQty);
            Assert.Equal(0, wo.ScrapQty);
            //验证生产产品版本
            versionList = _commonController.GetDatas<WipProductVersion>(p => p.WorkOrderId == wo.Id);
            Assert.Equal(barcodeList.Count, versionList.Count);
            Assert.All(versionList, p => Assert.True(p.IsFinish));
            Assert.All(versionList, p => Assert.False(p.IsScrapped));
            Exception vExc1 = null;
            Parallel.ForEach(versionList, (version) =>
            {
                RT.InvOrg = invOrg;
                try
                {
                    //验证当前版本
                    Assert.Equal(version.Id, version.Product.CurrentVersionId);
                    int processRecordCount = pqcFailBarcodes.Contains(version.Sn) ? processList.Length + 1 : processList.Length;
                    //验证工序记录
                    var wipProcessList = version.ProcessList;
                    Assert.Equal(processRecordCount, wipProcessList.Count);
                    if (pqcFailBarcodes.Contains(version.Sn))
                    {
                        //验证缺陷 
                        var wipDefects = version.DefectList;
                        Assert.Equal(processDefects.Count, wipDefects.Count);
                        var equalList = processDefects.Select(p => p.DefectId).Intersect(wipDefects.Select(p => p.DefectId ?? 0));//集合取交集
                        Assert.Equal(processDefects.Count, equalList.Count());
                        Assert.All(wipDefects, p => Assert.True(p.IsFixed));

                        //验证维修
                        var wipRepairs = version.RepaireList;
                        wipDefects.ForEach(wipDefect =>
                        {
                            var wipRepairDefect = wipRepairs.FirstOrDefault()?.WipProductRepairDefectList
                                .FirstOrDefault(p => p.WipProductDefectId == wipDefect.Id);
                            Assert.NotNull(wipRepairDefect);
                        });
                    }
                }
                catch (Exception ex)
                {
                    vExc1 = ex;
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
            Assert.Null(vExc1);
        }

        private void Collect(WipContext context)
        {
            switch (context.ProcessType)
            {
                case ProcessType.Assembly:
                    if (context.IsBuckleMaterial)
                        InitAsserblyData(context);
                    break;
                case ProcessType.Pqc:
                case ProcessType.Fqc:
                    InitFqcData(context);
                    break;
                case ProcessType.Packing:
                    InitPackingData(context);
                    break;
            }
            context.Controller.Collect(new[] { context.Barcode }, context.CollectData, context.Workcell);
        }

        private void InitPackingData(WipContext context)
        {
            context.Controller = RT.Service.Resolve<WipPackingController>();
        }

        private void InitFqcData(WipContext context)
        {
            context.Controller = RT.Service.Resolve<InspectController>();
        }

        private void InitAsserblyData(WipContext context)
        {
            context.Controller = RT.Service.Resolve<AssemblyController>();
        }
    }
}