using SIE.Barcodes;
using SIE.MES.BatchWIP.Products;
using SIE.MES.PackingPrints;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.MES;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.ConnectorPacking;
using SIE.Wpf.MES.ConnectorPackings;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.NewPackingQC;
using SIE.Wpf.MES.NewPackingQcC;
using SIE.Wpf.MES.NewPackingQcD;
using SIE.Wpf.MES.OnOffDutyB;
using SIE.Wpf.MES.PackingPrints;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.PanelBindings;
using SIE.Wpf.MES.TouchScreenHomepage;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Moves;
using SIE.Wpf.MES.WIP.NewPackages;
using SIE.Wpf.MES.WIP.Packings;
using SIE.Wpf.MES.WIP.PackRecombine;
using SIE.Wpf.MES.WIP.Pressure;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.Reworks;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.MES
{
    /// <summary>
    /// 模块
    /// </summary>
    class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, System.EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "检验采集",
                EntityType = typeof(InspectViewModel),
                BlocksTemplate = typeof(InspectUITemplate)
            }, new WPFModuleMeta
            {
                Label = "检验项目采集",
                EntityType = typeof(InspectByItemViewModel),
                BlocksTemplate = typeof(InspectByItemUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "上料采集",
                EntityType = typeof(AssemblyViewModel),
                BlocksTemplate = typeof(AssemblyUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "过站采集",
                EntityType = typeof(MoveViewModel),
                BlocksTemplate = typeof(MoveUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "包装采集",
                EntityType = typeof(NewPackingViewModel),
                BlocksTemplate = typeof(NewPackingUITemplate<NewPackingViewModel>)
            },
            new WPFModuleMeta()
            {
                Label = "返工采集",
                EntityType = typeof(ReworkViewModel),
                BlocksTemplate = typeof(ReworkUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "生产通用报表",
                EntityType = typeof(WipProductVersion)
            }, new WPFModuleMeta()
            {
                Label = "维修采集",
                EntityType = typeof(RepairViewModel),
                BlocksTemplate = typeof(RepairUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次上料采集",
                EntityType = typeof(BatchAssemblyViewModel),
                BlocksTemplate = typeof(BatchAssemblyUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次维修采集",
                EntityType = typeof(BatchRepairViewModel),
                BlocksTemplate = typeof(BatchRepairUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次过站采集",
                EntityType = typeof(BatchMoveViewModel),
                BlocksTemplate = typeof(BatchMoveUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次检验采集",
                EntityType = typeof(BatchInspectViewModel),
                BlocksTemplate = typeof(BatchInspectUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次包装采集",
                EntityType = typeof(BatchPackingViewModel),
                BlocksTemplate = typeof(BatchPackingUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "批次生产通用报表",
                EntityType = typeof(BatchWipProductVersion)
            }, new WPFModuleMeta()
            {
                Label = "包装管理",
                EntityType = typeof(PackRecombineViewModel),
                BlocksTemplate = typeof(PackRecombineUITemplate),
                ViewGroup = ViewConfig.DetailsView
            }, new WPFModuleMeta()
            {
                Label = "批次包装管理",
                EntityType = typeof(BatchPackRecombineViewModel),
                BlocksTemplate = typeof(BatchPackRecombineUITemplate),
                ViewGroup = ViewConfig.DetailsView
            }, new WPFModuleMeta()
            {
                Label = "条码绑定",
                EntityType = typeof(PanelBindingViewModel),
                BlocksTemplate = typeof(PanelBindingUITemplate)
            }, new WPFModuleMeta()
            {
                Label = "包装号打印",
                EntityType = typeof(PackingWorkOrder),
                BlocksTemplate = typeof(ListUITemplate),
                ViewGroup = PackingWorkOrderViewConfig.PackingPrintView
            },
            new WPFModuleMeta()
            {
                Label = "触摸屏首页",
                EntityType = typeof(TouchScreenHomepageViewModel),
                BlocksTemplate = typeof(TouchScreenHomepageUITemplate)
            }
            , new WPFModuleMeta()
            {
                Label = "新包装采集",
                EntityType = typeof(NewPackageViewModel),
                BlocksTemplate = typeof(NewPackageUITemplate)
            },
             new WPFModuleMeta()
             {
                 Label = "临时维修采集",
                 EntityType = typeof(TemporaryRepairViewModel),
                 BlocksTemplate = typeof(TemporaryRepairUITemplate)
             }
            , new WPFModuleMeta()
            {
                Label = "直接包装采集",
                EntityType = typeof(DirectPackingViewModel),
                BlocksTemplate = typeof(DirectPackingUITemplate)
            },
             new WPFModuleMeta()
             {
                 Label = "B上岗/下岗",
                 EntityType = typeof(OnOffDutyBViewModel),
                 BlocksTemplate = typeof(OnOffDutyBUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "汇流排包装采集",//-耐压标签
                 EntityType = typeof(PackingQcViewModel),
                 BlocksTemplate = typeof(PackingQcUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "B包装采集",//-耐压-批次标签
                 EntityType = typeof(NewPackingQcViewModel),
                 BlocksTemplate = typeof(NewPackingQcUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "C包装采集",//-物料标签
                 EntityType = typeof(NewPackingQcCViewModel),
                 BlocksTemplate = typeof(NewPackingQcCUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "蓝标包装采集",//只管蓝标
                 EntityType = typeof(NewPackingQcDViewModel),
                 BlocksTemplate = typeof(NewPackingQcDUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "耐压采集",
                 EntityType = typeof(PressureViewModel),
                 BlocksTemplate = typeof(PressureUITemplate)
             }, new WPFModuleMeta()
             {
                 Label = "包装QC确认".L10N(),
                 EntityType = typeof(PackingQcModel)
             },
             new WPFModuleMeta()
             {
                 Label = "连接器批次包装采集",
                 EntityType = typeof(ConnectorPackingViewModel),
                 BlocksTemplate = typeof(ConnectorPackingUITemplate)
             },
             new WPFModuleMeta()
             {
                 Label = "连接器单体包装采集",
                 EntityType = typeof(ConnectorSnPackingViewModel),
                 BlocksTemplate = typeof(ConnectorSnPackingUITemplate)
             }



            );
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemPropertyDefinitionLookUpEditor.EditorName, typeof(ItemPropertyDefinitionLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EmployeeLookUpEditor.EditorName, typeof(EmployeeLookUpEditor));

                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WorkOrderEditor.EditorName, typeof(WorkOrderEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BoolSwitchEditor.EditorName, typeof(BoolSwitchEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ChangeQtyEditor.EditorName, typeof(ChangeQtyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(LockableLookUpEditor.EditorName, typeof(LockableLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BatchBarcodeEditor.EditorName, typeof(BatchBarcodeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BatchLoadItemBarcodeEditor.EditorName, typeof(BatchLoadItemBarcodeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ProductGradeLookupEditor.EditorName, typeof(ProductGradeLookupEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BatchSplitQtyEditor.EditorName, typeof(BatchSplitQtyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(StationMaterialCallQtyEditor.EditorName, typeof(StationMaterialCallQtyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CalculateEditor.EditorName, typeof(CalculateEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BatchMoveEditor.EditorName, typeof(BatchMoveEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BoolButtonEditor.EditorName, typeof(BoolButtonEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CallMateriaQtyEditor.EditorName, typeof(CallMateriaQtyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PackRecombineEditor.EditorName, typeof(PackRecombineEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BoolCheckEditor.EditorName, typeof(BoolCheckEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(IsOkBoolCheckEditor.EditorName, typeof(IsOkBoolCheckEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(IsNgBoolCheckEditor.EditorName, typeof(IsNgBoolCheckEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WeighEditor.EditorName, typeof(WeighEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BoolToggleButtonEditor.EditorName, typeof(BoolToggleButtonEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EquipEnterpriseResourceEditor.EditorName, typeof(EquipEnterpriseResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ForkPlateQtyEditor.EditorName, typeof(ForkPlateQtyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CheckResultBoolEditor.EditorName, typeof(CheckResultBoolEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ChangeWorkStationEditor.EditorName, typeof(ChangeWorkStationEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ConfigCategoryExtEditor.EditorName, typeof(ConfigCategoryExtEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PrinterExEditor.EditorName, typeof(PrinterExEditor));
            };
        }
    }
}