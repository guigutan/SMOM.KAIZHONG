using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using SIE.Wpf.MES.Controls.Messager;
using System;
using System.Windows;

namespace SIE.Wpf.MES.WIP.PackRecombine
{
    /// <summary>
    /// 包装拆合模板基类
    /// </summary>
    public class PackRecombineBaseUITemplate : DetailsUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackRecombineBaseUITemplate()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public PackRecombineBaseUITemplate(Type entityType) : base(entityType)
        {
            ViewGroup = ViewConfig.DetailsView;
        }

        /// <summary>
        /// 创建消息控件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual FrameworkElement CreateMessagerControl(BatchPackRecombineViewModel model)
        {
            var ctl = new MessagerControl();
            ctl.Margin = new Thickness(-8);
            model.MessagerControl = ctl;
            return ctl;
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            base.OnUIGenerated(ui);
            var model = ui.MainView.Data as PackRecombineBaseViewModel;
            bool isFirst = true;
            ui.MainView.Control.Loaded += (o, e) =>
            {
                if (isFirst)
                {
                    model?.Onload();
                    isFirst = false;
                }
                else
                    model.FocuseBarcode();
            };
            ui.MainView.Closed += (s, e) =>
            {
                model?.OnClose();
            };
            MarkSaveModel(ui);
        }

        /// <summary>
        /// 创建标签页
        /// </summary>
        /// <param name="header">表头</param>
        /// <param name="control">子页签控件</param>
        /// <returns>标签页</returns>
        protected DXTabItem CreateTabItem(string header, FrameworkElement control)
        {
            var item = new DXTabItem() { Content = control };
            item.SetResourceBinding(DXTabItem.HeaderProperty, header);
            return item;
        }

        /// <summary>
        /// 标识已保存
        /// </summary>
        /// <param name="ui">控件结果</param>
        private void MarkSaveModel(ControlResult ui)
        {
            var module = CommonModel.Modules.FindModule(ui.MainView.EntityType);
            var viewContent = CRT.Workbench.GetViewContent(module?.Key);
            if (viewContent != null)
            {
                viewContent.Closing += (s, e) =>
                {
                    ui.MainView.Data.MarkSaved();
                };
            }
        }

        /// <summary>
        /// 获取当前模板的结构定义。
        /// 结构定义包括：块间的结构、布局、块对应的视图的扩展名。
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(PackRecombineLayout));
            return blocks;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateListControl(LogicalView mainView, EntityList data, string viewGroup = null)
        {
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, viewGroup.IsNullOrEmpty() ? ViewGroup : viewGroup, mainView.ModuleKey);
            uiTemplate.BlocksDefined += (s, e) =>
            {
                e.Blocks.Surrounders.Clear();
            };
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui.Control;
        }
    }
}