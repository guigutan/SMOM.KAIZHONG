using SIE.MES.BarcodeProcesses;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 单体工序查询视图配置
    /// </summary>
    public class SingleProcessCriteriaViewConfig : WebViewConfig<SingleProcessCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery);
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.ProcessType).UseEnumEditor(p => p.FilterCategoery = "Single").Show();
            View.Property(p => p.ProductFamily).Show();
        }
    }
}
