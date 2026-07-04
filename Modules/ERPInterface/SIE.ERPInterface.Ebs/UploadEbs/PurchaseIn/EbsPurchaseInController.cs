using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData.Upload;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.UploadEbs.PurchaseIn
{
    /// <summary>
    /// 采购入库回传ERP
    /// </summary>
    public class EbsPurchaseInController : EbsUploadBaseController
    {
        /// <summary>
        /// 上传数据到EBS
        /// </summary>
        /// <param name="tuples">单据大类，事务类型</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult UploadToEbs(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            var ebsPara = EbsHelper.GetEbsParameter(false);
            var uploadBaseCtl = RT.Service.Resolve<UploadBaseController>();
            //Copy必改内容
            ebsPara.InterfaceCode = "S_W2E_PO_DELIVER";//接口编码，接口卡有            
            ebsPara.UploadJobType = JobType.PurchaseIn;//Copy必改内容
            ebsPara.OrderType = OrderType.PurchaseIn;//Copy必改内容
            //End
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);//查询未处理事务上传表数据
            List<PurchaseInData> saleOutUploadDatas = new List<PurchaseInData>(); //Copy必改内容
            List<TransactionType> transactionTypes = new List<TransactionType>() {
             TransactionType.RecInStorage,
             TransactionType.InStorage
            };
            uploadTransactions.Where(p => transactionTypes.Contains(p.TransactionType) && !p.PoNo.IsNullOrEmpty() && !p.PoLineErpKey.IsNullOrEmpty() && !p.ErpOrganizationName.IsNullOrEmpty()).ForEach(f =>
             {
                 PurchaseInData item = new PurchaseInData();
                 GetEbsUploadDataBase(f, item);//类型必改//非基类的字段要自己写赋值
                 item.ItemCode = f.ItemCode;//Copy必改内容
                 item.PoNum = f.PoNo;
                 item.ErpDetailId = f.PoLineErpKey;
                 saleOutUploadDatas.Add(item);
                 string cusStr = "";//Copy必改内容
                                    //非基类属性的需要自己写例如
                 cusStr = @"{
                ""SCUX_SOURCE_NUM"":""{0}"",
                ""SCUX_SOURCE_LINE_NUM"":""{1}"",
                ""SCUX_SOURCE_LOT_NUM"":""{2}"",
                ""ORGANIZATION_NAME"":""{3}"",
                ""PO_NUMBER"":""{4}"",
                ""LINE_NUMBER"":""{5}"",
                ""RECEIPT_NUM"": ""{0}"",
                ""TRANSACTION_TYPE"": ""DELIVER"",
                ""TRANSACTION_DATE"": ""{6}"",
                ""ITEM_NUM"": ""{7}"",
                ""QUANTITY"": ""{8}"",
                ""UOM_CODE"": ""{9}"",
                ""SUBINVENTORY"": ""{10}"",
                ""LOT_NUMBER"": ""{11}""
                }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.PoNum, item.ErpDetailId, item.TransactionDate, item.ItemCode, item.Quantity, item.UnitCode, item.ErpWarehouseCode, f.ProductBatch);
                 //item.RequestStr = GetRequireStr(item, cusStr);
                 item.RequestStr = cusStr;
             });
            ebsPara.UploadStr = SetUploadStr(saleOutUploadDatas);
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(saleOutUploadDatas, tuple.Item2, ebsPara);
            }

            return tuple.Item1;
        }
    }
}
