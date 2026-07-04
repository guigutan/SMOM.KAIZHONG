using SIE.Api;
using SIE.Core.Enums;
using SIE.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 单据小类API接口控制器
    /// </summary>
    public partial class TransactionController : DomainController
    {
        /// <summary>
        /// 获取单据小类列表
        /// </summary>
        /// <param name="type">单据大类</param>
        /// <returns>单据小类数据</returns>
        [ApiService("获取单据小类列表")]
        [return: ApiReturn("返回单据小类集合:List<Transaction>")]
        public virtual List<Transaction> GetTransactionDatas([ApiParameter("单据大类")] int type)
        {
            OrderType orderType = (OrderType)type;
            var tranDatas = GetTransactions(orderType);
            List<Transaction> datas = tranDatas.ToList();
            return datas;
        }
    }
}
