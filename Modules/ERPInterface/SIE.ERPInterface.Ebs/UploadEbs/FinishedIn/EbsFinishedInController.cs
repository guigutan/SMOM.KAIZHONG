using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData.Upload;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SIE.ERPInterface.Ebs.UploadEbs.FinishedIn
{
    /// <summary>
    /// 完工入库(成品/半成品)
    /// </summary>
    public class EbsFinishedInController : EbsUploadBaseController
    {
        /// <summary>
        /// 上传数据到EBS
        /// </summary>
        /// <param name="tuples">单据大类，事务类型</param>
        /// <param name="type">类型</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult UploadToEbs(List<Tuple<OrderType, TransactionType, double?, string>> tuples, OrderType type)
        {

            var ebsPara = EbsHelper.GetEbsParameter(false);
            var uploadBaseCtl = RT.Service.Resolve<UploadBaseController>();
            //Copy必改内容
            ebsPara.InterfaceCode = "S_W2E_WIP_COMPLATE";//接口编码，接口卡有

            ebsPara.OrderType = type;
            if (type == OrderType.Finished)
                ebsPara.UploadJobType = Common.Enums.JobType.Finished;
            else
                ebsPara.UploadJobType = Common.Enums.JobType.PartedIn;

            //End
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);//查询未处理事务上传表数据
            List<FinishedInUploadData> saleOutUploadDatas = new List<FinishedInUploadData>(); //Copy必改内容
            uploadTransactions.Where(p => p.ErpWarehouseCode.IsNotEmpty() && p.ErpOrganizationName.IsNotEmpty() && p.WoNo.IsNotEmpty()).ForEach(f =>
              {
                  FinishedInUploadData item = new FinishedInUploadData();
                  GetEbsUploadDataBase(f, item);//类型必改
                                                //非基类的字段要自己写赋值
                  item.WoNo = f.WoNo;                             //item.ItemCode = f.ItemCode;//Copy必改内容
                  saleOutUploadDatas.Add(item);
                  string cusStr = "";//Copy必改内容
                                     //非基类属性的需要自己写例如
                  cusStr = @"{
                ""SCUX_SOURCE_NUM"":""{0}"",
                ""SCUX_SOURCE_LINE_NUM"":""{1}"",
                ""SCUX_SOURCE_LOT_NUM"":""{2}"",
                ""ORGANIZATION_NAME"":""{3}"",
                ""WIP_ENTITY_NAME"":""{9}"",
                ""INVENTORY_ITEM"":""{4}"",
                ""TRANSACTION_DATE"":""{5}"",
                ""SUBINVENTORY"":""{6}"",
                ""QUANTITY"":""{7}"",
                ""TRANSACTION_UOM"":""{8}"",
                ""LOT_NUMBER"":""{10}""
                }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, f.ItemCode, item.TransactionDate, item.ErpWarehouseCode, item.Quantity, item.UnitCode, item.WoNo, f.ProductBatch);
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
