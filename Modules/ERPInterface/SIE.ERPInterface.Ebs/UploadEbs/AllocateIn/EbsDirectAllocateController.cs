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

namespace SIE.ERPInterface.Ebs.UploadEbs.AllocateIn
{
    /// <summary>
    /// 库存调拨
    /// </summary>
    public class EbsDirectAllocateController : EbsUploadBaseController
    {
        /// <summary>
        /// 上传数据到EBS
        /// </summary>
        /// <param name="tuples"></param>
        /// <returns></returns>
        public virtual ProcessResult UploadToEbs(List<Tuple<OrderType, TransactionType, double?, string>> tuples)
        {
            var ebsPara = EbsHelper.GetEbsParameter(false);
            var uploadBaseCtl = RT.Service.Resolve<UploadBaseController>();

            // 接口编码
            ebsPara.InterfaceCode = "S_W2E_INV_TRANSFER";            
            ebsPara.UploadJobType = JobType.DirectAllocate;
            ebsPara.OrderType = OrderType.DirectAllocate;

            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<AllocateInUploadData> allocateInDatas = new List<AllocateInUploadData>();
            // 过滤来源子库与目标子库 都有值，且不同
            uploadTransactions.Where(p => p.ErpWarehouseCode.IsNotEmpty()
            && p.TargetErpWarehouseCode.IsNotEmpty()
            && p.ErpWarehouseCode != p.TargetErpWarehouseCode).ForEach(f =>
            {
                AllocateInUploadData item = new AllocateInUploadData();
                GetEbsUploadDataBase(f, item);
                item.ItemCode = f.ItemCode;
                item.TransactionType = "Subinventory Transfer";
                item.FromLocationCode = f.FromLocationCode;
                item.TargetErpWarehouseCode = f.TargetErpWarehouseCode;
                item.ToLocationCode = f.ToLocationCode;
                allocateInDatas.Add(item);
                string cusStr = "";
                cusStr =
                @"{
            ""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"": ""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",
            ""ORGANIZATION_NAME"": ""{3}"",
            ""TRANSACTION_TYPE"":""{4}"",
            ""TRANSACTION_DATE"": ""{5}"",
            ""SUBINVENTORY_CODE"":""{6}"",
            ""TRANSFER_SUBINVENTORY"": ""{7}"",
            ""TRANSACTION_QUANTITY"":""{8}"",
            ""ITEM_CODE"":""{9}"",
            ""TRANSACTION_UOM"": ""{10}"",
            ""LOT_NUMBER"":""{11}""}".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.TransactionType, item.TransactionDate, item.ErpWarehouseCode,
            item.TargetErpWarehouseCode, item.Quantity,item.ItemCode, item.UnitCode, item.ProductBatch);
                item.RequestStr = cusStr;
            });
            ebsPara.UploadStr = SetUploadStr(allocateInDatas);
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(allocateInDatas, tuple.Item2, ebsPara);
            }

            return tuple.Item1;
        }
    }
}
