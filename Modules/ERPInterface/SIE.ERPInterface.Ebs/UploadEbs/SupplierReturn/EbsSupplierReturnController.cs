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

namespace SIE.ERPInterface.Ebs.UploadEbs.SupplierReturn
{
    /// <summary>
    /// 采购退货控制器
    /// </summary>
    public class EbsSupplierReturnController : EbsUploadBaseController
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

            // 接口编码
            ebsPara.InterfaceCode = "S_W2E_PO_RETURN_VENDOR";
            ebsPara.UploadJobType = JobType.SupplierReturn;
            ebsPara.OrderType = OrderType.SupplierReturn;

            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<SupplierReturnUploadData> pucharseReturnUploadDatas = new List<SupplierReturnUploadData>();
            uploadTransactions.Where(p => p.OrderType == ebsPara.OrderType && (p.Transaction.Code == "SupplierReturn" || p.Transaction.Code == "供应商退货") &&
            p.ErpWarehouseCode.IsNotEmpty() && p.PoErpKey.IsNotEmpty() && p.PoLineErpKey.IsNotEmpty()).ForEach(f =>
            {
                SupplierReturnUploadData item = new SupplierReturnUploadData();
                GetEbsUploadDataBase(f, item);//类型必改
                //非基类的字段要自己写赋值
                item.PoErpKey = f.PoErpKey;
                item.PoLineErpKey = f.PoLineErpKey;
                item.ItemCode = f.ItemCode;
                item.TransactionType = "RETURN TO VENDOR";
                pucharseReturnUploadDatas.Add(item);
                string cusStr = "";//Copy必改内容
                //非基类属性的需要自己写例如
                cusStr =
                    @"{
            ""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"":""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",            
            ""ORGANIZATION_NAME"":""{4}"",
            ""PO_NUMBER"": ""{5}"",
            ""LINE_NUMBER"":""{6}"",
            ""TRANSACTION_TYPE"":""{7}"",
            ""TRANSACTION_DATE"": ""{8}"",
            ""ITEM_NUM"": ""{9}"",
            ""QUANTITY"": ""{10}"",
            ""UOM_CODE"": ""{11}"",
            ""SUBINVENTORY"": ""{12}"",
            ""LOT_NUMBER"" : ""{13}""
            }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrgName, item.OrganizationName, item.PoErpKey, item.PoLineErpKey,
            item.TransactionType, item.TransactionDate, item.ItemCode, item.Quantity, item.UnitCode, item.ErpWarehouseCode, item.ProductBatch);
                item.RequestStr = cusStr;
            });
            ebsPara.UploadStr = SetUploadStr(pucharseReturnUploadDatas);
            if (ebsPara.UploadStr.IsNotEmpty())
            {
                var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
                if (tuple.Item2.Count > 0)
                {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                    tuple.Item1.TailMsg = AddEbsUploadLog(pucharseReturnUploadDatas, tuple.Item2, ebsPara);
                }

                return tuple.Item1;
            }
            else
            {
                return new ProcessResult() { HeadMsg = "没有需要上传的数据".L10N() };
            }
        }
    }
}
