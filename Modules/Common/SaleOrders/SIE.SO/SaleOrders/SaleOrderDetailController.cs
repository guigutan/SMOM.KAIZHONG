using SIE.Common.InvOrg;
using SIE.Core.Common;
using SIE.Domain;
using SIE.EventMessages.APS.SaleOrderEvents;
using SIE.Items;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单明细控制器
    /// </summary>
    public partial class SaleOrderDetailController : DomainController
    {
        /// <summary>
        /// 根据销售订单明细ID获取销售订单明细数据
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns>销售订单明细数据</returns>
        public virtual EntityList<SaleOrderDetail> GetSalesOrderDetails(List<double> idList)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SaleOrderDetail.ItemProperty);
            elo.LoadWith(SaleOrderDetail.EnterpriseProperty);
            elo.LoadWith(SaleOrderDetail.UnitProperty);

            return GetSalesOrderDetailList(idList, elo);
        }


        /// <summary>
        /// 通过订单明细Id列表获取销售订单明细列表
        /// </summary>
        /// <param name="salesOrderDetailIds">Id列表</param>
        /// <param name="elo">贪婪对象</param>
        /// <returns>销售订单明细列表</returns>
        public virtual EntityList<SaleOrderDetail> GetSalesOrderDetailList(List<double> salesOrderDetailIds, EagerLoadOptions elo)
        {
            return salesOrderDetailIds.SplitContains((tmpIds) =>
            {
                return Query<SaleOrderDetail>().Where(p => tmpIds.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取类型的物料信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="Key">关键字</param>
        /// <returns>返回类型为自制或外协的物料信息</returns>
        public virtual EntityList<Item> GetSelfMadeOrOutMadeItem(PagingInfo pagingInfo, String Key)
        {
            var list = Query<Item>().Where(p => (p.Code.Contains(Key) || p.Name.Contains(Key))).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取所有的销售订单明细
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetAllSalesOrderDetailList()
        {
            return Query<SaleOrderDetail>().ToList();
        }

        /// <summary>
        /// 获取销售订单列表
        /// </summary>
        /// <param name="idList">销售订单id集合</param>
        /// <returns></returns>
        public virtual List<SaleOrderDetailObject> GetSaleOrderDetailList(List<double> idList)
        {
            return DataProcessEx.SplitContains(idList, (ids) =>
             {
                 return Query<SaleOrderDetail>().Where(p => ids.Contains(p.SaleOrderId) && p.LineState != LineState.COMPLETE).Select(p => new
                 {
                     SaleOrderId = p.SaleOrderId,
                     SaleOrderDetailId = p.Id,
                     LineNo = p.LineNo,
                     PromiseDelivery = p.PromiseDelivery
                 }).ToList<SaleOrderDetailObject>().ToList();
             });
        }

        /// <summary>
        ///  获取ID获取销售订单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SaleOrderDetailViewModel GetSalesOrderDetail(double id)
        {
            return Query<SaleOrderDetailViewModel>().Where(p => p.Id == id).ToList().FirstOrDefault();
        }


        /// <summary>
        /// 反写销售订单明细特殊工艺字段
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public virtual void SaveSpecialProcessStr(SaleOrderDetail detail)
        {
            String str = string.Empty;
            foreach (var item in detail.SpecialProcessList)
            {
                str += (EnumViewModel.EnumToLabel(item.Process) + ":" + item.Value + ";");
            }
            if (!string.IsNullOrWhiteSpace(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            var SaleOrderDetail = GetById<SaleOrderDetail>(detail.Id);
            if (SaleOrderDetail != null)
            {
                SaleOrderDetail.SpecialProcessStr = str.ToString();
                RF.Save(SaleOrderDetail);
            }
        }

        /// <summary>
        /// 验证销售订单明细是否有引用将要删除的单位
        /// </summary>
        /// <param name="id">单位</param>
        public virtual Int32 ValidateInvolveSaleOrderItem(double id)
        {
            return Query<SaleOrderDetail>().Where(p => p.UnitId == id).Count();
        }

        /// <summary>
        /// 根据销售订单编号获取销售订单信息(不区分库存组织)
        /// </summary>
        /// <param name="codeList">订单编号集合</param>
        /// <param name="elo">贪婪加载对象</param>
        /// <returns>返回销售订单信息</returns>
        public virtual EntityList<SaleOrder> GetSaleOrderByCodes(List<string> codeList, EagerLoadOptions elo = null)
        {
            EntityList<SaleOrder> soList = new EntityList<SaleOrder>();
            using (SIE.Common.InvOrg.InvOrgs.WithAll(true))
            {
                soList = codeList.SplitContains((tmpCodes) =>
                {
                    return Query<SaleOrder>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, elo);
                });
            }

            return soList;
        }

        /// <summary>
        /// 根据回写信息保存销售订单的工厂交期
        /// </summary>
        /// <param name="deliveryEvent"></param>
        public virtual void SavePromiseDelivery(CallBackPromiseDeliveryEvent deliveryEvent)
        {
            List<string> soCodes = deliveryEvent.InfoList.Select(p => p.Code).Distinct().ToList();
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SaleOrder.SaleOrderDetailListProperty);
            EntityList<SaleOrder> soList = GetSaleOrderByCodes(soCodes, elo);
            foreach (var groupInfo in deliveryEvent.InfoList.GroupBy(p => p.Code))
            {
                SaleOrder tmpSo = soList.Where(p => p.Code == groupInfo.Key).OrderBy(p => InvOrgIdExtension.GetInvOrgId(p) == RT.InvOrg).FirstOrDefault();
                if (tmpSo == null) continue;
                foreach (var info in groupInfo)
                {
                    SaleOrderDetail tmpDetail = tmpSo.SaleOrderDetailList.FirstOrDefault(p => p.LineNo == info.LineNo);
                    if (tmpDetail != null)
                    {
                        tmpDetail.PromiseDelivery = info.PromiseDelivery;
                    }
                }
            }

            RF.Save(soList);
        }
    }
}