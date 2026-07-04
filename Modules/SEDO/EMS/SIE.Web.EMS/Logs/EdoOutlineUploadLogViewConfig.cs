using SIE.EMS.Logs;
using SIE.MetaModel.View;
using SIE.Web.EMS.Logs.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Logs
{
    /// <summary>
    /// 上传日志视图
    /// </summary>
    public class EdoOutlineUploadLogViewConfig : WebViewConfig<EdoOutlineUploadLog>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.FormEdit();
            View.UseCommands(typeof(ViewDataCommand).FullName, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.BillNo);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.MachineCode);
                View.Property(p => p.MachineName);
                View.Property(p => p.EncodeCode);
                //View.Property(p => p.EncodeName);
                View.Property(p => p.Sn);
                View.Property(p => p.FailReason);
                View.Property(p => p.UploadState);
                View.Property(p => p.UploadType);
                //View.Property(p => p.DetailMsg);
                View.Property(p => p.CreateByName);
                View.Property(p => p.CreateDate);
            }
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();            
            View.Property(p => p.DetailMsg).UseMemoEditor(p => { p.IsReadonly = true; });
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BillNo);
                View.Property(p => p.UploadState);
                View.Property(p => p.UploadType);
                View.Property(p => p.MachineCode);
                View.Property(p => p.EncodeCode);
                View.Property(p => p.Sn);
                View.Property(p => p.CreateBy);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
                View.Property(p => p.Remark);
            }
        }
    }
}
