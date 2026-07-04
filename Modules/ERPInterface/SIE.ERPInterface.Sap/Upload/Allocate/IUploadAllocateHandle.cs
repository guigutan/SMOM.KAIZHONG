using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Allocate;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Sap.Upload.Allocate
{
    /// <summary>
    /// 库存调拨
    /// </summary>
    public class IUploadAllocateHandle : IUploadDataHandler<SapAllocateUploadData<UploadAllocateData>>
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

        SapUploadParam<SapAllocateUploadData<UploadAllocateData>> IUploadDataHandler<SapAllocateUploadData<UploadAllocateData>>.SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var rst = new SapUploadParam<SapAllocateUploadData<UploadAllocateData>>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions.GroupBy(f => f.OrdKey).ForEach(f =>
            {
                var sapOrderParam = new SapAllocateUploadData<UploadAllocateData>();
                sapOrderParam.EXTDOCNO = f.Key; //头部数据
                sapOrderParam.BILL_NO = f.FirstOrDefault().BillNo;
                sapOrderParam.BLDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().CreateDate);
                sapOrderParam.BUDAT = RT.Service.Resolve<SapUploadController>().ConvertDate(f.FirstOrDefault().TransactionDate);
                sapOrderParam.DBDH = f.FirstOrDefault().BillErpKey;
                f.OrderByDescending(p => p.TransactionDate).ForEach(p =>
                {
                    var item = new UploadAllocateData();
                    RT.Service.Resolve<SapUploadController>().SetSapItemParamBase(p, item);
                    item.BWART = p.TransactionCode; //移动类型-暂时取311
                    item.BILL_DTL_NO = p.BillLineNo;
                    item.WERKS = invOrg.ExternalId;//ERP库存组织Id
                    //如果是Z12
                    if(p.TransactionCode == "Z12")
                    {
                        item.LGORT = p.ToWarehouseCode;
                        item.UMLGO = p.FromWarehouseCode;
                    }
                    else
                    {
                        item.LGORT = p.FromWarehouseCode;
                        item.UMLGO = p.ToWarehouseCode;
                    }
              
                    item.UMMAT = p.ItemCode;
                    item.ERFMG = p.Quantity;
                    item.ERFME = p.UnitCode;
                    //item.CHARG = p.ProductLot;
                    //item.UMCHA = p.ProductLot;
                

                    sapOrderParam.ITEM.Add(item);
                });
                rst.ITEMS.Add(sapOrderParam);
            });
            return rst;
        }
    }
}
