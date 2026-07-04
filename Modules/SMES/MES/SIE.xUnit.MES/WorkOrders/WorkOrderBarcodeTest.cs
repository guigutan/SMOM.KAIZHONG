using SIE.Barcodes;
using SIE.Barcodes.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.xUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.MES.WorkOrders
{
    public class WorkOrderBarcodeTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 打印条码测试
        /// </summary>
        [Fact]
        public void PrintBarcodesTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = new EntityList<Barcode>();
            var workOrder = RT.Service.Resolve<MesTestController>().CreateWorkOrder();
            var numberRule = RT.Service.Resolve<ContextControllerTest>().CreateNumberRule("SN条码打印_0820");
            var printTemplate = new PrintTemplate();
            printTemplate.FileName = workOrder.No + "模板";
            printTemplate.Type = ".siedev";
            RF.Save(printTemplate);
            //打印工单条码
            var info = new PrinterInfo(workOrder.Id, numberRule.Id, printTemplate.Id, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
            var result = barcodeController.PrintBarcodes(info);
            Assert.Equal(0, result.Item1.Length);
            barcodes.AddRange(result.Item2);
            workOrder = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.NotEmpty(barcodes);
            Assert.Equal(workOrder.PrintedQty, barcodes.Count);

            //错误数据打印
            var info1 = new PrinterInfo(-4854745, 718.475545, printTemplate.Id, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
            Assert.Throws<EntityNotFoundException>(() => barcodeController.Print(info1));
            var info2 = new PrinterInfo(workOrder.Id, numberRule.Id, printTemplate.Id, (int)workOrder.PlanQty + 1000, 1, workOrder.PrintedQty);
            Assert.Throws<ValidationException>(() => barcodeController.Print(info2));
            var info3 = new PrinterInfo(workOrder.Id, numberRule.Id, printTemplate.Id, (int)workOrder.PlanQty, -1, workOrder.PrintedQty);
            Assert.Throws<ValidationException>(() => barcodeController.Print(info3));
            var result1 = barcodeController.PrintBarcodes(info1);
            Assert.NotEqual(0, result1.Item1.Length);
            var info4 = new PrinterInfo(workOrder.Id, -852, printTemplate.Id, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
            Assert.Throws<EntityNotFoundException>(() => barcodeController.Print(info4));
            var info5 = new PrinterInfo(workOrder.Id, numberRule.Id, -2551, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
            Assert.Throws<EntityNotFoundException>(() => barcodeController.Print(info5));
        }

        /// <summary>
        /// 补打条码测试
        /// </summary>
        [Fact]
        public void ReprintBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            var printTimes = barcodes.FirstOrDefault().PrintTimes;
            //条码补打
            var errMsg1 = barcodeController.ReprintBarcode(barcodes, BarcodeLogType.Remedy, "测试补打", 1);
            Assert.Equal(string.Empty, errMsg1);
            var errMsg2 = barcodeController.ReprintBarcode(barcodes, BarcodeLogType.Remedy, "", 1);
            Assert.Equal("补打原因不允许为空.", errMsg2);
            Assert.Throws<ArgumentNullException>(() => barcodeController.Reprint(new EntityList<Barcode> { }, BarcodeLogType.Remedy, "测试补打", 1));
            Assert.Throws<ValidationException>(() => barcodeController.Reprint(barcodes, BarcodeLogType.Remedy, "测试补打", -1));
            var barcode = barcodeController.GetBarcode(barcodes.FirstOrDefault().Sn);
            Assert.Equal(printTimes + 1, barcode.PrintTimes);
            //查询条码打印日志
            var barcodeLogCriteria = new BarcodeLogCriteria();
            var barcodeLog1 = barcodeController.GetBarcodeLogs(barcodeLogCriteria);
            Assert.NotEmpty(barcodeLog1);
            barcodeLogCriteria.Barcode = barcodes.FirstOrDefault().Sn;
            barcodeLogCriteria.Type = BarcodeLogType.Remedy;
            var barcodeLog2 = barcodeController.GetBarcodeLogs(barcodeLogCriteria);
            Assert.NotEmpty(barcodeLog2);
        }

        /// <summary>
        /// 工单条码测试
        /// </summary>
        [Fact]
        public void WorkOrderBarcodesTest1()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            var workOrder = RF.GetById<WorkOrder>(barcodes.FirstOrDefault().WorkOrderId);
            //获取生产工单Id所对应的所有条码信息列表
            var barcodes1 = barcodeController.GetBarcodes(new List<double> { });
            Assert.Empty(barcodes1);
            var barcodes2 = barcodeController.GetBarcodes(new List<double> { -36856, 75.578558454 });
            Assert.Empty(barcodes2);
            var barcodes3 = barcodeController.GetBarcodes(new List<double> { workOrder.Id });
            Assert.Equal(workOrder.PrintedQty, barcodes3.Count);
            //根据工单ID获取条码
            var barcodes4 = barcodeController.GetBarcodes(workOrder.Id, null, null);
            Assert.Equal(workOrder.PrintedQty, barcodes4.Count);
            var barcodes5 = barcodeController.GetBarcodes(workOrder.Id, null, new PagingInfo(1, 10));
            Assert.InRange(barcodes5.Count, 0, 10);
            //条码是否存在
            var barcodeExist1 = barcodeController.Exists(barcodes.FirstOrDefault()?.Sn, workOrder.Type);
            Assert.True(barcodeExist1);
            var barcodeExist2 = barcodeController.Exists(barcodes.FirstOrDefault()?.Sn, SIE.Core.WorkOrders.WorkOrderType.Rework);
            Assert.False(barcodeExist2);
            //检验是否存在条码和工单
            var barcodeExist3 = barcodeController.Exists(barcodes.FirstOrDefault()?.Sn, workOrder.Id);
            Assert.True(barcodeExist3);
            var barcodeExist4 = barcodeController.Exists(barcodes.FirstOrDefault()?.Sn, 0d);
            Assert.False(barcodeExist4);
        }

        /// <summary>
        /// 工单条码测试
        /// </summary>
        [Fact]
        public void WorkOrderBarcodesTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            var workOrder = RF.GetById<WorkOrder>(barcodes.FirstOrDefault().WorkOrderId);
            //获取工单条码数量
            var qty1 = barcodeController.GetBarcodeCount(workOrder.Id);
            Assert.Equal(workOrder.PrintedQty, qty1);
            var qty2 = barcodeController.GetBarcodeCount(-854);
            Assert.Equal(0, qty2);
            //计算工单已生成条码数
            var qty3 = barcodeController.CountGenerateBarcode(workOrder.Id);
            Assert.Equal(workOrder.PrintedQty, qty3);
            var qty4 = barcodeController.CountGenerateBarcode(484.44456846565454);
            Assert.Equal(0, qty4);
            //根据条码打印条件获取工单
            var itemBatchRule = new SIE.Core.Items.ItemBatchRule();
            itemBatchRule.Qty = 1;
            itemBatchRule.ItemId = workOrder.ProductId;
            itemBatchRule.RetrospectType = SIE.Core.Items.RetrospectType.Single;
            RF.Save(itemBatchRule);
            var printWorkOrderCriteria = new PrintWorkOrderCriteria();
            var workOrders1 = barcodeController.GetWorkOrders(printWorkOrderCriteria);
            Assert.NotEmpty(workOrders1);
            printWorkOrderCriteria.No = workOrder.No;
            var workOrders2 = barcodeController.GetWorkOrders(printWorkOrderCriteria);
            Assert.All(workOrders2, p => Assert.Equal(workOrder.No, p.No));
            //通过工单Id获取条码打印
            var workOrders3 = barcodeController.GetPrintWorkOrder(workOrder.Id);
            Assert.NotNull(workOrders3);
            var workOrders4 = barcodeController.GetPrintWorkOrder(-5645454);
            Assert.Null(workOrders4);
            //查询条码范围
            var barcodeRange1 = barcodeController.GetBarcodeRanges(workOrder.Id);
            Assert.NotEmpty(barcodeRange1);
            var barcodeRange2 = barcodeController.GetBarcodeRanges(-123456784);
            Assert.Empty(barcodeRange2);
        }

        /// <summary>
        /// 报废条码测试
        /// </summary>
        [Fact]
        public void ScrapBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            var workOrder = RF.GetById<WorkOrder>(barcodes.FirstOrDefault().WorkOrderId);
            var barcodes1 = barcodeController.GetBarcodes(workOrder.Id, null, new PagingInfo(1, 10));
            var barcodesPaging5 = barcodeController.GetBarcodes(workOrder.Id, null, new PagingInfo(5, 10));
            var barcodes1Sns = barcodes1.Select(p => p.Sn).ToList<string>();
            //条码报废
            barcodeController.BarcodeScrap(barcodes1, "报废条码测试");
            var barcodes2 = barcodeController.GetBarcodesBySns(barcodes1Sns);
            Assert.All(barcodes2, p => Assert.True(p.IsScraped));
            Assert.Throws<ValidationException>(() => barcodeController.BarcodeScrap(barcodes1, ""));
            Assert.Throws<ValidationException>(() => barcodeController.BarcodeScrap(new EntityList<Barcode>(), "报废条码测试"));
            Assert.Throws<ValidationException>(() => barcodeController.BarcodeScrap(barcodes1, "报废条码测试"));
            barcodesPaging5.ForEach(p => p.WorkOrderId = -9);
            Assert.Throws<EntityNotFoundException>(() => barcodeController.BarcodeScrap(barcodesPaging5, "报废条码测试"));
            //根据【是否报废】获取工单条码列表
            var barcodes3 = barcodeController.GetBarcodes(workOrder.Id, false);
            Assert.Equal(barcodes.Count - 10, barcodes3.Count);
            var barcodes4 = barcodeController.GetBarcodes(workOrder.Id, true);
            Assert.Equal(10, barcodes4.Count);
            //条码报废
            var barcodes5 = barcodes3.Take(20);
            var barcodes5Sns = barcodes5.Select(p => p.Sn).ToList<string>();
            barcodeController.BarcodeScrap(barcodes5Sns, "报废条码测试");
            var barcodes6 = barcodeController.GetBarcodesBySns(barcodes5Sns);
            Assert.All(barcodes6, p => Assert.True(p.IsScraped));
            //条码是否已经报废
            var isScrap1 = barcodeController.Exists(barcodes5Sns.FirstOrDefault(), true);
            Assert.True(isScrap1);
            var isScrap2 = barcodeController.Exists(barcodes5Sns.LastOrDefault(), false);
            Assert.False(isScrap2);
            //获取工单报废条码数量
            var scrapQty = barcodeController.GetScrapBarcodeCount(workOrder.Id);
            Assert.Equal(30, scrapQty);
            var scrapQty1 = barcodeController.GetScrapBarcodeCount(0);
            Assert.Equal(0, scrapQty1);
        }

        /// <summary>
        /// 领用条码测试
        /// </summary>
        [Fact]
        public void ReceiveBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            var range = barcodes.FirstOrDefault().Range;
            //条码领用
            Assert.Throws<ValidationException>(() => barcodeController.BarcodeReceive(range.Id, "", ""));
            Assert.Throws<EntityNotFoundException>(() => barcodeController.BarcodeReceive(-4574587, "SysAdmin", "666666"));
            barcodeController.BarcodeReceive(range.Id, "SysAdmin", "666666");
            var range1 = RF.GetById<BarcodeRange>(range.Id);
            Assert.Equal(ReceiveState.Received, range1.State);
            Assert.Equal(RT.IdentityId, range1.ReceiveById);
            Assert.Throws<ValidationException>(() => barcodeController.BarcodeReceive(range.Id, "SysAdmin", "666666"));
            //查询条码领用
            var barcodeRangeCriteria = new BarcodeRangeCriteria();
            var barcodeRanges = barcodeController.GetBarcodeRanges(barcodeRangeCriteria);
            Assert.NotEmpty(barcodeRanges);
            barcodeRangeCriteria.ReceiveById = RT.IdentityId;
            var barcodeRanges1 = barcodeController.GetBarcodeRanges(barcodeRangeCriteria);
            Assert.NotEmpty(barcodeRanges1);
        }

        /// <summary>
        /// 挂起\恢复 条码测试
        /// </summary>
        [Fact]
        public void PendingBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var barcodes = RT.Service.Resolve<MesTestController>().CreateWorkOrderBarcode();
            //条码挂起
            barcodeController.BarcodePending(barcodes, "测试条码挂起");
            var barcodes2 = barcodeController.GetBarcodesBySns(barcodes.Select(p => p.Sn).ToList<string>());
            Assert.All(barcodes2, p => Assert.True(p.IsPending));
            Assert.Throws<ValidationException>(() => barcodeController.BarcodePending(barcodes, ""));
            Assert.Throws<ValidationException>(() => barcodeController.BarcodePending(new EntityList<Barcode>(), "测试条码挂起"));
            //条码恢复
            barcodeController.BarcodeResume(barcodes2.Select(p => p.Id).ToArray());
            var barcodes3 = barcodeController.GetBarcodesBySns(barcodes.Select(p => p.Sn).ToList<string>());
            Assert.All(barcodes3, p => Assert.False(p.IsPending));
        }

        /// <summary>
        /// 导入条码测试
        /// </summary>
        [Fact]
        public void ImportBarcodeTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            //条码导入
            Assert.Throws<ValidationException>(() => barcodeController.ImportBarcode(0, new List<string>()));
            Assert.Throws<ValidationException>(() => barcodeController.ImportBarcode(0, new List<string>() { "测试" }));
            Assert.Throws<EntityNotFoundException>(() => barcodeController.ImportBarcode(0, new List<string>() { "SNSNSNSN848454547" }));
            var workOrder = RT.Service.Resolve<MesTestController>().CreateWorkOrder();
            var sns = new List<string> { };
            var barcodeSn = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            for (var i = 0; i < 10; i++)
            {
                sns.Add("ISN" + barcodeSn + i);
            }
            barcodeController.ImportBarcode(workOrder.Id, sns);
            var workOrder1 = RF.GetById<WorkOrder>(workOrder.Id);
            Assert.Equal(10, workOrder1.PrintedQty);
            var barcodes = barcodeController.GetBarcodesBySns(sns);
            Assert.Equal(10, barcodes.Count);
            Assert.Equal(workOrder.Id, barcodes.FirstOrDefault().WorkOrderId);
            Assert.Throws<ValidationException>(() => barcodeController.ImportBarcode(workOrder.Id, sns));

            //根据生产订单Id列表获取条码范围列表
            var barcodeRange1 = barcodeController.GetBarcodeRanges(new List<double> { workOrder.Id });
            Assert.Single(barcodeRange1);
            var barcodeRange2 = barcodeController.GetBarcodeRanges(new List<double> { });
            Assert.Empty(barcodeRange2);
        }

        /// <summary>
        /// 获取打印模板测试
        /// </summary>
        [Fact]
        public void GetPrintTemplateTest()
        {
            BarcodeController barcodeController = RT.Service.Resolve<BarcodeController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var workOrder = RT.Service.Resolve<MesTestController>().CreateWorkOrder();
            //获取工单的标签打印模板
            var printTemplate1 = barcodeController.GetPrintTemplateByWo(workOrder.Id);
            Assert.Null(printTemplate1);
            var printTemplate = new PrintTemplate();
            printTemplate.FileName = workOrder.No + "模板";
            printTemplate.Type = ".siedev";
            printTemplate.State = State.Enable;
            printTemplate.EntityType = typeof(BarcodePrintable).GetQualifiedName();
            RF.Save(printTemplate);
            var labelPrintTemplate = new LabelPrintTemplate();
            labelPrintTemplate.LabelTemplateId = printTemplate.Id;
            RF.Save(labelPrintTemplate);
            workOrder.TemplateId = labelPrintTemplate.Id;
            RF.Save(workOrder);
            var printTemplate2 = barcodeController.GetPrintTemplateByWo(workOrder.Id);
            Assert.NotNull(printTemplate2);
            //根据规则Id查询打印模板
            var printTemplates = barcodeController.GetPrintTemplatesByRuleId(0, "", null, "测试");
            Assert.Empty(printTemplates);
            var numberRule = RT.Service.Resolve<ContextControllerTest>().CreateNumberRule("SN条码打印_0820");
            var numberRuleInTemplate = new NumberRuleInTemplate();
            numberRuleInTemplate.RuleId = numberRule.Id;
            numberRuleInTemplate.TemplateId = printTemplate.Id;
            RF.Save(numberRuleInTemplate);
            var printTemplates1 = barcodeController.GetPrintTemplatesByRuleId(numberRule.Id, typeof(BarcodePrintable).GetQualifiedName(), null, "");
            Assert.NotEmpty(printTemplates1);
            var printTemplates2 = barcodeController.GetPrintTemplatesByRuleId(numberRule.Id, typeof(BarcodePrintable).GetQualifiedName(), null, printTemplate.FileName);
            Assert.Single(printTemplates2);
        }
    }
}
