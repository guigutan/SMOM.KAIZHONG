using SIE.MES.PackingQC;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC
{
    /// <summary>
    /// 装箱明细确认
    /// </summary>
    public class PackingDetailViewConfig : WebViewConfig<PackingDetail>
    {
        /// <summary>
        /// 装箱QC确认
        /// </summary>
        public const string SnView = "SnView";

        /// <summary>
        /// 总视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SnView);
            if (ViewGroup == SnView)
            {
                SnViewConfig();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.ExportXls);
                View.Property(p => p.WorkOrderNo).ShowInList(width: 200);
                View.Property(p => p.ProductLabel).ShowInList(width:250);
                View.Property(p => p.ReportsType).ShowInList(width: 80);
                View.Property(p => p.LabelType).ShowInList(width: 80);
                View.Property(p => p.PackingNum).ShowInList(width: 80);
                View.Property(p => p.BatchLabel).ShowInList(width: 200);
                View.Property(p => p.TestValue).ShowInList(width: 200);
            }
        }

        /// <summary>
        /// 批次追溯报表下的SN视图配置
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SnViewConfig()
        {
            View.Property(p => p.ProductLabel).ShowInList(width: 250);
        }
    }
}
