using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OtherOut;
using SIE.ERPInterface.Sap.Upload.Allocate;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.OtherOut
{

    /// <summary>
    /// 其他出库上传业务数据接口实现
    /// </summary>
    public class UploadOtherOutStorageHandle : IUploadDataHandler<SapOtherOutStorageUploadData<SapOtherOutStorageUploadDataDetail>>
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

        SapUploadParam<SapOtherOutStorageUploadData<SapOtherOutStorageUploadDataDetail>> IUploadDataHandler<SapOtherOutStorageUploadData<SapOtherOutStorageUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOtherOutStorageUploadData<SapOtherOutStorageUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapOtherOutStorageUploadData<SapOtherOutStorageUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                //sapOrderParam.ZSRMID = f.FirstOrDefault().SrmNo;
                //sapOrderParam.BKTXT = f.FirstOrDefault().BillRemark;
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                f.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapOtherOutStorageUploadDataDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    //var sapOrderParam = new SapInStorageUploadData<SapInStorageUploadDataDetail>();
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.WERKS = invOrg.ExternalId;
                    item.BWART = p.TransactionCode;
                    item.LGORT = p.FromWarehouseCode;
                    item.ERFME = p.UnitCode;
                    item.ERFMG = p.Quantity;
                    
                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
