using Amazon.Runtime.Internal.Transform;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MES.WorkReportPlans;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.Web.MES.WorkReportPlans
{
    /// <summary>
    /// 报工方案视图配置
    /// </summary>
    public class ProcessInfoViewConfig : WebViewConfig<ProcessInfo>
    {
        /// <summary>
        /// 配置类别
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProductFamilyName), nameof(e.Process.ProductFamilyName));
               m.DicLinkField = keyValues;
                
            }).Show( ShowInWhere.List);
            View.Property(p => p.ProductFamilyName).Show(ShowInWhere.List).Readonly();
        }
    }
}
