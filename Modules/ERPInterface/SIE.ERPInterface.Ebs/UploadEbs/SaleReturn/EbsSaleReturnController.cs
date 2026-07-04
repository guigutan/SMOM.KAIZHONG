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
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Ebs.UploadEbs.SaleReturn
{
    /// <summary>
    /// 销售退货入库控制器
    /// </summary>
    public class EbsSaleReturnController : EbsUploadBaseController
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
            ebsPara.InterfaceCode = "S_W2E_OM_RETURN_TO_INV";//接口编码，接口卡有          
            ebsPara.UploadJobType = JobType.SaleReturn;
            ebsPara.OrderType = OrderType.SaleReturn;
            //End
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);//查询未处理事务上传表数据
            List<SaleReturnUploadData> saleOutUploadDatas = new List<SaleReturnUploadData>(); //Copy必改内容
            uploadTransactions.Where(f=>f.BillLineErpKey.IsNotEmpty() && f.ErpOrganizationName.IsNotEmpty() && f.ErpWarehouseCode.IsNotEmpty()).ForEach(f =>
            {
                SaleReturnUploadData item = new SaleReturnUploadData();
                GetEbsUploadDataBase(f, item);//类型必改
                //非基类的字段要自己写赋值
                //item.ItemCode = f.ItemCode;//Copy必改内容
                saleOutUploadDatas.Add(item);
                string cusStr = "";//Copy必改内容
                //非基类属性的需要自己写例如
                cusStr = @"{
                ""SCUX_SOURCE_NUM"":""{0}"",
                ""SCUX_SOURCE_LINE_NUM"":""{1}"",
                ""SCUX_SOURCE_LOT_NUM"":""{2}"",
                ""ORGANIZATION_NAME"":""{3}"",
                ""RECEIPT_DATE"":""{4}"",
                ""LINE_ID"":""{5}"",
                ""TRANSACTION_UOM"":""{6}"",
                ""QUANTITY"":""{7}"",
                ""SUBINVENTORY"":""{8}"",
                ""LOT_NUMBER"":""{9}""
                }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.TransactionDate,f.BillLineErpKey,item.UnitCode,item.Quantity,item.ErpWarehouseCode,item.ProductBatch);
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
