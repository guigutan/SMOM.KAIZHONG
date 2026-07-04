using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Suppliers;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.SupplierReturn
{
    /// <summary>
    /// 供应商退货业务上传
    /// </summary>
    internal class IUploadSupplierReturnHandle : IUploadDataHandler<SapOrderParamSupplierReturn<SupplierReturnUploadData>>
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
        SapUploadParam<SapOrderParamSupplierReturn<SupplierReturnUploadData>> IUploadDataHandler<SapOrderParamSupplierReturn<SupplierReturnUploadData>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOrderParamSupplierReturn<SupplierReturnUploadData>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.Where(p => !p.BillErpKey.IsNullOrEmpty() && !p.BillLineErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapOrderParamSupplierReturn<SupplierReturnUploadData>();
                var order = f.OrderByDescending(a => a.TransactionDate).FirstOrDefault();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.TransactionDate);
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(order.CreateDate);
                f.Where(p=> !p.BillErpKey.IsNullOrEmpty() && !p.BillLineErpKey.IsNullOrEmpty()).OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SupplierReturnUploadData();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    if (p.TransactionCode == "161")
                    {
                        item.BWART = "101"; //供应商退货 
                    }
                    else
                    {
                        item.BWART = "104"; //暂收不合格退货 
                    }
                    item.WERKS = invOrg.ExternalId;//ERP库存组织Id
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.EBELN = p.BillErpKey;
                    item.EBELP = p.BillLineErpKey;
                    item.LGORT = p.FromWarehouseCode;
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
