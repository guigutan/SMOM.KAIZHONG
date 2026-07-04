using SIE.Barcodes;
using SIE.Domain;
using SIE.EventMessages.MES.Barcodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 条码控制器
    /// </summary>
    public partial class WipProductBarcodeController : DomainController
    {
        /// <summary>
        /// 查询条码
        /// </summary>
        /// <param name="criteria">条码查询实体</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>条码列表</returns>
        public virtual EntityList<WipProductBarcode> GetBarcodes(WipProductBarcodeCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            using (Diagnostics.DebugTrace.Start("条码查询：".L10N()))
            {
                var query = Query<WipProductBarcode>();
                if (!criteria.Sn.IsNullOrWhiteSpace())
                {
                    query.Where(p => p.Sn.Contains(criteria.Sn));
                }

                if (!criteria.WorkOrderNo.IsNullOrWhiteSpace())
                {
                    query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
                }

                return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

    }
}
