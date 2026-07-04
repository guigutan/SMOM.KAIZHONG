using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpo;
using SIE.Reflection;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIE.Wpf.WorkBenchCommon.Editors
{
    /// <summary>
    /// 指标分类编辑器
    /// </summary>
    public class QuotaCategoryEditor : BaseEditor<QuotaCategoryConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public static readonly string EditorName = "QuotaCategoryEditor";

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
            settings.IsTextEditable = false;
            var propertyType = Meta.PropertyType;
            if (Config.AllowNullInput.HasValue)
                settings.AllowNullInput = Config.AllowNullInput.Value;
            else
                settings.AllowNullInput = propertyType.IsNullable();
            if (settings.AllowNullInput)
            {
                settings.Items.Add(new { Value = string.Empty, Display = string.Empty });
            }

            InitSettingsItems(settings);
            return settings;
        }

        /// <summary>
        /// 初始化设置指标分类的类型
        /// </summary>
        /// <param name="settings">设置参数</param>
        private void InitSettingsItems(ComboxEditorSettings settings)
        {
            List<string> lists = new List<string>();
            var types = CRT.GetAllModules().SelectMany(p => p.Assembly.GetTypes());
            foreach (var type in types)
            {
                var definition = type.GetCustomAttribute<QuotaAttribute>();
                if (definition != null && !lists.Contains(definition.Code))
                {
                    lists.Add(definition.Code);
                }
            }

            lists.Add(string.Empty);
            foreach (var eachlist in lists)
            {
                settings.Items.Add(new { Value = eachlist, Display = eachlist });
            }
        }
    }

    /// <summary>
    /// 指标分类配置类
    /// </summary>
    public class QuotaCategoryConfig : BaseEditorConfig
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
    }
}
