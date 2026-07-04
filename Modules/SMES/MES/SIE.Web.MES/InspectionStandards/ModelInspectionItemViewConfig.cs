using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.InspectionStandards;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.EMS.InventoryTasks.Commands;
using SIE.Web.MES.InspectionStandards.Commands;
using System.Collections.Generic;

namespace SIE.Web.MES.InspectionStandards
{
    /// <summary>
    /// 机型项目检验视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class ModelInspectionItemViewConfig : WebViewConfig<ModelInspectionItem>
    {
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
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(InspectionAddCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Save, typeof(InspectionSaveCommand).FullName);
            View.UseCommands(typeof(ImportInspectionCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.UseClientOrder();
            View.Property(p => p.Process).UseDataSource((s, p, k) =>
            {
                var source = s as ModelInspectionItem;
                if (source != null)
                {
                    return RT.Service.Resolve<ModelInspectionItemController>().GetPqcProcess(p, k);
                }
                else
                {
                    return new EntityList<Process>();
                }
            });
            View.Property(p => p.Model);
            View.Property(p => p.ProductItem).UseDataSource((s, p, k) =>
            {
                var source = s as ModelInspectionItem;
                if (source != null)
                {
                    return RT.Service.Resolve<ModelInspectionItemController>().GetTargetItems(p, k);
                }
                else
                {
                    return new EntityList<Item>();
                }
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>
                {
                    { nameof(e.ProductItemName), nameof(e.ProductItem.Name) }
                };
                m.DicLinkField = keyValues;
            }).HasLabel("产品编码");
            View.Property(p => p.ProductItemName).Readonly();
            View.Property(p => p.Name);
            View.Property(p => p.OrderNum).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.Category);
            View.Property(p => p.TestTool);
            View.Property(p => p.CheckTag).Cascade(p => p.LimitLowCompare, null).Cascade(p => p.LimitLow, null)
                .Cascade(p => p.LimitMaxCompare, null).Cascade(p => p.LimitMax, null);
            View.Property(p => p.InspectionMode).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<InspectionItemController>().GetInspectionModes(p, s);
            }).Show(ShowInWhere.All);
            View.Property(p => p.InspectionBasis);
            View.Property(p => p.DefectGrade);
            View.Property(p => p.TechnicalRequirements);
            View.Property(p => p.LimitLowCompare).UseEnumEditor("Greater").Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.LimitLow).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.LimitMaxCompare).UseEnumEditor("Less").Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.LimitMax).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.Unit).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.EffectiveStartTime);
            View.Property(p => p.EffectiveEndTime);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseCommands(WebCommandNames.FormSave);
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
                    View.Property(p => p.InspectionMode).UseDataSource((e, p, s) =>
                    {
                        return RT.Service.Resolve<InspectionItemController>().GetInspectionModes( p, s);
                    }).Show(ShowInWhere.All);
                    View.Property(p => p.InspectionBasis).UseMemoEditor().ShowInDetail(rowSpan: 2);
                    View.Property(p => p.DefectGrade).Show(ShowInWhere.All);
                    View.Property(p => p.TechnicalRequirements).UseMemoEditor().ShowInDetail(rowSpan: 2);
                    View.Property(p => p.LimitLow).Visibility(p => p.CheckTag == CheckTag.Quantitative).Show(ShowInWhere.All);
                    View.Property(p => p.LimitMax).Visibility(p => p.CheckTag == CheckTag.Quantitative).Show(ShowInWhere.All);
                    View.Property(p => p.Unit).Show(ShowInWhere.All).Visibility(p => p.CheckTag == CheckTag.Quantitative);
                    View.Property(p => p.EffectiveStartTime).Show(ShowInWhere.All);
                    View.Property(p => p.EffectiveEndTime).Show(ShowInWhere.All);
                }
            }
        }

        /// <summary>
        ///配置导入方法
        /// </summary>
        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).HasLabel("检验项目");
                View.PropertyRef(p => p.Model.Name).HasLabel("机型");
                View.PropertyRef(p => p.ProductItem.Code).ImportIndexer().HasLabel("产品编码");
                View.PropertyRef(p => p.Process.Code).ImportIndexer().HasLabel("工序");
                View.Property(p => p.Category).HasLabel("检验类别");
                View.Property(p => p.TestTool).HasLabel("检验工具");
                View.Property(p => p.CheckTag).HasLabel("检验标识");
                View.PropertyRef(p => p.InspectionMode.Name).HasLabel("检验方式");

                View.Property(p => p.InspectionBasis).HasLabel("检验依据");
                View.PropertyRef(p => p.DefectGrade.Name).HasLabel("缺陷等级");
                View.Property(p => p.TechnicalRequirements).HasLabel("技术要求");
                View.Property(p => p.LimitLowCompare).UseEnumEditor("Greater");
                View.Property(p => p.LimitLow);
                View.Property(p => p.LimitMaxCompare).UseEnumEditor("Less");
                View.Property(p => p.LimitMax);
                View.PropertyRef(p => p.Unit.Name).HasLabel("单位名称");
                View.Property(p => p.EffectiveStartTime).HasLabel("生效日期");
                View.Property(p => p.EffectiveEndTime).HasLabel("失效日期");
            }

        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置选择视图
        }
    }
}
