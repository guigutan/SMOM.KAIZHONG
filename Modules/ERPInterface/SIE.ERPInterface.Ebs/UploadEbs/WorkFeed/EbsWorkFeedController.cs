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

namespace SIE.ERPInterface.Ebs.UploadEbs.WorkFeed
{
    /// <summary>
    /// 工单发料控制器
    /// </summary>
    public class EbsWorkFeedController : EbsUploadBaseController
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
            ebsPara.InterfaceCode = "S_W2E_WIP_ISSUE";
            ebsPara.UploadJobType = JobType.WorkFeed;
            ebsPara.OrderType = OrderType.WorkFeed;

            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);
            List<WorkFeedUploadData> workFeedUploadDatas = new List<WorkFeedUploadData>();
            uploadTransactions.Where(p => p.OrderType == OrderType.WorkFeed
            && p.ErpWarehouseCode.IsNotEmpty()).ForEach(f =>
            {
                WorkFeedUploadData item = new WorkFeedUploadData();
                GetEbsUploadDataBase(f, item);//类型必改
                //非基类的字段要自己写赋值
                item.WoNo = f.WoNo;
                item.ItemCode = f.ItemCode;
                item.TransactionType = "SCUX_WIP_ISSUE";
                workFeedUploadDatas.Add(item);
                string cusStr = "";//Copy必改内容
                //非基类属性的需要自己写例如
                cusStr =
                @"{""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"": ""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",
            ""ORGANIZATION_NAME"": ""{3}"",
            ""WIP_ENTITY_NAME"":""{4}"",
            ""ITEM_CODE"":""{5}"",
            ""TRANSACTION_UOM"": ""{6}"",
            ""TRANSACTION_TYPE"":""{7}"",
            ""TRANSACTION_DATE"": ""{8}"",
            ""SUBINVENTORY"": ""{9}"",           
            ""QUANTITY"": ""{10}"",
            ""LOT_NUMBER"":""{11}""}".FormatArgs(item.BillNo, item.LineNo, item.TranId, item.OrganizationName, item.WoNo, item.ItemCode, item.UnitCode,
            item.TransactionType, item.TransactionDate, item.ErpWarehouseCode, -item.Quantity, item.ProductBatch);
                item.RequestStr = cusStr;
            });
            ebsPara.UploadStr = SetUploadStr(workFeedUploadDatas);
            var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
            if (tuple.Item2.Count > 0)
            {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑
                tuple.Item1.TailMsg = AddEbsUploadLog(workFeedUploadDatas, tuple.Item2, ebsPara);
            }

            return tuple.Item1;
        }
    }
}
