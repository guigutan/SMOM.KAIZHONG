using SIE.Domain;
using SIE.Wpf.MES.WIP;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 批次采集模板
    /// </summary>
    public class BatchCollectionUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public BatchCollectionUITemplate(Type entityType) : base(entityType)
        {
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="input">入站批次列表</param>
        /// <param name="output">出站批次列表</param>
        /// <param name="inputViewGroup">入站视图组名称</param>
        /// <param name="outputViewGroup">出站视图组名称</param>
        /// <returns>返回UI</returns>
        protected virtual FrameworkElement CreateBatchListControl(LogicalView mainView, EntityList input, EntityList output, string inputViewGroup = null, string outputViewGroup = null)
        {
            var inputUI = CreateListControl(mainView, input, inputViewGroup);
            var outputUI = CreateListControl(mainView, output, outputViewGroup);
            return new BatchControl(inputUI, outputUI);
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <returns>返回UI</returns>
        protected ControlResult CreateListControl(LogicalView mainView, EntityList data, string viewGroup = null)
        {
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, viewGroup.IsNullOrEmpty() ? ViewGroup : viewGroup, mainView.ModuleKey);
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            //ui.MainView.CommandsContainer.ItemsPanel = CreateTemplate();
            ui.Control.MouseRightButtonDown += (sender, e) => e.Handled = true;
            ui.Control.MouseRightButtonUp += (sender, e) => e.Handled = true;
            return ui;
        }

        /// <summary>
        /// 创建ItemsControl布局模板
        /// </summary>
        /// <returns>布局模板</returns>
        ItemsPanelTemplate CreateTemplate()
        {
            const string template = @"<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""><DockPanel HorizontalAlignment=""Right"" LastChildFill=""False""/></ItemsPanelTemplate> ";
#pragma warning disable S4055 // Literals should not be passed as localized parameters
            return (ItemsPanelTemplate)XamlReader.Parse(template);
#pragma warning restore S4055 // Literals should not be passed as localized parameters
        }
    }
}