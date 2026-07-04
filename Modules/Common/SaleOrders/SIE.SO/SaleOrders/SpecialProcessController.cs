using SIE.Domain;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 特殊工艺控制器
    /// </summary>
    public class SpecialProcessController : DomainController
    {

        /// <summary>
        /// 根据销售订单明细ID获取特殊工艺数据
        /// </summary>
        /// <param name="lstOrderDetailIds">销售单明细ID集合</param>
        /// <returns></returns>
        public virtual EntityList<SpecialProcess> GetDataByOrderDetail(List<double> lstOrderDetailIds)
        {
            return lstOrderDetailIds.SplitContains((tmpIds) =>
            {
                return Query<SpecialProcess>().Where(p => tmpIds.Contains(p.SaleOrderDetailId)).ToList();
            });
        }

        /// <summary>
        /// 根据销售订单明细ID获取特殊工艺表明细
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns>销售订单明细数据</returns>
        public virtual String SaveSpecialProcess(EntityList<SpecialProcess> idList)
        {
            if (idList == null && idList.DeletedList == null)
                return null;

            EntityList<SpecialProcess> AllSp = new EntityList<SpecialProcess>();
            if (idList != null)
            {
                AllSp.AddRange(idList);
            }
            if (idList.DeletedList != null)
            {
                AllSp.AddRange(idList.DeletedList);
            }
            var SaleOrderDetailId = AllSp[0].SaleOrderDetailId;
            foreach (var item in AllSp)
            {
                item.CreateBy = RT.IdentityId;
                item.CreateDate = DateTime.Now;
                item.UpdateBy = RT.IdentityId;
                item.UpdateDate = DateTime.Now;
                RF.Save(item);
            }
            var SaleOrderDetail = GetById<SaleOrderDetail>(SaleOrderDetailId);
            StringBuilder str = new StringBuilder();

            foreach (var item in SaleOrderDetail.SpecialProcessList)
            {
                str.Append(EnumViewModel.EnumToLabel(item.Process) + ":" + item.Value + " ");
            }
            SaleOrderDetail.SpecialProcessStr = str.ToString();
            RF.Save(SaleOrderDetail);
            return str.ToString();
        }
    }
}
