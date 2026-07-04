using SIE.EventMessages.Common.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.StockDeducRecords
{
    [Services.Service(FallbackType = typeof(DefaultIStockDeducRecord))]
    public interface IStockDeducRecord
    {
        /// <summary>
        /// 创建扣料记录、上料记录扣料
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="woCostItemId"></param>
        public void DeducRecord(double taskId, double woCostItemId);
    }

    public class DefaultIStockDeducRecord : IStockDeducRecord
    {
        public void DeducRecord(double taskId, double woCostItemId)
        {
            throw new NotImplementedException();
        }
    }
}
