using SIE.Domain;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.Tech.Routings
{
    /// <summary>
    /// RoutingDegisn.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingDesign : UserControl
    {
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        RoutingVersion routingVersion;

        /// <summary>
        /// 工艺路线设计
        /// </summary>
        public RoutingDesign()
        {
            InitializeComponent();
            routingPanel.Caption = new RoutingPath { Title = "工艺路线".L10N() };
            routingTree.AddFlow_Click += container.AddFlow_Click;
            routingTree.EditFlow_Click += container.EditFlow_Click;
            routingTree.CopyRoutingVersion_Click += container.CopyRoutingVersion_Click;
            routingTree.PasteRoutingVersion_Click += container.PasteRoutingVersion_Click;
            ////工艺路线Tree版本选择变更事件，清除已选择BOM
            routingTree.GetLogicalChild<DevExpress.Xpf.Grid.GridControl>().PreviewMouseDoubleClick += RoutingDesign_PreviewMouseDoubleClick;
            container.ModelChanged += Container_ModelChanged;
            container.CopyViewModel = routingTree.CopyViewModel;
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void RoutingDesign_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            processBomControl.Children.Clear();
        }

        /// <summary>
        /// 设计器模型变更事件
        /// </summary>
        /// <param name="obj">容器接口</param>
        private void Container_ModelChanged(IContainer obj)
        {
            if (routingVersion == null || routingVersion.Id != container.Model.RoutingVersionId)
            {
                routingVersion = RF.Find<RoutingVersion>().GetById(container.Model.RoutingVersionId) as RoutingVersion;
                container.IsReadOnly = routingVersion?.State == RoutingState.Release;
                var caption = new RoutingPath
                {
                    Title = "工艺路线".L10N()
                };

                //工艺模型变更时，加载路径
                if (routingVersion != null)
                {
                    var nodeNames = routingTree.SetCurrentInfo(routingVersion.Id, nameof(RoutingVersion));
                    nodeNames.Reverse();
                    caption.VersionPath = string.Join("/", nodeNames);
                }

                routingPanel.Caption = caption;
            }

            if (obj != null)
            {
                obj.SelectedElementChanged += Model_SelectedElementChanged;
            }
        }

        /// <summary>
        /// 流程元素选中事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Model_SelectedElementChanged(IElement obj)
        {
            container.svContainer.Focus();
            processBomControl.InitProcessBom(obj as IActivity, routingVersion?.State);
            activityProperty.InitActivityProperty(obj as IActivity, routingVersion?.State);
            ruleProperty.InitRuleProperty(obj as IRule, routingVersion?.State);
            containerProperty.InitContainerProperty(obj as IContainer, routingVersion?.State);
        }

        /// <summary>
        /// 保存流程命令
        /// </summary>
        public static RoutedUICommand Save { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 发布流程命令
        /// </summary>
        public static RoutedUICommand Publish { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 水平居中命令
        /// </summary>
        public static RoutedUICommand HorizontalCenter { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 垂直居中命令
        /// </summary>
        public static RoutedUICommand VerticalCenter { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 横向分布命令
        /// </summary>
        public static RoutedUICommand HorizontalDistribution { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 纵向分布命令
        /// </summary>
        public static RoutedUICommand VerticalDistribution { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 控制保存命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null || container.Model.RoutingId <= 0)
            {
                return;
            }

            if (container.Model != null && container.Model.RoutingId > 0)
            {
                e.CanExecute = true;
            }

            if (routingVersion != null && routingVersion?.State == RoutingState.Release)
            {
                e.CanExecute = false;
            }
        }

        /// <summary>
        /// 保存流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (container.Model.RoutingId > 0)
                {
                    if (container.Model.RoutingVersionId > 0)
                    {
                        var version = RF.Find<RoutingVersion>().GetById(container.Model.RoutingVersionId) as RoutingVersion;
                        if (version != null)
                        {
                            version.Layout.Layout = container.Model.Serialize();
                            RF.Save(version.Layout);
                        }
                    }
                    else
                    {
                        var routLayoutMsg = new RoutingLayoutMsg()
                        {
                            RoutingId = container.Model.RoutingId,
                            Layout = container.Model.Serialize()
                        };
                        var version = RT.Service.Resolve<RoutingController>().CreateRoutingVersion(routLayoutMsg);
                        container.Model.RoutingVersionId = version.Id;
                    }

                    CRT.MessageService.ShowInstantMessage("保存成功".L10N(), "提示", 3);
                    Model_SelectedElementChanged(null);
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                //保存后设置工艺路线面板路径
                var nodeNames = routingTree.RefreshData(container.Model.RoutingVersionId, nameof(RoutingVersion));
                nodeNames.Reverse();
                routingPanel.Caption = new RoutingPath { Title = "工艺路线".L10N(), VersionPath = string.Join("/", nodeNames) };
                routingVersion = null;
                routingVersion = RF.Find<RoutingVersion>().GetById(container.Model.RoutingVersionId) as RoutingVersion;
                if (routingVersion != null && routingVersion.Layout != null)
                {
                    container.IsReadOnly = routingVersion?.State == RoutingState.Release;
                    container.EditFlow_Click(routingVersion.Layout.Layout, null);
                }
            }
        }

        /// <summary>
        /// 控制发布命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Publish_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null || container.Model.RoutingId <= 0 || container.Model.RoutingVersionId <= 0)
            {
                return;
            }

            if (routingVersion != null && routingVersion.State == RoutingState.Save)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 发布流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Publish_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (container.Model.RoutingId > 0 && container.Model.RoutingVersionId > 0)
                {
                    if (CRT.MessageService.AskQuestion("发布后不允许再修改，是否确定发布？".L10N(), "提示".L10N()))
                    {
                        RT.Service.Resolve<RoutingController>().ReleaseRoutingVersion(container.Model.RoutingVersionId, container.Model.Serialize());
                        CRT.MessageService.ShowMessage("发布成功！".L10N());
                    }
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                //发布后设置工艺路线面板路径
                var nodeNames = routingTree.RefreshData(container.Model.RoutingVersionId, nameof(RoutingVersion));
                nodeNames.Reverse();
                routingPanel.Caption = new RoutingPath { Title = "工艺路线".L10N(), VersionPath = string.Join("/", nodeNames) };
                routingVersion = null;
                routingVersion = RF.Find<RoutingVersion>().GetById(container.Model.RoutingVersionId) as RoutingVersion;
                if (routingVersion != null && routingVersion.Layout != null)
                {
                    container.IsReadOnly = routingVersion?.State == RoutingState.Release;
                    container.EditFlow_Click(routingVersion.Layout.Layout, null);
                }
            }
        }

        /// <summary>
        /// 控制纵向分布命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Count(p => p is IActivity) >= 3)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 纵向分布命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalDistribution();
        }

        /// <summary>
        /// 控制横向分布命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Count(p => p is IActivity) >= 3)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 横向分布命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalDistribution();
        }

        /// <summary>
        /// 控制上下居中命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Count(p => p is IActivity) > 1)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 上下居中命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalCenter();
        }

        /// <summary>
        /// 控制左右居中命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Count(p => p is IActivity) > 1)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 左右居中命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalCenter();
        }
    }

    /// <summary>
    /// 工艺路线路径
    /// </summary>
    class RoutingPath
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 版本对应路径
        /// </summary>
        public string VersionPath { get; set; }
    }
}
