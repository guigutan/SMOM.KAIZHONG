using SIE.Domain;
using SIE.MetaModel.View;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Web.CSM.ItemInspCharacteristicses.Commands;
using System;
using System.Linq.Expressions;

namespace SIE.Web.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 物料检验特性维护视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemInspCharacteristicsViewConfig : WebViewConfig<ItemInspCharacteristics>
    {
        /// <summary>
        /// 批量设置扩展视图名称
        /// </summary>
        public const string BatchSettingView = "BatchSettingView";

        #region 周期类型、间隔周期是否只读 IsReadonlyProperty
        /// <summary>
        /// 周期类型、间隔周期是否只读
        /// </summary>
        public static readonly Property<bool> IsReadonlyProperty = P<ItemInspCharacteristics>.RegisterExtensionReadOnly("IsReadonly", typeof(ItemInspCharacteristicsViewConfig),
            GetIsReadonly, ItemInspCharacteristics.PeriodTypeProperty, ItemInspCharacteristics.IntervalPeriodProperty);

        /// <summary>
        /// 周期类型、间隔周期是否只读
        /// </summary>
        /// <param name="me">物料检验特性</param>
        /// <returns>true/false</returns>
        public static bool GetIsReadonly(ItemInspCharacteristics me)
        {
            return !me.RecurringInspection;
        }
        #endregion

        #region 供方状态禁用时只读

        /// <summary>
        /// 供方状态禁用时只读
        /// </summary>
        public static readonly Expression<Func<ItemInspCharacteristics, bool>> IsReadonlyForbitProperty = p => p.SupplierState != State.Enable;
        #endregion

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchSettingView);
            if (ViewGroup == BatchSettingView)
                ConfigBatchSettingView();
        }
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(BatchSettingCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, typeof(ListSaveCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.Help);
            View.Property(p => p.Item).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.Supplier).Readonly();
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.SupplierState).Readonly();
            View.Property(p => p.RecurringInspection).Readonly(IsReadonlyForbitProperty).UseListSetting(e => { e.HelpInfo = "供方状态等于可用时可编辑，不可与确认检、免检同时勾选"; })
                .UseCheckEditor(x => x.ColumnXType = "CSM_ItemInspCharacteristics_GridCheckBoxColumn");
            View.Property(p => p.PeriodType).Readonly(IsReadonlyForbitProperty).UseListSetting(e => { e.HelpInfo = "供方状态等于可用时可编辑"; }).Cascade(p => p.InspectDateBegin, null);
            View.Property(p => p.IntervalPeriod).UseSpinEditor(p =>
            {
                p.MinValue = 1;
                p.AllowDecimals = false;
            }).Readonly(IsReadonlyForbitProperty).UseListSetting(e => { e.HelpInfo = "供方状态等于可用时可编辑"; });
            View.Property(p => p.ConfirmInspection).Readonly(IsReadonlyForbitProperty).UseListSetting(e => { e.HelpInfo = "供方状态等于可用时可编辑，不可与周期检、免检同时勾选"; })
                .UseCheckEditor(x => x.ColumnXType = "CSM_ItemInspCharacteristics_GridCheckBoxColumn");
            View.Property(p => p.InspectionFree).Readonly(IsReadonlyForbitProperty).UseListSetting(e => { e.HelpInfo = "供方状态等于可用时可编辑，不可与周期检、确认检同时勾选"; })
            .UseCheckEditor(x => x.ColumnXType = "CSM_ItemInspCharacteristics_GridCheckBoxColumn");
            View.Property(p => p.EffectiveStartTime).Readonly(p => p.InspectionFree == false).UseListSetting(e => { e.HelpInfo = "免检时可编辑"; }).DefaultValue(System.DateTime.Today).UseDateEditor();
            View.Property(p => p.EffectiveEndTime).Readonly(p => p.InspectionFree == false).UseListSetting(e => { e.HelpInfo = "免检时可编辑"; }).UseDateEditor();
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Supplier);
            View.Property(p => p.SupplierName);
            View.Property(p => p.Item);
            View.Property(p => p.ItemName);
            View.Property(p => p.SupplierState).UseEnumEditor(p => p.AllowBlank = true);
        }
        /// <summary>
        /// 配置批量设置视图
        /// </summary>
        void ConfigBatchSettingView()
        {
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.RecurringInspection).Show(ShowInWhere.Detail);
                View.Property(p => p.PeriodType).Show(ShowInWhere.Detail);
                View.Property(p => p.IntervalPeriod).Show(ShowInWhere.Detail).UseSpinEditor(p =>
                {
                    p.MinValue = 1;
                    p.AllowDecimals = false;
                });
                View.Property(p => p.ConfirmInspection).Show(ShowInWhere.Detail);
            }
        }
    }
}
