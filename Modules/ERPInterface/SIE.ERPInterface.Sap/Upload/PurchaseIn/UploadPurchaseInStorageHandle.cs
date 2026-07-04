using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.PurchaseIn;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.PurchaseIn
{
    /// <summary>
    /// 采购入库上传业务数据接口实现
    /// </summary>
    public class UploadPurchaseInStorageHandle : IUploadDataHandler<SapInStorageUploadData<SapInStorageUploadDataDetail>>
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

        SapUploadParam<SapInStorageUploadData<SapInStorageUploadDataDetail>> IUploadDataHandler<SapInStorageUploadData<SapInStorageUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapInStorageUploadData<SapInStorageUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapInStorageUploadData<SapInStorageUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
              
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                uploadTransactions.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapInStorageUploadDataDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    item.BWART = "105"; //移动类型 
                  
                    item.EBELN = p.PoNo;
                    item.EBELP = p.PoLineNo;
                    item.WERKS = invOrg.Code.ToString();
                    item.LGORT = p.ToWarehouseCode;
                    item.MATNR = p.ItemCode;
                    item.ERFMG = p.Quantity;
                    item.ERFME = p.UnitCode;
                 
                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
