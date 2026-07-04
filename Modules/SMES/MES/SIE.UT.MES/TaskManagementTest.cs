using SIE.Barcodes;
using SIE.Core.Barcodes;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WIP;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using System;
using Xunit;

namespace SIE.UT.MES
{
    public class TaskManagementTest : IClassFixture<AppInit>
    {
        [Fact]
        public void AutoReportTest()
        {
            ////var controller = RT.Service.Resolve<ReportController>();
            ////var dbTime = RF.Find<DispatchTask>().GetDbTime();
            ////string barcode = "SN2020022000000031";
            ////string stationCode = "HCQ包装工位";
            ////decimal okQty = 3;
            ////decimal ngQty = 0;
            ////RoutingProcessSign sign = RoutingProcessSign.End;
            ////WipCollectedEvent collectedEvent = InitCollectedEvent(controller, stationCode, dbTime, barcode, okQty, ngQty, sign);
            ////controller.AutoTaskReport(collectedEvent);

            ////var config = RT.Service.Resolve<ReportController>().GetReportRuleConfig(9);
            ////var model = config.ReportMode;
            ////if ((int)model == -1)
            ////{

            ////}
        }

        ////private WipCollectedEvent InitCollectedEvent(ReportController controller, string stationCode, DateTime dbTime, string barcode, decimal okQty, decimal ngQty, RoutingProcessSign sign)
        ////{
        ////    var station = RT.Service.Resolve<StationController>().GetStation(stationCode);
        ////    double workOrderId = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode).WorkOrderId.Value;
        ////    double resourceId = station.ResourceId;
        ////    double processId = station.ProcessId;
        ////    double stationId = station.Id;
        ////    double employeeId = RT.IdentityId;
        ////    controller.ValidateWipReport(workOrderId, employeeId, processId);
        ////    var product = new product()
        ////    {
        ////        WorkOrderId = workOrderId,
        ////        Qty = okQty,
        ////        NgQty = ngQty,
        ////        Routing = new routing(),
        ////    };
        ////    product.Routing.CurrentId = processId;
        ////    product.Routing.Processes.Add(new process() { Sign = sign, Id = processId, Type = station.Process.Type.Value });
        ////    var collectBarcode = new CollectBarcode(barcode, BarcodeType.SN);
        ////    var collectData = new CollectData();
        ////    var workcell = new Workcell()
        ////    {
        ////        EmployeeId = employeeId,
        ////        ResourceId = resourceId,
        ////        ProcessId = processId,
        ////        StationId = stationId
        ////    };
        ////    var data = new CollectEventData(product, new CollectBarcode[] { collectBarcode }, collectData, workcell, dbTime);
        ////    WipCollectedEvent collectedEvent = new WipCollectedEvent(data);
        ////    return collectedEvent;
        ////}
    }
}