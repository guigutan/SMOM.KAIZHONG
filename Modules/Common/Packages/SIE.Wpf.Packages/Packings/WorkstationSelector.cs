using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Wpf.Packages.Packings
{
    /// <summary>
    /// 工作站选择器
    /// </summary>
    [RootEntity]
    [Label("仓库选择")]
    public class WorkstationSelector : ViewModel
    {
        /// <summary>
        /// 工作站
        /// </summary>
        public Workstation Workstation { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public WorkstationSelector()
        {
        }

        /// <summary>
        /// 带参构造方法
        /// </summary>
        /// <param name="workstation">工作站</param>
        public WorkstationSelector(Workstation workstation)
        {
            Workstation = workstation;
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<WorkstationSelector>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<WorkstationSelector>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }

        /// <summary>
        /// 货区ID
        /// </summary>
        public static readonly IRefIdProperty StorageAreaIdProperty =
        P<WorkstationSelector>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 货区ID
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)this.GetRefNullableId(StorageAreaIdProperty); }
            set { this.SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty =
            P<WorkstationSelector>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return this.GetRefEntity(StorageAreaProperty); }
            set { this.SetRefEntity(StorageAreaProperty, value); }
        }

        /// <summary>
        /// 货位ID
        /// </summary>
        public static readonly IRefIdProperty StorageLocationIdProperty =
             P<WorkstationSelector>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 货位ID
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 货位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<WorkstationSelector>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 货位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }

        /// <summary>
        /// 选择工作单元
        /// </summary>
        /// <param name="workstation">工作站信息</param>
        public static void SelectOperation(Workstation workstation)
        {
            var model = new WorkstationSelector(workstation);
            var template = new DetailsUITemplate(typeof(WorkstationSelector));
            var ui = template.CreateUI();
            ui.MainView.Data = model;
            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "工序选择".L10N();
                w.Width = 400;
                w.Height = 240;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var broken = model.Validate(ValidatorActions.None);
                        if (broken.Count > 0)
                        {
                            CRT.MessageService.ShowMessage(broken.ToString());
                            e.Cancel = true;
                        }
                    }
                };
            });

            if (result == 0)
            {
                //赋值顺序不能更换，工位信息选择触发相应的事件
                workstation.Warehouse = model.Warehouse;
                workstation.StorageArea = model.StorageArea;
                workstation.StorageLocation = model.StorageLocation;
            }
        }
    }

    /// <summary>
    /// 工作站选择器
    /// </summary>
    class WorkstationSelectorConfig : EntityConfig<WorkstationSelector>
    {
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(WorkstationSelector.StorageLocationProperty, new RequiredRule());
            rules.Add(WorkstationSelector.StorageAreaProperty, new RequiredRule());
            rules.Add(WorkstationSelector.WarehouseProperty, new RequiredRule());
        }
    }

    /// <summary>
    /// 工作单元信息视图配置
    /// </summary>
    public class WorkstationSelectorViewConfig : WPFViewConfig<WorkstationSelector>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(1);
            using (View.OrderProperties())
            {
                View.Property(p => p.Warehouse).Show(ShowInWhere.Detail);
                View.Property(p => p.StorageArea).Show(ShowInWhere.Detail).UseSelectionViewMeta(
                    new MetaModel.View.SelectionViewMeta
                    {
                        DataSourceProvider = (entity, p, criteria) =>
                        {
                            var workstation = entity as WorkstationSelector;
                            if (workstation.Workstation.User == null || workstation.Warehouse == null)
                                return new EntityList<StorageArea>();

                            return RT.Service.Resolve<WarehouseController>().GetStorageAreas(workstation.WarehouseId.Value, criteria, p);
                        }
                    });
                View.Property(p => p.StorageLocation).Show(ShowInWhere.Detail).UseSelectionViewMeta(
                        new MetaModel.View.SelectionViewMeta
                        {
                            DataSourceProvider = (entity, p, criteria) =>
                            {
                                var workstation = entity as WorkstationSelector;
                                if (workstation.Workstation.User == null || workstation.StorageArea == null)
                                    return new EntityList<StorageLocation>();

                                return RT.Service.Resolve<WarehouseController>().GetStorageLocations(workstation.StorageAreaId.Value);
                            }
                        });
            }
        }
    }
}
