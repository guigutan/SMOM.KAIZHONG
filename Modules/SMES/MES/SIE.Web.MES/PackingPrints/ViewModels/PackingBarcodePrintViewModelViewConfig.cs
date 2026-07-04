using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using System;

namespace SIE.Web.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 包装号打印视图配置
    /// </summary>
    internal class PackingBarcodePrintViewModelViewConfig : WebViewConfig<PackingBarcodePrintViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDetail(dialogWidth: 900);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(PackingWorkOrder));
            View.UseCommands(typeof(Commands.PrintCommand).FullName);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("工单信息", 3))
                {
                    View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly();
                    View.Property(p => p.WorkOrderPlanQty).HasLabel("计划数量").UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.WorkOrderPlanBeginDate).HasLabel("计划日期").UseDateTimeEditor().Readonly();
                    View.Property(p => p.PackageRuleDetail).UseDataSource((e, c, key) =>
                    {
                        var entity = e as PackingBarcodePrintViewModel;
                        if (entity == null)
                            return new EntityList<PackageRuleDetailViewModel>();
                        return RT.Service.Resolve<PackingBarcodeController>().GetWorkOrderPackageRuleDetailList(entity.WorkOrderId, key, c);
                    }).HasLabel("包装规则");
                    View.Property(p => p.ProductQty).Readonly();
                    View.Property(p => p.NumberRule).HasLabel("条码规则").Readonly();
                    View.Property(p => p.BarcodeRuleDtl).HasLabel("规则明细").Readonly();
                }
                using (View.DeclareGroup("打印信息", 4))
                {
                    View.Property(p => p.Template).UseDataSource((e, p, r) =>
                    {
                        return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(PackingPrintable).GetQualifiedName(), p, r);
                    }).ShowInDetail(columnSpan: 2).HasLabel("打印模板");
                    View.Property(p => p.PrintedQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.ResidualQty).UseSpinEditor(e => e.MinValue = 0).Readonly();
                    View.Property(p => p.PrintQty).UseSpinEditor(e => e.MinValue = 0);
                    View.Property(p => p.PageCount).UseSpinEditor(e => e.MinValue = 0);
                    View.Property(p => p.BeginSn).Readonly();
                    View.Property(p => p.EndSn).Readonly();
                }
                using (View.DeclareGroup("打印控制"))
                {
                    View.Property(p => p.PrintControl);
                }
            }
        }
    }
}
