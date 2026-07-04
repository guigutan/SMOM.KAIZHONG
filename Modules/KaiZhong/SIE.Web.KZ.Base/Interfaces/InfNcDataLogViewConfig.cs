using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    /// <summary>
    /// 主数据NC接口日志
    /// </summary>
    public class InfNcDataLogViewConfig : WebViewConfig<InfNcDataLog>
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
            View.Property(p => p.InfType).ShowInList(width: 150);
            View.Property(p => p.InfCode).ShowInList(width: 150);
            View.Property(p => p.OperationType).ShowInList(width: 150);
            View.Property(p => p.DataJsons).ShowInList(width: 150);
            View.Property(p => p.GroupGuid).Show().Readonly();
            View.Property(p => p.CallDirection);
            View.Property(p => p.CallResult);
            View.Property(p => p.ErrorMsg);
            View.Property(p => p.Remark);
            View.Property(p => p.ResponseContent);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.InfType).ShowInList(width: 150);
            View.Property(p => p.InfCode).ShowInList(width: 150);
            View.Property(p => p.OperationType).ShowInList(width: 150);
            View.Property(p => p.CallResult);
        }
    }
}
