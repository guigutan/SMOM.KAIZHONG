using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Projects;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.EarlierStage.Budgets.Commands;
using SIE.Web.EMS.EarlierStage.Projects;
using SIE.Web.EMS.EarlierStage.WorkFlows;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;

namespace SIE.Web.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算-界面
    /// </summary>
    internal class BudgetViewConfig : WebViewConfig<Budget>
    {
        /// <summary>
        /// 千分位
        /// </summary>
        private const string percent = "0,000.00";

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior");
            View.UseCommands(typeof(AddBudgetCommand).FullName, "SIE.Web.EMS.EarlierStage.Budgets.Commands.EditBudgetCommand",
                typeof(DeleteBudgetCommand).FullName, typeof(SubmitBudgetCommand).FullName, typeof(CancelBudgetCommand).FullName,
                typeof(ExamineBudgetCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection,
                typeof(BudgetImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.FactoryId).FixColumn().ShowInList(120);
            View.Property(p => p.DepartmentId).FixColumn().ShowInList(120);
            View.Property(p => p.BudgetNo).ShowInList(130).FixColumn();
            View.Property(p => p.BudgetName).ShowInList(200).FixColumn();
            View.Property(p => p.BudgeGrade).ShowInList(80).FixColumn();
            View.Property(p => p.ApprovalStatus).ShowInList(100);
            View.Property(p => p.InvestClass).ShowInList(160).UseCatalogEditor(e => { e.CatalogType = Budget.InvestClassify; e.ReloadDataOnPopping = true; });
            View.Property(p => p.Year).ShowInList(80).UseYearEditor();
            View.Property(p => p.BudgetContent).ShowInList(200).HasLabel("预算说明");
            View.Property(p => p.BudgetClass).UseCatalogEditor(e => { e.CatalogType = Budget.BudgetClassify; e.ReloadDataOnPopping = true; });
            View.Property(p => p.TargetName).ShowInList(200);
            View.Property(p => p.Qty).ShowInList(60);
            View.Property(p => p.UnitName).ShowInList(60);
            View.Property(p => p.Price).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.PriceBasis).ShowInList(200);
            View.Property(p => p.BudgetAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.Currency).ShowInList(60);
            View.Property(p => p.AmountUnit).ShowInList(80);
            View.Property(p => p.ReserveAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.UsedAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.Property(p => p.CanUseAmount).UseSpinEditor(c => c.Format = percent).ShowInList(110);
            View.AttachChildrenProperty(typeof(Project), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Budget>();
                if (parent == null)
                {
                    return new EntityList<Project>();
                }
                return RT.Service.Resolve<ProjectController>().GetProjectByBudgetId(parent.Id, args.SortInfo, args.PagingInfo);
            }, ProjectViewConfig.SeeView).HasLabel("项目列表");
            View.AttachChildrenProperty(typeof(BudgetChange), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Budget>();
                if (parent == null)
                {
                    return new EntityList<BudgetChange>();
                }
                return RT.Service.Resolve<BudgetChangeController>().GetChangeByBudgetId(parent.Id, args.SortInfo, args.PagingInfo);
            }, BudgetChangeViewConfig.BudgetView).HasLabel("变更记录");
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Budget>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(Budget).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核进度");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SaveBudgetCommand).FullName, typeof(EditSubmitBudgetCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Budgets.BudgetBehavior");
            using (View.DeclareGroup("基本信息", 8))
            {
                View.Property(p => p.BudgetNo).Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.ApprovalStatus).ShowInDetail(columnSpan: 2).HasLabel("预算状态").Readonly();
                View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor();
                View.Property(p => p.BudgeGrade).ShowInDetail(columnSpan: 2);
                View.Property(p => p.InvestClass).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Budget.InvestClassify; e.CatalogReloadData = true; });
                View.Property(p => p.BudgetName).ShowInDetail(columnSpan: 4);
            }
            using (View.DeclareGroup("单位、人员", 8))
            {
                View.Property(p => p.FactoryId).ShowInDetail(columnSpan: 2).UseFactoryEditor().Cascade(p => p.DepartmentId, null);
                View.Property(p => p.DepartmentId).ShowInDetail(columnSpan: 2).UseUserBudgetDepartmentEditor();
                View.Property(p => p.CreateByName).ShowInDetail(columnSpan: 2).HasLabel("编制人");
                View.Property(p => p.CreateDate).ShowInDetail(columnSpan: 2).HasLabel("编制时间");
            }
            using (View.DeclareGroup("预算内容", 8))
            {
                View.Property(p => p.BudgetClass).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Budget.BudgetClassify; e.CatalogReloadData = true; });
                View.Property(p => p.BudgetAmount).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 2;
                }).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Currency);
                View.Property(p => p.AmountUnit).Readonly();
                View.Property(p => p.TargetName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Qty).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = false;
                }).ShowInDetail(columnSpan: 2);
                View.Property(p => p.UnitName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Price).HasLabel("预估单价").UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 2;
                }).ShowInDetail(columnSpan: 2);
                View.Property(p => p.PriceBasis).ShowInDetail(columnSpan: 2);
                View.Property(p => p.BudgetContent).UseMemoEditor().HasLabel("预算说明").ShowInDetail(columnSpan: 8, rowSpan: 2);
            }
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.BudgetNo);
            View.Property(p => p.BudgetName);
            View.Property(p => p.CanUseAmount);
        }

        /// <summary>
        /// 配置导入模版
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.BudgetNo);

            View.PropertyRef(p => p.Factory.Name).HasLabel("工厂").ImportRemark("必填；事业部下一级单位，填写规则如：顺德工厂");
            View.PropertyRef(p => p.Department.Name).HasLabel("部门").ImportRemark("必填；可以到分厂或车间一级，填写规则如：生产部_供应链管理组");
            View.Property(p => p.BudgetNo).ImportRemark("预算编号；必填；手工填写");
            View.Property(p => p.BudgetName).ImportRemark("预算名称；必填；手工填写"); 
            View.Property(p => p.BudgeGrade).ImportRemark("必填；选择一级/二级；一级预算允许多次立项"); 
            View.Property(p => p.InvestClass)
                .ImportCatalogType(Budget.InvestClassify, Catalog.NameProperty.Name);
            View.Property(p => p.BudgetYear).ImportRemark("必填；格式：YYYY");
            View.Property(p => p.BudgetContent).ImportRemark("预算内容；必填；手工填写");
            View.Property(p => p.BudgetClass)
                .ImportCatalogType(Budget.BudgetClassify, Catalog.NameProperty.Name);
            View.Property(p => p.TargetName).ImportRemark("标的名称；手工填写");
            View.Property(p => p.Qty).ImportRemark("数量；手工填写"); 
            View.Property(p => p.Price).ImportRemark("预算单价；手工填写"); 
            View.Property(p => p.UnitName).ImportRemark("单位；手工填写");
            View.Property(p => p.PriceBasis).ImportRemark("单价依据；手工填写");
            View.Property(p => p.BudgetAmount).ImportRemark("预算金额；必填；手工填写");
            View.Property(p => p.Currency);
        }
    }
}
