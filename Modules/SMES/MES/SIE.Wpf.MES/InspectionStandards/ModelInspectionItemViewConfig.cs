using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.InspectionStandards;
using SIE.Wpf.Defects.Editors;
using SIE.Wpf.MES.InspectionStandards.Commands;
using SIE.Wpf.MES.InspectionStandards.Editors;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.InspectionStandards
{
    /// <summary>
    /// 机型检验项目视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class ModelInspectionItemViewConfig : WPFViewConfig<ModelInspectionItem>
    {
        /// <summary>
        /// "添加"ViewGroup
        /// </summary>
        public const string AddConfig = "AddConfig";

        /// <summary>
        /// "编辑"ViewGroup
        /// </summary>
        public const string EditConfig = "EditConfig";

        #region 根据检测标识判断某字段是否显示
        /// <summary>
        /// 根据检测标识判断某字段是否显示
        /// </summary>
        public static readonly Property<bool> IsCheckTagShowProperty = P<ModelInspectionItem>.RegisterExtensionReadOnly("IsCheckTagShow", typeof(ModelInspectionItemViewConfig), GetIsCheckTagShow, ModelInspectionItem.CheckTagProperty);

        /// <summary>
        /// 根据检测标识判断某字段是否显示
        /// </summary>
        /// <param name="categoryInspectionItem">机型检验项目</param>
        /// <returns>工序名称</returns>
        public static bool GetIsCheckTagShow(ModelInspectionItem categoryInspectionItem)
        {
            return categoryInspectionItem.CheckTag == CheckTag.Quantitative;
        }
        #endregion

        #region 检测标识切换时更改规范上下限和单位的数值  
        /// <summary>
        /// 是否显示
        /// </summary>
        public static readonly Property<bool> IsQualitiveProperty = P<ModelInspectionItem>.RegisterExtensionReadOnly("IsQualitive", typeof(ModelInspectionItemViewConfig),
            GetIsQualitive, ModelInspectionItem.CheckTagProperty);

        /// <summary>
        /// 获取字段是否显示
        /// </summary>
        /// <param name="me">机型检验项目</param>
        /// <returns>bool</returns>
        public static bool GetIsQualitive(ModelInspectionItem me)
        {
            if (me.CheckTag == CheckTag.Qualitative)
            {
                me.LimitLow = null;
                me.LimitLowCompare = null;
                me.LimitMaxCompare = null;
                me.LimitMax = null;
                me.Unit = null;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.GroupBy(ModelInspectionItem.ProcessNameProperty);
            View.DeclareExtendViewGroup(AddConfig, EditConfig);
            switch (ViewGroup)
            {
                case AddConfig:
                    AddConfigView();
                    break;
                case EditConfig:
                    EditConfigView();
                    break;
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddModelInspectionItemCommand), typeof(EditModelInspectionItemCommand));
            View.UseCommands(WPFCommandNames.ListDelete, WPFCommandNames.ListCopy, WPFCommandNames.Export);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ModelName);
            View.Property(p => p.Name);
            View.Property(p => p.Category);
            View.Property(p => p.TestTool);
            View.Property(p => p.CheckTag);
            View.Property(p => p.InspectionMode);
            View.Property(p => p.ModeInspType);
            View.Property(p => p.InspectionBasis).UseMemoEditor(e => { e.AcceptsReturn = true; e.AllowWrapping = TextWrapping.NoWrap; });
            View.Property(p => p.DefectGrade);
            View.Property(p => p.TechnicalRequirements).UseMemoEditor(e => { e.AcceptsReturn = true; e.AllowWrapping = TextWrapping.NoWrap; });
            View.Property(p => p.LimitLow).Visibility(IsCheckTagShowProperty);
            View.Property(p => p.LimitMax).Visibility(IsCheckTagShowProperty);
            View.Property(p => p.UnitName);
            View.Property(p => p.EffectiveStartTime);
            View.Property(p => p.EffectiveEndTime);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Model);
            View.Property(p => p.Process);
        }

        /// <summary>
        /// 添加视图
        /// </summary>
        void AddConfigView()
        {
            View.AssignAuthorize(typeof(ModelInspectionItem));
            View.UseCommands(WPFCommandNames.FormSave);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 3, true))
                {
                    View.Property(p => p.ProcessId).Show(ShowInWhere.All);
                    View.Property(p => p.ModelId).Show(ShowInWhere.All);
                }

                using (View.DeclareGroup("判定标准", 3, true))
                {
                    View.Property(p => p.Name).Show(ShowInWhere.All);
                    View.Property(p => p.Category).Show(ShowInWhere.All);
                    View.Property(p => p.TestTool).Show(ShowInWhere.All);
                    View.Property(p => p.CheckTag).Show(ShowInWhere.All);
                    View.Property(p => p.InspectionMode).Show(ShowInWhere.All).UseEditor(LoadInspectionModeEditor.EditorName);
                    View.Property(p => p.ModeInspType);
                    View.Property(p => p.InspectionBasis).UseMemoEditor(e => { e.AcceptsReturn = true; e.VerticalScrollBarVisibility = ScrollBarVisibility.Auto; }).UseFormSetting(p => p.RowSpan = 2).Show(ShowInWhere.All);
                    View.Property(p => p.DefectGrade).Show(ShowInWhere.All);
                    View.Property(p => p.TechnicalRequirements).UseMemoEditor(e => { e.AcceptsReturn = true; }).UseFormSetting(p => p.RowSpan = 2).Show(ShowInWhere.All);
                    View.Property(p => p.LimitLow).UseEditor(LimitLowEditor.EditorName).Readonly(IsQualitiveProperty).Visibility(IsCheckTagShowProperty).Show(ShowInWhere.All);
                    View.Property(p => p.LimitMax).UseEditor(LimitMaxEditor.EditorName).Readonly(IsQualitiveProperty).Visibility(IsCheckTagShowProperty).Show(ShowInWhere.All);
                    View.Property(p => p.UnitId).Readonly(IsQualitiveProperty).Show(ShowInWhere.All).Visibility(IsCheckTagShowProperty).UsePagingLookUpEditor(p => p.DisplayMember = Unit.NameProperty.Name);
                    View.Property(p => p.EffectiveStartTime).Show(ShowInWhere.All);
                    View.Property(p => p.EffectiveEndTime).Show(ShowInWhere.All);
                }
            }
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        void EditConfigView()
        {
            View.AssignAuthorize(typeof(ModelInspectionItem));
            View.UseCommands(WPFCommandNames.FormSave);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 3, true))
                {
                    View.Property(p => p.ProcessId).Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.ModelId).Show(ShowInWhere.All).Readonly();
                }

                using (View.DeclareGroup("判定标准", 3, true))
                {
                    View.Property(p => p.Name).Show(ShowInWhere.All);
                    View.Property(p => p.Category).Show(ShowInWhere.All);
                    View.Property(p => p.TestTool).Show(ShowInWhere.All);
                    View.Property(p => p.CheckTag).Show(ShowInWhere.All);
                    View.Property(p => p.InspectionMode).Show(ShowInWhere.All).Show(ShowInWhere.All).UseEditor(LoadInspectionModeEditor.EditorName);
                    View.Property(p => p.ModeInspType);
                    View.Property(p => p.InspectionBasis).UseMemoEditor(e => { e.AcceptsReturn = true; }).UseFormSetting(p => p.RowSpan = 2).Show(ShowInWhere.All);
                    View.Property(p => p.DefectGrade).Show(ShowInWhere.All);
                    View.Property(p => p.TechnicalRequirements).Show(ShowInWhere.All).UseMemoEditor(e => { e.AcceptsReturn = true; }).UseFormSetting(p => p.RowSpan = 2);
                    View.Property(p => p.LimitLow).UseEditor(LimitLowEditor.EditorName).Show(ShowInWhere.All).Readonly(IsQualitiveProperty).Visibility(IsCheckTagShowProperty);
                    View.Property(p => p.LimitMax).UseEditor(LimitMaxEditor.EditorName).Show(ShowInWhere.All).Readonly(IsQualitiveProperty).Visibility(IsCheckTagShowProperty);
                    View.Property(p => p.UnitId).UsePagingLookUpEditor(p => p.DisplayMember = Unit.NameProperty.Name).Show(ShowInWhere.All).Readonly(IsQualitiveProperty).Visibility(IsCheckTagShowProperty);
                    View.Property(p => p.EffectiveStartTime).Show(ShowInWhere.All);
                    View.Property(p => p.EffectiveEndTime).Show(ShowInWhere.All);
                }
            }
        }
    }
}