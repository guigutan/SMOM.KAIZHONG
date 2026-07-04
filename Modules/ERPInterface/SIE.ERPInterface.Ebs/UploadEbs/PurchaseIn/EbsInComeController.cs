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
    /// 来料暂收控制器
    /// </summary>
    public class EbsInComeController : EbsUploadBaseController
    {
        /// <summary>
        /// 采购订单入库暂收上传数据到EBS
        /// </summary>
        /// <param name="tuples">单据大类，事务类型</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult UploadToEbs(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            var ebsPara = EbsHelper.GetEbsParameter(false);
            var uploadBaseCtl = RT.Service.Resolve<UploadBaseController>();

            // 接口编码
            ebsPara.InterfaceCode = "S_W2E_PO_RCV";
            ebsPara.UploadJobType = JobType.Receive;
            ebsPara.OrderType = OrderType.PurchaseIn;

            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<InComeUploadData> inComeUploadDatas = new List<InComeUploadData>();
            uploadTransactions.Where(p => p.OrderType == OrderType.PurchaseIn && p.TransactionType == TransactionType.Receive && p.PoIsReturnErp == true
            && p.ErpWarehouseCode.IsNotEmpty() && p.PoErpKey.IsNotEmpty() && p.PoLineErpKey.IsNotEmpty())
                .ForEach(f =>
                {
                    InComeUploadData item = new InComeUploadData();
                    GetEbsUploadDataBase(f, item);//类型必改
                    item.TransactionType = "RECEIVE";
                    item.ItemCode = f.ItemCode;
                    item.ReceiveNo = f.BillNo;

                    item.PurchaseNo = f.PoErpKey;
                    item.BillLineErpKey = f.PoLineErpKey;
                    inComeUploadDatas.Add(item);

                    string cusStr =
                    @"{
            ""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"":""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",          
            ""ORGANIZATION_NAME"":""{4}"",
            ""TRANSACTION_TYPE"": ""{5}"",
            ""RECEIPT_NUM"":""{6}"",
            ""PO_NUMBER"":""{7}"",
            ""PO_LINE_NUMBER"": ""{8}"",
            ""ITEM_CODE"": ""{9}"",
            ""UOM_CODE"": ""{10}"",
            ""QUANTITY"": ""{11}"",
            ""TRANSACTION_DATE"": ""{12}""
            }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrgName, item.OrganizationName, item.TransactionType, item.ReceiveNo,
            item.PurchaseNo, item.BillLineErpKey, item.ItemCode, item.UnitCode, item.Quantity, item.TransactionDate);
                    item.RequestStr = cusStr;
                });
            ebsPara.UploadStr = SetUploadStr(inComeUploadDatas);
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(inComeUploadDatas, tuple.Item2, ebsPara);
            }

            return tuple.Item1;
        }
    }
}
