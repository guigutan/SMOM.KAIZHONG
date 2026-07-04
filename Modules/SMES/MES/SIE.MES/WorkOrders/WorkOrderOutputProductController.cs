using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderOutputProductController : DomainController
    {
        /// <summary>
        /// 是否已存在相同行号
        /// </summary>
        /// <param name="rownumber"></param>
        /// <param name="Id"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual bool IsExsitedRowNumber(string rownumber, double Id,double woId)
        {
            var workOrderOutput = Query<WorkOrderOutputProduct>().Where(p => p.RowNumber == rownumber && p.Id != Id&&p.WorkOrderId==woId).FirstOrDefault();
            if (workOrderOutput != null)
            {
                return true;
            }
            return false;
        }
    }
}
