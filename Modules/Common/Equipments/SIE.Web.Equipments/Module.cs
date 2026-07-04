using SIE.Equipments.Abnormal;
using SIE.Equipments.DeviceControls;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipFaults;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipStatus;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.FinancialCategorys;
using SIE.MetaModel;
using SIE.Modules;
using System;
[assembly: Module(typeof(SIE.Web.Equipments.Module))]

namespace SIE.Web.Equipments
{
    /// <summary>
    /// 视图插件
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 重写插件初始化方法
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }
        /// <summary>
        /// 程序模块初始化
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(EquipType),
                Label = "设备类型维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipModel),
                Label = "设备型号维护",
                UIGenerator = "SIE.Web.Equipments.EquipModels.Layouts.EquipModelUIGenerator"
            }, new WebModuleMeta
            {
                Label = "设备控制记录",
                EntityType = typeof(DeviceControl)
            }, new WebModuleMeta
            {
                Label = "设备控制来源清单",
                EntityType = typeof(SourceControl)
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipFaultAndDefect),
                Label = "检测缺陷对照关系",
            },
            new WebModuleMeta()
            {
                EntityType = typeof(SIE.Equipments.EquipAccountCarrieds.EquipAccountCarried),
                Label = "设备载位信息",
            }, new WebModuleMeta()
            {
                Label = "停线信息管理",
                EntityType = typeof(AbnormalCause)
            }, new WebModuleMeta()
            {
                EntityType = typeof(DeviceIOTPara),
                Label = "设备物联参数"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipAccount),
                Label = "设备台账维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipmentCard),
                Label = "设备立卡"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FinancialCategory),
                Label = "财务分类"
            },new WebModuleMeta()
            { 
                EntityType = typeof(SIE.Equipments.EquipStatus.EquipStatus),
                Label = "设备状态"
            });
        }
    }
}
