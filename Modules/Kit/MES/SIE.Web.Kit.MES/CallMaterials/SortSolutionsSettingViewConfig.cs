using SIE.Kit.MES.CallMaterials;
using SIE.MetaModel.View;
using SIE.Web.Kit.MES.CallMaterials.Commands;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 排序方案设置视图配置
    /// </summary>
    internal class SortSolutionsSettingViewConfig : WebViewConfig<SortSolutionsSetting>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CallMaterialWorkOrder));
            View.HasDelegate(SortSolutionsSetting.NameProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RequirModuleResource("SIE.Web.Kit.MES.CallMaterials.Commands.SortSolutionsSettingCommand.js");
            View.RequirModuleResource("SIE.Web.Kit.MES.CallMaterials.Commands.SetDefaultSolSettingCommand.js");
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(SaveSolSettingCommand).FullName);
            View.RemoveCommands(WebCommandNames.ExportXls, WebCommandNames.Copy);
            View.UseCommands(typeof(SetDefaultSolSettingCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Name);
                View.Property(p => p.Description);
                View.Property(p => p.IsDefault).Readonly(true);
            }

            View.ChildrenProperty(p => p.PriorityList);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            //方法重写
        }
    }
}