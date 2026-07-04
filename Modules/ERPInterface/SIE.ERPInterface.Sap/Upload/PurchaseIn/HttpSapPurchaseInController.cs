using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;

namespace SIE.ERPInterface.Sap.Upload.PurchaseIn
{


    /// <summary>
    /// Sap采购入库控制器
    /// </summary>
    public class HttpSapPurchaseInController : SapUploadController
    {
        #region 采购送货单暂收上传

        /// <summary>
        /// 采购送货单暂收上传
        /// </summary>      
        public virtual ProcessResult UploadTemporaryReceiveToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "采购送货单暂收上传接口";
            string zifcd = "SMOM0001";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadTemporaryReceiveHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //处理暂收凭证
            TemporaryReceiveSuccessCallBack(processResult.SuccessSapKey);
            return processResult;
        }

        /// <summary>
        /// 暂收更新SAP凭证
        /// </summary>
        /// <param name="dic"></param>
        private void TemporaryReceiveSuccessCallBack(Dictionary<double, string> dic)
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
                    //更新暂收凭证
                    //DB.Update<AsnDetail>().Set(c => c.SapTempReceiveNo, kvp.Value).Where(c => c.Id == item.BillLineId).Execute();
                    //更新采购入库事务交易的暂收凭证
                   
                    DB.Update<InvTransaction>().Set(c => c.SapTempReceiveNo, kvp.Value).Where(c => c.OrderNo == item.BillNo && c.OrderLineNo == item.BillLineNo && c.OrderType == OrderType.SupplierReturn && c.TransactionType == TransactionType.OutStorage).Execute();
                }

                trans.Complete();
            }

        }

        #endregion

        #region 采购入库上传

        /// <summary>
        /// 采购入库上传
        /// </summary>      
        public virtual ProcessResult UploadInStorageToErp(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            //接口名称,必改
            string interfaceName = "采购入库上传接口";
            string zifcd = "SMOM0002";//SAP接口参数，接口卡提供
            var uploadDataHandler = new UploadPurchaseInStorageHandle();
            var processResult = UploadDataToErp(tuples, uploadDataHandler, interfaceName, zifcd);
            //处理暂收凭证
            InStorageSuccessCallBack(processResult.SuccessSapKey);
            return processResult;
        }

        /// <summary>
        /// 采购入库更新SAP凭证
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
