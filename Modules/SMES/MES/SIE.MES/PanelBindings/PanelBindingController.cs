using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.PanelBindings;
using SIE.EventMessages.MES.PanelBindings.Models;
using SIE.EventMessages.MES.Panels;
using SIE.MES.PanelBindings.Models;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using Barcode = SIE.Barcodes.Barcode;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// 拼板码绑定控制器
    /// </summary>
    public partial class PanelBindingController : WipController
    {
        /// <summary>
        /// 验证扫描的拼板码
        /// </summary>
        /// <param name="barcode">拼板码编码</param>
        /// <returns>拼板码</returns>
        public virtual Panel GetPanel(string barcode)
        {
            var panel = RT.Service.Resolve<PanelController>().GetPanel(barcode);
            if (panel == null)
            {
                throw new ValidationException("采集失败！该拼板码[{0}]不存在".L10nFormat(barcode));
            }

            if (panel.IsScrap)
            {
                throw new ValidationException("采集失败！拼板码[{0}]处于报废状态".L10nFormat(barcode));
            }

            //验证工单
            var workOrder = RF.GetById<WorkOrder>(panel.WorkOrderId);

            if (workOrder.IsPause == YesNo.Yes
                || workOrder.State == Core.WorkOrders.WorkOrderState.Close
                || workOrder.State == Core.WorkOrders.WorkOrderState.Finish)
            {
                throw new ValidationException("采集失败！工单已暂停/已关闭/已完工".L10N());
            }

            if (workOrder.PanelQty <= 0)
            {
                throw new ValidationException("采集失败！当前工单的拼板数不能小于0".L10N());
            }

            return panel;
        }

        /// <summary>
        /// 获取未绑定的条码
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns></returns>
        public virtual EntityList<Barcode> GetUnBindingRecords(double workOrderId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var workOrder = RF.GetById<WorkOrder>(workOrderId);

            if (workOrder == null)
            {
                return new EntityList<Barcode>();
            }

            if (workOrder.IsPanelWorkOrder)
            {
                //组合板工单取子工单(工单的组合板工单ID等于查询的工单ID)的生产条码
                var query = Query<Barcode>()
                    .Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id)
                    .Where<WorkOrder>((x, y) => y.PanelWorkOrderId == workOrderId)
                    .NotExists<PanelAndBarcode>((x, y) => y.Where(b => b.SN == x.Sn));
                return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                //非组合板工单取工单的生产条码
                var query = Query<Barcode>()
                    .Where(x => x.WorkOrderId == workOrderId)
                    .NotExists<PanelAndBarcode>((x, y) => y.Where(b => b.SN == x.Sn));
                return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 验证扫描的拼板码
        /// </summary>
        /// <param name="barcode">拼板码编码</param>
        /// <returns>拼板码</returns>
        public virtual Panel ValidatePanel(string barcode)
        {
            var panel = RT.Service.Resolve<PanelController>().GetPanel(barcode);
            if (panel == null)
                throw new ValidationException("采集失败！该拼板码[{0}]不存在".L10nFormat(barcode));
            if (panel.IsScrap)
                throw new ValidationException("采集失败！拼板码[{0}]处于[报废]状态".L10nFormat(barcode));

            if (panel.IsPending == true)
            {
                throw new ValidationException("采集失败！拼板码[{0}]处于[暂停]状态".L10nFormat(barcode));
            }

            //if (RT.Service.Resolve<PanelBindingController>().PanelIsBinding(panel.Id))
            //    throw new ValidationException("采集失败！该拼板码已完成绑定".L10N());

            //验证工单
            ValidateElecWorkOrder(panel.WorkOrderId);
            return panel;
        }

        /// <summary>
        /// 验证采集的工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        protected virtual void ValidateElecWorkOrder(double workOrderId)
        {
            var elecWorkOrder = RF.GetById<WorkOrder>(workOrderId);
            if (elecWorkOrder.IsPause == YesNo.Yes || elecWorkOrder.State == Core.WorkOrders.WorkOrderState.Close
                || elecWorkOrder.State == Core.WorkOrders.WorkOrderState.Finish)
                throw new ValidationException("采集失败！工单已暂停/已关闭/已完工".L10N());
            var bindingSnQty = RT.Service.Resolve<PanelBindingController>().GetWorkOrderPanelBindingSnQty(elecWorkOrder.Id);

            var needBindingQty = elecWorkOrder.PlanQty;
            if (elecWorkOrder.IsPanelWorkOrder)
            {
                var childWoTotalPlanQty = Query<WorkOrder>()
                    .Select(x => x.PlanQty.SUM())
                    .Where(x => x.PanelWorkOrderId == workOrderId)
                    .FirstOrDefault<decimal>();

                needBindingQty = childWoTotalPlanQty;
            }

            if (needBindingQty - bindingSnQty <= 0)
                throw new ValidationException("采集失败！当前工单未绑定SN条码数小于0".L10N());

            if (elecWorkOrder.PanelQty <= 0)
                throw new ValidationException("采集失败！当前工单的拼板数不能小于0".L10N());
        }

        /// <summary>
        /// 验证扫描的条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrder">工单</param>
        /// <param name="DicSnItemList">工单</param>
        public virtual Dictionary<string, double> ValidatePanelSn(string barcode, WorkOrder workOrder, Dictionary<string, double> DicSnItemList)
        {
            var sn = ValidateSn(barcode);
            if (!workOrder.IsPanelWorkOrder && !workOrder.PanelWorkOrderId.HasValue && sn.WorkOrderId != workOrder.Id)
                throw new ValidationException("采集失败！该SN条码与当前工单不匹配".L10N());

            var workOrderOfSn = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(sn.WorkOrderId.Value);

            if (workOrder.IsPanelWorkOrder || workOrder.PanelWorkOrderId.HasValue)
            {
                if (workOrder.IsPanelWorkOrder && workOrderOfSn.PanelWorkOrderId != workOrder.Id)
                    throw new ValidationException("采集失败！生产条码[{0}]对应组合板工单号，与当前工单号不匹配".L10nFormat(sn));

                if (!workOrder.IsPanelWorkOrder && workOrder.PanelWorkOrderId.HasValue
                    && workOrderOfSn.PanelWorkOrderId != workOrder.PanelWorkOrderId)
                {
                    throw new ValidationException("采集失败！生产条码[{0}]对应组合板工单号，与当前工单号不匹配".L10nFormat(sn));
                }

                if (workOrder.IsPanelWorkOrder)
                {
                    var pcbItemDetails = RT.Service.Resolve<IPanelBinding>().GetPcbItemDetailInfos(workOrder.ProductId);
                    var dicItemDetails = pcbItemDetails.ToDictionary(p => p.ItemId);

                    if (!dicItemDetails.TryGetValue(workOrderOfSn.ProductId, out PcbItemDetailInfo info))
                    {
                        throw new ValidationException("采集失败！生产条码[{0}]对应产品不属于[{1}]的子产品！"
                            .L10nFormat(sn, workOrder.ProductCode));
                    }

                    if (DicSnItemList.Values.Count(p => p == workOrderOfSn.ProductId) + 1 > info.Qty * workOrder.PanelQty)
                    {
                        throw new ValidationException("采集失败！子产品[{0}]扫描条码数已满！"
                            .L10nFormat(workOrderOfSn.ProductCode));
                    }
                }
                else
                {
                    var panelWo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(workOrder.PanelWorkOrderId.Value);
                    var pcbItemDetails = RT.Service.Resolve<IPanelBinding>().GetPcbItemDetailInfos(panelWo.ProductId);
                    var dicItemDetails = pcbItemDetails.ToDictionary(p => p.ItemId);

                    if (!dicItemDetails.TryGetValue(workOrderOfSn.ProductId, out PcbItemDetailInfo info))
                    {
                        throw new ValidationException("采集失败！生产条码[{0}]对应产品不属于[{1}]的子产品！"
                            .L10nFormat(sn, panelWo.ProductCode));
                    }

                    if (DicSnItemList.Values.Count(p => p == workOrderOfSn.ProductId) + 1 > info.Qty * workOrder.PanelQty)
                    {
                        throw new ValidationException("采集失败！子产品[{0}]扫描条码数已满！"
                            .L10nFormat(workOrderOfSn.ProductCode));
                    }
                }
            }

            DicSnItemList.Add(barcode, workOrderOfSn.ProductId);

            return DicSnItemList;
        }

        /// <summary>
        /// 验证扫描的条码
        /// </summary>
        /// <param name="barcode">扫描的条码</param>
        /// <returns>条码</returns>
        private Barcode ValidateSn(string barcode)
        {
            var barcodeEntity = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode);

            if (barcodeEntity == null)
            {
                throw new ValidationException("采集失败！条码[{0}]不存在".L10nFormat(barcode));
            }

            if (barcodeEntity.IsScraped)
            {
                throw new ValidationException("采集失败！条码[{0}]处于报废状态".L10nFormat(barcode));
            }

            var panelAndBarcode = GetPanelAndBarcodeRecord(barcode);
            if (panelAndBarcode != null)
            {
                if (!panelAndBarcode.IsBinding)
                {
                    throw new ValidationException("采集失败！条码[{0}]已经分板".L10nFormat(barcode));
                }

                if (panelAndBarcode.IsBinding)
                {
                    throw new ValidationException("采集失败！条码[{0}]已完成绑定".L10nFormat(barcode));
                }
            }

            var product = RT.Service.Resolve<RuntimeController>().FindProduct(barcode, BarcodeType.SN);
            if (product != null)
            {
                throw new ValidationException("采集失败！条码[{0}]已上线".L10nFormat(barcode));
            }

            if (RT.Service.Resolve<WipProductVersionController>().IsSnDownline(barcode) == true)
            {
                throw new ValidationException("条码[{0}]已完工，不支持绑定".L10nFormat(barcode));
            }

            if (!barcodeEntity.WorkOrderId.HasValue)
            {
                throw new ValidationException("生产条码[{0}]找不到工单信息"
                    .L10nFormat(barcode));
            }

            var childWorkOrder = RT.Service.Resolve<WorkOrderController>()
                .GetWorkOrder(barcodeEntity.WorkOrderId.Value);

            if (childWorkOrder == null)
            {
                throw new ValidationException("生产条码[{0}]找不到工单信息"
                    .L10nFormat(barcode));
            }

            if (childWorkOrder.State == Core.WorkOrders.WorkOrderState.CancelRelease
                || childWorkOrder.State == Core.WorkOrders.WorkOrderState.Close
                || childWorkOrder.State == Core.WorkOrders.WorkOrderState.Finish)
            {
                throw new ValidationException("工单[{0}]状态为{1}，不允许扫描"
                    .L10nFormat(childWorkOrder.No, childWorkOrder.State.ToLabel()));
            }

            if (childWorkOrder.IsPause == YesNo.Yes)
            {
                throw new ValidationException("工单[{0}]已经暂停，不允许扫描"
                    .L10nFormat(childWorkOrder.No, childWorkOrder.State.ToLabel()));
            }

            return barcodeEntity;
        }

        /// <summary>
        /// 根据SN条码获取正在采集的工单
        /// </summary>
        /// <param name="barcode">SN条码</param>
        /// <returns>工单</returns>
        public virtual WorkOrder GetElecWorkOrderBySn(string barcode)
        {
            var sn = ValidateSn(barcode);
            //验证工单
            var workOrder = RF.GetById<WorkOrder>(sn.WorkOrderId);

            if (workOrder == null)
                throw new ValidationException("采集失败！该SN条码无工单信息！".L10N());

            if (workOrder.PanelWorkOrderId.HasValue)//验证组合板工单
            {
                var panelWorkOrder = RF.GetById<WorkOrder>(workOrder.PanelWorkOrderId.Value);

                if (panelWorkOrder == null)
                    throw new ValidationException("采集失败！该SN条码的组合板工单不存在！".L10N());

                ValidateElecWorkOrder(workOrder.PanelWorkOrderId.Value);

                return panelWorkOrder;
            }
            else
            {
                ValidateElecWorkOrder(workOrder.Id);

                return workOrder;
            }
        }

        /// <summary>
        /// 绑定SN条码
        /// </summary>
        /// <param name="bindingSns">SN条码编码列表</param>
        /// <param name="boardNoList">叉板板号</param>
        /// <param name="panelCode">拼板码编码</param>
        /// <param name="forkPlateQty">叉板数</param>
        /// <param name="existPanel">是否存在拼板码</param>
        /// <param name="workOrder">工单</param>
        /// <param name="canBindQty">待绑定子产品数量</param>
        public virtual void PanelBindingSn(List<BindingSn> bindingSns, List<int> boardNoList, string panelCode,
            int forkPlateQty, bool existPanel, WorkOrder workOrder, int canBindQty)
        {
            EntityList<PanelAndBarcode> panelAndBarcodes = new EntityList<PanelAndBarcode>();
            DateTime now = RF.Find<PanelAndBarcode>().GetDbTime();

            var canBindingBoardNos = GetPanelCanBindingBoardNo(panelCode, canBindQty, forkPlateQty, boardNoList);
            canBindingBoardNos.Sort();

            if (canBindingBoardNos.Count < bindingSns.Count)
                throw new ValidationException("绑定失败，拼板可绑定SN数量超待绑定SN数量加叉板数量".L10N());

            Panel panel = null;

            if (existPanel)
                panel = ValidatePanel(panelCode);

            var sns = bindingSns.Select(p => p.Sn).ToList();

            var bindingQty = bindingSns.Count;

            //原有绑定记录
            var existPanelAndBarcodes = GetPanelAndBarcodesByPanleCode(panelCode);
            if (existPanelAndBarcodes.Any())
            {
                bindingQty += existPanelAndBarcodes.Count();

                foreach (var existPanelAndBarcode in existPanelAndBarcodes)
                {
                    existPanelAndBarcode.BindingQty = bindingQty;
                    existPanelAndBarcode.IsBindComplete = true;
                    panelAndBarcodes.Add(existPanelAndBarcode);
                }
            }

            var result = GetPanelAndBarcodesBySns(sns).ToDictionary(p => p.SN);

            var barcodes = RT.Service.Resolve<BarcodeController>().GetBarcodesBySns(sns);

            for (int i = 0; i < bindingSns.Count; i++)
            {
                var bindingSn = bindingSns[i];
                string sn = bindingSn.Sn;
                var barcode = barcodes.FirstOrDefault(p => p.Sn == sn);

                if (barcode == null)
                    throw new ValidationException("绑定失败！条码[{0}]不存在！".L10nFormat(sn));

                if (!result.TryGetValue(sn, out PanelAndBarcode panelAndBarcode))
                {
                    panelAndBarcode = new PanelAndBarcode();
                }

                panelAndBarcode.PanelCode = panelCode;
                panelAndBarcode.SN = sn;
                panelAndBarcode.Qty = bindingSn.Qty;
                panelAndBarcode.PanelQty = workOrder.PanelQty;
                panelAndBarcode.ForkPlateQty = forkPlateQty;
                panelAndBarcode.BindingQty = bindingQty;
                panelAndBarcode.BindingDate = now;
                panelAndBarcode.IsBinding = true;
                panelAndBarcode.Panel = panel;
                panelAndBarcode.OperatorId = AppRuntime.IdentityId;
                panelAndBarcode.WorkOrderId = workOrder.Id;
                panelAndBarcode.BarcodeId = barcode.Id;
                panelAndBarcode.BoardNo = canBindingBoardNos[i];

                //记录SN的工单信息
                panelAndBarcode.ChildWorkOrderId = barcode.WorkOrderId;

                //绑定完成状态更新
                panelAndBarcode.IsBindComplete = true;

                panelAndBarcodes.Add(panelAndBarcode);
            }

            if (panel != null)
            {
                panel.ForkPlateQty = forkPlateQty;
                panel.ForkPlate = string.Join(" ", boardNoList);
                RF.Save(panel);
            }

            RF.Save(panelAndBarcodes);
        }

        /// <summary>
        /// 绑定SN条码
        /// </summary>
        /// <param name="sns">SN条码编码列表</param>
        /// <param name="boardNoList">叉板板号</param>
        /// <param name="panelCode">拼板码编码</param>
        /// <param name="forkPlateQty">叉板数</param>
        /// <param name="existPanel">是否存在拼板码</param>
        /// <param name="workOrder">工单</param>
        /// <param name="canBindingQty"></param>
        public virtual void PanelBindingSn(List<string> sns, List<int> boardNoList, string panelCode,
            int forkPlateQty, bool existPanel, WorkOrder workOrder, int canBindingQty)
        {
            var bindingSns = sns.Select(sn => new BindingSn() { Sn = sn, Qty = 1 }).ToList();

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                PanelBindingSn(bindingSns, boardNoList, panelCode, forkPlateQty, existPanel, workOrder, canBindingQty);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取拼板剩余可绑定的板号集合
        /// </summary>
        /// <param name="panelCode">拼板码</param>
        /// <param name="canBindQty">待绑定子产品数量</param>
        /// <param name="forkPlateQty">叉板数</param>
        /// <param name="forkPlateBoardNos">叉板板号集合</param>
        /// <returns>拼板剩余可绑定的板号集合</returns>
        public virtual List<int> GetPanelCanBindingBoardNo(string panelCode, int canBindQty, int forkPlateQty, List<int> forkPlateBoardNos)
        {
            var boardAndSns = RT.Service.Resolve<PanelBindingController>().GetBoardAndSnInfos(panelCode);
            if (forkPlateQty > canBindQty)
                throw new ValidationException("叉板数不能超过拼板数".L10N());

            if (forkPlateBoardNos.Count != forkPlateQty)
                throw new ValidationException("叉板板号数量与叉板数不匹配".L10N());

            int remainBoardQty = canBindQty - boardAndSns.Count;  //拼板剩余可绑定SN数量
            if (forkPlateBoardNos.Count > remainBoardQty)
                throw new ValidationException("叉板板号数量{0}超出拼板剩余可绑定SN数量{1}".L10nFormat(forkPlateBoardNos.Count, remainBoardQty));

            if (forkPlateBoardNos.Any(p => p > canBindQty))
                throw new ValidationException("叉板板号不能大于拼板数，请在1-{0}中选择".L10nFormat(canBindQty));

            var snBoardNos = boardAndSns.Select(p => p.BoardNo);    //已绑定SN的板号
            List<int> canBindingBoardNos = new List<int>();

            for (int i = 1; i <= canBindQty; i++)
            {
                if (!snBoardNos.Contains(i))
                    canBindingBoardNos.Add(i);
            }

            var bindingBoardNos = snBoardNos.Intersect(forkPlateBoardNos);

            if (bindingBoardNos.Any())
                throw new ValidationException("叉板板号指定失败，叉板板号已绑定SN，剩余可使用板号[{0}]".L10nFormat(string.Join(",", canBindingBoardNos)));

            return canBindingBoardNos.Except(forkPlateBoardNos).ToList();   //排除叉板板号，拼板剩余可绑定的板号集合
        }

        /// <summary>
        /// 移除绑定拼板码
        /// </summary>
        /// <param name="panleCode">拼板码</param>
        /// <returns>工单</returns>
        public virtual void RemoveBindingPanel(string panleCode)
        {
            var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(panleCode);

            if (panelAndBarcodes.Any(x => !x.IsBinding))
            {
                throw new ValidationException("已经分板的绑定记录不能解除绑定".L10N());
            }

            var sns = panelAndBarcodes.Select(p => p.SN).ToList();

            var barcodes = RT.Service.Resolve<BarcodeController>().GetBarcodesBySns(sns);

            foreach (var barcode in barcodes)
            {
                var query = Query<WipProductVersion>()
                    .Where(x => x.Sn == barcode.Sn
                        && (x.WorkOrderId == barcode.WorkOrderId || x.WorkOrderId == barcode.WorkOrder.PanelWorkOrderId));
                if (query.Count() > 0)
                {
                    throw new ValidationException("拼板码绑定的生产条码已经过站，不能解除绑定".L10N());
                }
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var panelAndBarcode in panelAndBarcodes)
                {
                    panelAndBarcode.PersistenceStatus = PersistenceStatus.Deleted;

                    RF.Save(panelAndBarcode);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 移除绑定SN
        /// </summary>
        /// <param name="sn">条码</param>        
        /// <returns>工单</returns>
        public virtual PanelAndBarcode RemoveBindingSn(string sn)
        {
            PanelAndBarcode panelAndBarcode = Query<PanelAndBarcode>()
                .Where(x => x.SN == sn)
                .FirstOrDefault(new EagerLoadOptions().LoadWith(PanelAndBarcode.WorkOrderProperty));

            if (panelAndBarcode == null)
            {
                throw new ValidationException("移除绑定失败！该SN条码[{0}]不存在".L10N());
            }

            if (!panelAndBarcode.IsBinding)
            {
                throw new ValidationException("已经分板的绑定记录不能解除绑定".L10N());
            }

            var barcode = RT.Service.Resolve<BarcodeController>().GetBarcode(sn);

            //组合板
            var query = Query<WipProductVersion>()
                .Where(x => x.Sn == barcode.Sn
                    && (x.WorkOrderId == barcode.WorkOrderId || x.WorkOrderId == barcode.WorkOrder.PanelWorkOrderId));

            if (query.Count() > 0)
            {
                throw new ValidationException("生产条码已经过站，不能解除绑定".L10N());
            }

            var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>()
                .GetPanelAndBarcodesByPanleCode(panelAndBarcode.PanelCode);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var panelAndBarcodeItem in panelAndBarcodes)
                {
                    panelAndBarcodeItem.BindingQty--;
                    panelAndBarcodeItem.IsBindComplete = false;
                }

                RF.Save(panelAndBarcodes);

                //移除条码时，将条码删除
                panelAndBarcode.PersistenceStatus = PersistenceStatus.Deleted;

                RF.Save(panelAndBarcode);

                tran.Complete();
            }

            return panelAndBarcode;
        }

        /// <summary>
        /// MES工单条码绑定记录查询
        /// </summary>
        /// <param name="criteria">MES工单条码绑定记录查询实体</param>
        /// <returns>MES工单条码绑定记录</returns>
        public virtual EntityList<PanelBindingRecord> GetPanelBindingRecords(PanelBindingRecordCriteria criteria)
        {
            var q = Query<PanelBindingRecord>();
            if (criteria.No.IsNotEmpty())
                q.Where(p => p.No.Contains(criteria.No));
            if (criteria.ProductCode.IsNotEmpty())
                q.Where(p => p.ProductCode.Contains(criteria.ProductCode));
            if (criteria.Product != null)
                q.Where(p => p.ProductId == criteria.ProductId);
            if (criteria.Resource != null)
                q.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.WorkShop != null)
                q.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.PlanBeginDate.BeginValue.HasValue)
                q.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            if (criteria.PlanBeginDate.EndValue.HasValue)
                q.Where(p => p.PlanBeginDate <= criteria.PlanBeginDate.EndValue);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取绑定记录
        /// </summary>
        /// <param name="panelId"></param>
        /// <returns></returns>
        public virtual EntityList<PanelAndBarcode> GetPanelAndBarcodes(double panelId)
        {
            return Query<PanelAndBarcode>().Where(p => p.PanelId == panelId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// SN查找绑定记录
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual EntityList<PanelAndBarcode> GetPanelAndBarcodesBySn(string sn)
        {
            EntityList<PanelAndBarcode> panelAndBarcodes = Query<PanelAndBarcode>().As("x")
                  .Join<PanelAndBarcode>("y", (x, y) => x.PanelCode == y.PanelCode)
                  .Where<PanelAndBarcode>((x, y) => y.SN == sn && x.IsBinding)
                  .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return panelAndBarcodes;
        }

        /// <summary>
        /// 判断该拼板码是否已经有绑定信息
        /// </summary>
        /// <param name="panelId">拼板码</param>
        /// <returns>是否已经绑定</returns>
        public virtual bool PanelIsBinding(double panelId)
        {
            return Query<PanelAndBarcode>().Where(p => p.PanelId == panelId && p.IsBinding).Count() > 0;
        }

        /// <summary>
        /// 判断该拼板码是否已经有绑定信息
        /// </summary>
        /// <param name="panelCode">拼板码号</param>
        /// <returns>是否已经绑定</returns>
        public virtual bool PanelIsBinding(string panelCode)
        {
            return Query<PanelAndBarcode>()
                .Join<Panel>((b, p) => b.PanelId == p.Id && p.Code == panelCode)
                .Where(p => p.IsBinding).Count() > 0;
        }

        /// <summary>
        /// 判断条码是否已绑定拼板码
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns>已绑定返回true，否则返回false</returns>
        private PanelAndBarcode GetPanelAndBarcodeRecord(string sn)
        {
            return Query<PanelAndBarcode>().Where(p => p.SN == sn).FirstOrDefault();
        }

        /// <summary>
        /// 根据条码号获取拼板码与条码关系
        /// </summary>
        /// <param name="sn">条码号</param>
        /// <returns>拼板码与条码关系</returns>
        public virtual PanelAndBarcode GetPanelAndBarcodeBySn(string sn)
        {
            return Query<PanelAndBarcode>().Where(p => p.SN == sn && p.IsBinding).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码号获取拼板码与条码关系
        /// </summary>
        /// <param name="sn">条码号</param>
        /// <returns>拼板码与条码关系</returns>
        public virtual EntityList<PanelAndBarcode> GetBindPanelAndBarcodesBySns(List<string> sns)
        {
            return Query<PanelAndBarcode>().Where(p => sns.Contains(p.SN) && p.IsBinding).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据拼板码获取拼板码与条码关系列表
        /// </summary>
        /// <param name="panleCode">拼板码</param>
        /// <returns>拼板码与条码关系列表</returns>
        public virtual EntityList<PanelAndBarcode> GetPanelAndBarcodesByPanleCode(string panleCode)
        {
            return Query<PanelAndBarcode>().Where(p => p.PanelCode == panleCode)
                .ToList(null, new EagerLoadOptions()
                .LoadWithViewProperty());
        }

        /// <summary>
        /// 根据拼板码获取拼板码与条码关系列表
        /// </summary>
        /// <param name="panleCode">拼板码</param>
        /// <param name="isBinding">是否已绑定</param>
        /// <returns>拼板码与条码关系列表</returns>
        public virtual EntityList<PanelAndBarcode> GetPanelAndBarcodesByPanleCode(string panleCode, bool isBinding)
        {
            return Query<PanelAndBarcode>().Where(p => p.PanelCode == panleCode && p.IsBinding == isBinding).ToList(null, new EagerLoadOptions().LoadWith(PanelAndBarcode.BarcodeProperty));
        }

        /// <summary>
        /// 获取拼板码绑定条码列表(按照绑定时间正序排序)
        /// </summary>
        /// <param name="panleCode">拼板码</param>
        /// <returns>条码列表</returns>
        public virtual IList<string> GetPanelBindingSn(string panleCode)
        {
            return Query<PanelAndBarcode>().Where(p => p.PanelCode == panleCode && p.IsBinding).Select(p => p.SN).OrderBy(p => p.BindingDate).ToList<string>();
        }

        /// <summary>
        /// 获取拼板绑定条码信息(按照绑定时间正序排序)
        /// </summary>
        /// <param name="panleCode">拼板码</param>
        /// <returns>板号与SN对应信息</returns>
        public virtual IList<BoardAndSnInfo> GetBoardAndSnInfos(string panleCode)
        {
            return Query<PanelAndBarcode>()
                .Where(p => p.PanelCode == panleCode && p.IsBinding)
                .Select(p => new { Sn = p.SN, BoardNo = p.BoardNo })
                .OrderBy(p => p.BindingDate)
                .ToList<BoardAndSnInfo>();
        }

        /// <summary>
        /// 获取条码号列表所对应的所有拼板码与条码关系列表
        /// </summary>
        /// <param name="snList">条码号列表</param>
        /// <returns>拼板码与条码关系列表</returns>
        public virtual EntityList<PanelAndBarcode> GetPanelAndBarcodesBySns(List<string> snList)
        {
            return Query<PanelAndBarcode>().Where(p => snList.Contains(p.SN)).ToList();
        }

        /// <summary>
        /// 获取该工单下拼板码与条码关系列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>拼板码与条码关系列表</returns>
        public virtual EntityList<PanelAndBarcode> GetBindingRecords(double workOrderId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<PanelAndBarcode>().Where(p => p.WorkOrderId == workOrderId)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取该工单下拼板码与条码关系
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="isBinding">是否已绑定</param>
        /// <returns>拼板数量</returns>
        public virtual int GetBindingPanelCount(double workOrderId, bool isBinding)
        {
            return Query<PanelAndBarcode>().Where(p => p.WorkOrderId == workOrderId && p.IsBinding == isBinding).Select(p => p.PanelCode).Distinct().ToList().Count();
        }

        /// <summary>
        /// 获取该工单下拼板码与条码关系未绑定SN数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>未绑定SN数量</returns>
        public virtual int GetBindingSnCount(double workOrderId)
        {
            return Query<PanelAndBarcode>().Where(p => p.WorkOrderId == workOrderId && !p.IsBinding).Count();
        }

        /// <summary>
        /// 报废拼板码与条码关系信息
        /// </summary>
        /// <param name="arg">条码报废后事件</param>
        public virtual void ScrapPanelBarcodes(BarcodeScrapEvent arg)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var sns = arg.BarcodeInfos.Select(p => p.Sn).ToList();
                var panelAndBarcodes = GetPanelAndBarcodesBySns(sns);
                foreach (var panelAndBarcode in panelAndBarcodes)
                {
                    panelAndBarcode.PersistenceStatus = PersistenceStatus.Deleted;
                }
                RF.Save(panelAndBarcodes);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取工单已打印拼板数（未报废）
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>打印拼板数</returns>
        public virtual int GetWorkOrderPanelQtyWithoutScrap(double workOrderId)
        {
            return Query<Panel>().Where(p => p.WorkOrderId == workOrderId && !p.IsScrap).Count();
        }

        /// <summary>
        /// 获取工单已打印(未报废)未绑定的拼板数量
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>拼板数量</returns>
        public virtual int GetWorkOrderUnBindingPanelQty(double workOrderId)
        {
            return Query<Panel>()
                .NotExists<PanelAndBarcode>((b, q) => q.Where(p => p.PanelCode == b.Code))
                .Where(p => p.WorkOrderId == workOrderId)
                .Count();
        }

        /// <summary>
        /// 获取工单已绑定拼板的SN数量（排除已报废条码）
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>已绑定拼板的SN数量</returns>
        public virtual int GetWorkOrderPanelBindingSnQty(double workOrderId)
        {
            return Query<PanelAndBarcode>()
                .Where(p => p.WorkOrderId == workOrderId)
                .Count();
        }

        /// <summary>
        /// 获取工单拼板绑定信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>工单拼板绑定信息</returns>
        public virtual WorkOrderPanleBindingInfo GetWorkOrderPanleBindingInfo(double workOrderId)
        {
            var unBindPanelQty = GetWorkOrderUnBindingPanelQty(workOrderId);
            var bindingSnQty = GetWorkOrderPanelBindingSnQty(workOrderId);
            WorkOrderPanleBindingInfo workOrderPanleBindingInfo = new WorkOrderPanleBindingInfo()
            {
                UnBindingPanelQty = unBindPanelQty,
                BindingSnQty = bindingSnQty
            };

            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder != null)
            {
                if (workOrder.IsPanelWorkOrder)
                {
                    var childWoTotalPlanQty = Query<WorkOrder>()
                        .Select(x => x.PlanQty.SUM())
                        .Where(x => x.PanelWorkOrderId == workOrder.Id)
                        .FirstOrDefault<decimal>();

                    //workOrderPanleBindingInfo.UnBindingSnQty = (int)(workOrder.PlanQty * workOrder.PanelQty - bindingSnQty);
                    workOrderPanleBindingInfo.UnBindingSnQty = (int)(childWoTotalPlanQty - bindingSnQty);
                }
                else
                {
                    workOrderPanleBindingInfo.UnBindingSnQty = (int)(workOrder.PlanQty - bindingSnQty);
                }

                workOrderPanleBindingInfo.WorkOrderOnlineQty = workOrder.OnlineQty;
            }

            return workOrderPanleBindingInfo;
        }

        /// <summary>
        /// 拼板码报废
        /// </summary>
        /// <param name="e">拼板码报废事件</param>
        public virtual void PanelScrap(PanelScrapEvent e)
        {
            var barcodes = GetPanelBindingSn(e.PanelCode);
            if (barcodes.Count == 0)
                return;
            RT.Service.Resolve<BarcodeController>().BarcodeScrap(barcodes.ToList(), e.ScrapReason);
        }

        /// <summary>
        /// 获取组合板工单待绑定子产品数量
        /// </summary>
        /// <param name="workOrder">组合板工单</param>
        /// <returns></returns>
        public virtual int GetPanelWorkOrderCanBindingQty(SIE.Core.WorkOrders.WorkOrder workOrder)
        {
            var pcbItemDetails = RT.Service.Resolve<IPanelBinding>().GetPcbItemDetailInfos(workOrder.ProductId);

            if (pcbItemDetails == null || pcbItemDetails.Count <= 0)
            {
                throw new ValidationException("组合板工单[{0}]对应的产品编码没有维护PCB物料属性明细".L10nFormat(workOrder.No));
            }

            int qty = 0;

            foreach (var pcbItemDetail in pcbItemDetails)
            {
                qty += workOrder.PanelQty * pcbItemDetail.Qty;
            }

            return qty;
        }


        /// <summary>
        /// 获取拼板码实际数量（拼板码的拼板码-叉板数）
        /// </summary>
        /// <param name="code">拼板码号</param>
        /// <param name="barcodeType">条码类型</param>
        /// <returns>拼板码实际数量</returns>
        public virtual decimal GetActualPanelQty(string code, BarcodeType? barcodeType)
        {
            Check.NotNullOrEmpty(code, nameof(code));
            if (barcodeType == BarcodeType.SN)
            {
                var sns = RT.Service.Resolve<PanelBindingController>().GetPanelBindingSn(code);
                return sns.Count;
            }
            else
            {
                var panel = Query<Panel>().Where(p => p.Code == code)
                    .FirstOrDefault(new EagerLoadOptions().LoadWith(Panel.WorkOrderProperty));

                var workOrder = panel.WorkOrder;
                if (workOrder.IsPanelWorkOrder)
                {
                    var candBindQty = GetPanelWorkOrderCanBindingQty(workOrder);
                    return candBindQty - panel.ForkPlateQty;
                }
                else
                {
                    return workOrder.PanelQty - panel.ForkPlateQty;
                }
            }
        }

    }
}