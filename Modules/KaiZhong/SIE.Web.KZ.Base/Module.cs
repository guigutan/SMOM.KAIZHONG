using SIE.Web.KZ.Base;
using SIE.KZ.Base.SmomControl;
using SIE.MetaModel;
using SIE.Modules;
using System;
using SIE.KZ.Base.Interfaces;

[assembly: Module(typeof(Module))]
namespace SIE.Web.KZ.Base
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app"></param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            app.MetaCompiled += App_MetaCompiled;
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_MetaCompiled(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        ///模块定义操作
        /// </summary>
        /// <param name="sender">事件的源对象</param>
        /// <param name="e">The <see cref="EventArgs"/> 事件的数据.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta
            {
                Label = "SMOM总控配置",
                EntityType = typeof(SmomControlSetting)
            }, new WebModuleMeta()
            {
                Label = "总控与NC接口日志",
                EntityType = typeof(InfNcDataLogGroup)
            }, new WebModuleMeta()
            {
                Label = "总控与子工厂接口日志",
                EntityType = typeof(InfNcDataLogFactory)
            }, new WebModuleMeta()
            {
                Label = "基础数据接口信息",
                EntityType = typeof(InfMapping)
            }, new WebModuleMeta()
            {
                Label = "MOM相关接口日志",
                EntityType = typeof(InfDataLog)
            }, new WebModuleMeta()
            {
                Label = "主数据NC接口日志",
                EntityType = typeof(InfNcDataLog)
            }, new WebModuleMeta()
            {
                Label = "SAP工单接口查询与重传",
                EntityType = typeof(InfNcDataLogSO)
            }, new WebModuleMeta()
            {
                Label = "GUID与工厂关系",
                EntityType = typeof(GuidFactoryRelastion)
            }, new WebModuleMeta()
            {
                Label = "跨组织物料标签同步",
                EntityType = typeof(SyncItemLabel)
            }, new WebModuleMeta()
            {
                EntityType = typeof(MtartZtflRelation),
                Label = "是否启用制卡数量维护"
            });
        }
    }
}