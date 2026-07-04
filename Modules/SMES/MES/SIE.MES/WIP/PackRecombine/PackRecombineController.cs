using SIE.Barcodes;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Inspection;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.PackRecombine.Logs;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using System;
using System.Linq;

namespace SIE.MES.WIP.PackRecombine
{
    /// <summary>
    /// 包装拆合控制器
    /// </summary>
    public class PackRecombineController : WipPackingController
    {
        /// <summary>
        /// 包装移除
        /// </summary>
        /// <param name="barcode">移除条码</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <returns>工单ID</returns>
        public virtual RecombineInfo SplitPackingRelation(string barcode, bool isBatch)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (barcode.IsNullOrEmpty())
                    throw new ValidationException("包装号不能为空".L10N());
                var info = new RecombineInfo();
                var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, isBatch);
                WorkOrder wo;
                if (relation == null)
                {
                    //包装号为空，移除的是包装关系单个产品      
                    var itemLabel = ValidationBarcode(barcode);
                    if (itemLabel.Relation == null)
                        throw new ValidationException("移除失败,物料标签未包装或者已移除".L10N());
                    var masterUnit = RT.Service.Resolve<PackageController>().GetMasterUnit();
                    info.OldPackingNo = itemLabel.Relation.PackageNo;
                    info.OldPackingUnitId = itemLabel.Relation.PackageUnitId;
                    info.PackingUnit = "主单位";
                    info.PackingUnitId= masterUnit.Id;
                    wo = GetRelationWorkOrder(itemLabel.Relation.WorkOrderId);
                    PackageMoveOut(itemLabel.Relation, true);
                    itemLabel.RelationId = null;
                    RF.Save(itemLabel);
                    RT.Service.Resolve<IToStorageBarcode>().DeleteToStoreDetailByCode(barcode);
                }
                else
                {
                    ValidationPackBarcode(barcode, isBatch);
                    wo = GetRelationWorkOrder(relation.WorkOrderId);
                    CheckIsOutestPack(relation, wo);
                    RT.Service.Resolve<IToStorageBarcode>().MoveStorageDetailByPackcode(relation.Id, isBatch);
                    var parentRelation = RF.GetById<PackingRelation>(relation.TreePId);
                    if (parentRelation == null)
                        throw new ValidationException("包装[{0}]未找到外层包装".L10nFormat(relation.PackageNo));
                    info.OldPackingNo = PackageMoveOut(relation, false);                    
                    info.OldPackingUnitId = parentRelation.PackageUnitId;
                    info.PackingUnit = relation.PackageUnit?.Name;
                    info.PackingUnitId = relation.PackageUnitId;
                    info.RelationId = relation.Id;
                }

                info.WorkOrderId = wo.Id;

                SaveRecombineLog(ScanMode.Move, isBatch, barcode, info.PackingUnitId, info.OldPackingNo, info.OldPackingUnitId);
                tran.Complete();
                return info;
            }
        }

        /// <summary>
        /// 获取包装关系工单信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>工单</returns>
        /// <exception cref="ValidationException">未找到包装工单信息</exception>
        WorkOrder GetRelationWorkOrder(double workOrderId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            if (wo == null)
                throw new ValidationException("未找到包装工单信息".L10N());
            return wo;
        }

        /// <summary>
        /// 验证是否最外层包装
        /// </summary>
        /// <param name="relation">包装关系</param>
        /// <param name="wo">工单</param>     
        /// <exception cref="ValidationException">工单包装规则异常；当前包装是最外层包装</exception>  
        void CheckIsOutestPack(PackingRelation relation, WorkOrder wo)
        {
            if (relation.TreePId == null || relation.TreePId == 0)
                throw new ValidationException("当前包装是最外层包装,不能移除".L10N());
            var packRuleDetial = wo.PackageRuleDetailList.Where(f => !f.PackageUnit.IsMasterUnit)
                .OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (packRuleDetial == null)
                throw new ValidationException("工单包装规则异常".L10N());
            if (packRuleDetial.PackageUnitId == relation.PackageUnitId)
                throw new ValidationException("当前包装是最外层包装,不能移除".L10N());
        }

        /// <summary>
        /// 递归移除包装
        /// </summary>
        /// <param name="relation">包装关系</param>
        /// <param name="info">包装关系</param>
        /// <param name="isSingle">是否单个产品条码</param>
        /// <returns>包装号</returns>
        string PackageMoveOut(PackingRelation relation, bool isSingle)
        {
            string parentNo = string.Empty;
            var moveQty = relation.ItemQty;
            PackingRelation parentRelation = RF.GetById<PackingRelation>(relation.TreePId);
            if (isSingle)
            {
                moveQty = 1;
                relation.PackedQty--;
                relation.ItemQty--;
                parentNo = relation.PackageNo;
            }
            else
            {
                if (parentRelation == null)
                    throw new ValidationException("包装[{0}]未找到外层包装".L10nFormat(relation.PackageNo));
                parentNo = parentRelation.PackageNo;
                parentRelation.PackedQty--;
                ////当前包装父至空 
                relation.TreePId = null;
                relation.ParentNo = string.Empty;
            }

            UpdateParentRelationItemQty(parentRelation, -moveQty);
            RF.Save(relation);
            return parentNo;
        }

        /// <summary>
        /// 加入包装第一次扫描容器包装条码
        /// </summary>
        /// <param name="parBarcode">容器包装条码</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <returns>容器包装的包装关系</returns>
        public virtual BatchPackingRelation JoinPackingRelationScanParent(string parBarcode, bool isBatch)
        {
            if (parBarcode.IsNullOrEmpty())
                throw new ValidationException("包装号不能为空".L10N());
            var ctl = RT.Service.Resolve<PackingRelationController>();
            var relation = ctl.GetBatchPackingRelation(parBarcode);
            ValidationPackBarcode(parBarcode, isBatch);
            var lessQty = GetLessItemQtyByPackNo(relation);
            if (lessQty == 0)
            {
                throw new ValidationException("识别失败，包装数已满".L10N());
            }

            return relation;
        }

        /// <summary>
        /// 加入包装第二次扫描加入的条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="parRelation">上层包装</param>
        /// <param name="isBatch">是否批次包装</param>
        /// <returns>包装拆合信息</returns>
        public virtual RecombineInfo JoinPackingRelationScanSon(string barcode, BatchPackingRelation parRelation, bool isBatch)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (barcode.IsNullOrEmpty())
                    throw new ValidationException("包装号不能为空".L10N());
                var info = new RecombineInfo();
                var wo = RF.GetById<WorkOrder>(parRelation.WorkOrderId);
                var detail = RT.Service.Resolve<WipPackingController>().GetInnerPackageRuleDetail(parRelation.PackageUnit, wo);
                var innerPackageUnit = detail.PackageUnit;
                var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, false);
                if (relation == null)
                {
                    if (!innerPackageUnit.IsMasterUnit)
                        throw new ValidationException("加入失败,请扫描[{0}]条码".L10nFormat(innerPackageUnit.Name));
                    ////获取ItemLabel更改Relation为当前parRelation
                    var itemLabel = ValidationBarcode(barcode);
                    ValidateSameProduct(itemLabel.WorkOrderId.Value, parRelation.WorkOrderId);
                    ////未移除包装的包装先移除
                    if (itemLabel.Relation != null)
                    {
                        if (itemLabel.RelationId == parRelation.Id)
                        {
                            throw new ValidationException("产品[{0}]已存在于当前包装[{1}]，无需加入".L10nFormat(itemLabel.Label, parRelation.PackageNo));
                        }

                        info.IsRemove = true;
                        info.OldPackingNo = itemLabel.Relation.PackageNo;
                        info.OldPackingUnitId = itemLabel.Relation.PackageUnitId;
                        PackageMoveOut(itemLabel.Relation, true);
                    }
                    ////加入新包装
                    var newItemLabel = UpdateParentRelation(itemLabel, parRelation);
                    info.NewPackingNo = parRelation.PackageNo;
                    info.NewPackingUnitId = parRelation.PackageUnitId;
                    RT.Service.Resolve<IToStorageBarcode>().JoinStorageDetailByPackcode((double)newItemLabel.RelationId, isBatch, barcode);
                }
                else
                {
                    ////获取任意子的包装单位，比对加入的包装单位 
                    if (relation.PackageUnitId != innerPackageUnit.Id)
                        throw new ValidationException("加入失败,请扫描[{0}]条码".L10nFormat(innerPackageUnit.Name));
                    if (isBatch && parRelation.BatchNo != relation.BatchNo)
                        throw new ValidationException("加入失败，不同工单批次不能加入".L10N());
                    ValidateSameProduct(relation.WorkOrderId, parRelation.WorkOrderId);
                    ValidationPackBarcode(barcode, isBatch);
                    ////移除包装
                    if (relation.TreePId != null)
                    {
                        if (relation.TreePId == parRelation.Id)
                        {
                            throw new ValidationException("包装[{0}]已存在于当前父包装[{1}]，无需加入".L10nFormat(relation.PackageNo, parRelation.PackageNo));
                        }

                        info.IsRemove = true;
                        var parentRelation = RF.GetById<PackingRelation>(relation.TreePId);
                        if (parentRelation == null)
                            throw new ValidationException("包装[{0}]未找到外层包装".L10nFormat(relation.PackageNo));
                        info.OldPackingNo = PackageMoveOut(relation, false);
                        info.OldPackingUnitId = parentRelation.PackageUnitId;
                    }

                    ValidatePackingQty(parRelation, relation);
                    ////加入新包装
                    UpdateParentRelation(relation, parRelation);
                    RT.Service.Resolve<IToStorageBarcode>().JoinStorageDetailByPackcode(relation.Id, isBatch, string.Empty);
                    info.NewPackingNo = parRelation.PackageNo;
                    info.NewPackingUnitId = parRelation.PackageUnitId;
                }

                info.PackingUnit = innerPackageUnit.Name;
                info.PackingUnitId = innerPackageUnit.Id;
                info.IsFullPack = IsPackingRelationPackageQtyFull(RF.GetById<PackingRelation>(parRelation.Id), RF.GetById<WorkOrder>(parRelation.WorkOrderId));

                CreateRecombineLog(barcode, info, isBatch);
                tran.Complete();
                return info;
            }
        }

        /// <summary>
        /// 创建加入/移除包装记录日志
        /// </summary>
        /// <param name="barcode">包装号</param>
        /// <param name="info">包装拆合信息</param>
        /// <param name="isBatch">是否批次</param>
        private void CreateRecombineLog(string barcode, RecombineInfo info, bool isBatch)
        {
            if (info.IsRemove)
                SaveRecombineLog(ScanMode.Move, isBatch, barcode, info.PackingUnitId, info.OldPackingNo, info.OldPackingUnitId);
            SaveRecombineLog(ScanMode.Join, isBatch, barcode, info.PackingUnitId, info.NewPackingNo, info.NewPackingUnitId);
        }

        /// <summary>
        /// 保存加入/移除包装记录日志
        /// </summary>
        /// <param name="mode">操作类型</param>
        /// <param name="isBatch">是否批次</param>
        /// <param name="barcode">包装号</param>
        /// <param name="packingUnitId">包装单位Id</param>
        /// <param name="outBarcode">外层包装号</param>
        /// <param name="parentUnitId">外层包装单位Id</param>
        private void SaveRecombineLog(ScanMode mode, bool isBatch, string barcode, double packingUnitId, string outBarcode, double parentUnitId)
        {
            var log = new RecombineLog()
            {
                PackageNo = barcode,
                ParentNo = outBarcode,
                ScanMode = mode,
                IsBatch = isBatch,
                PackageUnitId = packingUnitId,
                ParentUnitId = parentUnitId,
                PersistenceStatus = PersistenceStatus.New
            };

            RF.Save(log);
        }

        /// <summary>
        /// 验证加入包装剩余包装数
        /// </summary>
        /// <param name="parRelation">外层包装关系</param> 
        /// <param name="relation">内层包装关系</param>
        private void ValidatePackingQty(BatchPackingRelation parRelation, BatchPackingRelation relation)
        {
            var remainQty = GetLessItemQtyByPackNo(parRelation);
            if (remainQty <= 0)
                throw new ValidationException("加入失败，当前[{0}]数量为{1}大于外包装[{2}]可加入数量[{3}]".L10nFormat(relation.PackageUnit?.Name, relation.PackedQty, parRelation.PackageUnit?.Name, remainQty));
        }

        /// <summary>
        /// 验证加入产品与外包装是否同一工单
        /// </summary>
        /// <param name="workOrderId">待加入产品工单ID</param>
        /// <param name="parentWorkOrderId">外包装工单ID</param>
        private void ValidateSameProduct(double workOrderId, double parentWorkOrderId)
        {
            if (workOrderId != parentWorkOrderId)
                throw new ValidationException("不同工单产品不允许混合打包".L10N());
        }

        /// <summary>
        /// 更新父包装关系
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="parRelation">父包装</param>
        /// <returns>已保存的物料标签</returns>
        private ItemLabel UpdateParentRelation(ItemLabel itemLabel, BatchPackingRelation parRelation)
        {
            UpdateParentRelationItemQty(parRelation.Id, itemLabel.Qty);
            itemLabel.RelationId = parRelation.Id;
            RF.Save(itemLabel);
            return itemLabel;
        }

        /// <summary>
        /// 更新父包装关系
        /// </summary>
        /// <param name="relation">待加入包装</param>
        /// <param name="parRelation">父包装</param>
        private void UpdateParentRelation(BatchPackingRelation relation, BatchPackingRelation parRelation)
        {
            UpdateParentRelationItemQty(parRelation.Id, relation.ItemQty);
            relation.ParentNo = parRelation.PackageNo;
            relation.TreePId = parRelation.Id;
            RF.Save(relation);
        }

        /// <summary>
        /// 更新父包装物料数量
        /// </summary>
        /// <param name="relationId">父包装关系ID</param>
        /// <param name="itemQty">物料数量</param>
        private void UpdateParentRelationItemQty(double relationId, decimal itemQty)
        {
            var parent = RF.GetById<PackingRelation>(relationId);
            parent.PackedQty++;
            UpdateParentRelationItemQty(parent, itemQty);
        }

        /// <summary>
        /// 更新父包装物料数量
        /// </summary>
        /// <param name="relation">父包装关系</param>
        /// <param name="itemQty">物料数量</param>
        void UpdateParentRelationItemQty(PackingRelation relation, decimal itemQty)
        {
            if (relation == null)
                return;
            DB.Update<PackingRelation>()
                 .Set(p => p.ItemQty, p => p.ItemQty + itemQty)
                 .Set(p => p.PackedQty, relation.PackedQty)
                 .Where(p => p.Id == relation.Id)
                 .Execute();
            UpdateParentRelationItemQty(RF.GetById<PackingRelation>(relation.TreePId), itemQty);
        }

        /// <summary>
        /// 验证单体产品条码
        /// </summary>
        /// <param name="barcode">单体产品条码</param>
        /// <returns>物料标签</returns>
        private ItemLabel ValidationBarcode(string barcode)
        {
            if (RT.Service.Resolve<IToStorageBarcode>().IsExistsStorageDetailByBarcode(barcode))
                throw new ValidationException("条码存在已入库数据，不允许包装调整".L10N());
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
            if (itemLabel == null)
                throw new ValidationException("没有该条码的物料标签".L10N());
            return itemLabel;
        }

        /// <summary>
        /// 验证包装条码是否入库
        /// </summary>
        /// <param name="packBarcode">包装条码</param>
        /// <param name="isBatch">是否批次</param>
        private void ValidationPackBarcode(string packBarcode, bool isBatch)
        {
            if (RT.Service.Resolve<IToStorageBarcode>().IsExistsPakStorageDetail(packBarcode, isBatch))
                throw new ValidationException("条码存在已入库数据，不允许包装调整".L10N());
        }

        /// <summary>
        /// 返回当前包装可加入数量
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <returns>可加入数量</returns>
        decimal GetLessItemQtyByPackNo(BatchPackingRelation packingRelation)
        {
            var detail = RT.Service.Resolve<WipPackingController>().GetPackageRuleDetail(packingRelation.PackageUnit, RF.GetById<WorkOrder>(packingRelation.WorkOrderId));
            var lessQty = detail.LevelQty - packingRelation.PackedQty;
            if (lessQty < 0) lessQty = 0;
            return lessQty;
        }

        /// <summary>
        /// 查询包装条码
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <param name="isBatch">批次</param>
        /// <returns>包装拆合信息</returns>
        public virtual RecombineInfo SearchPackingRelation(string barcode, bool isBatch)
        {
            var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, true);
            var info = new RecombineInfo();
            info.WorkOrderId = relation.WorkOrderId;
            info.RelationId = relation.Id;
            info.PackingUnit = relation.PackageUnit.Name;
            if (relation.TreePId != null || relation.TreePId > 0)
                info.OldPackingNo = RF.GetById<BatchPackingRelation>(relation.TreePId)?.PackageNo;
            return info;
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="packRelationId">包装Id</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabels(double packRelationId)
        {
            var batchRelation = RF.GetById<BatchPackingRelation>(packRelationId);
            EntityList<BatchPackingRelation> sonEntityList = new EntityList<BatchPackingRelation>();
            sonEntityList.Add(batchRelation);
            sonEntityList = RT.Service.Resolve<PackageController>().GetStoreRelationByUnitId(sonEntityList);
            var packRelationIds = sonEntityList.Select(p => p.Id).ToList();
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(packRelationIds);
            return labels;
        }

        /// <summary>
        /// 根据SN获取最外层包装信息
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>包装拆合信息</returns>
        public virtual RecombineInfo SearchPackingRelationBySn(string barcode)
        {
            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode);
            if (sn == null)
                throw new ValidationException("条码[{0}]不存在".L10nFormat(barcode));
            var label = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
            if (label == null)
                throw new ValidationException("条码[{0}]的物料标签不存在".L10nFormat(barcode));
            if (!label.RelationId.HasValue)
                throw new ValidationException("条码[{0}]的物料标签[{1}]包装关系未维护".L10nFormat(barcode, label.Label));
            var relation = RF.GetById<PackingRelation>(label.RelationId.Value);
            if (relation == null)
                throw new ValidationException("条码[{0}]的物料标签[{1}]包装关系未维护".L10nFormat(barcode, label.Label));
            var rootRelation = RT.Service.Resolve<PackingRelationController>().GetRootPackingRelation(relation.RootId);
            var masterUnit = RT.Service.Resolve<PackageController>().GetMasterUnit();
            var info = new RecombineInfo();
            info.WorkOrderId = rootRelation.WorkOrderId;
            info.RelationId = rootRelation.Id;
            info.PackingUnit = masterUnit.Name;
            info.OldPackingNo = relation.PackageNo;
            return info;
        }
    }
}