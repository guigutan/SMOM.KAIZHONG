using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SIE.UT.MES
{
    public class ErpWorkOrderApiTest : IClassFixture<AppInit>
    {
        ////static string[] _itemList = new string[6] { "ML20181109005", "ML20181217001", "ML20181217002", "ML20180914009", "ML20180914005", "ML20180914011" };

        [Fact]
        public void SaveWorkOrderTest()
        { 
            ////List<ErpWorkOrderData> erpInfoDatas = GetErpWorkOrderDatas(1);
            ////using (SIE.Diagnostics.DebugTrace.Start("ERP工单下载耗时："))
            ////{
            ////    try
            ////    {
            ////        var res = RT.Service.Resolve<WorkOrderController>().SaveWorkOrders(erpInfoDatas);
            ////        //RecoverData(erpInfoDatas);
            ////    }
            ////    catch (Exception exc)
            ////    {
            ////        Console.WriteLine(exc.Message);
            ////    }
            ////}
        }

        [Fact]
        public void SaveWorkOrderBomTest()
        {
            ////string wo = "W2020041000001";
            ////List<ErpWorkOrderBomData> erpInfoDatas = GetErpWorkOrderBomDatas(wo);
            ////try
            ////{
            ////    var res = RT.Service.Resolve<WorkOrderController>().SaveWorkOrderBoms(erpInfoDatas);
            ////}
            ////catch (Exception exc)
            ////{
            ////    Console.WriteLine(exc.Message);
            ////}
        }

        //private List<ErpWorkOrderBomData> GetErpWorkOrderBomDatas(string no)
        //{
        //    Random r = new Random();
        //    List<ErpWorkOrderBomData> res = new List<ErpWorkOrderBomData>();
        //    var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(no);
        //    if (wo != null)
        //    {
        //        var bomCount = r.Next(1, 5);
        //        for (int j = 1; j <= bomCount; j++)
        //        {
        //            var singleQty = j / 2 == 0 ? 1 : 2;
        //            res.Add(new ErpWorkOrderBomData()
        //            {
        //                WorkOrderNo = no,
        //                ItemCode = _itemList[j],
        //                RequireQty = singleQty * wo.PlanQty,
        //                SingleQty = singleQty
        //            });
        //        }
        //    }
        //    return res;
        //}

        //private void RecoverData(List<ErpWorkOrderData> erpInfoDatas)
        //{
        //    erpInfoDatas.ForEach(erpInfoData =>
        //    {
        //        DB.Delete<WorkOrder>().Where(p => p.No == erpInfoData.WorkOrderNo).Execute();
        //    });
        //}

        //private List<ErpWorkOrderData> GetErpWorkOrderDatas(int count)
        //{
        //    Random r = new Random();
        //    var today = DateTime.Today;
        //    var data = today.ToString("yyyyMMdd");
        //    List<ErpWorkOrderData> erpInfoDatas = new List<ErpWorkOrderData>();
        //    for (int i = 1; i <= count; i++)
        //    {
        //        var workOrderData = new ErpWorkOrderData()
        //        {
        //            WorkOrderNo = $"W{data}{i.ToString("00000")}",
        //            CustomerCode = "SIE",
        //            CustomerOrderNo = $"O{data}{i.ToString("00000")}",
        //            MakerCode = "HCQ",
        //            OrderQty = 1000,
        //            PlanQty = 1000,
        //            PlanBeginDate = today,
        //            PlanEndDate = today.AddDays(10),
        //            ProductCode = "ML20181109005",
        //            ResourceCode = "PCBA1线",
        //            WorkshopCode = "A车间",
        //            SaleOrderNo = $"S{data}{i.ToString("00000")}",
        //            WorkOrderType = 0
        //        };
        //        if (i / 2 == 0)
        //        {
        //            var bomCount = r.Next(1, 5);
        //            for (int j = 1; j <= bomCount; j++)
        //            {
        //                var singleQty = j / 2 == 0 ? 1 : 2;
        //                workOrderData.BomList.Add(new ErpWorkOrderBomData()
        //                {
        //                    ItemCode = _itemList[singleQty],
        //                    RequireQty = singleQty * workOrderData.PlanQty,
        //                    SingleQty = singleQty
        //                });
        //            }
        //        }
        //        erpInfoDatas.Add(workOrderData);
        //    }
        //    return erpInfoDatas;
        //}
    }
}