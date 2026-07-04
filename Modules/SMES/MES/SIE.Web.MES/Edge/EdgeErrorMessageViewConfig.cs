using SIE.MES.Edge.Models;
using SIE.Web.MES.Edge.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Edge
{
    /// <summary>
    /// 消息接收日志 视图配置
    /// </summary>
    internal class EdgeErrorMessageViewConfig : WebViewConfig<EdgeErrorMessage>
    {
        protected override void ConfigListView()
        {
            View.UseCommand(typeof(ReSubmitMessageCommand).FullName);
            View.DisableEditing();
            View.Property(p => p.Guid);
            View.Property(p => p.Barcode);
            View.Property(p => p.Process);
            View.Property(p => p.WorkOrder);
            View.Property(p => p.Name);
            View.Property(p => p.Bodys);
            View.Property(p => p.ErrorContent);
            View.Property(p => p.IsError);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Barcode);
            View.Property(p => p.Process);
            View.Property(p => p.WorkOrder);
            View.Property(p => p.Name);
            View.Property(p => p.IsError).DefaultValue(YesNo.Yes);
        }
    }
}
