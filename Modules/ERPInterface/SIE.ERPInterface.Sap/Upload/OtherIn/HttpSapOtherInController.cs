using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;

namespace SIE.ERPInterface.Sap.Upload.OtherIn
{
    /// <summary>
    /// Sap其他入库控制器
    /// </summary>
    public class HttpSapOtherInController : SapUploadController
    {
        #region 其他入库上传

        /// <summary>
        /// 其他入库上传
        /// </summary>      
        public virtual ProcessResult UploadInStorageToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改--对应其他出入库3
            string interfaceName = "其他出入库3上传接口";
            string zifcd = "SMOM0010";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadOtherInStorageHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //处理暂收凭证
            //InStorageSuccessCallBack(processResult.SuccessSapKey);
            return processResult;
        }

       
        /// <summary>
        /// 其他入库接口调用成功数据处理
        /// </summary>
        /// <param name="dic"></param>
        private void InStorageSuccessCallBack(Dictionary<double, string> dic)
        {
            List<double> ids = dic.Keys.ToList();
            EntityList<UploadTransaction> list = ids.SplitContains(items =>
            {
                return Query<UploadTransaction>().Where(m => items.Contains(m.Id)).ToList();
            });
            using (var trans = DB.TransactionScope(SapInterfaceEntityDataProvider.ConnectionStringName))
            {
                foreach (var kvp in dic)
                {
                    var item = list.FirstOrDefault(c => c.Id == kvp.Key);
                    if (item == null)
                        continue;
                    if (!item.BillLineId.HasValue)
                        continue;
                    //DB.Update<AsnDetail>().Set(c => c.SapInStorageNo, kvp.Value).Where(c => c.Id == item.BillLineId).Execute();
                }
                trans.Complete();
            }
        }

        #endregion     
    }
}
