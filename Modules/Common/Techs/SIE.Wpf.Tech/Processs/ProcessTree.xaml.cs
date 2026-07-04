using DevExpress.Xpf.Grid;
using Resources.IconPacks;
using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Wpf.Tech.Helpers;
using SIE.Tech.Processs.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.Tech.Processs
{
    /// <summary>
    /// ProcessTree.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessTree : UserControl
    {
        /// <summary>
        /// 树帮助类
        /// </summary>
        TreeHelper helper = new TreeHelper();

        /// <summary>
        /// 产品簇分类列表
        /// </summary>
        EntityList<ProductFamilyCategory> _categoryList = new EntityList<ProductFamilyCategory>();

        /// <summary>
        /// 工序列表
        /// </summary>
        EntityList<Process> _processList = new EntityList<Process>();

        /// <summary>
        /// 工序树信息
        /// </summary>
        ObservableCollection<TreeInfo> _treeInfos = new ObservableCollection<TreeInfo>();

        /// <summary>
        /// 快捷菜单字典
        /// </summary>
        Dictionary<string, ContextMenu> _contextMenus = new Dictionary<string, ContextMenu>();

        /// <summary>
        /// 自定义树实体ID，用于构建树
        /// </summary>
        int _id;

        /// <summary>
        /// 用于存储已展开Node
        /// </summary>
        Dictionary<string, Tuple<string, double>> keyValuePairs = new Dictionary<string, Tuple<string, double>>();

        /// <summary>
        /// 当前选择节点信息
        /// </summary>
        TreeInfo currentInfo;

        /// <summary>
        /// 工序树控件
        /// </summary>
        public ProcessTree()
        {
            InitializeComponent();
            RefreshData();
            processControl.ContextMenuOpening += ProcessControl_ContextMenuOpening;
            processControl.PreviewMouseLeftButtonDown += ProcessControl_PreviewMouseLeftButtonDown;
            processControl.PreviewMouseDoubleClick += ProcessControl_PreviewMouseDoubleClick;
            processControl.CurrentItemChanged += ProcessControl_CurrentItemChanged;
            processControl.AllowInitiallyFocusedRow = false;

            var treeListView = (processControl.View as TreeListView);
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
        /// 工序树控件左键点击事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">鼠标按键参数</param>
        private void ProcessControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var control = sender as GridControl;
                var selectItem = processControl.SelectedItem as TreeInfo;
                if (processControl.SelectedItem == null || selectItem.Tag == null || !(selectItem.Tag is Process))
                {
                    return;
                }

                var activityModel = new ModelHelper().ConvertToActivity(selectItem.Tag as Process);
                DragDrop.DoDragDrop(control.View, activityModel, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 工序信息列表双击事件，如果节点为工序则编辑工序，否则不做处理
        /// </summary>
        /// <param name="sender">工序控件</param>
        /// <param name="e">参数</param>
        private void ProcessControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                return;
            }

            var selectedItem = processControl.SelectedItem as TreeInfo;
            if (selectedItem != null && selectedItem.Type == nameof(Process))
            {
                var process = selectedItem.Tag as Process;
                if (process != null)
                {
                    EditProcess(process);
                }
            }

            var view = (sender as GridControl)?.View as TreeListView;
            if (view != null)
            {
                var row = processControl.GetSelectedRowHandles();
                if (row.Any())
                {
                    var node = view.GetNodeByRowHandle(row[0]);
                    node.IsExpanded = !node.IsExpanded;
                }
            }
        }

        /// <summary>
        /// 设置右键快捷菜单
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void ProcessControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var selectedItem = processControl.SelectedItem as TreeInfo;
            if (selectedItem == null || selectedItem.Type == nameof(ProductFamilyCategory))
            {
                e.Handled = true;
                return;
            }

            if (selectedItem.Type == nameof(ProductFamily))
            {
                processControl.ContextMenu = GetCategoryContextMenu();
            }
            else if (selectedItem.Type == nameof(Process))
            {
                processControl.ContextMenu = GetProcessContextMenu();
            }
        }

        /// <summary>
        /// 当前项变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void ProcessControl_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (currentInfo != e.NewItem && e.NewItem != null)
            {
                currentInfo = e.NewItem as TreeInfo;
            }
        }

        /// <summary>
        /// 获取分类快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetCategoryContextMenu()
        {
            if (!_contextMenus.ContainsKey(nameof(ProductFamily)))
            {
                ContextMenu contextMenu = new ContextMenu();
                var addProcess = CreateMenuItem("新增", "新增工序", null, PackIconKind.AddEntity);
                addProcess.Click += Add_Click;
                contextMenu.Items.Add(addProcess);
                _contextMenus[nameof(ProductFamily)] = contextMenu;
                return contextMenu;
            }

            return _contextMenus[nameof(ProductFamily)];
        }

        /// <summary>
        /// 获取工序快捷菜单
        /// </summary>
        /// <returns>快捷菜单</returns>
        ContextMenu GetProcessContextMenu()
        {
            if (!_contextMenus.ContainsKey(nameof(Process)))
            {
                ContextMenu contextMenu = new ContextMenu();
                var editProcess = CreateMenuItem("修改", "修改工序", null, PackIconKind.EditEntity);
                editProcess.Click += Edit_Click;
                contextMenu.Items.Add(editProcess);
                var deleteProcess = CreateMenuItem("删除", "删除工序", null, PackIconKind.DeleteEntity);
                deleteProcess.Click += Delete_Click;
                contextMenu.Items.Add(deleteProcess);
                _contextMenus[nameof(Process)] = contextMenu;
                return contextMenu;
            }

            return _contextMenus[nameof(Process)];
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="header">标题</param>
        /// <param name="tooltip">提示信息</param>
        /// <param name="dataContext">上下文</param>
        /// <param name="icon">图标</param>
        /// <returns>菜单项</returns>
        MenuItem CreateMenuItem(string header, string tooltip, object dataContext, PackIconKind? icon)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.SetResourceBinding(HeaderedItemsControl.HeaderProperty, header);
            menuItem.SetResourceBinding(ToolTipProperty, tooltip);
            if (icon != null)
            {
                var packIcon = new PackIcon() { Kind = icon.Value };
                packIcon.Width = 13;
                menuItem.Icon = packIcon;
            }

            return menuItem;
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="arrangeBounds">原尺寸</param>
        /// <returns>尺寸</returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var size = base.ArrangeOverride(arrangeBounds);
            return size;
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        void RefreshData()
        {
            _categoryList = RT.Service.Resolve<ItemController>().GetProductFamilyCategories();
            _processList = RT.Service.Resolve<ProcessController>().GetProcess();
            _contextMenus.Clear();
            QueryInfo();

            helper.ExpandingNodes(processControl, _treeInfos, keyValuePairs, currentInfo);
        }

        /// <summary>
        /// 查询过来信息，并加载出节点
        /// </summary>
        void QueryInfo()
        {
            processControl.ItemsSource = null;
            _treeInfos.Clear();
            _id = 0;
            var categoryList = _categoryList;
            var processList = _processList;
            var query = searchEdit.EditValue?.ToString();
            if (!query.IsNullOrEmpty())
            {
                processList = _processList.Where(p => p.Name.Contains(query)).AsEntityList();
                var categoryIds = processList.Select(p => p.ProductFamily?.CategoryId).Distinct().ToList();
                categoryList = _categoryList.Where(p => categoryIds.Contains(p.Id)).AsEntityList();
            }

            LoadProductFamilyNodes(categoryList, processList);
            processControl.ItemsSource = _treeInfos;
        }

        /// <summary>
        /// 加载产品簇分类节点
        /// </summary>
        /// <param name="categoryList">产品簇分类列表</param>
        /// <param name="processList">工序列表</param>
        void LoadProductFamilyNodes(IEnumerable<ProductFamilyCategory> categoryList, IEnumerable<Process> processList)
        {
            foreach (var category in categoryList.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = "{0}({1})".FormatArgs(category.Name, category.Code),
                    Tag = category,
                    Type = nameof(ProductFamilyCategory),
                    EntityId = category.Id
                };
                _treeInfos.Add(treeInfo);
                LoadProductFamilyCategoryNodes(category, processList, treeInfo);
            }
        }

        /// <summary>
        /// 加载产品簇分类节点
        /// </summary>
        /// <param name="category">产品簇分类</param>
        /// <param name="processList">工序列表</param>
        /// <param name="parentTreeInfo">工序树信息</param>
        private void LoadProductFamilyCategoryNodes(ProductFamilyCategory category, IEnumerable<Process> processList, TreeInfo parentTreeInfo)
        {
            var families = RT.Service.Resolve<ItemController>().GetProductFamilies(category.Id);
            foreach (var family in families.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = "{0}({1})".FormatArgs(family.Name, family.Code),
                    Pid = parentTreeInfo.Id,
                    Tag = family,
                    Type = nameof(ProductFamily),
                    EntityId = family.Id
                };
                _treeInfos.Add(treeInfo);
                LoadProcessNodes(processList.Where(p => p.ProductFamilyId == family.Id), treeInfo);
            }
        }

        /// <summary>
        /// 加载工序节点
        /// </summary>
        /// <param name="processList">工序列表</param>
        /// <param name="parentTreeInfo">工序树信息</param>
        private void LoadProcessNodes(IEnumerable<Process> processList, TreeInfo parentTreeInfo)
        {
            foreach (var process in processList.OrderBy(p => p.Name))
            {
                var treeInfo = new TreeInfo()
                {
                    Id = ++_id,
                    DisplayName = "{0}({1}){2}".FormatArgs((process.Type == ProcessType.BatchAssembly || process.Type == ProcessType.BatchFix || process.Type == ProcessType.BatchPacking || process.Type == ProcessType.BatchPqc) ? "[批]" : "[单]", process.Name, process.ReferenceTimes),
                    Pid = parentTreeInfo.Id,
                    Tag = process,
                    Type = nameof(Process),
                    EntityId = process.Id
                };
                _treeInfos.Add(treeInfo);
            }
        }

        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="sender">菜单项</param>
        /// <param name="e">参数</param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            if (item == null)
            {
                return;
            }

            Process process = item.Tag as Process;
            if (process != null)
            {
                DeleteProcess(process);
            }
        }

        /// <summary>
        /// 修改工序
        /// </summary>
        /// <param name="sender">菜单项</param>
        /// <param name="e">参数</param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            if (item == null)
            {
                return;
            }

            Process process = item.Tag as Process;
            if (process != null)
            {
                EditProcess(process);
            }
        }

        /// <summary>
        /// 添加工序
        /// </summary>
        /// <param name="sender">菜单项</param>
        /// <param name="e">路由事件参数</param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            if (item == null)
            {
                return;
            }

            ProductFamily category = item.Tag as ProductFamily;
            if (category != null)
            {
                AddProcess(category);
            }
        }

        /// <summary>
        /// 定义新增命令
        /// </summary>
        public static RoutedUICommand Add { get; } = new RoutedUICommand();

        /// <summary>
        /// 定义刷新命令
        /// </summary>
        public static RoutedUICommand Refresh { get; } = new RoutedUICommand();

        /// <summary>
        /// 定义编辑命令
        /// </summary>
        public static RoutedUICommand Edit { get; } = new RoutedUICommand();

        /// <summary>
        /// 定义删除命令
        /// </summary>
        public static RoutedUICommand Delete { get; } = new RoutedUICommand();

        /// <summary>
        /// 刷新命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 添加命令是否可执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = processControl != null && processControl.SelectedItem != null && ((TreeInfo)processControl.SelectedItem).Tag is ProductFamily;
        }

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        public void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            ProductFamily category = item.Tag as ProductFamily;
            AddProcess(category);
        }

        /// <summary>
        /// 编辑命令是否可执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = processControl != null && processControl.SelectedItem != null && ((TreeInfo)processControl.SelectedItem).Tag is Process;
        }

        /// <summary>
        /// 编辑命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            var process = item.Tag as Process;
            EditProcess(process);
        }

        /// <summary>
        /// 删除命令是否可执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = processControl != null && processControl.SelectedItem != null && ((TreeInfo)processControl.SelectedItem).Tag is Process;
        }

        /// <summary>
        /// 删除命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var item = processControl.SelectedItem as TreeInfo;
            var process = item.Tag as Process;
            DeleteProcess(process);
        }

        /// <summary>
        /// 添加工序
        /// </summary>
        /// <param name="category">产品族分类</param>
        public void AddProcess(ProductFamily category)
        {
            Process process = new Process();
            process.GenerateId();
            var processPropertyChanged = new ProcessPropertyChanged();
            process.PropertyChanged += processPropertyChanged.PropertyChanged;
            process.ProductFamily = category;
            ShowProcessDialog("新增工序", process, () =>
            {
                RF.Save(process);
                RefreshData();
            });
        }

        /// <summary>
        /// 编辑工序
        /// </summary>
        /// <param name="process">工序</param>
        public void EditProcess(Process process)
        {
            var processPropertyChanged = new ProcessPropertyChanged();
            var newProcess = RF.GetById<Process>(process.Id);
            newProcess.PropertyChanged += processPropertyChanged.PropertyChanged;
            ShowProcessDialog("编辑工序", newProcess, () =>
            {
                RF.Save(newProcess);
                ////刷新树
                RefreshData();
            });
        }

        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="process">工序</param>
        void DeleteProcess(Process process)
        {
            if (CRT.MessageService.AskQuestion("确定删除工序？".L10N(), "提示".L10N()))
            {
                process.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(process);
                ////刷新树
                RefreshData();
            }
        }

        /// <summary>
        /// 显示工序编辑窗口
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="process">工序</param>
        /// <param name="save">处理方法</param>
        /// <returns>窗口编辑结果，0为确定，1为取消</returns>
        private void ShowProcessDialog(string title, Process process, Action save)
        {
            var template = new DetailsUITemplate(typeof(Process), ViewConfig.DetailsView);
            var ui = template.CreateUI();
            ui.MainView.Data = process;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = title.L10N();
                w.MinHeight = 550;
                w.MinWidth = 750;
                w.Height = 550;
                w.Width = 750;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            save();
                        }
                        catch (Exception ex)
                        {
                            ex.Alert();
                            e.Cancel = true;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 查找工序
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void SearchEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryInfo();
            }
        }

        /// <summary>
        /// 查找工序
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            QueryInfo();
        }
    }
}