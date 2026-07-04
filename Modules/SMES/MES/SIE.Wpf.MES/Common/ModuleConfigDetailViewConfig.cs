using SIE.Common.Configs;
using SIE.Common.Configs.Module;
using SIE.Domain;
using SIE.Wpf.Common.Configs;
using SIE.Wpf.Common.Templates;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIE.Wpf.MES.Common
{
    internal class ModuleConfigDetailViewConfig : WPFViewConfig<ModuleConfigDetail>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.Category).UseEditor(ConfigCategoryExtEditor.EditorName);
        }

    }

}
