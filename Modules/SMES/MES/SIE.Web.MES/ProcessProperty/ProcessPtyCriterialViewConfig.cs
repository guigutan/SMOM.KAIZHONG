using SIE.MES.Fixture;
using SIE.MES.ProcessProperty;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.MES.ProcessProperty.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessProperty
{
    /// <summary>
    /// 维护工序属性维护实体查询
    /// </summary>
    public class ProcessPtyCriterialViewConfig : WebViewConfig<ProcessPtyCriterial>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
                View.Property(p => p.Scheduling).ShowInList(width: 150);
                View.Property(p => p.DispatchWork).ShowInList(width: 150);
                View.Property(p => p.IsPrepare).ShowInList(width: 150);
                View.Property(p => p.IsTransfer).ShowInList(width: 150);
            }
        }
    }
}
