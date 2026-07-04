using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.MES.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验--不良录入模板
    /// </summary>
    public class BatchDefectiveKeyInUITemplate : DetailsUITemplate
    {
        /// <summary>
        /// 批次检验不良集合
        /// </summary>
        private readonly BatchDefectiveSetViewModel _batchDefectSetVmdl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="batchDefectSetVmdl">批次检验不良集合</param>
        public BatchDefectiveKeyInUITemplate(BatchDefectiveSetViewModel batchDefectSetVmdl) : base(typeof(BatchInspectViewModel))
        {
            _batchDefectSetVmdl = batchDefectSetVmdl;
        }

        /// <summary>
        /// 获取当前模板的结构定义。
        /// 结构定义包括：块间的结构、布局、块对应的视图的扩展名。
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockPanelLayout));
            return blocks;
        }

        /// <summary>
        /// 控件创建后方法
        /// </summary>
        /// <param name="ui">生成的控件结果集合</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = this._batchDefectSetVmdl;
            var layout = ui.Control as DockPanelLayout;
            layout.Children.Clear();
            var defectControl = CreateDefectControl(model);  ////--批次检验缺陷选择
            FrameworkElement defectInfos = CreateDefectInfos(ui.MainView, _batchDefectSetVmdl.BatchDefectiveViewModels, ViewConfig.ListView); ////批次检验不良记录
            var control = new BatchDefectiveKeyInControl(defectControl, defectInfos, _batchDefectSetVmdl);
            layout.Children.Add(control);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建缺陷控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        protected virtual FrameworkElement CreateDefectControl(BatchDefectiveSetViewModel model)
        {
            var ctl = DefectControlFactory.CreateControl();
            ctl.AllowMultiple = true;
            ctl.DataContext = model;
            ctl.SetBinding(DefectControl.SelectedValueProperty, new Binding("DefectItemList"));
            ctl.SetBinding(DefectControl.DefectsProperty, new Binding("DefectList"));
            ctl.Margin = new Thickness(-8);
            return ctl;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateDefectInfos(LogicalView mainView, EntityList data, string viewGroup = null)
        {
            var defectInfoCtl = CreateDetailListControl(mainView, data, viewGroup);
            return defectInfoCtl;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateDetailListControl(LogicalView mainView, EntityList data, string viewGroup = null)
        {
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, viewGroup.IsNullOrEmpty() ? ViewGroup : viewGroup, mainView.ModuleKey);
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui.Control;
        }
    }

    /// <summary>
    /// DockLayout布局控件
    /// </summary>
    public class DockPanelLayout : DockPanel, ILayoutControl  //Grid
    {
        /// <summary>
        /// 重置布局
        /// </summary>
        /// <param name="components">控件容器</param>
        public void Arrange(UIComponents components)
        {
            var toolBar = components.CommandsContainer;
            if (toolBar != null)
            {
                toolBar.Control.Margin = new Thickness(5);
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }

            var control = components.Main;
            if (control != null)
            {
                control.Control.Margin = new Thickness(-5, -18, -5, 0);
                SetDock(control.Control, Dock.Top);
                Children.Add(control.Control);
            }
        }
    }

    /// <summary>
    /// Grid布局
    /// </summary>
    public class GridLayout : Grid, ILayoutControl
    {
        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5, GridUnitType.Star) });
            var toolBar = components.CommandsContainer;
            if (toolBar != null)
            {
                SetRow(toolBar.Control, 0);
                Children.Add(toolBar.Control);
            }

            var control = components.Main;
            if (control != null)
            {
                SetRow(control.Control, 1);
                Children.Add(control.Control);
            }
        }
    }
}