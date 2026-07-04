using SIE.MetaModel;
using SIE.Modules;
using SIE.Packages;
using SIE.Packages.Boxs;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Wpf.Packages.Packages.Editors;
using SIE.Wpf.Packages.Packings.Editors;

[assembly: Module(typeof(SIE.Wpf.Packages.Module))]

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">Application</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddNewPropertyEditors(app);
        }

        /// <summary>
        /// 模块配置项
        /// </summary>
        /// <param name="sender">支持 .NET Framework 类层次结构中的所有类，并为派生类提供低级别服务。 这是 .NET Framework 中所有类的最终基类；它是类型层次结构的根。若要浏览此类型的.NET Framework 源代码，请参阅Reference Source。</param>
        /// <param name="e">表示包含事件数据的类的基类，并提供用于不包含事件数据的事件的值</param>
        private void App_ModuleOperations(object sender, System.EventArgs e)
        {
            CommonModel.Modules.AddModules(
            new WPFModuleMeta()
            {
                Label = "周转箱",
                EntityType = typeof(TurnoverBox)
            },
            new WPFModuleMeta()
            {
                Label = "包装单位",
                EntityType = typeof(PackingUnit)
            },
            new WPFModuleMeta()
            {
                Label = "包装规则设置",
                EntityType = typeof(PackageRule)
            },
            new WPFModuleMeta()
            {
                Label = "物料标签",
                EntityType = typeof(ItemLabel)
            });
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private static void AddNewPropertyEditors(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BoolToggleButtonEditor.EditorName, typeof(BoolToggleButtonEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PackageRuleDetailLookUpEditor.EditorName, typeof(PackageRuleDetailLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ProductFamilyLookUpEditor.EditorName, typeof(ProductFamilyLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ProductModelLookUpEditor.EditorName, typeof(ProductModelLookUpEditor));
            };
        }
    }
}
