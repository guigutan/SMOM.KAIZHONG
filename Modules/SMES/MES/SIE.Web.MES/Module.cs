using Amazon.Util.Internal;
using SIE.MES.Andon;
using SIE.MES.BarcodeProcesses;
using SIE.MES.BatchGeneration;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport;
using SIE.MES.BlueLable;
using SIE.MES.Capacitys;
using SIE.MES.Checker;
using SIE.MES.DataBarcode;
using SIE.MES.DesignerAreas;
using SIE.MES.DispoLookups;
using SIE.MES.Edge.Models;
using SIE.MES.EmpWork;
using SIE.MES.Engrave;
using SIE.MES.FeedingIntervals;
using SIE.MES.Fixture;
using SIE.MES.InspectionStandards;
using SIE.MES.ItemChecker;
using SIE.MES.ItemCPN;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.MES.ItemLine;
using SIE.MES.ItemProcess;
using SIE.MES.LineAndon;
using SIE.MES.ListAtts;
using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItems;
using SIE.MES.MtartProcessLookups;
using SIE.MES.OnOffDuty;
using SIE.MES.OnOffDutyA;
using SIE.MES.OnOffDutyB;
using SIE.MES.OrgLevels;
using SIE.MES.Outsourcing;
using SIE.MES.PackingPrints;
using SIE.MES.PackingQC;
using SIE.MES.PackRule;
using SIE.MES.PanelBindings;
using SIE.MES.PrepareProducts;
using SIE.MES.ProcessPrepareRecords;
using SIE.MES.ProcessProperty;
using SIE.MES.ProductAgingProcesss;
using SIE.MES.ProjectDesigns;
using SIE.MES.Projects;
using SIE.MES.QTimes;
using SIE.MES.QTimes.ViewModels;
using SIE.MES.ReworkLayoutVersions;
using SIE.MES.RoutingSettings;
using SIE.MES.ScheProdVariance;
using SIE.MES.SpcFromMess;
using SIE.MES.Validitys;
using SIE.MES.WIP.PackRecombine.Logs;
using SIE.MES.WIP.PackRecombine.Relations;
using SIE.MES.WIP.Pressure;
using SIE.MES.WIP.Products;
using SIE.MES.WoBarcodes;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders._Routing_;
using SIE.MES.WorkReportPlans;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.MES;
using SIE.Web.MES.BarcodeProcesses;
using SIE.Web.MES.BatchProductRoutings;
using SIE.Web.MES.BlueLabel.Commands;
using SIE.Web.MES.PackingPrints;
using SIE.Web.MES.PackingQC;
using SIE.Web.MES.PrepareProducts;
using SIE.Web.MES.ProductRoutings;
using SIE.Web.MES.RoutingSettings;
using SIE.Web.MES.SpcFromMess;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.MES
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            //WebResourceConfig.AddFilterModule(GetType());
            AddPropertyEditor(app);
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    Label = "批次生成并过站".L10N(),
                    EntityType = typeof(WOBatchGeneration),
                },
                new WebModuleMeta()
                {
                    Label = "工单".L10N(),
                    EntityType = typeof(WorkOrder),
                }, new WebModuleMeta()
                {
                    Label = "产品工艺路线设置".L10N(),
                    EntityType = typeof(ProductRouting),
                    BlocksTemplate = typeof(RoutingSettingsTemplate)
                }, new WebModuleMeta()
                {
                    Label = "产线工艺路线设置".L10N(),
                    EntityType = typeof(ResourceRouting)
                }, new WebModuleMeta()
                {
                    Label = "产品工艺路线".L10N(),
                    EntityType = typeof(WipProductRouting),
                    BlocksTemplate = typeof(ProductRoutingTemplate),
                }, new WebModuleMeta()
                {
                    Label = "批次产品工艺路线".L10N(),
                    EntityType = typeof(BatchWipProductRouting),
                    BlocksTemplate = typeof(BatchProductRoutingTemplate),
                }, new WebModuleMeta()
                {
                    EntityType = typeof(PanelBindingRecord),
                    Label = "条码绑定记录".L10N()
                }, new WebModuleMeta()
                {
                    Label = "检验项目".L10N(),
                    EntityType = typeof(ModelInspectionItem)
                }, new WebModuleMeta()
                {
                    Label = "包装号打印".L10N(),
                    EntityType = typeof(PackingWorkOrder),
                    ViewGroup = PackingWorkOrderViewConfig.PackingPrintView
                }, new WebModuleMeta()
                {
                    Label = "包装操作日志".L10N(),
                    EntityType = typeof(RecombineLog),
                }, new WebModuleMeta()
                {
                    Label = "包装清单查询".L10N(),
                    EntityType = typeof(PackingRelationQuery),
                }, new WebModuleMeta()
                {
                    Label = "条码领用".L10N(),
                    EntityType = typeof(WoBarcodeRange),
                }
            //, new WebModuleMeta()
            //{
            //    Label = "拼板码领用".L10N(),
            //    EntityType = typeof(WoPanelRange),
            //}
            //, new WebModuleMeta()
            //{
            //    Label = "工序交接记录".L10N(),
            //    EntityType = typeof(ProcessTransferRecord),
            //}
            , new WebModuleMeta()
            {
                Label = "工序BOM管理".L10N(),
                EntityType = typeof(SIE.MES.Routings.RoutingBoms.RoutingBom)
            }, new WebModuleMeta()
            {
                Label = "工单耗用单".L10N(),
                EntityType = typeof(WoCostItem)
            }, new WebModuleMeta()
            {
                Label = "工单制造档案".L10N(),
                EntityType = typeof(WorkOrderArchive),
            }, new WebModuleMeta()
            {
                Label = "工序委外需求单".L10N(),
                EntityType = typeof(OutsourcingRequest),
            }, new WebModuleMeta()
            {
                Label = "产前准备项目维护".L10N(),
                EntityType = typeof(PrepareProject),
            }, new WebModuleMeta()
            {
                Label = "产品产前准备设置".L10N(),
                EntityType = typeof(PrepareProduct),
            }, new WebModuleMeta()
            {
                Label = "产前准备记录".L10N(),
                ViewGroup = PrepareRecordViewConfig.PrepareRecordViewStr,
                EntityType = typeof(PrepareRecord),
            }, new WebModuleMeta()
            {
                Label = "上料下料记录".L10N(),
                EntityType = typeof(LoadItemsRecord),
            }, new WebModuleMeta()
            {
                Label = "消息队列接收日志",
                EntityType = typeof(EdgeErrorMessage)
            }, new WebModuleMeta()
            {
                Label = "在岗信息",
                EntityType = typeof(OnOffDutyRecrods)
            }, new WebModuleMeta()
            {
                Label = "QTime标准维护",
                EntityType = typeof(QTimeStandard)
            }, new WebModuleMeta()
            {
                Label = "QTime超时报表",
                EntityType = typeof(QTimeReportViewModel)
            }, new WebModuleMeta()
            {
                Label = "有效期标准维护",
                EntityType = typeof(ValidityStandard)
            },
            new WebModuleMeta()
            {
                Label = "工位库龄查询".L10N(),
                EntityType = typeof(BatchWipProductReport),
            }
            , new WebModuleMeta()
            {
                Label = "条码工序指派".L10N(),
                EntityType = typeof(BarcodeProcess),
                ViewGroup = BarcodeProcessViewConfig.MainListViewStr,
            },
            new WebModuleMeta()
            {
                Label = "项目参数表".L10N(),
                EntityType = typeof(ProjectParam),
            },
            new WebModuleMeta()
            {
                Label = "报工方案维护".L10N(),
                EntityType = typeof(WorkReportPlan),
            },
            new WebModuleMeta()
            {
                Label = "工序标准参数管理".L10N(),
                EntityType = typeof(ProcessStandardParam),
            },
            new WebModuleMeta()
            {
                Label = "项目号需求设计".L10N(),
                EntityType = typeof(ProjectDesign),
            },
            new WebModuleMeta()
            {
                Label = "工装维护".L10N(),
                EntityType = typeof(FixtureUphold),
            }, new WebModuleMeta()
            {
                Label = "检具维护".L10N(),
                EntityType = typeof(CheckerUphold),
            }, new WebModuleMeta()
            {
                Label = "产品与产线关系".L10N(),
                EntityType = typeof(ProductLine),
            }, new WebModuleMeta()
            {
                Label = "工序与物料的关系".L10N(),
                EntityType = typeof(ProcessItem),
            }, new WebModuleMeta()
            {
                Label = "工装与产品的关系".L10N(),
                EntityType = typeof(FixtureItem),
            }, new WebModuleMeta()
            {
                Label = "检具与产品的关系".L10N(),
                EntityType = typeof(CheckerItem),
            }, new WebModuleMeta()
            {
                Label = "模具与产品的关系".L10N(),
                EntityType = typeof(EquipAccountItem),
            }, new WebModuleMeta()
            {
                Label = "维护工序属性".L10N(),
                EntityType = typeof(ProcessPty),
            }, new WebModuleMeta()
            {
                Label = "安灯区域".L10N(),
                EntityType = typeof(AndonUphold),
            }, new WebModuleMeta()
            {
                Label = "人员与工作中心".L10N(),
                EntityType = typeof(EmpWorkCentrt),
            }, new WebModuleMeta()
            {
                Label = "产线与安灯区域".L10N(),
                EntityType = typeof(AndonLine),
            }, new WebModuleMeta()
            {
                EntityType = typeof(SIE.MES.Threshold.Threshold),
                Label = "可疑品阈值".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(SIE.MES.BlueLable.BlueLable),
                Label = "HU外箱蓝标".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(PackingQc),
                Label = "包装QC确认".L10N(),
                ViewGroup = PackingQcViewConfig.QcViewStr,
            }, new WebModuleMeta()
            {
                EntityType = typeof(WipPressurePrintTemlpate),
                Label = "耐压工序产品与打印模板关系".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(WipPressure),
                Label = "耐压测试数据查询".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(SIE.MES.BlueLable.BlueLableLevel),
                Label = "蓝标层级".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(MtartProcessLookup),
                Label = "物料分类与工序关系对照表".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(DispoLookup),
                Label = "MRP控制者对照表".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(StandardCapacity),
                Label = "标准产能维护".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(SIE.MES.DataBarcode.DataBarcode),
                Label = "数据条码化".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(QRCodeRule),
                Label = "包装二维码规则维护".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(ItemQRCodeRule),
                Label = "包装物料二维码规则关系".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(EngraveLabel),
                Label = "刻码标签查询".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(PackingReportRecord),
                Label = "包装报工记录".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(OutboundConfirmDetail),
                Label = "发货确认明细".L10N()
            },new WebModuleMeta()
            { 
                EntityType = typeof(SingleQtyRoundUp),
                Label = "单位耗用量向上取整配置表".L10N()
            },new WebModuleMeta()
            { 
                EntityType = typeof(ReworkLayoutVersion),
                Label = "返工工艺路线版本".L10N()
            },new WebModuleMeta()
            { 
                EntityType = typeof(ReworkInfoRecord),
                Label = "返工信息".L10N()
            }, new WebModuleMeta()
            {
                Label = "A在岗信息",
                EntityType = typeof(OnOffDutyRecrodsA)
            }, new WebModuleMeta()
            {
                Label = "B在岗信息",
                EntityType = typeof(OnOffDutyBRecrods)
            }, new WebModuleMeta()
            {
                Label = "看板区域维护",
                EntityType = typeof(DesignerArea)
            }, new WebModuleMeta()
            {
                Label = "人员组织架构",
                EntityType = typeof(OrgLevel)
            }, new WebModuleMeta()
            {
                Label = "产品老化工艺时间维护",
                EntityType = typeof(ProductAgingProcess)
            }, new WebModuleMeta()
            {
                Label = "考勤原始数据",
                EntityType = typeof(ListAtt)
            },new WebModuleMeta()
            { 
                EntityType = typeof(LineArea),
                Label = "产线区域维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(ItemCustomerPartNo),
                Label = "物料客户料码查询"
            }, new WebModuleMeta()
            {
                EntityType = typeof(ScheduleProductionVariance),
                Label = "当日排程生产数量差异"
            }, new WebModuleMeta()
            { 
                EntityType = typeof(EngraveLabelSnReplaceLog),
                Label = "刻码替换记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FeedingInterval),
                Label = "上料间隔时间"
            }, new WebModuleMeta()
            {
                EntityType = typeof(SpcFromMes),               
                Label = "统计过程控制"
            }, new WebModuleMeta()
            {
                EntityType = typeof(ProcessParamCollect),
                Label = "工序参数数采"
            }, new WebModuleMeta()
            {
                EntityType = typeof(BlueLabelReplace),
                Label = "蓝标替换"
            }
            );
        }

        /// <summary>
        /// 添加属性编辑器
        /// </summary>
        /// <param name="app">应用程序</param>
        private void AddPropertyEditor(IApp app)
        {
            //方法重写
        }
    }
}