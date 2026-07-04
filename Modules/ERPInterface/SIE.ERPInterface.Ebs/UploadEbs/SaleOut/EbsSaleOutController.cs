using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Inventory.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Upload
{
    /// <summary>
    /// 销售出库控制器
    /// </summary>
    public class EbsSaleOutController : EbsUploadBaseController
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
            ebsPara.InterfaceCode = "S_W2E_OM_MOVE_ORDER";//接口编码，接口卡有
          
            ebsPara.UploadJobType =  JobType.SaleOut;
            ebsPara.OrderType = OrderType.SaleOut;

            //End
            //构建上传接口参数明细           
            var uploadTransactions = uploadBaseCtl.GetUploadTransactions(tuples);//查询未处理事务上传表数据
            List<SaleOutUploadData> saleOutUploadDatas = new List<SaleOutUploadData>(); //Copy必改内容
            uploadTransactions.ForEach(f =>
            {
                SaleOutUploadData item = new SaleOutUploadData();
                GetEbsUploadDataBase(f, item); //类型必改
                //非基类的字段要自己写赋值
                item.ItemCode = f.ItemCode;//Copy必改内容
                saleOutUploadDatas.Add(item);
                //非基类属性的需要自己写例如
                
                item.RequestStr = GetRequireStr(item, "");

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
