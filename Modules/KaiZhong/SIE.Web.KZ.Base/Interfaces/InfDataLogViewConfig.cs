using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    /// <summary>
    /// MOM与其它系统接口日志
    /// </summary>
    public class InfDataLogViewConfig : WebViewConfig<InfDataLog>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();

        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.BeginDate).ShowInList(width: 150).FixColumn();
            View.Property(p => p.EndDate).ShowInList(width: 150).FixColumn();
            View.Property(p => p.InfType).ShowInList(width: 150).FixColumn();
            View.Property(p => p.CallDirection);
            View.Property(p => p.CallResult);
            View.Property(p => p.Qty);
            View.Property(p => p.TipMsg);
            View.Property(p => p.ErrorMsg);
            View.Property(p => p.Remark);
            View.Property(p => p.RequestContent);
            View.Property(p => p.ResponseContent);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.InfType);
            View.Property(p => p.CallDirection);
            View.Property(p => p.CallResult);
            View.Property(p => p.RequestContent);
        }
    }
}
