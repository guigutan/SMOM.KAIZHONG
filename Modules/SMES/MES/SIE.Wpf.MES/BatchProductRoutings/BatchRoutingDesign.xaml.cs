using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// RoutingDesign.xaml 的交互逻辑
    /// </summary>
    public partial class BatchRoutingDesign : UserControl
    {
        #region 命令 
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
        private BatchRoutingViewModel model = null;

        /// <summary>
        /// 根据选中节点的变更触发该事件
        /// </summary>
        public event Action<IActivity> SelectedActivityChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchRoutingDesign()
        {
            InitializeComponent();
            container.ModelChanged += Container_ModelChanged;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="model">产品工艺路线ViewModel</param>
        public void InitModel(BatchRoutingViewModel model)
        {
            this.model = model;
            InitLayout(model);
        }

        /// <summary>
        /// 初始化布局
        /// </summary>
        /// <param name="model">工艺路线视图模型</param>
        private void InitLayout(BatchRoutingViewModel model)
        {
            if (model.BatchRelation == null || model.WorkOrder == null)
                return;
            container.LoadFromXmlString(GetLayout());
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(model.BatchRelation.Bid, BarcodeType.BatchBarocde);
            if ((this.model.BatchRelation == null || (product != null && product.Routing.Current == null))
                && (!CheckBidOnLine(model.BatchRelation.Bid)))
            {
                //(批次未上线，或者上线但未通过任何工序) && (子批次未上线 或未通过任务工序)
                ClearContextMenu();
                SetActivitysProcessState(ProcessState.Not);
                return;
            }
            if (this.model.BatchRelation.IsPause == YesNo.No)
                ClearContextMenu();

            //批次已完工或者批次完全拆分后，所有工序未已通过
            if (this.model.BatchRelation.IsFinish || this.model.BatchRelation.RemainQty == 0)
                container.Model.Activitys.Where(p => p.Type == ActivityType.Interaction)
                    .ForEach(p => p.ProcessState = ProcessState.Has);
            else
                UpdateActivityProcessState(product);
        }

        /// <summary>
        /// 判断是否有子批次号已上线
        /// </summary>
        /// <param name="pidCode">父批次号</param>
        /// <returns>true:已上线; false:未上线</returns>
        private bool CheckBidOnLine(string pidCode)
        {
            bool checkFlag = false;
            var childBatchRelations = RT.Service.Resolve<BatchManageController>().GetBatchRelations(pidCode);
            if (childBatchRelations == null || childBatchRelations.Count == 0)
                checkFlag = true;
            else
            {
                foreach (var batchRelationItem in childBatchRelations)
                {
                    var product = RT.Service.Resolve<RuntimeController>().FindProduct(batchRelationItem.Bid, BarcodeType.BatchBarocde);
                    if (product != null && product.Routing.Current != null)
                    {
                        checkFlag = true;
                        break;
                    }
                }
            }

            return checkFlag;
        }

        /// <summary>
        /// 设置工序状态
        /// </summary>
        /// <param name="processState">工序状态</param>
        void SetActivitysProcessState(ProcessState processState)
        {
            container.Model.Activitys.ForEach(e => e.ProcessState = processState);
        }

        /// <summary>
        /// 清除右键菜单
        /// </summary>
        private void ClearContextMenu()
        {
            foreach (FrameworkElement child in container.Children)
                child.ContextMenu = null;
        }

        /// <summary>
        /// 更新活动的工序状态
        /// </summary>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        private void UpdateActivityProcessState(product product)
        {
            if (model.BatchRelation.IsPause == YesNo.No)
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
                BatchWipProductProcessDetail detail;
                if (model.Batch.IsChild)
                    detail = process.DetailList.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.Out && p.PlugType == PlugType.Out && (p.SubBatchNo == model.Batch.BatchNo));
                else
                    detail = process?.DetailList?.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.Out && p.PlugType == PlugType.Out && p.BatchNo == model.Batch.BatchNo && p.SubBatchNo.IsNullOrEmpty());
                if (activity != null && detail != null && activity.ProcessState != ProcessState.Current)
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
            var wipProductRouting = RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRouting((model.BatchRelation.Id).ConvertTo<double>(0));
            if (wipProductRouting != null && wipProductRouting.Layout != null)
            {
                return wipProductRouting.Layout.Layout;
            }
            else
            {
                //拆分生成的子批工艺路线信息取获取运行时的产品工艺路线 
                var layout = RT.Service.Resolve<BatchWipProductRoutingController>().GetBatchRuntimeRoutingLayout(model.BatchRelation.Bid, model.WorkOrder.Id);
                if (layout != null)
                {
                    return layout.Layout;
                }
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
        /// 设置ToolTip
        /// </summary>
        /// <param name="activities">活动接口</param>
        void SetToolTip(ObservableCollection<IActivity> activities)
        {
            activities.ForEach(activity => activity.ToolTip = GetToolTip(activity, model.Version?.ProcessList?.FirstOrDefault(p => p.ProcessId == activity.ProcessId)));
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
        /// 获取工序的活动的 ToolTip 提示信息
        /// </summary>
        /// <param name="activity">活动接口</param>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <returns>ToolTip 提示信息</returns>
        private string GetToolTip(IActivity activity, BatchWipProductProcess wipProductProcess)
        {
            if (activity.Type != ActivityType.Interaction)
                return null;
            BatchWipProductProcessDetail inDetail;
            BatchWipProductProcessDetail outDetail;
            var batch = model.Batch;
            if (batch.IsChild)
            {
                //出站时可能发生拆分情况，所有需要批次号或者子批次号去查找入站明细
                inDetail = wipProductProcess?.DetailList?.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.In && p.PlugType == PlugType.In && (p.SubBatchNo == batch.BatchNo || p.BatchNo == model?.GetWipBatchNo()));
                outDetail = wipProductProcess?.DetailList?.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.Out && p.PlugType == PlugType.Out && (p.SubBatchNo == batch.BatchNo));
            }
            else
            {
                inDetail = wipProductProcess?.DetailList?.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.In && p.PlugType == PlugType.In && p.BatchNo == batch.BatchNo && p.SubBatchNo.IsNullOrEmpty());
                outDetail = wipProductProcess?.DetailList?.OrderByDescending(p => p.CreateDate).FirstOrDefault(p => p.BatchState == BatchState.Out && p.PlugType == PlugType.Out && p.BatchNo == batch.BatchNo && p.SubBatchNo.IsNullOrEmpty());
            }
            return "工序：{0}\n入站时间：{1}\n入站操作人：{2}\n出站时间：{3}\n出站操作人：{4}".L10nFormat(activity.Text, inDetail?.InputDate, inDetail?.OperateBy?.Name, outDetail?.OutputDate, outDetail?.OperateBy?.Name);
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
            if (model != null && model.BatchRelation != null && container.Model.SelectElement != null &&
                container.Model.SelectElement is IActivity && model.BatchRelation.IsPause == YesNo.Yes)
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
            if (model != null && model.BatchRelation != null)
                e.CanExecute = model.BatchRelation.IsPause == YesNo.Yes;
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
                if (model.BatchRelation == null)
                    throw new Exception("批次：{0} 还未上线".L10nFormat(model.Batch?.BatchNo));
                model.UpdateProcessBomToModel();
                RT.Service.Resolve<BatchWipProductRoutingController>().EnableProductRouting(model.BatchRelation.Id, GetLayout(), container.Model.Serialize());
                model.BatchRelation = RF.GetById<BatchRelation>(model.BatchRelation.Id);
                CRT.MessageService.ShowMessage("批次：{0} 工艺路线启用成功".L10nFormat(model.Batch?.BatchNo));
                container.Model.SelectElement = null;
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
            finally
            {
                model.RefreshEvent();
                InitLayout(model);
            }
        }

        /// <summary>
        /// 暂停是否可执行（在不暂停情况下可以允许暂停）
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.BatchRelation != null)
                e.CanExecute = model.BatchRelation.IsPause == YesNo.No && model.BatchRelation.IsFinish == false;
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
                bool result = CRT.MessageService.AskQuestion("批次：{0} 暂停，会影响批次产品继续生产过站，是否暂停？".L10nFormat(model.Batch.BatchNo));
                if (result)
                {
                    model.UpdateProcessBomToModel();
                    RT.Service.Resolve<BatchWipProductRoutingController>().PauseProductRouting(model.BatchRelation.Id, GetLayout(), container.Model.Serialize());
                    model.BatchRelation = RF.GetById<BatchRelation>(model.BatchRelation.Id);
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
                InitLayout(model);
            }
        }

        /// <summary>
        /// 保存是否可执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">可执行路由参数</param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model != null && model.BatchRelation != null)
            {
                e.CanExecute = model.BatchRelation.IsPause == YesNo.Yes;//&& changedFlag;
            }
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
                if (model.BatchRelation == null)
                    throw new Exception("批次：{0} 还未上线".L10nFormat(model.Batch?.BatchNo));
                model.UpdateProcessBomToModel();
                RT.Service.Resolve<BatchWipProductRoutingController>().SaveProductRouting(model.BatchRelation.Id, GetLayout(), container.Model.Serialize());
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