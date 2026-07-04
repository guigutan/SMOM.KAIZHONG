using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleOut;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.SaleOut
{
    /// <summary>
    /// 销售出库上传业务数据接口实现
    /// </summary>
    public class IUploadSaleOutHandle : IUploadDataHandler<SapOrderParamSale<SapSaleOutUploadDetail>>
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
        /// 上传接口实现
        /// </summary>
        /// <param name="uploadTransactions"></param>
        /// <returns></returns>
        SapUploadParam<SapOrderParamSale<SapSaleOutUploadDetail>> IUploadDataHandler<SapOrderParamSale<SapSaleOutUploadDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOrderParamSale<SapSaleOutUploadDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.Where(f=>!f.BillErpKey.IsNullOrEmpty() && !f.BillLineErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var order = f.OrderByDescending(a => a.TransactionDate).FirstOrDefault();
                var sapOrderParam = new SapOrderParamSale<SapSaleOutUploadDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
               
                sapOrderParam.VBELN = order.BillErpKey;
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.TransactionDate);
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.CreateDate);
                
                f.Where(p => !p.BillErpKey.IsNullOrEmpty() && !p.BillLineErpKey.IsNullOrEmpty()).OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapSaleOutUploadDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);                    
                    
                    item.POSNR = p.BillLineErpKey;
                    item.WERKS = invOrg.ExternalId;//ERP库存组织Id                
                    item.LGORT = p.FromWarehouseCode;
                    item.MENGE = p.Quantity;
                    item.MEINS = p.UnitCode;                   
                    
                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
