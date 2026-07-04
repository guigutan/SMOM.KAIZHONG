using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.xUnit.Core;
using SIE.xUnit.Resources.WipResources;
using SIE.xUnit.Techs;
using System;

namespace SIE.xUnit.MES
{
    public partial class MesTestController : DomainController
    {
        /// <summary>
        /// 创建带工单的条码列表
        /// </summary>
        /// <returns>条码列表</returns>
        public virtual EntityList<Barcode> CreateWorkOrderBarcode()
        {
            var barcodes = new EntityList<Barcode>();
            var workOrder = RT.Service.Resolve<MesTestController>().CreateWorkOrder();
            var numberRule = RT.Service.Resolve<ContextControllerTest>().CreateNumberRule("SN条码打印_0820");
            var printTemplate = new PrintTemplate();
            printTemplate.FileName = workOrder.No + "模板";
            printTemplate.Type = ".siedev";
            RF.Save(printTemplate);
            //打印工单条码
            var info = new PrinterInfo(workOrder.Id, numberRule.Id, printTemplate.Id, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
            var result = RT.Service.Resolve<BarcodeController>().PrintBarcodes(info);
            if (result.Item1.Length > 0)
                throw new ValidationException(result.Item1);
            else
                barcodes.AddRange(result.Item2);
            foreach (var barcode in barcodes)
            {
                barcode.PersistenceStatus = PersistenceStatus.Unchanged;
            }
            return barcodes;
        }

        /// <summary>
        /// 创建一个工单（用于测试条码打印）
        /// </summary>
        /// <returns>工单</returns>
        public virtual WorkOrder CreateWorkOrder()
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                var workOrderNo = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                var workOrder = new WorkOrder();
                workOrder.GenerateId();
                workOrder.No = "MO" + workOrderNo + workOrder.Id;
                workOrder.PlanBeginDate = DateTime.Now;
                workOrder.PlanEndDate = DateTime.Now;
                workOrder.OrderQty = 300;
                workOrder.PlanQty = 300;
                workOrder.State = SIE.Core.WorkOrders.WorkOrderState.Release;
                //查找车间和产线（没有时新建）
                var wipResource = RT.Service.Resolve<WipResourceTestController>().GetFirstWipResource();
                workOrder.WorkShopId = wipResource.WorkShopId;
                workOrder.ResourceId = wipResource.Id;
                //创建物料和工艺路线
                var productRouting = CreateProductRouting();
                workOrder.ProductId = productRouting.ProductId.Value;
                workOrder.RoutingId = productRouting.RoutingId.Value;
                workOrder.VersionId = productRouting.Routing.DefaultVersionId.Value;
                RF.Save(workOrder);
                tran.Complete();
                return workOrder;
            }
        }

        public virtual void ConfigWorkOrderNoConfig()
        {
            var controller = RT.Service.Resolve<ContextControllerTest>();
            var config = ConfigService.GetConfig(new WorkOrderNoConfig(), typeof(WorkOrder));
            if (config == null || config.BacodeRule == null)
            {
                var configValue = new WorkOrderNoConfigValue();
                configValue.BacodeRule = controller.CreateNumberRule("工单号生成规则", "WO");

                string value = DomainJsonConvert.SerializeObject(configValue, ConfigValueSerializerSettings.Settings);
                controller.CreateConfig(typeof(WorkOrder).GetQualifiedName(), typeof(WorkOrderNoConfig).GetQualifiedName(), value);
            }
        }
    }
}