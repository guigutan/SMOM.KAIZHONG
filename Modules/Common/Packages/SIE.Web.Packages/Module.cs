using SIE.MetaModel;
using SIE.Modules;
using SIE.Packages;
using SIE.Packages.Boxs;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.QrCodeParseRules;
using SIE.Web.Packages;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Packages
{
    /// <summary>
    /// Module配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "周转箱",
                EntityType = typeof(TurnoverBox),
                UIGenerator = "SIE.Web.Packages.Boxs.BoxsAuthUIGenerator"
            }, new WebModuleMeta
            {
                Label = "周转箱型号",
                EntityType = typeof(TurnoverBoxModel)
            }, new WebModuleMeta
            {
                Label = "包装规则设置",
                EntityType = typeof(PackageRule)
            }, new WebModuleMeta()
            {
                Label = "包装单位".L10N(),
                EntityType = typeof(PackingUnit)
            }, new WebModuleMeta()
            {
                Label = "物料标签",
                EntityType = typeof(ItemLabel)
            }, new WebModuleMeta()
            {
                EntityType = typeof(QrCodeParseRule),
                Label = "二维码解析规则",
            }, new WebModuleMeta()
            {
                EntityType = typeof(RePackageRule),
                Label = "复核装箱规则",
            });
        }
    }
}