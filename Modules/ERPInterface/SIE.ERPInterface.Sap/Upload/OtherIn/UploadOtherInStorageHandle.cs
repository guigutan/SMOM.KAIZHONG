using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OtherIn;
using SIE.ERPInterface.Sap.Upload.Allocate;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.OtherIn
{

    /// <summary>
    /// 其他入库上传业务数据接口实现
    /// </summary>
    public class UploadOtherInStorageHandle : IUploadDataHandler<SapOtherInStorageUploadData<SapOtherInStorageUploadDataDetail>>
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

        SapUploadParam<SapOtherInStorageUploadData<SapOtherInStorageUploadDataDetail>> IUploadDataHandler<SapOtherInStorageUploadData<SapOtherInStorageUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapOtherInStorageUploadData<SapOtherInStorageUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapOtherInStorageUploadData<SapOtherInStorageUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                //sapOrderParam.ZSRMID = f.FirstOrDefault().SrmNo;
                //sapOrderParam.BKTXT = f.FirstOrDefault().BillRemark;
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                f.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapOtherInStorageUploadDataDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item); 
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.WERKS = invOrg.ExternalId;
                    item.BWART = p.TransactionCode;
                    //其他出入库也会混合在一起 所以当目标仓库没有值的时候 取来源仓库
                    item.LGORT = p.ToWarehouseCode.IsNullOrEmpty() ? p.FromWarehouseCode: p.ToWarehouseCode;
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
