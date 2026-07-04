using SIE.Alert;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.CallMaterials;
using SIE.EventMessages.Shipment;
using SIE.EventMessages.StationStorage;
using SIE.Items;
using SIE.Items.ProductModels;
using SIE.Kit.MES.CallMaterials.Configs;
using SIE.Kit.MES.CallMaterials.Interfaces;
using SIE.Kit.MES.Stations;
using SIE.Kit.MES.StationStorages;
using SIE.Kit.MES.Storages;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单控制器
    /// </summary>
    public partial class CallMaterialController : DomainController
    {
        #region 叫料
        /// <summary>
        /// 更新叫料单
        /// 处理发运单取消、分配、发货操作
        /// </summary>
        /// <param name="processSOEvent">发运单</param>
        public virtual void UpdateCallMaterialBill(ProcessSOToWorkFeedEvent processSOEvent)
        {
            var processSOEventList = processSOEvent?.ProcessSOEventList?.Where(p => p.OrderType == 80).ToList();
            if (processSOEventList != null && processSOEventList.Count > 0)
            {
                var requireNos = processSOEventList.Select(p => p.RequireNo).ToList();
                var callBills = RT.Service.Resolve<CallMaterialController>().GetCallMaterialBills(requireNos);
                foreach (var eventItem in processSOEventList)
                {
                    var workOrderBill = callBills.FirstOrDefault(p => p.No == eventItem.RequireNo);
                    var processBomList = workOrderBill?.CallWorkOrder?.WorkOrder?.ProcessBomList;
                    callBills.Where(p => p.No == eventItem.RequireNo).ForEach(e => e.Remark = eventItem.Remark);

                    if (eventItem.Result == 0)
                    {
                        callBills.Where(p => p.No == eventItem.RequireNo).ForEach(e =>
                        {
                            e.Status = CallMaterialStatus.Cancel;
                            SendStationItemStoreEvent(e);
                        });
                    }
                    else if (eventItem.Result == 10)
                    {
                        foreach (var detailFeed in eventItem.DetailList)
                        {
                            callBills.Where(p => p.No == eventItem.RequireNo).ForEach(m =>
                            {
                                //发料成功更新发料时间和发料数量，找不到主料到替代料中找
                                m.DetailList.Where(e => e.RowNo.ToString() == detailFeed.LineNo
                                && (e.ItemId == detailFeed.ItemId || processBomList.Count > 0 && processBomList.Any(a => a.ItemId == e.ItemId && a.AlternativeList.Any(b => b.ItemId == detailFeed.ItemId))))
                                .ForEach(n =>
                                {
                                    n.ActualQty = detailFeed.ShippingQty;
                                });
                            });
                        }

                        callBills.Where(p => p.No == eventItem.RequireNo).ForEach(e =>
                        {
                            e.Status = UpdateBillStatus(e);
                            e.SendingTime = eventItem.ShippingDate;
                        });
                    }
                    else
                    {
                        //
                    }
                }

                RF.Save(callBills);
            }
        }

        /// <summary>
        /// 发送工位物料变更事件
        /// </summary>
        /// <param name="bill">叫料单</param>
        void SendStationItemStoreEvent(CallMaterialBill bill)
        {
            ////叫料单取消：变更工位物料库存中的在途数量
            bill.DetailList.OrderBy(p => p.ItemId).ForEach(detail =>
              {
                  StationStorageHelper.ItemStoreChanged(bill.StationId, detail.Bill?.CallWorkOrder?.WorkOrderId, detail.ItemId, 0, 0, -detail.CalledQty);
              });
        }

        /// <summary>
        /// 叫料单更新状态
        /// 1、叫料单所有明细实际数量>=叫料数量:待接收
        /// 2、叫料单所有明细都已经接收：已接收
        /// </summary>
        /// <param name="callBill">叫料单</param>     
        /// <param name="isPDA">是否PDA要更新</param>
        /// <returns>叫料单状态</returns>
        CallMaterialStatus UpdateBillStatus(CallMaterialBill callBill, bool isPDA = false)
        {
            if (isPDA && callBill.DetailList.All(p => p.IsReceived))
            {
                callBill.Status = CallMaterialStatus.Received;
            }
            else
            {
                callBill.Status = CallMaterialStatus.ToReceive;
            }

            return callBill.Status;
        }

        /// <summary>
        /// MES取消叫料或设置紧急
        /// </summary>
        /// <param name="billNo">叫料单号</param>
        /// <param name="op">操作类型0取消叫料;10设置优先级</param>
        /// <param name="priority">优先级0普通;1紧急</param>
        void OperateCallMaterialBill(string billNo, int op, int priority = 0)
        {
            CallMaterialEvent callEvent = new CallMaterialEvent();
            callEvent.BillNo = billNo;
            callEvent.Operation = op;
            callEvent.Priority = priority;
            RT.EventBus.Publish<CallMaterialEvent>(callEvent);
        }

        /// <summary>
        /// 发送叫料单到WMS
        /// </summary>
        /// <param name="callBills">叫料单集合</param>      
        void SendToWMS(EntityList<CallMaterialBill> callBills)
        {
            if (callBills.Count == 0)
            {
                return;
            }

            RemoteWorkFeedSOEvent callBillEvent = new RemoteWorkFeedSOEvent();
            callBillEvent.RemoteSOEventList = new List<RemoteSOEvent>();

            var callMaterialBill = callBills.First();

            //callMaterialBill.ResourceId 这个资源是采集界面所选择的资源（产线）
            double wareHouseId = GetWareHouseId(callMaterialBill.ResourceId);

            double? enterpriseId = 0;

            var wipResouce = RF.GetById<WipResource>(callMaterialBill.ResourceId);
            if (wipResouce != null && wipResouce.SourceType == SyncSourceType.Enterprise)
            {
                enterpriseId = wipResouce.SourceId;
            }

            foreach (var bill in callBills)
            {
                RemoteSOEvent callEvent = new RemoteSOEvent();
                #region 赋值单头信息发送给WMS
                callEvent.RequireNo = bill.No;
                callEvent.OrderType = 80;
                callEvent.PriorityType = (int)bill.Priority;
                callEvent.EnterpriseId = enterpriseId;
                callEvent.ShippingWareHouseId = wareHouseId;
                callEvent.IsPartShipping = true;
                #endregion
                callEvent.DetailList = new List<RemoteSODTLEvent>();
                #region 赋值明细信息发送给WMS
                foreach (var detail in bill.DetailList.OrderBy(p => p.ItemId))
                {
                    RemoteSODTLEvent dtlCallEvent = new RemoteSODTLEvent();
                    dtlCallEvent.LineNo = detail.RowNo.ToString();
                    dtlCallEvent.WorkNo = bill.CallWorkOrder.WorkOrder.No;
                    dtlCallEvent.ItemId = detail.ItemId;
                    dtlCallEvent.ExpectQty = detail.CalledQty;
                    callEvent.DetailList.Add(dtlCallEvent);
                    //工位物料在途数量变更
                    StationStorageHelper.ItemStoreChanged(bill.StationId, bill.CallWorkOrder?.WorkOrderId, detail.ItemId, 0, 0, detail.CalledQty);
                }

                callBillEvent.RemoteSOEventList.Add(callEvent);
                #endregion
            }

            RT.EventBus.Publish(callBillEvent);
        }
        #endregion

        #region 叫料工单 
        /// <summary>
        /// 获取叫料工单数据
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>叫料工单</returns>
        public virtual EntityList<CallMaterialWorkOrder> GetCallMaterialWo(CallMaterialWoCriteria criteria)
        {
            var query = Query<CallMaterialWorkOrder>().Where(p => p.WorkOrder.IsPause == YesNo.No);

            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.No));
            }

            if (!criteria.ProductCode.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            }

            if (!criteria.ProductName.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName));
            }

            if (criteria.WorkOrderState.HasValue)
            {
                query.Where(p => p.WorkOrder.State == criteria.WorkOrderState);
            }
            else
            {
                query.Where(p => p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release || p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing);
            }

            if (criteria.ResourceId != 0)
            {
                query.Where(p => p.WorkOrder.ResourceId == criteria.ResourceId);
                CreateMaterialSetting(criteria.ResourceId);
            }

            return query.OrderByDescending(p => p.WorkOrder.State).OrderBy(p => p.Index).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取下一个工单->叫料
        /// </summary>
        /// <param name="currWo">工单</param>
        /// <returns>叫料工单</returns>
        public virtual CallMaterialWorkOrder GetNextWoToCallMaterial(WorkOrder currWo)
        {
            if (currWo == null || currWo.ResourceId == null)
            {
                return null;
            }

            var resourceId = (double)currWo.ResourceId;
            ////获取当前woId对应的单是否生产中列表的最后一个单
            var lastWo = Query<CallMaterialWorkOrder>().Where(p => p.WorkOrder.ResourceId == resourceId
            && (p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing)).OrderByDescending(p => p.Index).FirstOrDefault();
            if (currWo.Id != lastWo?.WorkOrderId)
            {
                return null;
            }

            var set = GetIsAutoCall((double)currWo.ResourceId);
            if (set != null && set.IsAuto)
            {
                var nextWo = GetNextWoForAuto(currWo);
                if (nextWo != null && !nextWo.IsCalled)
                {
                    //判断当前生产工单完工数量是否<=当前产品机型工时*下个产品机型配送周期数量
                    var currModel = currWo.Product.Model;
                    if (currModel == null)
                    {
                        return null;
                    }

                    var currModelLine = RT.Service.Resolve<ProductModelController>().GetProductModelLine(resourceId, currModel.Id);
                    var currHours = (currModelLine == null ? currModel.WorkingHours : currModelLine.WorkingHours);
                    var nextModel = nextWo.WorkOrder.Product.Model;
                    if (nextModel == null || nextModel.SendingHours == null || nextModel.SendingHours == 0)
                    {
                        //没维护配送周期停止叫料                 
                        SetAuto(resourceId, false);
                    }
                    if (nextModel != null)
                    {
                        var qty = currHours * nextModel.SendingHours;
                        if (currWo.FinishQty <= qty)
                        {
                            return nextWo;
                        }
                    }
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// 自动叫料获取下一个叫料工单
        /// </summary>
        /// <param name="currWo">当前叫料工单对应工单</param>
        /// <returns>下一个叫料工单</returns>
        public virtual CallMaterialWorkOrder GetNextWoForAuto(WorkOrder currWo)
        {
            var res = Query<CallMaterialWorkOrder>().Where(p => p.WorkOrder.ResourceId == currWo.ResourceId && p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release
            && p.WorkOrder.IsPause == YesNo.No).OrderBy(p => p.Index).FirstOrDefault();
            return res;
        }

        /// <summary>
        /// 根据工单获取叫料工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>叫料工单</returns>
        public virtual CallMaterialWorkOrder GetCallMaterialWoByWoId(double workOrderId)
        {
            return Query<CallMaterialWorkOrder>().Where(p => p.WorkOrderId == workOrderId).FirstOrDefault();
        }

        /// <summary>
        /// 保存叫料工单报错信息
        /// </summary>
        /// <param name="woCall">叫料工单</param>
        /// <param name="isFail">bool</param>
        /// <param name="reason">原因</param>
        public virtual void SaveFailWoCall(CallMaterialWorkOrder woCall, bool isFail, string reason)
        {
            if (woCall == null)
            {
                return;
            }
            woCall.IsSendedFail = isFail;
            woCall.FailReason = reason;
            RF.Save(woCall);
            if (isFail)
            {
                //自动叫料停止叫料
                SetAuto((double)woCall.WorkOrder.ResourceId, false);
                throw new ValidationException(reason);
            }
        }

        /// <summary>
        /// 停止自动叫料
        /// </summary>
        /// <param name="resourceName">资源编码</param>
        /// <param name="isAuto">是否自动Bool</param>
        public virtual void SetAuto(string resourceName, bool isAuto)
        {
            var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByName(resourceName);
            SetAuto(resource.Id, isAuto);
        }

        /// <summary>
        /// 停止自动叫料
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="isAuto">是否自动Bool</param>
        public virtual void SetAuto(double resourceId, bool isAuto)
        {
            var set = GetIsAutoCall(resourceId);
            set.IsAuto = isAuto;
            RF.Save(set);
        }

        /// <summary>
        /// 获取工单匹配列表
        /// </summary>
        /// <param name="callWo">当前叫料工单</param>
        /// <returns>匹配物料列表</returns>
        public virtual EntityList<CallMatchWorkOrder> GetCallMatchWorkOrder(CallMaterialWorkOrder callWo)
        {
            var nextWo = GetNextCallMaterialWo(callWo);
            if (nextWo == null || callWo == null || callWo.WorkOrder == null)
            {
                return new EntityList<CallMatchWorkOrder>();
            }

            var callBomListItemId = callWo.WorkOrder.ProcessBomList.Select(e => e.ItemId);
            var nextbomList = nextWo.WorkOrder.ProcessBomList.Where(p => callBomListItemId.Contains(p.ItemId)).ToList();
            EntityList<CallMatchWorkOrder> list = new EntityList<CallMatchWorkOrder>();
            var matchItemList = GetCallMatchItems(callWo.Id, nextWo.WorkOrderId);
            var rate = Math.Round(((double)nextbomList.Count / nextWo.WorkOrder.ProcessBomList.Count), 2);

            foreach (var item in nextbomList)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder st = new StringBuilder();
                CallMatchWorkOrder matchWo = new CallMatchWorkOrder();
                matchWo.CallWorkOrderId = callWo.Id;
                matchWo.WorkOrderId = nextWo.WorkOrderId;
                matchWo.MatchRate = rate;
                matchWo.ProcessId = (double)item.ProcessId;
                if (item.ProcessId != null)
                {
                    matchWo.ProcessName = item.Process.Name;
                }

                matchWo.ItemId = item.ItemId;
                matchWo.ItemCode = item.Item?.Code;
                matchWo.ItemName = item.Item?.Name;
                matchWo.ItemUnit = item.Item?.Unit?.Name;
                matchWo.IsChange = false;
                var useItem = matchItemList.FirstOrDefault(p => p.ItemId == item.ItemId);
                if (useItem != null)
                {
                    matchWo.IsUse = true;
                }

                foreach (var alternative in item.AlternativeList)
                {
                    sb.Append(alternative.Item?.Code + ";");
                    st.Append(alternative.ItemName + ";");
                }
                matchWo.AlternativeCode = sb.ToString();
                matchWo.AlternativeName = st.ToString();

                list.Add(matchWo);
            }

            list.SetTotalCount(list.Count);
            return list;
        }

        /// <summary>
        /// 获取叫料工单的物料占比
        /// </summary>
        /// <param name="callWo">叫料工单</param>
        /// <returns>料工单的物料占比</returns>
        public virtual double? GetMatchRate(CallMaterialWorkOrder callWo)
        {
            return GetCallMatchWorkOrder(callWo).FirstOrDefault()?.MatchRate;
        }

        /// <summary>
        /// 获取下个叫料单
        /// </summary>
        /// <param name="currWo">当前叫料单</param>
        /// <returns>下个叫料单</returns>
        public virtual CallMaterialWorkOrder GetNextCallMaterialWo(CallMaterialWorkOrder currWo)
        {
            if (currWo.WorkOrder == null)
            {
                return null;
            }
            var list = Query<CallMaterialWorkOrder>().Where(p => p.WorkOrder.ResourceId == currWo.WorkOrder.ResourceId && p.WorkOrder.IsPause == YesNo.No &&
            (p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing || p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release))
                .OrderByDescending(p => p.WorkOrder.State)
                .OrderBy(p => p.Index).ToList();
            ////先将工单按状态和排序号排序
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == currWo.Id)
                {
                    return i == list.Count - 1 ? null : list[i + 1];
                }
            }

            return null;
        }

        /// <summary>
        /// 新增叫料工单
        /// </summary>
        /// <param name="workOrderId">新增工单Id</param>
        public virtual void InsertCallMaterialWorkOrder(double workOrderId)
        {
            var order = Query<CallMaterialWorkOrder>()
                .Where(p => p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release).OrderByDescending(p => p.Index).FirstOrDefault();
            var index = order == null ? -1 : order.Index;

            var callMaterialWorkOrder = new CallMaterialWorkOrder()
            {
                WorkOrderId = workOrderId,
                IsCalled = false,
                Index = index + 1,
            };

            callMaterialWorkOrder.GenerateId();

            RF.Save(callMaterialWorkOrder);
        }

        /// <summary>
        /// 获取自动手动设置参数
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>CallMaterialSetting</returns>
        public virtual CallMaterialSetting GetIsAutoCall(double resourceId)
        {
            return Query<CallMaterialSetting>().Where(p => p.ResourceId == resourceId).FirstOrDefault();
        }

        /// <summary>
        /// 设置默认参数手动
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        public virtual void CreateMaterialSetting(double resourceId)
        {
            var query = GetIsAutoCall(resourceId);
            if (query == null)
            {
                CallMaterialSetting call = new CallMaterialSetting();
                call.IsAuto = false;
                call.ResourceId = resourceId;
                RF.Save(call);
            }
        }

        /// <summary>
        /// 获取生产中的工单和发放中而且当天在计划时间内的
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>叫料工单列表</returns>
        public virtual EntityList<CallMaterialWorkOrder> GetWorkOrdersByProducting(double resourceId)
        {
            var today = RF.Find<CallMaterialWorkOrder>().GetDbTime().Date;
            return Query<CallMaterialWorkOrder>()
                .Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && y.ResourceId == resourceId && y.IsPause == YesNo.No
                && (y.State == Core.WorkOrders.WorkOrderState.Producing || (y.PlanEndDate >= today && y.PlanBeginDate <= today && y.State == Core.WorkOrders.WorkOrderState.Release)))
                .ToList();
        }

        /// <summary>
        /// 保存查询设置
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        public virtual void SaveQuerySetting(double resourceId)
        {
            var set = Query<CallMaterialQuerySetting>().Where(p => p.EmployeeId == RT.IdentityId).FirstOrDefault();
            if (set != null)
            {
                set.ResourceId = resourceId;
                RF.Save(set);
            }
            else
            {
                CallMaterialQuerySetting cur = new CallMaterialQuerySetting()
                {
                    EmployeeId = RT.IdentityId,
                    ResourceId = resourceId
                };
                RF.Save(cur);
            }
        }

        /// <summary>
        /// 获取用户查询的资源Id
        /// </summary>      
        /// <returns>资源Id</returns>
        public virtual double? GetQuerySetting()
        {
            return Query<CallMaterialQuerySetting>().Where(p => p.EmployeeId == RT.IdentityId).FirstOrDefault()?.ResourceId;
        }

        /// <summary>
        /// 根据资源编码获取叫料模式为自动、且失败的叫料工单
        /// </summary>
        /// <param name="LineId">资源ID</param>
        /// <returns>叫料工单</returns>
        public virtual EntityList<CallMaterialWorkOrder> GetCallMaterialWorkOrders(double LineId)
        {
            var results = new EntityList<CallMaterialWorkOrder>();
            EagerLoadOptions eagerLoad = new EagerLoadOptions();
            eagerLoad.LoadWith(CallMaterialWorkOrder.BillListProperty);
            var querys = Query<CallMaterialWorkOrder>().Where(p => p.IsSendedFail).ToList();
            foreach (var queryItem in querys)
            {
                ////var checkFlag = queryItem.BillList.Where(x => x.Mode == CallMaterialMode.Auto && x.Resource.Code == wipResourceCode).Any();
                var checkFlag = (queryItem.WorkOrder.ResourceId == LineId);
                if (checkFlag)
                {
                    results.Add(queryItem);
                }
            }

            return results;
        }

        /// <summary>
        /// 保存叫料工单上移结果
        /// </summary>
        /// <param name="callMaterialWOs">叫料工单列表</param>
        public virtual void SaveCallMaterialWorkOrderUp(EntityList<CallMaterialWorkOrder> callMaterialWOs)
        {
            if (callMaterialWOs != null && callMaterialWOs.Count == 2)
            {
                var currentWO = callMaterialWOs[0];
                var currentIndex = currentWO.Index;
                var nextWO = callMaterialWOs[1];
                currentWO.Index = nextWO.Index;
                nextWO.Index = currentIndex;
                currentWO.PersistenceStatus = PersistenceStatus.Modified;
                nextWO.PersistenceStatus = PersistenceStatus.Modified;

                RF.Save(callMaterialWOs);
            }
        }
        #endregion

        #region 叫料单 
        /// <summary>
        /// 获取叫料单
        /// </summary>
        /// <param name="no">叫料单号</param>
        /// <returns>叫料单</returns>
        public virtual CallMaterialBill GetCallMaterialBill(string no)
        {
            return Query<CallMaterialBill>().Where(p => p.No == no).FirstOrDefault();
        }

        /// <summary>
        /// 触发下一生产工单叫料
        /// </summary>
        /// <param name="wipFinishedEvent">采集完成</param>
        public virtual void AutoAddCallMaterialBill(WipFinishedEvent wipFinishedEvent)
        {
            if (wipFinishedEvent == null)
            {
                return;
            }
            var wo = wipFinishedEvent.Data.Product.WorkOrder;
            var nextWo = GetNextWoToCallMaterial(wo);
            if (nextWo != null)
            {
                AddCallMaterialBill(nextWo, true);
            }
        }

        /// <summary>
        /// 生成叫料单
        /// </summary>
        /// <param name="woCallId">叫料单Id</param>
        public virtual void AddCallMaterialBill(double woCallId)
        {
            var woCall = RF.GetById<CallMaterialWorkOrder>(woCallId);
            AddCallMaterialBill(woCall);
        }

        /// <summary>
        /// 生成叫料单
        /// </summary>
        /// <param name="woCall">叫料单</param>
        /// <param name="isAuto">自动叫料</param>
        public virtual void AddCallMaterialBill(CallMaterialWorkOrder woCall, bool isAuto = false)
        {
            if (woCall == null)
            {
                return;
            }
            //再查一遍工单，刷新内存缓存的工单数据
            var wo = RF.GetById<WorkOrder>(woCall.WorkOrderId);
            if (wo.IsPause == YesNo.Yes)
            {
                throw new ValidationException("暂停中的工单不能叫料".L10N());
            }
            ////工单没维护工序BOM不停止自动叫料
            if (wo.ProcessBomList.Count == 0)
            {
                string _failReason = "工单没有维护工序BOM信息".L10N();
                SaveFailWoCall(woCall, true, _failReason);
            }

            if (wo.Product.Model == null || wo.Product.Model.SendingHours == null || wo.Product.Model.SendingHours <= 0)
            {
                SaveFailWoCall(woCall, true, "工单产品没有维护产品机型".L10N());
            }

            ////TODO 需求时间考虑跳过下班休息时间段
            DateTime requireTime = RF.Find<WorkOrder>().GetDbTime().AddHours((double)wo.Product.Model.SendingHours.Value);

            if (!wo.ResourceId.HasValue)
            {
                //“SaveFailWoCall”里面会抛出异常，所以这里不用加退出
                SaveFailWoCall(woCall, true, "工单没有维护【资源(产线)】,无法自动叫料".L10N());
            }

            var resourceId = wo.ResourceId.Value;

            EntityList<CallMaterialBill> billList = new EntityList<CallMaterialBill>();
            ////先筛选出工序BOM中的工序，通过工序获取工位生成叫料单主
            var processList = wo.ProcessBomList.Select(p => p.Process).DistinctBy(e => e.Id);
            foreach (var process in processList)
            {
                ////通过工序BOM获取工序，然后到工位表找到该资源和工序对应的所有工位             
                var stations = RT.Service.Resolve<StationController>().GetStations(resourceId, process.Id, null, string.Empty);
                if (stations.Count == 0)
                {
                    SaveFailWoCall(woCall, true, "工序【{0}】没有设置相应的工位".L10nFormat(process.Name));
                }

                var areas = RT.Service.Resolve<StorageController>().GetAreaByStationIds(stations.Select(p => p.Id).ToList());
                foreach (var st in stations)
                {
                    CallMaterialBill bill = new CallMaterialBill()
                    {
                        Status = CallMaterialStatus.Pending,
                        Priority = Priority.Normal,
                        FactoryId = wo.FactoryId,
                        StationId = st.Id,
                        ProcessId = process.Id,
                        StorageAreaId = areas.FirstOrDefault(p => p.StationId == st.Id && p.StorageArea.Type == Storages.StorageAreaType.Input)?.StorageAreaId,
                        ResourceId = resourceId,
                        RequiredTime = requireTime,
                        CallWorkOrderId = woCall.Id,
                        No = GetMaterialBillNo()
                    };
                    #region 叫料单明细生成
                    ////获取回工序对应的所有工序BOM物料，添加到叫料单明细中
                    CreateCallMaterialBill(woCall, wo, process, st, bill);
                    #endregion
                    billList.Add(bill);
                }
            }

            if (billList.Count > 0)
            {
                try
                {
                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        RF.Save(billList);
                        SendToWMS(billList);
                        woCall.IsCalled = true;
                        if (woCall.IsSendedFail)
                        {
                            SaveFailWoCall(woCall, false, string.Empty);
                        }
                        else
                        {
                            RF.Save(woCall);
                        }

                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    if (!isAuto)
                    {
                        throw new ValidationException(ex.Message);
                    }

                    SaveFailWoCall(woCall, true, ex.Message.Substring(0, 500));
                }
            }
        }
        /// <summary>
        /// 获取回工序对应的所有工序BOM物料，添加到叫料单明细中
        /// </summary>
        /// <param name="woCall"></param>
        /// <param name="wo"></param>
        /// <param name="process"></param>
        /// <param name="st"></param>
        /// <param name="bill"></param>
        private void CreateCallMaterialBill(CallMaterialWorkOrder woCall, WorkOrder wo, Process process, Station st, CallMaterialBill bill)
        {
            var processItems = wo.ProcessBomList.Where(p => p.ProcessId == process.Id).AsEntityList();
            EntityList<CallMaterialBillDetail> detailList = new EntityList<CallMaterialBillDetail>();
            int rowNo = 1;
            foreach (var item in processItems)
            {
                var stationItem = RT.Service.Resolve<StationController>().GetStationItem(item.ItemId, st.Id);
                if (stationItem == null)
                {
                    SaveFailWoCall(woCall, true, "工位没有维护工位物料信息：工位【{0}】，物料【{1}】".L10nFormat(st.Name, item.ItemName));
                    continue;
                }

                decimal callQty = 0;
                var planItemQty = wo.PlanQty * item.SingleQty;
                if (stationItem?.Capacity > planItemQty)
                {
                    callQty = planItemQty;
                }
                else
                {
                    callQty = stationItem.Capacity;
                }

                CallMaterialBillDetail billDetail = new CallMaterialBillDetail()
                {
                    ItemId = item.ItemId,
                    CalledQty = callQty,
                    RowNo = rowNo
                };
                detailList.Add(billDetail);
                rowNo++;
            }

            bill.DetailList.AddRange(detailList);
        }

        /// <summary>
        /// 获取叫料单By单号集合
        /// </summary>
        /// <param name="requireNos">单号集合</param>
        /// <returns>列表</returns>
        public virtual EntityList<CallMaterialBill> GetCallMaterialBills(List<string> requireNos)
        {
            return Query<CallMaterialBill>().Where(p => requireNos.Contains(p.No)).ToList();
        }

        /// <summary>
        /// 获取叫料单ByIDs
        /// </summary>
        /// <param name="ids">Ids集合</param>
        /// <returns>列表</returns>
        public virtual EntityList<CallMaterialBill> GetCallMaterialBills(List<double> ids)
        {
            return Query<CallMaterialBill>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取工单叫料单列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>叫料单列表</returns>
        public virtual EntityList<CallMaterialBill> GetCallMaterialBills(double workOrderId)
        {
            return Query<CallMaterialBill>().Where(p => p.CallWorkOrder.WorkOrderId == workOrderId).ToList();
        }

        /// <summary>
        /// 根据资源编码获取叫料模式为自动、其失败的叫料单
        /// </summary>
        /// <param name="wipResourceCode">资源编码(产线)</param>
        /// <returns>叫料单集合</returns>
        public virtual EntityList<CallMaterialBill> GetCallMaterialBills(string wipResourceCode)
        {
            var query = Query<CallMaterialBill>().Where(p => p.CallWorkOrder.IsSendedFail
              && p.Mode == CallMaterialMode.Auto && p.Resource.Code == wipResourceCode).ToList();
            return query;
        }

        /// <summary>
        /// 取消工单的所有叫料单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        public virtual void CancelCallMaterialBills(double workOrderId)
        {
            var bills = GetCallMaterialBills(workOrderId);
            bills.ForEach(bill => CancelCallMaterialBill(bill));
        }

        /// <summary>
        /// 取消叫料
        /// </summary>
        /// <param name="billId">叫料单Id</param>
        public virtual void CancelCallMaterialBill(double billId)
        {
            var bill = GetById<CallMaterialBill>(billId);
            CancelCallMaterialBill(bill);
        }

        /// <summary>
        /// 取消叫料
        /// </summary>
        /// <param name="bill">叫料单</param>
        public virtual void CancelCallMaterialBill(CallMaterialBill bill)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                bill.Status = CallMaterialStatus.Cancel;
                RF.Save(bill);
                OperateCallMaterialBill(bill.No, 0);
                bill.DetailList.OrderBy(p => p.ItemId).ForEach(detail =>
                {
                    StationStorageHelper.ItemStoreChanged(bill.StationId, detail.Bill?.CallWorkOrder?.WorkOrderId, detail.ItemId, 0, 0, -detail.CalledQty);
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 设置紧急
        /// </summary>
        /// <param name="billId">叫料单Id</param>
        public virtual void SetUrgency(double billId)
        {
            var bill = GetById<CallMaterialBill>(billId);
            SetUrgency(bill);
        }

        /// <summary>
        /// 设置紧急
        /// </summary>
        /// <param name="bill">叫料单</param>
        public virtual void SetUrgency(CallMaterialBill bill)
        {
            if (bill == null)
            {
                return;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                bill.Priority = Priority.Urgency;
                RF.Save(bill);
                OperateCallMaterialBill(bill.No, 10, 1);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据叫料工单集合获取叫料单
        /// </summary>
        /// <param name="callWoIds">叫料工单集合</param>
        /// <returns>叫料单</returns>
        public virtual EntityList<CallMaterialBillDetail> GetBillDetail(List<double> callWoIds)
        {
            return Query<CallMaterialBillDetail>().Where(p => callWoIds.Contains(p.Bill.CallWorkOrderId)).ToList();
        }

        /// <summary>
        /// 获取当前需求的叫料单列表 需求时间是当天的单
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>叫料单列表</returns>
        public virtual EntityList<CallMaterialBillDetail> GetCallBills(double resourceId)
        {
            var today = RF.Find<CallMaterialBillDetail>().GetDbTime();
            return Query<CallMaterialBillDetail>().Where(p => p.Bill.RequiredTime > today && p.Bill.RequiredTime < today.AddDays(1)
                        && p.Bill.Status != CallMaterialStatus.Cancel && !p.IsLoaded)
                        .Join<CallMaterialWorkOrder>((x, y) => x.Bill.CallWorkOrderId == y.Id)
                        .Join<WorkOrder>((x, y) => x.Bill.CallWorkOrder.WorkOrderId == y.Id && y.ResourceId == resourceId)
                        .ToList();
        }

        /// <summary>
        /// 获取工序的类型
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns>工序的类型</returns>
        private ProcessType? GetProcessType(double processId)
        {
            ProcessType? prcType = null;
            var curProcess = RF.GetById<Process>(processId);
            prcType = curProcess == null ? throw new EntityNotFoundException(typeof(Process), processId) : curProcess.Type;

            return prcType;
        }

        /// <summary>
        /// 获取工位物料清单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>工位物料清单</returns>
        public virtual IList<StationMateriaViewModel> GetStationMateriaViewModels(double workOrderId, double processId, double stationId, double resourceId)
        {
            var prcType = GetProcessType(processId);
            var query = Query<WorkOrderProcessBom>().Where(p => p.WorkOrderId == workOrderId);
            if (prcType != null && (prcType == ProcessType.Assembly || prcType == ProcessType.BatchAssembly))
            {
                query.Where(p => p.ProcessId == processId);
            }

            query.LeftJoin<StationItem>((o, e) => o.ItemId == e.ItemId && e.StationId == stationId);
            var modelList = query.LeftJoin<LoadItem>((o, e) => o.ItemId == e.ItemId && e.StationId == stationId && e.ResourceId == resourceId && e.Qty > 0)
                .GroupBy<LoadItem, StationItem>((w, l, s) => new { w.ItemId, w.UnitId, s.Capacity, s.Warning })
                .Select<StationItem, LoadItem>((x, y, z) => new
                {
                    ItemId = x.ItemId,
                    UnitId = x.UnitId,
                    Capacity = y.Capacity,
                    AlterValue = y.Warning,
                    RemainQty = z.Qty.SUM()
                }).ToList<StationMateriaViewModel>();

            ProcessStationMaterialViewModels(modelList, workOrderId, stationId, processId, prcType);
            return modelList;
        }

        /// <summary>
        /// 工位物料列表的设置：工位库存、在途数量、物料需求诊断
        /// </summary>
        /// <param name="modelList">StationMateriaViewModel集合</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="prcType">工序类型</param>
        private void ProcessStationMaterialViewModels(IList<StationMateriaViewModel> modelList, double workOrderId, double stationId, double processId, ProcessType? prcType)
        {
            GetCallMateriaDetails(workOrderId, stationId, null, null, null);
            var storageController = RT.Service.Resolve<StationStorageController>();
            foreach (var model in modelList)
            {
                if (!model.ItemId.HasValue)
                {
                    continue;
                }

                var storage = storageController.GetOrCreateStationItemStorage(workOrderId, stationId, model.ItemId.Value);
                //////10.工位库存数量
                model.StockQty = storage.BudgetQty;
                //////20.叫料单在途数量
                model.SendingQty = storage.SendingQty;
                ////30. 物料需求诊断
                model.RequirementDiagnosis = GetRequirementDiagnosis(model.ItemId.Value, stationId, workOrderId, processId, model.StockQty, model.SendingQty, prcType);
            }
        }

        /// <summary>
        /// 获取物料需求诊断
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="stockQty">工位库存数量</param>
        /// <param name="sendingQty">叫料单在途数量</param>
        /// <param name="prcType">工序类型</param>
        /// <returns>物料需求诊断</returns>
        private string GetRequirementDiagnosis(double itemId, double stationId, double workOrderId, double processId, decimal stockQty, decimal sendingQty, ProcessType? prcType)
        {
            string requirementDiagnosis = string.Empty;
            decimal warningQty = 0;
            decimal remainQty = 0;
            decimal diagCriteria = 0; ////诊断标准数量

            var curItem = RF.GetById<Item>(itemId);
            if (curItem == null)
            {
                throw new EntityNotFoundException(typeof(Item), itemId);
            }

            var curStation = RF.GetById<Station>(stationId);
            if (curStation == null)
            {
                throw new EntityNotFoundException(typeof(Station), stationId);
            }

            var curStationItem = curStation.GetStationItemList().FirstOrDefault(x => x.ItemId == itemId);
            warningQty = curStationItem == null
                ? throw new ValidationException("工位[{0}] , 物料[{1}]的工位物料信息不存在! ".L10nFormat(curStation.Code, curItem.Code))
                : curStationItem.Warning;
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItems(curStation.ResourceId, stationId, itemId);
            remainQty = loadItems == null || loadItems.Count <= 0 ? 0 : loadItems.Sum(x => x.Qty);

            if ((prcType != null && (prcType == ProcessType.Assembly || prcType == ProcessType.BatchAssembly)))
            {
                decimal woNeedQty = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNeetItemQty(workOrderId, itemId, processId);
                diagCriteria = woNeedQty > warningQty ? warningQty : woNeedQty; ////Min(woNeedQty , warningQty)
            }
            else
            {  ////非装配且非批次装配 , 诊断标准数量取预警值
                diagCriteria = warningQty;
            }

            if (remainQty >= diagCriteria)
            {
                requirementDiagnosis = "物料充足，无需上料! ".L10N();
            }
            else if (remainQty + stockQty >= diagCriteria)
            {
                requirementDiagnosis = "库存充足，请及时上料! ".L10N();
            }
            else if (remainQty + stockQty + sendingQty >= diagCriteria)
            {
                requirementDiagnosis = "物料不足，请确认在途物料! ".L10N();
            }
            else
            {
                requirementDiagnosis = "物料严重不足! ".L10N();
            }

            return requirementDiagnosis;
        }

        /// <summary>
        /// 获取工位叫料清单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns>叫料单明细</returns>
        public virtual EntityList<CallMaterialBillDetail> GetCallMateriaDetails(double workOrderId, double stationId)
        {
            var query = Query<CallMaterialBillDetail>().Where(p => p.Bill.StationId == stationId && !p.IsLoaded && p.Bill.Status != CallMaterialStatus.Cancel && p.Bill.Status != CallMaterialStatus.Received)
                .Join<CallMaterialWorkOrder>((x, y) => x.Bill.CallWorkOrderId == y.Id && y.WorkOrderId == workOrderId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取叫料单明细集合
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="isLoaded">是否已上料</param>
        /// <param name="status">叫料单状态</param>
        /// <returns>叫料单明细集合</returns>
        public virtual EntityList<CallMaterialBillDetail> GetCallMateriaDetails(double? workOrderId = null, double? stationId = null,
            double? itemId = null, bool? isLoaded = null, CallMaterialStatus? status = null)
        {
            if (workOrderId == null && stationId == null && itemId == null && isLoaded == null && status == null)
            {
                throw new ValidationException("方法[GetCallMateriaDetails]参数不能全部为Null! ".L10N());
            }

            var query = Query<CallMaterialBillDetail>();
            if (workOrderId != null)
            {
                query.Join<CallMaterialWorkOrder>((x, y) => x.Bill.CallWorkOrderId == y.Id && y.WorkOrderId == workOrderId);
            }

            if (stationId != null)
            {
                query.Where(x => x.Bill.StationId == stationId);
            }
            if (itemId != null)
            {
                query.Where(x => x.ItemId == itemId);
            }
            if (isLoaded != null)
            {
                query.Where(x => x.IsLoaded == isLoaded);
            }
            if (status != null)
            {
                query.Where(x => x.Bill.Status == status);
            }

            return query.ToList();
        }

        /// <summary>
        /// 手动叫料
        /// </summary>
        /// <param name="model">叫料模型</param>
        /// <param name="mode">叫料方式</param>
        public virtual void CallMateriaBillInStation(CallMateriaInStationViewModel model, CallMaterialMode mode)
        {
            if (model == null || !model.StationItemList.Any())
            {
                return;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DateTime? requireTime = null;
                requireTime = RF.Find<CallMaterialBill>().GetDbTime().AddHours(model.WorkOrder?.Product?.Model?.SendingHours != null ? (double)model.WorkOrder.Product.Model.SendingHours : 0);

                var callMateriaWo = GetCallMaterialWoByWoId(model.WorkOrderId.Value);
                if (callMateriaWo == null)
                {
                    throw new ValidationException("工单[{0}]不存在对应的叫料工单".L10nFormat(model.WorkOrder.No));
                }
                var bill = new CallMaterialBill
                {
                    No = GetMaterialBillNo(),
                    CallWorkOrderId = callMateriaWo.Id,
                    ResourceId = model.SendtoStation.ResourceId,
                    StationId = model.SendtoStationId.Value,
                    ProcessId = model.ProcessId,
                    Priority = model.Priority,
                    Status = CallMaterialStatus.Pending,
                    Mode = mode,
                    FactoryId = callMateriaWo.WorkOrder.FactoryId,
                    RequiredTime = requireTime.Value,
                    StorageAreaId = RT.Service.Resolve<StorageController>().GetInputAreaByStationId(model.SendtoStationId.Value)?.Id,
                };

                int row = 1;
                model.StationItemList.ForEach(p =>
                {
                    if (p.CallQty > 0)
                    {
                        bill.DetailList.Add(new CallMaterialBillDetail
                        {
                            RowNo = row++,
                            ItemId = p.ItemId.Value,
                            CalledQty = p.CallQty
                        });
                    }
                });

                model.ReasonList.ForEach(p =>
                {
                    bill.ReasonList.Add(new UrgencyCallMeterialReason
                    {
                        ReasonId = p.Id
                    });
                });

                bill.GenerateId();
                RF.Save(bill);
                SendToWMS(new EntityList<CallMaterialBill> { bill });

                tran.Complete();
            }
        }

        /// <summary>
        /// 工位自动叫料 V2.0
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="processId">工序Id</param>
        public virtual void AutoMateriaBillInStation(double workOrderId, double stationId, double processId)
        {
            var model = new CallMateriaInStationViewModel
            {
                WorkOrderId = workOrderId,
                SendtoStationId = stationId,
                ProcessId = processId,
                Priority = Priority.Normal,
            };
            model.MarkSaved();
            var callingItems = new EntityList<StationMateriaViewModel>();
            callingItems.AddRange(model.StationItemList);
            if (callingItems.Count > 0)
            {
                model.StationItemList.Clear();
                model.StationItemList.DeletedList.Clear();
                foreach (var curCallingItem in callingItems)
                {
                    decimal stationCountQty = curCallingItem.RemainQty + curCallingItem.StockQty + curCallingItem.SendingQty;
                    if (stationCountQty >= curCallingItem.AlterValue)  //库存未低于预警值
                    {
                        continue;
                    }
                    var woNeedQty = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNeetItemQty(workOrderId, curCallingItem.ItemId.Value, processId);  //工单需要物料数
                    if (stationCountQty >= woNeedQty)  //工位剩余库存数大于工单剩余产品所需物料数量，不需要叫料
                    {
                        continue;
                    }
                    if (stationCountQty + curCallingItem.Capacity > woNeedQty)
                    {
                        //工位剩余数量+容量大于工单所需数量，则只需要叫足够工单生产数量即可
                        curCallingItem.CallQty = woNeedQty - stationCountQty;
                        model.StationItemList.Add(curCallingItem);
                    }
                    else
                    {
                        curCallingItem.CallQty = curCallingItem.Capacity;
                        model.StationItemList.Add(curCallingItem);
                    }
                }

                CallMateriaBillInStation(model, CallMaterialMode.Auto);
            }
        }


#pragma warning disable CS1572 // XML 注释中有“wipResourceCode”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 获取配送超时的叫料单集合
        /// </summary>
        /// <param name="wipResourceCode">资源编号(产线编号)</param>
        /// <returns>配送超时的叫料单集合</returns>
#pragma warning disable CS1573 // 参数“lineId”在“CallMaterialController.GetDeliveryCallMaterialBills(double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        public virtual EntityList<CallMaterialBill> GetDeliveryCallMaterialBills(double lineId)
#pragma warning restore CS1572 // XML 注释中有“wipResourceCode”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“lineId”在“CallMaterialController.GetDeliveryCallMaterialBills(double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            var nowDate = RF.Find<CallMaterialBill>().GetDbTime();
            EagerLoadOptions eagerLoad = new EagerLoadOptions();
            eagerLoad.LoadWith(CallMaterialBill.DetailListProperty);
            var query = Query<CallMaterialBill>().Where(p => p.ResourceId == lineId &&
                 (p.Status == CallMaterialStatus.Timeout || (p.Status == CallMaterialStatus.Pending && p.RequiredTime < nowDate)));
            return query.ToList(null, eagerLoad);
        }

        /// <summary>
        /// 保存叫料单
        /// </summary>
        public virtual string SaveCallMaterialBill(CallMaterialBill newBill)
        {
            string errMsg = "操作成功";
            if (newBill == null)
            {
                return "保存失败";
            }
            try
            {
                var bill = RF.GetById<CallMaterialBill>(newBill.Id);
                bill.ResourceId = newBill.ResourceId;
                bill.StationId = newBill.StationId;
                bill.ProcessId = newBill.ProcessId;
                RF.Save(bill);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 获取查询条件默认资源
        /// </summary>
        /// <returns>默认资源</returns>
        public virtual WipResource GetDefaultResource()
        {
            var resource = Query<WipResource>().Exists<CallMaterialQuerySetting>((x, y) => y.Where(e => e.ResourceId == x.Id && e.EmployeeId == RT.IdentityId)).FirstOrDefault();

            return resource ?? Query<WipResource>().Exists<EmployeeResource>((x, y) => y.Where(e => e.ResourceId == x.Id && e.EmployeeId == RT.IdentityId))
                .Where(p => p.ResourceState != ResourceState.Diseffect && p.SourceType == SyncSourceType.Enterprise)
                .FirstOrDefault();
        }
        #endregion

        #region 配置项 
        /// <summary>
        /// 获取叫料单单号
        /// </summary>
        /// <returns>叫料单单号</returns>
        public virtual string GetMaterialBillNo()
        {
            var config = ConfigService.GetConfig(new CallMaterialConfig(), typeof(CallMaterialBill));
            if (config == null || !config.NumberRuleId.HasValue)
                throw new ValidationException("未找到叫料单单号配置规则，请配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        /// <param name="resouseId">资源Id</param>
        /// <returns>返回仓库ID</returns>
        public virtual double GetWareHouseId(double resouseId)
        {
            var config = ConfigService.GetConfig(new WareHouseConfig(), typeof(CallMaterialBill), ResourceWarehouse.Find(resouseId));
            if (config == null || config.WarehouseId == 0)
            {
                throw new ValidationException("未找到发料仓库，请配置".L10N());
            }
            return config.WarehouseId;
        }

        /// <summary>
        /// 获取叫料单配置的仓库信息
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>返回Mes配置的发货仓库</returns>
        public virtual Warehouse GetWareHouse(double resourceId)
        {
            Warehouse configWHouse = null;
            var config = ConfigService.GetConfig(new WareHouseConfig(), typeof(CallMaterialBill), ResourceWarehouse.Find(resourceId));
            configWHouse = config == null || config.WarehouseId == 0 ? null : config.Warehouse;
            return configWHouse;
        }
        #endregion

        #region 匹对物料
        /// <summary>
        /// 获取继续使用列表
        /// </summary>
        /// <param name="callWoId">叫料单Id</param>
        /// <param name="woId">匹配工单Id</param>
        /// <returns>匹配物料列表</returns>
        public virtual EntityList<CallMatchItem> GetCallMatchItems(double callWoId, double woId)
        {
            return Query<CallMatchItem>().Where(p => p.CallWorkOrderId == callWoId && p.WorkOrderId == woId).ToList();
        }

        /// <summary>
        /// 获取工单匹配物料列表
        /// </summary>
        /// <param name="workOrderIds">工单ID数组</param>
        /// <returns>工单匹配物料列表</returns>
        public virtual EntityList<CallMatchItem> GetCallMatchItems(double[] workOrderIds)
        {
            return Query<CallMatchItem>()
                 .Join<CallMaterialWorkOrder>((x, y) => x.CallWorkOrderId == y.Id)
                 .Where<CallMaterialWorkOrder>((x, y) => workOrderIds.Contains(y.WorkOrderId))
                 .ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取工单匹配继续使用物料
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>匹配物料列表</returns>
        public virtual EntityList<CallMatchItem> GetCallMatchItems(double workOrderId)
        {
            return Query<CallMatchItem>().Where(p => p.WorkOrderId == workOrderId).ToList();
        }

        /// <summary>
        /// 获取工单匹配物料
        /// </summary>
        /// <param name="callWoId">叫料工单Id</param>
        /// <param name="woId">匹配工单Id</param>
        /// <param name="itemId">物料Id</param>
        /// <returns>CallMatchItem</returns>
        public virtual CallMatchItem GetCallMatchItem(double callWoId, double woId, double itemId)
        {
            return Query<CallMatchItem>().Where(p => p.CallWorkOrderId == callWoId && p.WorkOrderId == woId && p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 保存操作过的工单匹配物料
        /// </summary>
        /// <param name="matchWo">匹配工单集合</param>        
        public virtual void SaveUseCallMatchItem(EntityList<CallMatchWorkOrder> matchWo)
        {
            var callWoId = matchWo[0].CallWorkOrderId;
            var nextWo = GetNextCallMaterialWo(matchWo[0].CallWorkOrder);
            if (nextWo == null)
            {
                return;
            }
            var matchWoId = nextWo.WorkOrderId;
            var itemList = GetCallMatchItems(matchWo[0].CallWorkOrderId, matchWoId);
            var newList = matchWo.Where(p => !itemList.Select(e => e.ItemId).Contains(p.ItemId) && p.IsUse).AsEntityList();
            var oldList = itemList.Where(p => matchWo.Where(e => !e.IsUse).Select(e => e.ItemId).Contains(p.ItemId)).AsEntityList();
            EntityList<CallMatchItem> listSave = new EntityList<CallMatchItem>();
            foreach (var item in newList)
            {
                CallMatchItem m = new CallMatchItem();
                m.CallWorkOrderId = callWoId;
                m.WorkOrderId = matchWoId;
                m.ItemId = item.ItemId;
                listSave.Add(m);
            }

            if (oldList.Count > 0)
            {
                var delIds = oldList.Select(e => e.Id).ToList();
                DB.Delete<CallMatchItem>().Where(p => delIds.Contains(p.Id)).Execute();
            }

            if (listSave.Count > 0)
            {
                RF.Save(listSave);
            }
        }
        #endregion

        #region 排序方案 
        /// <summary>
        /// 查询排序方案设置
        /// </summary>
        /// <param name="isDefalt">是否默认</param>
        /// <returns>排序方案设置列表</returns>
        public virtual EntityList<SortSolutionsSetting> GetSortSolutionsSettings(bool? isDefalt = null)
        {
            var query = Query<SortSolutionsSetting>();
            if (isDefalt.HasValue)
                query.Where(p => p.IsDefault == isDefalt.Value);
            query.OrderByDescending(p => p.IsDefault);
            return query.ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取排序优先级设置列表
        /// </summary>
        /// <param name="solSettingIds">排序方案Id列表</param>
        /// <returns>排序优先级设置列表</returns>
        public virtual EntityList<PrioritySetting> GetPrioritySettings(List<double> solSettingIds)
        {
            return Query<PrioritySetting>().Where(p => solSettingIds.Contains(p.SolutionsId)).ToList();
        }

        /// <summary>
        /// 根据某排序方案Id获取排序优先级列表
        /// </summary>
        /// <param name="solutionSetId">排序方案Id</param>
        /// <returns>排序优先级列表</returns>
        public virtual EntityList<PrioritySetting> GetPrioritySettings(double solutionSetId)
        {
            return Query<PrioritySetting>().Where(p => p.SolutionsId == solutionSetId).ToList();
        }

        /// <summary>
        /// 获取默认排序方案
        /// </summary>
        /// <returns>默认排序方案</returns>
        public virtual SortSolutionsSetting GetDefaultSortSolSetting()
        {
            var query = Query<SortSolutionsSetting>().Where(p => p.IsDefault);
            var defaultSetting = query.FirstOrDefault();
            if (defaultSetting == null)
            {
                throw new ValidationException("不存在默认排序方案设置，请维护默认排序方案设置！".L10N());
            }
            return defaultSetting;
        }

        /// <summary>
        /// 获取默认排序方案
        /// </summary>        
        public virtual void ExcuteSortSolution()
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                var defaultSetting = GetDefaultSortSolSetting();

                var priorityList = defaultSetting.PriorityList.OrderBy(p => p.Priority).ToList();
                var latestProducingOrder = Query<CallMaterialWorkOrder>()
                    .Where(p => p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing).OrderByDescending(p => p.Index).FirstOrDefault();
                var orderList = Query<CallMaterialWorkOrder>()
                    .Where(p => p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release).ToList(null, new EagerLoadOptions().LoadWith(CallMaterialWorkOrder.WorkOrderProperty));

                priorityList.ForEach(p =>
                {
                    orderList = SortByPrioritySetting(p, orderList);
                });

                int i = latestProducingOrder == null ? 0 : latestProducingOrder.Index + 1;
                orderList.ForEach(p =>
                {
                    p.Index = i++;
                });

                RF.Save(orderList);
            }
        }

        /// <summary>
        /// 根据排序优先级排序
        /// </summary>
        /// <param name="prioritySetting">排序优先级</param>
        /// <param name="orderList">待排序列表</param>
        /// <returns>叫料工单列表</returns>
        private EntityList<CallMaterialWorkOrder> SortByPrioritySetting(PrioritySetting prioritySetting, EntityList<CallMaterialWorkOrder> orderList)
        {
            EntityList<CallMaterialWorkOrder> sortedOrderList = new EntityList<CallMaterialWorkOrder>();
            if (prioritySetting.SortMode == SortMode.Asc)
            {
                switch (prioritySetting.SortPropertyName)
                {
                    case "PlanQty":
                        sortedOrderList.AddRange(orderList.OrderBy(q => q.WorkOrder.PlanQty));
                        break;
                    case "ActuStartDate":
                        sortedOrderList.AddRange(orderList.OrderBy(q => q.WorkOrder.ActuStartDate));
                        break;
                    case "PlanBeginDate":
                        sortedOrderList.AddRange(orderList.OrderBy(q => q.WorkOrder.PlanBeginDate));
                        break;
                    default:
                        break;
                }

            }
            else if (prioritySetting.SortMode == SortMode.Desc)
            {
                switch (prioritySetting.SortPropertyName)
                {
                    case "PlanQty":
                        sortedOrderList.AddRange(orderList.OrderByDescending(q => q.WorkOrder.PlanQty));
                        break;
                    case "ActuStartDate":
                        sortedOrderList.AddRange(orderList.OrderByDescending(q => q.WorkOrder.ActuStartDate));
                        break;
                    case "PlanBeginDate":
                        sortedOrderList.AddRange(orderList.OrderByDescending(q => q.WorkOrder.PlanBeginDate));
                        break;
                    default:
                        break;
                }

            }
            else
            {
                //
            }

            return sortedOrderList;
        }

        /// <summary>
        /// 工单上线时重新排序
        /// </summary>
        /// <param name="workOrderId">上线工单Id</param>
        public virtual void ReSortInStartProducing(double workOrderId)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                var orderList = Query<CallMaterialWorkOrder>()
                .Where(p => p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release || p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing).ToList(null, new EagerLoadOptions().LoadWith(CallMaterialWorkOrder.WorkOrderProperty));
                var orderGroups = orderList.GroupBy(p => p.WorkOrder.State).OrderByDescending(p => p.Key);

                var producingOrders = orderGroups.FirstOrDefault(p => p.Key == Core.WorkOrders.WorkOrderState.Producing)?.OrderByDescending(p => p.Index);
                if (producingOrders == null)
                {
                    var releaseOrders = orderGroups.FirstOrDefault(p => p.Key == Core.WorkOrders.WorkOrderState.Release);
                    releaseOrders?.ForEach(p => { p.Index += 1; });
                }
                else
                {
                    var order = producingOrders.FirstOrDefault(p => p.WorkOrderId != workOrderId);
                    var index = order == null ? -1 : order.Index + 1;
                    var currentCMW = producingOrders.FirstOrDefault(p => p.WorkOrderId == workOrderId);

                    if (currentCMW != null && currentCMW.Index != index)
                    {
                        currentCMW.Index = index;
                        var releaseOrders = orderGroups.FirstOrDefault(p => p.Key == Core.WorkOrders.WorkOrderState.Release);
                        releaseOrders?.ForEach(p => { p.Index += 1; });
                    }
                }

                RF.Save(orderList);
            }
        }

        /// <summary>
        /// 设置缺省排序方案
        /// </summary>
        /// <param name="solutionId">排序方案ID</param>
        public virtual void SetDefaultSortSolution(double solutionId)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<SortSolutionsSetting>().Set(p => p.IsDefault, true).Where(p => p.Id == solutionId).Execute();
                DB.Update<SortSolutionsSetting>().Set(p => p.IsDefault, false).Where(p => p.Id != solutionId).Execute();
                tran.Complete();
            }
        }
        #endregion

        #region 物料接收&退料 
        /// <summary>
        /// 获取物料标签接收信息
        /// </summary>
        /// <param name="label">物料标签号</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>物料标签接收信息</returns>
        public virtual CallMaterialReceive GetCallMaterialReceive(string label = null, double? resourceId = null)
        {
            var query = Query<CallMaterialReceive>();
            if (!string.IsNullOrEmpty(label))
            {
                query.Where(x => x.Label == label);
            }
            if (resourceId != null)
            {
                query.Where(x => x.ResourceId == resourceId);
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取物料标签接收信息
        /// </summary>
        /// <param name="label">物料标签号</param> 
        /// <returns>物料标签接收信息</returns>
        public virtual CallMaterialReceive GetCallMaterialReceive(string label)
        {
            return Query<CallMaterialReceive>().Where(x => x.Label == label).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料标签接收信息
        /// </summary>
        /// <param name="label">物料标签号</param> 
        /// <param name="resourceId">资源Id</param>
        /// <returns>物料标签接收信息</returns>
        public virtual CallMaterialReceive GetCallMaterialReceive(string label, double resourceId)
        {
            return Query<CallMaterialReceive>().Where(x => x.Label == label && x.ResourceId == resourceId).OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 物料是否已接收
        /// </summary>
        /// <param name="label">物料标签号</param> 
        /// <param name="resourceId">资源Id</param>
        /// <returns>已接收返回true，否则返回false</returns>
        public virtual bool IsCallMaterialReceive(string label, double resourceId)
        {
            return Query<CallMaterialReceive>().Where(x => x.Label == label && x.ResourceId == resourceId).Count() > 0;
        }

        /// <summary>
        /// 工位物料虚退
        /// </summary>
        /// <param name="workOrderId">当前在产工单</param>
        /// <param name="resourceId">资源ID</param>        
        /// <param name="stationId">工位ID</param>
        public virtual void VirtualStoresReturned(double workOrderId, double resourceId, double stationId)
        {
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(resourceId, stationId); //过滤掉单体/半成品上料记录保证上料有工单记录，进行匹对，实际上单体/半成品是不会提前上料的
            var matchItems = GetCallMatchItems(workOrderId);
            var items = matchItems.Select(p => p.ItemId).ToArray();
            var returnItems = loadItems.Where(p => items.Contains(p.ItemId)).AsEntityList();
            returnItems.ForEach(e => e.WorkOrderId = workOrderId);
            RF.Save(returnItems);
        }

        /// <summary>
        /// 获取物料接收集合
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="distStationId">配送工位Id</param>
        /// <param name="loadStationId">上料工位Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="callMaterialBillId">叫料单Id</param>
        /// <param name="isLoadItem">是否已上料(true: 已上料; false:未上料)</param>
        /// <returns>物料接收集合</returns>
        public virtual EntityList<CallMaterialReceive> GetCallMaterialReceives(double? itemId = null, double? distStationId = null, double? loadStationId = null, double? resourceId = null, double? workOrderId = null, double? callMaterialBillId = null, bool? isLoadItem = null)
        {
            if (itemId == null && distStationId == null && loadStationId == null && resourceId == null && workOrderId == null && callMaterialBillId == null && isLoadItem == null)
            {
                throw new ValidationException("方法GetCallMaterialReceives的参数不能同时Null !".L10nFormat());
            }
            var query = Query<CallMaterialReceive>();
            if (itemId != null)
            {
                query.Where(x => x.ItemId == itemId);
            }
            if (distStationId != null)
            {
                query.Where(x => x.DistStationId == distStationId);
            }
            if (loadStationId != null)
            {
                query.Where(x => x.LoadStationId == loadStationId);
            }
            if (resourceId != null)
            {
                query.Where(x => x.ResourceId == resourceId);
            }
            if (workOrderId != null)
            {
                query.Where(x => x.WorkOrderId == workOrderId);
            }
            if (callMaterialBillId != null)
            {
                query.Where(x => x.CallMaterialBillId == callMaterialBillId);
            }
            if (isLoadItem != null)
            {
                query.Where(x => x.IsLoadItem == isLoadItem);
            }

            return query.ToList();
        }
        /// <summary>
        /// 获取物料接收集合
        /// </summary>
        /// <param name="receiveInfo">物料接收查询信息</param> 
        /// <returns>物料接收集合</returns>
        public virtual EntityList<CallMaterialReceive> GetCallMaterialReceives(ReceiveQueryInfo receiveInfo)
        {
            var query = Query<CallMaterialReceive>();
            if (receiveInfo.ItemId.HasValue)
            {
                query.Where(x => x.ItemId == receiveInfo.ItemId);
            }
            if (receiveInfo.DistStationId.HasValue)
            {
                query.Where(x => x.DistStationId == receiveInfo.DistStationId);
            }
            if (receiveInfo.LoadStationId.HasValue)
            {
                query.Where(x => x.LoadStationId == receiveInfo.LoadStationId);
            }
            if (receiveInfo.ResourceId.HasValue)
            {
                query.Where(x => x.ResourceId == receiveInfo.ResourceId);
            }
            if (receiveInfo.WorkOrderId.HasValue)
            {
                query.Where(x => x.WorkOrderId == receiveInfo.WorkOrderId);
            }
            if (receiveInfo.callMaterialBillId.HasValue)
            {
                query.Where(x => x.CallMaterialBillId == receiveInfo.callMaterialBillId);
            }
            if (receiveInfo.IsLoadItem.HasValue)
            {
                query.Where(x => x.IsLoadItem == receiveInfo.IsLoadItem);
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取物料接收信息
        /// </summary>
        /// <param name="woIds">工单集合</param>
        /// <returns>物料接收信息</returns>
        public virtual EntityList<CallMaterialReceive> GetCallMaterialReceives(List<double> woIds)
        {
            return Query<CallMaterialReceive>().Where(p => woIds.Contains(p.WorkOrderId) && p.IsLoadItem).ToList();
        }


        /// <summary>
        /// 获取物料标签退料信息
        /// </summary>
        /// <param name="label">物料标签</param>
        /// <param name="curDateTime">退料日期</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>物料标签退料信息</returns>
        public virtual EntityList<CallMaterialWithdrawal> GetCallMaterialWithdrawals(string label = null, DateTime? curDateTime = null, double? resourceId = null)
        {
            var querys = Query<CallMaterialWithdrawal>();
            if (!string.IsNullOrEmpty(label))
            {
                querys.Where(x => x.ItemLabel == label);
            }
            if (curDateTime != null)
            {
                var curDate = ((DateTime)curDateTime).Date;
                querys.Where(x => x.WithdrawalDate >= curDate && x.WithdrawalDate < curDate.AddDays(1));
            }

            if (resourceId != null)
            {
                querys.Where(x => x.ResourceId == resourceId);
            }
            return querys.ToList();
        }

        /// <summary>
        /// 标签是否已退料
        /// </summary>
        /// <param name="label">标签号</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns>已退料返回true，未退料返回false</returns>
        public virtual bool IsCallMaterialWithdrawal(string label, double resourceId)
        {
            return Query<CallMaterialWithdrawal>().Where(p => p.ItemLabel == label && p.ResourceId == resourceId).Count() > 0;
        }

        ///// <summary>
        ///// Check物料标签是否有退料记录
        ///// </summary>
        ///// <param name="label">物料标签号</param>
        ///// <param name="date">退料日期</param>
        ///// <param name="resourceId">资源Id</param>
        ///// <returns>true : 存在退料记录; false : 不存在退料记录</returns>
        ////public virtual bool CheckMaterialWithdrawalExists(string label = null, DateTime? date = null, double? resourceId = null)
        ////{
        ////    bool checkFlag = false;
        ////    var querys = GetCallMaterialWithdrawals(label, date, resourceId);
        ////    if (querys != null && querys.Count > 0)
        ////        checkFlag = true;
        ////    else
        ////        checkFlag = false;
        ////    return checkFlag;
        ////}

        #region 物料标签退料(获取可退料信息、提交退料、获取已退料信息)

        #region 获取可退料物料标签信息
        /// <summary>
        /// 获取可退料物料标签信息
        /// 数据源: 已接收未上料的物料标签接收信息
        /// </summary>
        /// <param name="label">物料标签</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>可退料物料标签信息</returns>
        WithdrawalableItemLabelInfo GetWithdrawalInfo(string label, double resourceId)
        {
            var receive = GetCallMaterialReceive(label, resourceId);
            if (receive != null && !receive.IsLoadItem)
            {
                return CreateWithdrawalInfo(receive);
            }
            return null;
        }

        /// <summary>
        /// 获取可退料物料标签信息
        /// 数据源: 已下料 从工位下料明细中
        /// </summary>
        /// <param name="label">物料标签</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="sourceType">物料来源类型</param>
        /// <returns>可退料物料信息</returns>
        WithdrawalableItemLabelInfo GetWithdrawalInfo(string label, double resourceId, LoadItemSourceType sourceType)
        {
            var unloadItems = RT.Service.Resolve<LoadItemController>().GetUnloadItems(label, resourceId, sourceType);
            var receive = GetCallMaterialReceive(label, resourceId);
            return CreateWithdrawalInfo(unloadItems, receive);
        }

        /// <summary>
        /// 创建可退料物料标签信息
        /// </summary>
        /// <param name="receive">物料标签接收信息</param>
        /// <returns>可退料物料标签信息</returns>
        private WithdrawalableItemLabelInfo CreateWithdrawalInfo(CallMaterialReceive receive)
        {
            var warehouse = GetWareHouse(receive.ResourceId);
            var curWithdrawalableInfo = new WithdrawalableItemLabelInfo();
            curWithdrawalableInfo.Label = receive.Label;
            curWithdrawalableInfo.ItemId = receive.ItemId;
            curWithdrawalableInfo.ItemCode = receive.Item?.Code;
            curWithdrawalableInfo.ItemName = receive.Item?.Name;
            curWithdrawalableInfo.SpecificationModel = receive.Item?.SpecificationModel;
            curWithdrawalableInfo.UnitName = receive.Item?.Unit?.Name;

            curWithdrawalableInfo.RemainQty = receive.Qty;
            curWithdrawalableInfo.WithdrawalQty = receive.Qty;
            if (warehouse != null)
            {
                curWithdrawalableInfo.ReceiveWarehouseId = warehouse.Id; ////默认取发运单发货仓库
                curWithdrawalableInfo.ReceiveWarehouseName = warehouse.Name;
            }

            curWithdrawalableInfo.WorkOrderId = receive.WorkOrderId;
            curWithdrawalableInfo.WorkOrderNo = receive.WorkOrder?.No;

            curWithdrawalableInfo.BatchNo = receive.BatchNo;
            if (receive.LoadStationId == null)
            {
                curWithdrawalableInfo.LoadStationId = receive.DistStationId;
                curWithdrawalableInfo.LoadStationName = receive.DistStation?.Name;
            }
            else
            {
                curWithdrawalableInfo.LoadStationId = (double)receive.LoadStationId;
                curWithdrawalableInfo.LoadStationName = receive.LoadStation?.Name;
            }

            curWithdrawalableInfo.ResourceId = receive.ResourceId;
            curWithdrawalableInfo.ResourceName = receive.Resource?.Name;

            return curWithdrawalableInfo;
        }

        /// <summary>
        /// 创建可退料物料标签信息
        /// </summary>
        /// <param name="unloadItems">下料信息集合</param>
        /// <param name="receive">物料接收信息</param>
        /// <returns>可退料物料标签信息</returns>
        private WithdrawalableItemLabelInfo CreateWithdrawalInfo(EntityList<UnloadItem> unloadItems, CallMaterialReceive receive)
        {
            if (receive == null)
            {
                return null;
            }
            var warehouse = GetWareHouse(receive.ResourceId);
            var firstUnloadItem = unloadItems.FirstOrDefault();
            var curWithdrawalableInfo = new WithdrawalableItemLabelInfo();

            curWithdrawalableInfo.Label = firstUnloadItem.SourceCode;
            curWithdrawalableInfo.ItemId = firstUnloadItem.ItemId;
            curWithdrawalableInfo.ItemCode = firstUnloadItem.Item?.Code;
            curWithdrawalableInfo.ItemName = firstUnloadItem.Item?.Name;
            curWithdrawalableInfo.SpecificationModel = firstUnloadItem.Item?.SpecificationModel;
            curWithdrawalableInfo.UnitName = firstUnloadItem.Item?.Unit?.Name;

            curWithdrawalableInfo.RemainQty = unloadItems.Sum(x => x.RemainderQty);
            curWithdrawalableInfo.WithdrawalQty = unloadItems.Sum(x => x.RemainderQty); ////或取"LoadItem.UnloadQty"
            if (warehouse != null)
            {
                curWithdrawalableInfo.ReceiveWarehouseId = warehouse.Id; ////默认取发运单发货仓库
                curWithdrawalableInfo.ReceiveWarehouseName = warehouse.Name;
            }

            curWithdrawalableInfo.WorkOrderId = (double)firstUnloadItem.WorkOrderId;
            curWithdrawalableInfo.WorkOrderNo = firstUnloadItem.WorkOrder?.No;

            curWithdrawalableInfo.BatchNo = receive.BatchNo;
            curWithdrawalableInfo.LoadStationId = firstUnloadItem.StationId;
            curWithdrawalableInfo.LoadStationName = firstUnloadItem.Station?.Name;
            curWithdrawalableInfo.ResourceId = firstUnloadItem.ResourceId;
            curWithdrawalableInfo.ResourceName = firstUnloadItem.Resource?.Name;

            return curWithdrawalableInfo;
        }

        #endregion 获取可退料物料标签信息

        #region 获取已退料物料标签信息
        /// <summary>
        /// 获取已退料物料标签信息
        /// </summary>
        /// <param name="label">物料标签</param>
        /// <param name="date">退料日期</param>
        /// <returns>已退料物料信息集合</returns>
        public virtual List<WithdrawaledItemLabelInfo> GetWithdrawaledInfos(string label = null, DateTime? date = null)
        {
            List<WithdrawaledItemLabelInfo> withdrawaledlInfos = null;
            var callMaterialWithdrawals = GetCallMaterialWithdrawals(label, date, null);
            if (callMaterialWithdrawals != null && callMaterialWithdrawals.Count > 0)
            {
                withdrawaledlInfos = CreateWithdrawaledInfos(callMaterialWithdrawals);
            }

            return withdrawaledlInfos;
        }

        /// <summary>
        /// 创建已退料物料标签信息
        /// </summary>
        /// <param name="callMaterialWithdrawals">物料退料集合</param>
        /// <returns>已退料物料标签信息</returns>
        private List<WithdrawaledItemLabelInfo> CreateWithdrawaledInfos(EntityList<CallMaterialWithdrawal> callMaterialWithdrawals)
        {
            List<WithdrawaledItemLabelInfo> withdrawaledItemLabelInfos = new List<WithdrawaledItemLabelInfo>();
            foreach (var curItem in callMaterialWithdrawals)
            {
                var curWithdrawaledInfo = new WithdrawaledItemLabelInfo();

                curWithdrawaledInfo.Label = curItem.ItemLabel;
                curWithdrawaledInfo.ItemId = curItem.ItemId;
                curWithdrawaledInfo.ItemCode = curItem.Item?.Code;
                curWithdrawaledInfo.ItemName = curItem.Item?.Name;
                curWithdrawaledInfo.SpecificationModel = curItem.Item?.SpecificationModel;
                curWithdrawaledInfo.UnitName = curItem.Item?.Unit?.Name;

                curWithdrawaledInfo.RemainQty = curItem.RemainQty;
                curWithdrawaledInfo.WithdrawalQty = curItem.WithdrawalQty;
                curWithdrawaledInfo.ReceiveWarehouseId = curItem.ReceiveWarehouseId; ////默认取发运单发货仓库
                curWithdrawaledInfo.ReceiveWarehouseName = curItem.ReceiveWarehouse.Name;
                curWithdrawaledInfo.WorkOrderId = curItem.WorkOrderId;
                curWithdrawaledInfo.WorkOrderNo = curItem.WorkOrder?.No;

                curWithdrawaledInfo.BatchNo = curItem.BatchNo;
                curWithdrawaledInfo.LoadStationId = curItem.LoadStationId;
                curWithdrawaledInfo.LoadStationName = curItem.LoadStation?.Name;
                curWithdrawaledInfo.ResourceId = curItem.ResourceId;
                curWithdrawaledInfo.ResourceName = curItem.Resource?.Name;
                curWithdrawaledInfo.WithdrawalById = curItem.WithdrawalById;
                curWithdrawaledInfo.WithdrawalByName = curItem.WithdrawalBy?.Name;
                curWithdrawaledInfo.WithdrawalDate = curItem.WithdrawalDate.ToString();

                withdrawaledItemLabelInfos.Add(curWithdrawaledInfo);
            }

            return withdrawaledItemLabelInfos;
        }
        #endregion 获取已退料物料标签信息

        #region 物料标签退料提交
        /// <summary>
        /// 保存物料标签退料数据
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">可退料物料信息集合</param>
        public virtual void SaveWithdrawalableInfos(List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            CheckSubmitWithdrawalableInfos(withdrawalableItemLabelInfos);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                WithdrawalCreateCallMaterialWithdrawals(withdrawalableItemLabelInfos);
                WithdrawalUpdateCallMaterialReceives(withdrawalableItemLabelInfos); ////退料更新物料接收的剩余可用数量
                WithdrawalUpdateUnloadItems(withdrawalableItemLabelInfos);
                tran.Complete();
            }
        }

        /// <summary>
        /// 检查物料标签退料的数据合法性
        /// 如果物料退料数据不合法，则抛异常
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">可退料物料标签信息</param>
        private void CheckSubmitWithdrawalableInfos(List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            ////1.操作人是否存在
            var employee = RF.GetById<Employee>(RT.IdentityId);
            if (employee == null)
            {
                throw new ValidationException("操作人员: {0}不存在!".L10nFormat(RT.Identity.Name));
            }

            foreach (var curWithdrawalable in withdrawalableItemLabelInfos)
            {
                string label = curWithdrawalable.Label;
                ////10.物料标签是否存在
                ////CheckItemLabelExistException(curWithdrawalable.Label);
                var receive = GetCallMaterialReceive(curWithdrawalable.Label);
                if (receive == null)
                {
                    throw new ValidationException("不能退料,物料标签: {0}没有做接收!".L10nFormat(label));
                }
                ////20. 物料是否存在
                var item = RF.GetById<Item>(curWithdrawalable.ItemId);
                if (item == null)
                {
                    throw new ValidationException("物料标签: {0} 的物料: {1}不存在!".L10nFormat(curWithdrawalable.Label, curWithdrawalable.ItemName));
                }
                ////30.工单是否存在
                var wo = RF.GetById<WorkOrder>(curWithdrawalable.WorkOrderId);
                if (wo == null)
                {
                    throw new ValidationException("物料标签: {0} 的工单: {1}不存在!".L10nFormat(curWithdrawalable.Label, curWithdrawalable.WorkOrderNo));
                }
                ////40.上料工位是否存在
                var station = RF.GetById<Station>(curWithdrawalable.LoadStationId);
                if (station == null)
                {
                    throw new ValidationException("物料标签: {0} 的工位: {1}不存在!".L10nFormat(curWithdrawalable.Label, curWithdrawalable.LoadStationName));
                }
                ////50.所属资源是否存在
                var resource = RF.GetById<WipResource>(curWithdrawalable.ResourceId);
                if (resource == null)
                {
                    throw new ValidationException("物料标签: {0} 的生产资源: {1}不存在!".L10nFormat(curWithdrawalable.Label, curWithdrawalable.ResourceName));
                }
                ////60.退料数量 <= 剩余数量
                if (curWithdrawalable.WithdrawalQty > curWithdrawalable.RemainQty)
                {
                    throw new ValidationException("物料标签: {0} 退料数量不能大于剩余数量!".L10nFormat(curWithdrawalable.Label, curWithdrawalable.Label));
                }
            }
        }

        /// <summary>
        /// 退料时保存物料退料实体
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">可退料物料信息</param>
        private void WithdrawalCreateCallMaterialWithdrawals(List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            var nowDate = RF.Find<CallMaterialWithdrawal>().GetDbTime();
            EntityList<CallMaterialWithdrawal> callMaterialWithdrawals = new EntityList<CallMaterialWithdrawal>();
            foreach (var curWithdrawalable in withdrawalableItemLabelInfos.OrderBy(p => p.ItemId))
            {
                var curCallMaterialWithdrawal = new CallMaterialWithdrawal();
                curCallMaterialWithdrawal.ItemLabel = curWithdrawalable.Label;
                curCallMaterialWithdrawal.RemainQty = curWithdrawalable.RemainQty;
                curCallMaterialWithdrawal.WithdrawalQty = curWithdrawalable.WithdrawalQty;
                curCallMaterialWithdrawal.BatchNo = curWithdrawalable.BatchNo;
                curCallMaterialWithdrawal.WithdrawalDate = nowDate;
                curCallMaterialWithdrawal.WithdrawalById = RT.IdentityId;
                curCallMaterialWithdrawal.ItemId = curWithdrawalable.ItemId;
                curCallMaterialWithdrawal.LoadStationId = curWithdrawalable.LoadStationId;
                curCallMaterialWithdrawal.ResourceId = curWithdrawalable.ResourceId;
                curCallMaterialWithdrawal.WorkOrderId = curWithdrawalable.WorkOrderId;
                curCallMaterialWithdrawal.ReceiveWarehouseId = curWithdrawalable.ReceiveWarehouseId;
                curCallMaterialWithdrawal.WorkShopId = RF.GetById<WipResource>(curWithdrawalable.ResourceId)?.WorkShopId;
                callMaterialWithdrawals.Add(curCallMaterialWithdrawal);
                UpdateStationStorage(curWithdrawalable);
            }

            RF.Save(callMaterialWithdrawals);
        }

        /// <summary>
        /// 退料更新工位库存
        /// </summary>
        /// <param name="curWithdrawalable">退料标签信息</param>
        private void UpdateStationStorage(WithdrawalableItemLabelInfo curWithdrawalable)
        {
            bool isLoadItem = RT.Service.Resolve<LoadItemController>().IsUnloadItem(curWithdrawalable.Label, LoadItemSourceType.ItemLabel);
            //下料做退料更新实际库存、接收未上料退料更新预库存
            decimal actStoreQty = 0;
            decimal budgetQty = 0;
            if (isLoadItem)
            {
                actStoreQty = -curWithdrawalable.WithdrawalQty;
            }
            else
            {
                budgetQty = -curWithdrawalable.WithdrawalQty;
            }

            StationStorageHelper.ItemStoreChanged(curWithdrawalable.LoadStationId, curWithdrawalable.WorkOrderId, curWithdrawalable.ItemId, actStoreQty, budgetQty, 0);
        }

        /// <summary>
        /// 退料更新物料接收实体
        /// 更新"剩余可用数量"
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">可退料物料信息</param>
        private void WithdrawalUpdateCallMaterialReceives(List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            EntityList<CallMaterialReceive> callMaterialReceives = new EntityList<CallMaterialReceive>();
            foreach (var curWithdrawalable in withdrawalableItemLabelInfos)
            {
                var curCallMaterialReceive = GetCallMaterialReceive(curWithdrawalable.Label);
                if (curCallMaterialReceive != null)
                {
                    curCallMaterialReceive.RemainQty -= curWithdrawalable.WithdrawalQty;
                    callMaterialReceives.Add(curCallMaterialReceive);
                }
            }

            RF.Save(callMaterialReceives);
        }

        /// <summary>
        /// 退料更新物料下料明细实体
        /// 删除"下料明细"
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos">可退料物料信息</param>
        private void WithdrawalUpdateUnloadItems(List<WithdrawalableItemLabelInfo> withdrawalableItemLabelInfos)
        {
            EntityList<UnloadItem> unloadItemList = new EntityList<UnloadItem>();
            const LoadItemSourceType sourceType = LoadItemSourceType.ItemLabel;

            foreach (var curWithdrawalable in withdrawalableItemLabelInfos)
            {
                var unloadItems = RT.Service.Resolve<LoadItemController>().GetUnloadItems(curWithdrawalable.Label, curWithdrawalable.ResourceId, sourceType);
                if (unloadItems != null && unloadItems.Count > 0)
                {
                    foreach (var curUnloadItem in unloadItems)
                    {
                        curUnloadItem.PersistenceStatus = PersistenceStatus.Deleted;
                    }

                    unloadItemList.AddRange(unloadItems);
                }
            }

            RF.Save(unloadItemList);
        }
        #endregion 物料标签退料提交
        #endregion 物料标签退料(获取可退料标签信息、提交退料、获取已退料标签信息)
        #endregion

        #region 预警
        /// <summary>
        /// 获取预警配置的预警信息
        /// </summary>
        /// <param name="alerterCode">预警编码</param>
        /// <returns>预警配置的预警信息</returns>
        public virtual EntityList<Alerter> GetAlerters(string alerterCode = null)
        {
            var querys = Query<Alerter>().Where(x => x.State == State.Enable);
            if (!string.IsNullOrEmpty(alerterCode))
            {
                querys.Where(x => x.Code == alerterCode);
            }
            var alerters = querys.ToList();
            return alerters;
        }
        #endregion
    }


    /// <summary>
    /// 物料接收查询信息
    /// </summary>
    public class ReceiveQueryInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 目标工位ID
        /// </summary>
        public double? DistStationId { get; set; }

        /// <summary>
        /// 上料工位ID
        /// </summary>
        public double? LoadStationId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 叫料单ID
        /// </summary>
        public double? callMaterialBillId { get; set; }

        /// <summary>
        /// 是否已上料
        /// </summary>
        public bool? IsLoadItem { get; set; }
    }
}