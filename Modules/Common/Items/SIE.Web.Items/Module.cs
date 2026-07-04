using SIE.Items;
using SIE.Items.Items;
using SIE.Items.KzItemCategorys;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using System;

[assembly: Module(typeof(SIE.Web.Items.Module))]

namespace SIE.Web.Items
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
            Configs.Page82Config.Add(typeof(SIE.Items.Item).FullName, Page82.Page8);
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
                Label = "单位",
                EntityType = typeof(Unit)
            }, new WebModuleMeta
            {
                Label = "分类",
                EntityType = typeof(ItemCategory)
            }, new WebModuleMeta()
            {
                Label = "物料属性定义",
                EntityType = typeof(ItemPropertyDefinition)
            }, new WebModuleMeta
            {
                Label = "产品族",
                EntityType = typeof(ProductFamily)
            }, new WebModuleMeta
            {
                Label = "产品机型",
                EntityType = typeof(ProductModel)
            }, new WebModuleMeta()
            {
                Label = "物料",
                EntityType = typeof(Item)
            }, new WebModuleMeta
            {
                Label = "产品BOM",
                EntityType = typeof(ProductBom)
            }, new WebModuleMeta
            {
                Label = "单位转换",
                EntityType = typeof(UnitConvert)
            }, new WebModuleMeta
            {
                EntityType = typeof(KzItemCategory),
                Label = "产品工艺属性维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(KzCategory),
                Label = "工艺属性分类"
            }, new WebModuleMeta()
            {
                EntityType = typeof(UnValidFactoryItem),
                Label = "不校验工厂物料清单"
            });
        }
    }
}
