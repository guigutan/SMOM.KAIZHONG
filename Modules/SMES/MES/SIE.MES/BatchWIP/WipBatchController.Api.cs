using Castle.Core.Internal;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.BatchWIP.APIModels;
using SIE.MES.BatchWIP.Assemlys;
using SIE.MES.BatchWIP.Exceptions;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.Tech.Processs.Models;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 批次生产API控制器
    /// </summary>
    public class WipBatchApiController : SIE.Barcodes.WipBatchs.WipBatchController
    {

        /// <summary>
        /// 批次扫描记录-版本记录
        /// </summary>
        private EntityList<BatchWipProductVersion> lastBarcodeWipProductVersions = new EntityList<BatchWipProductVersion>();

        /// <summary>
        /// 批次扫描记录-条码记录
        /// </summary>
        private EntityList<WipBatch> lastBarcodeWipBatches = new EntityList<WipBatch>();

        #region 批次过站

        /// <summary>
        /// 扫描条码返回条码对象
        /// </summary>
        /// <param name="currentBarcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("扫描条码返回条码对象")]
        [return: ApiReturn("返回条码对象 WipBatchMoveBarcodeInfo")]
        public virtual WipBatchMoveBarcodeInfo GetBatchMoveBarcodeInfo([ApiParameter("当前扫描条码号")] string currentBarcode, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            if (string.IsNullOrEmpty(currentBarcode))
            {
                throw new ValidationException("请扫描批次条码".L10N());
            }
            if (workcell == null)
            {
                throw new ValidationException("工作单元不能为空".L10N());
            }

            var wipBatch = this.GetWipBatch(currentBarcode);
            if (wipBatch == null)
            {
                throw new ValidationException("批次号[{0}]不存在".L10nFormat(currentBarcode));
            }

            //验证工作单元
            RT.Service.Resolve<WipController>().ValidateWorkcell(workcell);

            var workcellProcess = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWith(Process.CollectStepListProperty));
            if (workcellProcess == null)
            {
                throw new ValidationException("入站失败，当前所选工序无法找到数据,请检查".L10N());
            }

            //获取当前工序的采集步骤
            EntityList<ProcessCollectStep> collectStepList = workcellProcess.CollectStepList;
            var collectStep = collectStepList?.FirstOrDefault(p => p.PlugType == PlugType.In);
            if (collectStep == null)
            {
                throw new ValidationException("工序[{0}]未定义入站采集步骤".L10nFormat(workcellProcess.Name));
            }

            if (wipBatch.Qty <= 0)
            {
                throw new ValidationException("批次数量已为0，请检查是否被拆分或合并");
            }

            var inputBatch = RT.Service.Resolve<BatchManageController>().GetInputBatch(currentBarcode, wipBatch.IsChild);
            if (inputBatch != null)
            {
                if (inputBatch.ProcessId != workcell.ProcessId)
                {
                    throw new ValidationException("批次条码[{0}]已入站，入站工序[{1}],不为当前工序，入站失败".L10nFormat(currentBarcode, inputBatch?.Process?.Name));
                }
                if (inputBatch.ResourceId != workcell.ResourceId)
                {
                    throw new ValidationException("批次条码[{0}]已入站，入站资源[{1}],不为当前资源，入站失败".L10nFormat(currentBarcode, inputBatch?.Resource?.Name));
                }
                if (inputBatch.StationId != workcell.StationId)
                {
                    throw new ValidationException("批次条码[{0}]已入站，入站工位[{1}],不为当前工位，入站失败".L10nFormat(currentBarcode, inputBatch?.Station?.Name));
                }
                //已经入站了则不再入站 而是带出
                return new WipBatchMoveBarcodeInfo()
                {
                    BatchBarcode = currentBarcode,
                    Id = wipBatch.Id,
                    ProductCode = wipBatch.ProductCode,
                    ProductName = wipBatch.ProductName,
                    Qty = inputBatch.RemainQty,
                    WorkOrderNo = wipBatch.WorkOrderNo,
                    CompleteNum = 0,
                    ScrapNum = 0,

                };
            }

            var collectBarcode = new CollectBarcode { Code = currentBarcode, Type = collectStep.BarcodeType };
            BatchProductInfo batchProductInfo = null;
            try
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    batchProductInfo = RT.Service.Resolve<BatchAssemblyController>().MoveIn(collectBarcode, workcell);
                    tran.Complete();
                }
            }
            catch (ReMoveInException ex)
            {
                throw new ValidationException(ex.Message);
            }
            return new WipBatchMoveBarcodeInfo()
            {
                BatchBarcode = currentBarcode,
                Id = wipBatch.Id,
                ProductCode = wipBatch.ProductCode,
                ProductName = wipBatch.ProductName,
                Qty = batchProductInfo == null ? wipBatch.Qty : batchProductInfo.InputBatch.RemainQty,
                WorkOrderNo = wipBatch.WorkOrderNo,
                CompleteNum = 0,
                ScrapNum = 0,

            };
        }

        /// <summary>
        /// 扫描条码返回工位库存
        /// </summary>
        /// <param name="currentBarcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        [ApiService("扫描已入站条码")]
        [return: ApiReturn("返回已入站条码列表 WipBatchMoveBarcodeInfo")]
        public virtual List<WipBatchMoveBarcodeInfo> GetBatchMoveStockInfo([ApiParameter("当前扫描条码号")] string currentBarcode, [ApiParameter("工作单元")] Workcell workcell)
        {

            if (workcell == null)
            {
                throw new ValidationException("工作单元不能为空");
            }
            if (workcell.StationId <= 0)
            {
                throw new ValidationException("工位不能为空");
            }
            if (workcell.ResourceId <= 0)
            {
                throw new ValidationException("资源不能为空");
            }
            if (workcell.ProcessId <= 0)
            {
                throw new ValidationException("工序不能为空");
            }
            List<WipBatchMoveBarcodeInfo> wipBatchMoveBarcodeInfos = new List<WipBatchMoveBarcodeInfo>();
            var queryList = Query<InputBatch>().Join<WipBatch>((x, y) => x.BatchNo == y.BatchNo && y.BatchState == BatchState.In)
                .Exists<BatchWipProductVersion>((a, b) => b.Where(f => f.BatchNo == a.BatchNo
                && f.RemainQty > 0
                && f.ProcessId == workcell.ProcessId && f.ResourceId == workcell.ResourceId && f.StationId == workcell.StationId))
                .Where(p => p.BatchState == BatchState.In && p.ProcessId == workcell.ProcessId && p.ResourceId == workcell.ResourceId && p.StationId == workcell.StationId)
                .WhereIf(!currentBarcode.IsNullOrEmpty(), p => p.BatchNo == currentBarcode)

                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var inputBatch in queryList)
            {
                var moveBarcodeInfo = new WipBatchMoveBarcodeInfo()
                {
                    BatchBarcode = inputBatch.BatchNo,
                    Id = inputBatch.Id,
                    ProductCode = inputBatch.ProductCode,
                    ProductName = inputBatch.ProductName,
                    Qty = inputBatch.RemainQty,
                    WorkOrderNo = inputBatch.WorkOrderNo,
                    CompleteNum = 0,
                    ScrapNum = 0,

                };
                wipBatchMoveBarcodeInfos.Add(moveBarcodeInfo);
            }
            return wipBatchMoveBarcodeInfos;
        }

        /// <summary>
        /// 提交条码出站
        /// </summary>
        /// <param name="batchMoveBarcodeInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="numberRuleId"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("提交条码出站")]
        [return: ApiReturn("返回最新的批次数据")]
        public virtual WipBatchMoveBarcodeInfo SubmitBatchMoveBarcode([ApiParameter("当前扫描条码号")] WipBatchMoveBarcodeInfo batchMoveBarcodeInfo,
        [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("编码规则Id")] double? numberRuleId)
        {
            if (batchMoveBarcodeInfo == null)
            {
                throw new ValidationException("批次信息为空 请检查".L10N());
            }
            //校验批次条码和批次关系以及转入批次
            var outputBatch = ValidateBatchBarcode(batchMoveBarcodeInfo, workcell, out InputBatch inputBatch, out Process workcellProcess);
            CollectData collectData = new CollectData();
            collectData.ApiData = true;
            //判断完成数量是否大于批次数量 小于则要求拆批 等于则原批次过站
            if (outputBatch == null)
            {
                throw new ValidationException("生成转出批次失败 请检查".L10N());
            }
            var finishQty = batchMoveBarcodeInfo.CompleteNum.HasValue ? batchMoveBarcodeInfo.CompleteNum.Value : 0;
            var scrapQty = batchMoveBarcodeInfo.ScrapNum.HasValue ? batchMoveBarcodeInfo.ScrapNum.Value : 0;
            //完工数量+报废数量＜批次数量
            if (outputBatch.Qty < (finishQty + scrapQty))
            {
                throw new ValidationException("完成数量+报废数量超出了当前批次数量:{0} ,请检查".L10nFormat(batchMoveBarcodeInfo.BatchBarcode));
            }
            if (outputBatch.Qty == (finishQty + scrapQty))//原批次过站
            {
                outputBatch.IsGenerateBatch = false;
                outputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = inputBatch, Qty = finishQty + scrapQty });
            }
            var nextMoveInBarcode = "";//下一工序入站批次条码
            EntityList<ProcessCollectStep> collectStepList = workcellProcess.CollectStepList;
            var collectStep = collectStepList?.FirstOrDefault(p => p.PlugType == PlugType.Out);
            //是否拆分子批
            BatchSplitMergeRecord batchSplitMergeRecord = null;

            try
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    batchSplitMergeRecord = CreateSplitMergeRecordAndMoveOut(numberRuleId, outputBatch, inputBatch, collectData, finishQty, scrapQty, collectStep, batchSplitMergeRecord, out nextMoveInBarcode);
                    var product = RT.Service.Resolve<BatchAssemblyController>().MoveOut(collectData, workcell, workcellProcess,true);
                    //生成批次拆分关系
                    if (batchSplitMergeRecord != null)
                    {
                        var version = Query<BatchWipProductVersion>().OrderByDescending(p => p.CreateDate).Where(p => p.BatchNo == batchSplitMergeRecord.InputBatchNo).FirstOrDefault();
                        if (version != null)
                        {
                            batchSplitMergeRecord.VersionId = version.Id;
                        }
                        RF.Save(batchSplitMergeRecord);
                    }
                    else//不拆分
                    {
                        var oldWipBatch = this.GetWipBatch(batchMoveBarcodeInfo.BatchBarcode);
                        if (oldWipBatch != null)
                        {
                            if (!oldWipBatch.ScrapQty.HasValue)
                            {
                                oldWipBatch.ScrapQty = 0;
                            }
                            oldWipBatch.ScrapQty += scrapQty;
                            RF.Save(oldWipBatch);
                        }
                    }

                    //判断是否执行下一个工序入站
                    var isNextMoveIn = product.Routing.Current == null ? workcellProcess.IsNextMoveIn : product.Routing.Current.IsNextMoveIn;
                    //下一工序要入站的前提是当前工序不是最后工序
                    if (isNextMoveIn == true && (product.Routing.Current != null && product.Routing.Current.Sign!= Tech.Routings.RoutingProcessSign.End))
                    {
                        //验证下一工序入站
                        ValidateNextProcessMoveIn(workcell, product, out Process process, out EntityList<Station> stations);
                        //执行下一工序入站
                        ExecutedNextProcessMoveIn(workcell, nextMoveInBarcode, process, stations);
                    }
                    tran.Complete();
                }
            }
            catch (ReMoveInException ex)
            {
                throw new ValidationException(ex.Message);
            }
            //重新获取一次转入批次 
            var wipBatch = this.GetWipBatch(batchMoveBarcodeInfo.BatchBarcode);
            var newInputBatch = RT.Service.Resolve<BatchManageController>().GetInputBatch(batchMoveBarcodeInfo.BatchBarcode, wipBatch.IsChild);
            if (newInputBatch == null)
            {
                batchMoveBarcodeInfo = new WipBatchMoveBarcodeInfo();
            }
            else
            {
                batchMoveBarcodeInfo.Qty = newInputBatch.RemainQty;
                batchMoveBarcodeInfo.CompleteNum = 0;
                batchMoveBarcodeInfo.ScrapNum = 0;
            }
            return batchMoveBarcodeInfo;
        }

        /// <summary>
        /// 创建批次合并记录和执行出站
        /// </summary>
        /// <param name="numberRuleId"></param>
        /// <param name="outputBatch"></param>
        /// <param name="inputBatch"></param>
        /// <param name="collectData"></param>
        /// <param name="finishQty"></param>
        /// <param name="scrapQty"></param>
        /// <param name="collectStep"></param>
        /// <param name="batchSplitMergeRecord"></param>
        /// <param name="barcode">实际过站的条码</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private BatchSplitMergeRecord CreateSplitMergeRecordAndMoveOut(double? numberRuleId, OutputBatch outputBatch, InputBatch inputBatch, CollectData collectData,
            decimal finishQty, decimal scrapQty, ProcessCollectStep collectStep,
            BatchSplitMergeRecord batchSplitMergeRecord, out string barcode)
        {
            barcode = "";
            if (inputBatch.RemainQty > finishQty + scrapQty)
            {
                if (!numberRuleId.HasValue || numberRuleId <= 0)
                {
                    throw new ValidationException("请选择编码规则".L10N());
                }
                //生成子批次编码
                var barcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId.Value, 1, inputBatch.WorkOrder);
                var setting = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(inputBatch.WorkOrderId);
                //生成子批次数量
                var childWipBatchBarcode = new SubWipBatch()
                {
                    WorkOrderId = inputBatch.WorkOrderId,
                    BatchNo = barcodes.FirstOrDefault(),
                    Qty = finishQty,
                    BoxesQty = (setting == null || setting.Qty == null) ? 0 : setting.Qty.Value,
                    PrintDate = DateTime.Now,
                    BatchState = BatchState.Generated,
                    //IsChild = true,
                    ScrapQty = scrapQty,
                };
                childWipBatchBarcode.GenerateId();
                var subOutputBatch = new OutputBatch()
                {
                    BarcodeType = collectStep.BarcodeType,
                    IsGenerateBatch = true,
                    WorkOrder = inputBatch.WorkOrder,
                    SubBatchNo = childWipBatchBarcode.BatchNo,
                    SubWipBatch = childWipBatchBarcode,
                    Qty = finishQty
                };
                subOutputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = inputBatch, Qty = subOutputBatch.Qty + scrapQty });
                inputBatch.SplitQty = finishQty + scrapQty;
                subOutputBatch.BatchNo = subOutputBatch.RelationBatchList[0].InputBatch.BatchNo;
                subOutputBatch.Qty = subOutputBatch.RelationBatchList.Sum(p => p.Qty);
                //子批过站
                collectData.OutputBatch = subOutputBatch;
                collectData.ScrapQty = scrapQty;
                collectData.CollectBarcode = new CollectBarcode()
                {
                    BarcodeType = collectStep.BarcodeType,
                    Type = collectStep.BarcodeType,
                    Code = subOutputBatch.SubBatchNo
                };
                barcode = subOutputBatch.SubBatchNo;
                //生成批次拆分记录
                batchSplitMergeRecord = new BatchSplitMergeRecord()
                {
                    InputBatchNo = inputBatch.BatchNo,
                    InputQty = inputBatch.Qty,
                    OutputBatchNo = subOutputBatch.SubBatchNo,
                    OutputQty = subOutputBatch.Qty,
                    BatchOperationType = BatchOperationType.Split,
                };

            }
            //b)	如果完工数量+报废数量=批次数量，则不进行批次拆分，以原批次进行出站
            if (outputBatch.Qty == finishQty + scrapQty)//原批次整体出站
            {
                collectData.OutputBatch = outputBatch;
                collectData.ScrapQty = scrapQty;
                collectData.CollectBarcode = new CollectBarcode()
                {
                    BarcodeType = outputBatch.BarcodeType,
                    Type = outputBatch.BarcodeType,
                    Code = inputBatch.BatchNo,
                };
                barcode = inputBatch.BatchNo;
            }

            return batchSplitMergeRecord;
        }

        /// <summary>
        /// 执行下一工序入站
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="nextMoveInBarcode"></param>
        /// <param name="process"></param>
        /// <param name="stations"></param>
        /// <exception cref="ValidationException"></exception>

        public virtual void ExecutedNextProcessMoveIn(Workcell workcell, string nextMoveInBarcode, Process process, EntityList<Station> stations)
        {
            var nextCollectStepList = process.CollectStepList;
            var nextcollectStep = nextCollectStepList?.FirstOrDefault(p => p.PlugType == PlugType.In);
            if (nextcollectStep == null)
            {
                throw new ValidationException("下一工序[{0}]未定义入站采集步骤,无法进行下工序入站".L10nFormat(process.Name));
            }
            var nextWipBatch = this.GetWipBatch(nextMoveInBarcode);
            if (nextWipBatch == null)
            {
                throw new ValidationException("批次号[{0}]不存在".L10nFormat(nextMoveInBarcode));
            }
            var nextInputBatch = RT.Service.Resolve<BatchManageController>().GetInputBatch(nextMoveInBarcode, nextWipBatch.IsChild);
            if (nextInputBatch != null)
            {
                throw new ValidationException("批次条码[{0}]已入站，入站工位[{1}]，执行下一工序入站失败".L10nFormat(nextMoveInBarcode, nextInputBatch?.Station?.Name));
            }
            workcell.ProcessId = process.Id;
            workcell.StationId = stations[0].Id;
            var nextCollectBarcode = new CollectBarcode { Code = nextMoveInBarcode, Type = nextcollectStep.BarcodeType };
            RT.Service.Resolve<BatchAssemblyController>().MoveIn(nextCollectBarcode, workcell);
        }

        /// <summary>
        /// 校验下一工序入站的条件
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="product"></param>
        /// <param name="process"></param>
        /// <param name="stations"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidateNextProcessMoveIn(Workcell workcell, product product, out Process process, out EntityList<Station> stations)
        {
            var nextProcessList = product.Routing.GetNext();
            if (nextProcessList == null || !nextProcessList.Any())
            {
                throw new ValidationException("无法执行下一工序入站，下一工序为空，请检查".L10N());
            }
            if (nextProcessList.Count() > 1)
            {
                throw new ValidationException("存在多个下一工序，无法执行下工序入站，请检查".L10N());
            }
            var nextProcess = nextProcessList.First();
            if (!nextProcess.ProcessId.HasValue)
            {
                throw new ValidationException("不存在下一工序的工序，请检查".L10N());
            }

            process = Query<Process>().Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == RT.IdentityId && a.Id == nextProcess.ProcessId))
                .FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty));
            if (process == null)
            {
                throw new ValidationException("当前用户没有下一工序的权限,无法进行下工序入站，请检查".L10N());
            }
            stations = RT.Service.Resolve<StationController>().GetStationsByResourceId(workcell.ResourceId, nextProcess.ProcessId.Value);
            if (stations.Count == 0)
            {
                throw new ValidationException("不存在与下工序【{0}】关联的工位，无法进行下工序入站".L10nFormat(nextProcess.Name));
            }
            if (stations.Count > 1)
            {
                throw new ValidationException("存在多个与下工序关联的工位，无法进行下工序入站".L10N());
            }
        }

        /// <summary>
        /// 校验批次条码
        /// </summary>
        /// <param name="batchMoveBarcodeInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="inputBatch"></param>
        /// <param name="workcellProcess"></param>
        /// <exception cref="ValidationException"></exception>
        private OutputBatch ValidateBatchBarcode(WipBatchMoveBarcodeInfo batchMoveBarcodeInfo, Workcell workcell, out InputBatch inputBatch, out Process workcellProcess)
        {

            if (string.IsNullOrEmpty(batchMoveBarcodeInfo.BatchBarcode))
            {
                throw new ValidationException("请扫描批次条码".L10N());
            }
            if (workcell == null)
            {
                throw new ValidationException("工作单元不能为空".L10N());
            }
            //验证工作单元
            RT.Service.Resolve<WipController>().ValidateWorkcell(workcell);
            var wipBatch = this.GetWipBatch(batchMoveBarcodeInfo.BatchBarcode);
            if (wipBatch == null)
            {
                throw new ValidationException("批次号[{0}]不存在".L10nFormat(batchMoveBarcodeInfo.BatchBarcode));
            }
            workcellProcess = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWith(Process.CollectStepListProperty));
            if (workcellProcess == null)
            {
                throw new ValidationException("过站失败，当前所选工序无法找到数据,请检查".L10N());
            }
            EntityList<ProcessCollectStep> collectStepList = workcellProcess.CollectStepList;
            var collectStep = collectStepList?.FirstOrDefault(p => p.PlugType == PlugType.Out);
            if (collectStep == null)
            {
                throw new ValidationException("工序[{0}]未定义出站采集步骤".L10nFormat(workcellProcess.Name));
            }

            inputBatch = RT.Service.Resolve<BatchManageController>().GetInputBatch(batchMoveBarcodeInfo.BatchBarcode, wipBatch.IsChild);
            if (inputBatch == null)
            {
                throw new ValidationException("批次条码[{0}]未入站,请先扫描入站".L10nFormat(batchMoveBarcodeInfo.BatchBarcode));
            }
            else
            {
                if (inputBatch.ProcessId != workcell.ProcessId)
                {
                    throw new ValidationException("批次条码[{0}]已入站，入站工序[{1}],不为当前工序，提交失败".L10nFormat(batchMoveBarcodeInfo.BatchBarcode, inputBatch?.Process?.Name));
                }
                if (inputBatch.StationId != workcell.StationId)
                {
                    throw new ValidationException("批次条码[{0}]已入站，入站工位[{1}],不为当前工位，提交失败".L10nFormat(batchMoveBarcodeInfo.BatchBarcode, inputBatch?.Station?.Name));
                }
            }
            //此步骤避免在CS和PDA同时出现入站
            if (inputBatch.RemainQty != batchMoveBarcodeInfo.Qty)
            {
                throw new ValidationException("批次号[{0}]的批次信息在系统中发生了变更，请检查".L10nFormat(batchMoveBarcodeInfo.BatchBarcode));
            }
            OutputBatch outputBatch = new OutputBatch()
            {
                BarcodeType = collectStep.BarcodeType,
                BatchNo = inputBatch.BatchNo,
                ContainerNo = inputBatch.ContainerNo,
                InputDate = inputBatch.InputDate,
                IsNg = false,
                Qty = inputBatch.RemainQty,
                WorkOrderId = inputBatch.WorkOrderId,
                OutputDate = DateTime.Now,
                IsGenerateBatch = true,
                WorkOrder = inputBatch.WorkOrder
            };
            return outputBatch;
        }
        #endregion

        #region 批次合并PDA

        /// <summary>
        /// 获取类型为产品条码的编码规则
        /// </summary>
        /// <returns></returns>
        [ApiService("获取类型为产品条码的编码规则")]
        [return: ApiReturn("产品条码的编码规则集合")]
        public virtual List<BaseDataInfo> GetBarcodeNumberRule()
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            var barcodeNumberRules = Query<NumberRule>().Where(m => m.Type == RuleType.Barcode).ToList();
            foreach (var item in barcodeNumberRules)
            {
                baseDataInfos.Add(new BaseDataInfo()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                });
            }
            return baseDataInfos;
        }

        /// <summary>
        /// 根据编码规则ID生产新的条码号并返回
        /// </summary>
        /// <param name="numberRuleId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("根据编码规则ID生产新的批次条码号")]
        [return: ApiReturn("返回新的批次条码号")]
        public virtual string GetNewBarcode([ApiParameter("编码规则Id")] double numberRuleId)
        {
            if (numberRuleId <= 0)
            {
                throw new ValidationException("请选择编码规则".L10N());
            }
            var numberRule = GetById<NumberRule>(numberRuleId);
            if (numberRule == null)
            {
                throw new ValidationException("编码规则不能为空".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 提交批次合并
        /// </summary>
        /// <param name="mergeBarcode"></param>
        /// <param name="workcell"></param>
        /// <param name="lastBarcodes"></param>
        [ApiService("提交批次合并")]
        [return: ApiReturn("")]
        public virtual void SubmitBatchMerge([ApiParameter("合并条码号")] string mergeBarcode, [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("历史扫描条码信息")] List<WipBatchBarcodeInfo> lastBarcodes)
        {
            if (string.IsNullOrEmpty(mergeBarcode))
            {
                throw new ValidationException("合并批次号不能空，请先生成合并批次号".L10N());
            }
            if (workcell == null)
            {
                throw new ValidationException("工作单元不能为空，请检查".L10N());
            }
            if (workcell.ResourceId <= 0)
            {
                throw new ValidationException("请选择资源".L10N());
            }
            if (workcell.StationId <= 0)
            {
                throw new ValidationException("请选择工位".L10N());
            }
            if (lastBarcodes == null)
            {
                throw new ValidationException("请扫描批次号".L10N());
            }
            if (lastBarcodes.Count <= 1)
            {
                throw new ValidationException("请至少扫描两个批次号".L10N());
            }

            var barcodes = lastBarcodes.Select(m => m.BatchBarcode).ToList();
            var wipBatch = Validate(barcodes, out bool result, out BatchWipProductVersion firstBarcodeWipProductVersion);

            if (result)
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    var runtimeController = RT.Service.Resolve<RuntimeController>();
                    ////深复制一个批次
                    var mergeBatch = JsonConvert.DeserializeObject<WipBatch>(JsonConvert.SerializeObject(wipBatch));

                    mergeBatch.GenerateId();//重新生成Id
                    mergeBatch.BatchList.Clear();//清除子批关系
                    mergeBatch.BatchNo = mergeBarcode;
                    mergeBatch.RemainQty = mergeBatch.Qty = lastBarcodes.Sum(m => m.Qty);
                    mergeBatch.PersistenceStatus = PersistenceStatus.New;
                    mergeBatch.RangeId = null;
                    RF.Save(mergeBatch);
                    //创建批次关系
                    RT.Service.Resolve<BatchManageController>().CreateSubWipBatchRelation(mergeBatch.BatchNo, mergeBatch.BatchNo, mergeBatch.Qty, mergeBatch.WorkOrderId, null);
                    //复制合并前的运行时
                    var oldRuntimeProduct = RT.Service.Resolve<RuntimeController>().FindProduct(barcodes.FirstOrDefault(), Core.Barcodes.BarcodeType.BatchBarocde);
                    //克隆运行时
                    var mergeRuntimeProduct = oldRuntimeProduct.Clone();
                    //创建一个PUID用于后面与合并批次号关联
                    var puid = Guid.NewGuid().ToString("N").ToUpper();
                    //复制新生成的合并批次号的Puid为新的Puid
                    mergeRuntimeProduct.Puid = puid;
                    //创建新条码的工艺路线布局
                    RT.Service.Resolve<WipController>().SaveBatchRuntimeRouting(mergeBatch.WorkOrderId, mergeBarcode, wipBatch.BatchNo);
                    //合并批次号与产品运行时Puid做关联
                    runtimeController.MapPuid(new CollectBarcode() { Code = mergeBarcode, Type = BarcodeType.BatchBarocde }, puid);
                    //保存产品运行时
                    runtimeController.Save(mergeRuntimeProduct);
                    //复制旧的批次版本数据到新的合并批次号
                    BatchWipProductVersion mergeBarcodeWipProductVersion = CopyWipProductVersion(mergeBarcode, workcell, firstBarcodeWipProductVersion, mergeBatch);
                    //MergeDefect(mergeBarcodeWipProductVersion);


                    var batchSplitMergeList = new EntityList<BatchSplitMergeRecord>();
                    //更新原批次号的当前数量，当前数量=当前数量-合并数量
                    foreach (var item in lastBarcodeWipBatches)
                    {
                        item.Qty = 0;
                        item.RemainQty = 0;
                    }
                    List<CollectBarcode> collectBarcodes = new List<CollectBarcode>();
                    //更新原批次生产版本的剩余数量和当前数量
                    foreach (var item in lastBarcodeWipProductVersions)
                    {
                        //记录拆分关系
                        var batchSplitMerge = new BatchSplitMergeRecord()
                        {
                            BatchOperationType = BatchOperationType.Merge,
                            InputBatchNo = item.BatchNo,
                            InputQty = item.Qty,
                            OutputBatchNo = mergeBarcode,
                            VersionId = mergeBarcodeWipProductVersion.Id,
                            OutputQty = mergeBatch.Qty,
                        };
                        batchSplitMergeList.Add(batchSplitMerge);
                        collectBarcodes.Add(new CollectBarcode()
                        {
                            Code = item.BatchNo,
                            BarcodeType = BarcodeType.BatchBarocde,
                            Type = BarcodeType.BatchBarocde
                        });
                        //item.Qty = 0;
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
                    RF.Save(batchRelations);
                    RF.Save(mergeBarcodeWipProductVersion);
                    //如果当前合并批次状态为入站，执行一次入站
                    if (wipBatch.BatchState == BatchState.In)
                    {
                        //执行入站
                        var collectBarcode = new CollectBarcode { Code = mergeBarcodeWipProductVersion.BatchNo, Type = BarcodeType.BatchBarocde };
                        workcell.ProcessId = firstBarcodeWipProductVersion.ProcessId.HasValue ? firstBarcodeWipProductVersion.ProcessId.Value : workcell.ProcessId;

                        RT.Service.Resolve<BatchAssemblyController>().MoveIn(collectBarcode, workcell);

                    }
                    if (batchSplitMergeList.Any())
                    {
                        RF.Save(batchSplitMergeList);
                    }
                    RF.Save(lastBarcodeWipBatches);
                    RF.Save(lastBarcodeWipProductVersions);
                    tran.Complete();
                }

            }

        }

        /// <summary>
        /// 合并缺陷（待定）
        /// </summary>
        /// <param name="mergeBarcodeWipProductVersion"></param>
        private void MergeDefect(BatchWipProductVersion mergeBarcodeWipProductVersion)
        {
            EntityList<BatchWipProductDefect> defectList = new EntityList<BatchWipProductDefect>();
            var defectIds = mergeBarcodeWipProductVersion.DefectList?.Select(x => x.Id).ToList();
            EntityList<BatchWipProductDefectDetail> detailList = Query<BatchWipProductDefectDetail>().Where(m => defectIds.Contains(m.DefectId)).ToList();
            foreach (var item in mergeBarcodeWipProductVersion.DefectList)//更新缺陷记录的指向
            {
                var itemOldId = item.Id;
                item.VersionId = mergeBarcodeWipProductVersion.Id;
                item.GenerateId();
                item.PersistenceStatus = PersistenceStatus.New;
                defectList.Add(item);
                var defectDetailList = detailList.Where(m => m.DefectId == itemOldId);
                foreach (var detail in defectDetailList)
                {
                    detail.GenerateId();
                    detail.PersistenceStatus = PersistenceStatus.New;
                    detail.BatchWipProductDefectId = item.Id;
                }
            }

            if (defectList.Any())
            {
                RF.Save(defectList);
            }
            if (detailList.Any())
            {
                RF.Save(detailList);
            }
        }

        /// <summary>
        /// 复制旧的批次版本数据到新的合并批次号
        /// </summary>
        /// <param name="mergeBarcode"></param>
        /// <param name="workcell"></param>
        /// <param name="firstBarcodeWipProductVersion"></param>
        /// <param name="mergeBatch"></param>
        /// <returns>返回复制后的合并批次号生产版本记录</returns>
        private BatchWipProductVersion CopyWipProductVersion(string mergeBarcode, Workcell workcell, BatchWipProductVersion firstBarcodeWipProductVersion, WipBatch mergeBatch)
        {
            var mergeBarcodeWipProductVersion = JsonConvert.DeserializeObject<BatchWipProductVersion>(JsonConvert.SerializeObject(firstBarcodeWipProductVersion));
            mergeBarcodeWipProductVersion.BatchNo = mergeBarcode;
            mergeBarcodeWipProductVersion.CurrentProcessId = null;
            mergeBarcodeWipProductVersion.ResourceId = workcell.ResourceId;
            mergeBarcodeWipProductVersion.StationId = workcell.StationId;
            mergeBarcodeWipProductVersion.Qty = mergeBatch.Qty;
            mergeBarcodeWipProductVersion.RemainQty = mergeBatch.Qty;
            mergeBarcodeWipProductVersion.GenerateId();
            mergeBarcodeWipProductVersion.PersistenceStatus = PersistenceStatus.New;

            //更新缺陷ID指向
            mergeBarcodeWipProductVersion.DefectList.Clear();
            mergeBarcodeWipProductVersion.RepaireList.Clear();
            mergeBarcodeWipProductVersion.ProcessList.Clear();
            return mergeBarcodeWipProductVersion;
        }

        /// <summary>
        /// 获取批次条码信息并执行校验
        /// </summary>
        /// <param name="currentBarcode">当前扫描条码号</param>
        /// <param name="lastBarcodesInfo">历史扫描条码号</param>
        [ApiService("获取批次条码信息并执行校验")]
        [return: ApiReturn("返回验证后的条码信息")]
        public virtual WipBatchBarcodeInfo GetBatchBarcodeInfo([ApiParameter("当前扫描条码号")] string currentBarcode, [ApiParameter("历史扫描条码号")] List<WipBatchBarcodeInfo> lastBarcodesInfo)
        {
            if (string.IsNullOrEmpty(currentBarcode))
            {
                throw new ValidationException("请扫描批次条码".L10N());
            }
            if (lastBarcodesInfo == null)
            {
                lastBarcodesInfo = new List<WipBatchBarcodeInfo>();
            }
            var lastBarcodes = lastBarcodesInfo.Select(m => m.BatchBarcode).ToList();
            lastBarcodes.Insert(0, currentBarcode);
            var firstBarcodeWipBatch = Validate(lastBarcodes, out bool validateResult, out BatchWipProductVersion firstBarcodeWipProductVersion);
            if (validateResult)//成功验证返回批次条码信息
            {
                return new WipBatchBarcodeInfo()
                {
                    Id = firstBarcodeWipBatch.Id,
                    BatchBarcode = firstBarcodeWipBatch.BatchNo,
                    Qty = firstBarcodeWipProductVersion.RemainQty,
                    CurrenProcess= firstBarcodeWipProductVersion.ProcessName
                };
            }
            return null;
        }

        /// <summary>
        /// 验证批次条码信息
        /// </summary>
        /// <param name="lastBarcodes"></param>
        /// <param name="result"></param>
        /// <param name="firstBarcodeWipProductVersion"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private WipBatch Validate(List<string> lastBarcodes, out bool result, out BatchWipProductVersion firstBarcodeWipProductVersion)
        {
            result = false;

            lastBarcodeWipBatches = this.GetWipBatches(lastBarcodes);
            lastBarcodeWipProductVersions = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(lastBarcodes);

            var firstBarcode = lastBarcodes.FirstOrDefault();
            var firstBarcodeWipBatch = lastBarcodeWipBatches.FirstOrDefault(m => m.BatchNo == firstBarcode);
            firstBarcodeWipProductVersion = lastBarcodeWipProductVersions.FirstOrDefault(m => m.BatchNo == firstBarcode);

            if (firstBarcodeWipBatch == null)
            {
                throw new ValidationException("批次条码【{0}】不存在，请检查！".L10nFormat(firstBarcode));
            }
            if (firstBarcodeWipProductVersion == null)
            {
                throw new ValidationException("生产批次版本中不存在【{0}】批次号的记录，请检查是否已上线！".L10nFormat(firstBarcode));
            }
            if (firstBarcodeWipProductVersion.RemainQty <= 0)
            {
                throw new ValidationException("批次【{0}】的当前数量＞0才能进行批次合并操作".L10nFormat(firstBarcode));
            }

            //使用第一个条码作为标准 后续条码均和第一个比较验证
            foreach (var barcode in lastBarcodes)
            {
                if (barcode == firstBarcode)
                {
                    continue;
                }

                var nextBarcodeWipBatch = lastBarcodeWipBatches.FirstOrDefault(m => m.BatchNo == barcode);
                var nextBarcodeWipProductVersion = lastBarcodeWipProductVersions.FirstOrDefault(m => m.BatchNo == barcode);
                //校验条码本身
                if (nextBarcodeWipBatch == null)
                {
                    throw new ValidationException("批次条码【{0}】不存在，请检查！".L10nFormat(barcode));
                }
                if (nextBarcodeWipProductVersion == null)
                {
                    throw new ValidationException("生产批次版本中不存在【{0}】批次号的记录，请检查是否已上线！".L10nFormat(barcode));
                }
                if (nextBarcodeWipProductVersion.RemainQty <= 0)
                {
                    throw new ValidationException("批次【{0}】的当前数量＞0才能进行批次合并操作".L10nFormat(barcode));
                }

                //与最新批次条码比较
                if (firstBarcodeWipBatch.WorkOrderId != nextBarcodeWipBatch.WorkOrderId)
                {
                    throw new ValidationException("批次号【{0}】与已扫描批次号【{1}】的工单不一致,无法进行批次合并，请检查！".L10nFormat(firstBarcode, barcode));
                }
                if (firstBarcodeWipProductVersion.ProcessId != nextBarcodeWipProductVersion.ProcessId)
                {
                    throw new ValidationException("批次号【{0}】与已扫描批次号【{1}】的当前工序不一致,无法进行批次合并，请检查！".L10nFormat(firstBarcode, barcode));
                }
                if (firstBarcodeWipProductVersion.NextProcessId != nextBarcodeWipProductVersion.NextProcessId)
                {
                    throw new ValidationException("批次号【{0}】与已扫描批次号【{1}】的下工序不一致,无法进行批次合并，请检查！".L10nFormat(firstBarcode, barcode));
                }
                //比较缺陷记录是否相等

                ValidateDefectDetails(firstBarcode, firstBarcodeWipProductVersion, barcode, nextBarcodeWipProductVersion);
            }
            result = true;
            return firstBarcodeWipBatch;
        }

        /// <summary>
        /// 比较缺陷记录是否相等
        /// </summary>
        /// <param name="firstBarcode"></param>
        /// <param name="firstBarcodeWipProductVersion"></param>
        /// <param name="barcode"></param>
        /// <param name="nextBarcodeWipProductVersion"></param>
        /// <exception cref="ValidationException"></exception>
        private static void ValidateDefectDetails(string firstBarcode, BatchWipProductVersion firstBarcodeWipProductVersion, string barcode, BatchWipProductVersion nextBarcodeWipProductVersion)
        {
            var nextBarcodeDefectList = new EntityList<BatchWipProductDefectDetail>();
            var firstBarcodeDefectList = new EntityList<BatchWipProductDefectDetail>();

            nextBarcodeWipProductVersion.DefectList.ForEach(
                m =>
                {
                    if (m.DetailList.Any())
                    {
                        nextBarcodeDefectList.AddRange(m.DetailList);
                    }
                });
            firstBarcodeWipProductVersion.DefectList.ForEach(
                m =>
                {
                    if (m.DetailList.Any())
                    {
                        firstBarcodeDefectList.AddRange(m.DetailList);
                    }
                });

            if (nextBarcodeDefectList.Count != firstBarcodeDefectList.Count)
            {
                throw new ValidationException("批次号【{0}】与已扫描批次号【{1}】的缺陷记录不一致,无法进行批次合并，请检查！".L10nFormat(firstBarcode, barcode));
            }
            foreach (var item in firstBarcodeDefectList)
            {
                if (nextBarcodeDefectList.FindIndex(m => m.DefectId == item.DefectId) < 0)
                {
                    throw new ValidationException("批次号【{0}】与已扫描批次号【{1}】的缺陷记录不一致,无法进行批次合并，请检查！".L10nFormat(firstBarcode, barcode));
                }
            }
        }
        #endregion
    }
}
