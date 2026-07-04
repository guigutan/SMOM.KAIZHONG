using SIE.Barcodes.WipBatchs;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchGeneration.Daos;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Assemlys;
using SIE.MES.WIP;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using static IronPython.Modules._ast;

namespace SIE.MES.BatchGeneration.Services
{
    /// <summary>
    /// 批次打印并过站SERVICE层
    /// </summary>
    public partial class WOBatchGenerationService : DomainService
    {
        /// <summary>
        /// 批次打印并过站访问
        /// </summary>
        private readonly WOBatchGenerationDao _WOBatchGenerationDao;

        /// <summary>
        /// Service构造函数
        /// </summary>
        /// <param name="woBatchGenerationDao"></param>
        public WOBatchGenerationService(WOBatchGenerationDao woBatchGenerationDao)
        {
            _WOBatchGenerationDao = woBatchGenerationDao;
        }

        /// <summary>
        /// 批次打印并过站查询服务
        /// </summary>
        /// <param name="workOrderArchiveCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<WOBatchGeneration> QueryWOBatchGenerationList(WOBatchGenerationCriteria workOrderArchiveCriteria)
        {
            return _WOBatchGenerationDao.QueryWorkOrderArchiveList(workOrderArchiveCriteria);
        }
        /// <summary>
        /// 获取批次信息
        /// </summary>
        /// <param name="workorderId"></param>
        /// <param name="isChild"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatchViewModel> GetWipBatchsViewModelByWorkOrder(double workorderId, bool? isChild = false, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            return _WOBatchGenerationDao.GetWipBatchsViewModelByWorkOrder(workorderId, isChild, sortInfo, pagingInfo);

        }

        /// <summary>
        /// 获取工单内的所有批次装配工序
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="processType"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcessesByWoId(double woId, ProcessType processType, bool isStart = true, PagingInfo pagingInfo = null, string keyword = "")
        {
            return _WOBatchGenerationDao.GetProcessesByWoId(woId, pagingInfo, keyword, processType, isStart);
        }

        /// <summary>
        /// 获取工单首工序
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="processType"></param>
        /// <returns></returns>
        public virtual Process GetFirstProcessesByWoId(double woId, ProcessType processType)
        {
            return _WOBatchGenerationDao.GetFirstProcessesByWoId(woId, processType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public virtual Tuple<BatchWorkOrder, EntityList<WipBatch>> BatchsGeneratingAndMove(BatchBarcodeInfo info, Workcell workcell)
        {
            var batchWo = RF.GetById<BatchWorkOrder>(info.WorkOrderId);
            var process = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWithViewProperty());
            if (batchWo == null)
            {
                throw new EntityNotFoundException(typeof(WorkOrder), info.WorkOrderId);
            }
            if (workcell == null)
            {
                throw new EntityNotFoundException("工作单元不能为空".L10N());
            }
            if ((batchWo.PlanQty - batchWo.GeneratedQty + batchWo.ScrapQty) < info.BatchQty)
            {
                throw new ValidationException("批次数量超过剩余数量,过站失败".L10N());
            }

            var reportModel = RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(info.WorkOrderId);
            if (reportModel == 1)
            {
                throw new ValidationException("不允许采集，当前条码所属工单任务单报工方式为手动报工".L10N());
            }
            RT.Service.Resolve<IWipTaskReport>().ValidateAutoReport(info.WorkOrderId, workcell.EmployeeId, workcell.ProcessId);
            EntityList<WipBatch> barcodes = new EntityList<WipBatch>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                info.GeneratingQty = info.BatchQty;
                //创建并保存批次条码
                var batchBarcodes = RT.Service.Resolve<WipBatchController>().SaveBatchBarcodes(info, batchWo, 1, process?.Code);
                if (batchBarcodes.Any())
                {
                    barcodes.AddRange(batchBarcodes);
                }
                else
                {
                    throw new ValidationException("生产批次失败,请检查".L10N());
                }
                //验证工作单元
                RT.Service.Resolve<WipController>().ValidateWorkcell(workcell);
                var workcellProcess = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWith(Process.CollectStepListProperty));
                if (workcellProcess == null)
                {
                    throw new ValidationException("过站失败，当前所选工序无法找到数据,请检查".L10N());
                }
                EntityList<ProcessCollectStep> collectStepList = workcellProcess.CollectStepList;
                var collectStep = collectStepList?.FirstOrDefault(p => p.PlugType == PlugType.In);
                if (collectStep == null)
                {
                    throw new ValidationException("工序[{0}]未定义入站采集步骤".L10nFormat(workcellProcess.Name));
                }

                var collectBarcode = new CollectBarcode { Code = batchBarcodes[0].BatchNo, Type = collectStep.BarcodeType };
                //获取当前工序的采集步骤
                BatchProductInfo result = RT.Service.Resolve<BatchAssemblyController>().MoveIn(collectBarcode, workcell);
                OutputBatch outputBatch = new OutputBatch()
                {

                    BarcodeType = Core.Barcodes.BarcodeType.BatchBarocde,
                    BatchNo = result.InputBatch.BatchNo,
                    ContainerNo = result.InputBatch.ContainerNo,
                    InputDate = result.InputBatch.InputDate,
                    IsNg = false,
                    Qty = result.InputBatch.Qty,
                    WorkOrderId = result.InputBatch.WorkOrderId,
                    OutputDate = DateTime.Now,
                    IsGenerateBatch = false,
                    WorkOrder = result.InputBatch.WorkOrder
                };
                var outCollectStep = collectStepList?.FirstOrDefault(p => p.PlugType == PlugType.Out);
                if (outCollectStep == null)
                {
                    throw new ValidationException("工序[{0}]未定义出站采集步骤".L10nFormat(workcellProcess.Name));
                }
                RT.Service.Resolve<WipController>().ValidateOutputBarcode(result.InputBatch.BatchNo, outCollectStep.BarcodeType);
                //转出
                var collectData = new CollectData()
                {
                    OutputBatch = outputBatch,
                    CollectBarcode = new CollectBarcode()
                    {
                        BarcodeType = outCollectStep.BarcodeType,
                        Type = outCollectStep.BarcodeType,
                        Code = result.InputBatch.BatchNo
                    }
                };
                outputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = result.InputBatch, Qty = result.InputBatch.Qty });
                //执行出站逻辑
                RT.Service.Resolve<BatchAssemblyController>().MoveOut(collectData, workcell, workcellProcess);
                tran.Complete();
            }

            return new Tuple<BatchWorkOrder, EntityList<WipBatch>>(batchWo, barcodes);
        }
    }
}
