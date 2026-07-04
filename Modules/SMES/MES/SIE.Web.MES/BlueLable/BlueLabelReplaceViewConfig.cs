using SIE.MES.BlueLable;
using SIE.MetaModel.View;
using SIE.Web.MES.BlueLabel.Commands;
using SIE.Web.MES.LineAndon.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    /// <summary>
    /// 蓝标视图查询
    /// </summary>
    public class BlueLabelReplaceViewConfig : WebViewConfig<BlueLabelReplace>
    {
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands(typeof(BlueLabelImportCommand).FullName);
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Save, typeof(BlueLabelReplaceCommand).FullName);
            {
                View.Property(p => p.OldBlueLabel).ShowInList(width: 150);
                View.Property(p => p.NewBlueLabel).ShowInList(width: 150);
            }
        }
    }
}
