using SIE.Domain;
using SIE.Items.ProductFamilys;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Tech.Processs.Scripts;
using SIE.Tech.VictoryStandards;
using System;
using System.Collections.Generic;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序视图配置
    /// </summary>
    public class ProcessViewConfig : WebViewConfig<Process>
    {
        public static readonly string packingUnitProcess = "PackingUnitProcess";
        /// <summary>
        /// 工艺路线属性视图
        /// </summary>
        public const string ProcessRoutingDataViewGroup = "ProcessBaseDataViewGroup";

        /// <summary>
        /// 编码帮助信息
        /// </summary>
        private readonly string _codeHelpInfo = "引用数量大于0且不为新增状态不可编辑".L10N();

        /// <summary>
        /// 产品族编码的Label
        /// </summary>
        private readonly string _productFamilyLabel = "产品族编码".L10N();

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.RequierModels(typeof(ScriptCondition));
            if (ViewGroup == packingUnitProcess)
                PackingUnitProcess();
            View.DeclareExtendViewGroup(ProcessRoutingDataViewGroup);
            if (ViewGroup == ProcessRoutingDataViewGroup) { ProcessRoutingDataView(); }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseCommands("SIE.Web.Tech.Processs.Commands.ProcessImportCommand",
                "SIE.Web.Tech.Processs.Commands.ProcessDownloadTemplateCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.Tech.Processs.Commands.EditProcessCommand");
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Tech.Processs.Commands.AddProcessCommand");
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Tech.Processs.Commands.CopyProcessCommand");
            View.ReplaceCommands(WebCommandNames.Save, "SIE.Web.Tech.Processs.Commands.SaveProcessCommand");


            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.ReferenceTimes > 0 && p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = _codeHelpInfo; });
            View.Property(p => p.Name).ShowInList(150).Readonly(p => p.ReferenceTimes > 0 && p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = _codeHelpInfo; });

            View.Property(p => p.ProductFamily).HasLabel(_productFamilyLabel).UsePagingLookUpEditor((e, o) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(o.CategoryName), nameof(o.ProductFamily.Name));
                e.DicLinkField = keyValues;
            }).ShowInList(150);
            View.Property(p => p.CategoryName).ShowInList(150).HasLabel("产品族名称".L10N()).Readonly();
            View.Property(p => p.Type).UseEnumEditor(p => p.XType = "ProcessTypeEditor").HasLabel("工序类型".L10N()).Readonly(p => p.UpdateDate != null)
                .UseListSetting(e => { e.HelpInfo = "更新日期为空时可编辑".L10N(); });
            //View.Property(p => p.Segment).HasLabel("工段名称").Readonly(p => p.ReferenceTimes > 0)
            //    .UseListSetting(e => { e.HelpInfo = "引用数量大于0不可编辑".L10N(); });
            View.Property(p => p.EnableMoveInControl).Readonly(p => p.IsOutsourcing);
            View.Property(p => p.TransferType).Show(ShowInWhere.Hide);
            View.Property(p => p.IsOutsourcing);
            View.Property(p => p.IsNeedEquipment).ShowInList(130);
            View.Property(p => p.IsReportByZcode).ShowInList();
            View.Property(p => p.ZcodeThreshold).ShowInList(150).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });

            View.Property(p => p.NumberRule).ShowInList(150);
            View.Property(p => p.PrintTemplate).ShowInList(150);

            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.ChildrenProperty(p => p.ParameterList).LazyLoad(false);
            View.ChildrenProperty(p => p.CollectStepList).LazyLoad(false);
            View.ChildrenProperty(p => p.DefectList);
            View.ChildrenProperty(p => p.EmployeeList).IsVisible(false);
            View.ChildrenProperty(p => p.ProcessPackingUnitList);
            View.ChildrenProperty(p => p.SkillList);
            View.ChildrenProperty(p => p.WorkStepList);

            View.AttachDetailChildrenProperty(typeof(Process), (c) =>
            {
                var model = c.Parent as Process;
                model = RF.GetById<Process>(model.Id, new EagerLoadOptions().LoadWithViewProperty());
                return model;
            }, ProcessRoutingDataViewGroup).HasLabel("流程属性默认值").Show(ChildShowInWhere.All).HasOrderNo(210);
        }

        /// <summary>
        /// 工序工艺路线属性页面
        /// </summary>

        protected void ProcessRoutingDataView()
        {
            View.ClearCommands();
            View.DefineFormChildSaveMode(FormChildSaveMode.Save);
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.CanChoose).Show();
                View.Property(p => p.IsRepeat).Show().Visibility(p => p.Type != ProcessType.BatchAssembly && p.Type != ProcessType.BatchFix && p.Type != ProcessType.BatchPacking && p.Type != ProcessType.BatchPqc);
                View.Property(p => p.IsCreateSku).Show();
                View.Property(p => p.IsCalculate).Show();
                View.Property(p => p.IsGenerateTask).Show().Visibility(p => p.Type != ProcessType.Fix && p.Type != ProcessType.BatchFix);
                View.Property(p => p.IsRequirementTask).Show().Visibility(p => p.Type != ProcessType.Fix && p.Type != ProcessType.BatchFix);

                View.Property(p => p.VictoryStandard).Show().Visibility(p => p.Type == ProcessType.Pqc).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<VictoryStandardController>().GetEnableVictoryStandards(c, r);
                });
                View.Property(p => p.RepairVictory).Show().Visibility(p => p.Type == ProcessType.Pqc).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<VictoryStandardController>().GetEnableVictoryStandards(c, r);
                });
                View.Property(p => p.IsStricter).Show().Visibility(p => p.Type == ProcessType.Pqc);
                View.Property(p => p.IsBinding).Show().Visibility(p => p.Type != ProcessType.BatchAssembly && p.Type != ProcessType.BatchFix && p.Type != ProcessType.BatchPacking && p.Type != ProcessType.BatchPqc);

                View.Property(p => p.IsUnBinding).Show().Visibility(p => p.Type != ProcessType.BatchAssembly && p.Type != ProcessType.BatchFix && p.Type != ProcessType.BatchPacking && p.Type != ProcessType.BatchPqc);
                View.Property(p => p.MaxPassNum).UseSpinEditor(p => { p.MinValue = 1; p.AllowDecimals = false; p.AllowBlank = true; }).Show();
                View.Property(p => p.IsNextMoveIn).Show().Visibility(p => p.Type == ProcessType.BatchAssembly || p.Type == ProcessType.BatchFix || p.Type == ProcessType.BatchPacking || p.Type == ProcessType.BatchPqc);
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(3);
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.ReferenceTimes > 0 && p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = _codeHelpInfo; });
            View.Property(p => p.Name).ShowInList(150).Readonly(p => p.ReferenceTimes > 0 && p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = _codeHelpInfo; });
            View.Property(p => p.CategoryCode).HasLabel(_productFamilyLabel).ShowInList(150).Readonly();
            View.Property(p => p.CategoryName).ShowInList(150).HasLabel("产品族名称").Readonly();
            View.Property(p => p.Type).UseEnumEditor(p => p.XType = "ProcessTypeEditor").HasLabel("工序类型").Readonly(p => p.CreateBy > 0)
                .UseListSetting(e => { e.HelpInfo = "创建人存在不可编辑".L10N(); });
            View.Property(p => p.Segment).HasLabel("工段名称").Readonly(p => p.ReferenceTimes > 0)
                .UseListSetting(e => { e.HelpInfo = "引用数量大于0不可编辑".L10N(); });
            View.Property(p => p.EnableMoveInControl).Readonly(p => p.IsOutsourcing);
            View.Property(p => p.TransferType).Show(ShowInWhere.Hide);
            View.Property(p => p.IsOutsourcing);
            View.ChildrenProperty(p => p.ParameterList).UseViewGroup(ProcessParameterViewConfig.ProcessParameterView);
            View.ChildrenProperty(p => p.CollectStepList).UseViewGroup(ProcessCollectStepViewConfig.ProcessCollectStepView);
            View.ChildrenProperty(p => p.DefectList).UseViewGroup(ProcessDefectViewConfig.SelProcessDefectViewGroup);
            View.ChildrenProperty(p => p.EmployeeList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProcessPackingUnitList);
            View.ChildrenProperty(p => p.SkillList).UseViewGroup("ReadOnlyView").Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.WorkStepList).UseViewGroup(WorkStepViewConfig.WorkStepView);
            View.AttachDetailChildrenProperty(typeof(Process), (c) =>
            {
                var model = c.Parent as Process;
                model = RF.GetById<Process>(model.Id, new EagerLoadOptions().LoadWithViewProperty());
                return model;
            }, ProcessRoutingDataViewGroup).HasLabel("流程属性默认值").Show(ChildShowInWhere.All).HasOrderNo(210);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).HasLabel("工序类型");
            View.Property(p => p.ProductFamily).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProductFamilyController>().GetProductFamily(keyword, pagingInfo);
            }).HasLabel(_productFamilyLabel);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ProductFamilyCategoryCode).DisableSort().HasLabel("产品族分类".L10N());
            View.Property(p => p.Type).HasLabel("工序类型".L10N());
            View.Property(p => p.Segment).HasLabel("工段名称".L10N());
            View.Property(p => p.ProductFamilyCode).DisableSort().HasLabel(_productFamilyLabel);
            View.Property(p => p.ProductFamilyName).DisableSort().HasLabel("产品族名称".L10N());
        }

        /// <summary>
        /// 工单包装单位选择工序视图
        /// </summary>
        protected void PackingUnitProcess()
        {
            View.Property(p => p.Code).ShowInList(150);
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.PropertyRef(p => p.ProductFamily.Code).HasLabel(_productFamilyLabel);
            View.Property(p => p.Type).HasLabel("工序类型");
            View.PropertyRef(p => p.Segment.Code).HasLabel("工段编码");
        }
    }
}