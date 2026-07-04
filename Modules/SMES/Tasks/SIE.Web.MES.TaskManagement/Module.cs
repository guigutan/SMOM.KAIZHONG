using SIE.MES.TaskManagement.Completion;
using SIE.MES.TaskManagement.DailyOutputReports;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.HeatTreatments;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.PackingDetailReports;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.TaskManagement.ProcessTaskLists;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.MES.TaskManagement.SchedulingInfs.Reports;
using SIE.MES.TaskManagement.Specifications;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.MES.TaskManagement.StockDeducRecords;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using SIE.MES.TaskManagement.WipProgress;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.MES.TaskManagement;
using SIE.Web.MES.TaskManagement.Dispatchs;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.MES.TaskManagement
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
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "产品规格件对照表".L10N(),
                EntityType = typeof(ProductSpecification)
            }, new WebModuleMeta()
            {
                Label = "规格件清单".L10N(),
                EntityType = typeof(Specification)
            }, new WebModuleMeta()
            {
                Label = "派工管理".L10N(),
                EntityType = typeof(DispatchTask),
            },
             new WebModuleMeta()
             {
                 Label = "报工管理".L10N(),
                 EntityType = typeof(ReportDispatchTask),
                 ViewGroup = ReportDispatchTaskViewConfig.reportDispatchListView,
                 UIGenerator = "SIE.Web.MES.TaskManagement.Reports.ReportGenerator"
             }, new WebModuleMeta()
             {
                 Label = "报工记录".L10N(),
                 EntityType = typeof(ReportRecordExamine),
             }, new WebModuleMeta()
             {
                 Label = "MES排程导入中间表".L10N(),
                 EntityType = typeof(SchedulingInf),
             }, new WebModuleMeta()
             {
                 Label = "排程导入详情".L10N(),
                 EntityType = typeof(SchedulingInfReport),
             }, new WebModuleMeta()
             {
                 Label = "排程导入详情新".L10N(),
                 EntityType = typeof(SIE.MES.TaskManagement.SchedulingInfReports.SchedulingInfReport),
             }, new WebModuleMeta()
             {
                 EntityType = typeof(ProcessPrepareRecord),
                 Label = "工序产前准备记录".L10N()
             },
             new WebModuleMeta()
             {
                 Label = "产品标准工时维护".L10N(),
                 EntityType = typeof(StandardHourSet),
             },
             new WebModuleMeta()
             {
                 Label = "工序任务清单".L10N(),
                 EntityType = typeof(ProcessTaskListViewModel),
             },
             new WebModuleMeta()
             {
                 Label = "上料记录".L10N(),
                 EntityType = typeof(FeedingRecord)
             },
             new WebModuleMeta()
             {
                 Label = "开机准备记录查询".L10N(),
                 EntityType = typeof(PreStartupSetupRecord)
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(SuspectProductLabel),
                 Label = "可疑品标签".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(DeductionRecord),
                 Label = "扣料记录".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(WipProgressViewModel),
                 Label = "在制品查询".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(HeatTreatment),
                 Label = "老化炉标签进出炉记录".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(ReportScanLabelLog),
                 Label = "报工扫描标签记录".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(FeedingArea),
                 Label = "供料区维护".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(ProductCompletion),
                 Label = "资源生产完成情况查询".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(DailyOutputReport),
                 Label = "日产出报表".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(ScrapWeighingRecord),
                 Label = "余料称重记录".L10N()
             }
             , new WebModuleMeta()
             {
                 EntityType = typeof(AxisChangeRecord),
                 Label = "IOT押出换轴记录".L10N()
             }
             ,new WebModuleMeta()
             { 
                EntityType = typeof(WeightOfSamplingReport),
                Label= "取样净重详情表".L10N()
             }
              , new WebModuleMeta()
              {
                  EntityType = typeof(ScrapDetail),
                  Label = "报废明细".L10N()
              }
               , new WebModuleMeta()
               {
                   EntityType = typeof(PackingDetailReport),
                   Label = "包装QC确认明细报表".L10N()
               }
             );
        }
    }
}