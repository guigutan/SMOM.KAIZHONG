using SIE.Barcodes.WipBatchs;
using SIE.MES.BatchGeneration;
using SIE.ObjectModel;
using SIE.Web.Items._Extentions_;
using System;
using System.Linq.Expressions;
using SIE.Web.Barcodes;
using SIE.Resources.WipResources;
using AngleSharp.Common;
using SIE.Tech.Stations;
using SIE.Domain;
using SIE.MES.BatchGeneration.Services;
using System.Linq;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 批次生成ViewModel视图配置
    /// </summary>
    public class BatchGeneratingModelViewConfig : WebViewConfig<BatchGeneratingViewModel>
    {

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WOBatchGeneration));
        }

        /// <summary>
        /// 表单试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands("SIE.Web.MES.BatchGeneration.Commands.GenerateCommand", "SIE.Web.MES.BatchGeneration.Commands.GenerateAndPrintCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工作单元信息", 3))
                {
                    View.Property(p => p.ResourceId).UseDataSource((r, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<WipResourceController>().GetWipResources(RT.IdentityId, pagingInfo, keyword);
                    }).HasLabel("资源");
                    View.Property(p => p.ProcessId).UseDataSource((r, pagingInfo, keyword) =>
                    {
                        if (r == null || (r as BatchGeneratingViewModel) == null)
                        {
                            return new EntityList<Station>();
                        }
                        var entity = r as BatchGeneratingViewModel;
                        var result = RT.Service.Resolve<WOBatchGenerationService>().GetProcessesByWoId(entity.BatchWoId.Value, SIE.Tech.Processs.ProcessType.BatchAssembly, true, pagingInfo, keyword);
                        if (!result.Any())//如果一个都取不到。取所有非开始工序
                        {
                            return RT.Service.Resolve<WOBatchGenerationService>().GetProcessesByWoId(entity.BatchWoId.Value, SIE.Tech.Processs.ProcessType.BatchAssembly, false, pagingInfo, keyword);
                        }
                        return result;

                    }).HasLabel("工序");
                    View.Property(p => p.StationId).UseDataSource((r, pagingInfo, keyword) =>
                    {
                        if (r == null || (r as BatchGeneratingViewModel) == null)
                        {
                            return new EntityList<Station>();
                        }
                        var entity = r as BatchGeneratingViewModel;
                        return RT.Service.Resolve<StationController>().GetStationsByResourceId(entity.ResourceId, entity.ProcessId);
                    }).HasLabel("工位");
                }

                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.BatchWoNo).HasLabel("工单号").Readonly();
                    View.Property(p => p.PlanQty).HasLabel("计划数量").Readonly();
                    View.Property(p => p.NotGenerateQty).HasLabel("剩余数量").Readonly()
                        .UseListSetting(e => { e.HelpInfo = "剩余数量=计划数量-已生成批次数量+报废数量"; });
                }

                using (View.DeclareGroup("批次信息", 2))
                {
                    View.Property(p => p.BatchQty).UseSpinEditor(p => { p.MinValue = 0; }).HasLabel("批次数量");
                    View.Property(p => p.NumberRule).UseNumberRuleBarcodeLookUpEditor()
                        .UseListSetting(e => { e.HelpInfo = "显示规则类型为产品条码的编码规则"; });
                }

                using (View.DeclareGroup("打印信息", 3))
                {
                    View.Property(p => p.Template).UseBatchPrintTemplateLookUpEditor();
                    //View.Property(p => p.PageCount).UseSpinEditor(p => { p.AllowDecimals = false; p.MinValue = 1; });
                }
            }
        }
    }
}