using SIE.EMS.RunStandards;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.RunStandards
{
    /// <summary>
    /// 操作记录
    /// </summary>
    public class RunStandardLogViewConfig : WebViewConfig<RunStandardLog>
    {
        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.OperationTypeText);
            View.Property(p => p.OperatorId);
            View.Property(p => p.OperationDateTime);

            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
