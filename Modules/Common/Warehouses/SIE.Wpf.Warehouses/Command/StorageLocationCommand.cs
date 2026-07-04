using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Warehouses.Events;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Warehouses.Printables;
using SIE.Wpf.Warehouses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Warehouses.Command
{
    #region StorageLocationAddCommand 库位添加命令

    /// <summary>
    /// 库位添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class StorageLocationAddCommand : ListAddCommand
    {
        /// <summary>
        /// 重写创建实体后方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void OnItemCreated(Entity entity)
        {
            var storagelocation = entity as StorageLocation;
            storagelocation.Code = RT.Service.Resolve<WarehouseController>().GetStorageLocationCode();
            base.OnItemCreated(entity);
        }
    }
    #endregion

    #region StorageLocationSaveCommand 保存命令
    /// <summary>
    /// 保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", Gestures = "Ctrl+S", GroupType = 10)]
    public class StorageLocationSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 保存前逻辑
        /// </summary>
        /// <param name="view">当前视图对象</param>
        protected override void OnSaving(ListLogicalView view)
        {
            List<StorageLocation> storageLocationList = view.Data.OfType<StorageLocation>().Where(p => p.PersistenceStatus == PersistenceStatus.New).ToList();
            RT.Service.Resolve<WarehouseController>().ValidateStorageLocationCodes(storageLocationList);

            base.OnSaving(view);
        }
    }
    #endregion

    #region StorageLocationFrozenCommand 冻结解冻命令
    /// <summary>
    /// 冻结解冻命令
    /// </summary>
    [Command(Label = "冻结/释放", ToolTip = "冻结/释放", GroupType = CommandGroupType.Business)]
    public class StorageLocationFrozenCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities == null || view.SelectedEntities.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                return storageLocation != null && storageLocation.PersistenceStatus == PersistenceStatus.Unchanged && !storageLocation.Area.IsFrozen && !storageLocation.Warehouse.IsFrozen;
            }
            else
            {
                var sel = view.SelectedEntities.OfType<StorageLocation>().Where(p => p.PersistenceStatus == PersistenceStatus.Unchanged).ToList();
                return sel.Count == view.SelectedEntities.Count &&
                    !sel.Any(p => p.Area.IsFrozen) &&
                    !sel.Any(p => p.Warehouse.IsFrozen) &&
                    sel.Select(p => p.IsFrozen).Distinct().Count() == 1;
            }
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            StorageLocation storageLocation = view.Current as StorageLocation;
            if (!storageLocation.IsFrozen && !CRT.MessageService.AskQuestion("冻结库位后，该库位下的物料将不能进行出入库操作，正在操作的任务在解冻前不能保存，是否继续？".L10N()))
            {
                return;
            }

            IEnumerable<StorageLocation> selStorageLocation = view.SelectedEntities.OfType<StorageLocation>();
            List<double> storageLocationIdList = selStorageLocation.Select(p => p.Id).ToList();
            if (storageLocationIdList.Count == 0)
            {
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().FrozenOrThawLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                CRT.MessageService.ShowMessage(errorMsg, "提示".L10N());
            }
            else
            {
                selStorageLocation.ForEach(p => { p.IsFrozen = !p.IsFrozen; p.MarkSaved(); });
            }
        }
    }
    #endregion

    #region PrintStorageLocationCommand 打印标签
    /// <summary>
    /// 打印标签
    /// </summary>
    [Command(ImageName = "PrintData", Label = "打印标签", ToolTip = "打印标签", GroupType = CommandGroupType.Business)]
    public class PrintStorageLocationCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities == null || view.SelectedEntities.Count == 0)
            {
                return view.Current != null && view.Current.PersistenceStatus == PersistenceStatus.Unchanged;
            }
            else
            {
                return view.SelectedEntities.Any() && view.SelectedEntities.Count == view.SelectedEntities.Count(p => p.PersistenceStatus == PersistenceStatus.Unchanged);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            StorageLocationPrintViewModel entity = new StorageLocationPrintViewModel();
            DetailsUITemplate template = new DetailsUITemplate(entity.GetType(), ViewConfig.DetailsView, view.ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = entity;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "打印".L10N();
                w.Width = 300;
                w.Height = 250;
                w.Closing += (o, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (entity.PrintCount < 1)
                        {
                            CRT.MessageService.ShowMessage("打印份数必须大于0".L10N(), "操作提示".L10N());
                            e.Cancel = true;
                            return;
                        }

                        List<StorageLocation> selStorageLocation = view.SelectedEntities.OfType<StorageLocation>().ToList();
                        if (!selStorageLocation.Any())
                        {
                            if (view.Current as StorageLocation == null)
                            {
                                CRT.MessageService.ShowMessage("请选择需要打印的库位".L10N(), "操作提示".L10N());
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                selStorageLocation.Add(view.Current as StorageLocation);
                            }
                        }

                        PrintTemplate printTemplate = RT.Service.Resolve<WarehouseController>().GetStorageLocationPrintTemplate();
                        if (printTemplate == null)
                        {
                            CRT.MessageService.ShowMessage("请配置库位的打印规则".L10N(), "操作提示".L10N());
                            e.Cancel = true;
                            return;
                        }

                        var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(printTemplate.Id);
                        var printable = new StorageLocationPrintable();
                        var report = ReportFactory.Current.GetReportByExtension(printTemplate.Type);
                        report.Print(printable, filePath, entity.Printer, () =>
                        {
                            List<StorageLocation> printData = new List<StorageLocation>();
                            for (int i = 0; i < entity.PrintCount; i++)
                            {
                                printData.AddRange(selStorageLocation);
                            }

                            return printData;
                        }, () =>
                        {
                            CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
                        });
                    }
                };
            });
        }
    }

    #endregion

    #region StorageLocationDeleteCommand 删除命令
    /// <summary>
    /// 库位删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = 10)]
    public class StorageLocationDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 能否执行删除
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>返回是否可执行删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities == null || view.SelectedEntities.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                return storageLocation != null && (storageLocation.State == State.Disable || storageLocation.PersistenceStatus == PersistenceStatus.New);
            }
            else
            {
                return view.SelectedEntities.OfType<StorageLocation>().Count(p => p.PersistenceStatus == PersistenceStatus.New || p.State == State.Disable) == view.SelectedEntities.Count;
            }
        }
    }

    #endregion

    #region StorageLocationEnableCommand 启用命令
    /// <summary>
    /// 启用命令
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用", GroupType = CommandGroupType.Business)]
    public class StorageLocationEnableCommand : EnableCommand
    {
        /// <summary>
        /// 能否执行启用
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>返回是否可执行启用</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities == null || view.SelectedEntities.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                return storageLocation != null && storageLocation.PersistenceStatus == PersistenceStatus.Unchanged && storageLocation.Area.State == State.Enable && storageLocation.State == State.Disable;
            }
            else
            {
                var sel = view.SelectedEntities.OfType<StorageLocation>().Where(p => p.PersistenceStatus == PersistenceStatus.Unchanged).ToList();
                return !view.Control.View.AllowEditing && sel.Count == view.SelectedEntities.Count && !sel.Any(p => p.Area.State == State.Disable) && !sel.Any(p => p.State == State.Enable);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion("确定启用选中的资料？".L10N()))
            {
                return;
            }

            IEnumerable<StorageLocation> selStorageLocation = view.SelectedEntities.OfType<StorageLocation>();
            List<double> storageLocationIdList = selStorageLocation.Select(p => p.Id).ToList();
            if (storageLocationIdList.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().EnableStorageLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                CRT.MessageService.ShowMessage(errorMsg, "提示".L10N());
            }
            else
            {
                selStorageLocation.ForEach(p => { p.State = State.Enable; p.MarkSaved(); });
            }
        }
    }
    #endregion

    #region StorageLocationDisableCommand 禁用命令
    /// <summary>
    /// 启用命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "停用", ToolTip = "停用", GroupType = CommandGroupType.Business)]
    public class StorageLocationDisableCommand : DisableCommand
    {
        /// <summary>
        /// 能否执行启用
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>返回是否可执行启用</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities == null || view.SelectedEntities.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                return storageLocation != null && storageLocation.PersistenceStatus == PersistenceStatus.Unchanged && storageLocation.Area.State == State.Enable && storageLocation.State == State.Enable;
            }
            else
            {
                var sel = view.SelectedEntities.OfType<StorageLocation>().Where(p => p.PersistenceStatus == PersistenceStatus.Unchanged).ToList();
                return !view.Control.View.AllowEditing && sel.Count == view.SelectedEntities.Count && !sel.Any(p => p.Area.State == State.Disable)&& !sel.Any(p => p.State == State.Disable);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var location = view.Current as StorageLocation;
            if (!CRT.MessageService.AskQuestion("确定禁用选中的资料？".L10N()))
            {
                return;
            }

            RT.EventBus.Publish(new HasLocationStockEvent() { LocId = location.Id });

            IEnumerable<StorageLocation> selStorageLocation = view.SelectedEntities.OfType<StorageLocation>();
            List<double> storageLocationIdList = selStorageLocation.Select(p => p.Id).ToList();
            if (storageLocationIdList.Count == 0)
            {
                StorageLocation storageLocation = view.Current as StorageLocation;
                storageLocationIdList.Add(storageLocation.Id);
            }

            string errorMsg = RT.Service.Resolve<WarehouseController>().DisableStorageLocation(storageLocationIdList);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                CRT.MessageService.ShowMessage(errorMsg, "提示".L10N());
            }
            else
            {
                selStorageLocation.ForEach(p => { p.State = State.Disable; p.MarkSaved(); });
            }
        }
    }
    #endregion

    #region StorageLocationCopyCommand 复制新增命令
    /// <summary>
    /// 库位复制新增命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", ToolTip = "复制添加", GroupType = CommandGroupType.Edit)]
    public class StorageLocationCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Current != null && view.SelectedEntities.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 复制新增
        /// </summary>
        /// <param name="entity">仓库实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            var location = entity as StorageLocation;
            var storageLocation = View.Current as StorageLocation;
            location.Code = storageLocation.Code + "-复制".L10N();
            location.Name = storageLocation.Name;
            location.LibraryType = storageLocation.LibraryType;
            if (storageLocation.Area != null) location.AreaId= storageLocation.AreaId;
            if (storageLocation.Warehouse!=null) location.WarehouseId = storageLocation.WarehouseId;
            location.State = State.Enable;
            location.IsFrozen = false;
            location.ErpInvOrg = storageLocation.ErpInvOrg;
            location.ErpSubLibrary = storageLocation.ErpSubLibrary;
            location.ErpLocation = storageLocation.ErpLocation;
        }
    }
    #endregion

    #region StorageLocationItemLookUpCommand 选择物料
    /// <summary>
    /// 选择物料
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class StorageLocationItemLookUpCommand : LookupCommand
    {
        /// <summary>
        /// 当前选中的缺陷信息
        /// </summary>
        private EntityList<StorageLocationItemList> currStorageLocationItemList = new EntityList<StorageLocationItemList>();

        /// <summary>
        /// 能否执行弹出窗口
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>是否可弹出窗口</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return (view.Parent != null && view.Parent?.Current != null) && view.CanAddItem();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            currStorageLocationItemList = View.Data as EntityList<StorageLocationItemList>;
            base.Execute(view);
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        protected override void OnAccept()
        {
            List<Entity> itemList = new List<Entity>();
            itemList.AddRange(SelectedView.Data.DeletedList);
            itemList.AddRange(SelectedView.Data.OfType<Entity>().Where(f => f.PersistenceStatus == PersistenceStatus.New));
            itemList.ForEach(p =>
            {
                if (!View.Data.DeletedList.Contains(p) && !View.Data.Contains(p))
                {
                    View.Data.Add(p);
                }
            });

            currStorageLocationItemList = View.Data as EntityList<StorageLocationItemList>;
        }

        /// <summary>
        /// 加载已选择数据
        /// </summary>
        /// <param name="parent">父</param>
        /// <returns>已选择数据</returns>
        protected override IDomainComponent LoadSelectedViewDataCore(Entity parent)
        {
            return currStorageLocationItemList;
        }
    }

    #endregion
}