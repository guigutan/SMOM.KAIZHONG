using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData.Upload;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.UploadEbs.CrossAllocate
{
    /// <summary>
    /// 跨组织调拨接口
    /// </summary>
    public class EbsCrossAllocateController : EbsUploadBaseController
    {
        /// <summary>
        /// 上传数据到EBS
        /// </summary>
        /// <param name="tuples"></param>
        /// <param name="type">1- 出库 2-入库</param>
        /// <returns></returns>
        public virtual ProcessResult UploadToEbs(List<Tuple<OrderType, TransactionType, double?, string>> tuples, int type)
        {
            var ebsPara = EbsHelper.GetEbsParameter(false);
            var uploadBaseCtl = RT.Service.Resolve<UploadBaseController>();

            // 接口编码
            ebsPara.InterfaceCode = "S_W2E_INV_TRANS_ORGANIZATION";
            if (type == 1)
            {
                ebsPara.UploadJobType = JobType.CrossOrgTransferOut;
                ebsPara.OrderType = OrderType.OtherOut;
            }
            else
            {
                ebsPara.UploadJobType = JobType.CrossOrgTransferIn;
                ebsPara.OrderType = OrderType.CrossOrgTransferIn;
            }
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<AllocateInUploadData> allocateInDatas = new List<AllocateInUploadData>();
            List<UploadTransaction> realUploadData = new List<UploadTransaction>();
            if (type == 1)
            {
                realUploadData = uploadTransactions.Where(p => p.AllotModel == Inventory.Commom.AllotModel.ACROSS && p.TargetErpOrganizationName.IsNotEmpty() && p.ErpWarehouseCode.IsNotEmpty()).ToList();
            }
            else
            {
                realUploadData = uploadTransactions.Where(p => p.TargetErpOrganizationName.IsNotEmpty() && p.ErpWarehouseCode.IsNotEmpty() && p.SoNo.IsNotEmpty() && p.BillLineErpKey.IsNotEmpty()).ToList();
            }
            // 过滤来源子库与目标子库 都有值，且不同
            realUploadData.ForEach(f =>
            {
                AllocateInUploadData item = new AllocateInUploadData();
                GetEbsUploadDataBase(f, item);
                item.ItemCode = f.ItemCode;
                //em.TransactionType = "Subinventory Transfer";
                item.FromLocationCode = f.FromLocationCode;
                item.TargetErpWarehouseCode = f.TargetErpWarehouseCode;
                item.ToLocationCode = f.ToLocationCode;
                item.TargetOrganizationName = f.TargetErpOrganizationName;
                if (type == 1)
                {
                    //出库 来源子库为ErpWareCode 目标为空
                    item.TransactionType = "21";
                    item.TargetErpWarehouseCode = "";
                    item.SoNo = f.BillNo;
                    item.SourceBillNo = f.BillNo;
                    item.SourceBillLineNo = item.LineNo;
                }
                else
                {
                    //入库 来源子库为TargetErpWareCode 来源为空
                    item.TransactionType = "12";
                    item.TargetErpWarehouseCode = item.ErpWarehouseCode;
                    item.ErpWarehouseCode = "";
                    item.SoNo = f.SoNo;
                    //入库需要交换库存组织
                    var newTargetOrgName = f.TargetErpOrganizationName;
                    item.TargetOrganizationName = item.OrganizationName;
                    item.OrganizationName = newTargetOrgName;
                    //入库来源单号是入库的发运单号 来源行号也是发运单的行号
                    item.SourceBillNo = f.SoNo;
                    item.SourceBillLineNo = item.BillLineErpKey;

                }
                allocateInDatas.Add(item);
                string cusStr = "";
                cusStr =
               @"{
            ""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"": ""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",
            ""ORGANIZATION_NAME"": ""{3}"",
            ""TRANSFER_ORGANIZATION_NAME"": ""{4}"",
            ""SUBINVENTORY_CODE"": ""{5}"",
            ""TRANSFER_SUBINVENTORY"": ""{6}"",
            ""TRANSACTION_DATE"": ""{7}"",
            ""TRANSACTION_QUANTITY"": ""{8}"",
            ""INVENTORY_ITEM"":""{9}"",
            ""TRANSACTION_UOM"": ""{10}"",
            ""SHIPMENT_NUMBER"":""{11}"",
            ""TRANSACTION_TYPE"":""{12}"",
            ""LOT_NUMBER"":""{13}""
           }".FormatArgs(item.SourceBillNo, item.SourceBillLineNo, item.TranId, item.OrganizationName, item.TargetOrganizationName, item.ErpWarehouseCode, item.TargetErpWarehouseCode,
           item.TransactionDate, item.Quantity, item.ItemCode, item.UnitCode, item.SoNo, item.TransactionType, f.ProductBatch);
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
