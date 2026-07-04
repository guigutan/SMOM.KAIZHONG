using MimeKit.Tnef;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OutPurchase;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleOut;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Suppliers;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.WorkFeed;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.OutPurchase
{
    internal class IUploadOutPurchaseInHandle : IUploadDataHandler<SapOrderParamOutPur<UploadOutPurchaseInData>>
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
        /// 上传业务数据接口
        /// </summary>
        /// <param name="uploadTransactions"></param>    
        /// <returns></returns>
        SapUploadParam<SapOrderParamOutPur<UploadOutPurchaseInData>> IUploadDataHandler<SapOrderParamOutPur<UploadOutPurchaseInData>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOrderParamOutPur<UploadOutPurchaseInData>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.Where(p=>p.SupplierId.HasValue &&  !p.BillErpKey.IsNullOrEmpty() && !p.BillLineErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapOrderParamOutPur<UploadOutPurchaseInData>();
                var order = f.OrderByDescending(a => a.TransactionDate).FirstOrDefault();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.TransactionDate);
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.CreateDate);
                f.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new UploadOutPurchaseInData();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    item.BWART = "542"; //移动类型 
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.LGORT = p.ToWarehouseCode;
                    item.MATNR = p.ItemCode;
                    item.ERFMG = p.Quantity;
                    item.ERFME = p.UnitCode;
                    
                    item.EBELN = p.BillErpKey;
                    item.EBELP = p.BillLineErpKey;
                    item.WERKS = invOrg.ExternalId;
                   
                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
