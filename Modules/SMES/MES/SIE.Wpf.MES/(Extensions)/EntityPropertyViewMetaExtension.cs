using DevExpress.Xpf.Editors;
using SIE.Items;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Security;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Editors;
using SIE.Wpf.MES.Editors;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES
{
    /// <summary>
    /// 编辑器扩展类
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 员工编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseEmployeeEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EmployeeLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 工单明细编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseWorkOrderDetailEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<TextEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = WorkOrderEditor.EditorName;
            var config = new TextEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 设置属性的编辑器
        /// </summary>
        /// <typeparam name="T">泛参</typeparam>
        /// <param name="meta">实体属性视图元数据(参数)</param>
        /// <param name="foreground">字体颜色,默认#FFFF0000</param>
        /// <param name="mask">掩码</param>
        /// <param name="maskType">掩码类型，默认值 MaskType.Numeric</param>
        /// <returns>实体属性视图元数据(返回)</returns>
        public static WPFEntityPropertyViewMeta<T> UseHighlightEditor<T>(this WPFEntityPropertyViewMeta<T> meta, string foreground = "#FFFF0000", string mask = "", MaskType maskType = MaskType.Numeric)
        {
            meta.UseDynamicEditor((e) =>
            {
                var ctl = new TextEdit();
                if (!mask.IsNullOrEmpty())
                {
                    ctl.MaskUseAsDisplayFormat = true;
                    ctl.MaskType = maskType;
                    ctl.Mask = mask;
                }
                Color color = (Color)ColorConverter.ConvertFromString(foreground);
                ctl.HorizontalContentAlignment = HorizontalAlignment.Right;
                ctl.FontWeight = FontWeights.Bold;
                ctl.Foreground = new SolidColorBrush(color);
                var binding = new Binding(meta.ViewMeta.DisplayPath());
                ctl.SetBinding(TextEdit.TextProperty, binding);
                ctl.IsReadOnly = e.IsReadOnly == MetaModel.ReadOnlyStatus.ReadOnly;
                ctl.Visibility = e.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                ctl.Name = e.Meta.Name;
                return ctl;
            });
            return meta;
        }

        /// <summary>
        /// 使用换料数量编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性实体元数据(参数)</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性实体元数据(返回)</returns>
        public static WPFEntityPropertyViewMeta<T> UseChangeQtyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ChangeQtyEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 带锁的员工编辑器扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseLockableLookUpEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = LockableLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.SetExtendedProperty(LockableLookUpEditor.HasLock, true);
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        ///// <summary>
        ///// 默认工艺路线版本显示编辑器
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="meta">属性视图元数据</param>
        ///// <param name="action">委托</param>
        ///// <returns>泛型属性视图元数据</returns>
        //public static WPFEntityPropertyViewMeta<T> UseDefaultRoutingDisplayEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<BaseEditorConfig> action = null)
        //{
        //    meta.ViewMeta.EditorName = DefaultRoutingDisplayEditor.EditorName;
        //    var config = new BaseEditorConfig();
        //    meta.ViewMeta.Config = config;
        //    action?.Invoke(config);
        //    return meta;
        //}

        /// <summary>
        /// 获取产品等级编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseProductGradeLookupEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ProductGradeLookupEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 拆分数量编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseBatchSplitQtyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CalcurateEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = BatchSplitQtyEditor.EditorName;
            var config = new CalcurateEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 拆分数量编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseStationMaterialCallQtyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CalcurateEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = StationMaterialCallQtyEditor.EditorName;
            var config = new CalcurateEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 计算器编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseCalcurateEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CalcurateEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CalculateEditor.EditorName;
            var config = new CalcurateEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 布尔切换按钮编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseBoolSwitchEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<BoolSwitchEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = BoolSwitchEditor.EditorName;
            var config = new BoolSwitchEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 检查结果布尔按钮编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseCheckResultBoolEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = CheckResultBoolEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        ///// <summary>
        ///// 叫料数量控件编辑器
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="meta">属性视图元数据</param>
        ///// <param name="action">委托</param>
        ///// <returns>泛型属性视图元数据</returns>
        //public static WPFEntityPropertyViewMeta<T> UseCallMateriaQtyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<SpinEditorConfig> action = null)
        //{
        //    meta.ViewMeta.EditorName = CallMateriaQtyEditor.EditorName;
        //    var config = new SpinEditorConfig();
        //    meta.ViewMeta.Config = config;
        //    action?.Invoke(config);
        //    return meta;
        //}

        /// <summary>
        /// 使用物料属性定义编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UseItemPropertyDefinitionEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ItemPropertyDefinitionLookUpEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            config.OpenDataEditAction = () =>
            {
                var editModuleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(Item));
                if (!PermissionService.CanShowModule(editModuleKey))
                    return;
                var editModule = CommonModel.Modules.FindModule(editModuleKey) as WPFModuleMeta;
                if (editModule != null)
                    CRT.Workbench.OpenModule(editModule);
            };
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用叉板数量编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性实体元数据(参数)</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性实体元数据(返回)</returns>
        public static WPFEntityPropertyViewMeta<T> UseForkPlateQtyEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = ForkPlateQtyEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        ///// <summary>
        ///// 布尔编辑器
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="meta">wpf实体属性实体元数据(参数)</param>
        ///// <param name="action">委托</param>
        ///// <returns>wpf实体属性实体元数据(返回)</returns>
        //public static WPFEntityPropertyViewMeta<T> UseBoolCheckEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CheckEditorConfig> action = null)
        //{
        //    meta.ViewMeta.EditorName = BoolCheckEditor.EditorName;
        //    var config = new CheckEditorConfig();
        //    meta.ViewMeta.Config = config;
        //    action?.Invoke(config);
        //    return meta;
        //}

        /// <summary>
        /// 布尔编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性实体元数据(参数)</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性实体元数据(返回)</returns>
        public static WPFEntityPropertyViewMeta<T> UseIsOkBoolCheckEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CheckEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = IsOkBoolCheckEditor.EditorName;
            var config = new CheckEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 布尔编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性实体元数据(参数)</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性实体元数据(返回)</returns> 
        public static WPFEntityPropertyViewMeta<T> UseIsNgBoolCheckEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<CheckEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = IsNgBoolCheckEditor.EditorName;
            var config = new CheckEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 使用物料属性定义编辑器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">属性视图元数据</param>
        /// <param name="action">委托</param>
        /// <returns>泛型属性视图元数据</returns>
        public static WPFEntityPropertyViewMeta<T> UsePropertyValueEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<EditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = PropertyValueEditor.EditorName;
            var config = new EditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }

        /// <summary>
        /// 生产资源编辑器：企业模型、设备台账
        /// 排除自定义类型的生产资源
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="meta">wpf实体属性视图元数据（参数）</param>
        /// <param name="action">委托</param>
        /// <returns>wpf实体属性视图元数据（返回）</returns>
        public static WPFEntityPropertyViewMeta<T> UseEquipEnterpriseResourceEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PagingLookUpEditorConfig> action = null)
        {
            meta.ViewMeta.EditorName = EquipEnterpriseResourceEditor.EditorName;
            var config = new PagingLookUpEditorConfig();
            meta.ViewMeta.Config = config;
            action?.Invoke(config);
            return meta;
        }


        /// <summary>
        /// 打印机编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WPFEntityPropertyViewMeta<T> UsePrinterExEditor<T>(this WPFEntityPropertyViewMeta<T> meta, Action<PrinterConfig> action = null)
        {
            meta.ViewMeta.EditorName = "PrinterExEditor";
            PrinterConfig printerConfig = new PrinterConfig();
            meta.ViewMeta.Config = printerConfig;
            action?.Invoke(printerConfig);
            return meta;
        }
    }
}