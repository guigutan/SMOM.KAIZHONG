using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.WIP.Inspects.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验项目视图配置
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    public class InspectionItemViewModelViewConfig : WPFViewConfig<InspectionItemViewModel>
    {
        #region 检验标识
        /// <summary>
        /// 检验标识
        /// </summary>
        public static readonly Property<string> CheckTagLabelProperty = P<InspectionItemViewModel>.RegisterExtensionReadOnly("CheckTagLabe", typeof(InspectionItemViewModelViewConfig),
            GetCheckTag, InspectionItemViewModel.ModelInspecitonItemProperty);

        /// <summary>
        /// 获取检验标识
        /// </summary>
        /// <param name="me">检验项目</param>
        /// <returns>返回检验标识</returns>
        public static string GetCheckTag(InspectionItemViewModel me)
        {
            if (me.ModelInspecitonItem != null)
            {
                return me.ModelInspecitonItem?.CheckTag.ToLabel();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 检验标识为定性
        /// <summary>
        /// 检验标识是否定性
        /// </summary>
        public static readonly Property<bool> IsQualitativeProperty = P<InspectionItemViewModel>.RegisterExtensionReadOnly("IsQualitative", typeof(InspectionItemViewModelViewConfig),
            GetIsQualitative, InspectionItemViewModel.ModelInspecitonItemProperty);

        /// <summary>
        /// 检验标识是否定性
        /// </summary>
        /// <param name="me">检验项目</param>
        /// <returns>返回检验标识是否为定性</returns>
        public static bool GetIsQualitative(InspectionItemViewModel me)
        {
            return me.ModelInspecitonItem.CheckTag == CheckTag.Qualitative;
        }
        #endregion

        #region 检验标识为定量
        /// <summary>
        /// 检验标识为定量
        /// </summary>
        public static readonly Property<bool> IsQuantitativeProperty = P<InspectionItemViewModel>.RegisterExtensionReadOnly("IsQuantitative", typeof(InspectionItemViewModelViewConfig),
            GetIsQuantitative, InspectionItemViewModel.ModelInspecitonItemProperty);

        /// <summary>
        /// 检验标识是否定量
        /// </summary>
        /// <param name="me">检验项目</param>
        /// <returns>返回检验标识为定量</returns>
        public static bool GetIsQuantitative(InspectionItemViewModel me)
        {
            return me.ModelInspecitonItem.CheckTag == CheckTag.Quantitative;
        }
        #endregion

        ///// <summary>
        ///// 检验结果数据模板，根据检验标识控制检验结果是否只读
        ///// </summary>
        ///// <param name="isResultBinding">检验结果名称</param>
        ///// <param name="isQualitativeBinding">是否定性名称</param>
        ///// <returns>返回数据模板</returns>
        //private DataTemplate CreateDataTemplate(string isResultBinding, string isQualitativeBinding)
        ////{
        ////    var factory = new FrameworkElementFactory(typeof(CheckBox));
        ////    factory.SetBinding(CheckBox.IsEnabledProperty, new Binding(isQualitativeBinding));
        ////    factory.SetBinding(CheckBox.IsCheckedProperty, new Binding(isResultBinding));
        ////    var dataTemplate = new DataTemplate() { VisualTree = factory };
        ////    dataTemplate.Seal();
        ////    return dataTemplate;
        ////}

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CollectionUITemplate.CollectionUIViewGroup);
            if (ViewGroup == CollectionUITemplate.CollectionUIViewGroup)
            {
                ConfigCollectionView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigCollectionView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(DefectItemAddCommand));
                View.Property(p => p.ModelInspecitonItem.Name).HasLabel("检验项目").Show(ShowInWhere.All).Readonly(true);
                View.Property(CheckTagLabelProperty).HasLabel("检验标识").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.ModelInspecitonItem.LimitLow).HasLabel("规范下限").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.ModelInspecitonItem.LimitMax).HasLabel("规范上限").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.ModelInspecitonItem.Unit.Name).HasLabel("单位名称").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.InspectionValue).Show(ShowInWhere.All).Readonly(IsQualitativeProperty);
                View.Property(p => p.IsOk).UseIsOkBoolCheckEditor().Show(ShowInWhere.All).Readonly(IsQuantitativeProperty);
                View.Property(p => p.IsNg).UseIsNgBoolCheckEditor().Show(ShowInWhere.All).Readonly(IsQuantitativeProperty);
                // View.Property(p => p.IsOk).UseCheckEditor().Show(ShowInWhere.All)
                //    .UseDataTemplate(CreateDataTemplate(InspectionItemViewModel.IsOkProperty.Name, IsQualitativeProperty.Name))
                //View.Property(p => p.IsNg).Show(ShowInWhere.All)
                //    .UseDataTemplate(CreateDataTemplate(InspectionItemViewModel.IsNgProperty.Name, IsQualitativeProperty.Name))
                View.Property(p => p.Remarks).Show(ShowInWhere.All);
            }
        }
    }
}
