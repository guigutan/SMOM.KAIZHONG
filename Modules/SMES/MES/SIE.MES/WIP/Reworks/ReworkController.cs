using SIE.Barcodes;
using SIE.Core.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders.Reworks;
using SIE.Packages.ItemLabels;
using SIE.Tech.Processs;
using SIE.Utils;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using Barcode = SIE.Barcodes.Barcode;

namespace SIE.MES.WIP.Reworks
{
    /// <summary>
    /// 返工控制器
    /// </summary>
    public partial class ReworkController : WipController
    {
        #region 返工采集
        /// <summary>
        /// 验证采集步骤
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="steps">采集步骤数组</param>
        /// <param name="collectBarcode">采集步骤（前端传入）</param>
        /// <param name="noValidateStep">不验证采集步骤</param>
        /// <returns>采集条码集合</returns>
        /// <exception cref="ValidationException">条码与采集步骤不一致、条码为空</exception>
        protected override IList<CollectBarcode> ValidateCollectStep(string[] barcodes, ProcessCollectStep[] steps, CollectBarcode collectBarcode, bool noValidateStep)
        {
            var collectBarcodes = new List<CollectBarcode>();
            for (int i = 0; i < barcodes.Length; i++)
            {
                if (barcodes[i].IsNullOrWhiteSpace())
                {
                    throw new ValidationException("条码类型：{0} 的条码不允许为空".L10nFormat(EnumViewModel.EnumToLabel(steps[i].BarcodeType).L10N()));
                }
                collectBarcodes.Add(new CollectBarcode { Code = barcodes[i], Type = steps[i].BarcodeType });
            }

            return collectBarcodes;
        }

        /// <summary>
        /// 采集完成，子类重写实现特殊逻辑
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品</param>
        /// <param name="collectBarcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param> 
        protected override void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {            
            if (wipProductProcess == null)
            {
                throw new EntityNotFoundException(nameof(wipProductProcess));
            }

            if (collectData == null)
            {
                throw new EntityNotFoundException(nameof(collectData));
            }

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            base.OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);


            OnWipProductProcessKeyItemsUnbound(collectData);

            // 生产产品版本的属性值设置
            var wipPrcVersion = wipProductProcess.Version;

            if (wipPrcVersion != null)
            {
                wipPrcVersion.RelevanceSn = collectData.ReworkData.OriginalBarcode;

                //这里不要保存，在OnWipProductProcessFinished后面会保存 WipPrcVersion
                //RF.Save(wipPrcVersion);
            }

            // 工单关联条码的属性值设置
            var originalBarcode = collectData.ReworkData.OriginalBarcode;
            var curUnionBarcode = GetUnionBarcode(product.WorkOrderId, WorkOrderType.Rework, originalBarcode, null);
            if (curUnionBarcode == null)
            {
                throw new ValidationException("原工单条码[{0}]未进行返工配置!"
                    .L10nFormat(originalBarcode));
            }
            else
            {
                if (curUnionBarcode.ReworkBarcode.IsNullOrEmpty())
                {
                    var rewrkBarcode = collectData.ReworkData.ReworkBarcode;
                    curUnionBarcode.ReworkBarcode = rewrkBarcode;
                    curUnionBarcode.CodeState = CodeState.Associated;
                    RF.Save(curUnionBarcode);
                }
            }
        }

        /// <summary>
        /// 处理采集数据的关键件解绑--WipProductProcessKeyItem
        /// </summary>
        /// <param name="collectData">采集数据</param>
        private void OnWipProductProcessKeyItemsUnbound(CollectData collectData)
        {
            List<double> keyItemIds = collectData.ReworkData.KeyItems;
            if (keyItemIds != null && keyItemIds.Count > 0)
            {
                RT.Service.Resolve<ReworkController>().UnboundKeyItems(keyItemIds);
            }
        }




        /// <summary>
        /// 解绑关键件集合
        /// </summary>
        /// <param name="wipKeyItemIds">产品生产关健件Id集合</param>
        public virtual void UnboundKeyItems(List<double> wipKeyItemIds)
        {
            var wipKeyItems = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItems(wipKeyItemIds);
            if (wipKeyItemIds != null && wipKeyItemIds.Count > 0)
            {
                foreach (var keyItem in wipKeyItems)
                {
                    keyItem.IsUnbound = true;
                }

                RF.Save(wipKeyItems);
            }
        }

        /// <summary>
        /// 解绑关键件
        /// </summary>
        /// <param name="wipKeyItemId">关健件Id</param>
        public virtual void UnboundKeyItem(double wipKeyItemId)
        {
            var wipKeyItem = RF.GetById<WipProductProcessKeyItem>(wipKeyItemId);
            if (wipKeyItem != null)
            {
                wipKeyItem.IsUnbound = true;
                RF.Save(wipKeyItem);
            }
        }

        /// <summary>
        /// 获取解绑的关健件配置
        /// </summary>
        /// <param name="originalBarcode">原生产条码</param>
        /// <returns>关健件解绑配置</returns>
        public virtual EntityList<KeyItemUnboundConfig> GetKeyItemUnboundConfigs(string originalBarcode)
        {
            EntityList<KeyItemUnboundConfig> keyItemUnbdCfgs = null;
            var unionBarcode = Query<UnionBarcode>().Where(x => x.OriginalBarcode == originalBarcode && x.CodeState == CodeState.Associated).FirstOrDefault();
            if (unionBarcode != null)
                keyItemUnbdCfgs = Query<KeyItemUnboundConfig>().Where(p => p.WorkOrderId == unionBarcode.WorkOrderId && p.IsUnbound).ToList();
            return keyItemUnbdCfgs;
        }

        /// <summary>
        /// 获取工单关联条码实体
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="type">工单类型</param>
        /// <param name="originalBarcode">原工单条码</param>
        /// <param name="reworkBarcode">返工工单条码</param>
        /// <returns>工单关联条码实体</returns>
        public virtual UnionBarcode GetUnionBarcode(double workOrderId, WorkOrderType type, string originalBarcode, string reworkBarcode)
        {
            var query = Query<UnionBarcode>().Where(x => x.WorkOrderId == workOrderId && x.WorkOrder.Type == type);
            if (originalBarcode.IsNotEmpty())
                query.Where(x => x.OriginalBarcode == originalBarcode);
            if (reworkBarcode.IsNotEmpty())
                query.Where(x => x.ReworkBarcode == reworkBarcode);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 工单关联条码是否存在
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="type">工单类型</param>
        /// <param name="originalBarcode">原工单条码</param>
        /// <param name="reworkBarcode">返工工单条码</param>
        /// <returns>true: 条码关联存在; false:条码关联不存在</returns>
        public virtual bool ExistUnionBarcode(double workOrderId, WorkOrderType type, string originalBarcode, string reworkBarcode)
        {
            var query = GetUnionBarcode(workOrderId, type, originalBarcode, reworkBarcode);
            if (query == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断原条码是否已经置换
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="type">工单类型</param>
        /// <param name="originalBarcode">原条码</param>
        /// <returns>true: 已经被置换; false: 未置换</returns>
        public virtual bool CheckOriginalBarcodeHavePermuted(double workOrderId, WorkOrderType type, string originalBarcode)
        {
            var curUnionBarcode = GetUnionBarcode(workOrderId, type, originalBarcode, null);
            if (curUnionBarcode == null)
                return false;
            else if (curUnionBarcode.ReworkBarcode.IsNullOrEmpty())
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查工单是否已经设置关联
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>true: 已关联; false:未关联</returns>
        public virtual bool CheckUnionBarcodeExist(string workOrderNo)
        {
            if (workOrderNo.IsNullOrEmpty())
                throw new ValidationException("参数工单号不能为空!".L10N());
            var unionExist = true;
            var query = Query<UnionBarcode>().Where(p => p.WorkOrder.No == workOrderNo);
            if (query.Count() > 0)
                unionExist = true;
            else
                unionExist = false;
            return unionExist;
        }
        #endregion 返工采集

        #region 返工配置
        /// <summary>
        /// 返工条码关联
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="configs">关键件解绑配置</param>
        /// <param name="unionBarcodes">关联条码列表</param>
        public virtual void UnionBarcode(double workOrderId, EntityList<KeyItemUnboundConfig> configs, EntityList<UnionBarcode> unionBarcodes)
        {
            var workOrder = GetById<WorkOrder>(workOrderId);

            if (workOrder == null)
            {
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                ////条码关联
                UnionBarcode(workOrder, unionBarcodes);

                ////保存关键件解绑配置
                SaveKeyItemUnboundConfig(configs);

                tran.Complete();
            }
        }

        /// <summary>
        /// 保存关键件列表
        /// </summary>
        /// <param name="configs">关键件列表</param>
        void SaveKeyItemUnboundConfig(EntityList<KeyItemUnboundConfig> configs)
        {
            if (!configs.Any())
            {
                return;
            }

            RF.Save(configs);
        }

        /// <summary>
        /// 保存关联条码
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="unionBarcodes">关联条码列表</param>
        void UnionBarcode(WorkOrder workOrder, EntityList<UnionBarcode> unionBarcodes)
        {
            if (!unionBarcodes.Any())
            {
                return;
            }

            var relevancyQty = RT.Service.Resolve<ReworkController>().GetUnionBarcodeCount(workOrder.Id);
            if (workOrder.PlanQty < relevancyQty + unionBarcodes.Count)
            {
                throw new ValidationException("已超过工单计划数量：{0}".L10nFormat(workOrder.PlanQty));
            }

            /*
             关联条码是原工单条码的，保存步骤：1.关联条码工单改为返修工单；2.原工单条码运行时记录清除；3.原工单条码下线(与原工单关联关系设置为未关联)；4.返修工单已打印数递增(使用已有条码的情况，原关联工单条码如果没上线，原关联工单打印数减1)
             */
            var snList = unionBarcodes.Select(x => x.OriginalBarcode).Distinct().ToList();

            //生产条码原始信息
            var barcodesOrignal = RT.Service.Resolve<BarcodeController>().GetBarcodesBySns(snList);

            //已关联
            var unionBarcodesOfExists = RT.Service.Resolve<ReworkController>().GetUnionBarcodesBySnList(snList);

            //采集运行进
            var productsDictionary = RuntimeController.FindProduct(snList, BarcodeType.SN);

            //未完成的生产版本
            var wipProductVersions = RT.Service.Resolve<ReworkController>().GetCurrentWipProductVersionsBySnList(snList);

            List<string> completeSnList = (from p in unionBarcodes
                                           let sn = p.OriginalBarcode//没有运行时，已经完成的条码
                                           where !productsDictionary.ContainsKey(sn)
                                           select sn).ToList();
            //获取生产通用报表
            var wipProductVersionsOfComplete = RT.Service.Resolve<ReworkController>().GetWipProductVersionsBySnList(completeSnList);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var barcodes = new EntityList<Barcode>();

                foreach (var p in unionBarcodes)
                {
                    Barcode barcode = ValidationUnionBarcode(barcodesOrignal, unionBarcodesOfExists, p);

                    WorkOrder oldWo = barcode.WorkOrder;

                    ////1.使用原工单条码的时候
                    if (!p.ReworkBarcode.IsNullOrEmpty())
                    {
                        ////1-1.关联条码工单改为返修工单
                        barcode.WorkOrder = workOrder;
                        barcode.WorkOrderId = workOrder.Id;

                        ////1-2.原工单条码运行时记录清除，原工单条码下线(未上线过要减打印数)
                        if (productsDictionary.ContainsKey(barcode.Sn))
                        {
                            var product = productsDictionary[barcode.Sn];
                            RuntimeController.RemoveProduct(product);

                            var version = wipProductVersions.FirstOrDefault(x => x.Sn == barcode.Sn);
                            if (version != null)
                            {
                                version.IsFinish = true;
                                version.FinishDateTime = DateTime.Now;
                                version.NextProcess = null;
                                RF.Save(version);

                                DB.Update<WipProduct>()
                                    .Set(x => x.State, WipProductState.Finish)
                                    .Where(x => x.Id == version.ProductId);
                            }
                        }
                        else
                        {
                            ////完工下线也会找不到运行时记录，此时要区分是否完工下线还是从来没上线                            
                            if (wipProductVersionsOfComplete == null
                                || !wipProductVersionsOfComplete.Any(x => x.Sn == barcode.Sn && x.WorkOrderId == oldWo.Id))
                            {
                                ////使用旧条码，旧条码还没上线，打印数要减一
                                oldWo.PrintedQty--;
                                RF.Save(oldWo);
                            }
                        }

                        ////1-3设置之前关联的记录为未关联
                        DB.Update<UnionBarcode>()
                            .Set(a => a.CodeState, CodeState.NotAssociated)
                            .Where(a => (a.OriginalBarcode == p.OriginalBarcode || a.ReworkBarcode == p.OriginalBarcode)
                                && (a.WorkOrderId == oldWo.Id)
                                && (a.CodeState == CodeState.Associated))
                            .Execute();

                        ////1-4.返修工单已打印数递增（只有使用原工单条码的时候才新增打印数）
                        workOrder.PrintedQty++;
                    }
                    else ////2.使用新条码的时候
                    {
                        ////2-1.旧工单不是返工工单或旧工单使用旧条码，这两种情况都不影响旧工单(数量和关联)
                        if (oldWo.Type != WorkOrderType.Rework || (oldWo.Type == WorkOrderType.Rework && oldWo.UseOldSn))
                        {
                            ////2-1-2.原工单条码运行时记录清除 2-1-3.原工单条码下线
                            ReMoveProduct(productsDictionary, wipProductVersions, barcode);

                            ////此条码关联的工单是使用旧条码的A工单，但条码已经关联到使用新条码的返工工单(还未置换)
                            DB.Update<UnionBarcode>()
                                .Set(a => a.CodeState, CodeState.NotAssociated)
                                .Where(a => (a.OriginalBarcode == p.OriginalBarcode && a.ReworkBarcode == null)
                                    && (a.WorkOrderId != oldWo.Id)
                                    && (a.CodeState == CodeState.Associated))
                                .Execute();
                        }
                        else ////2-2.旧工单使用新条码：2-2-1.旧工单已置换条码(只下线)，2-2-2.旧工单未置换，只移除之前的关联
                        {
                            ////2.原工单条码运行时记录清除
                            ReMoveProduct(productsDictionary, wipProductVersions, barcode);

                            //旧工单未置换，移除旧工单关联条码
                            DB.Update<UnionBarcode>()
                                .Set(a => a.CodeState, CodeState.NotAssociated)
                                .Where(a => (a.OriginalBarcode == p.OriginalBarcode && a.ReworkBarcode == null)
                                    && (a.CodeState == CodeState.Associated))
                                .Execute();
                        }
                    }

                    barcodes.Add(barcode);
                    p.CodeState = CodeState.Associated;
                }

                RF.Save(unionBarcodes);
                RF.Save(barcodes);
                RF.Save(workOrder);

                //推送条码关联消息到边端
                var printBarcodeInfo = new PrintBarcodeInfo();
                printBarcodeInfo.MsgType = "4";
                printBarcodeInfo.WorkOrderNo = workOrder.No;
                printBarcodeInfo.BarcodeList.AddRange(barcodes);
                RT.EventBus.Publish<PrintBarcodeInfo>(printBarcodeInfo);
                tran.Complete();
            }
        }

        /// <summary>
        /// 清除运行时
        /// </summary>
        /// <param name="productsDictionary"></param>
        /// <param name="wipProductVersions"></param>
        /// <param name="barcode"></param>
        private void ReMoveProduct(Dictionary<string, product> productsDictionary, EntityList<WipProductVersion> wipProductVersions, Barcode barcode)
        {
            if (productsDictionary.ContainsKey(barcode.Sn))
            {
                var product = productsDictionary[barcode.Sn];
                RuntimeController.RemoveProduct(product);

                var version = wipProductVersions.FirstOrDefault(x => x.Sn == barcode.Sn);
                if (version != null)
                {
                    version.IsFinish = true;
                    version.FinishDateTime = DateTime.Now;
                    version.NextProcess = null;
                    RF.Save(version);

                    DB.Update<WipProduct>()
                        .Set(x => x.State, WipProductState.Finish)
                        .Where(x => x.Id == version.ProductId);
                }
            }
        }

        private static Barcode ValidationUnionBarcode(EntityList<Barcode> barcodesOrignal, EntityList<UnionBarcode> unionBarcodesOfExists, UnionBarcode p)
        {
            var sn = p.OriginalBarcode;

            var barcode = barcodesOrignal.FirstOrDefault(x => x.Sn == sn);

            if (barcode == null)
            {
                throw new ValidationException("未找到条码：{0}!".L10nFormat(sn));
            }

            if (barcode.IsScraped) //报废条码不能关联
            {
                throw new ValidationException("条码{0}已报废!".L10nFormat(sn));
            }

            if (barcode.IsPending) //挂起条码不能关联
            {
                throw new ValidationException("条码{0}已挂起!".L10nFormat(sn));
            }

            if (unionBarcodesOfExists.Any(x => x.OriginalBarcode == sn))
            {
                throw new ValidationException("条码：{0}已被替换，无法重复关联!".L10nFormat(sn));
            }

            return barcode;
        }

        /// <summary>
        /// 获取查询实体条件下的关联条码数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>关联条码数量</returns>
        public virtual int GetUnionBarcodeCount(double workOrderId)
        {
            return Query<UnionBarcode>().Where(p => p.WorkOrderId == workOrderId && p.CodeState == CodeState.Associated).Count();
        }

        /// <summary>
        /// 查询关联条码
        /// </summary>
        /// <param name="no">工单号</param>
        /// <returns>关联条码列表</returns>
        public virtual EntityList<UnionBarcode> GetUnionBarcodes(string no)
        {
            var query = Query<UnionBarcode>();
            if (!no.IsNullOrWhiteSpace())
                query.Where(p => p.WorkOrder.No.Contains(no) && p.CodeState == CodeState.Associated);
            return query.ToList();
        }

        /// <summary>
        /// 已经被替换的条码
        /// </summary>
        /// <param name="barcodes">原条码列表</param>
        /// <returns>返回已被替换条码列表</returns>
        public virtual EntityList<UnionBarcode> GetUnionBarcodesBySnList(List<string> barcodes)
        {
            return barcodes.SplitContains(tempBarcodes =>
            {
                return Query<UnionBarcode>().Where(p => tempBarcodes.Contains(p.OriginalBarcode)
                    && p.ReworkBarcode != null
                    && p.ReworkBarcode != p.OriginalBarcode
                    && p.CodeState == CodeState.Associated)
                .ToList();
            });
        }


        /// <summary>
        /// 条码是否已经被替换，关联新旧条码，旧条码关联了新条码后，就不能再使用了(不能再重复关联等)
        /// </summary>
        /// <param name="barcode">原条码</param>
        /// <returns>返回条码是否已被替换</returns>
        public virtual bool IsBarcodeReplace(string barcode)
        {
            return Query<UnionBarcode>().Where(p => p.OriginalBarcode == barcode && p.ReworkBarcode != null && p.ReworkBarcode != p.OriginalBarcode && p.CodeState == CodeState.Associated).Count() > 0;
        }

        /// <summary>
        /// 获取返工关联条码列表
        /// </summary>
        /// <param name="criteria">返工关联条码查询实体</param>
        /// <returns>关联条码列表</returns>
        public virtual EntityList GetUnionBarcodeViews(UnionBarcodeViewCriteria criteria)
        {
            var query = DB.Query<UnionBarcodeView>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrderNo.Contains(criteria.WorkOrderNo));
            }

            if (!criteria.InspetNo.IsNullOrEmpty())
            {
                query.Where(p => p.InspetNo.Contains(criteria.InspetNo));
            }

            if (!criteria.Barcode.IsNullOrEmpty())
            {
                query.Where(p => p.Barcode.Contains(criteria.Barcode));
            }

            var result = query.Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            result.SetTotalCount(result.Count);
            return result;
        }

        /// <summary>
        /// 获取返工配置
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>返工配置</returns>
        public virtual bool GetReworkIsUseOld(string workOrderNo)
        {
            return Query<WorkOrder>().Where(p => p.No == workOrderNo).Select(p => p.UseOldSn).FirstOrDefault<bool>();
        }

        /// <summary>
        /// 获取工单的关健件配置
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>关健件解绑配置</returns>
        public virtual EntityList<KeyItemUnboundConfig> GetTaskKeyItemUnboundConfigs(double workOrderId)
        {
            return Query<KeyItemUnboundConfig>().Where(p => p.WorkOrderId == workOrderId).ToList();
        }
        #endregion 返工配置

        /// <summary>
        /// 更新物料标签
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="stageStorageLocation"></param>
        /// <param name="wipKeyUnboundItem"></param>
        public virtual void UpdateItemLabel(double warehouseId, StorageLocation stageStorageLocation, WipProductProcessKeyItem wipKeyUnboundItem)
        {
            DB.Update<ItemLabel>()
                                           .Set(x => x.Qty, x => x.Qty + wipKeyUnboundItem.Qty).
                                            Set(x => x.StorageLocationId, stageStorageLocation.Id)
                                            .Set(x => x.WarehouseId, warehouseId)
                                               .Where(x => x.Id == wipKeyUnboundItem.SourceId)
                                               .Execute();
        }
    }
}