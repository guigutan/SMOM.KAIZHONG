using SIE.Domain;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using SIE.Wpf.Tech.Processs.Behaviors;
using SIE.Wpf.Tech.Processs.Commands;
using System;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// 工序配置视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ProcessViewConfig : WPFViewConfig<Process>
    {
        /// <summary>
        /// 产品族编码的Label
        /// </summary>
        private readonly string _productFamilyLabel = "产品族编码".L10N();

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly Property<string> CategoryFormatProperty = P<Process>.RegisterExtensionReadOnly("CategoryFormat", typeof(ProcessViewConfig),
            GetCategoryFormat, Process.ProductFamilyProperty);

        /// <summary>
        /// 获取产品族（编码+名称）
        /// </summary>
        /// <param name="me">工序</param>
        /// <returns>编码+名称</returns>
        public static string GetCategoryFormat(Process me)
        {
            return me.ProductFamily?.Code + "-" + me.ProductFamily?.Name;
        }

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置默认视图
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.AddBehavior(typeof(ProcessBehavior));
            View.RemoveCommands(WPFCommandNames.Undo);
            View.RemoveCommands(typeof(RedoCommand));
            View.ReplaceCommands(typeof(ListCopyCommand), typeof(CopyProcessCommand));
            View.ReplaceCommands(typeof(ListAddCommand), typeof(AddProcessCommand));
            View.ReplaceCommands(typeof(ListEditCommand), typeof(EditProcessCommand));
            View.Property(p => p.Name).Readonly(p => p.ReferenceTimes > 0 && p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.ProductFamilyId).HasLabel(_productFamilyLabel).UsePagingLookUpEditor(p => p.DisplayMember = ProductFamily.CodeProperty.Name);
            View.Property(p => p.ProductFamilyName).HasLabel("产品族名称").Readonly();
            View.Property(p => p.Type).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Segment).Readonly(p => p.ReferenceTimes > 0);
            View.Property(p => p.EnableMoveInControl).Readonly(p => p.IsOutsourcing);
            View.Property(p => p.TransferType).Show(ShowInWhere.Hide);
            View.Property(p => p.IsOutsourcing);
            View.ChildrenProperty(p => p.ParameterList);
            View.ChildrenProperty(p => p.CollectStepList);
            View.ChildrenProperty(p => p.DefectList);
            View.ChildrenProperty(p => p.EmployeeList).IsVisible(false);
            View.ChildrenProperty(p => p.ProcessPackingUnitList);
            View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(ProcessBehavior));
            View.UseDetail(columnCount: 3);
            View.Property(p => p.Name).Readonly(p => p.ReferenceTimes > 0);
            View.Property(p => p.ProductFamily).HasLabel(_productFamilyLabel).UsePagingLookUpEditor(p => p.DisplayMember = ProductFamily.CodeProperty.Name);
            View.Property(p => p.ProductFamily.Name).HasLabel("产品族名称").Readonly();
            View.Property(p => p.Type);
            View.Property(p => p.Segment).Readonly(p => p.ReferenceTimes > 0);
            View.ChildrenProperty(p => p.ParameterList).UseViewGroup(ProcessParameterViewConfig.ProcessParameterView);
            View.ChildrenProperty(p => p.CollectStepList).UseViewGroup(ProcessCollectStepViewConfig.ProcessCollectStepView);
            View.ChildrenProperty(p => p.DefectList).UseViewGroup(ProcessDefectViewConfig.SelProcessDefectViewGroup);
            View.ChildrenProperty(p => p.EmployeeList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProcessPackingUnitList);
            View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.ProductFamily).HasLabel(_productFamilyLabel);
        }

        /// <summary>
        /// 选择实体配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ProductFamily.Code).HasLabel(_productFamilyLabel);
            View.Property(p => p.ProductFamily.Name).HasLabel("产品族名称");
            View.Property(p => p.Type);
            View.Property(p => p.Segment);
            View.Property(CategoryFormatProperty).Show(ShowInWhere.Hide);
        }
    }
}
