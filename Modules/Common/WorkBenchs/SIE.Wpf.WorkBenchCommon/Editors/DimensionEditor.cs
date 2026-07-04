using DevExpress.Xpf.Editors.Settings;
using SIE.Reflection;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Editors;
using System;
using System.ComponentModel;

namespace SIE.Wpf.WorkBenchCommon.Editors
{
    /// <summary>
    /// 时间周期编辑器
    /// </summary>
    public class DimensionEditor : BaseEditor<DimensionConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public static readonly string EditorName = "DimensionEditor";

        /// <summary>
        /// 编辑器基本设置核心方法
        /// </summary>
        /// <returns>返回编辑器基础设置</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            var settings = new ComboxEditorSettings();
            settings.NullText = Config.NullText;
            settings.ValueMember = "Value";
            settings.DisplayMember = "Display";
            var propertyType = Meta.PropertyType;
            if (Config.AllowNullInput.HasValue)
                settings.AllowNullInput = Config.AllowNullInput.Value;
            else
                settings.AllowNullInput = propertyType.IsNullable();
            if (settings.AllowNullInput)
            {
                settings.Items.Add(new { Value = (int?)null, Display = string.Empty });
            }

            InitSettingsItems(Config, settings);
            return settings;
        }

        /// <summary>
        /// 初始化设置方法
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="settings">设置参数</param>
        private void InitSettingsItems(DimensionConfig config, ComboxEditorSettings settings)
        {
            int maxValue = GetItemsMaxValue(config);
            int minValue = GetItemsMinValue(config);
            InitItems(config, settings, minValue, maxValue);
        }

        /// <summary>
        /// 获得时间周期最大值方法
        /// </summary>
        /// <param name="config">配置</param>
        /// <returns>返回时间周期最大值</returns>
        int GetItemsMaxValue(DimensionConfig config)
        {
            int maxValue = 0;
            switch (config.DateType)
            {
                case DateType.YEAR:
                    maxValue = DateTime.Now.Year + 5;
                    break;
                case DateType.SEASON:
                    maxValue = 4;
                    break;
                case DateType.MONTH:
                    maxValue = 12;
                    break;
                case DateType.WEEK:
                    maxValue = 53;
                    break;
            }

            return maxValue;
        }

        /// <summary>
        /// 获得时间周期最小值方法
        /// </summary>
        /// <param name="config">配置</param>
        /// <returns>返回时间周期最小值</returns>
        int GetItemsMinValue(DimensionConfig config)
        {
            int minValue = 0;
            switch (config.DateType)
            {
                case DateType.YEAR:
                    minValue = DateTime.Now.Year;
                    break;
                case DateType.SEASON:
                    minValue = 1;
                    break;
                case DateType.MONTH:
                    minValue = 1;
                    break;
                case DateType.WEEK:
                    minValue = 1;
                    break;
            }

            return minValue;
        }

        /// <summary>
        /// 获得周期类型的范围
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="settings">编辑器设置参数</param>
        /// <param name="minValue">时间最小值</param>
        /// <param name="maxValue">时间最大值</param>
        private void InitItems(DimensionConfig config, ComboxEditorSettings settings, int minValue, int maxValue)
        {
            string formatArgs = GetFormatArgs(config);
            for (int i = minValue; i <= maxValue; i++)
            {
                settings.Items.Add(new { Value = i, Display = formatArgs.IsNullOrEmpty() ? formatArgs : formatArgs.FormatArgs(i) });
            }
        }

        /// <summary>
        /// 时间周期显示格式
        /// </summary>
        /// <param name="config">配置</param>
        /// <returns>返回处理好的时间周期显示格式</returns>
        string GetFormatArgs(DimensionConfig config)
        {
            string formatArgs = string.Empty;
            if (!config.Format.IsNullOrEmpty() && config.Format.Contains("{0}"))
                formatArgs = config.Format;

            return formatArgs;
        }
    }

    /// <summary>
    /// 时间维度配置类
    /// </summary>
    public class DimensionConfig : BaseEditorConfig
    {
        /// <summary>
        /// 掩码表达式
        /// </summary>
        [DisplayName("是否允许空输入")]
        public bool? AllowNullInput
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DisplayName("数据类型")]
        public DateType DateType
        {
            get { return GetProperty<DateType>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        [DisplayName("格式化")]
        public string Format
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
