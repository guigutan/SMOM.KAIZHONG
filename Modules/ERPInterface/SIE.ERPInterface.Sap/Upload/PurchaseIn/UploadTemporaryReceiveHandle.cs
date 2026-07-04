using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.PurchaseIn;
using SIE.Rbac.InvOrgs;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.PurchaseIn
{
    /// <summary>
    /// 采购送货单暂收上传业务数据接口实现
    /// </summary>
    public class UploadTemporaryReceiveHandle : IUploadDataHandler<SapTemporaryReceiveUploadData<SapTemporaryReceiveUploadDataDetail>>
    {
        public Dictionary<string, List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions)
        {
            throw new System.NotImplementedException();
        }

        public string SetParam(EntityList<UploadTransaction> uploadTransactions)
        {
            throw new System.NotImplementedException();
        }

        public ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string str)
        {
            throw new System.NotImplementedException();
        }

        SapUploadParam<SapTemporaryReceiveUploadData<SapTemporaryReceiveUploadDataDetail>> IUploadDataHandler<SapTemporaryReceiveUploadData<SapTemporaryReceiveUploadDataDetail>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapTemporaryReceiveUploadData<SapTemporaryReceiveUploadDataDetail>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapTemporaryReceiveUploadData<SapTemporaryReceiveUploadDataDetail>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据            
             
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                f.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new SapTemporaryReceiveUploadDataDetail();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    item.BWART = "103";
                    //移动类型 
                   
                    item.WERKS = invOrg.Code.ToString();
                    item.EBELN = p.PoNo;
                    item.EBELP = p.PoLineNo;
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
