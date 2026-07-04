using SIE.Common;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.WorkOrders;
using SIE.EventMessages.LES;
using SIE.EventMessages.LES.Datas;
using SIE.LES.LinesideWarehouses;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialReceives.APIModel;
using SIE.LES.Reports;
using SIE.LES.Reports.Datas;
using SIE.Packages.ItemLabels;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收 控制器
    /// </summary>
    public partial class MaterialReceiveController : DomainController, ILesMaterialReceive
    {
        /// <summary>
        /// 查询获取物料接收信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<MaterialReceive> GetMaterialReceives(MaterialReceiveCriteria criteria)
        {
            var existsSql = string.Empty;
            var dtlTable = RF.Find<MaterialReceiveDetail>().EntityMeta.TableMeta.TableName;
            var itemTable = RF.Find<Item>().EntityMeta.TableMeta.TableName;

            var query = DB.Query<MaterialReceive>("T").Where(p => p.State == ReceiveState.TobeReceived || p.State == ReceiveState.PartReceived);
            if (!criteria.SoNo.IsNullOrEmpty())
            {
                query.Where(p => p.SoNo.Contains(criteria.SoNo));
            }
            if (!criteria.SourceNo.IsNullOrEmpty())
            {
                query.Where(p => p.MaterialPreparation.No.Contains(criteria.SourceNo));
            }
            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State.Value);
            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                if (existsSql.Length > 0)
                    existsSql += " AND ";
                existsSql += @$" EXISTS (SELECT 1 FROM {dtlTable} A1
                                            INNER JOIN {itemTable} A2 ON A1.ITEM_ID = A2.ID 
                                            WHERE A1.IS_PHANTOM = '0' 
                                              AND A1.MATERIAL_RECEIVE_ID = T.ID 
                                              AND A2.CODE LIKE '{criteria.ItemCode}')";

            }
            if (criteria.ItemName.IsNotEmpty())
            {
                if (existsSql.Length > 0)
                    existsSql += " AND ";
                existsSql += @$" EXISTS (SELECT 1 FROM {dtlTable} A1
                                            INNER JOIN {itemTable} A2 ON A1.ITEM_ID = A2.ID 
                                            WHERE A1.IS_PHANTOM = '0' 
                                              AND A1.MATERIAL_RECEIVE_ID = T.ID 
                                              AND A2.NAME LIKE '{criteria.ItemName}')";
            }
            //if (!criteria.LabelNo.IsNullOrEmpty())
            //{
            //    query.Where(p => p.LabelNo.Contains(criteria.LabelNo));
            //}
            //if (!criteria.LotNo.IsNullOrEmpty())
            //{
            //    query.Where(p => p.LotNo.Contains(criteria.LotNo));
            //}
            if (criteria.ShippingWarehouseId != 0 && criteria.ShippingWarehouseId != null)
            {
                query.Where(p => p.ShippingWarehouseId == criteria.ShippingWarehouseId.Value);
            }
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.WorkShopId != 0 && criteria.WorkShopId != null)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            }
            if (criteria.ResourceId != 0 && criteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId.Value);
            }
            if (!criteria.SoNo.IsNullOrEmpty())
            {
                query.Where(p => p.SoNo.Contains(criteria.SoNo));
            }
            if (criteria.CreateTime.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateTime.BeginValue.Value);
            }
            if (criteria.CreateTime.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateTime.EndValue.Value);
            }

            if (existsSql.Length > 0) query.Where(p => p.SQL<bool>(new FormattedSql(existsSql)));

            var list = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            list.ForEach(p => SetReceiveState(p));

            return list;
        }

        /// <summary>
        /// 查询接收明细列表
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveDetail> GetMaterialReceiveDetails(double receiveId, ReceiveState? state = null)
        {

            return Query<MaterialReceiveDetail>().Where(p => p.MaterialReceiveId == receiveId).WhereIf(state != null, p => p.State == state).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 查询接收标签列表
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveLabel> GetMaterialReceiveLabels(double receiveId, ReceiveState? state = null, List<double?> dtlIds = null)
        {
            return Query<MaterialReceiveLabel>().Where(p => p.MaterialReceiveId == receiveId)
                .WhereIf(state != null, p => p.State == state)
                .WhereIf(dtlIds != null, p => dtlIds.Contains(p.MaterialReceiveDetailId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 接口创建物料接收单据数据
        /// </summary>
        /// <param name="datas"></param>
        public virtual void CreateMaterialReceives(List<ShippingOrderData> datas)
        {
            //线边产线仓库维护
            var lineWhs = RF.GetAll<LinesideWarehouse>();

            //根据单号匹配备料需求单
            var noList = datas.Select(p => p.SourceNo).Distinct().ToList();
            var elo = new EagerLoadOptions()
                .LoadWith(MaterialPreparation.FactoryProperty)
                .LoadWith(MaterialPreparation.WorkOrderProperty)
                .LoadWith(MaterialPreparation.WorkShopProperty)
                .LoadWith(MaterialPreparation.ResourceProperty);
            var preBills = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreparationList(noList, elo);
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(noList);

            var itemCodes = datas.SelectMany(p => p.DetailLists.Select(x => x.ItemCode)).Distinct().ToList();
            var dicItems = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty()).ToDictionary(p => p.Code);

            var soNos = datas.Select(p => p.BillNo).ToList();
            var whCodes = datas.Select(p => p.ShippingWarehouseCode).ToList();
            var whs = RT.Service.Resolve<WarehouseController>().GetWarehouseList(whCodes);
            var list = Query<MaterialReceive>().Where(p => soNos.Contains(p.SoNo)).ToList(null);
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var data in datas)
                {
                    //接收单
                    var receive = list.FirstOrDefault(p => p.SoNo == data.BillNo);
                    receive = CreateMaterialReceive(receive, data, preBills, wos, lineWhs, whs);

                    //接收单明细
                    foreach (var dtl in data.DetailLists)
                    {
                        var detail = receive.DetailList.FirstOrDefault(p => p.State == ReceiveState.TobeReceived && p.SoLineNo == dtl.SoLineNo);
                        if (detail == null)
                        {
                            //明细
                            detail = CreateMaterialReceiveDetail(dtl, receive, dicItems);
                            receive.DetailList.Add(detail);
                        }

                        //标签
                        var snList = CreateMaterialReceiveLabels(dtl.SnList, receive, detail);
                        receive.LabelList.AddRange(snList);
                        detail.IsSerialNumber = snList.Any(p => p.IsSerialNumber);
                        detail.IssuedQty = receive.LabelList.Where(p => p.MaterialReceiveDetailId == detail.Id && p.State == ReceiveState.TobeReceived).Sum(p => p.IssuedQty);

                    }
                    SetReceiveState(receive);
                    RF.Save(receive);

                    //更新备料单
                    if (data.SourceNo.IsNotEmpty())
                    {
                        var sourceNo = data.SourceNo;
                        var lineNos = data.DetailLists.Select(p => p.SoLineNo).ToList();
                        RT.Service.Resolve<MaterialPreparationController>().UpdateMaterialPreQty(sourceNo, lineNos);
                    }

                    //更新工单BOM
                    UpdateWoBomQty(receive);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 设置接收状态
        /// </summary>
        /// <param name="receive"></param>
        public virtual void SetReceiveState(MaterialReceive receive)
        {
            if (receive.DetailList.Any(p => p.State == ReceiveState.TobeReceived))
                receive.State = ReceiveState.TobeReceived;
            else if (receive.DetailList.All(p => p.State == ReceiveState.Rejected))
                receive.State = ReceiveState.Rejected;
            else if (receive.DetailList.All(p => p.State != ReceiveState.TobeReceived))
                receive.State = ReceiveState.Received;
            else if (receive.DetailList.All(p => p.State == ReceiveState.Received))
                receive.State = ReceiveState.Received;
            else if (receive.DetailList.Any(p => p.State == ReceiveState.Received))
                receive.State = ReceiveState.PartReceived;
        }

        /// <summary>
        /// 创建物料接收实体
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="data"></param>
        /// <param name="preBills"></param>
        /// <param name="wos"></param>
        /// <param name="lineWhs"></param>
        /// <param name="whs"></param>
        /// <returns></returns>
        MaterialReceive CreateMaterialReceive(MaterialReceive receive, ShippingOrderData data, EntityList<MaterialPreparation> preBills, EntityList<WorkOrder> wos, EntityList<LinesideWarehouse> lineWhs, EntityList<Warehouse> whs)
        {
            if (receive == null)
            {
                receive = new MaterialReceive()
                {
                    SourceNo = data.SourceNo,
                    SoNo = data.BillNo,
                    DeliveryDate = data.DeliveryDate,
                    TransactionName = data.TransactionName,
                };
            }
            var preBill = preBills.FirstOrDefault(p => p.No == data.SourceNo);
            var wo = preBill?.WorkOrder ?? wos.FirstOrDefault(p => p.No == data.SourceNo);
            Warehouse shipWh = whs.FirstOrDefault(p => p.Code == data.ShippingWarehouseCode); //发货仓库
            Warehouse recWh = null; //根据车间/产线匹配接收仓库
            StorageLocation recLoc = null;//接收库位
             
            var resourceId = preBill?.ResourceId ?? wo?.ResourceId;
            var workShopId = preBill?.WorkShopId ?? wo?.WorkShopId ?? data.EnterpriseId;

            LinesideWarehouse lineWh = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(workShopId, resourceId);  //产线线边仓

            recWh = lineWh?.Warehouse;
            recLoc = lineWh?.StorageLocation;

            receive.MaterialPreparation = preBill;
            receive.MaterialPreparationId = preBill?.Id;
            receive.WorkOrder = wo;
            receive.WorkOrderId = wo?.Id;
            receive.WorkShopId = wo?.WorkShopId ?? lineWh?.WorkShopId;
            receive.ResourceId = wo?.ResourceId ?? lineWh?.WipResouceId;
            receive.FactoryId = wo?.FactoryId ?? lineWh?.FactoryId;
            receive.ShippingWarehouse = shipWh;
            receive.ShippingWarehouseId = shipWh?.Id;
            receive.ReceiveWarehouse = recWh;
            receive.ReceiveWarehouseId = recWh?.Id;
            receive.ReceiveLocation = recLoc;
            receive.ReceiveLocationId = recLoc?.Id;

            return receive;
        }

        /// <summary>
        /// 创建物料接收明细实体
        /// </summary>
        /// <param name="data"></param>
        /// <param name="receive"></param>
        /// <param name="dicItems"></param>
        /// <returns></returns>
        MaterialReceiveDetail CreateMaterialReceiveDetail(ShippingOrderDetailData data, MaterialReceive receive, Dictionary<string, Item> dicItems)
        {
            var item = dicItems.GetValue<Item>(data.ItemCode);
            if (item == null)
                throw new ValidationException("物料编码[{0}]不存在");
            var detail = new MaterialReceiveDetail()
            {
                MaterialReceive = receive,
                SoLineNo = data.SoLineNo,
                Item = item,
                ItemId = item?.Id,
                ItemCode = item?.Code,
                ItemName = item?.Name,
                ItemExtPropName = data.ItemExtPropName,
                ItemExtProp = data.ItemExtProp,
                ItemUnitName = data.SecondUnitName,
                ProjectNo = data.ProjectNo,
                IssuedQty = 0,
                State = ReceiveState.TobeReceived
            };
            detail.GenerateId();
            return detail;
        }

        /// <summary>
        /// 创建物料接收标签实体
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="receive"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        List<MaterialReceiveLabel> CreateMaterialReceiveLabels(List<ShippingOrderSnData> datas, MaterialReceive receive, MaterialReceiveDetail detail)
        {
            var list = new List<MaterialReceiveLabel>();
            datas.ForEach(p =>
            {
                var item = detail.Item;
                list.Add(new MaterialReceiveLabel()
                {
                    MaterialReceive = receive,
                    MaterialReceiveDetail = detail,
                    Item = item,
                    ItemId = item?.Id,
                    ItemCode = item?.Code,
                    ItemName = item?.Name,
                    ItemUnitName = detail.ItemUnitName,
                    ItemExtPropName = detail.ItemExtPropName,
                    ItemExtProp = detail.ItemExtProp,
                    ProjectNo = p.ProjectNo,
                    LotCode = p.LotCode,
                    IsSerialNumber = p.IsSerialNumber,
                    IsMerge = p.IsMerge,
                    LabelNo = p.SN,
                    IssuedQty = p.SecondPickQty,
                    ReceivedQty = 0,
                    SoLineNo = detail.SoLineNo,
                    State = ReceiveState.TobeReceived
                });
            });
            return list;
        }

        /// <summary>
        /// 根据发运单号查询物料接收单
        /// </summary>
        /// <param name="soNo"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual MaterialReceive GetMaterialReceive(string soNo, EagerLoadOptions elo = null)
        {
            return Query<MaterialReceive>().Where(p => p.SoNo == soNo).FirstOrDefault(elo);
        }

        /// <summary>
        /// 根据发运单号查询接收明细
        /// </summary>
        /// <param name="soNo"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveDetail> GetMaterialReceiveDetails(string soNo, ReceiveState? state = null)
        {
            MaterialReceive receive = GetMaterialReceive(soNo);

            var list = GetMaterialReceiveDetails(receive.Id, state);

            return list;
        }

        /// <summary>
        /// 根据发运单号查询接收标签明细
        /// </summary>
        /// <param name="soNo"></param>
        /// <param name="soLineNos"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveLabel> GetMaterialReceiveLabels(string soNo, List<string> soLineNos = null, ReceiveState? state = null)
        {

            MaterialReceive receive = GetMaterialReceive(soNo);

            var list = GetMaterialReceiveLabels(receive.Id, state);

            if (soLineNos != null)
            {
                list = list.Where(p => soLineNos.Contains(p.SoLineNo)).AsEntityList();
            }

            return list;
        }

        /// <summary>
        /// 物料接收(按单)
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <param name="isRejected">是否拒收</param>
        public virtual void MaterialReceive(List<double> receiveIds, bool isRejected = false)
        {
            foreach (var receiveId in receiveIds)
            {
                MaterialReceive(receiveId, isRejected);
            }
        }

        /// <summary>
        /// 物料接收(按单)
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="isRejected">是否拒收</param>
        public virtual void MaterialReceive(double receiveId, bool isRejected = false)
        {
            var details = GetMaterialReceiveDetails(receiveId, ReceiveState.TobeReceived).ToList();
            MaterialReceive(details, isRejected);
        }

        /// <summary>
        /// 物料接收(按单据行)
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="receiveDetailIds"></param>
        /// <param name="isRejected"></param>
        public virtual void MaterialReceive(double receiveId, List<double> receiveDetailIds, bool isRejected = false)
        {
            var details = GetMaterialReceiveDetails(receiveId, ReceiveState.TobeReceived).ToList();
            if (receiveDetailIds != null && details.Count > 0)
            {
                details = details.Where(p => receiveDetailIds.Contains(p.Id)).ToList();
            }
            MaterialReceive(details, isRejected);
        }

        /// <summary>
        /// 物料自动接收
        /// </summary>
        /// <param name="datas"></param>
        public virtual void MaterialAutoReceive(List<MaterialReceiveData> datas)
        {
            var aotuReceiveDetails = new List<MaterialReceiveDetail>();
            foreach (var data in datas)
            {
                var details = GetMaterialReceiveDetails(data.SoNo, ReceiveState.TobeReceived).ToList();

                if (data.SoLineNos != null && details.Count > 0)
                {
                    details = details.Where(p => data.SoLineNos.Contains(p.SoLineNo)).ToList();
                }
                if (details.Count == 0)
                    continue;
                //    throw new ValidationException("发运单[{0}]没有要接收的数据".L10nFormat(data.SoNo));

                //物料接收
                var receiveId = details.FirstOrDefault().MaterialReceiveId ?? 0;
                bool isAutoReceive = RT.Service.Resolve<LinesideWarehouseController>().IsAutoReceive(data.EnterpriseCode);
                if (isAutoReceive)
                {
                    details.ForEach(p => p.ReceiveType = ReceiveType.Auto);
                    aotuReceiveDetails.AddRange(details);
                }
            }
            if (aotuReceiveDetails.Count > 0)
                MaterialReceive(aotuReceiveDetails, receiveType: ReceiveType.Auto);
        }


        /// <summary>
        /// 物料接收(按单据行)
        /// </summary>
        /// <param name="details"></param>
        /// <param name="isRejected">是否拒收</param>
        /// <param name="receiveType">接收方式</param>
        public virtual void MaterialReceive(List<MaterialReceiveDetail> details, bool isRejected = false, ReceiveType receiveType = ReceiveType.Hand)
        {
            if (details == null || details.Count == 0)
                throw new ValidationException("没有要接收的数据".L10nFormat());

            var receiveIds = details.Select(p => p.MaterialReceiveId ?? 0).Distinct().ToList();
            var receives = GetMaterialReceives(receiveIds, new EagerLoadOptions().LoadWithViewProperty());

            var receiveLabels = new List<MaterialReceiveLabel>();

            //匹配并校验明细与标签接收数量
            foreach (var detail in details)
            {
                var receive = receives.FirstOrDefault(p => p.Id == detail.MaterialReceiveId);
                var sourceDtl = receive.DetailList.FirstOrDefault(p => p.Id == detail.Id);
                if (receive == null || sourceDtl == null)
                    throw new ValidationException("发运单[{0}]行[{1}]不存在".L10nFormat(receive.SoNo, detail.SoLineNo));
                if (sourceDtl.State != ReceiveState.TobeReceived)
                    throw new ValidationException("发运单[{0}]行[{1}]状态为[{2}]".L10nFormat(receive.SoNo, detail.SoLineNo, sourceDtl.State.ToLabel()));
                if (detail.IssuedQty != sourceDtl.IssuedQty)
                    throw new ValidationException("发运单[{0}]行[{1}]数据有更新,请重新扫码确认".L10nFormat(receive.SoNo, detail.SoLineNo));

                //如果存在扫码标签,则按实际标签数量接收,否则按行号匹配所有待接收标签
                var partLabelIds = detail.LabelList.Select(p => p.Id).ToList();
                var labels = receive.LabelList.Where(p => !partLabelIds.Contains(p.Id) && p.MaterialReceiveDetailId == detail.Id && p.State == ReceiveState.TobeReceived).ToList();
                var isPartReceive = partLabelIds.Count > 0; //明细中包含标签,则为部分接收
                if (isPartReceive)
                {
                    //部分接收时, 按实际标签接收数接收, 其余标签为拒收
                    labels.ForEach(p => p.ReceivedQty = 0);
                }
                else
                {
                    //非部分接收, 则全量接收或拒收
                    labels.ForEach(p => p.ReceivedQty = isRejected ? 0 : p.IssuedQty);
                    detail.ReceivedQty = isRejected ? 0 : detail.IssuedQty;
                }
                detail.LabelList.AddRange(labels);

                var sumReceiveQty = detail.LabelList.Sum(p => p.ReceivedQty);
                if (detail.ReceivedQty != sumReceiveQty)
                    throw new ValidationException("发运单[{0}]行[{1}]接收数[{2}]与对应的标签接收汇总数[{3}]不一致".L10nFormat(receive.SoNo, detail.SoLineNo, detail.ReceivedQty, sumReceiveQty));

                receiveLabels.AddRange(detail.LabelList);
            }

            if (receiveLabels.Count > 0)
            {
                //合并拣货标签,需要同时接收或者拒收处理
                var megreLabels = GetMegreLabels(receiveLabels);
                receiveLabels.AddRange(megreLabels);
                //接收类型
                receiveLabels.ForEach(p => p.ReceiveType = receiveType);
                MaterialReceive(receiveLabels);
            }
        }


        /// <summary>
        /// 获取合并关联标签
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveLabel> GetMegreLabels(List<MaterialReceiveLabel> labels)
        {
            var labelNos = labels.Where(p => p.IsMerge && p.LabelNo.IsNotEmpty()).Select(p => p.LabelNo).Distinct().ToList();
            var labelIds = labels.Where(p => p.IsMerge && p.LabelNo.IsNotEmpty()).Select(p => p.Id).Distinct().ToList();
            var list = Query<MaterialReceiveLabel>().Where(p => labelNos.Contains(p.LabelNo) && !labelIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            list.ForEach(p =>
            {
                var orgin = labels.FirstOrDefault(l => l.LabelNo == p.LabelNo);
                p.ReceivedQty = orgin.ReceivedQty == 0 ? 0 : p.IssuedQty;   //标签明细拒收时,需要拒收整个合并标签 
            });

            return list;
        }

        /// <summary>
        /// 物料接收(按标签)
        /// </summary>
        /// <param name="labels"></param>
        public virtual void MaterialReceive(List<MaterialReceiveLabel> labels)
        {
            //查询备料需求单
            var preNos = labels.Select(p => p.MaterialPreparationNo).Distinct().ToList();
            var sounceNos = labels.Select(p => p.SourceNo).Distinct().ToList();
            var elo = new EagerLoadOptions().LoadWith(MaterialPreparation.WorkOrderProperty).LoadWith(MaterialPreparation.DetailListProperty);
            var preBills = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreparationList(preNos, elo);
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(sounceNos);
            var receiveType = labels.FirstOrDefault()?.ReceiveType;

            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //回写WMS发料数
                ReceiveShipData(labels);

                //接收记录
                var records = CreateMaterialReceiveRecords(labels, preBills, wos);

                //更新工单线边仓汇总表
                UpdateWoDemandReports(records, preBills);

                //创建物料标签
                CreateItemLabels(labels);

                //更新物料接收状态
                UpdateMaterialReceiveStates(labels);

                //更新备料单接收数
                UpdateMaterialPreparations(labels);

                //更新工单BOM
                UpdateWoBomQty(records);

                trans.Complete();
            }
        }

        /// <summary>
        /// 回写WMS发料数
        /// </summary>
        void ReceiveShipData(List<MaterialReceiveLabel> labels)
        {
            var receiveShipDataParams = labels.GroupBy(p => new
            {
                p.SoNo,
                p.SoLineNo,
                p.ItemCode,
                p.ItemUnitName,
                p.ReceiveWarehouseCode,
                p.ReceiveLocationCode,
                IsReject = p.ReceivedQty > 0 ? false : true
            }).Select(p => new ReceiveShipDataParam()
            {
                InvOrgId = RT.InvOrg.Value,
                BillNo = p.Key.SoNo,
                LineNo = p.Key.SoLineNo,
                ItemCode = p.Key.ItemCode,
                UnitName = p.Key.ItemUnitName,
                Qty = p.Sum(x => x.ReceivedQty),
                ReceiveWarehouseCode = p.Key.ReceiveWarehouseCode,
                ReceiveLocationCode = p.Key.ReceiveLocationCode,
                ScanLabels = p.Select(x => new LabelData() { Sn = x.LabelNo, LotCode = x.LotCode, Qty = x.ReceivedQty }).ToList()
            }).ToList();
            try
            {
                RT.Service.Resolve<ILesShippingOrder>().ReceiveShipData(receiveShipDataParams);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.GetBaseException().Message;
                throw new ValidationException("调用WMS接口失败: {0}".L10nFormat(errorMsg));
            }
        }

        /// <summary>
        /// 更新备料单接收数
        /// </summary>
        /// <param name="labels"></param>
        void UpdateMaterialPreparations(List<MaterialReceiveLabel> labels)
        {
            labels.Where(p => p.MaterialPreparationNo.IsNotEmpty()).GroupBy(p => p.MaterialPreparationNo).ForEach(p =>
            {
                var sourceNo = p.Key;
                var lineNos = p.Select(p => p.SoLineNo).ToList();
                RT.Service.Resolve<MaterialPreparationController>().UpdateMaterialPreQty(sourceNo, lineNos);

            });
        }
        /// <summary>
        /// 创建物料标签
        /// </summary>
        /// <param name="labels"></param>
        void CreateItemLabels(List<MaterialReceiveLabel> labels)
        {
            var list = labels.Where(p => p.ReceiveWarehouseId > 0 && p.ReceivedQty > 0).ToList();
            if (list.Count == 0)
                return;
            var labelNos = list.Select(p => p.LabelNo).Distinct().ToList();
            var lotCodes = list.Select(p => p.LotCode).Distinct().ToList();
            var itemCodes = list.Select(p => p.ItemCode).Distinct().ToList();
            labelNos.AddRange(lotCodes);
            labelNos.AddRange(itemCodes);
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(labelNos);
            itemLabels.ForEach(p => p.ProjectNo = p.ProjectNo.IsNullOrEmpty() ? "*" : p.ProjectNo);
            var newLabels = new EntityList<ItemLabel>();
            foreach (var data in list)
            {
                if (data.ReceiveWarehouseId == null || data.ReceiveWarehouseId == 0)  //无接收仓库不创建标签
                    continue;
                if (data.ReceivedQty == 0)
                    continue;
                if (data.ProjectNo.IsNullOrEmpty())
                    data.ProjectNo = "*";
                var labelNo = data.LabelNo;
                if (labelNo.IsNullOrEmpty())
                    labelNo = data.LotCode;    //无标签号则用 LotCode
                if (labelNo.IsNullOrEmpty())
                    labelNo = data.ItemCode;    //无批次号则用 ItemCode
                var label = itemLabels.FirstOrDefault(p =>
                        p.Label == labelNo
                        && p.ItemId == data.ItemId
                        && p.ItemExtProp == data.ItemExtProp
                        && p.ItemExtPropName == data.ItemExtPropName
                        && p.ProjectNo == data.ProjectNo
                        && p.StorageLocationId == data.ReceiveLocationId
                        && p.FactoryId == data.FactoryId
                        && p.SourceType == LabelSource.Receive);
                if (label == null)
                {
                    label = new ItemLabel()
                    {
                        Label = labelNo,
                        Qty = data.ReceivedQty,
                        InitialQty = data.ReceivedQty,
                        ItemId = data.ItemId.Value,
                        ItemExtProp = data.ItemExtProp,
                        ItemExtPropName = data.ItemExtPropName,
                        IsSerialNumber = data.IsSerialNumber,
                        Lot = data.LotCode,
                        ProjectNo = data.ProjectNo,
                        FactoryId = data.FactoryId,
                        WarehouseId = data.ReceiveWarehouseId,
                        StorageLocationId = data.ReceiveLocationId,
                        SourceType = LabelSource.Receive,
                    };
                    RF.Save(label);
                    itemLabels.Add(label);
                }
                else
                {
                    DB.Update<ItemLabel>().Set(x => x.Qty, x => x.Qty + data.ReceivedQty).Where(p => p.Id == label.Id).Execute();
                }
            }

        }

        /// <summary>
        /// 创建接收记录
        /// </summary>
        /// <param name="labels"></param>
        /// <param name="preBills"></param>
        EntityList<MaterialReceiveRecord> CreateMaterialReceiveRecords(List<MaterialReceiveLabel> labels, EntityList<MaterialPreparation> preBills, EntityList<WorkOrder> wos)
        {
            var records = new EntityList<MaterialReceiveRecord>();
            var time = RF.Find<MaterialReceiveRecord>().GetDbTime();
            foreach (var p in labels)
            {
                var preBill = preBills.FirstOrDefault(x => x.No == p.MaterialPreparationNo);
                var wo = preBill?.WorkOrder ?? wos.FirstOrDefault(x => x.No == p.SourceNo);
                var record = new MaterialReceiveRecord()
                {
                    SourceNo = p.SourceNo,
                    SoNo = p.SoNo,
                    SoLineNo = p.SoLineNo,
                    ReceiveType = p.ReceiveType,    // ReceiveType.Hand,
                    State = ReceiveState.Received,
                    LabelNo = p.LabelNo,
                    LotCode = p.LotCode,
                    ItemId = p.ItemId ?? 0,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                    ProjectNo = p.ProjectNo,
                    IssuedQty = p.IssuedQty,
                    ReceivedQty = p.ReceivedQty,
                    RejectedQty = p.IssuedQty - p.ReceivedQty,
                    //OverReceivedQty = p.ReceivedQty - p.IssuedQty,
                    ReceiveTime = time,
                    ReceiveById = RT.IdentityId,
                    ReceiveWarehouseId = p.ReceiveWarehouseId,
                    ShippingWarehouseId = p.ReceiveWarehouseId,
                    WorkOrderId = wo?.Id,
                    WorkShopId = wo?.WorkShopId,
                    ResourceId = wo?.ResourceId,
                    FactoryId = wo?.FactoryId,
                    PrepareType = preBill?.PrepareType
                };
                if (record.OverReceivedQty < 0)
                    record.OverReceivedQty = 0;
                if (record.ReceivedQty > 0 && record.RejectedQty > 0)
                    record.State = ReceiveState.PartReceived;
                else if (record.ReceivedQty == 0 && record.RejectedQty > 0)
                    record.State = ReceiveState.Rejected;
                RF.Save(record);

                records.Add(record);
            }

            return records;
        }

        /// <summary>
        /// 更新工单线边仓汇总表
        /// </summary>
        /// <param name="records"></param>
        /// <param name="preBills"></param>
        void UpdateWoDemandReports(EntityList<MaterialReceiveRecord> records, EntityList<MaterialPreparation> preBills)
        {
            //无仓库、接收数为0、车间备料不记录工单占用库存
            var list = records.Where(p => p.ReceiveWarehouseId > 0 && p.ReceivedQty > 0 && p.PrepareType != MaterialPreparations.Enums.PrepareType.Sfmr).ToList();
            if (list.Count == 0)
                return;
            var adjustParms = new List<AdjustQtyParams>();
            foreach (var p in list)
            {
                var overReceive = 0m;
                //有超收数时,超收数量不记入工单库存
                if (p.SourceNo.IsNotEmpty())
                {
                    var preBill = preBills.FirstOrDefault(x => x.No == p.SourceNo);
                    var preBillDtl = preBill?.DetailList.FirstOrDefault(x => x.LineNo == p.SoLineNo);
                    if (preBillDtl != null)
                    {
                        if (preBillDtl.ReceiveQty >= preBillDtl.Qty)    //备料单超收时,不记入工单占用库存
                            continue;
                        else
                            preBillDtl.ReceiveQty += p.ReceivedQty;
                        overReceive = preBillDtl.ReceiveQty - preBillDtl.Qty;   //超收数
                    }
                }

                var param = new AdjustQtyParams()
                {
                    WarehouseId = p.ReceiveWarehouseId.Value,
                    WorkShopId = p.WorkShopId,
                    WorkOrderId = p.WorkOrderId,
                    ResourceId = p.ResourceId,
                    ItemId = p.ItemId,
                    ItemExtProp = p.ItemExtProp,
                    ItemExtPropName = p.ItemExtPropName,
                    Qty = overReceive > 0 ? (p.ReceivedQty - overReceive) : p.ReceivedQty,
                };
                adjustParms.Add(param);
            }
            if (adjustParms.Count > 0)
            {
                //合并汇总
                adjustParms = adjustParms.GroupBy(p => new
                {
                    p.WarehouseId,
                    p.WorkShopId,
                    p.WorkOrderId,
                    p.ResourceId,
                    p.ItemId,
                    p.ItemExtProp,
                    p.ItemExtPropName
                }).Select(p => new AdjustQtyParams()
                {
                    WarehouseId = p.Key.WarehouseId,
                    WorkShopId = p.Key.WorkShopId,
                    WorkOrderId = p.Key.WorkOrderId,
                    ResourceId = p.Key.ResourceId,
                    ItemId = p.Key.ItemId,
                    ItemExtProp = p.Key.ItemExtProp,
                    ItemExtPropName = p.Key.ItemExtPropName,
                    Qty = p.Sum(x => x.Qty),
                }).ToList();
                RT.Service.Resolve<WoDemandReportController>().AdjustReportReceiveQty(adjustParms);
            }
        }

        /// <summary>
        /// 更新物料接收状态
        /// </summary>
        /// <param name="labels"></param>
        void UpdateMaterialReceiveStates(List<MaterialReceiveLabel> labels)
        {
            var ids = labels.Select(p => p.MaterialReceiveId ?? 0).Distinct().ToList();
            var dtlIds = labels.Select(p => p.MaterialReceiveDetailId).Distinct().ToList();
            //更新标签状态
            foreach (var label in labels)
            {
                var state = ReceiveState.Received;
                if (label.ReceivedQty == 0)
                    state = ReceiveState.Rejected;
                else if (label.ReceivedQty < label.IssuedQty)
                    state = ReceiveState.PartReceived;
                DB.Update<MaterialReceiveLabel>().Set(p => p.State, state).Set(p => p.ReceivedQty, label.ReceivedQty).Where(p => p.Id == label.Id).Execute();
            }
            //更新未接收的标签为拒收
            DB.Update<MaterialReceiveLabel>().Set(p => p.State, ReceiveState.Rejected).Set(p => p.ReceivedQty, 0).Where(p => dtlIds.Contains(p.MaterialReceiveDetailId) && p.State == ReceiveState.TobeReceived).Execute();

            //更新明细状态
            var labelList = Query<MaterialReceiveLabel>().Where(p => dtlIds.Contains(p.MaterialReceiveDetailId)).ToList();
            foreach (var dtlId in dtlIds)
            {
                var issuedQty = labelList.Where(p => p.MaterialReceiveDetailId == dtlId).Sum(p => p.IssuedQty);
                var receivedQty = labelList.Where(p => p.MaterialReceiveDetailId == dtlId).Sum(p => p.ReceivedQty);
                var state = ReceiveState.Received;
                if (receivedQty == 0)
                    state = ReceiveState.Rejected;
                else if (receivedQty < issuedQty)
                    state = ReceiveState.PartReceived;
                DB.Update<MaterialReceiveDetail>().Set(p => p.State, state).Set(p => p.ReceivedQty, receivedQty).Where(p => p.Id == dtlId).Execute();
            }
            //更新单据状态
            var receives = GetMaterialReceives(ids);
            foreach (var receive in receives)
            {
                SetReceiveState(receive);
                RF.Save(receive);
            }
        }

        /// <summary>
        /// 更新工单BOM
        /// </summary>
        /// <param name="records"></param>
        void UpdateWoBomQty(EntityList<MaterialReceiveRecord> records)
        {
            records.Where(p => p.WorkOrderId > 0).GroupBy(p => p.WorkOrderId).ForEach(p =>
            {
                var woId = p.Key.Value;
                var itemIds = p.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
            });
        }

        /// <summary>
        /// 更新工单BOM
        /// </summary>
        /// <param name="receive"></param>
        void UpdateWoBomQty(MaterialReceive receive)
        {
            if (!receive.WorkOrderId.HasValue) return;
            var woId = receive.WorkOrderId.Value;
            var itemIds = receive.DetailList.Select(p => p.ItemId ?? 0).Distinct().ToList();
            RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
        }

        /// <summary>
        /// 查询获取物料接收记录信息
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveRecord> GetMaterialReceiveRecords(MaterialReceiveRecordCriteria criteria)
        {
            var query = Query<MaterialReceiveRecord>();
            if (criteria == null)
            {
                throw new ValidationException("物料接收查询实体数据异常!".L10N());
            }
            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State.Value);
            }
            if (!criteria.ItemCode.IsNullOrEmpty())
            {
                query.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            }
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            }
            if (!criteria.LabelNo.IsNullOrEmpty())
            {
                query.Where(p => p.LabelNo.Contains(criteria.LabelNo));
            }
            if (!criteria.LotCode.IsNullOrEmpty())
            {
                query.Where(p => p.LotCode.Contains(criteria.LotCode));
            }
            if (criteria.WarehouseId != 0 && criteria.WarehouseId != null)
            {
                query.Where(p => p.ReceiveWarehouseId == criteria.WarehouseId.Value);
            }
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.WorkShopId != 0 && criteria.WorkShopId != null)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            }
            if (criteria.ResourceId != 0 && criteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId.Value);
            }
            if (!criteria.SoNo.IsNullOrEmpty())
            {
                query.Where(p => p.SoNo.Contains(criteria.SoNo));
            }
            if (criteria.ReceiveById != 0 && criteria.ReceiveById != null)
            {
                query.Where(p => p.ReceiveById == criteria.ReceiveById);
            }
            if (criteria.ReceiveTime.BeginValue.HasValue)
            {
                query.Where(p => p.ReceiveTime >= criteria.ReceiveTime.BeginValue.Value);
            }
            if (criteria.ReceiveTime.EndValue.HasValue)
            {
                query.Where(p => p.ReceiveTime <= criteria.ReceiveTime.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ids查询接收列表
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceive> GetMaterialReceives(List<double> receiveIds, EagerLoadOptions elo = null)
        {
            var receives = receiveIds.SplitContains(temp =>
            {
                return Query<MaterialReceive>().Where(p => temp.Contains(p.Id)).ToList(null, elo);
            });
            return receives;
        }

        /// <summary>
        /// 根据发运单号查询接收列表
        /// </summary>
        /// <param name="soNos"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceive> GetMaterialReceives(List<string> soNos, EagerLoadOptions elo = null)
        {
            var receives = soNos.SplitContains(temp =>
            {
                return Query<MaterialReceive>().Where(p => temp.Contains(p.SoNo)).ToList(null, elo);
            });
            return receives;
        }
        /// <summary>
        /// 根据ids查询接收明细列表
        /// </summary>
        /// <param name="receiveDtlIds"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceiveDetail> GetMaterialReceiveDetails(List<double> receiveDtlIds, EagerLoadOptions elo = null)
        {
            var receiveDeatils = receiveDtlIds.SplitContains(temp =>
            {
                return Query<MaterialReceiveDetail>().Where(p => temp.Contains(p.Id)).ToList(null, elo);
            });
            return receiveDeatils;
        }

        /// <summary>
        /// 校验单据是否超收
        /// </summary>
        /// <param name="receiveIds"></param>
        /// <returns></returns>
        public virtual List<string> ValidationOverReceive(List<double> receiveIds)
        {
            var msgs = new List<string>();
            var elo = new EagerLoadOptions();
            elo.LoadWith(SIE.LES.MaterialReceives.MaterialReceive.MaterialPreparationProperty);

            var receives = GetMaterialReceives(receiveIds, elo);
            receives.Where(p => p.MaterialPreparationId > 0).ForEach(p =>
            {
                var preBill = p.MaterialPreparation;
                if (preBill != null)
                {
                    var preDtls = p.MaterialPreparation.DetailList;
                    p.DetailList.Where(d => d.State == ReceiveState.TobeReceived).ForEach(d =>
                    {
                        var preDtl = preDtls.FirstOrDefault(x => x.LineNo == d.SoLineNo);
                        if (preDtl != null && preDtl.Qty < preDtl.ReceiveQty + d.IssuedQty) //全量接收
                        {
                            var issuedQty = preDtl?.ReceiveQty + d.IssuedQty;
                            msgs.Add("发运单[{0}]行[{1}]累计发料数[{2}]大于需求数[{3}]".L10nFormat(d.SoNo, d.SoLineNo, issuedQty, preDtl.Qty));
                        }
                    });
                }
            });
            return msgs;
        }


        /// <summary>
        /// 校验明细是否超收
        /// </summary>
        /// <param name="receiveDtlIds"></param>
        /// <returns></returns>
        public virtual List<string> ValidationOverReceiveDetail(List<double> receiveDtlIds)
        {
            var msgs = new List<string>();
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var receiveDtls = GetMaterialReceiveDetails(receiveDtlIds, elo);
            var noList = receiveDtls.Select(p => p.MaterialPreparationNo).Distinct().ToList();
            var preBills = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreparationList(noList, new EagerLoadOptions().LoadWith(MaterialPreparation.DetailListProperty));
            receiveDtls.Where(d => d.MaterialPreparationNo.IsNotEmpty()).ForEach(d =>
            {
                var preBill = preBills.FirstOrDefault(x => x.No == d.MaterialPreparationNo);
                if (preBill != null)
                {
                    var preDtl = preBill.DetailList.FirstOrDefault(x => x.LineNo == d.SoLineNo);
                    if (preDtl != null && preDtl.Qty < preDtl.ReceiveQty + d.IssuedQty) //全量接收
                    {
                        var issuedQty = preDtl?.ReceiveQty + d.IssuedQty;
                        msgs.Add("发运单[{0}]行[{1}]累计发料数[{2}]大于需求数[{3}]".L10nFormat(d.SoNo, d.SoLineNo, issuedQty, preDtl.Qty));
                    }
                }
            });
            return msgs;
        }
        /// <summary>
        /// 校验明细是否超收
        /// </summary>
        /// <param name="receiveDtls"></param>
        /// <returns></returns>
        public virtual List<string> ValidationOverReceiveDetail(List<MaterialReceiveDetail> receiveDtls)
        {
            var msgs = new List<string>();
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            //var receiveDtls = GetMaterialReceiveDetails(receiveDtlIds, elo);
            var noList = receiveDtls.Select(p => p.MaterialPreparationNo).Distinct().ToList();
            var preBills = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreparationList(noList, new EagerLoadOptions().LoadWith(MaterialPreparation.DetailListProperty));
            receiveDtls.Where(d => d.MaterialPreparationNo.IsNotEmpty()).ForEach(d =>
            {
                var preBill = preBills.FirstOrDefault(x => x.No == d.MaterialPreparationNo);
                if (preBill != null)
                {
                    var preDtl = preBill.DetailList.FirstOrDefault(x => x.LineNo == d.SoLineNo);
                    if (preDtl != null && preDtl.Qty < preDtl.ReceiveQty + d.ReceivedQty)
                    {
                        var issuedQty = preDtl?.ReceiveQty + d.ReceivedQty;
                        msgs.Add("发运单[{0}]行[{1}]累计发料数[{2}]大于需求数[{3}]".L10nFormat(d.SoNo, d.SoLineNo, issuedQty, preDtl.Qty));
                    }
                }
            });
            return msgs;
        }

        /// <summary>
        /// 获取接收记录已接收数
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string>, decimal> GetReceivedDic(List<double> woIds)
        {
            List<MaterialReceInfo> materialReceInfos = new List<MaterialReceInfo>();
            woIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReceiveRecord>().Where(p => p.WorkOrderId != null && temps.Contains((double)p.WorkOrderId))
                .GroupBy(p => new
                {
                    p.WorkOrderId,
                    p.ItemId,
                    p.ItemExtProp,
                })
                .Select(p => new
                {
                    WoId = p.WorkOrderId,
                    ItemId = p.ItemId,
                    ItemExtProp = p.ItemExtProp,
                    Qty = p.ReceivedQty.SUM(),
                }).ToList<MaterialReceInfo>();
                materialReceInfos.AddRange(list);
            });
            return materialReceInfos.ToDictionary(p => new Tuple<double, double, string>(p.WoId, p.ItemId, p.ItemExtProp), p => p.Qty);
        }

        /// <summary>
        /// 获取接收记录待接收数
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual Dictionary<Tuple<double, double, string>, decimal> GetToReceiveDic(List<double> woIds)
        {
            List<MaterialReceInfo> materialReceInfos = new List<MaterialReceInfo>();
            woIds.SplitDataExecute(temps =>
            {
                var list = Query<MaterialReceiveDetail>().LeftJoin<MaterialReceive>((mrd, mr) => mrd.MaterialReceiveId == mr.Id)
                .Where(mrd => mrd.State == ReceiveState.TobeReceived)
                .GroupBy<MaterialReceive>((mrd, mr) => new { mr.WorkOrderId, mrd.ItemId, mrd.ItemExtProp })
                .Select<MaterialReceive>((mrd, mr) => new
                {
                    WoId = mr.WorkOrderId,
                    ItemId = mrd.ItemId,
                    ItemExtProp = mrd.ItemExtProp,
                    Qty = mrd.IssuedQty.SUM(),
                }).ToList<MaterialReceInfo>();
                materialReceInfos.AddRange(list);
            });
            return materialReceInfos.ToDictionary(p => new Tuple<double, double, string>(p.WoId, p.ItemId, p.ItemExtProp), p => p.Qty);
        }
    }
}
