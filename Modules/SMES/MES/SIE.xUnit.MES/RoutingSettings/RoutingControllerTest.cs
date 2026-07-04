using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.RoutingSettings;
using SIE.xUnit.Core;
using SIE.xUnit.Resources.WipResources;
using SIE.xUnit.Techs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.MES.RoutingSettings
{
    public class RoutingControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 获取产品\产线工艺路线设置测试
        /// </summary>
        [Fact]
        public void GetRoutingSettingTest()
        {
            RoutingSettingController routingSettingController = RT.Service.Resolve<RoutingSettingController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var productRouting = RT.Service.Resolve<MesTestController>().CreateProductRouting();
            Assert.NotNull(productRouting);
            //获取同一产品，工单类型，工艺路线，制程工艺、有效期的产品工艺路线数量
            var count = routingSettingController.CountProductRouting(productRouting);
            Assert.Equal(0, count);
            var productRouting1 = new ProductRouting();
            productRouting1.EndDate = productRouting1.StartDate.AddDays(7);
            productRouting1.OrderType = productRouting.OrderType;
            productRouting1.RoutingId = productRouting.RoutingId;
            productRouting1.ProductId = productRouting.ProductId;
            productRouting1.ProcessSegmentId = productRouting.ProcessSegmentId;
            Assert.Throws<ValidationException>(() => RF.Save(productRouting1));
            Assert.Throws<ArgumentNullException>(() => routingSettingController.CountProductRouting(null));
            //获取同一产线，工单类型，工艺路线，有效期的产线工艺路线数量
            var wipResource = RT.Service.Resolve<WipResourceTestController>().GetFirstWipResource();
            Assert.NotNull(wipResource);
            var resourceRouting = RT.Service.Resolve<MesTestController>().GetOrCreateResourceRouting(productRouting.RoutingId.Value, wipResource.Id, DateTime.Today, DateTime.Today);
            var count2 = routingSettingController.CountResourceRouting(resourceRouting);
            Assert.Equal(0, count2);
            Assert.Throws<ArgumentNullException>(() => routingSettingController.CountResourceRouting(null));
            var newResourceRouting = new ResourceRouting();
            newResourceRouting.StartDate = resourceRouting.StartDate;
            newResourceRouting.EndDate = resourceRouting.EndDate;
            newResourceRouting.OrderType = SIE.Core.WorkOrders.WorkOrderType.Mass;
            newResourceRouting.RoutingId = resourceRouting.RoutingId;
            newResourceRouting.ResourceId = resourceRouting.ResourceId;
            Assert.Throws<ValidationException>(() => RF.Save(newResourceRouting));
        }

        /// <summary>
        /// 获取工艺路线版本测试
        /// </summary>
        [Fact]
        public void GetRoutingVersionsTest()
        {
            RoutingSettingController routingSettingController = RT.Service.Resolve<RoutingSettingController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var productRouting = RT.Service.Resolve<MesTestController>().CreateProductRouting();
            Assert.NotNull(productRouting);
            var wipResource = RT.Service.Resolve<WipResourceTestController>().GetFirstWipResource();
            Assert.NotNull(wipResource);
            var resourceRouting = RT.Service.Resolve<MesTestController>().GetOrCreateResourceRouting(productRouting.RoutingId.Value, wipResource.Id, DateTime.Today, DateTime.Today);
            var wType = SIE.Core.WorkOrders.WorkOrderType.Mass;
            //获取工艺路线版本 
            var routingVersions1 = routingSettingController.GetRoutingVersions(wType, DateTime.Today);
            Assert.Empty(routingVersions1);
            var routingVersions2 = routingSettingController.GetRoutingVersions(wType, DateTime.Today, productRouting.ProductId);
            Assert.Single(routingVersions2);
            var routingVersions3 = routingSettingController.GetRoutingVersions(wType, DateTime.Today, productRouting.ProductId, resourceRouting.ResourceId);
            Assert.NotEmpty(routingVersions3);
            var routingVersions4 = routingSettingController.GetRoutingVersions(wType, DateTime.Today, null, resourceRouting.ResourceId);
            Assert.NotEmpty(routingVersions4);
            Assert.Throws<EntityNotFoundException>(() => routingSettingController.GetRoutingVersions(wType, DateTime.Today, 0.5457474544));
            Assert.Throws<EntityNotFoundException>(() => routingSettingController.GetRoutingVersions(wType, DateTime.Today, productRouting.ProductId, 0.45744745));
            //获取工艺路线版本列表
            var routings = routingSettingController.GetRoutingVersionViewModels(productRouting.Id, new List<OrderInfo>(), new PagingInfo());
            Assert.Single(routings);
            Assert.Equal(YesNo.Yes, routings.FirstOrDefault().IsDefault);
            var routings1 = routingSettingController.GetRoutingVersionViewModels(-5457, new List<OrderInfo>(), new PagingInfo());
            Assert.Empty(routings1);
        }

        /// <summary>
        /// 工艺路线设置是否引用测试
        /// </summary>
        [Fact]
        public void RoutingHasUsedTest()
        {
            RoutingSettingController routingSettingController = RT.Service.Resolve<RoutingSettingController>();
            RT.Service.Resolve<ContextControllerTest>().InitContext();
            var wipResource = RT.Service.Resolve<WipResourceTestController>().GetFirstWipResource();
            Assert.NotNull(wipResource);
            var routing = RT.Service.Resolve<TechTestController>().CreateRouting();
            RT.Service.Resolve<MesTestController>().GetOrCreateResourceRouting(routing.Id, wipResource.Id, DateTime.Today, DateTime.Today);
            //根据引用判断是否可以删除企业模型和设备模型
            var isHasUsed1 = routingSettingController.IsHasUsedResourse(wipResource.SourceId.Value, wipResource.SourceType);
            Assert.False(isHasUsed1);
            var isHasUsed2 = routingSettingController.IsHasUsedResourse(-45684, wipResource.SourceType);
            Assert.True(isHasUsed2);
            //判断产线工艺路线设置是否引用指定的生产资源
            var isHasUsed3 = routingSettingController.ResourceRoutingHasUsedWipResource(wipResource.Id);
            Assert.True(isHasUsed3);
            var isHasUsed4 = routingSettingController.ResourceRoutingHasUsedWipResource(-54475475);
            Assert.False(isHasUsed4);
        }
    }
}
