using DocumentFormat.OpenXml.Spreadsheet;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemCusotmerDataController : DomainController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetShiftDate()
        {
            var Shift = ShiftAlgorithmGetCode();
            //一个识别日期的流水号
            var curTime = DateTime.Now.Date;
            //当晚班的时候 ，可能会出现隔天的情况，这个时间仍然算是昨天的流水
            if (Shift == "B")
            {
                //如果出现这种在隔天的0点到8点的情况，那么就算是昨天的夜班还在继续（切记一定要小于8点，如果只是大于0，那么隔天20点以后的晚班仍然会算成昨天的，这样是不对的）
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
                {
                    curTime = DateTime.Now.Date.AddDays(-1);
                }
            }
            return curTime;
        }

        /// <summary>
        /// (早A晚B)
        /// </summary>
        /// <returns></returns>
        public virtual string ShiftAlgorithmGetCode()
        {
            //早八到晚八为A，晚八到早八为B
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
                return "A";
            else
                return "B";
        }

        /// <summary>
        /// 获取客户料码数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="customer"></param>
        /// <param name="batchNo"></param>
        /// <param name="lineCode"></param>
        public virtual ItemCusotmerData GetItemCusotmerData(double itemId, string customer = null, string batchNo = null, string lineCode = null)
        {
            var q = Query<ItemCusotmerData>().Where(p => p.ItemId == itemId);
            if (customer.IsNotEmpty())
                q.Where(p => p.Customer == customer);
            var data = q.FirstOrDefault();
            if (data != null)
            {
                data.BatchNo = batchNo;
                data.LineCode = lineCode;
            }
            return data;
        }
    }
}
