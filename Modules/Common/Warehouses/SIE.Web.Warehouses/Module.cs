using SIE.MetaModel;
using SIE.Modules;
using SIE.Warehouses;
using SIE.Warehouses.Stations;
using SIE.Web.Warehouses;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// UIModule
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
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
            new WebModuleMeta()
            {
                Label = "仓库",
                EntityType = typeof(Warehouse)
            },
            new WebModuleMeta()
            {
                Label = "库区",
                EntityType = typeof(StorageArea)
            },
             new WebModuleMeta()
             {
                 Label = "库位",
                 EntityType = typeof(StorageLocation)
             },
             new WebModuleMeta()
             {
                 Label = "工作区",
                 EntityType = typeof(WorkArea)
             },
             new WebModuleMeta()
             {
                 Label = "巷道",
                 EntityType = typeof(Routeway)
             },
             new WebModuleMeta()
             {
                 Label = "逻辑分区",
                 EntityType = typeof(LogicArea)
             },
             new WebModuleMeta()
             {
                 Label = "站台",
                 EntityType = typeof(Station),
             },
             new WebModuleMeta()
             {
                 Label = "站台组",
                 EntityType = typeof(StationGroup),
             },
            new WebModuleMeta()
            {
                Label = "ERP子库",
                EntityType = typeof(ErpWarehouse),
            },
             new WebModuleMeta()
             {
                 Label = "LED屏幕基础数据",
                 EntityType = typeof(LED),
             },
             new WebModuleMeta()
             {
                 Label = "LED屏幕显示的风格样式",
                 EntityType = typeof(LEDShowStyle),
             }
             );
        }
    }
}
