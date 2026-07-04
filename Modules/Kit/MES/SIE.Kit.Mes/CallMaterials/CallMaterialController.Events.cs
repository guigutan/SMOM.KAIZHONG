using SIE.Api;
using SIE.Core.Barcodes;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.CallMaterials;
using SIE.EventMessages.StationStorage;
using SIE.Items;
using SIE.Kit.MES.CallMaterials.Interfaces;
using SIE.Kit.MES.CallMaterials.Statistics;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单控制器
    /// </summary>
    public partial class CallMaterialController
    {
        #region 工单用料数统计
        /// <summary>
        /// 更新工单用料数统计
        /// </summary>
        /// <param name="wipEvent">在制品采集后事件</param>
        public virtual void UpdateMaterialStatistics(WipCollectedEvent wipEvent)
        {
            var data = wipEvent.Data;
            var product = data.Product;
            var workcell = data.Workcell;
            var process = product.Routing.Current;
            ////非装配工序不会用到扣料，不处理
            if (!(process.Type == Tech.Processs.ProcessType.Assembly || process.Type == Tech.Processs.ProcessType.BatchAssembly))
                return;
            if (ValidateIsRepeat(data.Barcodes[0].Code, data.Barcodes[0].Type, workcell.ResourceId, workcell.ProcessId, product.ItemId))
                return;
            process.Boms.ForEach(bom => UpdateMaterialStatistics(product.WorkOrderId, bom.ItemId, bom.Qty * product.Qty));
        }

        /// <summary>
        /// 验证是否重复过站
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>重复过站返回true</returns>
        bool ValidateIsRepeat(string barcode, BarcodeType type, double resourceId, double processId, double itemId)
        {
            var rule = RT.Service.Resolve<ItemController>().GetRetrospectType(itemId);
            ////重复过站不记录
            if (rule == Core.Items.RetrospectType.Single && RT.Service.Resolve<WipProductVersionController>().IsRepeatProcess(barcode, type, resourceId, processId))
                return true;
            ////批次不考虑重复过站
            return false;
        }

        /// <summary>
        /// 更新工单用料数统计
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="qty">用料数量</param>
        void UpdateMaterialStatistics(double workOrderId, double itemId, decimal qty)
        {
            var result = DB.Update<MaterialStatistics>()
                        .Where(p => p.WorkOrderId == workOrderId && p.ItemId == itemId)
                        .Set(p => p.UsedQty, p => p.UsedQty + qty)
                        .Execute();
            if (result == 0)
            {
                var statistics = new MaterialStatistics()
                {
                    WorkOrderId = workOrderId,
                    ItemId = itemId,
                    UsedQty = qty
                };
                RF.Save(statistics);
            }
        }

        /// <summary>
        /// 获取工单物料用来数
        /// </summary>
        /// <param name="woIds">工单集合</param>
        /// <returns>数据集合</returns>
        public virtual EntityList<MaterialStatistics> GetMaterialStatistics(List<double> woIds)
        {
            return Query<MaterialStatistics>().Where(p => woIds.Contains(p.WorkOrderId)).ToList();
        }
        #endregion

        #region 物料接收（API)
        /// <summary>
        /// 获取接收标签信息
        /// 1、序列号管理标签：验证标签是否存在；是否已接收；是否关联叫料单；标签所属资源是否与接收资源一致；叫料工单是否已关闭
        /// 2、非序列号管理标签（需叫料单接收）：同上验证；叫料单是否有该物料需求
        /// </summary>
        /// <param name="label">标签号</param>
        /// <param name="billNo">叫料单号</param>
        /// <param name="resourceId">所属资源ID</param>
        /// <returns>接收标签信息</returns>
        [ApiService("获取接收标签信息")]
        [return: ApiReturn("接收标签信息。参数类型：ReceiveLabelInfo")]
        public virtual ReceiveLabelInfo GetItemLabelInfo([ApiParameter("标签号")] string label, [ApiParameter("叫料单号")] string billNo, [ApiParameter("所属资源")] double resourceId)
        {
            ReceiveLabelInfo receive = new ReceiveLabelInfo();
            if (label.IsNullOrEmpty())
                return SetReceiveMessage(receive, "标签号不能为空".L10N());
            //记录接口日志
            SaveGetPackingLabelLog(label);
            ////获取标签关联叫料单号
            var callInfo = RT.Service.Resolve<ICallMaterial>().GetPackingLabel(label);
            if (billNo.IsNullOrEmpty())
            {
                ////序列号管理物料，已关联叫料单
                if (callInfo == null || callInfo.BillNo.IsNullOrEmpty())
                    return SetReceiveMessage(receive, "标签{0}未关联叫料单信息，请扫描叫料单关联".L10nFormat(label), 2);
                CreateReceiveLabelInfo(receive, label, callInfo.BillNo, resourceId, false);
            }
            else
            {
                ////非序列号管理物料，需输入叫料单接收 
                if (callInfo != null && !callInfo.BillNo.IsNullOrEmpty())
                    return SetReceiveMessage(receive, "序列号管理标签不允许叫料单接收".L10N());
                CreateReceiveLabelInfo(receive, label, billNo, resourceId, true);
            }

            return receive;
        }

        /// <summary>
        /// 保存获取标签信息日志
        /// </summary>
        /// <param name="labelNo">标签号</param>
        private void SaveGetPackingLabelLog(string labelNo)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "标签号:{0}".L10nFormat(labelNo);
                var log = new InterfaceLog()
                {
                    Name = "ICallMaterial",
                    Method = "GetPackingLabel",
                    ControllerName = "DistributionController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建物料接收信息
        /// </summary>
        /// <param name="receive">接收信息</param>
        /// <param name="label">标签号</param>
        /// <param name="billNo">叫料单号</param>
        /// <param name="resourceId">所属资源</param>
        /// <param name="validateItem">是否验证物料</param>
        /// <returns>验证后接收信息</returns>
        private void CreateReceiveLabelInfo(ReceiveLabelInfo receive, string label, string billNo, double resourceId, bool validateItem)
        {
            ////判断标签是否存在 
            var packLabel = RT.Service.Resolve<PackingLabelController>().GetPackingLabel(label);
            if (packLabel == null)
            {
                SetReceiveMessage(receive, "标签{0}不存在，请检查标签是否有误".L10nFormat(label));
                return;
            }
            ////判断是否已接收
            var isReceived = RT.Service.Resolve<ItemLabelController>().IsReceived(label);
            if (isReceived)
            {
                SetReceiveMessage(receive, "标签{0}已接收成功，请勿重复接收".L10nFormat(label));
                return;
            }
            var bill = RT.Service.Resolve<CallMaterialController>().GetCallMaterialBill(billNo);
            if (bill == null)
            {
                SetReceiveMessage(receive, "叫料单不存在".L10N());
                return;
            }
            if (bill.ResourceId != resourceId)
            {
                SetReceiveMessage(receive, "叫料单所属资源与当前资源不一致".L10N());
                return;
            }
            if (bill.Status != CallMaterialStatus.ToReceive)
            {
                SetReceiveMessage(receive, "只有[{0}]的叫料单才能接收，当前叫料单状态为[{1}]".L10nFormat(CallMaterialStatus.ToReceive.ToLabel(), bill.Status.ToLabel()));
                return;
            }
            var workOrder = RF.GetById<WorkOrder>(bill.CallWorkOrder.WorkOrderId);
            if (workOrder == null)
            {
                SetReceiveMessage(receive, "叫料工单不存在".L10N());
                return;
            }
            if (workOrder.State == Core.WorkOrders.WorkOrderState.Close)
            {
                SetReceiveMessage(receive, "标签所属工单已关闭".L10N());
                return;
            }
            if (validateItem && !bill.DetailList.Any(p => p.ItemId == packLabel.ItemId && !p.IsReceived))
            {
                SetReceiveMessage(receive, "叫料无此物料需求".L10N());
                return;
            }
            receive.Label = label;
            receive.ItemId = packLabel.ItemId.Value;
            receive.ItemCode = packLabel.Item?.Code;
            receive.ItemName = packLabel.Item?.Name;
            receive.SpecificationModel = packLabel.Item?.SpecificationModel;
            receive.WorkOrderId = bill.CallWorkOrder.WorkOrderId;
            receive.WorkOrderNo = bill.CallWorkOrder.WorkOrder?.No;
            receive.BillId = bill.Id;
            receive.BillNo = bill.No;
            receive.Qty = packLabel.Qty;
            receive.StationId = bill.StationId;
            receive.StationName = bill.Station?.Name;
            receive.BatchNo = "V" + RF.Find<CallMaterialBill>().GetDbTime().Date.ToString("yyyyMMdd");
            receive.ResourceId = bill.ResourceId;
            receive.ResourceName = bill.Resource?.Name;
            receive.Result = 0;
            receive.Message = "接收成功".L10N();
        }

        /// <summary>
        /// 设置提示验证结果跟提示信息
        /// </summary>
        /// <param name="receive">接收信息</param>
        /// <param name="message">提示信息</param>
        /// <param name="result">结果</param>
        /// <returns>验证后接收信息</returns>
        ReceiveLabelInfo SetReceiveMessage(ReceiveLabelInfo receive, string message, int result = 1)
        {
            receive.Result = result;
            receive.Message = message;
            return receive;
        }

        /// <summary>
        /// 接收物料标签信息,PDA需回写接收人
        /// </summary>
        /// <param name="labelInfos">标签信息列表</param>  
        [ApiService("接收物料标签信息")]
        public virtual void ReceiveItemLabel([ApiParameter("标签信息列表")] List<ReceiveLabelInfo> labelInfos)
        {
            if (labelInfos.Count == 0)
                throw new ValidationException("没有传入需要更新的数据".L10N());
            EntityList<CallMaterialReceive> recevies = new EntityList<CallMaterialReceive>();
            var billIds = labelInfos.Select(p => p.BillId).ToList();
            var billList = RT.Service.Resolve<CallMaterialController>().GetCallMaterialBills(billIds);
            labelInfos.ForEach(labelInfo =>
            {
                var bill = billList.FirstOrDefault(p => p.Id == labelInfo.BillId);
                //记录接口日志
                SaveGetPackingLabelLog(labelInfo.Label);
                var callInfo = RT.Service.Resolve<ICallMaterial>().GetPackingLabel(labelInfo.Label);
                if (callInfo == null)
                {
                    //非序列号管理物料
                    var packLabel = RT.Service.Resolve<PackingLabelController>().GetPackingLabel(labelInfo.Label);
                    recevies.Add(CreateCallMaterialReceive(labelInfo, bill, packLabel, true));
                }
                else
                {
                    callInfo.SnIdList.ForEach(labelId =>
                    {
                        var packLabel = RF.GetById<PackingLabel>(labelId);
                        recevies.Add(CreateCallMaterialReceive(labelInfo, bill, packLabel, false));
                    });
                }
            });
            labelInfos.ForEach(labelInfo =>
            {
                var bill = billList.FirstOrDefault(p => p.Id == labelInfo.BillId);
                var packLabel = RT.Service.Resolve<PackingLabelController>().GetPackingLabel(labelInfo.Label);
                bill.DetailList.ForEach(detail =>
                {
                    if (detail.ItemId == packLabel.ItemId)
                    {
                        //if (detail.IsReceived)
                        //throw new ValidationException("不允许接收，叫料单[{0}]的物料[{1}]已全部接收".L10nFormat(bill.No, detail.Item?.Name));
                        detail.ReceiveById = RT.IdentityId;
                        detail.ReceiveDate = RF.Find<CallMaterialBill>().GetDbTime();
                        detail.ReceiveQty += packLabel.Qty;
                        if (detail.ReceiveQty >= detail.CalledQty)
                            detail.IsReceived = true;
                    }
                });
            });
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(recevies);
                ////更新叫料单状态
                billList.ForEach(bill => bill.Status = UpdateBillStatus(bill, true));
                RF.Save(billList);
                ////生成物料标签
                GenerateItemLabel(recevies);
                ////更新工位物料库存
                UpdateStationStorage(recevies);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建物料接收
        /// </summary>
        /// <param name="labelInfo">接收标签信息</param>
        /// <param name="bill">叫料单</param>
        /// <param name="packLabel">包装标签</param>
        /// <param name="validateItem">是否验证物料</param>
        /// <returns>物料接收</returns>
        private CallMaterialReceive CreateCallMaterialReceive(ReceiveLabelInfo labelInfo, CallMaterialBill bill, PackingLabel packLabel, bool validateItem)
        {
            ValidateCallMeterialReceive(labelInfo, bill, packLabel, validateItem);
            CallMaterialReceive receive = new CallMaterialReceive()
            {
                Label = packLabel.No,
                Qty = packLabel.Qty,
                ReceiveDate = RF.Find<CallMaterialBill>().GetDbTime(),
                ItemId = packLabel.ItemId.Value,
                CallMaterialBillId = labelInfo.BillId,
                WorkOrderId = labelInfo.WorkOrderId,
                ResourceId = labelInfo.ResourceId,
                WorkShopId = RF.GetById<WipResource>(labelInfo.ResourceId)?.WorkShopId,
                RemainQty = labelInfo.Qty,
                DistStationId = labelInfo.StationId,
                BatchNo = labelInfo.BatchNo,
                ReceiveById = RT.IdentityId,
            };
            //bill.DetailList.ForEach(detail =>
            //{
            //    if (detail.ItemId == receive.ItemId)
            //    {
            //        if (detail.IsReceived)
            //            throw new ValidationException("不允许接收，叫料单[{0}]的物料[{1}]已全部接收".L10nFormat(bill.No, detail.Item?.Name));
            //        detail.ReceiveById = receive.ReceiveById;
            //        detail.ReceiveDate = receive.ReceiveDate;
            //        detail.ReceiveQty += packLabel.Qty;
            //        if (detail.ReceiveQty >= detail.CalledQty)
            //            detail.IsReceived = true;
            //    }
            //});
            return receive;
        }

        /// <summary>
        /// 验证物料接收
        /// </summary>
        /// <param name="labelInfo">接收标签信息</param>
        /// <param name="bill">叫料单</param>
        /// <param name="packLabel">包装标签</param>
        /// <param name="validateItem">是否验证物料</param> 
        private void ValidateCallMeterialReceive(ReceiveLabelInfo labelInfo, CallMaterialBill bill, PackingLabel packLabel, bool validateItem)
        {
            if (packLabel == null)
                throw new ValidationException("标签不存在，请检查标签是否有误".L10N());
            if (RT.Service.Resolve<ItemLabelController>().IsReceived(packLabel.No))
                throw new ValidationException("标签{0}已在接收".L10nFormat(packLabel.No));
            string label = packLabel.No;
            ////判断是否已接收 
            if (RT.Service.Resolve<ItemLabelController>().IsReceived(label))
                throw new ValidationException("标签{0}已接收成功，请勿重复接收".L10nFormat(label));
            if (bill == null)
                throw new ValidationException("叫料单不存在".L10N());
            if (bill.ResourceId != labelInfo.ResourceId)
                throw new ValidationException("叫料单所属资源与当前资源不一致".L10N());
            var workOrder = RF.GetById<WorkOrder>(bill.CallWorkOrder.WorkOrderId);
            if (workOrder == null)
                throw new ValidationException("叫料工单不存在".L10N());
            if (workOrder.State == Core.WorkOrders.WorkOrderState.Close)
                throw new ValidationException("标签所属工单已关闭".L10N());
            if (validateItem && !bill.DetailList.Any(p => p.ItemId == packLabel.ItemId && !p.IsReceived))
                throw new ValidationException("叫料无此物料需求".L10N());
        }

        /// <summary>
        /// 生成物料标签
        /// </summary>
        /// <param name="recevies">物料接收列表</param>
        private void GenerateItemLabel(EntityList<CallMaterialReceive> recevies)
        {
            recevies.ForEach(recevie =>
            {
                RF.Save(new ItemLabel()
                {
                    Label = recevie.Label,
                    ItemId = recevie.ItemId,
                    ItemSourceType = recevie.Item.ItemSourceType,
                    ItemType = recevie.Item.Type,                    
                    Qty = recevie.Qty,                    
                    SourceType = LabelSource.Receive,
                    Specification = recevie.Item?.SpecificationModel,
                    UnitId = recevie.Item?.UnitId,
                    WorkOrderId = recevie.WorkOrderId,
                    Factory=recevie.WorkOrder.Factory,
                });
            });
        }

        /// <summary>
        /// 更新工位库存
        /// </summary>
        /// <param name="recevies">物料接收列表</param>
        private void UpdateStationStorage(EntityList<CallMaterialReceive> recevies)
        {
            var dicStations = recevies.GroupBy(p => p.DistStationId);
            dicStations.ForEach(dicStation =>
            {
                var result = dicStation.ToList();
                var dicWos = result.GroupBy(p => p.WorkOrderId);
                dicWos.ForEach(dicwo =>
                {
                    var result1 = dicwo.ToList();
                    var dicItems = result1.GroupBy(p => p.ItemId);
                    dicItems.OrderBy(p => p).ForEach(dicItem =>
                      {
                          var qty = dicItem.Sum(p => p.Qty);
                          StationStorageHelper.ItemStoreChanged(dicStation.Key, dicwo.Key, dicItem.Key, 0, qty, -qty);
                      });
                });
            });
        }
        #endregion

        #region 物料标签退料API
        /// <summary>
        /// 获取可退料物料标签信息
        /// 从物料接收、物料下料中获取可退料物料标签信息
        /// </summary>
        /// <param name="label">物料标签号</param>
        /// <param name="resourceId">所属资源ID</param>
        /// <returns>可退料物料信息</returns>
        [ApiService("获取可退料物料标签信息")]
        [return: ApiReturn("可退料物料标签信息. 参数类型: WithdrawalableItemLabelInfo")]
        public virtual WithdrawalableItemLabelInfo GetWithdrawalableItemLabelInfo([ApiParameter("标签号")] string label, [ApiParameter("所属资源")] double resourceId)
        {
            ////退料条件：1、物料必须有接收才能退料；2、物料必须下料才能退料；3、已接收未上料可以退料
            ////验证是否已接收
            if (!IsCallMaterialReceive(label, resourceId))
                throw new ValidationException("不能退料,物料标签: {0}没有做接收!".L10nFormat(label));
            ////验证是否重复退料
            if (IsCallMaterialWithdrawal(label, resourceId))
                throw new ValidationException("物料标签: {0}不能重复退料!".L10nFormat(label));
            WithdrawalableItemLabelInfo info = null;
            const LoadItemSourceType sourceType = LoadItemSourceType.ItemLabel;
            LoadItemController loadItemController = RT.Service.Resolve<LoadItemController>();
            if (loadItemController.IsUnloadItem(label, sourceType))
            {
                ////从下料列表进行退料
                info = GetWithdrawalInfo(label, resourceId, sourceType);
            }
            else
            {
                ////判断是否已接收未上料，然后进行退料
                if (loadItemController.IsLoadItem(label, sourceType))
                    throw new ValidationException("物料标签: {0} 使用中，不允许退料，请先做下料操作!".L10nFormat(label));
                info = GetWithdrawalInfo(label, resourceId);
            }

            if (info == null)
                throw new ValidationException("物料标签: {0} 在当前资源无退料明细!".L10nFormat(label));
            return info;
        }

        /// <summary>
        /// 提交物料退料信息
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">待提交的可退料物料信息</param>
        [ApiService("提交物料退料信息")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void SubmitWithdrawalableItemLabelInfos([ApiParameter("可退料物料信息")] List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            try
            {
                SaveWithdrawalableInfos(withdrawalableItemLabelInfos);
            }
            catch (Exception exmsg)
            {
                throw new ValidationException("物料退料提交失败, 异常信息: {0}".L10nFormat(exmsg.Message));
            }
        }

        /// <summary>
        /// 物料退料记录查询
        /// </summary>
        /// <param name="label">物料标签</param>
        /// <param name="date">退料日期</param>
        /// <returns>物料退料信息</returns>
        [ApiService("获取已退料物料标签信息")]
        [return: ApiReturn("已退料物料标签信息.  参数类型: List<WithdrawaledItemLabelInfo>")]
        public virtual List<WithdrawaledItemLabelInfo> GetWithdrawaledItemLabelInfos([ApiParameter("标签号")] string label = null, [ApiParameter("退料日期")] string date = null)
        {
            if (string.IsNullOrEmpty(label) && string.IsNullOrEmpty(date))
                throw new ValidationException("标签号和退料日期不能同时为Null".L10nFormat());

            DateTime? dataTime = null;
            if (!string.IsNullOrEmpty(date))
                dataTime = DateTime.Parse(date);
            var withdrawaledlInfos = GetWithdrawaledInfos(label, dataTime);
            if (withdrawaledlInfos == null || withdrawaledlInfos.Count == 0)
                throw new ValidationException("物料标签: {0} 无已退料信息!".L10nFormat(label));
            return withdrawaledlInfos;
        }

        #endregion 物料标签退料API
    }
}