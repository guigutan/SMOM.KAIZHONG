using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.PackingPrints;
using SIE.MES.WIP;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{
    /// <summary>
    /// 包装采集API控制器
    /// </summary>
    public class WinFormPackingApiController : NewWipPackingController
    {
        /// <summary>
        /// 工位工序未完成包装
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <returns>未完成包装关系列表</returns>
        [ApiService("工位工序未完成包装")]
        [return: ApiReturn("未完成包装关系列表")]
        public virtual Tuple<List<PackingRelation>, List<ItemLabel>> FindWorkPackingRelationByStation([ApiParameter("工序ID")] double? processId, [ApiParameter("工位ID")] double? stationId)
        {
            var relationList = RT.Service.Resolve<PackingRelationController>().FindWorkPackingRelationByStation(processId, stationId);
            var relationIds = relationList.Select(p => p.Id).ToList();
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(relationIds);
            return new Tuple<List<PackingRelation>, List<ItemLabel>>(relationList.ToList(), labels.ToList());
        }

        /// <summary>
        /// 包装采集-校验条码信息
        /// </summary>
        /// <param name="collectBarcode">扫描条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="woId">当前工单Id</param>
        /// <returns>条码信息</returns>
        [ApiService("包装采集-校验条码信息")]
        [return: ApiReturn("条码信息")]
        public virtual ValidateResult PackingValidate([ApiParameter("扫描条码")] CollectBarcode collectBarcode, [ApiParameter("工作单元")] Workcell workcell, [ApiParameter("当前工单Id")] double woId)
        {
            var ct = RT.Service.Resolve<WinFormMoveApiController>();
            ApiModels.WorkOrderInfo resultWo = null;
            int? reportModel = -1;
            var product = this.Validate(collectBarcode, workcell);
            if (product.WorkOrderId != 0)
            {
                var workOrder = RF.GetById<WorkOrder>(product.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null && product.WorkOrderId != woId)
                {
                    resultWo = new ApiModels.WorkOrderInfo(workOrder);
                    this.ChangeWipResourceWorkOrder(workOrder.Id, workcell);
                    RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                    reportModel = ct.UpdateWorkOrdeReportModel(product.WorkOrderId);
                }
            }
            ct.ValidateTaskReport(product.WorkOrderId, workcell, reportModel);
            var result = new ValidateResult()
            {
                ProductInfo = product,
                WorkOrderInfo = resultWo,
                Context = product.Context
            };
            return result;
        }

        /// <summary>
        /// 包装采集过站
        /// </summary>
        /// <param name="barcode">采集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="needMove">是否需要过站</param>
        /// <param name="scanMode">扫描模式</param>
        /// <returns>最外层的包装（关系）</returns>
        [ApiService("包装采集-过站")]
        [return: ApiReturn("最外层的包装")]
        public virtual Tuple<PackingRelation, Packages.Packings.NewPackingStrategyEvent> PackingCollectApi(
            [ApiParameter("采集的条码")] string barcode, [ApiParameter("采集数据")] CollectData collectData,
            [ApiParameter("工作单元")] Workcell workcell, [ApiParameter("是否需要过站")] bool needMove,
            [ApiParameter("扫描模式")] ScanMode scanMode)
        {
            var packingRelation = PackingCollect(barcode, collectData, workcell, needMove);
            //采集成功
            var unit = collectData.PackingData.CurrentPackingUnit;
            var workOrder = RF.GetById<WorkOrder>(packingRelation.WorkOrderId);
            var packingEvent = CreatePackingEvent("MesPacking", scanMode, barcode, packingRelation, workOrder, unit);
            RT.EventBus.Publish(packingEvent);
            if (packingEvent.IsPackageQtyFull || packingEvent.IsPackageItemFull)
            {
                RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.DoPacking,
                    "MesPacking", packingEvent.OuterPackingRelation));
            }
            return new Tuple<PackingRelation, Packages.Packings.NewPackingStrategyEvent>(packingRelation, packingEvent);
        }

        /// <summary>
        /// 验证包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="packingRelation">包装关系</param>
        [ApiService("包装采集-验证包装号")]
        public virtual void ValidatePackingBarcode([ApiParameter("包装号")] string packageNo,
            [ApiParameter("包装单位")] PackingUnit packingUnit, [ApiParameter("包装关系")] PackingRelation packingRelation)
        {
            RT.Service.Resolve<PackingBarcodeController>().ValidatePackingBarcode(packageNo, packingUnit, packingRelation);
        }

        /// <summary>
        /// 包装采集-加入
        /// </summary>
        /// <param name="barcode">采集的条码</param>
        /// <returns>包装关系</returns>
        [ApiService("包装采集-加入")]
        [return: ApiReturn("包装关系")]
        public virtual PackingRelation JoinPacking([ApiParameter("采集的条码")] string barcode)
        {
            var curRelation = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(barcode, false);
            if (curRelation == null)
            {
                var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
                if (itemLabel?.Relation != null)
                {
                    curRelation = itemLabel.Relation;
                }
            }
            if (curRelation == null)
            {
                throw new ValidationException("系统无此条码[{0}]包装记录".L10nFormat(barcode));
            }
            // 获取最外层的外包装关系
            PackingRelation _outPackRelation = null;
            if (curRelation.RootId > 0)
            {
                _outPackRelation = RF.GetById<PackingRelation>(curRelation.RootId, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                //这里为兼容原来没有写入 RootId 的包装关系，递归查找最外层的包装关系
                int count = 0;
                // 循环上钻获取最外层关系
                while (curRelation.GetTreePId() != null)
                {
                    if (curRelation.GetTreePId() != null)
                    {
                        curRelation = RF.GetById<PackingRelation>(curRelation.TreePId, new EagerLoadOptions().LoadWithViewProperty());
                    }
                    count++;
                    if (count > 20)
                    {
                        throw new ValidationException("此条码[{0}]包装关系异常，请联系管理员处理。".L10nFormat(barcode));
                    }
                }
                _outPackRelation = curRelation;
            }
            if (_outPackRelation == null)
            {
                throw new ValidationException("系统无此条码[{0}]包装记录".L10nFormat(barcode));
            }
            if (_outPackRelation.IsPacked)
            {
                throw new ValidationException("加入包装失败，{0}[{1}]已打包完成，无法加入操作。".L10nFormat(curRelation.PackageUnitName, barcode));
            }
            return _outPackRelation;
        }

        /// <summary>
        /// 包装采集-验证打包条件
        /// </summary>
        /// <param name="relations">包装关系</param>
        /// <returns>外包装最大包装数</returns>
        [ApiService("包装采集-验证打包条件")]
        [return: ApiReturn("外包装最大包装数")]
        public virtual decimal? ValidatePackingRelations([ApiParameter("包装关系")] List<PackingRelation> relations)
        {
            if (relations.Count <= 0)
                throw new ValidationException("请选择包装".L10N());
            var relation = relations.FirstOrDefault();
            if (relations.Count == 1)
            {
                if (relation.PackageNo.IsNullOrEmpty())
                {
                    var workOrder = RF.GetById<WorkOrder>(relation.WorkOrderId);
                    return workOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == relation.PackageUnitId)?.LevelQty;
                }
                else
                    return ValidateLevelQty(relations.Count, relation);
            }
            else
            {
                if (relations.Any(p => p.PackageNo.IsNullOrEmpty()) && !relations.Any(p => p.PackageUnitId != relation.PackageUnitId) && !relations.Any(p => p.WorkOrderId != relation.WorkOrderId))
                    throw new ValidationException("打包失败，多个包装打包必须都是同一包装层级的已打包包装且工单一致".L10N());
                return ValidateLevelQty(relations.Count, relation);
            }
        }

        /// <summary>
        /// 验证包装层级数量
        /// </summary>
        /// <param name="count">待打包包装数</param>
        /// <param name="relation">包装关系</param>
        /// <returns>包装层级数量</returns>
        private decimal? ValidateLevelQty(int count, PackingRelation relation)
        {
            var workOrder = RF.GetById<WorkOrder>(relation.WorkOrderId);
            var levelQty = RT.Service.Resolve<WipPackingController>().GetOutPackingLevelQty(relation.PackageUnit, workOrder);
            if (count > levelQty)
                throw new ValidationException("打包失败，外包装最大包装数为[{0}]，当前包装数为[{1}]".L10nFormat(levelQty, count));
            return levelQty;
        }

        /// <summary>
        /// 单层级打包
        /// </summary>
        /// <param name="currentPkg">外部包装关系</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isCasecade">是否级联打包</param>
        /// <returns>打包后的最顶包装关系</returns>
        [ApiService("包装采集-单层级打包")]
        [return: ApiReturn("打包后的最顶包装关系")]
        public virtual List<PackingRelation> DoPacking([ApiParameter("包装关系")] PackingRelation currentPkg,
            [ApiParameter("工作单元")] Workcell workcell, [ApiParameter("是否级联打包")] bool isCasecade)
        {
            var listPack = RT.Service.Resolve<WipPackingController>().DoPacking(currentPkg, workcell, isCasecade);
            return listPack.ToList();
        }

        /// <summary>
        /// 多层级打包
        /// </summary>
        /// <param name="relations">包装关系列表</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isCasecade">是否级联打包</param>
        /// <returns>上一包装层级</returns>
        [ApiService("包装采集-多层级打包")]
        [return: ApiReturn("上一包装层级")]
        public virtual List<PackingRelation> DoMultLevelPacking([ApiParameter("包装关系列表")] List<PackingRelation> relations,
            [ApiParameter("工作单元")] Workcell workcell, [ApiParameter("是否级联打包")] bool isCasecade)
        {
            var entitys = new EntityList<PackingRelation>();
            entitys.AddRange(relations);
            var listPack = RT.Service.Resolve<WipPackingController>().DoMultLevelPacking(entitys, workcell, isCasecade);
            return listPack.ToList();
        }

        /// <summary>
        /// 打包后事件
        /// </summary>
        /// <param name="relations">包装关系列表</param>   
        [ApiService("包装采集-打包后事件")]
        public virtual void OnDoListPacked([ApiParameter("包装关系列表")] List<PackingRelation> relations)
        {
            RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.Packed, "MesPacking", relations.ToArray()));
        }
    }
}
