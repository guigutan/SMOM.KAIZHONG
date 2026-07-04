using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.Resources;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// ProductRoutingDesign.xaml 的交互逻辑
    /// </summary>
    public partial class ProductRoutingDesign : UserControl
    {
        #region 按钮

        /// <summary>
        /// 保存命令
        /// </summary>
        public static RoutedUICommand Save = new RoutedUICommand();

        /// <summary>
        /// 暂停命令
        /// </summary>
        public static RoutedUICommand Pause = new RoutedUICommand();

        /// <summary>
        /// 启用命令
        /// </summary>
        public static RoutedUICommand Enable = new RoutedUICommand();

        /// <summary>
        /// 设为当前工序命令
        /// </summary>
        public static RoutedUICommand SetCurrent = new RoutedUICommand();

        /// <summary>
        /// 水平居中命令
        /// </summary>
        public static RoutedUICommand HorizontalCenter = new RoutedUICommand();

        /// <summary>
        /// 垂直居中命令
        /// </summary>
        public static RoutedUICommand VerticalCenter = new RoutedUICommand();

        /// <summary>
        /// 水平分布命令
        /// </summary>
        public static RoutedUICommand HorizontalDistribution = new RoutedUICommand();

        /// <summary>
        /// 垂直分布命令
        /// </summary>
        public static RoutedUICommand VerticalDistribution = new RoutedUICommand();

        #endregion

        /// <summary>
        /// 产品工艺路线数据
        /// </summary>
        private ProductRoutingViewModel model = null;

        /// <summary>
        /// 是否工单工艺路线的Layout
        /// </summary>
        private bool _isWorkOrderLayout = true;

        /// <summary>
        /// 根据选中节点的变更触发该事件
        /// </summary>
        public event Action<IActivity> SelectedActivityChanged;

        /// <summary>
        /// 工艺流程初始化
        /// </summary>
        public ProductRoutingDesign()
        {
            InitializeComponent();
            container.ModelChanged += Container_ModelChanged;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="model">产品工艺路线ViewModel</param>
        public void InitModel(ProductRoutingViewModel model)
        {
            this.model = model;
            InitLayout();
        }

        /// <summary>
        /// 初始化布局
        /// </summary>
        private void InitLayout()
        {
            container.LoadFromXmlString(GetLayout());
            if (this.model.Version == null)
            {
                foreach (FrameworkElement child in container.Children)
                    child.ContextMenu = null;
                return;
            }

            if (this.model.Version.IsPause == YesNo.No)
                foreach (FrameworkElement child in container.Children)
                    child.ContextMenu = null;

            var product = RT.Service.Resolve<RuntimeController>().FindProduct(this.model.Version.Product.Puid);
            this.model._product = product;

            ////如果已完工下线，那就直接全部活动设置为已过站，否则按照过站记录查找
            if (this.model.Version.IsFinish)
                container.Model.Activitys.Where(p => p.Type == ActivityType.Interaction)
                    .ForEach(p => p.ProcessState = ProcessState.Has);
            else
                UpdateActivityProcessState(product);
        }

        /// <summary>
        /// 更新活动的工序状态
        /// </summary>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        private void UpdateActivityProcessState(product product)
        {
            if (model.Version.IsPause == YesNo.No)
            {
                var currentActivity = container.Model.Activitys.FirstOrDefault(p => product.Routing.Current != null && p.ProcessId == product.Routing.Current.ProcessId);
                if (currentActivity != null)
                {
                    container.Model.Activitys.Where(p => p.ProcessState == ProcessState.Current)
                        .ForEach(p => p.ProcessState = ProcessState.Not);
                    currentActivity.ProcessState = ProcessState.Current;
                }
            }

            foreach (var process in model.Version.ProcessList)
            {
                var activity = container.Model.Activitys.FirstOrDefault(p => p.ProcessId == process.ProcessId);
                if (activity != null && activity.ProcessState != ProcessState.Current)
                    activity.ProcessState = ProcessState.Has;
            }

            if (!container.Model.Activitys.Any(p => p.ProcessState == ProcessState.Current))
                container.Model.Activitys.FirstOrDefault(p => p.Type == ActivityType.Initial).ProcessState = ProcessState.Current;
        }

        /// <summary>
        /// 获取工艺路线布局,如果产品工艺路线布局不为空则取产品工艺路线，否则取工单对应的工艺路线
        /// </summary>
        /// <returns>工艺路线布局</returns>
        private string GetLayout()
        {
            _isWorkOrderLayout = true;
            if (model.Barcode == null)
                return string.Empty;
            var wipProductRouting = RT.Service.Resolve<WipProductRoutingController>().GetWipProductRouting((model.Version?.Id).ConvertTo<double>(0));
            if (wipProductRouting != null && wipProductRouting.Layout != null)
            {
                _isWorkOrderLayout = false;
                return wipProductRouting.Layout.Layout;
            }
            return model.WorkOrder?.Layout?.Layout;
        }

        /// <summary>
        /// 选中项变更事件
        /// </summary>
        /// <param name="obj">容器对象</param>
        private void Container_ModelChanged(IContainer obj)
        {
            if (obj != null)
            {
                obj.SelectedElementChanged += Model_SelectedElementChanged;
                var activitys = obj.Activitys as ObservableCollection<IActivity>;
                if (activitys != null)
                    activitys.CollectionChanged += Activitys_CollectionChanged;
                SetToolTip(activitys);
            }
        }

        /// <summary>
        /// 活动集合变更触发
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Activitys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // 当增加活动节点时, 绑定新增活动节点的提示
                foreach (var newItem in e.NewItems)
                {
                    if (newItem is IActivity)
                    {
                        var activity = newItem as IActivity;
                        activity.ToolTip = GetToolTip(activity, model.Version?.ProcessList?.FirstOrDefault(p => p.ProcessId == activity.ProcessId));
                    }
                }
            }
        }

        /// <summary>
        /// 设置ToolTip
        /// </summary>
        /// <param name="activities">活动接口</param>
        void SetToolTip(ObservableCollection<IActivity> activities)
        {
            activities.ForEach(activity => activity.ToolTip = GetToolTip(activity, model.Version?.ProcessList?.FirstOrDefault(p => p.ProcessId == activity.ProcessId)));
        }

        /// <summary>
        /// 获取工序的活动的 ToolTip 提示信息
        /// </summary>
        /// <param name="activity">活动接口</param>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <returns>ToolTip 提示信息</returns>
        private string GetToolTip(IActivity activity, WipProductProcess wipProductProcess)
        {
            if (activity.Type != ActivityType.Interaction)
                return null;
            var createBy = RF.GetById<Employee>(wipProductProcess?.CreateBy);
            string createName = wipProductProcess == null ? string.Empty : createBy.Name;
            return "工序：{0}\n采集结果：{1}\n采集人：{2}\n采集时间：{3}".L10nFormat(activity.Text, wipProductProcess?.Result.ToLabel(), createName, wipProductProcess?.CreateDate);
        }

        /// <summary>
        /// 工艺流程选中节点变更后触发
        /// </summary>
        /// <param name="obj">选中的节点对象</param>
        private void Model_SelectedElementChanged(IElement obj)
        {
            container.svContainer.Focus();
            activityProperty.InitActivityProperty(obj as IActivity, null);
            ruleProperty.InitRuleProperty(obj as IRule, null);
            containerProperty.InitContainerProperty(obj as IContainer, null);
            SelectedActivityChanged(obj as IActivity);
        }

        /// <summary>
        /// 工艺流程面板大小改变触发事件
        /// </summary>
        /// <param name="sender">面板对象</param>
        /// <param name="e">大小更改路由参数</param>
        private void DockPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DockPanel stackPanel = sender as DockPanel;
            container.svContainer.Width = stackPanel.ActualWidth;
            ////出现负数，临时处理
            container.svContainer.Height = Math.Abs(stackPanel.ActualHeight - 40);
        }

        #region 命令处理事件

        /// <summary>
        /// 设置当前工序是否可执行（暂停并且选中节点不为终止节点才可以设置当前工序）
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void SetCurrent_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.Version != null && container.Model.SelectElement != null &&
                container.Model.SelectElement is IActivity && model.Version.IsPause == YesNo.Yes)
            {
                var activity = container.Model.SelectElement as IActivity;
                e.CanExecute = activity.Type != ActivityType.Completion && activity.ProcessState != ProcessState.Current;
            }
        }

        /// <summary>
        /// 设置当前工序执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void SetCurrent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //1、修改当前工序为已过工序；2、将选中工序设置为当前工序
            foreach (var activity in container.Model.Activitys.Where(p => p.ProcessState == ProcessState.Current))
            {
                if (model.Version.ProcessList.Any(p => p.ProcessId == activity.ProcessId))
                    activity.ProcessState = ProcessState.Has;
                else
                    activity.ProcessState = ProcessState.Not;
            }

            var selectElement = container.Model.SelectElement as IActivity;
            selectElement.ProcessState = ProcessState.Current;
            CRT.MessageService.ShowMessage("已设置 {0} 为当前工序".L10nFormat(selectElement.Text), "提示".L10N());
        }

        /// <summary>
        /// 启用是否可执行(暂停了才能启用)
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void Enable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.Version != null)
                e.CanExecute = model.Version.IsPause == YesNo.Yes;
        }

        /// <summary>
        /// 启用执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void Enable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (model.Version == null)
                    throw new Exception("产品：{0} 还未上线".L10nFormat(model.Barcode?.Sn));
                model.UpdateProcessBomToModel();
                RT.Service.Resolve<WipProductRoutingController>().EnableProductRouting(model.Version.Id, GetLayout(), container.Model.Serialize());
                model.Version = RF.GetById<WipProductVersion>(model.Version.Id);
                CRT.MessageService.ShowMessage("产品：{0} 工艺路线启用成功".L10nFormat(model.Barcode?.Sn));
                container.Model.SelectElement = null;
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                model.RefreshEvent();
                InitLayout();
            }
        }

        /// <summary>
        /// 暂停是否可执行（在不暂停情况下可以允许暂停）
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.Version != null)
                e.CanExecute = !model.Version.IsFinish && model.Version.IsPause == YesNo.No;
        }

        /// <summary>
        /// 暂停执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                bool result = CRT.MessageService.AskQuestion("产品：{0} 暂停，会影响产品继续生产过站，是否暂停？".L10nFormat(model.Barcode?.Sn));
                if (result)
                {
                    model.UpdateProcessBomToModel();
                    RT.Service.Resolve<WipProductRoutingController>().PauseProductRouting(model.Version.Id, GetLayout(), container.Model.Serialize());
                    model.Version = RF.GetById<WipProductVersion>(model.Version.Id);
                    container.Model.SelectElement = null;
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                model.RefreshEvent();
                InitLayout();
            }
        }

        /// <summary>
        /// 判断工艺路线的Layout是否修改
        /// </summary>
        /// <param name="oldLayout">原Layout</param>
        /// <param name="newLayout">新Layout</param>
        /// <returns>false 未修改；true:已经修改 </returns>
        private bool CheckLayoutChanged(string oldLayout, string newLayout)
        {
            var rtnFlag = true;
            if (oldLayout.Equals(newLayout))
                rtnFlag = false;
            return rtnFlag;
        }

        /// <summary>
        /// 保存是否可执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.Version != null)
                e.CanExecute = model.Version.IsPause == YesNo.Yes;
            //if (model != null && model.Version != null)
            //{
            //    var changedFlag = CheckLayoutChanged(GetLayout(), container.Model.Serialize());
            //    e.CanExecute = model.Version.IsPause == YesNo.Yes && changedFlag;
            //}
        }

        /// <summary>
        /// 保存执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (model.Version == null)
                    throw new Exception("产品：{0} 还未上线".L10nFormat(model.Barcode?.Sn));
                model.UpdateProcessBomToModel();
                RT.Service.Resolve<WipProductRoutingController>().SaveProductRouting(model.Version.Id, GetLayout(), container.Model.Serialize());
                CRT.MessageService.ShowMessage("保存成功".L10N());
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                model.RefreshEvent();
            }
        }

        /// <summary>
        /// 水平居中执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void HorizontalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (container == null) return;

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() > 1)
                e.CanExecute = true;
        }

        /// <summary>
        /// 水平居中逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void HorizontalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalCenter();
        }

        /// <summary>
        /// 垂直居中是否可执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void VerticalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (container == null) return;

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() > 1)
                e.CanExecute = true;
        }

        /// <summary>
        /// 垂直居中执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void VerticalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalCenter();
        }

        /// <summary>
        /// 水平分布是否可执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void HorizontalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (container == null) return;

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() >= 3)
                e.CanExecute = true;
        }

        /// <summary>
        /// 水平分布执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void HorizontalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalDistribution();
        }

        /// <summary>
        /// 垂直分布是否可执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void VerticalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (container == null) return;

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() >= 3)
                e.CanExecute = true;
        }

        /// <summary>
        /// 垂直分布执行逻辑
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">执行路由参数</param>
        private void VerticalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalDistribution();
        }

        #endregion
    }
}
