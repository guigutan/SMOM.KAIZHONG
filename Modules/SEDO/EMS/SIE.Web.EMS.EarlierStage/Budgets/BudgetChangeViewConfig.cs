using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.EarlierStage.Budgets.Commands;
using SIE.Web.EMS.EarlierStage.WorkFlows;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算变更-界面
    /// </summary>
    public class BudgetChangeViewConfig : WebViewConfig<BudgetChange>
    {
        /// <summary>
        /// 预算-变更记录
        /// </summary>
        public const string BudgetView = "BudgetView";

        /// <summary>
        /// 变更记录
        /// </summary>
        public const string ChangeRecordView = "ChangeRecordView";

        /// <summary>
        /// 千分位
        /// </summary>
        private const string percent = "0,000.00";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BudgetView, ChangeRecordView);
            if (ViewGroup == BudgetView)
            {
                ConfigBudgetView();
            }
            if (ViewGroup == ChangeRecordView)
            {
                ConfigChangeRecordView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.EarlierStage.Budgets.Commands.EditBudgetChangeCommand", typeof(DeleteBudgetChangeCommand).FullName,
                typeof(SubmitBudgetChangeCommand).FullName, typeof(CancelBudgetChangeCommand).FullName, typeof(ExamineBudgetChangeCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryName).ShowInList(120).FixColumn();
            View.Property(p => p.DepartmentName).ShowInList(120).FixColumn();
            View.Property(p => p.ApprovalStatus).ShowInList(100).FixColumn();
            View.Property(p => p.BudgetNo).ShowInList(150).FixColumn();
            View.Property(p => p.BudgetName).ShowInList(200);
            View.Property(p => p.BudgeGrade).ShowInList(80);
            View.Property(p => p.BudgetContent).ShowInList(200);
            View.Property(p => p.BudgetClass).ShowInList(120);
            View.Property(p => p.BudgetAmount).UseSpinEditor(c => c.Format = percent).ShowInList(120);
            View.Property(p => p.ReserveAmount).UseSpinEditor(c => c.Format = percent).ShowInList(120);
            View.Property(p => p.UsedAmount).UseSpinEditor(c => c.Format = percent).ShowInList(120);
            View.Property(p => p.CanUseAmount).UseSpinEditor(c => c.Format = percent).ShowInList(120);
            View.Property(p => p.ApprovalTime).ShowInList(150);
            View.AttachDetailChildrenProperty(typeof(BudgetChange), (c) =>
            {
                var model = c.Parent as BudgetChange;
                model = RF.GetById<BudgetChange>(model.Id);
                return model;
            }, ChangeRecordView).HasLabel("变更内容");
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<BudgetChange>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(BudgetChange).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核记录");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SaveBudgetChangeCommand).FullName);
            using (View.DeclareGroup("基本信息", 8))
            {
                View.Property(p => p.BudgetId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<BudgetController>().GetCanChangeBudgets(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ApprovalStatus), nameof(e.Budget.ApprovalStatus));
                    keyValues.Add(nameof(e.Year), nameof(e.Budget.Year));
                    keyValues.Add(nameof(e.BudgeGrade), nameof(e.Budget.BudgeGrade));
                    keyValues.Add(nameof(e.InvestClass), nameof(e.Budget.InvestClass));
                    keyValues.Add(nameof(e.BudgetName), nameof(e.Budget.BudgetName));
                    keyValues.Add(nameof(e.BudgetCreateByName), nameof(e.Budget.CreateByName));
                    keyValues.Add(nameof(e.BudgetCreateDate), nameof(e.Budget.CreateDate));
                    keyValues.Add(nameof(e.BudgetClass), nameof(e.Budget.BudgetClass));
                    keyValues.Add(nameof(e.BudgetAmount), nameof(e.Budget.BudgetAmount));
                    keyValues.Add(nameof(e.Currency), nameof(e.Budget.Currency));
                    keyValues.Add(nameof(e.AmountUnit), nameof(e.Budget.AmountUnit));
                    keyValues.Add(nameof(e.TargetName), nameof(e.Budget.TargetName));
                    keyValues.Add(nameof(e.Qty), nameof(e.Budget.Qty));
                    keyValues.Add(nameof(e.UnitName), nameof(e.Budget.UnitName));
                    keyValues.Add(nameof(e.Price), nameof(e.Budget.Price));
                    keyValues.Add(nameof(e.PriceBasis), nameof(e.Budget.PriceBasis));
                    keyValues.Add(nameof(e.BudgetContent), nameof(e.Budget.BudgetContent));
                    keyValues.Add(nameof(e.FactoryName), nameof(e.Budget.FactoryName));
                    keyValues.Add(nameof(e.DepartmentName), nameof(e.Budget.DepartmentName));
                    m.DicLinkField = keyValues;
                }).HasLabel("预算编号").ShowInDetail(columnSpan: 2).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.Property(p => p.ApprovalStatus).ShowInDetail(columnSpan: 2).HasLabel("预算状态").Readonly();
                View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor().Readonly();
                View.Property(p => p.BudgeGrade).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.InvestClass).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Budget.InvestClassify; e.ReloadDataOnPopping = true; }).Readonly();
                View.Property(p => p.BudgetName).ShowInDetail(columnSpan: 4).Readonly();
            }
            using (View.DeclareGroup("单位、人员", 8))
            {
                View.Property(p => p.FactoryName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.DepartmentName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.BudgetCreateByName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.BudgetCreateDate).ShowInDetail(columnSpan: 2).Readonly();
            }
            using (View.DeclareGroup("预算内容", 8))
            {
                View.Property(p => p.BudgetClass).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Budget.BudgetClassify; e.ReloadDataOnPopping = true; }).Readonly();
                View.Property(p => p.BudgetAmount).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Currency).Readonly();
                View.Property(p => p.AmountUnit).Readonly();
                View.Property(p => p.TargetName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Qty).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.UnitName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Price).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.PriceBasis).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.BudgetContent).UseMemoEditor().HasLabel("预算说明").ShowInDetail(columnSpan: 8, rowSpan: 2).Readonly();
                View.Property(p => p.NewAmount).HasLabel("变更后预算金额").ShowInDetail(columnSpan: 2).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                });
                View.Property(p => p.Description).ShowInDetail(columnSpan: 6);
            }
        }

        /// <summary>
        /// 预算-变更记录
        /// </summary>
        protected void ConfigBudgetView()
        {
            View.ClearCommands();
            View.AssignAuthorize(typeof(Budget));
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ChangeContent).ShowInList(80);
                View.Property(p => p.OriginalAmount).HasLabel("变更前").ShowInList(120);
                View.Property(p => p.NewAmount).HasLabel("变更后").ShowInList(120);
                View.Property(p => p.Description).ShowInList(200);
                View.Property(p => p.CreateByName).HasLabel("申请人").ShowInList(100);
                View.Property(p => p.CreateDate).HasLabel("申请时间").ShowInList(150);
                View.Property(p => p.ApprovalStatus).HasLabel("状态").ShowInList(100);
                View.Property(p => p.ApprovalTime).ShowInList(150);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 变更记录
        /// </summary>
        protected void ConfigChangeRecordView()
        {
            View.ClearCommands();
            View.AssignAuthorize(typeof(BudgetChange));
            View.DisableEditing();
            View.UseDetail(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.OriginalAmount).HasLabel("变更前预算金额").ShowInDetail(columnSpan: 2);
                View.Property(p => p.NewAmount).HasLabel("变更后预算金额").ShowInDetail(columnSpan: 2);
                View.Property(p => p.Description).UseMemoEditor().ShowInDetail(columnSpan: 4, rowSpan: 4);
            }
        }
    }
}
