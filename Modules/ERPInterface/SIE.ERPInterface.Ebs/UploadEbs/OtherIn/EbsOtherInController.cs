using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas.EbsData.Upload;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Core.Enums;
using SIE.ERPInterface.Common.Enums;

namespace SIE.ERPInterface.Ebs.UploadEbs.OtherIn
{
    /// <summary>
    /// 其他入库回传ERP
    /// </summary>
    public class EbsOtherInController : EbsUploadBaseController
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
            ebsPara.InterfaceCode = "S_W2E_INV_MISC";//接口编码，接口卡有
          
            ebsPara.UploadJobType = JobType.OtherIn;
            ebsPara.OrderType = OrderType.OtherIn;
            //End
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);//查询未处理事务上传表数据
            List<OtherInUploadData> saleOutUploadDatas = new List<OtherInUploadData>();//Copy必改内容
            var realUploadData = uploadTransactions.Where(p=>!p.ErpWarehouseCode.IsNullOrEmpty() && !p.ErpAccount.IsNullOrEmpty()).ToList();
            realUploadData.ForEach(f =>
            {
                OtherInUploadData item = new OtherInUploadData();
                GetEbsUploadDataBase(f, item);//类型必改                             
                //非基类的字段要自己写赋值
                item.ItemCode = f.ItemCode;//Copy必改内容
                item.ErpAccountCode = f.ErpAccount;
                saleOutUploadDatas.Add(item);
                string cusStr = "";//Copy必改内容
                                       //非基类属性的需要自己写例如
                    cusStr = @"{
                    ""SCUX_SOURCE_NUM"":""{0}"",
                    ""SCUX_SOURCE_LINE_NUM"":""{1}"",
                    ""SCUX_SOURCE_LOT_NUM"":""{2}"",
                    ""ORGANIZATION_NAME"":""{3}"",
                    ""TRANSACTION_DATE"":""{4}"",
                    ""TRANSACTION_TYPE"":""账户别名接收"",
                    ""TRANSACTION_SOURCE_NAME"":""{9}"",
                    ""ITEM_CODE"":""{5}"",
                    ""SUBINVENTORY_CODE"":""{6}"",
                    ""TRANSACTION_UOM"": ""{8}"",
                    ""TRANSACTION_QUANTITY"":""{7}"",
                    ""LOT_NUMBER"":""{10}""
                    }".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.TransactionDate, f.ItemCode, item.ErpWarehouseCode, item.Quantity,item.UnitCode, item.ErpAccountCode,f.ProductBatch);
                 item.RequestStr = cusStr;
            });
            ebsPara.UploadStr = SetUploadStr(saleOutUploadDatas);
            if (ebsPara.UploadStr.IsNullOrEmpty())
            {
                return new ProcessResult();
            }
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(saleOutUploadDatas, tuple.Item2, ebsPara);
            }
            return tuple.Item1;
        }
    }
}
