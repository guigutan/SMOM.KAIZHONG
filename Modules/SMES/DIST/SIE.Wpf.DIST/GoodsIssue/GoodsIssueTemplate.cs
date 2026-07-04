using SIE.DIST;
using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.MetaModel.View;
using SIE.Wpf.Common.Configs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 载具关联模板
    /// </summary>
    [RootEntity, Serializable]
    public class GoodsIssueTemplate : DetailsUITemplate
    {
        /// <summary>
        /// 工单发料信息
        /// </summary>
        GoodsIssue _goodsIssue = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsIssue">工单发料信息</param>
        public GoodsIssueTemplate(GoodsIssue goodsIssue)
          : base(typeof(GoodsIssueViewModel))
        {
            this._goodsIssue = goodsIssue;
        }

        /// <summary>
        /// 块定义，重新布局控件
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(GoodsIssueViewModelLayout));
            return blocks;
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new GoodsIssueViewModel() { GoodsIssue = _goodsIssue };
            var layout = ui.Control as GoodsIssueViewModelLayout;
            var tabs = new TabControl();
            tabs.Margin = new Thickness(5);
            tabs.Items.Add(new TabItem { Header = "扫描明细", Content = CreateDetailListControl(ui.MainView, model.BillList) });
            tabs.Items.Add(new TabItem { Header = "发料属性", Content = CreatePropertyListControl(ui.MainView, _goodsIssue.PropertyValueList) });
            layout.Children.Add(tabs);
            model.Restart();
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建配送明细控件
        /// </summary>
        /// <param name="mainView">主逻辑视图</param>
        /// <param name="data">配送明细</param>
        /// <returns>控件</returns>
        FrameworkElement CreateDetailListControl(LogicalView mainView, EntityList data)
        {
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, DistributionBillViewConfig.Distribution, ModuleKey);
            uiTemplate.BlocksDefined += UiTemplate_BlocksDefined;
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;
            ////移除配置项命令
            var command = ui.MainView.Commands.Find(typeof(ModuleConfigCommand));
            if (command != null)
                command.IsVisible = false;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui.Control;
        }

        /// <summary>
        /// 配送明细块定义，移除查询面板
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void UiTemplate_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            var queryBlock = e.Blocks.Surrounders.FirstOrDefault(p => p.MainBlock.EntityType == typeof(SIE.DIST.Distribution.DistributionBillCriteria));
            if (queryBlock != null)
                e.Blocks.Surrounders.Remove(queryBlock);
        }

        /// <summary>
        /// 创建属性控件
        /// </summary>
        /// <param name="mainView">主逻辑视图</param>
        /// <param name="data">属性列表</param>
        /// <returns>控件</returns>
        FrameworkElement CreatePropertyListControl(LogicalView mainView, EntityList data)
        {
            UITemplate uiTemplate = new ListUITemplate(typeof(PropertyValueViewModel), GoodsIssuePropertyValueExtViewConfig.VehicleAssociatedViewGroup, ModuleKey);
            var ui = uiTemplate.CreateUI();
            var goodsIssueValues = data as EntityList<GoodsIssuePropertyValue>;
            var result = goodsIssueValues.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.GoodsIssue).FirstOrDefault().GetType(), ParentId = f.Select(p => p.GoodsIssueId).FirstOrDefault() });
            EntityList<PropertyValueViewModel> list = new EntityList<PropertyValueViewModel>();
            list.AddRange(result);
            ui.MainView.Data = list;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui.Control;
        }
    }

    /// <summary>
    /// 载具关联布局控件
    /// </summary>
    public class GoodsIssueViewModelLayout : DockPanel, ILayoutControl
    {
        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">组件</param>
        public void Arrange(UIComponents components)
        {
            var toolBar = components.CommandsContainer;
            if (toolBar != null)
            {
                toolBar.Control.Margin = new Thickness(5, 5, -5, 5);
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }

            var control = components.Main;
            if (control != null)
            {
                SetDock(control.Control, Dock.Top);
                Children.Add(control.Control);
            }
        }
    }
}