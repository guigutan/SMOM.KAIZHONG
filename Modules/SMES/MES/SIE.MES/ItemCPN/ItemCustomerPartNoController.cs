using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemCPN
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemCustomerPartNoController : DomainController
    {
        /// <summary>
        /// 获取客户料码数据
        /// </summary>
        public virtual EntityList<ItemCustomerPartNo> GetItemCusotmerDataAll(ItemCustomerPartNoCriteria criteria)
        {
            // 使用视图进行查询
            var q = DB.Query<ItemCustomerPartNo>("icpn");

            // 物料编码查询
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode));

            // 物料名称查询
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.Item.Name.Contains(criteria.ItemName));

            // 物料客户查询
            if (!criteria.Customer.IsNullOrEmpty())
                q.Where(p => p.Customer.Contains(criteria.Customer));

            // 物料客户料码查询
            if (!criteria.CodeAlias.IsNullOrEmpty())
                q.Where(p => p.CodeAlias.Contains(criteria.CodeAlias));

            // 执行查询
            var list = q//.Where(p => p.InvOrgId == RT.InvOrg)
                        .OrderBy(criteria.OrderInfoList)
                        .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return list;
        }
    }
}
