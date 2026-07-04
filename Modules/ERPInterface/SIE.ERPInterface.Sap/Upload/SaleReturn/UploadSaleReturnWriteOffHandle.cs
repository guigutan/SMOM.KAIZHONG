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
    /// 销售退货上传（冲销）业务数据接口实现
    /// </summary>
    public class UploadSaleReturnWriteOffHandle : IUploadDataHandler<SapSaleReturnWriteOffUploadData<SapSaleReturnWriteOffUploadDataDetail>>
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

        SapUploadParam<SapSaleReturnWriteOffUploadData<SapSaleReturnWriteOffUploadDataDetail>> IUploadDataHandler<SapSaleReturnWriteOffUploadData<SapSaleReturnWriteOffUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapSaleReturnWriteOffUploadData<SapSaleReturnWriteOffUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.Where(f => !f.BillErpKey.IsNullOrEmpty()).GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapSaleReturnWriteOffUploadData<SapSaleReturnWriteOffUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                sapOrderParam.VBELN = f.FirstOrDefault().BillErpKey;
               
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
