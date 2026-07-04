using Newtonsoft.Json;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.Runtime;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 批次维修采集控制器
    /// </summary>
    public class BatchRepairController : RepairController
    {
        /// <summary>
        /// 维修数据收集
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">产品</param>
        /// <param name="barcodes">收集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="relationBatches">关联批次</param>     
        protected override void OnBatchWipProductProcessFinished(BatchWipRecord wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell, EntityList<RelationBatch> relationBatches)
        {
            base.OnBatchWipProductProcessFinished(wipProductProcess, product, barcodes, collectData, workcell, relationBatches);
            AddRepaire(wipProductProcess, collectData, workcell);
        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="uplineProcess">上线工序</param>
        public virtual void BatchCollect(CollectData collectData, Workcell workcell, double? uplineProcess = null)
        {
            if (collectData == null)
                throw new ArgumentNullException(nameof(collectData));

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                BatchMoveOut(collectData, workcell);

                //若无下一工序则释放运行时数据
                if (!uplineProcess.HasValue || uplineProcess <= 0)
                {
                    var relation = BatchController.GetBatchRelation(collectData.CollectBarcode);
                    var product = RuntimeController.FindProduct(collectData.CollectBarcode);
                    if (relation.RemainQty <= 0 && product != null)
                    {
                        CompleteBatchProduct(product, relation, workcell);
                    }
                }
                else
                {
                    BatchGoto(collectData.CollectBarcode, uplineProcess.Value, workcell);
                }

                tran.Complete();
            }
        }
        
        /// <summary>
        /// 添加产品维修记录
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected virtual void AddRepaire(BatchWipRecord wipProductProcess, CollectData collectData, Workcell workcell)
        {
            var version = wipProductProcess.BatchVersion;
            // 更新报废数、质量状态
            version.ScrapQty += collectData.ScrapQty;
            version.DefectState = Products.SplitAndMerge.Enums.QState.Pass;
            var outputBatch = collectData.OutputBatch;
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now);
            if (shift == null)
            {
                throw new ValidationException("当前生产资源班次为空！".L10N());
            }
            EntityList<BatchWipProductRepaire> repaireList = new EntityList<BatchWipProductRepaire>();
            foreach (var repair in collectData.RepairDefects)
            {
                var defect = version.DefectList.FirstOrDefault(p => p.Id == repair.ProductDefectId);
                if (defect != null)
                {
                    repair.Responsiblities.ForEach(e =>
                    {
                        defect.ResponsibilityList.Add(new BatchWipDefectResponsibility() { Responsibility = e, Defect = defect });
                    });
                    repair.Measures.ForEach(e =>
                    {
                        defect.MeasureList.Add(new BatchWipDefectMeasure() { RepairMeasure = e, Defect = defect });
                    });
                    defect.Remark = repair.Remark;
                    defect.FixedById = workcell.EmployeeId;
                    defect.FixedDate = DateTime.Now;
                    RF.Save(defect);
                }

                var repaired = new BatchWipProductRepaire
                {
                    ProcessId = workcell.ProcessId,
                    ReparieById = workcell.EmployeeId,
                    ResourceId = workcell.ResourceId,
                    RepaireTime = DateTime.Now,
                    ShiftId = shift.Id,
                    StationId = workcell.StationId,
                    DefectId = (double)repair.ProductDefectId,
                    VersionId = wipProductProcess.BatchVersionId,
                    BatchNo = outputBatch.BatchNo,
                    SubBatchNo = outputBatch.SubBatchNo,
                    ContainerNo = outputBatch.ContainerNo,
                    RepairQty = outputBatch.Qty,
                    Qty = outputBatch.Qty + repair.ScrapQty,
                    ScrapQty = repair.ScrapQty,
                    ScrapReason = repair.ScrapReason,
                };
                repaireList.Add(repaired);
            }
            RF.Save(version);
            RF.Save(repaireList);
        }

        /// <summary>
        /// 验证工艺路线并加载产品未维修的缺陷
        /// </summary>
        /// <param name="collectBarcode">条码</param>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>产品缺陷记录列表</returns>
        public virtual EntityList<BatchWipProductDefect> LoadBatchDefects(CollectBarcode collectBarcode, Workcell workcell)
        {
            if (collectBarcode == null)
                throw new ArgumentNullException(nameof(collectBarcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));

            var batchRelation = BatchController.GetBatchRelation(collectBarcode);
            var version = FindLastBatchWipProductVersion(batchRelation.Bid);
            if (collectBarcode.Type == BarcodeType.BatchBarocde)
                return version.DefectList.Where(p => p.BatchNo == collectBarcode.Code).AsEntityList();
            else
                return version.DefectList.Where(p => p.ContainerNo == collectBarcode.Code).AsEntityList();
        }

        /// <summary>
        /// 获取工艺路线中的工序列表
        /// </summary>
        /// <param name="collectBarcode">条码</param>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>工艺路线工序列表</returns>
        public virtual List<GotoProcessViewModel> GetRoutingProcessList(CollectBarcode collectBarcode, Workcell workcell)
        {
            if (collectBarcode == null)
                throw new ArgumentNullException(nameof(collectBarcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));

            List<GotoProcessViewModel> lists = new List<GotoProcessViewModel>();

            var product = ValidateBatchProduct(collectBarcode, workcell.ProcessId);

            //子工艺路线中的工序不要列出选择,非工序组及工序组下的工序
            var processes = product.Routing.Processes
                .Where(x => x.ParentNodeId.IsNullOrEmpty() && x.Type != ProcessType.Fix && x.Type != ProcessType.BatchFix && string.IsNullOrEmpty(x.GroupId))
                .ToList();
            var processOfCurrent = product.Routing.GetNext()
                .FirstOrDefault(x => x.ProcessId == workcell.ProcessId);
            if (processOfCurrent == null)
            {
                return lists;
            }
            var groupProcesses = product.Routing.Processes.Where(x => x.IsGroup == true).ToList();
            foreach (var groupProcess in groupProcesses)
            {
                //工序组下有工序没有通过，则只能去未通过的工序
                if (product.Routing.Processes
                    .Any(x => !x.IsPass && x.IsGroup != true && x.GroupId == groupProcess.GroupId))
                {
                    processes.AddRange(product.Routing.Processes
                        .Where(x => !x.IsPass && x.IsGroup != true && x.GroupId == groupProcess.GroupId));
                }
                else
                {
                    processes.AddRange(product.Routing.Processes
                        .Where(x => x.IsGroup != true && x.GroupId == groupProcess.GroupId));
                }
            }

            StringBuilder stringBuilderPathDesc = new StringBuilder();

            foreach (var process in processes)
            {
                stringBuilderPathDesc.Clear();

                GotoProcessViewModel optionalPathViewModel = new GotoProcessViewModel()
                {
                    Id = Guid.NewGuid().ToString("N").ToUpper(),
                    RoutingProcessId = process.Id,
                    PathName = process.Name
                };

                stringBuilderPathDesc.Append(process.Name);

                BuilderPathDescription(stringBuilderPathDesc, product.Routing.Processes, process,
                    processOfCurrent.Id);

                optionalPathViewModel.PathDescription = stringBuilderPathDesc.ToString();

                lists.Add(optionalPathViewModel);
            }

            List<double> nextProcessIds;
            var currentProcess = product.Routing.Processes.FirstOrDefault(x => workcell.ProcessId == x.ProcessId);
            if (currentProcess != null && currentProcess.Next.TryGetValue(ResultType.Pass, out nextProcessIds))
            {
                var next = product.Routing.Processes.FirstOrDefault(q => q.Id == nextProcessIds[0]);

                if (next != null)
                {
                    var defultViewModel = lists.FirstOrDefault(x => x.RoutingProcessId == next.Id);
                    if (defultViewModel != null)
                    {
                        defultViewModel.IsDefault = true;
                    }
                }
            }

            return lists;
        }

        private void BuilderPathDescription(StringBuilder stringBuilderPathDesc, List<process> processes,
            process currentProcess, double sourceProcessId)
        {
            if (currentProcess.Next.ContainsKey(ResultType.Pass))
            {
                var nextIds = currentProcess.Next[ResultType.Pass];
                if (nextIds.Any())
                {
                    var nextId = nextIds[0];
                    if (nextId != sourceProcessId)
                    {
                        var nextProcess = processes.FirstOrDefault(x => x.Id == nextId);
                        if (nextProcess != null && string.IsNullOrEmpty(nextProcess.ParentNodeId))
                        {
                            stringBuilderPathDesc.Append(" --> {0}".L10nFormat(nextProcess.Name));
                            BuilderPathDescription(stringBuilderPathDesc, processes, nextProcess, sourceProcessId);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 保存维修记录
        /// </summary>
        /// <param name="defects">产品缺陷记录列表</param>
        public virtual void SaveRepairRecord(EntityList<BatchWipProductDefect> defects)
        {
            RF.Save(defects);
        }
    }
}