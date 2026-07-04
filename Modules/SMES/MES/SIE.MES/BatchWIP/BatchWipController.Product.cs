using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 生产采集控制器
    /// </summary>
    public partial class WipController
    {
        /// <summary>
        /// 创建批次在制产品
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <returns>生产产品</returns>
        protected virtual BatchWipProduct CreateBatchWipProduct(product product)
        {
            var wipProduct = new BatchWipProduct()
            {
                Qty = product.Qty,
                State = WipProductState.Producing,
                ItemId = product.ItemId,
                Buid = product.Puid
            };
            wipProduct.GenerateId();
            return wipProduct;
        }

        /// <summary>
        /// 创建在制批次产品版本
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="product">运行时产品</param>
        /// <param name="wipBatch">生产批次</param>
        /// <returns>生产产品版本</returns>
        protected virtual BatchWipProductVersion CreateBatchWipProductVersion(BatchWipProduct wipProduct, product product, string wipBatch)
        {
            BatchWipProductVersion version = new BatchWipProductVersion()
            {
                ProductId = 0,
                WorkOrderId = product.WorkOrderId,
                RemainQty = wipProduct.Qty,
                Qty = wipProduct.Qty,
                NgQty = product.NgQty,
                ScrapQty = product.ScrapQty,
                BatchNo = wipBatch
            };
            version.GenerateId();
            wipProduct.VersionList.Add(version);
            wipProduct.CurrentVersion = version;
            RF.Save(wipProduct);
            return version;
        }

        /// <summary>
        /// 获取批次采集记录
        /// </summary> 
        /// <param name="wipBatch">批次号</param>
        /// <returns>批次采集记录列表</returns>
        public virtual EntityList<BatchWipProductProcess> GetBatchWipProductProcess(string wipBatch)
        {
            return Query<BatchWipProductProcess>().Join<BatchWipProductVersion>((x, y) => x.VersionId == y.Id && y.BatchNo == wipBatch).ToList();
        }

        /// <summary>
        /// 获取版本在当前工序对应的生产采集记录
        /// </summary>
        /// <param name="versionIds"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipRecord> GetBatchWipRecords(List<double> versionIds, Workcell workcell)
        {
            return versionIds.SplitContains(tempIds =>
            {
                return Query<BatchWipRecord>().Where(p => tempIds.Contains(p.BatchVersionId) && p.ResourceId == workcell.ResourceId && p.ProcessId == workcell.ProcessId && p.StationId == workcell.StationId).ToList();
            });
        }

        /// <summary>
        /// 查找最后一个产品版本(当前在制版本(wipBatch -> Bid,子批生成子批，父批生成父批))
        /// </summary>
        /// <param name="wipBatchNo">生产批次号</param>
        /// <returns>生产产品版本</returns>
        public virtual BatchWipProductVersion FindLastBatchWipProductVersion(string wipBatchNo)
        {
            return Query<BatchWipProductVersion>().OrderByDescending(p => p.CreateDate).Where(p => p.BatchNo == wipBatchNo /*&& p.Product.CurrentVersionId == p.Id*/).FirstOrDefault();
        }

        /// <summary>
        /// 查询所有批次版本
        /// </summary>
        /// <param name="wipBatchNos">生产批次号</param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProductVersion> FindLastBatchWipProductVersions(List<string> wipBatchNos)
        {
            return wipBatchNos.SplitContains(cds =>
            {
                return Query<BatchWipProductVersion>().OrderByDescending(p => p.CreateDate).Where(p => cds.Contains(p.BatchNo) /*&& p.Product.CurrentVersionId == p.Id*/).ToList();
            });
        }

        /// <summary>
        /// 创建父批次条码出站记录
        /// </summary>
        /// <param name="version">产品版本</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="outputQty">出站数量</param>
        /// <returns>工序记录</returns>
        private BatchWipRecord GenerateOutRecord(BatchWipProductVersion version, Workcell workcell, decimal outputQty)
        {
            BatchWipRecord batchWipRecord = new BatchWipRecord
            {
                BatchVersion = version,
                BatchNo = version.BatchNo,
                InOutType = PlugType.Out,
                Qty = outputQty,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                ResultType = ResultType.Pass,
            };
            return batchWipRecord;
        }

        /// <summary>
        /// 合并前子批次生产通用报表处理
        /// </summary>
        /// <param name="relations">批次关联关系</param>
        /// <param name="product">运行时产品</param>
        /// <param name="versions">生产版本</param>
        /// <param name="versionRecords">生产版本采集记录</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="batchWipProducts">生产批次产品</param>
        /// <param name="shift">班次</param>
        /// <param name="childVersion">子批次生产通用报表</param>
        /// <param name="batchWipRecord">子批次生产通用报表过站记录</param>
        void CreateMoveOutMergeWipRecord(List<BatchRelation> relations, product product, EntityList<BatchWipProductVersion> versions, EntityList<BatchWipRecord> versionRecords,
             CollectData collectData, Workcell workcell, EntityList<BatchWipProduct> batchWipProducts, Shift shift, out BatchWipProductVersion childVersion, out BatchWipRecord batchWipRecord, out EntityList<SIE.MES.BatchWIP.RelationBatch> relationBatches)
        {
            decimal mergeQty = 0;
            decimal ngQty = 0;
            StringBuilder sourceBatchNo = new StringBuilder();
            relationBatches = new EntityList<RelationBatch>();
            var relationBatchList = collectData.OutputBatch.RelationBatchList;
            foreach (var relation in relations)
            {
                // 更新批次来源
                relation.BatchSource = null;
                // 是否子批次
                var isChild = relation.Pid.IsNotEmpty();
                // 转入信息
                var inputView = isChild ? relationBatchList.FirstOrDefault(p => p.InputBatch.SubBatchNo == relation.Pid) : relationBatchList.FirstOrDefault(p => p.InputBatch.BatchNo == relation.Pid);
                if (relationBatches.Count == 0)
                {
                    relationBatches.Add(inputView);
                }
                // 生产通用信息
                var version = versions.FirstOrDefault(p => p.BatchNo == relation.Pid);
                // 父生产批次记录
                var versionRecord = versionRecords.FirstOrDefault(p => p.BatchVersionId == version.Id);
                var inputBatch = inputView.InputBatch;
                decimal outputQty = inputView.Qty;
                mergeQty += outputQty;
                ngQty += inputBatch.NgQty;
                version.RemainQty -= outputQty;
                versionRecord.SplitQty += outputQty;

                // 记录
                sourceBatchNo.Append("{0}:{1}".FormatArgs(inputBatch.IsChild ? inputBatch.SubBatchNo : inputBatch.BatchNo, inputBatch.Qty));
                sourceBatchNo.Append(';');
                //计算工艺路线后工序
                if (collectData.State == WipProductProcessState.Finish)
                {
                    ComputeNextProcess(product, collectData.Result, collectData);
                    SaveNextProcess(workcell, product, version);
                }
                //if (inputBatch.NgQty > 0)
                //{
                //    version.DefectState = BatchWIP.Products.SplitAndMerge.Enums.QState.UnPass;
                //}
            }
            // 生产批次产品
            var batchWipProduct = batchWipProducts.FirstOrDefault(p => p.ItemId == product.ItemId);
            childVersion = new BatchWipProductVersion()
            {
                Product = batchWipProduct,
                WorkOrderId = product.WorkOrderId,
                RemainQty = mergeQty,
                Qty = mergeQty,
                NgQty = ngQty,
                BatchNo = collectData.OutputBatch.SubBatchNo,
            };
            batchWipRecord = new BatchWipRecord
            {
                BatchVersion = childVersion,
                BatchNo = childVersion.BatchNo,
                SourceBatchNo = sourceBatchNo.ToString(),
                InOutType = PlugType.Out,
                Qty = childVersion.Qty,
                DefectQty = childVersion.NgQty,
                ResultType = ResultType.Pass,
                ShiftId = shift.Id,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
            };
            // 生成id是因为主表引用从表，同一事务无法保存2次
            childVersion.GenerateId();
            batchWipRecord.GenerateId();
            childVersion.CurrentProcessId = batchWipRecord.Id;
            childVersion.StationId = batchWipRecord.StationId;
            childVersion.ProcessId = batchWipRecord.ProcessId;
            childVersion.ResourceId = batchWipRecord.ResourceId;
            if (childVersion.NgQty > 0)
            {
                childVersion.DefectState = BatchWIP.Products.SplitAndMerge.Enums.QState.UnPass;
            }
        }

        /// <summary>
        /// 创建出站采集工序信息
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="version">生产版本</param>
        /// <param name="record">生产版本采集记录</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="batchWipProduct">生产批次产品</param>
        /// <param name="shift">班次</param>
        /// <param name="childVersion">子批次生产通用记录</param>
        /// <param name="batchWipRecord">批次关联关系</param>
        void CreateMoveOutSplitWipRecord(product product, BatchWipProductVersion version, BatchWipRecord record,
             CollectData collectData, Workcell workcell, BatchWipProduct batchWipProduct, Shift shift, out BatchWipProductVersion childVersion, out BatchWipRecord batchWipRecord)
        {
            var outputBatch = collectData.OutputBatch;
            decimal outputQty = outputBatch.Qty; // 转出数量

            // 1.父批拆批合批更新当前数量
            version.RemainQty -= outputQty;
            record.SplitQty += outputQty;
            // 2.拆分的子批或合并的子批生成批次通用报表和出站记录
            childVersion = new BatchWipProductVersion()
            {
                Product = batchWipProduct,
                WorkOrderId = product.WorkOrderId,
                RemainQty = outputQty - collectData.ScrapQty,//当前可用数量为转出数量-报废数量
                Qty = outputQty,
                NgQty = outputBatch.IsNg ? outputQty : 0,
                BatchNo = outputBatch.SubBatchNo,
            };
            batchWipRecord = new BatchWipRecord
            {
                BatchVersion = childVersion,
                BatchNo = childVersion.BatchNo,
                InOutType = PlugType.Out,
                Qty = childVersion.Qty,
                DefectQty = childVersion.NgQty,
                ResultType = ResultType.Pass,
                ShiftId = shift.Id,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                ScrapQty = collectData.ScrapQty,
            };
            childVersion.GenerateId();
            batchWipRecord.GenerateId();
            childVersion.CurrentProcessId = batchWipRecord.Id;
            childVersion.StationId = batchWipRecord.StationId;
            childVersion.ProcessId = batchWipRecord.ProcessId;
            childVersion.ResourceId = batchWipRecord.ResourceId;

            if (childVersion.NgQty > 0)
            {
                childVersion.DefectState = BatchWIP.Products.SplitAndMerge.Enums.QState.UnPass;
            }
        }


        /// <summary>
        /// 创建出站采集工序信息
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="version">生产版本</param>
        /// <param name="barcodes">采集条码</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="processType">工序类型</param>
        /// <returns>采集工序信息</returns>
        void CreateMoveOutWipProductProcess(product product, BatchWipProductVersion version,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell, ProcessType processType)
        {
            var outputBatch = collectData.OutputBatch;
            decimal outputQty = outputBatch.Qty;
            ////创建工序采集记录
            BatchWipProductProcess wipProductProcess = GetOrCreateProductProcess(version, workcell, outputQty);
            wipProductProcess.OutputQty += outputQty;
            wipProductProcess.OutputDate = RF.Find<BatchWipProductProcess>().GetDbTime();
            //创建工序采集记录明细 
            var outputDetail = CreateMoveOutProcessDetail(collectData, workcell, processType, outputBatch, outputQty, wipProductProcess);
            if (collectData.State == WipProductProcessState.Finish)
            {
                version.ScrapQty += collectData.ScrapQty;
                version.NgQty += collectData.NgQty;

                //工艺路线配置当前工序要创建SKU
                var workOrderMove = product.WorkOrderMove;

                if (product.Routing.Current.CreateSku)
                {
                    RT.Service.Resolve<ItemLabelController>()
                        .CreateItemLabel(product.WorkOrder.Product, product.Qty - product.NgQty, barcodes.Last().Code,
                         LabelSource.BatchWip, product.WorkOrderId, workOrderMove.FactoryId, workOrderMove.ItemExtProp,
                         workOrderMove.ItemExtPropName, workOrderMove.ProjectMaintain?.Code);
                }

                ////设置产品不良
                if (processType == ProcessType.BatchPqc && collectData.Result == ResultType.Fail)
                {
                    product.IsNg = true;
                }
            }

            RF.Save(wipProductProcess);
            RF.Save(outputDetail);

            if (collectData.State == WipProductProcessState.Finish)
            {
                //OnBatchWipProductProcessFinished(wipProductProcess, product, barcodes, collectData, workcell, outputDetail);
            }
        }

        /// <summary>
        /// 创建过站工序明细
        /// </summary>
        /// <param name="collectData">采集时间</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="processType">工序类型</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="outputQty">出站数量</param>
        /// <param name="wipProductProcess">批次产品工序</param>
        private BatchWipProductProcessDetail CreateMoveOutProcessDetail(CollectData collectData, Workcell workcell,
            ProcessType processType, OutputBatch outputBatch, decimal outputQty, BatchWipProductProcess wipProductProcess)
        {
            var outputDetail = CreateBatchProcessDetail(processType, collectData, workcell, outputBatch, outputQty);

            outputDetail.ProductProcessId = wipProductProcess.Id;
            foreach (RelationBatch item in outputBatch.RelationBatchList)
            {
                var inputBatch = item.InputBatch;
                var inputDetail = wipProductProcess.DetailList.FirstOrDefault(p => p.PlugType == PlugType.In
                    && p.BatchNo == inputBatch.BatchNo
                    && (p.SubBatchNo == inputBatch.SubBatchNo
                        || (!p.ContainerNo.IsNullOrEmpty() && p.ContainerNo == inputBatch.ContainerNo)));

                if (inputDetail == null)
                {
                    continue;
                }

                outputDetail.InputDetailId = inputDetail.Id;
                inputDetail.RemainQty -= item.Qty;
            }
            return outputDetail;
        }

        /// <summary>
        /// 获取或者创建批次产品工序记录
        /// </summary>
        /// <param name="version">产品版本</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="outputQty">出站数量</param>
        /// <returns>工序记录</returns>
        private BatchWipProductProcess GetOrCreateProductProcess(BatchWipProductVersion version, Workcell workcell, decimal outputQty)
        {
            BatchWipProductProcess wipProductProcess = GetProductProcess(version.Id, workcell);
            if (wipProductProcess == null)
            {
                var date = RF.Find<BatchWipProductProcess>().GetDbTime();
                wipProductProcess = new BatchWipProductProcess();

                //提前生成主键ID，后面要使用到
                wipProductProcess.GenerateId();

                wipProductProcess.ProcessId = workcell.ProcessId;
                wipProductProcess.ResourceId = workcell.ResourceId;
                wipProductProcess.InputDate = date;
                wipProductProcess.OutputDate = date;
                wipProductProcess.VersionId = version.Id;
                //version.ProcessList.Add(wipProductProcess); 2023/3/27 csp 优化为直接wipProductProcess.VersionId = version.Id;                
                version.CurrentProcessId = wipProductProcess.Id;
                version.StationId = workcell.StationId;
                version.ProcessId = wipProductProcess.ProcessId;
                version.ResourceId = wipProductProcess.ResourceId;
            }

            return wipProductProcess;
        }

        /// <summary>
        /// 创建工序明细
        /// </summary>
        /// <param name="processType">工序类型</param> 
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="outputQty">出站数量</param> 
        /// <returns>工序明细</returns>
        private BatchWipProductProcessDetail CreateBatchProcessDetail(ProcessType processType, CollectData collectData, Workcell workcell, OutputBatch outputBatch, decimal outputQty)
        {
            DateTime inputDate;
            if (processType == ProcessType.BatchFix)
                inputDate = (DateTime)collectData.Context["InputDate"];
            else
                inputDate = outputBatch.RelationBatchList.Select(p => p.InputBatch).OrderBy(p => p.InputDate).FirstOrDefault().InputDate;
            var detail = new BatchWipProductProcessDetail()
            {
                ProcessId = workcell.ProcessId,
                ResourceId = workcell.ResourceId,
                StationId = workcell.StationId,
                OperateById = workcell.EmployeeId,
                Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now),
                BatchNo = outputBatch.BatchNo,
                SubBatchNo = outputBatch.SubBatchNo,
                BatchState = BatchState.Out,
                PlugType = PlugType.Out,
                Qty = outputQty,
                RemainQty = outputQty,
                InputDate = inputDate,
                OutputDate = RF.Find<BatchWipProductProcessDetail>().GetDbTime(),
                ResultType = collectData.Result,
                ScrapQty = collectData.ScrapQty,
                NgQty = collectData.NgQty,
                ContainerNo = outputBatch.ContainerNo
            };
            detail.GenerateId();
            return detail;
        }

        /// <summary>
        /// 获取批次工序采集记录
        /// </summary>
        /// <param name="versionId">产品版本ID</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>批次采集记录</returns>
        BatchWipProductProcess GetProductProcess(double versionId, Workcell workcell)
        {
            return Query<BatchWipProductProcess>().Where(p => p.VersionId == versionId && p.ResourceId == workcell.ResourceId && p.ProcessId == workcell.ProcessId).FirstOrDefault();
        }

        /// <summary>
        /// 获取在工作单元下的类型是入站的记录
        /// </summary>
        /// <param name="versionId">生产通用报表</param>
        /// <param name="workcell">工作单元信息</param>
        /// <returns></returns>
        BatchWipRecord GetBatchWipRecord(double versionId, Workcell workcell)
        {
            return Query<BatchWipRecord>().Where(p => p.BatchVersionId == versionId && p.ResourceId == workcell.ResourceId && p.ProcessId == workcell.ProcessId && p.InOutType == PlugType.In).FirstOrDefault();
        }

        /// <summary>
        /// 创建批次产品版本信息
        /// </summary> 
        /// <param name="version">生产产品版本</param>
        /// <param name="inputBatch">转入批次</param>
        /// <param name="workcell">工作单元信息</param>
        void CreateMoveInProductProcess(BatchWipProductVersion version, InputBatch inputBatch, Workcell workcell)
        {
            BatchWipProductProcess wipProductProcess = GetProductProcess(version.Id, workcell);
            if (wipProductProcess == null)
            {
                var date = RF.Find<BatchWipProductProcess>().GetDbTime();
                wipProductProcess = new BatchWipProductProcess();
                wipProductProcess.GenerateId();
                wipProductProcess.ProcessId = workcell.ProcessId;
                wipProductProcess.ResourceId = workcell.ResourceId;
                wipProductProcess.InputDate = date;
                wipProductProcess.InputQty = inputBatch.RemainQty;
                wipProductProcess.VersionId = version.Id;
                // version.ProcessList.Add(wipProductProcess); 2023/3/27 优化为wipProductProcess.VersionId = version.Id; 
            }
            else
            {
                wipProductProcess.InputQty += inputBatch.RemainQty;
            }
            BatchWipProductProcessDetail detail = wipProductProcess.DetailList
                .FirstOrDefault(p => p.StationId == workcell.StationId
                    && p.BatchNo == inputBatch.BatchNo
                    && (p.SubBatchNo == inputBatch.SubBatchNo
                        || (!p.ContainerNo.IsNullOrEmpty() && p.ContainerNo == inputBatch.ContainerNo))
                    && p.PlugType == PlugType.In);

            if (detail == null)
            {
                detail = new BatchWipProductProcessDetail()
                {
                    ProcessId = workcell.ProcessId,
                    ResourceId = workcell.ResourceId,
                    StationId = workcell.StationId,
                    OperateById = workcell.EmployeeId,
                    Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now),
                    BatchNo = inputBatch.BatchNo,
                    SubBatchNo = inputBatch.SubBatchNo,
                    ContainerNo = inputBatch.ContainerNo,
                    BatchState = BatchState.In,
                    PlugType = PlugType.In,
                    Qty = inputBatch.Qty,
                    RemainQty = inputBatch.RemainQty,
                    InputDate = DateTime.Now,
                    ResultType = ResultType.Pass,
                };
                detail.GenerateId();
                wipProductProcess.DetailList.Add(detail);
            }
            else
            {
                detail.OperateById = workcell.EmployeeId;
                detail.ContainerNo = inputBatch.ContainerNo;
                detail.Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now);
                detail.BatchState = BatchState.In;
                detail.PlugType = PlugType.In;
                detail.Qty = inputBatch.Qty;
                detail.RemainQty = inputBatch.RemainQty;
                detail.InputDate = DateTime.Now;
                detail.ResultType = ResultType.Pass;
            }
            version.CurrentProcessId = wipProductProcess.Id;
            version.StationId = workcell.StationId;
            version.ProcessId = wipProductProcess.ProcessId;
            version.ResourceId = wipProductProcess.ResourceId;
            RF.Save(wipProductProcess);
            RF.Save(version);
        }

        /// <summary>
        /// 创建批次条码入站信息
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <param name="relation">批次关系</param>
        /// <param name="inputBatch">转入批次</param>
        /// <param name="workcell">工作单元信息</param>
        void CreateMoveInProductRecord(BatchWipProductVersion version, BatchRelation relation, InputBatch inputBatch, Workcell workcell)
        {
            // 班次
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now);
            if (shift == null)
            {
                throw new ValidationException("当前生产资源班次为空！".L10N());
            }
            // 240716新生成记录
            var record = new BatchWipRecord()
            {
                BatchVersion = version,
                BatchNo = inputBatch.IsChild ? inputBatch.SubBatchNo : inputBatch.BatchNo,
                InOutType = PlugType.In,
                Qty = inputBatch.RemainQty,
                DefectQty = inputBatch.NgQty,
                ShiftId = shift.Id,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                ResultType = ResultType.Pass,
            };
            // 维修入站如果批次关系不良则关系不良数量
            if (relation.IsNg)
            {
                record.DefectQty = relation.Qty;
            }
            record.GenerateId();
            version.CurrentProcessId = record.Id;
            version.StationId = record.StationId;
            version.ProcessId = record.ProcessId;
            version.ResourceId = record.ResourceId;
            RF.Save(record);
            RF.Save(version);
        }

        /// <summary>
        /// 更新采集记录
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="batch">转入批次</param>
        private void UpdateBProductRecord(Workcell workcell, string batchNo, InputBatch batch)
        {
            var record = Query<BatchWipRecord>().Join<BatchWipProductVersion>((x, y) => x.BatchVersionId == y.Id && y.BatchNo == batchNo)
                .Where(p => p.ResourceId == workcell.ResourceId && p.ProcessId == workcell.ProcessId && p.StationId == workcell.StationId).FirstOrDefault();
            if (record != null)
            {
                record.Qty -= batch.RemainQty;
                RF.Save(record);
            }
        }

        /// <summary>
        /// 更新采集记录
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="containerNo">载具号</param>
        /// <param name="batch">转入批次</param>
        /// <param name="relations">批次关联关系</param>
        private void UpdateProductProcessDetail(Workcell workcell, string containerNo, InputBatch batch, EntityList<BatchRelation> relations)
        {
            var version = FindLastBatchWipProductVersion(relations[0].WipBatch);
            BatchWipProductProcess wipProductProcess = GetProductProcess(version.Id, workcell);
            var detail = wipProductProcess?.DetailList.Where(p => p.BatchNo == batch.BatchNo && p.SubBatchNo == batch.SubBatchNo).OrderByDescending(p => p.InputDate).FirstOrDefault();
            if (detail != null)
            {
                detail.BatchState = BatchState.Removed;
                detail.RemainQty = 0;
                if (!containerNo.IsNullOrEmpty())
                    detail.ContainerNo = containerNo;
                RF.Save(detail);
                detail.ProductProcess.InputQty -= batch.RemainQty;
                RF.Save(detail.ProductProcess);
            }
        }

        /// <summary>
        /// 获取批次过站明细视图模型
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>采集明细</returns>
        public virtual CollectDetailViewModel GetMoveInDetailEvent(CollectDetailQuery queryInfo)
        {
            var detail = GetNewestMoveInDetail(queryInfo);
            if (detail == null)
                return new CollectDetailViewModel();
            return InitCollectDetail(detail);
        }

        /// <summary>
        /// 获取最新
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>批次工序过站明细</returns>
        BatchWipRecord GetNewestMoveInDetail(CollectDetailQuery queryInfo)
        {
            //return Query<BatchWipProductProcessDetail>()
            //             .Join<BatchWipProductProcess>((d, p) => d.ProductProcessId == p.Id)
            //             .Join<BatchWipProductProcess, BatchWipProductVersion>((p, v) => p.VersionId == v.Id && v.WorkOrderId == queryInfo.WorkOrderId)
            //             .Where(p => p.OperateById == queryInfo.OperateById && p.StationId == queryInfo.StationId
            //             && p.ResourceId == queryInfo.ResourceId && p.ProcessId == queryInfo.ProcessId
            //             && p.PlugType == queryInfo.PlugType)
            //             .OrderByDescending(p => p.CreateDate)
            //             .FirstOrDefault();
            return Query<BatchWipRecord>().Where(p => p.CreateBy == queryInfo.OperateById && p.StationId == queryInfo.StationId
                         && p.ResourceId == queryInfo.ResourceId && p.ProcessId == queryInfo.ProcessId
                         && p.InOutType == queryInfo.PlugType).OrderByDescending(p => p.CreateDate)
                         .FirstOrDefault();
        }

        /// <summary>
        /// 初始化采集明细
        /// </summary>
        /// <param name="detail">工序明细</param>
        /// <returns>采集明细</returns>
        CollectDetailViewModel InitCollectDetail(BatchWipRecord detail)
        {
            return new CollectDetailViewModel()
            {
                BatchNo = detail.BatchNo,
                PlugType = detail.InOutType,
                Qty = detail.Qty,
                SplitQty = detail.SplitQty,
                NgQty = detail.DefectQty,
                OptTme = detail.CreateDate,
                Result = detail.ResultType,
                ScrapQty = detail.ScrapQty,
            };
        }
    }
}