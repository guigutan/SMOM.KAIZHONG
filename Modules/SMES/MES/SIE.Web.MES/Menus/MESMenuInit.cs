using SIE.Common.Menus;
using SIE.MES.BarcodeProcesses;
using SIE.MES.BatchGeneration;
using SIE.MES.BatchWIP.Products;
using SIE.MES.InspectionStandards;
using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItems;
using SIE.MES.OnOffDuty;
using SIE.MES.Outsourcing;
using SIE.MES.PackingPrints;
using SIE.MES.PanelBindings;
using SIE.MES.PrepareProducts;
using SIE.MES.ProcessTransfers;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.Reports;
using SIE.MES.Projects;
using SIE.MES.QTimes;
using SIE.MES.QTimes.ViewModels;
using SIE.MES.RoutingSettings;
using SIE.MES.Validitys;
using SIE.MES.WIP.PackRecombine.Logs;
using SIE.MES.WIP.PackRecombine.Relations;
using SIE.MES.WIP.Products;
using SIE.MES.WoBarcodes;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class MESMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            res.Add(new MenuDto()
            {
                Label = "MES",
                Sort = 0,
                Icon = "mes icon-mes",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "工艺建模",
                IsLeafNode = false,
            });

            AddMesTechMenu(res);

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "生产管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "物料管理",
                IsLeafNode = false,
            });
            GetMenuDtosSonMethod(res);

            AddMesMaterialManage(res);

            return res;
        }

        /// <summary>
        /// 获取菜单Dto 子方法
        /// </summary>
        /// <param name="res"></param>
        private  void GetMenuDtosSonMethod(List<MenuDto> res)
        {
            const string mesProductionManage = "MES.生产管理";
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "工单",
                EntityType = typeof(WorkOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "工单制造档案",
                EntityType = typeof(WorkOrderArchive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "产品工艺路线",
                EntityType = typeof(WipProductRouting)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "批次产品工艺路线",
                EntityType = typeof(BatchWipProductRouting)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES.条码管理",
                Label = "条码领用",
                EntityType = typeof(WoBarcodeRange)
            });
            //res.Add(new MenuDto()
            //{
            //    TreeKey = "MES.条码管理",
            //    Label = "拼板码领用",
            //    EntityType = typeof(WoPanelRange)
            //});
            res.Add(new MenuDto()
            {
                TreeKey = "MES.条码管理",
                Label = "条码绑定记录",
                EntityType = typeof(PanelBindingRecord)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "包装管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.包装管理",
                Label = "包装号打印",
                EntityType = typeof(PackingWorkOrder)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.包装管理",
                Label = "包装操作日志",
                EntityType = typeof(RecombineLog)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产质量管理",
                Label = "检验项目",
                EntityType = typeof(ModelInspectionItem)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产报表",
                Label = "包装清单查询",
                EntityType = typeof(PackingRelationQuery)
            });
            //res.Add(new MenuDto()
            //{
            //    TreeKey = "MES.生产报表",
            //    Label = "工序交接记录",
            //    EntityType = typeof(ProcessTransferRecord)
            //});
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "工序委外需求单",
                EntityType = typeof(OutsourcingRequest)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "产前准备项目维护",
                EntityType = typeof(PrepareProject),
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "产品产前准备设置",
                EntityType = typeof(PrepareProduct),
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "产前准备记录",
                EntityType = typeof(PrepareRecord),
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "上料下料记录",
                EntityType = typeof(LoadItemsRecord),
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "在岗信息",
                EntityType = typeof(OnOffDutyRecrods)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "QTime标准维护",
                EntityType = typeof(QTimeStandard)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "QTime超时报表",
                EntityType = typeof(QTimeReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "有效期标准维护",
                EntityType = typeof(ValidityStandard)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "批次生成并过站",
                EntityType = typeof(WOBatchGeneration)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "条码工序指派",
                EntityType = typeof(BarcodeProcess)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "项目参数表",
                EntityType = typeof(ProjectParam)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "工序标准参数管理",
                EntityType = typeof(ProcessStandardParam)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "项目号需求设计",
                EntityType = typeof(ProjectDesign)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesProductionManage,
                Label = "项目号跟踪报表",
                EntityType = typeof(ProjectDesignReport)
            });
        }

        private static void AddMesMaterialManage(List<MenuDto> res)
        {
            const string mesMateralManage = "MES.物料管理";

            res.Add(new MenuDto()
            {
                TreeKey = mesMateralManage,
                Label = "工单耗用单".L10N(),
                EntityType = typeof(WoCostItem)
            });           
        }

        private static void AddMesTechMenu(List<MenuDto> res)
        {
            const string mesTech = "MES.工艺建模";
                       
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "产品工艺路线设置",
                EntityType = typeof(ProductRouting)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "产线工艺路线设置",
                EntityType = typeof(ResourceRouting)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "工序BOM管理",
                EntityType = typeof(SIE.MES.Routings.RoutingBoms.RoutingBom)
            });
            
        }
    }
}
