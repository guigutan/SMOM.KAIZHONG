using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.Transactions;

namespace SIE.ERPInterface.Sap.Upload.Finished
{

    /// <summary>
    /// SAP成品入库控制器
    /// </summary>
    public class HttpSapFinishedController : SapUploadController
    {
        #region 成品入库上传

        /// <summary>
        /// 成品入库上传
        /// </summary>      
        public virtual ProcessResult UploadInStorageToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "成品入库上传接口";
            string zifcd = "SMOM0008";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadFinishedInStorageHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //处理入库凭证
            InStorageSuccessCallBack(processResult.SuccessSapKey);
            return processResult;
        }

        /// <summary>
        /// 成品入库接口调用成功处理数据
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
