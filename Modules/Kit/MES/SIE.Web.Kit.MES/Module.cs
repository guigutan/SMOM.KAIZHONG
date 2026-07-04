using SIE.Kit.MES.CallMaterials;
using SIE.Kit.MES.StationStorages;
using SIE.Kit.MES.Storages;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.Kit.MES.CallMaterials;
using SIE.Web.MES;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Kit.MES
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            WebResourceConfig.AddFilterModule(GetType());
            AddPropertyEditor(app);
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工位货区".L10N(),
                EntityType = typeof(StorageArea)
            }, new WebModuleMeta()
            {
                Label = "工位库存".L10N(),
                EntityType = typeof(StationStorage)
            }, new WebModuleMeta()
            {
                Label = "工单".L10N(),
                EntityType = typeof(WorkOrder),
            }, new WebModuleMeta()
            {
                Label = "叫料单管理".L10N(),
                EntityType = typeof(CallMaterialWorkOrder),
                ViewGroup = CallMaterialWorkOrderViewConfig.CallMaterialView,
                UIGenerator = "SIE.Web.MES.CallMaterials.Scripts.CallMaterialWOGenerator"
            }, new WebModuleMeta()
            {
                Label = "叫料原因".L10N(),
                EntityType = typeof(CallMaterialReason),
            }, new WebModuleMeta()
            {
                Label = "物料退料记录表".L10N(),
                EntityType = typeof(CallMaterialWithdrawal)
            }, new WebModuleMeta()
            {
                Label = "物料接收记录表".L10N(),
                EntityType = typeof(CallMaterialReceive)
            });
        }

        /// <summary>
        /// 添加属性编辑器
        /// </summary>
        /// <param name="app">应用程序</param>
        private void AddPropertyEditor(IApp app)
        {
            //方法重写
        }
    }
}