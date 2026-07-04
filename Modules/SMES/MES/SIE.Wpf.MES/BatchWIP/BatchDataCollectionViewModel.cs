using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.NumberRules;
using SIE.Core.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Exceptions;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Edge.Models;
using SIE.MES.WIP;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using SIE.Packages.Packings;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Workcell = SIE.MES.WIP.Workcell;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 批次数据采集泛型基类
    /// </summary> 
    /// <typeparam name="T">采集控制器WipController</typeparam>
    [RootEntity, Serializable]

    public class BatchDataCollectionViewModel<T> : BatchDataCollectionViewModel where T : WipController
    {
        /// <summary>
        /// 采集控制器，通过泛型参数确定控制器的类型
        /// </summary>
        protected virtual T Controller { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BatchDataCollectionViewModel()
        {
            Step = new BatchCollectStep(this);
            Controller = RT.Service.Resolve<T>();
            timer = new DispatcherTimer();
        }

        /// <summary>
        /// 重置界面数据
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(ResetType.None);
            IsMoveIn = true;
            IsReceiveContainer = false;
            InputBatch = null;
            ShowTipsInfo();
            OutputBatchList.Clear();
            RefreshInputBatch();
            ContainerNo = null;
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        /// <param name="batch">转出批次</param>
        /// <returns>采集数据</returns>
        CollectData InitCollectData(OutputBatch batch)
        {
            var collectData = new CollectData() { OutputBatch = batch };
            collectData.ScrapQty += batch.ScrapQty;
            InitializedCollectData(collectData);
            return collectData;
        }

        /// <summary>
        /// 所有采集重写该方法添加采集结果数据
        /// </summary>
        /// <param name="collectData">采集结果</param>
        protected virtual void InitializedCollectData(CollectData collectData)
        {
        }

        /// <summary>
        /// 批次转入
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void MoveIn(CollectBarcode barcode, Workcell workcell)
        {
            try
            {
                var productInfo = Controller.GetBatchMoveIn(barcode, workcell);
                SwitchWorkOrder(workcell, productInfo.WorkOrderId);
                ValidateTaskReport(productInfo.WorkOrderId, workcell);
                var product = Controller.BatchMoveIn(barcode, workcell);
                if (product.InputBatch != null)
                {
                    product.InputBatch.PropertyChanged += InputBatchPropertyChanged;
                    InputBatchList.Add(product.InputBatch);
                }
                RefreshBetail(PlugType.In);
                RefrshReportTasks(Core.Items.RetrospectType.Batch, false);
                DisplayBarCode = barcode.Code;
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();
                if (baseExc is ReMoveInException)
                {
                    var workOrderId = (double)baseExc.Data[ReMoveInException.WorkOrderId];
                    SwitchWorkOrder(workcell, workOrderId);
                    throw new ValidationException((baseExc as ReMoveInException).Message);
                }
                throw;
            }
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        internal override void SwitchWorkOrder(Workcell workcell, double workOrderId)
        {
            if (workOrderId != WorkOrderId)
            {
                var wo = RF.GetById<WorkOrder>(workOrderId);
                if (WorkOrder != null)
                    ShowTips("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                WorkOrder = wo;
                Controller.ChangeWipResourceWorkOrder(wo.Id, workcell);
                RefreshInputBatch();
                OutputBatchList.Clear();
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = workOrderId });
                UpdateWorkOrdeReportModel(wo.Id);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            CRT.MainThread.InvokeAsync(() => { LoadWorkstationData(); });
            RefreshInputBatch();
            base.Onload();
        }

        /// <summary>
        /// 界面关闭事件
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            timer?.Stop();
            timer = null;
        }

        /// <summary>
        /// 刷新批次出入站明细
        /// </summary>
        /// <param name="plugType">出入类型</param>
        protected virtual void RefreshBetail(PlugType plugType)
        {
            Task.Run(() =>
            {
                var info = new CollectDetailQuery()
                {
                    ResourceId = Workstation.ResourceId ?? 0,
                    ProcessId = Workstation.ProcessId ?? 0,
                    StationId = Workstation.StationId ?? 0,
                    OperateById = Workstation.EmployeeId ?? 0,
                    PlugType = plugType,
                    WorkOrderId = WorkOrderId ?? 0
                };
                AddBatchDetail(Controller.GetMoveInDetailEvent(info));
            });
        }

        /// <summary>
        /// 添加采集明细
        /// </summary>
        /// <param name="detailEvent">采集明细</param>
        private void AddBatchDetail(CollectDetailViewModel detailEvent)
        {
            if (detailEvent != null && !detailEvent.BatchNo.IsNullOrEmpty())
            {
                CRT.MainThread.InvokeAsync(() =>
                {
                    CollectDetailList.Add(detailEvent);
                });
            }
        }

        /// <summary>
        /// 工作单元信息改变
        /// </summary>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);
            LoadWorkstationData();
        }

        /// <summary>
        /// 初始化工位信息
        /// </summary>
        protected virtual void LoadWorkstationData()
        {
            try
            {
                var workcell = GetWorkcell();
                var wipLineWorkOrder = Controller.GetWipResourceWorkOrder(workcell);
                if (wipLineWorkOrder != null)
                {
                    var workOrder = RF.GetById<WorkOrder>(wipLineWorkOrder.WorkOrderId);
                    this.WorkOrder = workOrder;
                    RefreshInputBatch();
                }
                else
                    this.WorkOrder = null;
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 新批次转出
        /// </summary>
        /// <param name="input">转入批次</param>
        internal override void NewBatchOutput(InputBatch input)
        {
            if (Step.OutputCollectStep.BarcodeType == BarcodeType.ContainerNo)
            {
                FocuseBarcode();
                IsMoveIn = false;
                ShowTips("请扫描载具号".L10N());
                ContainerNo = input.BatchNo;
                return;
            }
            var workcell = GetWorkcell();
            OutputBatch outputBatch = new OutputBatch
            {
                BatchNo = input.BatchNo,
                ContainerNo = input.ContainerNo,
                Qty = input.RemainQty,
                InputDate = input.InputDate,
                OutputDate = DateTime.Now,
                IsNg = input.NgQty > 0,
                BarcodeType = Step.OutputCollectStep.BarcodeType,
                WorkOrderId = input.WorkOrderId,
                ScrapQty = input.ScrapQty,
                NgQty = input.NgQty,
                WorkOrder = WorkOrder,
            };
            RelationBatch relationBatch = new RelationBatch
            {
                InputBatch = input,
                OutputBatch = outputBatch,
                Qty = outputBatch.Qty,
            };
            GetDefect(relationBatch, input, workcell);
            outputBatch.RelationBatchList.Add(relationBatch);
            Controller.BatchMoveOut(InitCollectData(outputBatch), GetWorkcell());
            RefreshStatistics();
            ShowTips("{0}{1}转出成功".L10nFormat(outputBatch.BarcodeType.ToLabel().L10N(), outputBatch.BarcodeType == BarcodeType.ContainerNo ? outputBatch.ContainerNo + "[" + outputBatch.BatchNo + "]" : outputBatch.BatchNo));
            RefreshBetail(PlugType.Out);
            RefreshInputBatch();
            RefrshReportTasks(Core.Items.RetrospectType.Batch, true);
        }

        /// <summary>
        /// 转出批次
        /// </summary>
        /// <param name="batch">转出批次</param>
        internal override void BatchOutput(OutputBatch batch)
        {
            BatchOutputValidation(batch);
            //同一中批才能合批
            var midBatchId = batch.RelationBatchList.FirstOrDefault().InputBatch.WipBatchId;
            if (batch.RelationBatchList.Any(p => p.InputBatch.WipBatchId != midBatchId))
                throw new ValidationException("转入批次不满足合并规则，无法转出！".L10N());
            Controller.BatchMoveOut(InitCollectData(batch), GetWorkcell());
            RefreshStatistics();
            ShowTips("{0}{1}转出成功".L10nFormat(batch.BarcodeType.ToLabel().L10N(), batch.BarcodeType == BarcodeType.ContainerNo ? batch.ContainerNo + "[" + batch.BatchNo + "]" : batch.BatchNo));

            RefreshBetail(PlugType.Out);
            RefreshInputBatch(batch); ////先刷新入站集合，再从出站集合中移除对象
            RemoveOutputBatch(batch);
            RefrshReportTasks(Core.Items.RetrospectType.Batch, true);
        }

        /// <summary>
        /// 转出批次的验证
        /// </summary>
        /// <param name="batch">转出批次</param>
        internal virtual void BatchOutputValidation(OutputBatch batch)
        {
            var splitQty = InputBatchList.Sum(p => p.SplitQty);
            if (splitQty <= 0)
                throw new ValidationException("转出批次未关联批次，请在转入批次输入拆分数量".L10N());
            if (batch.RelationBatchList.Count == 0)
                throw new ValidationException("转出批次未关联批次，请在转入批次输入拆分数量".L10N());
            if (splitQty != batch.RelationBatchList.Sum(p => p.Qty))
                throw new ValidationException("拆分数量与转出批次数量不一致".L10N());
        }

        /// <summary>
        /// 移除转出批次
        /// </summary>
        /// <param name="batch">转出批次</param>
        private void RemoveOutputBatch(OutputBatch batch)
        {
            //batch.PropertyChanged -= OutBatch_PropertyChanged;
            OutputBatchList.Remove(batch);
        }

        /// <summary>
        /// 生成转出批次
        /// 界面触发
        /// </summary>
        /// <param name="barcode">采集条码</param>  
        /// <param name="type">条码类型</param> 
        internal override void GenerateOutputBatch(string barcode, BarcodeType type)
        {
            try
            {
                IsChanged = true;
                ValidateOutBarcode(barcode, type);
                MatchInputBatch(barcode, type);
                DebugWrite();
            }
            finally
            {
                IsChanged = false;
            }
        }

        /// <summary>
        /// 生成转出批次
        /// 命令触发
        /// </summary> 
        internal override void GenerateOutputBatch()
        {
            try
            {
                IsChanged = true;
                //出站类型为载具号不能通过界面生成子批次
                if (Step.OutputCollectStep.BarcodeType == BarcodeType.ContainerNo)
                    throw new ValidationException("生成子批次失败，出站条码类型为[{0}]".L10nFormat(BarcodeType.ContainerNo.ToLabel().L10N()));
                ValidateOutBarcode(null, BarcodeType.BatchBarocde);
                MatchInputBatch(string.Empty, BarcodeType.BatchBarocde);
                DebugWrite();
            }
            finally
            {
                IsChanged = false;
            }
        }

        /// <summary>
        /// 复制旧的批次版本数据到新的合并批次号
        /// </summary>
        /// <param name="childBatchNo"></param>
        /// <param name="workcell"></param>
        /// <param name="pBatchVersion"></param>
        /// <param name="pWipBatch"></param>
        /// <param name="splitQty"></param>
        /// <returns>返回复制后的合并批次号生产版本记录</returns>
        private BatchWipProductVersion CopyWipProductVersion(string childBatchNo, Workcell workcell, BatchWipProductVersion pBatchVersion, WipBatch pWipBatch, decimal splitQty)
        {
            var mergeBarcodeWipProductVersion = JsonConvert.DeserializeObject<BatchWipProductVersion>(JsonConvert.SerializeObject(pBatchVersion));
            mergeBarcodeWipProductVersion.BatchNo = childBatchNo;
            mergeBarcodeWipProductVersion.CurrentProcessId = null;
            mergeBarcodeWipProductVersion.ResourceId = workcell.ResourceId;
            mergeBarcodeWipProductVersion.StationId = workcell.StationId;
            mergeBarcodeWipProductVersion.ScrapQty = 0;
            mergeBarcodeWipProductVersion.Qty = splitQty;
            mergeBarcodeWipProductVersion.RemainQty = splitQty;
            mergeBarcodeWipProductVersion.GenerateId();
            mergeBarcodeWipProductVersion.PersistenceStatus = PersistenceStatus.New;

            //更新缺陷ID指向
            mergeBarcodeWipProductVersion.DefectList.Clear();
            mergeBarcodeWipProductVersion.RepaireList.Clear();
            mergeBarcodeWipProductVersion.ProcessList.Clear();
            return mergeBarcodeWipProductVersion;
        }

        /// <summary>
        /// 验证2个缺陷一致
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private bool ValidateSameDefectDetail(InputBatch list1, InputBatch list2)
        {
            if (list1.DefectList != null && list1.DefectList.Count > 0 && list2.DefectList != null && list2.DefectList.Count > 0)
            {
                var detail1 = list1.DefectList.Select(p => p.DefectId).OrderBy(p => p).ToList();
                var detail2 = list2.DefectList.Select(p => p.DefectId).OrderBy(p => p).ToList();

                return detail1.SequenceEqual(detail2);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 转入报废
        /// </summary>
        /// <param name="input"></param>
        /// <param name="scrapQty"></param>
        internal override void NewScrapInput(InputBatch input, decimal scrapQty)
        {
            input = RT.Service.Resolve<WipController>().GetInputBatchById(new List<double> { input.Id }).FirstOrDefault();
            if (input == null)
            {
                throw new ValidationException("转入批次为空".L10N());
            }
            // 生成通用报表
            var pBatchVersion = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductByNo(input.BatchNo);
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(new List<double> { pBatchVersion.WorkOrderId }).FirstOrDefault();
            if (wo == null)
            {
                throw new ValidationException("批次产品工单不存在");
            }
            if (wo.IsPause == YesNo.Yes)
            {
                throw new ValidationException("批次产品工单已暂停，不允许生产");
            }
            if (wo.State == Core.WorkOrders.WorkOrderState.Finish || wo.State == Core.WorkOrders.WorkOrderState.Close || wo.State == Core.WorkOrders.WorkOrderState.CancelRelease)
            {
                throw new ValidationException("批次产品工单状态不为生产或发放，不允许生产");
            }
            if (pBatchVersion == null)
            {
                throw new ValidationException("批次产品不存在");
            }
            if (pBatchVersion.RemainQty <= 0)
            {
                throw new ValidationException("批次剩余数量小于等于0，不允许报废".L10N());
            }
            if (pBatchVersion.IsFinish)
            {
                throw new ValidationException("批次{0}已完工，请刷新界面".L10nFormat(pBatchVersion.BatchNo));
            }
            if (pBatchVersion.RemainQty < scrapQty)
            {
                throw new ValidationException("批次剩余数量{0}小于报废数{1}，不允许报废".L10nFormat(pBatchVersion.RemainQty, scrapQty));
            }
            // 批次生成
            var pWipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatches(new List<string> { input.BatchNo }).FirstOrDefault();
            if (pWipBatch == null)
            {
                throw new ValidationException("转入批次为空");
            }

            // 报废
            RT.Service.Resolve<WipController>().ScrapInput(pBatchVersion, pWipBatch, input, scrapQty);
        }

        /// <summary>
        /// 新移除命令
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workcell"></param>
        internal override void NewRemoveInput(InputBatch input, Workcell workcell)
        {
            
            input = RT.Service.Resolve<WipController>().GetInputBatchById(new List<double> { input.Id }).FirstOrDefault();
            if (input == null)
            {
                throw new ValidationException("转入批次为空".L10N());
            }
            // 生成通用报表
            var pBatchVersion = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductByNo(input.BatchNo);
            if (pBatchVersion == null)
            {
                throw new ValidationException("批次产品不存在");
            }
            if (pBatchVersion.IsFinish)
            {
                throw new ValidationException("批次{0}已完工，请刷新界面".L10nFormat(pBatchVersion.BatchNo));
            }
            // 批次生成
            var pWipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatches(new List<string> { input.BatchNo }).FirstOrDefault();
            if (pWipBatch == null)
            {
                throw new ValidationException("转入批次为空");
            }

            // 记录移除并创建一条移除批次记录
            RT.Service.Resolve<WipController>().RemoveInput(pBatchVersion, pWipBatch, input, workcell);
            // 刷新批次采集
            RefreshBetail(PlugType.Remove);
        }

        /// <summary>
        /// 工位批次列表新生成子批次(合并)
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="workcell"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal override string NewGenerateMergeInput(EntityList<InputBatch> inputs, Workcell workcell, BarcodeType type)
        {
            var count = inputs.Count;
            inputs = RT.Service.Resolve<WipController>().GetInputBatchById(inputs.Select(p => p.Id).ToList(), workcell, inputs[0].WorkOrderId);
            var process = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWithViewProperty());
            if (inputs == null)
            {
                throw new ValidationException("转入批次为空".L10N());
            }
            if (inputs.Count != count)
            {
                throw new ValidationException("存在转入批次为空或已转出".L10N());
            }
            // 验证生产通用版本及缺陷记录
            var pBatchVersionList = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(inputs.Select(p => p.BatchNo).ToList());
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(pBatchVersionList.Select(p => p.WorkOrderId).Distinct().ToList());
            if (wos == null)
            {
                throw new ValidationException("批次产品工单不存在");
            }
            if (wos.Select(p => p.No).Distinct().Count() > 1)
            {
                throw new ValidationException("不同工单批次不能合并");
            }
            if (wos.Any(p => p.IsPause == YesNo.Yes))
            {
                throw new ValidationException("批次产品工单已暂停，不允许生产");
            }
            if (wos.Any(p => p.State == Core.WorkOrders.WorkOrderState.Finish || p.State == Core.WorkOrders.WorkOrderState.Close || p.State == Core.WorkOrders.WorkOrderState.CancelRelease))
            {
                throw new ValidationException("批次产品工单状态不为生产或发放，不允许生产");
            }
            if (pBatchVersionList == null)
            {
                throw new ValidationException("批次生产不存在");
            }
            if (pBatchVersionList.Any(p => p.RemainQty <= 0))
            {
                throw new ValidationException("批次剩余数量小于等于0，不能合并".L10N());
            }
            if (pBatchVersionList.Any(p => p.IsFinish))
            {
                throw new ValidationException("批次已完工，不能合并".L10N());
            }

            // 验证缺陷是否一致
            var firstInput = inputs.FirstOrDefault();
            if (firstInput != null && inputs.Any(p => p.DefectList.Count > 0))
            {
                foreach(var input in inputs)
                {
                    if (!ValidateSameDefectDetail(firstInput, input))
                    {
                        throw new ValidationException("批次缺陷不一致，不能合并".L10N());
                    }
                }
            }


            //合并数量
            var mergeQty = inputs.Sum(p => p.RemainQty);
            //合并不良数量
            var mergeNgQty = inputs.Sum(p => p.NgQty);

            // 父批数量为0，记录清空
            foreach(var input in inputs)
            {
                input.RemainQty = 0;
                input.PersistenceStatus = PersistenceStatus.Deleted;
            }

            // 生成wipBatch
            var pWipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(inputs.Select(p => p.BatchNo).ToList());
            if (pWipBatchs == null || pWipBatchs.Count == 0)
            {
                throw new ValidationException("原批次条码不存在".L10N());
            }
            var config = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(inputs[0].WorkOrderId);
            if (config == null || config.NumberRuleId == 0 || config.NumberRuleId == null)
            {
                throw new ValidationException("未找到批次打印设置".L10N());
            }
            var childBarcode = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
            var childWipBatch = JsonConvert.DeserializeObject<WipBatch>(JsonConvert.SerializeObject(pWipBatchs[0]));
            childWipBatch.GenerateId();
            childWipBatch.BatchList.Clear();
            childWipBatch.BatchNo = childBarcode;
            childWipBatch.RemainQty = childWipBatch.Qty = mergeQty;
            childWipBatch.ScrapQty = null;
            childWipBatch.PersistenceStatus = PersistenceStatus.New;
            childWipBatch.RangeId = null;

            //创建批次关系
            var childRelation = RT.Service.Resolve<BatchManageController>().CreateSubWipBatchRelationNoSave(childWipBatch.BatchNo, childWipBatch.BatchNo, childWipBatch.Qty, childWipBatch.WorkOrderId, null);
            //复制拆分前的运行时
            var oldRuntimeProduct = RT.Service.Resolve<RuntimeController>().FindProduct(inputs[0].BatchNo, Core.Barcodes.BarcodeType.BatchBarocde);
            //克隆运行时
            var childRuntimeProduct = oldRuntimeProduct.Clone();
            childRuntimeProduct.Qty = mergeQty;
            //创建一个PUID用于后面与拆分批次号关联
            var puid = Guid.NewGuid().ToString("N").ToUpper();
            //复制新生成的拆分批次号的Puid为新的Puid
            childRuntimeProduct.Puid = puid;
            //创建新条码的工艺路线布局
            RT.Service.Resolve<WipController>().SaveBatchRuntimeRouting(childWipBatch.WorkOrderId, childBarcode, pWipBatchs[0].BatchNo);
            var runtimeController = RT.Service.Resolve<RuntimeController>();
            //拆分批次号与产品运行时Puid做关联
            runtimeController.MapPuid(new CollectBarcode() { Code = childBarcode, Type = BarcodeType.BatchBarocde }, puid);

            //复制原批次版本数据生成新的拆分批次
            BatchWipProductVersion childBatchVersion = CopyWipProductVersion(childBarcode, workcell, pBatchVersionList[0], pWipBatchs[0], mergeQty);

            //更新原批次号的当前数量
            foreach (var item in pWipBatchs)
            {
                if (item.Qty != 0)
                {
                    item.EditQtyProcessCode = process?.Code;
                }
                item.Qty = 0;
                item.RemainQty = 0;
            }

            List<CollectBarcode> collectBarcodes = new List<CollectBarcode>();
            var batchSplitMergeRecords = new EntityList<BatchSplitMergeRecord>();

            //更新原批次生产版本的剩余数量和当前数量
            foreach (var item in pBatchVersionList)
            {
                //记录拆分关系
                var batchSplitMerge = new BatchSplitMergeRecord()
                {
                    BatchOperationType = BatchOperationType.Merge,
                    InputBatchNo = item.BatchNo,
                    InputQty = item.RemainQty,
                    OutputBatchNo = childBarcode,
                    VersionId = childBatchVersion.Id,
                    OutputQty = childWipBatch.Qty,
                };
                batchSplitMergeRecords.Add(batchSplitMerge);
                collectBarcodes.Add(new CollectBarcode()
                {
                    Code = item.BatchNo,
                    BarcodeType = BarcodeType.BatchBarocde,
                    Type = BarcodeType.BatchBarocde
                });
                item.RemainQty = 0;
            }
            //批次关系数量更新为0 避免后面继续过站
            EntityList<BatchRelation> batchRelations = RT.Service.Resolve<BatchManageController>().GetBatchRelations(collectBarcodes);
            foreach (var item in batchRelations)
            {
                item.RemainQty = 0;
                item.Qty = 0;
                item.PersistenceStatus = PersistenceStatus.Modified;
            }

            // 保存
            RT.Service.Resolve<WipController>().SplitMergeSaveInfo(pBatchVersionList, childBatchVersion, childRelation, inputs, pWipBatchs, childWipBatch, childRuntimeProduct, batchRelations, batchSplitMergeRecords, workcell, firstInput.DefectList.Select(p => p.Defect).AsEntityList(), mergeNgQty);

            return childBarcode;
        }

        /// <summary>
        /// 工位批次列表新生成子批次(拆分)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workcell"></param>
        /// <param name="splitQty"></param>
        /// <param name="type"></param>
        internal override string NewGenerateSplitInput(InputBatch input, Workcell workcell, decimal splitQty, BarcodeType type)
        {
            // 
            if (splitQty <= 0)
            {
                throw new ValidationException("拆分数量必须大于0".L10N());
            }
            input = RT.Service.Resolve<WipController>().GetInputBatchById(input.Id, workcell, input.WorkOrderId);
            var process = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWithViewProperty());
            if (input == null)
            {
                throw new ValidationException("转入批次为空或已转出".L10N());
            }
            // 验证生产通用版本
            var pBatchVersion = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductByNo(input.BatchNo);
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(new List<double> { pBatchVersion.WorkOrderId}).FirstOrDefault();
            if (wo == null)
            {
                throw new ValidationException("批次产品工单不存在");
            }
            if (wo.IsPause == YesNo.Yes)
            {
                throw new ValidationException("批次产品工单已暂停，不允许生产");
            }
            if (wo.State == Core.WorkOrders.WorkOrderState.Finish || wo.State == Core.WorkOrders.WorkOrderState.Close || wo.State == Core.WorkOrders.WorkOrderState.CancelRelease)
            {
                throw new ValidationException("批次产品工单状态不为生产或发放，不允许生产");
            }
            if (pBatchVersion == null)
            {
                throw new ValidationException("批次产品不存在");
            }
            if (pBatchVersion.RemainQty <= 0)
            {
                throw new ValidationException("批次{0}剩余数量小于等于0，不能拆分".L10nFormat(pBatchVersion.BatchNo));
            }
            if (pBatchVersion.IsFinish)
            {
                throw new ValidationException("批次{0}已完工，不能拆分".L10nFormat(pBatchVersion.BatchNo));
            }
            if (pBatchVersion.RemainQty < splitQty)
            {
                throw new ValidationException("批次{0}剩余数量{1}小于拆分数量{2}，不能拆分".L10nFormat(pBatchVersion.BatchNo, pBatchVersion.RemainQty, splitQty));
            }

            // 父批数量减少，记录减少
            pBatchVersion.RemainQty -= splitQty;
            input.RemainQty -= splitQty;

            //生成wipBatch
            var pWipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatches(new List<string> { input.BatchNo}).FirstOrDefault();
            var config = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(input.WorkOrderId);
            if (config == null || config.NumberRuleId == 0 || config.NumberRuleId == null)
            {
                throw new ValidationException("未找到批次打印设置".L10N());
            }
            var childBarcode = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
            var childWipBatch = JsonConvert.DeserializeObject<WipBatch>(JsonConvert.SerializeObject(pWipBatch));
            childWipBatch.GenerateId();
            childWipBatch.BatchList.Clear();
            childWipBatch.BatchNo = childBarcode;
            childWipBatch.RemainQty = childWipBatch.Qty = splitQty;
            childWipBatch.ScrapQty = null;
            childWipBatch.PersistenceStatus = PersistenceStatus.New;
            childWipBatch.RangeId = null;

            //创建批次关系
            var childRelation = RT.Service.Resolve<BatchManageController>().CreateSubWipBatchRelationNoSave(childWipBatch.BatchNo, childWipBatch.BatchNo, childWipBatch.Qty, childWipBatch.WorkOrderId, null);
            //复制拆分前的运行时
            var oldRuntimeProduct = RT.Service.Resolve<RuntimeController>().FindProduct(input.BatchNo, Core.Barcodes.BarcodeType.BatchBarocde);
            //克隆运行时
            var childRuntimeProduct = oldRuntimeProduct.Clone();
            childRuntimeProduct.Qty = splitQty;
            //创建一个PUID用于后面与拆分批次号关联
            var puid = Guid.NewGuid().ToString("N").ToUpper();
            //复制新生成的拆分批次号的Puid为新的Puid
            childRuntimeProduct.Puid = puid;
            //创建新条码的工艺路线布局
            RT.Service.Resolve<WipController>().SaveBatchRuntimeRouting(childWipBatch.WorkOrderId, childBarcode, pWipBatch.BatchNo);
            var runtimeController = RT.Service.Resolve<RuntimeController>();
            //拆分批次号与产品运行时Puid做关联
            runtimeController.MapPuid(new CollectBarcode() { Code = childBarcode, Type = BarcodeType.BatchBarocde }, puid);

            //复制原批次版本数据生成新的拆分批次
            BatchWipProductVersion childBatchVersion = CopyWipProductVersion(childBarcode, workcell, pBatchVersion, pWipBatch, splitQty);

            // 更新wipBatch
            pWipBatch.RemainQty -= splitQty;
            if (pWipBatch.Qty != splitQty)
            {
                pWipBatch.EditQtyProcessCode = process?.Code;
            }
            pWipBatch.Qty -= splitQty;

            // 更新batchRelaiton
            EntityList<BatchRelation> batchRelations = RT.Service.Resolve<BatchManageController>().GetBatchRelations(new List<CollectBarcode> { new CollectBarcode()
                        {
                            Code = pBatchVersion.BatchNo,
                            BarcodeType = BarcodeType.BatchBarocde,
                            Type = BarcodeType.BatchBarocde
                        }});
            foreach (var item in batchRelations)
            {
                item.RemainQty -= splitQty;
                item.Qty -= splitQty;
                item.PersistenceStatus = PersistenceStatus.Modified;
            }

            EntityList<BatchSplitMergeRecord> batchSplitMergeRecords = new EntityList<BatchSplitMergeRecord>();
            // 创建一条批次合并拆分记录
            var batchSplitMerge = new BatchSplitMergeRecord()
            {
                BatchOperationType = BatchOperationType.Split,
                InputBatchNo = input.BatchNo,
                InputQty = splitQty,
                OutputBatchNo = childBarcode,
                VersionId = pBatchVersion.Id,
                OutputQty = splitQty,
            };
            batchSplitMergeRecords.Add(batchSplitMerge);

            EntityList<Defect> defects = new EntityList<Defect>();
            decimal ngQty = 0;
            if (input.DefectList != null && input.DefectList.Count > 0)
            {
                defects.AddRange(input.DefectList.Select(p => p.Defect).AsEntityList());
                ngQty = splitQty;
                input.NgQty -= ngQty;
            }
            // 保存
            RT.Service.Resolve<WipController>().SplitMergeSaveInfo(new EntityList<BatchWipProductVersion> { pBatchVersion }, childBatchVersion, childRelation,new EntityList<InputBatch> { input }, new EntityList<WipBatch> { pWipBatch }, childWipBatch, childRuntimeProduct, batchRelations, batchSplitMergeRecords, workcell, defects, ngQty);
            

            return childBarcode;
        }

        /// <summary>
        /// 匹配转入批次
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        void MatchInputBatch(string barcode, BarcodeType type)
        {
            if (type == BarcodeType.ContainerNo && ContainerNo.IsNullOrEmpty())
            {
                RT.Service.Resolve<WipController>().MatchInputBatch(barcode, GetWorkcell(), WorkOrderId.Value);
                ContainerNo = barcode;
                ShowTips("请扫描载具号".L10N());
                return;
            }
            Controller.ValidateOutputBarcode(barcode, type);
            // 把载具号绑定在input上并转出
            var workcell = GetWorkcell();
            InputBatch input = new InputBatch();
            if (type == BarcodeType.ContainerNo)
            {
                input = RT.Service.Resolve<WipController>().MatchInputBatch(ContainerNo, workcell, WorkOrderId.Value);
                input.ContainerNo = barcode;
            }
            else if (type == BarcodeType.BatchBarocde)
            {
                input = RT.Service.Resolve<WipController>().MatchInputBatch(barcode, workcell, WorkOrderId.Value);
            }
            else
            {
                throw new ValidationException("条码类型不为批次条码或载具号".L10N());
            }
            OutputBatch outputBatch1 = new OutputBatch
            {
                BatchNo = input.BatchNo,
                ContainerNo = input.ContainerNo,
                Qty = input.RemainQty,
                InputDate = input.InputDate,
                OutputDate = DateTime.Now,
                IsNg = input.NgQty > 0,
                BarcodeType = type,
                WorkOrderId = input.WorkOrderId,
                ScrapQty = input.ScrapQty,
                NgQty = input.NgQty,
            };
            RelationBatch relationBatch = new RelationBatch
            {
                InputBatch = input,
                OutputBatch = outputBatch1,
                Qty = outputBatch1.Qty,
            };
            GetDefect(relationBatch, input, workcell);
            outputBatch1.RelationBatchList.Add(relationBatch);
            Controller.BatchMoveOut(InitCollectData(outputBatch1), GetWorkcell());
            RefreshStatistics();
            ShowTips("{0}{1}转出成功".L10nFormat(outputBatch1.BarcodeType.ToLabel().L10N(), outputBatch1.BarcodeType == BarcodeType.ContainerNo ? outputBatch1.ContainerNo + "[" + outputBatch1.BatchNo + "]" : outputBatch1.BatchNo));
            RefreshBetail(PlugType.Out);
            RefreshInputBatch();
            RefrshReportTasks(Core.Items.RetrospectType.Batch, true);
        }

        /// <summary>
        /// 创建缺陷记录
        /// </summary>
        /// <param name="relationBatch"></param>
        /// <param name="input"></param>
        /// <param name="workcell"></param>
        private void GetDefect(RelationBatch relationBatch, InputBatch input, Workcell workcell)
        {
            if (input.DefectList.Count > 0)
            {
                // 生成缺陷信息
                BatchWipProductDefect batchWipProductDefect = new BatchWipProductDefect
                {
                    BatchNo = input.BatchNo,
                    ContainerNo = input.ContainerNo,
                    Qty = input.RemainQty,
                    ResourceId = workcell.ResourceId,
                    ProcessId = workcell.ProcessId,
                    StationId = workcell.StationId,
                };
                foreach (var defect in input.DefectList)
                {
                    BatchWipProductDefectDetail batchWipProductDefectDetail = new BatchWipProductDefectDetail
                    {
                        Defect = defect.Defect,
                        BatchWipProductDefect = batchWipProductDefect,
                    };
                    batchWipProductDefect.DetailList.Add(batchWipProductDefectDetail);
                }
                relationBatch.BatchWipProductDefects.Add(batchWipProductDefect);
            }
        }

        /// <summary>
        /// 验证转出条码
        /// </summary> 
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        internal virtual void ValidateOutBarcode(string barcode, BarcodeType type)
        {
            if (InputBatchList.Count <= 0)
                throw new ValidationException("生成转出批次失败，转入批次为空，请先转入".L10N());
            if (OutputBatchList.Count >= 1)
            {
                var output = OutputBatchList[0];
                var code = output.SubBatchNo.IsNullOrEmpty() ? output.ContainerNo.IsNullOrEmpty() ? output.BatchNo : output.ContainerNo : output.SubBatchNo;
                throw new ValidationException("已存在[转出批次：{0}]，请先转出！".L10nFormat(code));
            }
        }
    }

    /// <summary>
    /// 批次采集步骤
    /// </summary>
    public class BatchCollectStep : CollectStep
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vm">采集视图模型</param>
        public BatchCollectStep(DataCollectionViewModel vm) : base(vm)
        {
        }

#pragma warning disable S2372 // Exceptions should not be thrown from property getters
        /// <summary>
        /// 入站采集步骤
        /// </summary>
        public ProcessCollectStep InputCollectStep
        {
            get
            {
                var step = ProcessSteps?.FirstOrDefault(p => p.PlugType == PlugType.In);
                if (step == null && _viewModel.Workstation.Process != null)
                {
                    throw new ValidationException("工序[{0}]未定义入站采集步骤".L10nFormat(_viewModel.Workstation.Process?.Name));
                }
                return step;
            }
        }

        /// <summary>
        /// 出站采集步骤
        /// </summary>
        public ProcessCollectStep OutputCollectStep
        {
            get
            {
                var step = ProcessSteps?.FirstOrDefault(p => p.PlugType == PlugType.Out);
                if (step == null && _viewModel.Workstation.Process != null &&
                    _viewModel.Workstation.Process.Type != ProcessType.BatchPacking && _viewModel.Workstation.Process.Type != ProcessType.BatchFix)
                {
                    throw new ValidationException("工序[{0}]未定义出站采集步骤".L10nFormat(_viewModel.Workstation.Process?.Name));
                }
                return step;
            }
        }
#pragma warning restore S2372 // Exceptions should not be thrown from property getters

        /// <summary>
        /// 出站是否生成子批次
        /// </summary>
        public bool IsGenerateBatch
        {
            get
            {
                var step = ProcessSteps?.FirstOrDefault(p => p.PlugType == PlugType.Out);
                return step != null && step.IsGenerateBatch;
            }
        }
    }

    /// <summary>
    /// 批次数据采集视图模型基类
    /// </summary>
    public class BatchDataCollectionViewModel : DataCollectionViewModel
    {
        /// <summary>
        /// 批次采集步骤
        /// </summary>
        internal new virtual BatchCollectStep Step
        {
            get { return base.Step as BatchCollectStep; }
            set { base.Step = value; }
        }

        /// <summary>
        /// 定时器
        /// </summary>
        protected DispatcherTimer timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchDataCollectionViewModel()
        {
            PrintHelper = new PrintHelper();
            IsMoveIn = true;
        }

        #region 批次条码打印
        /// <summary>
        /// 批次条码打印
        /// </summary>
        PrintHelper PrintHelper { get; }

        /// <summary>
        /// 打印批次条码
        /// </summary>
        /// <param name="barcode">批次条码</param>
        internal void PrintBarcode(string barcode)
        {
            if (Step.OutputCollectStep.BarcodeType == BarcodeType.ContainerNo)
                throw new ValidationException("打印失败，出站条码类型为[{0}]".L10nFormat(BarcodeType.ContainerNo.ToLabel().L10N()));
            PrintHelper.PrintBarcode(barcode, WorkOrder.Id);
        }
        #endregion

        #region 是否转入 IsMoveIn
        /// <summary>
        /// 是否转入
        /// </summary>
        [Label("是否转入")]
        public static readonly Property<bool> IsMoveInProperty = P<BatchDataCollectionViewModel>.Register(e => e.IsMoveIn, new PropertyMetadata<bool>
        {
            PropertyChangedCallBack = (s, e) => (s as BatchDataCollectionViewModel).OnIsMoveInChanged(e),
        });

        /// <summary>
        /// 是否转入属性变更事件
        /// </summary>
        /// <param name="e">参数</param>
        private void OnIsMoveInChanged(ManagedPropertyChangedEventArgs e)
        {
            ShowTipsInfo();
            Barcode = null;
            ContainerNo = null;
        }

        /// <summary>
        /// 显示出入站提示信息
        /// </summary>
        protected virtual void ShowTipsInfo()
        {
            if (Step == null || Step.ProcessSteps == null)
                return;
            if (Step.InputCollectStep == null || Step.OutputCollectStep == null)
                ShowTips("请扫描入站条码".L10N());
            else
                ShowTips("请扫描{0}{1}".L10nFormat(IsMoveIn ? "入站".L10N() : "出站".L10N(), IsMoveIn ? Step.InputCollectStep.BarcodeType.ToLabel().L10N() : "批次条码".L10N()));
        }

        /// <summary>
        /// 是否转入
        /// </summary>
        public bool IsMoveIn
        {
            get { return this.GetProperty(IsMoveInProperty); }
            set { this.SetProperty(IsMoveInProperty, value); }
        }
        #endregion

        #region 是否接收移除载具号 IsReceiveContainer
        /// <summary>
        /// 是否接收移除载具号
        /// </summary>
        [Label("是否接收移除载具号")]
        public static readonly Property<bool> IsReceiveContainerProperty = P<BatchDataCollectionViewModel>.Register(e => e.IsReceiveContainer);

        /// <summary>
        /// 是否接收移除载具号
        /// </summary>
        public bool IsReceiveContainer
        {
            get { return this.GetProperty(IsReceiveContainerProperty); }
            set { this.SetProperty(IsReceiveContainerProperty, value); }
        }
        #endregion

        #region 绑定载具的批次号 ContainerNo
        /// <summary>
        /// 绑定载具的批次号
        /// </summary>
        [Label("绑定载具的批次号")]
        public static readonly Property<string> ContainerNoProperty = P<BatchDataCollectionViewModel>.Register(e => e.ContainerNo);

        /// <summary>
        /// 绑定载具的批次号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
            set { this.SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 转入批次 InputBatch
        /// <summary>
        /// 转入批次
        /// </summary>
        [Label("转入批次")]
        public static readonly Property<InputBatch> InputBatchProperty = P<BatchDataCollectionViewModel>.Register(e => e.InputBatch);

        /// <summary>
        /// 转入批次
        /// </summary>
        public InputBatch InputBatch
        {
            get { return this.GetProperty(InputBatchProperty); }
            set { this.SetProperty(InputBatchProperty, value); }
        }
        #endregion 

        #region 转入批次列表 InputBatchList
        /// <summary>
        /// 转入批次列表
        /// </summary>
        [Label("转入批次列表")]
        public static readonly ListProperty<EntityList<InputBatch>> InputBatchListProperty = P<BatchDataCollectionViewModel>.RegisterList(e => e.InputBatchList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<InputBatch>()
        });

        /// <summary>
        /// 转入批次列表
        /// </summary>
        public EntityList<InputBatch> InputBatchList
        {
            get { return this.GetLazyList(InputBatchListProperty); }
        }
        #endregion

        #region 转出批次列表 InputBatchList
        /// <summary>
        /// 转出批次列表
        /// </summary>
        [Label("转出批次列表")]
        public static readonly ListProperty<EntityList<OutputBatch>> OutputBatchListProperty = P<BatchDataCollectionViewModel>.RegisterList(e => e.OutputBatchList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<OutputBatch>()
        });

        /// <summary>
        /// 转出批次列表
        /// </summary>
        public EntityList<OutputBatch> OutputBatchList
        {
            get { return this.GetLazyList(OutputBatchListProperty); }
        }
        #endregion   

        /// <summary>
        /// 刷新转入批次
        /// </summary>
        internal virtual void RefreshInputBatch(OutputBatch outputBatch = null)
        {
            try
            {
                if (WorkOrder == null)
                {
                    return;
                }
                InputBatchList.ForEach(e => e.PropertyChanged -= InputBatchPropertyChanged);
                InputBatchList.Clear();
                var workcell = GetWorkcell();
                var inputBatchs = RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, WorkOrder.Id);
                inputBatchs.ForEach(e => e.PropertyChanged += InputBatchPropertyChanged);
                InputBatchList.MarkSaved();
                InputBatchList.AddRange(inputBatchs);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 拆分数量是否在修改，控制转入转出批次属性变更
        /// </summary>
        protected bool IsChanged { get; set; }

        /// <summary>
        /// 转入批次属性变更事件
        /// </summary>
        /// <param name="sender">转入批次</param>
        /// <param name="e">参数</param>
        public void InputBatchPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(InputBatch.SplitQty) || IsChanged)
            {
                return;
            }
            try
            {
                IsChanged = true;
                var inputBatch = sender as InputBatch;
                var outputBatch = OutputBatchList.FirstOrDefault(p => !p.IsNg);
                if (outputBatch == null)
                {
                    inputBatch.SplitQty = 0;
                }
                else
                {
                    if (inputBatch.SplitQty == 0)
                    {
                        var relation = outputBatch.RelationBatchList.FirstOrDefault(p => p.InputBatchId == inputBatch.Id);
                        if (relation != null)
                        {
                            outputBatch.RelationBatchList.Remove(relation);
                            if (outputBatch.RelationBatchList.Count == 0)
                                outputBatch.BatchNo = string.Empty;
                            outputBatch.Qty = outputBatch.RelationBatchList.Sum(p => p.Qty);
                            DebugWrite();
                        }
                        return;
                    }
                    if (outputBatch.IsGenerateBatch)
                    {
                        if (OutputBatchList.Count > 0 && OutputBatchList[0].RelationBatchList.Count > 0 && OutputBatchList[0].RelationBatchList[0].InputBatch.WipBatchId != inputBatch.WipBatchId)
                        {
                            inputBatch.SplitQty = 0;
                            ShowInstantMessage("不同生产批次不能合并".L10N());
                            return;
                        }
                        if (outputBatch.MaxQty.HasValue)
                        {
                            var allSplitQty = InputBatchList.Where(p => p.Id != inputBatch.Id).Sum(p => p.SplitQty);
                            var canSplitQty = outputBatch.MaxQty.Value - allSplitQty;
                            if (inputBatch.SplitQty > canSplitQty)
                            {
                                inputBatch.SplitQty = canSplitQty;
                                ShowInstantMessage("载具最大容量为[{0}],当前可拆分数量为[{1}]".L10nFormat(outputBatch.MaxQty.Value, canSplitQty > 0 ? canSplitQty : 0));
                            }
                        }
                        RefreshOutputBatch(inputBatch);
                    }
                    else
                    {
                        //不生成子批转出，则关联转入批次必须只能有一笔，且数量一致
                        outputBatch.RelationBatchList.ForEach(p => p.InputBatch.SplitQty = 0);
                        outputBatch.RelationBatchList.Clear();
                        inputBatch.SplitQty = inputBatch.RemainQty;
                        outputBatch.MaxQty = outputBatch.Qty = inputBatch.RemainQty;
                        outputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = inputBatch, Qty = inputBatch.RemainQty });
                        outputBatch.BatchNo = inputBatch.BatchNo;
                        outputBatch.SubBatchNo = inputBatch.SubBatchNo;
                    }
                    outputBatch.Qty = outputBatch.RelationBatchList.Sum(p => p.Qty);
                }
                DebugWrite();
            }
            finally
            {
                IsChanged = false;
            }
        }

        /// <summary>
        /// 移除转入批次
        /// </summary>
        internal void RemoveInputBatch(InputBatch inputBatch, string containerNo)
        {
            if (inputBatch == null)
            {
                throw new ValidationException("转入批次不能为空".L10N());
            }
            RT.Service.Resolve<WipController>().RemoveInputBatch(inputBatch.Id, GetWorkcell(), containerNo);
            inputBatch.PropertyChanged -= InputBatchPropertyChanged;
            InputBatchList.Remove(inputBatch);
            RemoveMapInputBatch(inputBatch);
            ShowTips("转入批次[{0}]移除成功".L10nFormat(inputBatch.SubBatchNo.IsNotEmpty() ? inputBatch.SubBatchNo : inputBatch.BatchNo));
            IsReceiveContainer = false;
            InputBatch = null;
        }

        /// <summary>
        /// 移除转入批次
        /// </summary>
        /// <param name="inputBatch">转入批次</param>
        internal void RemoveInputBatch(InputBatch inputBatch)
        {
            if (ValidateIsMapContanier(inputBatch))
            {
                IsReceiveContainer = true;
                ShowTips("请扫描移除关联载具".L10N());
            }
            else
            {
                RemoveInputBatch(inputBatch, string.Empty);
                InputBatchRemoved(inputBatch);
            }
        }

        /// <summary>
        /// 移除转出批次
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        internal virtual void RemoveOutBatch(OutputBatch outputBatch)
        {
            OutputBatchList.Remove(outputBatch);
        }

        /// <summary>
        /// 刷新转出批次
        /// </summary>
        /// <param name="inputBatch">转入批次</param>
        public void RefreshOutputBatch(InputBatch inputBatch)
        {
            var outputBatch = OutputBatchList.FirstOrDefault(p => !p.IsNg);
            if (outputBatch == null)
            {
                return;
            }
            var matchBatch = outputBatch.RelationBatchList.FirstOrDefault(p => p.InputBatchId == inputBatch.Id);
            if (matchBatch != null)
            {
                //已匹配，修改拆分数量，拆分数量为0是移除关联
                if (inputBatch.SplitQty <= 0)
                    outputBatch.RelationBatchList.Remove(matchBatch);
                else
                    matchBatch.Qty = inputBatch.SplitQty;
            }
            else
            {
                //未匹配，数量大于0添加关联
                if (inputBatch.SplitQty > 0)
                    outputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = inputBatch, Qty = inputBatch.SplitQty });
            }
            //outputBatch.Qty = outputBatch.RelationBatchList.Sum(p => p.Qty);
            if (outputBatch.RelationBatchList.Count > 0)
                outputBatch.BatchNo = outputBatch.RelationBatchList[0].InputBatch.BatchNo;
            else
                outputBatch.BatchNo = string.Empty;
        }

        /// <summary>
        /// 移除匹配转入批次
        /// </summary>
        /// <param name="inputBatch">转入批次</param>
        internal void RemoveMapInputBatch(InputBatch inputBatch)
        {
            foreach (OutputBatch item in OutputBatchList)
            {
                var relation = item.RelationBatchList.FirstOrDefault(p => p.InputBatchId == inputBatch.Id);
                if (relation == null)
                    continue;
                item.RelationBatchList.Remove(relation);
                item.Qty = item.RelationBatchList.Sum(p => p.Qty);
            }
        }

        /// <summary>
        /// 生产转出批次
        /// </summary>
        /// <param name="barcode">采集条码</param>  
        /// <param name="type">条码类型</param> 
        internal virtual void GenerateOutputBatch(string barcode, BarcodeType type)
        {
        }

        /// <summary>
        /// 生成转出批次
        /// </summary> 
        internal virtual void GenerateOutputBatch()
        {
        }

        /// <summary>
        /// 生成批次拆分
        /// </summary>
        /// <param name="input">转入</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="splitQty">拆分数量</param>
        /// <param name="type">条码类型</param>
        internal virtual string NewGenerateSplitInput(InputBatch input, Workcell workcell, decimal splitQty, BarcodeType type)
        {
            return string.Empty;
        }

        /// <summary>
        /// 生成批次合并
        /// </summary>
        /// <param name="inputs">转入列表</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="type">条码类型</param>
        /// <returns></returns>
        internal virtual string NewGenerateMergeInput(EntityList<InputBatch> inputs, Workcell workcell, BarcodeType type)
        {
            return string.Empty;
        }

        /// <summary>
        /// 新移除命令
        /// </summary>
        /// <param name="input">转入批次</param>
        /// <param name="workcell"></param>
        internal virtual void NewRemoveInput(InputBatch input, Workcell workcell)
        {

        }

        /// <summary>
        /// 转入报废
        /// </summary>
        /// <param name="input"></param>
        /// <param name="scrapQty"></param>
        internal virtual void NewScrapInput(InputBatch input, decimal scrapQty)
        {

        }

        /// <summary>
        /// 新批次转出
        /// </summary>
        /// <param name="input"></param>
        internal virtual void NewBatchOutput(InputBatch input)
        {
            
        }

        /// <summary>
        /// 批次转出
        /// </summary>
        /// <param name="batch">转出批次</param>
        internal virtual void BatchOutput(OutputBatch batch)
        {
        }

        /// <summary>
        /// 工序变更事件，通知生产子批次和打印命令隐藏
        /// </summary>
        protected override void ProcessChanged(Process process)
        {
            RT.EventBus.Publish(new ProcessChangedEvent() { type = Step.OutputCollectStep?.BarcodeType, });
        }

        /// <summary>
        /// 清除提示信息
        /// </summary>
        internal void ClearTipsInfos()
        {
            ClearInfos();
        }

        /// <summary>
        /// 控制台输出
        /// </summary>
        internal void DebugWrite()
        {
            OutputBatchList.ForEach(e =>
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            });
        }

        /// <summary>
        /// 验证移除转入批次
        /// </summary>
        /// <param name="intput">转入批次</param>
        /// <returns>需要关联载具移除返回true，否则返回false</returns>
        internal bool ValidateIsMapContanier(InputBatch intput)
        {
            return Step.InputCollectStep.BarcodeType == BarcodeType.ContainerNo && intput.ContainerNo.IsNullOrEmpty();
        }

        /// <summary>
        /// 转入批次移除后执行操作
        /// </summary>
        /// <param name="inputBatch">已移除转入批次</param>
        protected virtual void InputBatchRemoved(InputBatch inputBatch)
        {

        }

        /// <summary>
        /// 显示计时提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="isError">是否错误信息，默认为true</param>
        /// <param name="timeout">计时时间，默认3秒</param>
        void ShowInstantMessage(string message, bool isError = true, int timeout = 3)
        {
            if (isError)
                ShowError(message);
            else
                ShowTips(message);
            timer.Tag = isError;
            timer.Interval = new TimeSpan(0, 0, timeout);
            timer.Tick += (x, y) =>
            {
                if ((bool)timer.Tag)
                    Error = string.Empty;
                else
                    Tips = string.Empty;
                timer.Stop();
            };
            timer.Start();
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        internal virtual void SwitchWorkOrder(Workcell workcell, double workOrderId)
        {

        }
    }
}