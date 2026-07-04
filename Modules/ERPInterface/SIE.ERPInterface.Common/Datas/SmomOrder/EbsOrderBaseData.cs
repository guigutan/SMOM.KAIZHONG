using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ERP单据数据基类
    /// </summary>
    [Serializable]
    public class EbsOrderBaseData
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public double OrderId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 业务实体名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 明细Id
        /// </summary>
        public string ErpDetailId { get; set; }
    }

    /// <summary>
    /// 扩展
    /// </summary>
    public static class EbsCollections
    {
        /// <summary>
        /// 以创建时间升序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<T> OrderByLastUpdateDate<T>(this List<T> datas) where T : EbsOrderBaseData
        {
            return datas.OrderBy(p => p.CreateDate).ToList();
        }
    }

}
