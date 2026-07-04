using SIE.Domain;
using SIE.Warehouses;
using SIE.Warehouses.Stations;
using SIE.Web.Warehouses.Stations.Commands;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// 站台视图配置
    /// </summary>
    public class StationViewConfig : WebViewConfig<Station>
    {
        /// <summary>
        /// 站台配置
        /// </summary>
        internal const string BaseDataViewGroup = "BaseDataViewGroup";

        /// <summary>
        /// 楼层站台配置
        /// </summary>
        internal const string FloorViewGroup = "FloorViewGroup";

        /// <summary>
        /// 拣选站台配置
        /// </summary>
        internal const string PickViewGroup = "PickViewGroup";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { BaseDataViewGroup, FloorViewGroup, PickViewGroup });
            if (ViewGroup == BaseDataViewGroup)
                ConfigBaseDataView();
            if (ViewGroup == FloorViewGroup)
                ConfigFloorView();
            if (ViewGroup == PickViewGroup)
                ConfigPickView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(StationImportCommand).FullName);
            View.UseCommands("SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInList(width: 180).FixColumn();
            View.Property(p => p.Name);
            View.Property(p => p.StationType).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Warehouse).UseWarehouseEditor().Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Floor);
            View.Property(p => p.Routeway).UseDataSource((source, pagingInfo, keyword) =>
            {
                var station = source as Station;
                if (station != null)
                {
                    return RT.Service.Resolve<WarehouseController>().GetRouteways(station.WarehouseId, null, keyword, pagingInfo);
                }
                return new EntityList<Routeway>();
            });
            View.AttachDetailChildrenProperty(typeof(Station), (c) =>
            {
                var item = c.Parent as Station;
                item = RF.GetById<Station>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                return item;
            }, BaseDataViewGroup).HasLabel("站台配置数据").OrderNo = 10;
            View.AttachDetailChildrenProperty(typeof(Station), (c) =>
            {
                var item = c.Parent as Station;
                item = RF.GetById<Station>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                return item;
            }, FloorViewGroup).HasLabel("楼层站台属性").OrderNo = 20;
            View.AttachDetailChildrenProperty(typeof(Station), (c) =>
            {
                var item = c.Parent as Station;
                item = RF.GetById<Station>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                return item;
            }, PickViewGroup).HasLabel("拣选站台属性").OrderNo = 30;
        }

        /// <summary>
        /// 站台配置数据视图
        /// </summary>
        protected void ConfigBaseDataView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.Led).Show();
                View.Property(p => p.OpcSerialNo).Show();
                View.Property(p => p.OpcScannerNo).Show();
                View.Property(p => p.OpcSubTrayScannerNo).Show();
                View.Property(p => p.DeviceDockNo).Show();
                View.Property(p => p.IsNeedWriteInstructId).Show();
                View.Property(p => p.IsNeedWriteBarcode).Show();
                View.Property(p => p.BackupExitAddress).ShowInDetail(columnSpan: 2).UseListSetting(e => { e.HelpInfo = "同仓库其他站台编码，多个值用符号“|”隔开"; });
                View.Property(p => p.RelatedStation).Show();
                View.Property(p => p.InOutRelatedStation).Show().ShowInDetail(columnSpan: 2).UseListSetting(e => { e.HelpInfo = "同仓库其他站台编码，多个值用符号“|”隔开"; });
                View.Property(p => p.TrayType).Show();
                View.Property(p => p.IsLinkProduction).Show();
                View.Property(p => p.IsLock).Show();
                View.Property(p => p.FunctionDescription).Show();
                View.Property(p => p.LocationDescription).Show();
                View.Property(p => p.Note).Show();
            }
        }

        /// <summary>
        /// 楼层站台视图
        /// </summary>
        protected void ConfigFloorView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.Priority).Show();
                View.Property(p => p.DeviceLineNo).Show();
                View.Property(p => p.DeviceLayerNo).Show();
                View.Property(p => p.DeviceColumnNo).Show();
            }
        }

        /// <summary>
        /// 拣选站台视图
        /// </summary>
        protected void ConfigPickView()
        {
            View.Property(p => p.SerialNo).Show();
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.StationType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.Warehouse);
            View.Property(p => p.Routeway);
            View.Property(p => p.Led);
            View.Property(p => p.OpcSerialNo);
            View.Property(p => p.OpcSubTrayScannerNo);
            View.Property(p => p.IsNeedWriteInstructId);
            View.Property(p => p.IsNeedWriteBarcode);
            View.Property(p => p.BackupExitAddress);
            View.Property(p => p.RelatedStation);
            View.Property(p => p.InOutRelatedStation);
            View.Property(p => p.IsLinkProduction);
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(width: 180);
            View.Property(p => p.Name);
            View.Property(p => p.State);
            View.Property(p => p.StationType);
            View.Property(p => p.Warehouse);
            View.Property(p => p.Routeway);
            View.Property(p => p.Led);
            View.Property(p => p.OpcSerialNo);
            View.Property(p => p.OpcSubTrayScannerNo);
            View.Property(p => p.IsNeedWriteInstructId);
            View.Property(p => p.IsNeedWriteBarcode);
            View.Property(p => p.BackupExitAddress);
            View.Property(p => p.RelatedStation);
            View.Property(p => p.InOutRelatedStation);
            View.Property(p => p.IsLinkProduction);
            View.Property(p => p.IsLock);
            View.Property(p => p.Floor);
            View.Property(p => p.FunctionDescription);
            View.Property(p => p.LocationDescription);
            View.Property(p => p.Priority);
            View.Property(p => p.DeviceLineNo);
            View.Property(p => p.DeviceLayerNo);
            View.Property(p => p.DeviceColumnNo);
            View.Property(p => p.SerialNo);
            View.Property(p => p.OpcScannerNo);
            View.Property(p => p.DeviceDockNo);
            View.Property(p => p.TrayType);
            View.Property(p => p.Note);

        }

        /// <summary>
        /// 导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.PropertyRef(p => p.Warehouse.Code).HasLabel("仓库编码");
            View.Property(p => p.StationType);
            View.PropertyRef(p => p.Led.Code).HasLabel("LED屏显数据编码");
            View.PropertyRef(p => p.Routeway.Code).HasLabel("巷道编码");
            View.Property(p => p.IsLinkProduction);
            View.Property(p => p.Floor);
            View.Property(p => p.FunctionDescription);
            View.Property(p => p.LocationDescription);
            View.Property(p => p.Priority);
            View.Property(p => p.DeviceLineNo);
            View.Property(p => p.DeviceLayerNo);
            View.Property(p => p.DeviceColumnNo);
            View.Property(p => p.SerialNo);
            View.Property(p => p.OpcSerialNo);
            View.Property(p => p.OpcScannerNo);
            View.Property(p => p.OpcSubTrayScannerNo);
            View.Property(p => p.DeviceDockNo);
            View.Property(p => p.IsNeedWriteInstructId);
            View.Property(p => p.IsNeedWriteBarcode);
            View.Property(p => p.BackupExitAddress);
            View.Property(p => p.RelatedStation);
            View.Property(p => p.InOutRelatedStation);
            View.Property(p => p.TrayType);
            View.Property(p => p.Note);
        }
    }
}