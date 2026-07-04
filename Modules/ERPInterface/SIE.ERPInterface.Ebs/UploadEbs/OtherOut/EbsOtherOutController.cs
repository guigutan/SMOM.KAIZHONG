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

namespace SIE.ERPInterface.Ebs.UploadEbs.OtherOut
{
    /// <summary>
    /// 杂项出库控制器
    /// </summary>
    public class EbsOtherOutController : EbsUploadBaseController
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
            ebsPara.InterfaceCode = "S_W2E_INV_MISC";
           
            ebsPara.UploadJobType = JobType.OtherOut;
            ebsPara.OrderType = OrderType.OtherOut;

            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<OtherOutUploadData> otherOutUploadDatas = new List<OtherOutUploadData>();
            uploadTransactions.Where(p => p.OrderType == OrderType.OtherOut
            && p.ErpWarehouseCode.IsNotEmpty() && p.AllotModel != Inventory.Commom.AllotModel.ACROSS).ForEach(f =>
            {
                OtherOutUploadData item = new OtherOutUploadData();
                GetEbsUploadDataBase(f, item);
                item.TransactionType = "账户别名发放";
                item.ItemCode = f.ItemCode;
                item.ErpAccount = f.ErpAccount;
                otherOutUploadDatas.Add(item);
                string cusStr = "";
                cusStr = @"{""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"": ""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",
            ""ORGANIZATION_NAME"": ""{3}"",
            ""TRANSACTION_TYPE"":""{4}"",
            ""TRANSACTION_DATE"":""{5}"",
            ""TRANSACTION_SOURCE_NAME"": ""{6}"",
            ""SUBINVENTORY_CODE"":""{7}"",
            ""ITEM_CODE"": ""{8}"",           
            ""TRANSACTION_UOM"": ""{9}"",
            ""TRANSACTION_QUANTITY"": ""{10}"",
            ""LOT_NUMBER"":""{11}""}".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.TransactionType, item.TransactionDate,
            item.ErpAccount, item.ErpWarehouseCode, item.ItemCode, item.UnitCode, item.Quantity, item.ProductBatch);
                item.RequestStr = cusStr;
            });
            ebsPara.UploadStr = SetUploadStr(otherOutUploadDatas);
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(otherOutUploadDatas, tuple.Item2, ebsPara);
            }

            return tuple.Item1;

        }
    }
}
