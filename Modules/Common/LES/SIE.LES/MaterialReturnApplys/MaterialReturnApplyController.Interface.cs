using SIE.Domain;
using SIE.EventMessages.LES;
using SIE.EventMessages.LES.Datas;
using SIE.LES.Reports;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请控制器
    /// </summary>
    public partial class MaterialReturnApplyController : DomainController, ILesMaterialReturn
    {
        /// <summary>
        /// 发货更新备料需求
        /// </summary>
        /// <param name="datas"></param>
        public virtual void UpDateReturnApply(List<ReturnUpdateData> datas)
        {
            using (SIE.Common.InvOrg.InvOrgs.With(new List<double>() { datas.First().InvOrgId }))
            {
                var applyNos = datas.Select(p => p.ReturnApplyNo).Distinct().ToList();
                var applyIds = GetMaterialReturnApplyIdsByNos(applyNos);
                var applyDtlDic = GetMaterialReturnDtlDic(applyIds);

                using(var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    // 更新退料单主表状态为已完成
                    DB.Update<MaterialReturnApply>().Set(p => p.ReStatus, Enums.ReStatus.Finished).Where(p => applyIds.Contains(p.Id)).Execute();

                    foreach (var data in datas)
                    {
                        var detailKey = data.ReturnApplyNo + "-" + data.LineNo + "-" + data.ItemCode + "-" + data.ItemExtProp;
                        if (applyDtlDic.TryGetValue(detailKey, out var detail))
                        {
                            //更新明细在途数收货数
                            DB.Update<MaterialReturnApplyDetail>()
                                .Set(p => p.OnWayQty, p => p.OnWayQty - data.ReceiveQty)
                                .Set(p => p.CollectQty, p=> p.CollectQty+ data.ReceiveQty)
                                .Where(p => p.Id == detail.Id).Execute();
                            var woRemId = detail.WoDemandReportId;
                            var itemLabelId = detail.ItemLabelId;
                            var quality = detail.ReDetailQuality;
                            // 更新工单需求汇总良品
                            if (quality == 0)
                            {
                                if (woRemId != null)
                                {
                                    DB.Update<WoDemandReport>()
                                    .Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit - data.ReceiveQty)
                                    .Set(p => p.ReturnQty, p => p.ReturnQty + data.ReceiveQty)
                                    .Where(p => p.Id == woRemId).Execute();
                                }
                                if (itemLabelId != null)
                                {
                                    DB.Update<ItemLabel>()
                                        .Set(p => p.ReturnQtyInTransit, p => p.ReturnQtyInTransit - data.ReceiveQty)
                                        .Set(p => p.Qty, p => p.Qty + data.ReceiveQty)
                                        .Where(p => p.Id == itemLabelId).Execute();
                                }
                            }
                            else
                            {
                                if (woRemId != null)
                                {
                                    DB.Update<WoDemandReport>()
                                    .Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit - data.ReceiveQty)
                                    .Set(p => p.NgReturnQty, p => p.NgReturnQty + data.ReceiveQty)
                                    .Where(p => p.Id == woRemId).Execute();
                                }
                                if (itemLabelId != null)
                                {
                                    DB.Update<ItemLabel>()
                                        .Set(p => p.NgReturnQtyInTransit, p => p.NgReturnQtyInTransit - data.ReceiveQty)
                                        .Set(p => p.NgQty, p => p.NgQty + data.ReceiveQty)
                                        .Where(p => p.Id == itemLabelId).Execute();
                                }
                            }
                        }
                    }
                    tran.Complete();
                }
                
            }
        }
    }
}
