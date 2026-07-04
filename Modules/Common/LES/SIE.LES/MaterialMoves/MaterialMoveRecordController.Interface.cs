using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.EventMessages.LES.Datas;
using SIE.Inventory.Onhands;
using SIE.LES.MaterialMoves.ApiModels;
using SIE.LES.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialMoves
{
    /// <summary>
    /// 挪料控制器
    /// </summary>
    public partial class MaterialMoveRecordController : DomainController, ILesMaterialMove
    {
        /// <summary>
        /// 上料挪料
        /// </summary>
        /// <param name="data"></param>
        public virtual void LoadItemMove(LoadItemMoveData data)
        {
            using(SIE.Common.InvOrg.InvOrgs.With(new List<double>() { data.InvOrgId }))
            {
                var query = Query<LotLpnOnhand>().Where(l => l.ItemId == data.ItemId 
                && l.ItemExtProp == data.ItemExtProp && l.ItemExtPropName == data.ItemExtPropName && l.WarehouseId == data.WarehouseId
                && l.State == OnhandState.Ok && l.AvailableQty > 0)
                    .GroupBy(l => new { l.WarehouseId, l.ItemId, l.ItemExtProp, l.ItemExtPropName })
                    .Select(l => new
                    {
                        l.ItemId,
                        l.ItemExtProp,
                        l.WarehouseId,
                        l.ItemExtPropName,
                        AvailableQty = l.AvailableQty.SUM(),
                    }).ToList<MoveBomInfo>();

                // 扣减占用库存
                var woDemandDic = Query<WoDemandReport>().Where(wr => wr.ItemId == data.ItemId && wr.ItemExtProp == data.ItemExtProp && wr.WarehouseId == data.WarehouseId)
                    .GroupBy(wr => new {wr.ItemId, wr.ItemExtProp})
                    .Select(wr => new
                    {
                        ItemId = wr.ItemId,
                        ItemExtProp = wr.ItemExtProp,
                        ReceivedQty = wr.ReceivedQty.SUM(),
                        MovedInQty = wr.MovedInQty.SUM(),
                        FeedQty = wr.FeedQty.SUM(),
                        NgQty = wr.NgQty.SUM(),
                        MovedOutQty = wr.MovedOutQty.SUM(),
                        ReturnQtyInTransit = wr.ReturnQtyInTransit.SUM(),
                        NgReturnQtyInTransit = wr.NgReturnQtyInTransit.SUM(),
                        ReturnQty = wr.ReturnQty.SUM(),
                        NgReturnQty = wr.NgReturnQty.SUM()
                    }).ToList<MoveBomInfo>().ToDictionary(p => new Tuple<double, string>(p.ItemId, p.ItemExtProp), p => p);

                var lpn = query.FirstOrDefault();
                if (lpn == null)
                {
                    throw new ValidationException("物料[{0}]在仓库[{1}]无LPN库存".L10nFormat(data.ItemCode, data.WarehouseCode));
                }
                else
                {
                    if (woDemandDic.TryGetValue(new Tuple<double, string>(data.ItemId, data.ItemExtProp), out MoveBomInfo wr))
                    {
                        lpn.AvailableQty -= wr.ReceivedQty + wr.MovedInQty - wr.FeedQty - wr.NgQty - wr.MovedOutQty - wr.ReturnQtyInTransit - wr.NgReturnQtyInTransit - wr.ReturnQty - wr.NgReturnQty;
                    }

                }

                if (lpn.AvailableQty < data.MoveQty)
                {
                    throw new ValidationException("挪料数量[{0}]大于LPN库存数[{1}]".L10nFormat(data.MoveQty, lpn.AvailableQty));
                }
                else
                {
                    lpn.MoveQty = data.MoveQty;
                }
                // 提交挪料并生成一笔记录
                SubmitWorkOrderMove(null, data.WorkOrderId, data.WarehouseId, "上料挪料".L10N(), new List<MoveBomInfo> { lpn }, false);
            }
        }
    }
}
