using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.LES.Datas;
using SIE.LES.MaterialMoves;
using SIE.LES.Reports.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.Reports
{
    /// <summary>
    /// 工单需求汇总报表 控制器
    /// </summary>
    public class WoDemandReportController : DomainController
    {

        /// <summary>
        /// 查询工单需求汇总报表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<WoDemandReport> GetWoDemandReports(WoDemandReportCriteria criteria)
        {
            var query = Query<WoDemandReport>();

            //if (!criteria.ItemCode.IsNullOrEmpty())
            //{
            //    query.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            //}
            //if (!criteria.ItemName.IsNullOrEmpty())
            //{
            //    query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            //}
            if (criteria.ItemId > 0)
            {
                query.Where(p => p.ItemId == criteria.ItemId.Value);
            }
            if (criteria.WarehouseId > 0)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            }
            if (criteria.WorkOrderId > 0)
            {
                query.Where(p => p.WorkOrderId == criteria.WorkOrderId.Value);
            }
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.WorkShopId > 0 && (criteria.ResourceId == null || criteria.ResourceId == 0))
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            }
            if (criteria.ResourceId > 0)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId.Value);
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        #region 更新接收数

        /// <summary>
        /// 更新报表接收数量
        /// </summary>
        public virtual void AdjustReportReceiveQty(List<AdjustQtyParams> adjustParams)
        {
            if (adjustParams == null || adjustParams.Count == 0)
                return;

            //工单不为空时, 按工单+物料编码+扩展属性+线边仓的维度写入数据
            var parms1 = adjustParams.Where(p => p.WorkOrderId > 0).ToList();
            if (parms1.Any())
            {
                var woIds = parms1.Select(p => p.WorkOrderId).Distinct().ToList();
                var reports = woIds.SplitContains(temp =>
                {
                    return DB.Query<WoDemandReport>().Where(p => temp.Contains(p.WorkOrderId)).ToList();
                });
                AdjustReceiveQty(parms1, reports);
            }

            //工单为空时，按车间+物料编码+扩展属性+线边仓写入接收数
            var parms2 = adjustParams.Where(p => p.WorkOrderId == null || p.WorkOrderId == 0).ToList();
            parms2.ForEach(p => p.WorkOrderId = null);
            if (parms2.Any())
            {
                var workShopIds = parms1.Select(p => p.WorkShopId).Distinct().ToList();
                var reports = workShopIds.SplitContains(temp =>
                {
                    return DB.Query<WoDemandReport>().Where(p => temp.Contains(p.WorkShopId) && p.WorkOrderId == null).ToList();
                });
                AdjustReceiveQty(parms2, reports);
            }
        }

        /// <summary>
        /// 更新接收数
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="reports"></param>
        void AdjustReceiveQty(List<AdjustQtyParams> parms, EntityList<WoDemandReport> reports)
        {
            foreach (var parm in parms)
            {
                var report = reports.FirstOrDefault(p => p.WarehouseId == parm.WarehouseId && p.WorkOrderId == parm.WorkOrderId && p.ItemId == parm.ItemId && p.ItemExtProp == parm.ItemExtProp && p.WorkShopId == parm.WorkShopId);
                if (report == null)
                {
                    //新增库存
                    report = new WoDemandReport()
                    {
                        WarehouseId = parm.WarehouseId,
                        WorkOrderId = parm.WorkOrderId,
                        WorkShopId = parm.WorkShopId,
                        ResourceId = parm.ResourceId,
                        ItemId = parm.ItemId,
                        ItemExtProp = parm.ItemExtProp,
                        ItemExtPropName = parm.ItemExtPropName,
                        ReceivedQty = parm.Qty
                    };
                    RF.Save(report);
                    reports.Add(report);
                }
                else
                {
                    //更新库存
                    DB.Update<WoDemandReport>().Set(p => p.ReceivedQty, p => p.ReceivedQty + parm.Qty).Where(p => p.Id == report.Id).Execute();
                }
            }
        }

        /// <summary>
        /// MES更新线边库存数据(上料|下料)
        /// </summary>
        public virtual void MesUpdateOnhand(MesUpdateOnhandData data)
        {
            if (data.LabelDatas.Count == 0)
                return;
            var reports = DB.Query<WoDemandReport>().Where(p => p.WorkOrderId == data.WoId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var label in data.LabelDatas)
            {
                var report = reports.FirstOrDefault(p => p.WarehouseId == label.WarehouseId && p.WorkOrderId == label.WorkOrderId && p.ItemId == label.ItemId && p.ItemExtProp == label.ItemExtProp && p.ItemExtPropName == label.ItemExtPropName);

                if (data.OpType == 0 && (report == null || report.AvailableQty < label.Qty))
                {
                    throw new ValidationException("工单[{0}]物料[{1}]在线边仓[{2}]剩余可用数不足".L10nFormat(data.WoNo, label.ItemCode, label.WarehouseCode));
                }

                if (report != null)
                {
                    //上料时: 按工单 + 物料编码 + 扩展属性 + 线边仓 扣减‘可用数’，同时增加‘上料数’
                    //正常下料时: 按工单 + 物料编码 + 扩展属性 + 线边仓 增加‘可用数’，同时减少‘上料数’
                    //不良下料时: 按工单 + 物料编码 + 扩展属性 + 线边仓 增加‘不良数’，同时减少‘上料数’
                    if (data.OpType == 0)
                        DB.Update<WoDemandReport>().Set(p => p.FeedQty, p => p.FeedQty + label.Qty).Where(p => p.Id == report.Id).Execute();
                    else if (data.OpType == 1 && !label.IsFail)
                        DB.Update<WoDemandReport>().Set(p => p.FeedQty, p => p.FeedQty - label.Qty).Where(p => p.Id == report.Id).Execute();
                    else if (data.OpType == 1 && label.IsFail)
                        DB.Update<WoDemandReport>().Set(p => p.FeedQty, p => p.FeedQty - label.Qty).Set(p => p.NgQty, p => p.NgQty + label.Qty).Where(p => p.Id == report.Id).Execute();
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取工单占用剩余可用数
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isMove">无库存时,是否进去挪料</param>
        public virtual decimal GetWoDemandReportQty(MesLabelData label, bool isMove = false)
        {
            var report = DB.Query<WoDemandReport>().Where(p =>
                p.WarehouseId == label.WarehouseId
                && p.WorkOrderId == label.WorkOrderId
                && p.ItemId == label.ItemId
                && p.ItemExtProp == label.ItemExtProp
                && p.ItemExtPropName == label.ItemExtPropName).FirstOrDefault();
            if (report == null || report?.AvailableQty == 0)
            {
                //无库存时,进行挪料, 挪料成功时,返回挪料数
                if (isMove)
                {
                    if (report == null)
                    {
                        //创建工单占用数据
                        var wo = RF.GetById<WorkOrder>(label.WorkOrderId);
                        report = new WoDemandReport()
                        {
                            WarehouseId = label.WarehouseId,
                            WorkOrderId = label.WorkOrderId,
                            WorkShopId = wo?.WorkShopId,
                            ResourceId = wo?.ResourceId,
                            ItemId = label.ItemId,
                            ItemExtProp = label.ItemExtProp,
                            ItemExtPropName = label.ItemExtPropName,
                            //ReceivedQty = 0
                        };
                        RF.Save(report);
                    }
                    //挪料
                    var data = new LoadItemMoveData()
                    {
                        InvOrgId = RT.InvOrg ?? 0,
                        ItemId = label.ItemId,
                        ItemCode = label.ItemCode,
                        ItemExtProp = label.ItemExtProp,
                        ItemExtPropName = label.ItemExtPropName,
                        WorkOrderId = label.WorkOrderId ?? 0,
                        WarehouseId = label.WarehouseId,
                        WarehouseCode = label.WarehouseCode,
                        MoveQty = label.Qty
                    };
                    RT.Service.Resolve<MaterialMoveRecordController>().LoadItemMove(data);
                    return data.MoveQty;
                }
                return 0;
            }
            else
                return report.AvailableQty;
        }
    }
}
