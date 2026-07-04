using SIE.Barcodes.WipBatchs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 生产采集控制器
    /// </summary>
    public partial class WipController
    {
        /// <summary>
        /// 创建子批次运行时数据
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="processId">工序ID</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>产品信息</returns>
        private product CreateSubWipProduct(CollectData collectData, double processId, OutputBatch outputBatch, double workOrderId)
        {
            var wo = GetById<WorkOrder>(workOrderId);
            product product = CreateBatchProduct(Guid.NewGuid().ToString("N").ToUpper(), wo, processId, collectData, outputBatch);
            RuntimeController.MapPuid(new CollectBarcode() { Code = outputBatch.SubBatchNo, Type = BarcodeType.BatchBarocde }, product.Puid);
            if (outputBatch.BarcodeType == BarcodeType.ContainerNo)
                RuntimeController.MapPuid(new CollectBarcode() { Code = outputBatch.ContainerNo, Type = BarcodeType.ContainerNo }, product.Puid);
            return product;
        }

        /// <summary>
        /// 创建新运行时产品，同时创建在制品和在制品版本
        /// </summary>
        /// <param name="relation">批次关联</param>
        /// <param name="processId">工序ID</param>
        /// <returns>运行时产品</returns>
        protected virtual product CreateNewBatchProduct(BatchRelation relation, double processId)
        {
            var product = CreateBatchProduct(relation, processId);
            var wipProduct = CreateBatchWipProduct(product);
            CreateBatchWipProductVersion(wipProduct, product, relation.Bid);// 20240410 扫描子批次生成子批次的记录
            return product;
        }

        /// <summary>
        /// 创建新运行时产品
        /// </summary>
        /// <param name="relation">批次关联</param>
        /// <param name="processId">工序ID</param>
        /// <returns>运行时产品</returns>
        protected virtual product CreateBatchProduct(BatchRelation relation, double processId)
        {
            var wo = GetById<WorkOrder>(relation.WorkOrderId);
            if (wo == null)
                throw new ValidationException("找不到[{0}]对应的工单".L10nFormat(relation.Bid));
            var product = CreateBatchProduct(Guid.NewGuid().ToString("N").ToUpper(), wo, processId, relation);
            RuntimeController.MapPuid(new CollectBarcode() { Code = relation.Bid, Type = BarcodeType.BatchBarocde }, product.Puid);
            return product;
        }

        /// <summary>
        /// 产品运行时产品
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <param name="workOrder">工单</param> 
        /// <param name="processId">工序ID</param>
        /// <param name="relation">批次关系</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="ValidationException">产品未上线</exception>
        protected virtual product CreateBatchProduct(string puid, WorkOrder workOrder, double processId, BatchRelation relation)
        {
            var product = new product();
            product.Puid = puid;
            product.WorkOrderId = workOrder.Id;
            product.ItemId = workOrder.ProductId;
            product.IsHold = false;
            product.Routing.Processes.AddRange(GetRoutingProcess(workOrder.Id, workOrder.No));
            product.Qty = /*relation.Pid.IsNullOrEmpty() && !relation.BatchSource.HasValue ? relation.Qty :*/ relation.RemainQty;
            // 

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
            if (!startProcesses.Any(x => x.ProcessId == processId))
            {
                var startProcess = startProcesses.Select(x => x.Name).Concat("、");

                throw new ValidationException("[{0}]产品未上线，上线工序应该为[{1}]"
                    .L10nFormat(relation.Bid, startProcess));
            }

            //所有上线工序ID添加到后工序ID列表中
            product.Routing.Next.AddRange(startProcesses.Select(x => x.Id));

            RuntimeController.Save(product);
            SaveBatchRuntimeRouting(relation.Bid, workOrder.Id, workOrder.Layout?.Layout);
            return product;
        }

        /// <summary>
        /// 创建运行时产品信息
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <param name="workOrder">工单</param>
        /// <param name="processId">当前工序ID</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="outputBatch">转出批次</param>
        /// <returns>运行时产品</returns>
        protected virtual product CreateBatchProduct(string puid, WorkOrder workOrder, double processId, CollectData collectData, OutputBatch outputBatch)
        {
            var inputBatch = outputBatch.RelationBatchList.FirstOrDefault()?.InputBatch;
            if (inputBatch == null)
            {
                throw new ArgumentNullException("转入批次为空".L10N());
            }
            var batchNo = inputBatch.SubBatchNo.IsNullOrEmpty() ? inputBatch.BatchNo : inputBatch.SubBatchNo;
            var product = new product();
            product.Puid = puid;
            product.WorkOrderId = workOrder.Id;
            product.ItemId = workOrder.ProductId;
            product.IsHold = false;
            var parentProduct = GetParentProduct(batchNo);
            product.Routing.Processes.AddRange(GetRoutingProcess(workOrder.Id, workOrder.No));//parentProduct.Routing.Processes);
            product.IsNg = parentProduct.IsNg;
            product.Qty = collectData.OutputBatch.Qty;
            var parentNextId = parentProduct.Routing.GetNext().First()?.Id;

            //批次产品工艺路线切换后报空的问题
            var parentNextProcess = product.Routing.Processes.FirstOrDefault(p => p.Id == parentNextId);
            product.Routing.Current = parentNextProcess!=null ? parentNextProcess: product.Routing.Processes.FirstOrDefault(p => p.ProcessId == processId);
            RuntimeController.Save(product);
            SaveBatchRuntimeRouting(workOrder.Id, outputBatch.SubBatchNo, batchNo);
            return product;
        }

        /// <summary>
        /// 保存批次运行时工艺路线
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="subBatchNo">子批次号</param>
        /// <param name="batchNo">父批次号</param>
        public virtual void SaveBatchRuntimeRouting(double workOrderId, string subBatchNo, string batchNo)
        {
            var parentLayout = RT.Service.Resolve<BatchWipProductRoutingController>().GetBatchRuntimeRoutingLayout(batchNo, workOrderId);
            if (parentLayout == null)
                throw new ValidationException("未找到父批次运行时工艺路线布局，请检查".L10N());
            SaveBatchRuntimeRouting(subBatchNo, workOrderId, parentLayout.Layout);
        }

        /// <summary>
        /// 保存批次运行时工艺路线
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="layout">布局</param>
        private void SaveBatchRuntimeRouting(string batchNo, double workOrderId, string layout)
        {
            RT.Service.Resolve<BatchWipProductRoutingController>().CreateBatchRuntimeRouting(batchNo, workOrderId, layout);
        }

        /// <summary>
        /// 获取父工艺路线工序
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>工序列表</returns>
        product GetParentProduct(string batchNo)
        {
            var product = RuntimeController.FindProduct(batchNo, BarcodeType.BatchBarocde);
            if (product == null)
                throw new ValidationException("未找到父批次[{0}]运行时产品信息".L10nFormat(batchNo));
            return product;
        }

        /// <summary>
        /// 批次生产完成后下线，清空运行时数据
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="relation">批次关联关系</param>
        /// <param name="workcell"></param>
        protected virtual void CompleteBatchProduct(product product, BatchRelation relation, Workcell workcell)
        {
            if (relation == null)
            {
                throw new ArgumentNullException(nameof(relation));
            }

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            //过站逻辑-倒扣非工序BOM物料
            RT.Service.Resolve<BackflushMaterialController>()
                .BackflushMaterialByFinsh(relation.Bid, workcell.ResourceId, workcell.ProcessId, workcell.StationId, product);

            relation.IsFinish = true;
            ////更新生产批次状态
            RuntimeController.RemoveProduct(product);
            ////清除运行时数据
            var version = relation.BatchSource != null ? FindLastBatchWipProductVersion(relation.Pid) : FindLastBatchWipProductVersion(relation.Bid);
            version.FinishQty += relation.Qty;
            if (version.FinishQty + version.ScrapQty >= version.RemainQty)
            {
                version.IsFinish = true;
                version.Product.State = WipProductState.Finish;
                RF.Save(version);
                RF.Save(version.Product);
                var wipBatchProduct = relation.BatchSource != null ? RuntimeController.FindProduct(relation.Pid, BarcodeType.BatchBarocde) : RuntimeController.FindProduct(relation.Bid, BarcodeType.BatchBarocde);
                if (wipBatchProduct != null)
                {
                    RuntimeController.RemoveProduct(wipBatchProduct);
                }
            }
            else
            {
                RF.Save(version);
            }

            RF.Save(relation);
        }

        /// <summary>
        /// 验证采集运行时产品的工艺路线
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="processId">工序ID</param>
        /// <param name="isValidateProcess">是否验证下一工序</param>
        /// <returns>采集运行时产品</returns>
        protected virtual product ValidateBatchProduct(CollectBarcode barcode, double processId, bool isValidateProcess = true)
        {
            if (barcode is null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            var relation = BatchController.GetBatchRelation(barcode);
            if (relation == null)
            {
                throw new ValidationException("未找到[{0}]批次关系".L10nFormat(barcode.Code));
            }

            // 原扫描子批次会校验父批次
            //if (relation.Bid != relation.WipBatch)
            //{
            //    ValidateBatchProduct(new CollectBarcode() { Code = relation.WipBatch, Type = BarcodeType.BatchBarocde }, processId, false);
            //}

            var product = RuntimeController.FindProduct(barcode);
            var version = FindLastBatchWipProductVersion(relation.Bid);// 20240410 扫描子批次生成子批次的记录

            if (product == null) ////可能未上线，或者数据已清空
            {
                if (version == null) ////没有生产记录，创建新产品 
                {
                    product = CreateNewBatchProduct(relation, processId);
                }
                else
                {
                    //子批次是否完工下线
                    if (relation.IsFinish)
                    {
                        throw new ValidationException("[{0}]批次已生产完成,不允许再生产".L10nFormat(barcode));
                    }

                    //创建子批次产品
                    product = CreateBatchProduct(relation, processId);
                }
            }
            else
            {
                if (version.RemainQty <= 0)
                {
                    throw new ValidationException("[{0}]批次当前数量为0，已被拆分或合并，请检查".L10nFormat(barcode));
                }

                if (relation.IsPause == YesNo.Yes)
                {
                    throw new ValidationException("[{0}]产品已暂停，不能继续生产".L10nFormat(barcode));
                }

                if (version.IsOutsourcing)
                {
                    throw new ValidationException("产品【{0}】状态为【委外加工中】，不能继续过站，如委外加工完成，请确认是否已【委外入库】!"
                        .L10nFormat(barcode));
                }
            }

            ////验证工序
            if (isValidateProcess && !product.Routing.GetNext().Any(p => p?.ProcessId == processId))
            {
                var nextProcess = product.Routing.GetNext().Select(p => p?.Name).Concat("、");
                throw new ValidationException("[{0}]采集工序不正确，应该为[{1}]".L10nFormat(barcode, nextProcess));
            }

            return product;
        }

        /// <summary>
        /// 验证采集运行时产品的工艺路线
        /// </summary>
        /// <param name="collectData">条码</param>
        /// <param name="processId">工序ID</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="inputBatch">转入批次</param>
        /// <param name="collectBarcode">采集条码</param>
        /// <returns>采集运行时产品</returns>
        private product ValidateBatchProduct(CollectData collectData, double processId, OutputBatch outputBatch, InputBatch inputBatch, CollectBarcode collectBarcode)
        {
            product product;
            if (outputBatch.IsGenerateBatch)
            {
                //生成子批次信息、创建批次关联信息、根据工单生成新批次的运行时数据
                SubWipBatch childBarcode = CreateSubWipBatch(outputBatch, inputBatch.BatchNo, collectData.ScrapQty);
                product = CreateSubWipProduct(collectData, processId, outputBatch, childBarcode.WorkOrderId);
                ContainerBind(collectBarcode.Type, outputBatch.SubBatchNo, collectBarcode, outputBatch.Qty, true);
            }
            else
            {
                var batchNo = inputBatch.SubBatchNo.IsNullOrEmpty() ? inputBatch.BatchNo : inputBatch.SubBatchNo;
                ContainerBind(collectBarcode.Type, batchNo, collectBarcode, outputBatch.Qty);
                product = ValidateBatchProduct(collectBarcode, processId);
                var relation = BatchController.GetBatchRelation(collectBarcode);
                if (relation != null&& relation.RemainQty<=0)
                {
                    throw new ValidationException("批次的当前数量为0,可能批次已被拆分或合并，无法继续操作，请移除");
                }
                var nextId = product.Routing.GetNext().First(m=>m.ProcessId == processId)?.Id;//2024-7-1 csp 当前工序应该为当前工作单元工序下的工序 不能随机取第一个 如果存在可选的时候就会有问题
                product.Routing.Current = nextId.HasValue ? product.Routing.Processes.FirstOrDefault(m => m.Id == nextId) : product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == processId);
            }

            return product;
        }

        /// <summary>
        /// 恢复运行时产品，NoSql数据库可能会出现运行时产品数据与在制品数据不一致，预留此方法，未实现
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <returns>运行时产品</returns>
        /// <exception cref="NotImplementedException">未实现异常</exception>
        protected virtual product RecoverProduct(BatchWipProductVersion version)
        {
            throw new NotImplementedException("产品未生产完成，但没有生产运行时数据，数据一致性问题，暂时未实现数据恢复");
        }
    }
}