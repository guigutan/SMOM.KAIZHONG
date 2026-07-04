using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.MES.WIP.TaskExtensions;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.MES.Controls.Messager;
using SIE.Wpf.MES.WIP.Inspects;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP
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
            var model = ui.MainView.Data as WorkCellViewModel;
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
        /// 获取当前模板的结构定义。
        /// 结构定义包括：块间的结构、布局、块对应的视图的扩展名。
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockLayout));
            return blocks;
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
            mainView.Relations.Add(new RelationView(ui.MainView.EntityType.FullName, ui.MainView));
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

        /// <summary>
        /// 创建操作控件
        /// </summary>
        /// <param name="workstation">工作站对象</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateOperationControl(Workstation workstation)
        {
            var view = AutoUI.ViewFactory.CreateDetailView(typeof(Workstation));
            view.Data = workstation;
            view.Current = workstation;
            //view.Control.Margin = new Thickness(5, -10, 5, -10);
            DockPanel.SetDock(view.Control, Dock.Bottom); //将工作站信息置底部
            return view.Control;
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
        /// 
        /// </summary>
        /// <param name="tabs"></param>
        /// <param name="mainView"></param>
        /// <param name="model"></param>
        /// <param name="viewGroup"></param>
        protected virtual void CreateReportTaskControl(DXTabControl tabs, LogicalView mainView, DataCollectionViewModel model, string viewGroup = null)
        {
            if (RT.Service.Resolve<IWipTaskReport>().IsTaskWip())//任务单生产模式下才显示任务列表
                tabs.Items.Add(CreateTabItem("任务列表", CreateDetailListControl(mainView, model.TaskList, viewGroup)));
        }

        /// <summary>
        /// 创建历史消息控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        public virtual FrameworkElement CreateMessagerControl(DataCollectionViewModel model)
        {
            var ctl = new MessagerControl();
            ctl.Margin = new Thickness(-8);
            model.MessagerControl = ctl;
            return ctl;
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
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }
            var control = components.Main.Control as DevExpress.Xpf.LayoutControl.LayoutControl;
            if (control != null)
            {
                WipLayoutHelper.SetPanelMargin(control);
                //control.Margin = new Thickness(-5, -18, -5, 0);
                SetDock(control, Dock.Top);
                Children.Add(control);
            }
        }
    }
}