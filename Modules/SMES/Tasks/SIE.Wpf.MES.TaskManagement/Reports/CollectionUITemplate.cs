using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using SIE.Domain;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 采集UI模板
    /// </summary>
    public class CollectionUITemplate : DetailsUITemplate
    {
        /// <summary>
        /// 采集视图VIEWGROUP
        /// </summary>
        public static readonly string CollectionUIViewGroup = "CollectionView";

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            base.OnUIGenerated(ui);
            var model = ui.MainView.Data as TaskReportViewModel;
            bool isFirst = true;
            ui.MainView.Control.Loaded += (o, e) =>
              {
                  if (isFirst)
                  {
                      model?.Onload();
                      isFirst = false;
                  }
                  else
                  {
                      model.FocuseBarcode();
                  }
              };

            ui.MainView.Closed += (s, e) =>
            {
                model?.OnClose();
            };

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
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public CollectionUITemplate(Type entityType) : base(entityType)
        {
            ViewGroup = CollectionUIViewGroup;
        }

        /// <summary>
        /// 创建标签页
        /// </summary>
        /// <param name="header">标头</param>
        /// <param name="control">内容</param>
        /// <returns>标签页</returns>
        protected virtual DXTabItem CreateTabItem(string header, FrameworkElement control)
        {
            var item = new DXTabItem() { Content = control };
            item.SetResourceBinding(DXTabItem.HeaderProperty, header);
            return item;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <param name="canEdit">是否默认可编辑</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateDetailListControl(LogicalView mainView, EntityList data, string viewGroup = null, bool canEdit = false)
        {
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, viewGroup.IsNullOrEmpty() ? ViewGroup : viewGroup, mainView.ModuleKey);
            uiTemplate.BlocksDefined += Template_BlocksDefined;
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;

            //是否可编辑
            if (canEdit)
            {
                ui.MainView.IsReadOnly = MetaModel.ReadOnlyStatus.None;
            }

            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui.Control;
        }

        /// <summary>
        /// 块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        protected virtual void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
        }

    }

    /// <summary>
    /// DockLayout布局控件
    /// </summary>
    public class DockLayout : DockPanel, ILayoutControl
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
                toolBar.Control.Margin = new Thickness(5, 5, 5, 0);
                SetDock(toolBar.Control, System.Windows.Controls.Dock.Top);
                Children.Add(toolBar.Control);
            }
            var control = components.Main.Control as DevExpress.Xpf.LayoutControl.LayoutControl;
            if (control != null)
            {
                SetPanelMargin(control);
                SetDock(control, System.Windows.Controls.Dock.Top);
                Children.Add(control);
            }
        }

        /// <summary>
        /// 设置容器边距
        /// </summary>
        /// <param name="control">控件</param>
        void SetPanelMargin(LayoutControl control)
        {
            if (control == null)
            {
                return;
            }
            var groups = control.Children.OfType<FrameworkElement>().Where(p => p.GetType() == typeof(LayoutGroup)).ToList();
            for (int i = 0; i < groups.Count; i++)
            {
                if (i == 0)
                {
                    continue; //第一个组为提示信息框，不要设置边距
                }
                var formPanel = groups[i].GetLogicalChild<FormPanel>();
                if (formPanel != null)
                {
                    formPanel.Margin = new Thickness(0, -5, 0, -5);
                }
            }
        }
    }


}