using SIE.Common;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleReturn;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.TaskReport
{

    /// <summary>
    /// 报工业务数据接口实现
    /// </summary>
    public class UploadTaskReportHandle : IUploadDataHandler<KzTaskReportUploadData>
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

        SapUploadParam<KzTaskReportUploadData> IUploadDataHandler<KzTaskReportUploadData>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<KzTaskReportUploadData>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions
                .GroupBy(f => new { f.WoNo, f.ProcessCode })    //按工单工序合并上传
                .ForEach(f =>
                {
                    var list = f.AsEntityList();
                    var p = list.FirstOrDefault();
                    KzTaskReportUploadData sapOrderParam = new KzTaskReportUploadData();
                    sapOrderParam.AUFNR = p.WoNo;
                    sapOrderParam.VORNR = p.Vornr;
                    sapOrderParam.KTSCH = p.ProcessCode;
                    sapOrderParam.ARBPL = p.WorkCenter;
                    sapOrderParam.YIELD = list.Sum(x => x.OkQty ?? 0);
                    sapOrderParam.SCRAP = list.Sum(x => x.NgQty ?? 0);
                    sapOrderParam.ZTQTY = list.Sum(x => x.ReworkQty ?? 0);
                    sapOrderParam.WERKS = p.WERKS;
                    sapOrderParam.BUDAT = p.TransactionDate.ToString("yyyyMMdd");
                    sapOrderParam.ZUID = Guid.NewGuid().ToString();
                    rst.ITEMS.Add(sapOrderParam);

                    list.ForEach(x => { x.Zuid = sapOrderParam.ZUID; });
                    RF.Save(list);
                });
            return rst;
        }
    }
}
