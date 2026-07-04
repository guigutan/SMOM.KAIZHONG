using SIE.MES.QTimes;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes
{
    /// <summary>
    /// QT推送对象视图配置
    /// </summary>
    public class QTPushObjectViewConfig : WebViewConfig<QTPushObject>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(QTimeStandard));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, "SIE.Web.MES.QTimes.Commands.QTPushObjectEditCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.ObjectType).ShowInList(width: 120);
                View.Property(p => p.ObjectCode).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.MES.QTimes.Scripts.QTPushEditor"; p.Editable = false; }).ShowInList(width: 120);
                View.Property(p => p.ObjectName).Readonly().ShowInList(width: 120);
            }
        }
    }
}
