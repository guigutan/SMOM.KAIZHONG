using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleReturn;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.SaleReturn
{

    /// <summary>
    /// 销售退货上传（非冲销）业务数据接口实现
    /// </summary>
    public class UploadSaleReturnHandle : IUploadDataHandler<SapSaleReturnUploadData<SapSaleReturnUploadDataDetail>>
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

        SapUploadParam<SapSaleReturnUploadData<SapSaleReturnUploadDataDetail>> IUploadDataHandler<SapSaleReturnUploadData<SapSaleReturnUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapSaleReturnUploadData<SapSaleReturnUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.Where(f=>!f.BillErpKey.IsNullOrEmpty()&&!f.BillLineErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapSaleReturnUploadData<SapSaleReturnUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                sapOrderParam.VBELN = f.FirstOrDefault().BillErpKey;
              
                f.Where(p => !p.BillErpKey.IsNullOrEmpty() && !p.BillLineErpKey.IsNullOrEmpty()).OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapSaleReturnUploadDataDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    //item.BWART = "103"; //移动类型 
                    item.POSNR = p.BillLineErpKey;
                    item.WERKS = invOrg.ExternalId;//ERP库存组织Id      
                    item.LGORT = p.ToWarehouseCode;
                    item.MATNR = p.ItemCode;
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
