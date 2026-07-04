using MimeKit.Tnef;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleOut;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Suppliers;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.WorkFeed;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.MaterialReturn
{
    /// <summary>
    /// 工单退料事务上传
    /// </summary>
    internal class IUploadMaterialReturnHandle : IUploadDataHandler<SapOrderParamWorkFeed<WorkFeedUploadData>>
    {
        public Dictionary<string, List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions)
        {
            throw new NotImplementedException();
        }

        public string SetParam(EntityList<UploadTransaction> uploadTransactions)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 工单退料事务上传
        /// </summary>
        /// <param name="uploadTransactions"></param>    
        /// <returns></returns>
        SapUploadParam<SapOrderParamWorkFeed<WorkFeedUploadData>> IUploadDataHandler<SapOrderParamWorkFeed<WorkFeedUploadData>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOrderParamWorkFeed<WorkFeedUploadData>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();

            uploadTransactions.Where(f=>!f.BillErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapOrderParamWorkFeed<WorkFeedUploadData>();
                var order = f.Where(p=>!p.BillErpKey.IsNullOrEmpty()).OrderByDescending(a => a.TransactionDate).FirstOrDefault();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.TransactionDate);
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.CreateDate);
                f.Where(p => !p.BillErpKey.IsNullOrEmpty()).OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new WorkFeedUploadData();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    item.BWART = "262"; //移动类型 
                    item.WERKS = invOrg.ExternalId;//ERP库存组织Id
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.LGORT = p.ToWarehouseCode;
                    item.ERFMG = p.Quantity;
                    item.ERFME = p.UnitCode;
                    item.AUFNR = p.BillErpKey;                  
                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
