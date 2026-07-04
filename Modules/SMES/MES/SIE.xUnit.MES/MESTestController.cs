using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.RoutingSettings;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using SIE.xUnit.Core;
using SIE.xUnit.Items;
using SIE.xUnit.Techs;
using System;
using System.Linq;

namespace SIE.xUnit.MES
{
    public partial class MesTestController : DomainController
    {
        #region 工单       
        /// <summary>
        /// 创建工单
        /// </summary>
        /// <param name="wipResource">生产资源</param>
        /// <param name="productId">产品Id</param>
        /// <param name="routingId">工艺路线Id</param>
        /// <param name="isCommonMode">是否共模</param>
        /// <param name="isMainMaterial">是否主料</param>
        /// <param name="processTechId">制程工艺Id</param>
        /// <param name="planNo">计划单号</param>
        /// <param name="productionOrderCode">生产订单号</param>
        /// <param name="isVirtualPart">工单产品Bom对应产品是否虚拟件</param>
        /// <param name="virtualPartCount">工单产品Bom对应产品是否虚拟件</param>
        /// <returns>工单</returns>
        public virtual WorkOrder CreateWorkOrder(WipResource wipResource, double productId, double routingId, bool isCommonMode, bool isMainMaterial, double proportion, double? processTechId, string planNo, string productionOrderCode, bool isVirtualPart, int virtualPartCount)
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
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

                var no = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
                var workOrder = new WorkOrder();
                workOrder.GenerateId();
                workOrder.No = no;
                workOrder.PlanBeginDate = DateTime.Now;
                workOrder.PlanEndDate = DateTime.Now;
                workOrder.OrderQty = 300;
                workOrder.PlanQty = 300;
                workOrder.State = SIE.Core.WorkOrders.WorkOrderState.Release;
                workOrder.WorkShopId = wipResource.WorkShopId;
                workOrder.ResourceId = wipResource.Id;
                //创建物料和工艺路线
                var productRouting = CreateProductRouting(productId, routingId);
                workOrder.ProductId = productRouting.ProductId.Value;
                workOrder.RoutingId = productRouting.RoutingId.Value;
                workOrder.VersionId = productRouting.Routing.DefaultVersionId.Value;                
                if (isCommonMode)
                {
                    workOrder.IsCommonMode = isCommonMode;
                    workOrder.IsMainMaterial = isMainMaterial;
                    workOrder.Proportion = proportion;
                    workOrder.ProcessTechId = processTechId;
                    workOrder.PlanNo = planNo;
                    workOrder.ProductionOrderCode = productionOrderCode;
                }

                if (isVirtualPart)
                {
                    for (int i = 0; i < virtualPartCount; i++)
                    {
                        var family = RT.Service.Resolve<ItemTestController>().CreateProductFamily(1).FirstOrDefault();
                        var product = RT.Service.Resolve<ItemTestController>().CreateTaskProduct(isVirtualPart, family.Id);
                        var bom = new WorkOrderBom()
                        {
                            ItemId = product.Id,
                            IsVritualItem = isVirtualPart,
                            SingleQty = 1,
                            RequireQty = 300,
                        };
                        workOrder.BomList.Add(bom);
                    }
                }

                var numberRule = controller.CreateNumberRule("SN条码打印_0820");
                var printTemplate = new PrintTemplate();
                printTemplate.GenerateId();
                printTemplate.EntityName = "条码";
                printTemplate.EntityType = "SIE.Barcodes.Printables.BarcodePrintable,SIE.Barcodes";
                printTemplate.FileName = "{0}条码.siedev".L10nFormat(workOrder.No);
                printTemplate.PrintType = PrintType.Label;
                printTemplate.State = State.Enable;
                printTemplate.Type = ".siedev";
                RF.Save(printTemplate);
                var labelTemplate = new SIE.Core.Items.LabelPrintTemplate();
                labelTemplate.GenerateId();
                labelTemplate.NumberRuleId = numberRule.Id;
                RF.Save(labelTemplate);
                workOrder.TemplateId = labelTemplate.Id;
                RF.Save(workOrder);

                //打印工单条码
                var info = new PrinterInfo(workOrder.Id, numberRule.Id, printTemplate.Id, (int)workOrder.PlanQty, 1, workOrder.PrintedQty);
                var result = RT.Service.Resolve<BarcodeController>().PrintBarcodes(info);
                if (result.Item1.Length > 0)
                    throw new ValidationException(result.Item1);
                tran.Complete();
                return workOrder;
            }
        }
        #endregion

        #region 产品工艺路线设置
        /// <summary>
        /// 创建一个产品工艺路线设置
        /// </summary>
        /// <returns>产品工艺路线设置</returns>
        public virtual ProductRouting CreateProductRouting()
        {
            var productRouting = new ProductRouting();
            productRouting.EndDate = productRouting.StartDate.AddDays(30);
            productRouting.OrderType = SIE.Core.WorkOrders.WorkOrderType.Mass;
            productRouting.Routing = RT.Service.Resolve<TechTestController>().CreateRouting();
            productRouting.Product = RT.Service.Resolve<ItemTestController>().CreateItem();
            RF.Save(productRouting);
            return productRouting;
        }

        /// <summary>
        /// 创建产品工艺路线设置
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="routingId">工艺路线Id</param>
        /// <returns>产品工艺路线设置</returns>
        public virtual ProductRouting CreateProductRouting(double productId, double routingId)
        {
            var productRouting = new ProductRouting();
            productRouting.EndDate = productRouting.StartDate.AddDays(30);
            productRouting.OrderType = SIE.Core.WorkOrders.WorkOrderType.Mass;
            productRouting.RoutingId = routingId;
            productRouting.ProductId = productId;
            RF.Save(productRouting);
            return productRouting;
        }

        public virtual ProductRouting CreateProductRouting(Item product, Routing routing, SIE.Core.WorkOrders.WorkOrderType type)
        {
            var today = DateTime.Today;
            var productRouting = RT.Service.Resolve<CommonController>().GetData<ProductRouting>(p => p.OrderType == type && p.ProductId == product.Id, null);
            if (productRouting == null)
            {
                productRouting = new ProductRouting()
                {
                    StartDate = today,
                    EndDate = today.AddDays(1),
                    OrderType = type,
                    Routing = routing,
                    Product = product
                };
                productRouting.GenerateId();
            }
            else
            {
                productRouting.StartDate = today;
                productRouting.EndDate = today.AddDays(1);
                productRouting.Routing = routing;
            }
            RF.Save(productRouting);
            return productRouting;
        }
        #endregion

        #region 产线工艺路线设置
        /// <summary>
        /// 创建一个产线工艺路线设置
        /// </summary>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="routingId">工艺路线ID</param>
        /// <param name="resourceId">生产资源id</param>
        /// <returns>产线工艺路线设置</returns>
        public virtual ResourceRouting GetOrCreateResourceRouting(double routingId, double resourceId, DateTime startDate, DateTime endDate)
        {
            var resourceRouting = Query<ResourceRouting>().Where(p => p.ResourceId == resourceId && p.OrderType == SIE.Core.WorkOrders.WorkOrderType.Mass
            && p.StartDate <= endDate && p.EndDate >= startDate).FirstOrDefault();
            if (resourceRouting != null)
                return resourceRouting;
            var newResourceRouting = new ResourceRouting();
            newResourceRouting.StartDate = startDate;
            newResourceRouting.EndDate = endDate;
            newResourceRouting.OrderType = SIE.Core.WorkOrders.WorkOrderType.Mass;
            newResourceRouting.RoutingId = routingId;
            newResourceRouting.ResourceId = resourceId;
            RF.Save(newResourceRouting);
            return newResourceRouting;
        }
        #endregion
    }
}
