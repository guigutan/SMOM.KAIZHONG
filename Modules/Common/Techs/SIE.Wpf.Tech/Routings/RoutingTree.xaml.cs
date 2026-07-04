using DevExpress.Xpf.Grid;
using Resources.IconPacks;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Wpf.Tech.Helpers;
using SIE.Wpf.Tech.Processs;
using SIE.Wpf.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.Tech.Routings
{
    /// <summary>
    /// RoutingTree.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingTree : UserControl
    {
        /// <summary>
        /// 添加流程事件
        /// </summary>
        public event RoutedEventHandler AddFlow_Click;

        /// <summary>
        /// 编辑流程事件
        /// </summary>
        public event RoutedEventHandler EditFlow_Click;

        /// <summary>
        /// 复制工艺路线版本事件
        /// </summary>
        public event RoutedEventHandler CopyRoutingVersion_Click;

        /// <summary>
        /// 粘贴工艺路线版本事件
        /// </summary>
        public event RoutedEventHandler PasteRoutingVersion_Click;

        /// <summary>
        /// 树帮助类，用于控制树节点的是否展开
        /// </summary>
        TreeHelper helper = new TreeHelper();

        /// <summary>
        /// 产品族分类集合
        /// </summary>
        IEnumerable<ProductFamilyCategory> _productFamilyList = new List<ProductFamilyCategory>();

        /// <summary>
        /// 工艺路线集合
        /// </summary>
        IEnumerable<Routing> _routingList = new List<Routing>();

        /// <summary>
        /// 工艺信息树
        /// </summary>
        IList<TreeInfo> _treeInfos = new List<TreeInfo>();

        /// <summary>
        /// 快捷菜单字典
        /// </summary>
        Dictionary<string, ContextMenu> _contextMenus = new Dictionary<string, ContextMenu>();

        /// <summary>
        /// 用于构建工艺信息树
        /// </summary>
        int _id;

        /// <summary>
        /// 用于存储已展开Node
        /// </summary>
        Dictionary<string, Tuple<string, double>> keyValuePairs = new Dictionary<string, Tuple<string, double>>();

        /// <summary>
        /// 当前选择节点信息
        /// </summary>
        TreeInfo _currentInfo;

        /// <summary>
        /// 当前已加载节点信息
        /// </summary>
        TreeInfo _loadedInfo;

        /// <summary>
        /// 工艺流程版本复制选项ViewModel
        /// </summary>
        public RoutingVersionCopyViewModel CopyViewModel { get; } = new RoutingVersionCopyViewModel();

        /// <summary>
        /// 复制完成标记
        /// </summary>
        private bool IsCopyFinished { get; set; }

        /// <summary>
        /// 工艺信息树
        /// </summary>
        public RoutingTree()
        {
            InitializeComponent();
            CopyPasteRoutingVersionIni();
            RefreshData();
            ////routingControl.ContextMenuOpening += RoutingControl_ContextMenuOpening;
            routingControl.SelectedItemChanged += RoutingControl_SelectedItemChanged;
            routingControl.PreviewMouseDoubleClick += RoutingControl_PreviewMouseDoubleClick;
            routingControl.CurrentItemChanged += RoutingControl_CurrentItemChanged;
            routingControl.AllowInitiallyFocusedRow = false;

            var treeListView = (routingControl.View as TreeListView);
            treeListView.NodeExpanded += (o, e) =>
            {
                var node = e.Node.Content as TreeInfo;
                if (node != null)
                {
                    var key = node.Type + "_" + node.EntityId;
                    if (!keyValuePairs.ContainsKey(key))
                        keyValuePairs[key] = new Tuple<string, double>(node.Type, (double)node.EntityId);
                }
            };

            treeListView.NodeCollapsed += (o, e) =>
            {
                var node = e.Node.Content as TreeInfo;
                if (node != null)
                {
                    var key = node.Type + "_" + node.EntityId;
                    if (keyValuePairs.ContainsKey(key))
                        keyValuePairs.Remove(key);
                }
            };
        }

        /// <summary>
        /// 单击项变更时的事件
        /// </summary>
        /// <param name="sender">事件源对象</param>
        /// <param name="e">事件参数</param>
        private void RoutingControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = routingControl.SelectedItem as TreeInfo;
            if (selectedItem == null)
            {
                e.Handled = true;
                return;
            }

            if (selectedItem.Type == nameof(ProductFamilyCategory))
            {
                routingControl.ContextMenu = GetProductFamilyContextMenu();
            }
            else if (selectedItem.Type == nameof(Routing))
            {
                routingControl.ContextMenu = GetRoutingContextMenu();
            }
            else if (selectedItem.Type == nameof(RoutingVersion))
            {
                if ((selectedItem.Tag as RoutingVersion)?.State == RoutingState.Release)
                {
                    routingControl.ContextMenu = GetReleaseVersionContextMenu();
                }
                else
                {
                    routingControl.ContextMenu = GetVersionContextMenu();
                }
            }
        }

        /// <summary>
        /// 复制、粘贴工艺路线版本初始化
        /// </summary>
        private void CopyPasteRoutingVersionIni()
        {
            CreateCopyRVCommandBinding();
            CreatePasteRVCommandBinding();
            IsCopyFinished = false;
        }

        /// <summary>
        /// 双击工艺路线版本加载工艺路线
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void RoutingControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                return;
            }

            var selectedItem = routingControl.SelectedItem as TreeInfo;
            if (selectedItem != null && selectedItem.Type == nameof(RoutingVersion))
            {
                RoutingVersion routingVersion = selectedItem.Tag as RoutingVersion;
                EditFlow_Click?.Invoke(routingVersion?.Layout?.Layout, e);
            }

            var view = (sender as GridControl)?.View as TreeListView;
            var row = routingControl.GetSelectedRowHandles();
            if (row.Any() && view != null)
            {
                var node = view.GetNodeByRowHandle(row[0]);
                node.IsExpanded = !node.IsExpanded;
            }
        }

        /// <summary>
        /// 当前项变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void RoutingControl_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (_currentInfo != e.NewItem && e.NewItem != null)
            {
                _currentInfo = e.NewItem as TreeInfo;
            }
        }

        /// <summary>
        /// 设置当前选择和已加载节点信息
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <param name="type">实体类型</param>
        /// <returns>显示名称集合</returns>
        internal List<string> SetCurrentInfo(double entityId, string type)
        {
            var tmp = _treeInfos.FirstOrDefault(p => p.Type == type && p.EntityId == entityId);
            if (tmp != null)
            {
                _currentInfo = tmp;
                _loadedInfo = tmp.Type == nameof(RoutingVersion) ? tmp : _loadedInfo;
            }

            return GetDisplayNames(_loadedInfo);
        }

        /// <summary>
        /// 加载当前路径显示名称
        /// </summary>
        /// <param name="loadedInfo">已加载节点信息</param>
        /// <returns>各层级显示名称</returns>
        private List<string> GetDisplayNames(TreeInfo loadedInfo)
        {
            var info = loadedInfo;
            var displayNames = new List<string>();
            while (info != null)
            {
                if (info.Type == nameof(RoutingVersion))
                    displayNames.Add((info.Tag as RoutingVersion).Name);
                else
                    displayNames.Add(info.DisplayName);

                info = _treeInfos.FirstOrDefault(p => p.Id == info.Pid);
            }

            return displayNames;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <param name="type">实体类型</param>
        /// <returns>显示名称集合</returns>
        public List<string> RefreshData(double? entityId = null, string type = "")
        {
            var displayNames = new List<string>();
            _productFamilyList = RF.Find<ProductFamilyCategory>().GetAll() as IEnumerable<ProductFamilyCategory>;
            _routingList = RF.Find<Routing>().GetAll() as IEnumerable<Routing>;
            _contextMenus.Clear();
            QueryProductFamily();

            if (entityId.HasValue && type.IsNotEmpty())
                displayNames.AddRange(SetCurrentInfo(entityId.Value, type));
            helper.ExpandingNodes(routingControl, _treeInfos, keyValuePairs, _currentInfo);

            return displayNames;
        }

        /// <summary>
        /// 查询产品族大类
        /// </summary>
        void QueryProductFamily()
        {
            routingControl.ItemsSource = null;
            _treeInfos.Clear();
            _id = 0;
            var productFamilyList = _productFamilyList;
            var routingList = _routingList;
            var query = searchEdit.EditValue?.ToString();
            if (query.IsNotEmpty())
            {
                routingList = _routingList.Where(p => p.Name.Contains(query) || p.VersionList.Any(x => x.Name.Contains(query)));
                productFamilyList = _productFamilyList.Where(p => p.Name.Contains(query) || p.Code.Contains(query) || routingList.Any(r => r.CategoryId == p.Id)).ToList();
            }

            LoadProductFamilyNodes(productFamilyList, routingList);
            routingControl.ItemsSource = _treeInfos;
        }

        /// <summary>
        /// 加载产品族分类的节点
        /// </summary>
        /// <param name="productFamilyList">产品族分类集合</param>
        /// <param name="routingList">工艺路线集合</param>
        void LoadProductFamilyNodes(IEnumerable<ProductFamilyCategory> productFamilyList, IEnumerable<Routing> routingList)
        {
            foreach (var productFamily in productFamilyList.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = productFamily.Name,
                    Tag = productFamily,
                    Type = nameof(ProductFamilyCategory),
                    EntityId = productFamily.Id
                };
                _treeInfos.Add(treeInfo);
                LoadRoutingNodes(routingList.Where(p => p.CategoryId == productFamily.Id), treeInfo);
            }
        }

        /// <summary>
        /// 加载工艺路线节点
        /// </summary>
        /// <param name="routings">工艺路线集合</param>
        /// <param name="parentTreeInfo">父节点</param>
        void LoadRoutingNodes(IEnumerable<Routing> routings, TreeInfo parentTreeInfo)
        {
            foreach (var routing in routings.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = routing.Name,
                    Pid = parentTreeInfo.Id,
                    Tag = routing,
                    Type = nameof(Routing),
                    EntityId = routing.Id
                };
                _treeInfos.Add(treeInfo);
                LoadRoutingVersionNodes(routing.VersionList, treeInfo);
            }
        }

        /// <summary>
        /// 加载工艺路线版本节点
        /// </summary>
        /// <param name="versions">工艺路线版本集合</param>
        /// <param name="parentTreeInfo">父节点</param>
        void LoadRoutingVersionNodes(IEnumerable<RoutingVersion> versions, TreeInfo parentTreeInfo)
        {
            foreach (var version in versions.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = "{0}{1}({2})".FormatArgs(version.IsDefault == YesNo.Yes ? "*" : " ", version.Name, version.ReferenceQty),
                    Pid = parentTreeInfo.Id,
                    Tag = version,
                    Type = nameof(RoutingVersion),
                    EntityId = version.Id
                };
                _treeInfos.Add(treeInfo);
            }
        }

        #region 菜单操作
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="header">标题</param>
        /// <param name="tooltip">提示信心</param>
        /// <param name="dataContext">内容</param>
        /// <param name="icon">图标</param>
        /// <returns>菜单项</returns>
        MenuItem CreateMenuItem(string header, string tooltip, object dataContext, PackIconKind? icon)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.SetResourceBinding(HeaderedItemsControl.HeaderProperty, header);
            menuItem.SetResourceBinding(MenuItem.ToolTipProperty, tooltip);
            menuItem.Tag = dataContext;
            if (icon != null)
            {
                var packIcon = new PackIcon() { Kind = icon.Value };
                packIcon.Width = 12;
                menuItem.Icon = packIcon;
            }

            return menuItem;
        }

        /// <summary>
        /// 获取产品簇分类快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetProductFamilyContextMenu()
        {
            if (!_contextMenus.ContainsKey(nameof(ProductFamilyCategory)))
            {
                ContextMenu contextMenu = new ContextMenu();
                var addRounting = CreateMenuItem("新增工艺路线", "新增工艺路线", null, PackIconKind.AddEntity);
                addRounting.Click += AddRouting_Click;
                contextMenu.Items.Add(addRounting);
                CreatePasteRVMenuItem(contextMenu); ////粘贴工艺流程版本
                _contextMenus[nameof(ProductFamilyCategory)] = contextMenu;
                return contextMenu;
            }

            return _contextMenus[nameof(ProductFamilyCategory)];
        }

        /// <summary>
        /// 获取工艺路线快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetRoutingContextMenu()
        {
            if (!_contextMenus.ContainsKey(nameof(Routing)))
            {
                ContextMenu contextMenu = new ContextMenu();
                var addVersion = CreateMenuItem("新增流程", "新增流程", null, PackIconKind.AddEntity);
                addVersion.Click += AddRoutingVersion_Click;
                contextMenu.Items.Add(addVersion);
                var editVersion = CreateMenuItem("修改工艺路线", "修改工艺路线", null, PackIconKind.EditEntity);
                editVersion.Click += EditRouting_Click;
                contextMenu.Items.Add(editVersion);
                var deleteVersion = CreateMenuItem("删除工艺路线", "删除工艺路线", null, PackIconKind.DeleteEntity);
                deleteVersion.Click += DeleteRouting_Click;
                contextMenu.Items.Add(deleteVersion);
                CreatePasteRVMenuItem(contextMenu); ////粘贴工艺流程版本
                _contextMenus[nameof(Routing)] = contextMenu;
                return contextMenu;
            }

            return _contextMenus[nameof(Routing)];
        }

        /// <summary>
        /// 创建右键菜单-粘贴工艺流程版本
        /// </summary>
        /// <param name="contextMenu">菜单对象</param>
        private void CreatePasteRVMenuItem(ContextMenu contextMenu)
        {
            var pasteVersion = CreateMenuItem("粘贴版本", "粘贴版本", null, PackIconKind.ClipboardPaste);
            pasteVersion.InputGestureText = "Ctrl+V";
            pasteVersion.Command = ApplicationCommands.Paste;
            contextMenu.Items.Add(pasteVersion);
        }

        /// <summary>
        /// 创建粘贴工艺路线版本CommandBinding，绑定命令PasteRVCommand
        /// </summary>
        /// <returns>CommandBinding</returns>
        private void CreatePasteRVCommandBinding()
        {
            var pasteRVCmdBinding = new CommandBinding(ApplicationCommands.Paste);
            pasteRVCmdBinding.CanExecute += PasteRVCmdBinding_CanExecute;
            pasteRVCmdBinding.Executed += PasteRVCmdBinding_Executed;
            routingControl.CommandBindings.Add(pasteRVCmdBinding); ////注册粘贴工艺路线版本快捷键
        }

        /// <summary>
        /// pasteRVCmdBinding的是否可以执行方法
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PasteRVCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (routingControl.SelectedItem != null)
            {
                var curItem = routingControl.SelectedItem as TreeInfo;
                if (IsCopyFinished && (curItem.Type == nameof(Routing) || curItem.Type == nameof(ProductFamilyCategory)))
                    e.CanExecute = true;
            }
        }

        /// <summary>
        /// pasteRVCmdBinding的执行方法
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PasteRVCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PasteVersionEvent(sender, e);
        }

        /// <summary>
        /// 粘贴工艺路线版本到右键单击的工艺路线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PasteVersionEvent(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null)
            {
                if (!IsCopyFinished)
                    throw new ValidationException("粘贴失败，请先进行复制操作!".L10nFormat());
                if (item.Type == nameof(Routing))
                {
                    Routing routing = item.Tag as Routing;
                    PasteRoutingVersion(routing);
                }
                else if (item.Type == nameof(ProductFamilyCategory))
                {
                    var routing = CreateAddRouting(item);
                    PasteRoutingVersion(routing);
                }
            }
        }

        /// <summary>
        /// 粘贴工艺路线版本
        /// </summary>
        /// <param name="routing">工艺路线</param>
        void PasteRoutingVersion(Routing routing)
        {
            if (PasteRoutingVersion_Click != null && routing != null && routing.Id > 0)
            {
                var model = new ContainerModel();
                model.RoutingId = routing.Id;
                model.RoutingVersionId = 0;
                model.State = ElementState.New;
                PasteRoutingVersion_Click(model, null);
                CRT.MessageService.ShowInstantMessage("粘贴成功，请保存！".L10N(), "提示", 3);
            }
        }

        /// <summary>
        /// 获取工艺路线版本快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetVersionContextMenu()
        {
            if (!_contextMenus.ContainsKey(nameof(RoutingVersion)))
            {
                ContextMenu contextMenu = GetRoutingVersionCommonMenu();

                var deleteVersion = CreateMenuItem("删除流程", "删除流程", null, PackIconKind.DeleteEntity);
                deleteVersion.Click += DeleteRoutingVersion_Click;
                contextMenu.Items.Add(deleteVersion);
                _contextMenus[nameof(RoutingVersion).FormatArgs(nameof(RoutingVersion))] = contextMenu;
                return contextMenu;
            }

            return _contextMenus[nameof(RoutingVersion)];
        }

        /// <summary>
        /// 获取工艺路线发布版本快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetReleaseVersionContextMenu()
        {
            if (!_contextMenus.ContainsKey("Release{0}".FormatArgs(nameof(RoutingVersion))))
            {
                ContextMenu contextMenu = GetRoutingVersionCommonMenu();
                _contextMenus["Release{0}".FormatArgs(nameof(RoutingVersion))] = contextMenu;
                return contextMenu;
            }

            return _contextMenus["Release{0}".FormatArgs(nameof(RoutingVersion))];
        }

        /// <summary>
        /// 获取工艺路线版本通用右键菜单
        /// </summary>
        /// <returns>右键菜单</returns>
        private ContextMenu GetRoutingVersionCommonMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            var setDefault = CreateMenuItem("设置默认", "设置默认", null, null);
            setDefault.Click += SetDefault_Click;
            contextMenu.Items.Add(setDefault);
            var openVersion = CreateMenuItem("打开流程", "打开流程", null, null);
            openVersion.Click += EditRoutingVersion_Click;
            contextMenu.Items.Add(openVersion);
            CreateCopyRVMenuItem(contextMenu); ////复制工艺流程版本
            return contextMenu;
        }

        /// <summary>
        /// 创建右键菜单--复制工艺流程版本
        /// </summary>
        /// <param name="contextMenu">菜单对象</param>
        private void CreateCopyRVMenuItem(ContextMenu contextMenu)
        {
            var copyRoutingVersion = CreateMenuItem("复制版本", "复制版本", null, PackIconKind.CopyEntity);
            copyRoutingVersion.InputGestureText = "Ctrl+C";
            copyRoutingVersion.Command = ApplicationCommands.Copy;
            contextMenu.Items.Add(copyRoutingVersion);
        }

        /// <summary>
        /// 创建CommandBinding，绑定CopyRVCommand
        /// </summary>
        /// <returns>copyRVCmdBinding</returns>
        private void CreateCopyRVCommandBinding()
        {
            var copyRVCmdBinding = new CommandBinding(ApplicationCommands.Copy);
            copyRVCmdBinding.CanExecute += CopyRVCmdBinding_CanExecute;
            copyRVCmdBinding.Executed += CopyRVCmdBinding_Executed;
            routingControl.CommandBindings.Add(copyRVCmdBinding); ////注册复制工艺路线版本快捷键
        }

        /// <summary>
        /// copyRVCmdBinding的是否可以执行方法
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void CopyRVCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (routingControl.SelectedItem != null)
            {
                var curItem = routingControl.SelectedItem as TreeInfo;
                if (curItem.Type == nameof(RoutingVersion))
                    e.CanExecute = true;
            }
        }

        /// <summary>
        /// copyRVCmdBinding的执行方法
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void CopyRVCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyRoutingVersionEvent(sender, e);
        }

        /// <summary>
        /// 复制工艺路线版本
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void CopyRoutingVersionEvent(object sender, RoutedEventArgs e)
        {
            if (CopyRoutingVersion_Click != null)
            {
                try
                {
                    if (ShowCopyDialog(this.CopyViewModel) == 0)
                    {
                        var item = routingControl.SelectedItem as TreeInfo;
                        var layout = (item?.Tag as RoutingVersion)?.Layout?.Layout;
                        CheckProcessChanged(layout);
                        CopyRoutingVersion_Click(layout, e);
                        IsCopyFinished = true;
                    }
                }
                catch (Exception ex)
                {
                    CRT.MessageService.ShowMessage(ex.Message, "提示".L10N());
                }
            }
        }

        /// <summary>
        /// 工艺路线版本复制对话框
        /// </summary>
        /// <param name="copyModel">工艺路线版本复制ViewModel对象</param>
        /// <returns>-1或0</returns>
        private int ShowCopyDialog(RoutingVersionCopyViewModel copyModel)
        {
            var template = new DetailsUITemplate(typeof(RoutingVersionCopyViewModel), ViewConfig.DetailsView);
            var ui = template.CreateUI();
            ui.MainView.Data = copyModel;
            return CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = "工艺路线版本复制选项".L10N();
                w.Height = 200;
                w.Width = 100;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var broken = copyModel.Validate();
                        if (broken.Count > 0)
                        {
                            CRT.MessageService.ShowMessage(broken.ToString(), "属性错误".L10N());
                            e.Cancel = true;
                            return;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 校验工序是否发生变更
        /// </summary>
        /// <param name="xml">工艺路线版本</param>
        void CheckProcessChanged(string xml)
        {
            var containerModel = new ContainerModel();
            containerModel.Deserialize(xml);
            foreach (var activity in containerModel.Activitys.Where(p => p.ProcessId > 0))
            {
                var process = RF.GetById<Process>(activity.ProcessId);
                if (process == null)
                    throw new ValidationException("{0} 工序已删除，不能复制".L10nFormat(activity.Text));
                if (process.Name != activity.Text)
                    throw new ValidationException("{0} 工序已改名为：{1}，不能复制".L10nFormat(activity.Text, process.Name));
                var rules = containerModel.Rules.Where(p => p.SourceActivityId == activity.Id);
                foreach (var rule in rules)
                {
                    var parameter = process.ParameterList.FirstOrDefault(p => p.Id == rule.ParameterId);
                    if (parameter != null && (parameter.Type.ToLabel() == rule.Text || parameter.Description == rule.Text))
                        continue;
                    throw new ValidationException("{0} 工序参数已发生变化，不能复制".L10nFormat(process.Name));
                }
            }
        }

        #region 工艺路线新增、修改、删除
        /// <summary>
        /// 新增工艺路线
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void AddRouting_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null && item.Type == nameof(ProductFamilyCategory))
            {
                CreateAddRouting(item);
            }
        }

        /// <summary>
        /// 在当前产品族下新增工艺路线
        /// </summary>
        /// <param name="item">当前树节点</param>
        /// <returns>工艺路线</returns>
        private Routing CreateAddRouting(TreeInfo item)
        {
            ProductFamilyCategory productFamily = item.Tag as ProductFamilyCategory;
            Routing routing = new Routing() { Category = productFamily };
            AddRouting(routing);
            return routing;
        }

        /// <summary>
        /// 新增工艺路线
        /// </summary>
        /// <param name="routing">工艺路线</param>
        void AddRouting(Routing routing)
        {
            ShowRoutingDialog("新增工艺路线", routing, (e) =>
             {
                 RF.Save(routing);
                 RefreshData(routing.Id, nameof(Routing));
             });
        }

        /// <summary>
        /// 修改工艺路线
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void EditRouting_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null && item.Type == nameof(Routing))
            {
                Routing routing = item.Tag as Routing;
                EditRouting(routing);
            }
        }

        /// <summary>
        /// 修改工艺路线
        /// </summary>
        /// <param name="routing">工艺路线</param>
        void EditRouting(Routing routing)
        {
            if (routing != null)
            {
                var newRouting = RF.GetById<Routing>(routing.Id);
                ShowRoutingDialog("修改工艺路线", newRouting, (e) =>
                {
                    RF.Save(newRouting);
                    RefreshData();
                });
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void DeleteRouting_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null && item.Type == nameof(Routing))
            {
                Routing routing = item.Tag as Routing;
                DeleteRouting(routing);
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="routing">工艺路线</param>
        void DeleteRouting(Routing routing)
        {
            if (routing != null)
            {
                if (CRT.MessageService.AskQuestion("确定要删除选择的工艺路线吗?".L10N()))
                {
                    routing.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(routing);
                    if (_loadedInfo != null && routing.VersionList.Select(p => p.Id).Contains(_loadedInfo.EntityId.Value))
                        _loadedInfo = null;
                    RefreshData();
                }
            }
        }
        #endregion

        #region 工艺路线版本新增、修改、删除、设置默认
        /// <summary>
        /// 设置默认事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void SetDefault_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null)
            {
                var version = item.Tag as RoutingVersion;
                if (version != null)
                {
                    if (version.State != RoutingState.Release)
                        throw new ValidationException("该工艺路线版本未发布不允许设置为默认".L10N());
                    version.IsDefault = YesNo.Yes;
                    RF.Save(version);
                    RefreshData();
                }
            }
        }

        /// <summary>
        /// 新增流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void AddRoutingVersion_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null && item.Type == nameof(Routing))
            {
                Routing routing = item.Tag as Routing;
                AddRoutingVersion(routing);
            }
        }

        /// <summary>
        /// 新增工艺路线版本
        /// </summary>
        /// <param name="routing">工艺路线</param>
        void AddRoutingVersion(Routing routing)
        {
            if (AddFlow_Click != null && routing != null)
            {
                var model = new ContainerModel()
                {
                    RoutingId = routing.Id,
                    State = ElementState.New,
                };
                AddFlow_Click(model, null);
            }
        }

        /// <summary>
        /// 编辑工艺路线版本
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void EditRoutingVersion_Click(object sender, RoutedEventArgs e)
        {
            if (EditFlow_Click != null)
            {
                var item = routingControl.SelectedItem as TreeInfo;
                if (item != null && item.Type == nameof(RoutingVersion))
                {
                    var version = item.Tag as RoutingVersion;
                    EditFlow_Click(version?.Layout?.Layout, e);
                }
            }
        }

        /// <summary>
        /// 删除工艺路线版本
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void DeleteRoutingVersion_Click(object sender, RoutedEventArgs e)
        {
            var item = routingControl.SelectedItem as TreeInfo;
            if (item != null && item.Type == nameof(RoutingVersion))
            {
                var version = item.Tag as RoutingVersion;
                DeleteRoutingVersion(version);
            }
        }

        /// <summary>
        /// 删除工艺路线版本
        /// </summary>
        /// <param name="routingVersion">工艺路线版本</param>
        void DeleteRoutingVersion(RoutingVersion routingVersion)
        {
            if (CRT.MessageService.AskQuestion("确定要删除选择的工艺流程吗?".L10N()))
            {
                routingVersion.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(routingVersion);
                if (_loadedInfo != null && routingVersion.Id == _loadedInfo.EntityId)
                    _loadedInfo = null;
                RefreshData();
                EditFlow_Click?.Invoke(string.Empty, null);
            }
        }
        #endregion

        /// <summary>
        /// 弹出工艺路线对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="routing">工艺路线</param>
        /// <param name="save">行为</param>
        /// <returns>对话结果，0为确认，1为取消</returns>
        void ShowRoutingDialog(string title, Routing routing, Action<CancelEventArgs> save)
        {
            var detailView = AutoUI.ViewFactory.CreateDetailView(typeof(Routing));
            detailView.Data = routing;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), detailView.Control, w =>
            {
                w.Title = title.L10N();
                w.MinHeight = 230;
                w.MinWidth = 500;
                w.Width = 500;
                w.Height = 230;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            var broken = routing.Validate();
                            if (broken.Count > 0)
                            {
                                throw new ValidationException(broken.ToString());
                            }

                            save(e);
                        }
                        catch (Exception ex)
                        {
                            CRT.MessageService.ShowException(ex, "验证错误");
                            e.Cancel = true;
                        }
                    }
                };
            });
        }
        #endregion

        #region 命令
        /// <summary>
        /// 刷新工艺路线信息命令
        /// </summary>
        public static RoutedUICommand Refresh { get; } = new RoutedUICommand();

        /// <summary>
        /// 重新定位
        /// </summary>
        public static RoutedUICommand Locate { get; } = new RoutedUICommand();

        /// <summary>
        /// 添加工艺路线、工艺流程
        /// </summary>
        public static RoutedUICommand Add { get; } = new RoutedUICommand();

        /// <summary>
        /// 编辑工艺路线
        /// </summary>
        public static RoutedUICommand Edit { get; } = new RoutedUICommand();

        /// <summary>
        /// 删除工艺路线、工艺流程
        /// </summary>
        public static RoutedUICommand Delete { get; } = new RoutedUICommand();

        /// <summary>
        /// 导入工艺路线
        /// </summary>
        public static RoutedUICommand Import { get; } = new RoutedUICommand();

        /// <summary>
        /// 刷新工艺路线信息
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshData();
            CRT.MessageService.ShowInstantMessage("刷新成功".L10N(), "提示", 5);
        }

        /// <summary>
        /// 定位命令执行条件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Locate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = routingControl != null && routingControl.SelectedItem != null && _loadedInfo != null;
        }

        /// <summary>
        /// 定位命令执行方法
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Locate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            helper.ExpandingNodes(routingControl, _treeInfos, keyValuePairs, _loadedInfo);
        }

        /// <summary>
        /// 添加工艺路线、工艺流程命令是否能执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = routingControl != null && routingControl.SelectedItem != null && ((((TreeInfo)routingControl.SelectedItem).Tag is ProductFamilyCategory) || (((TreeInfo)routingControl.SelectedItem).Tag is Routing));
        }

        /// <summary>
        /// 添加工艺路线、工艺流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeInfo = routingControl.SelectedItem as TreeInfo;
            if (treeInfo.Tag is ProductFamilyCategory)
            {
                var productFamily = treeInfo.Tag as ProductFamilyCategory;
                AddRouting(new Routing() { Category = productFamily });
            }

            if (treeInfo.Tag is Routing)
            {
                var routing = treeInfo.Tag as Routing;
                AddRoutingVersion(routing);
            }
        }

        /// <summary>
        /// 编辑工艺路线命令能够执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = routingControl != null && routingControl.SelectedItem != null && ((TreeInfo)routingControl.SelectedItem).Tag is Routing;
        }

        /// <summary>
        /// 编辑工艺路线
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeViewItem = routingControl.SelectedItem as TreeInfo;
            var routing = treeViewItem.Tag as Routing;
            EditRouting(routing);
        }

        /// <summary>
        /// 删除工艺路线、工艺流程命令能够执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (routingControl != null && routingControl.SelectedItem != null)
            {
                if (((TreeInfo)routingControl.SelectedItem).Tag is Routing)
                {
                    e.CanExecute = true;
                }

                if (((TreeInfo)routingControl.SelectedItem).Tag is RoutingVersion)
                {
                    var routingVersion = ((TreeInfo)routingControl.SelectedItem).Tag as RoutingVersion;
                    if (routingVersion.State != RoutingState.Release)
                    {
                        e.CanExecute = true;
                    }
                }
            }
        }

        /// <summary>
        /// 删除工艺路线、工艺流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeViewItem = routingControl.SelectedItem as TreeInfo;
            if (treeViewItem.Tag is Routing)
            {
                var routing = treeViewItem.Tag as Routing;
                DeleteRouting(routing);
            }

            if (treeViewItem.Tag is RoutingVersion)
            {
                var routingVersion = treeViewItem.Tag as RoutingVersion;
                DeleteRoutingVersion(routingVersion);
            }
        }

        /// <summary>
        /// 导入命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(ImportRoutingCheckViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "导入工艺路线".L10N();
                var template = new DetailsUITemplate<ImportRoutingCheckViewModel>();
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                var vm = new ImportRoutingCheckViewModel();
                vm.ImportFinishEvent += (x, y) => { RefreshData(); };
                vm.MarkSaved();
                detailView.Data = vm;
                ////退出时，数据已被修改且未保存时，提示用户
                v.Closing += (o, f) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()))
                        {
                            f.Cancel = false;
                        }
                        else
                        {
                            f.Cancel = true;
                        }
                    }
                };
                return ui;
            });
        }

        /// <summary>
        /// 判断导入是否能执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Import_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        /// <summary>
        /// 工艺路线信息查询事件，回车触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void SearchEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryProductFamily();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 工艺路线信息查询事件，按钮触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            QueryProductFamily();
            e.Handled = true;
        }
    }
}
