using SIE.MES.TaskManagement.SuspectProductLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SuspectProductLabels
{
    internal class ScrapDetailCriteriaViewConfig:WebViewConfig<ScrapDetailCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo);
            View.Property(p => p.SubBatchNo);
            View.Property(p => p.ProductName);
            View.Property(p => p.LineType);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ClassType);
            View.Property(p => p.ItemName);
            View.Property(p => p.ItemType);
            View.Property(p => p.BadCode);
            View.Property(p => p.HandleName);
            View.Property(p => p.HandleDate);
            View.Property(p => p.ScrapDate);
            View.Property(p => p.MrpController);
            View.Property(p => p.ShortDescription);
        }
    }
}
