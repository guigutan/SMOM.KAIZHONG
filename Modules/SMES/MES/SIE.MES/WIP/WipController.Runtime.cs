using SIE.Barcodes;
using SIE.Barcodes.Configs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ProcessTransfers;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using SIE.Tech;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 在制品控制器
    /// </summary>
    partial class WipController
    {
        /// <summary>
        /// 运行时控制器
        /// </summary>
        RuntimeController _runtimeController;

        /// <summary>
        /// 运行时控制器
        /// </summary>
        protected virtual RuntimeController RuntimeController { get { return _runtimeController ?? (_runtimeController = RT.Service.Resolve<RuntimeController>()); } }

        /// <summary>
        /// 创建新运行时产品，同时创建在制品和在制品版本
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>运行时产品</returns>
        protected virtual product CreateNewProduct(CollectBarcode barcode, Workcell workcell)
        {
            var wo = GetWorkOrder(barcode, workcell);
            if (wo.PanelQty > 1 && barcode.Type == BarcodeType.SN)
            {
                throw new ValidationException("[{0}]工单拼板数大于1，不允许单生产条码上线".L10nFormat(wo.No));
            }
            var product = CreateProduct(Guid.NewGuid().ToString("N").ToUpper(), wo, barcode, workcell);
            var wipProduct = CreateWipProduct(product);
            CreateWipProductVersion(wipProduct, product, barcode);
            return product;
        }

        /// <summary>
        /// 创建新版本的运行时产品，同时创建在制器新版本，非返工，验证工单不能创建新版本
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="ValidationException">产品已生产完成</exception>
        protected virtual product CreateVersionProduct(WipProduct wipProduct, CollectBarcode barcode, Workcell workcell)
        {
            var wo = GetWorkOrderAndBelong(barcode, workcell, wipProduct.CurrentVersion.WorkOrderId);

            if (wipProduct.CurrentVersion.WorkOrderId == wo.Id
                && wo.WorkOrderType != SIE.Core.WorkOrders.WorkOrderType.Rework
                && wo.WorkOrderType != SIE.Core.WorkOrders.WorkOrderType.Verify)
            {
                throw new ValidationException("[{0}]产品已生产完成,非返工或验证工单不允许再生产".L10nFormat(barcode));
            }
            if (wo.PanelQty > 1 && barcode.Type == BarcodeType.SN)
            {
                throw new ValidationException("[{0}]工单拼板数大于1，不允许单生产条码上线".L10nFormat(wo.No));
            }
            var exists = Query<WipProductVersion>().Where(p => p.ProductId == wipProduct.Id && p.WorkOrderId == wo.Id).Count() > 0;
            if (exists)
            {
                throw new ValidationException("[{0}]产品已生产完成,不允许再生产,工单为[{1}]".L10nFormat(barcode, wo.No));
            }

            var product = CreateProduct(wipProduct.Puid, wo, barcode, workcell);

            CreateWipProductVersion(wipProduct, product, barcode);

            return product;
        }

        /// <summary>
        /// 报废重用创建新运行时产品
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="ValidationException">产品已报废</exception>
        protected virtual product CreateScrapReuseProduct(WipProductVersion version, CollectBarcode barcode, Workcell workcell)
        {
            if (!version.CanScrapReuse)
                throw new ValidationException("[{0}]产品已报废且不允许重用，不能再生产".L10nFormat(barcode));
            return CreateNewProduct(barcode, workcell);
        }

        /// <summary>
        /// 恢复运行时产品，NoSql数据库可能会出现运行时产品数据与在制品数据不一致，预留此方法，未实现
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="NotImplementedException">未实现异常</exception>
        protected virtual product RecoverProduct(WipProductVersion version)
        {
            throw new NotImplementedException("产品未生产完成，但没有生产运行时数据，数据一致性问题，暂时未实现数据恢复");
        }

        /// <summary>
        /// 验证采集运行时产品的工艺路线
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="wipResourceMove">生产资源</param>
        /// <returns>采集运行时产品</returns>
        protected virtual product ValidateProduct(CollectBarcode barcode, Workcell workcell, WipResourceMove wipResourceMove = null)
        {
            var product = RuntimeController.FindProduct(barcode);
            var version = FindLastWipProductVersion(barcode);
            if (product == null) //可能未上线，或者数据已清空
            {
                if (version == null) ////没有生产记录，创建新产品
                {
                    if (barcode.Type == BarcodeType.SN)
                    {
                        var rconfig = ConfigService.GetConfig(new CheckBcRangeStateConfig());
                        if (rconfig != null && rconfig.IsCheck)
                        {
                            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);
                            if (sn.Range.State == ReceiveState.NoReceive)
                                throw new ValidationException("[{0}]未领用,不允许上线".L10nFormat(barcode));
                        }
                    }
                    product = CreateNewProduct(barcode, workcell);
                }
                else if (version.IsFinish)
                {
                    if (version.IsScrapped) ////报废的尝试重用创建新产品
                        product = CreateScrapReuseProduct(version, barcode, workcell);
                    else
                    {
                        //完成的尝试创建新版本
                        product = CreateVersionProduct(version.Product, barcode, workcell);
                    }
                }
                else
                {
                    //根据version还原product
                    product = RecoverProduct(version);
                }
            }
            else
            {
                if (version != null)
                {
                    if (version.IsPause == YesNo.Yes)
                    {
                        throw new ValidationException("[{0}]产品已暂停，不能继续生产".L10nFormat(barcode));
                    }

                    if (version.IsOutsourcing)
                    {
                        throw new ValidationException("产品【{0}】状态为【委外加工中】，不能继续过站，如委外加工完成，请确认是否已【委外入库】!"
                            .L10nFormat(barcode));
                    }
                }
            }

            //验证工序
            var process = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);

            if (process == null)
            {
                var nextProcessList = product.Routing.GetNext()
                    .Where(x => !x.IsPass);

                if (!nextProcessList.Any())
                {
                    nextProcessList = product.Routing.GetNext();
                }

                var nextProcessString = nextProcessList.Select(p => p?.Name).Concat("、");

                throw new ValidationException("[{0}]采集工序不正确，应该为[{1}]"
                    .L10nFormat(barcode, nextProcessString));
            }

            //验证工序最大过站次数
            if (process.MaxPassNum.HasValue && process.PassNum >= process.MaxPassNum)
            {
                throw new ValidationException("[{0}]采集工序[{1}]已达到最大过站次数[{2}]，不允许采集"
                    .L10nFormat(barcode, process.Name, process.MaxPassNum));
            }

            if (process.WipProductProcessState == WipProductProcessState.Start
                && product.Routing.Current.ProcessId.HasValue)
            {
                //控制工序加工时长（工序最少停留时间如老化时长、烘烤时长等）
                var processDuration = RT.Service.Resolve<ProcessDurationController>()
                    .GetProcessDurations(product.ItemId, product.Routing.Current.ProcessId.Value);

                if (processDuration != null
                    && processDuration.Durations > 0)
                {
                    var hours = (DateTime.Now - product.Routing.LastMoveDateTime).TotalHours;
                    if (hours < (double)processDuration.Durations)
                    {
                        throw new ValidationException("[{0}]未达到工序的加工时长[{1}]小时"
                            .L10nFormat(barcode, processDuration.Durations));
                    }
                }
            }

            //非开始工序验证工序交接
            if (!process.IsStart && !process.InInning
                && (process.TransferType == Tech.Processs.TransferType.TransferIn
                    || process.TransferType == Tech.Processs.TransferType.TransferInOut
                    || product.Routing.Current?.TransferType == Tech.Processs.TransferType.TransferOut
                    || product.Routing.Current?.TransferType == Tech.Processs.TransferType.TransferInOut))
            {
                ValidateProcessTransfer(barcode, workcell, product, process, wipResourceMove);
            }

            return product;
        }

        /// <summary>
        /// 验证工序交接
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="product"></param>
        /// <param name="process"></param>
        /// <param name="wipResourceMove"></param>
        private static void ValidateProcessTransfer(CollectBarcode barcode, Workcell workcell, product product,
            process process, WipResourceMove wipResourceMove)
        {
            //根据配置项判断是否校验工序交接
            var processTransferCheck = RT.Service.Resolve<ProcessTransferRecordController>()
                .GetProcessTransferCheck(workcell.ResourceId, wipResourceMove);

            if (processTransferCheck
                && process.WipProductProcessState == WipProductProcessState.Finish
                && process.ProcessId.HasValue)
            {
                //如果本工序启用了入站交接则判断是否有入站记录(工序启用入站出站则在入站校验：考虑重复采集时判断上次采集时间后是否有入站交接)
                if (process.TransferType == Tech.Processs.TransferType.TransferIn || process.TransferType == Tech.Processs.TransferType.TransferInOut)
                {
                    //获取当前工序是否有入站交接记录
                    var preRd = RT.Service.Resolve<ProcessTransferRecordController>()
                        .GetRecord(barcode.Code, TransferBarcodeType.SN, process.ProcessId.Value, 0, product.Routing.LastMoveDateTime);

                    if (preRd == null)
                        throw new ValidationException("条码[{0}]在工序[{1}]未转入交接".L10nFormat(barcode.Code, process.Name));
                }
                //如果本工序未启用入站交接则判断上工序是否启用出站交接，启用则判断上工序是否有出站交接记录（上工序过站时间后）
                else
                {
                    if (product.Routing.Current.TransferType == Tech.Processs.TransferType.TransferOut || product.Routing.Current.TransferType == Tech.Processs.TransferType.TransferInOut)
                    {
                        //获取上工序是否有出站交接记录
                        var preRd = RT.Service.Resolve<ProcessTransferRecordController>().GetRecord(barcode.Code, TransferBarcodeType.SN, product.Routing.Current.ProcessId.Value, 1, product.Routing.LastMoveDateTime);
                        if (preRd == null)
                            throw new ValidationException("条码[{0}]在工序[{1}]未转出交接".L10nFormat(barcode.Code, product.Routing.Current.Name));
                    }
                }
            }
        }

        /// <summary>
        /// 验证采集运行时工单
        /// </summary>
        /// <param name="product">运行时产品</param> 
        protected virtual void ValidateWorkOrder(product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

          

            if (product.WorkOrderMove.State != SIE.Core.WorkOrders.WorkOrderState.Release
                && product.WorkOrderMove.State != SIE.Core.WorkOrders.WorkOrderState.Producing)
            {
                throw new ValidationException("工单[{0}]状态为[{1}]，只有[{2}、{3}]才允许生产"
                    .L10nFormat(product.WorkOrderMove.No, product.WorkOrderMove.State.ToLabel(),
                        SIE.Core.WorkOrders.WorkOrderState.Release.ToLabel(),
                        SIE.Core.WorkOrders.WorkOrderState.Producing.ToLabel()));
            }
            if (product.WorkOrderMove.IsPause == YesNo.Yes)
            {
                throw new ValidationException("工单[{0}]已暂停，不允许生产".L10nFormat(product.WorkOrderMove.No));
            }
            if (product.Routing.Current == null
                && product.WorkOrderMove.OnlineQty - product.WorkOrderMove.ScrapQty >= product.WorkOrderMove.PlanQty)
            {
                throw new ValidationException("超工单,工单[{0}],计划数量[{1}],已上线数量[{2}],报废数量[{3}]"
                    .L10nFormat(product.WorkOrderMove.No, product.WorkOrderMove.PlanQty,
                        product.WorkOrderMove.OnlineQty, product.WorkOrderMove.ScrapQty));
            }
        }

        /// <summary>
        /// 产品运行时产品
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <param name="workOrder">工单</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="ValidationException">产品未上线</exception>
        protected virtual product CreateProduct(string puid, WorkOrderMove workOrder, CollectBarcode barcode, Workcell workcell)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            var product = new product();
            product.Puid = puid;
            product.WorkOrderId = workOrder.Id;
            product.ItemId = workOrder.ProductId;
            product.ItemExtProp = workOrder.ItemExtProp;
            product.ItemExtPropName = workOrder.ItemExtPropName;
            product.IsHold = false;
            var rutingWo = workOrder.PanelWorkOrder ?? workOrder;
            product.Routing.Processes.AddRange(GetRoutingProcess(rutingWo.Id, rutingWo.No));
            product.Qty = GetBarcodeQty(barcode);

            //上线工序列表
            List<process> startProcesses = new List<process>();

            var startProcessesOfAll = product.Routing.Processes
                .Where(x => (x.Sign & Tech.Routings.RoutingProcessSign.Start) == Tech.Routings.RoutingProcessSign.Start)
                .ToList();

            //非工序组的工序
            startProcesses.AddRange(startProcessesOfAll.Where((x => x.IsGroup != true)));

            //加载工序组下面的工序
            foreach (var groupProcess in startProcessesOfAll.Where(x => x.IsGroup == true))
            {
                var processesOfGroup = product.Routing.Processes
                    .Where(x => x.GroupId == groupProcess.GroupId && x.IsGroup != true)
                    .ToList();

                startProcesses.AddRange(processesOfGroup);
            }

            //当前工序不是上线工序时，抛出异常
            if (!startProcesses.Any(x => x.ProcessId == workcell.ProcessId))
            {
                var startProcess = startProcesses.Select(x => x.Name).Concat("、");

                throw new ValidationException("[{0}]产品未上线，上线工序应该为[{1}]"
                    .L10nFormat(barcode, startProcess));
            }

            //所有上线工序ID添加到后工序ID列表中
            product.Routing.Next.AddRange(startProcesses.Select(x => x.Id));

            MapBarcode(puid, barcode, barcode.Code);
            RuntimeController.Save(product);
            return product;
        }

        /// <summary>
        /// 获取工艺路线工序
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <param name="no">工单no</param>
        /// <returns>工序集合</returns>
        /// <exception cref="EntityNotFoundException">工单为空</exception>
        /// <exception cref="ValidationException">工艺路线为空</exception>
        public virtual List<process> GetRoutingProcess(double workOrderId, string no)
        {
            var controller = RT.Service.Resolve<WorkOrderController>();
            var routingProcessList = controller.GetRoutingProcess(workOrderId);
            if (routingProcessList.Count == 0)
                throw new ValidationException("工单[{0}]未创建工单工序清单".L10nFormat(no));

            var routingProcessIds = routingProcessList.Select(s => s.Id).ToList();
            var result = new List<process>();
            var processBomList = controller.GetWoProcessBom(workOrderId);
            var parameterList = controller.GetWorkOrderRoutingProcessParameter(routingProcessIds);

            ////工单工序清单->产品工序清单
            foreach (var routingProcess in routingProcessList)
            {
                process process = CreateProcess(routingProcess);

                var processParameterList = parameterList.Where(w => w.ProcessId == routingProcess.Id).ToList();
                ////生成下级工序
                foreach (var parameter in processParameterList)
                {
                    if (!parameter.NextProcessId.HasValue) continue; ////结束工序没有后工序
                    if (((ResultType)parameter.ResultType & ResultType.Pass) == ResultType.Pass)
                    {
                        if (process.Next.ContainsKey(ResultType.Pass))
                        {
                            process.Next[ResultType.Pass].Add(parameter.NextProcessId.Value);
                        }
                        else
                            process.Next.Add(ResultType.Pass, new List<double>() { parameter.NextProcessId.Value });
                    }

                    if (((ResultType)parameter.ResultType & ResultType.Fail) == ResultType.Fail)
                    {
                        if (process.Next.ContainsKey(ResultType.Fail))
                        {
                            process.Next[ResultType.Fail].Add(parameter.NextProcessId.Value);
                        }
                        else
                            process.Next.Add(ResultType.Fail, new List<double>() { parameter.NextProcessId.Value });
                    }
                    //添加自定义
                    if (((ResultType)parameter.ResultType & ResultType.Custom) == ResultType.Custom)
                    {
                        if (process.Next.ContainsKey(ResultType.Custom))
                        {
                            process.Next[ResultType.Custom].Add(parameter.NextProcessId.Value);
                        }
                        else
                            process.Next.Add(ResultType.Custom, new List<double>() { parameter.NextProcessId.Value });
                        if (!process.Script.ContainsKey(parameter.NextProcessId.Value))
                            process.Script.Add(parameter.NextProcessId.Value, parameter.Expression);
                    }

                    //添加可选路径
                    if (((ResultType)parameter.ResultType & ResultType.Optional) == ResultType.Optional)
                    {
                        if (process.Next.ContainsKey(ResultType.Optional))
                        {
                            process.Next[ResultType.Optional].Add(parameter.NextProcessId.Value);
                        }
                        else
                        {
                            process.Next.Add(ResultType.Optional, new List<double>() { parameter.NextProcessId.Value });
                        }

                        if (!process.OptionalPathDictionary.ContainsKey(parameter.NextProcessId.Value))
                        {
                            process.OptionalPathDictionary.Add(parameter.NextProcessId.Value, parameter.Description);
                        }
                    }
                }
                // 生成工序bom
                CreateProcessBom(processBomList, process);

                result.Add(process);
            }

            return result;
        }

        /// <summary>
        /// 生成工序bom
        /// </summary>
        /// <param name="processBomList"></param>
        /// <param name="process"></param>
        private void CreateProcessBom(EntityList<WorkOrderProcessBom> processBomList, process process)
        {
            var processBomsOfCurrentProcess =
                processBomList.Where(x => process.Id == x.RoutingProcessId).ToList();

            foreach (var processBom in processBomsOfCurrentProcess
                .Where(x => !x.IsAlternative))
            {
                bom bom = new bom
                {
                    BomId = processBom.Id,
                    ItemId = processBom.ItemId,
                    Qty = processBom.SingleQty,
                    ItemExtProp = processBom.ItemExtProp,
                    ItemExtPropName = processBom.ItemExtPropName,
                    //添加工步
                    WorkStepId = processBom.WorkStepId,
                    Priority = processBom.Priority,
                    AlterGroup = processBom.AlterGroup
                };

                //勾子 (电子MES继承此方法，写入扩展字段）
                SetBomExtProperty(bom, processBom);

                if (!processBom.Alter.IsNullOrEmpty())
                {
                    foreach (var alt in processBomsOfCurrentProcess
                        .Where(x => x.IsAlternative && x.Alter == processBom.Alter))
                    {
                        bom.AltBom.Add(new bom
                        {
                            BomId = alt.Id,
                            ItemId = alt.ItemId,
                            Qty = alt.SingleQty,
                            ItemExtProp = alt.ItemExtProp,
                            ItemExtPropName = alt.ItemExtPropName,
                            //添加工步
                            WorkStepId = processBom.WorkStepId,
                            Priority = alt.Priority,
                            AlterGroup = alt.AlterGroup
                        });
                    }
                }

                process.Boms.Add(bom);
            }
        }

        /// <summary>
        /// 创建工序
        /// </summary>
        /// <param name="routingProcess"></param>
        /// <returns></returns>
        public virtual process CreateProcess(WorkOrderRoutingProcess routingProcess)
        {
            return new process()
            {
                Id = routingProcess.Id,
                ProcessId = routingProcess.ProcessId,
                Name = routingProcess.Name,
                Type = routingProcess.ProcessType,
                Sign = routingProcess.Sign,
                Optional = routingProcess.IsOptional,
                Repeat = routingProcess.IsRepeat,
                CreateSku = routingProcess.CreateSku,
                IsCalculate = routingProcess.IsCalculate,
                IsGenerateTask = routingProcess.IsGenerateTask,
                IsRequirementTask = routingProcess.IsRequirementTask,
                IsBuckleMaterial = routingProcess.IsBuckleMaterial,
                IsPassRate = routingProcess.IsPassRate,
                IsBinding = routingProcess.IsBinding,
                IsUnBinding = routingProcess.IsUnBinding,
                StartProcess = routingProcess.StartProcess,
                NormalVictory = routingProcess.NormalVictoryId,
                RepairVictory = routingProcess.RepairVictoryId,
                IsStricter = routingProcess.IsStricter,
                Overtime = routingProcess.Overtime,
                Index = routingProcess.Index,
                MaxPassNum = routingProcess.MaxPassNum,
                EnableMoveInControl = routingProcess.EnableMoveInControl,
                TransferType = routingProcess.TransferType,
                ParentNodeId = routingProcess.ParentNodeId,
                GroupId = routingProcess.GroupId,
                IsGroup = routingProcess.IsGroup,
                IsNextMoveIn= routingProcess.IsNextMoveIn,
                Outsourcing= routingProcess.Outsourcing,
                IsPass = false,
            };
        }

        /// <summary>
        /// 设置采集运行时工序BOM扩展属性
        /// </summary>
        /// <param name="bom">采集运行时工序BOM</param>
        /// <param name="workOrderProcessBom">工单BOM</param>
        protected virtual void SetBomExtProperty(bom bom, WorkOrderProcessBom workOrderProcessBom)
        {


        }

        /// <summary>
        /// 克隆新产品
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="product">旧产品</param>
        /// <param name="qty">产品数量</param>
        /// <param name="versionOfSource">来源生产版本</param>
        /// <param name="workOrderId"></param>
        /// <returns>新产品</returns>
        protected virtual product CloneProduct(CollectBarcode barcode, product product, decimal qty, WipProductVersion versionOfSource, double? workOrderId)
        {
            var puid = Guid.NewGuid().ToString("N").ToUpper();
            var newProduct = product.Clone();
            newProduct.Puid = puid;
            newProduct.Qty = qty;

            //工单有传值，且传的工单与源工单不同时，变更工单和产品
            if (workOrderId.HasValue && newProduct.WorkOrderId != workOrderId.Value)
            {
                newProduct.WorkOrderId = workOrderId.Value;

                //如果工单变更了，对应的产品也一并变更
                newProduct.ItemId = newProduct.WorkOrder.ProductId;
                newProduct.ItemExtProp = newProduct.WorkOrder.ItemExtProp;
                newProduct.ItemExtPropName = newProduct.WorkOrder.ItemExtPropName;
            }

            if (product.Routing.Current != null && product.Routing.Current.IsEnd)
            {
                var wipProduct = CreateWipProduct(newProduct);
                CreateFinishWipProductVersion(wipProduct, newProduct, barcode, versionOfSource);
                return null;
            }
            else
            {
                MapBarcode(puid, barcode, barcode.Code);
                RuntimeController.Save(newProduct);
                var wipProduct = CreateWipProduct(newProduct);
                CreateWipProductVersion(wipProduct, newProduct, barcode, versionOfSource);
                return newProduct;
            }
        }
    }
}
