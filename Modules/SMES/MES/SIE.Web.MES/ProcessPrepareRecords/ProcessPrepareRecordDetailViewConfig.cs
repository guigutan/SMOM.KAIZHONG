using SIE.MES.ProcessPrepareRecords;
using SIE.Web.MES.PrepareProducts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessPrepareRecords
{
    public class ProcessPrepareRecordDetailViewConfig : WebViewConfig<ProcessPrepareRecordDetail>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessPrepareRecordDetail));
            base.ConfigView();
            if (ViewGroup == "ExecuteViewStr")
            {
                ExecuteView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                //View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProjectCode).ShowInList(width: 150);
                View.Property(p => p.ProjectName).ShowInList(width: 150);
                View.Property(p => p.ProjectType).ShowInList(width: 150);
                View.Property(p => p.ProjectDesc).ShowInList(width: 150);
                View.Property(p => p.Counter).ShowInList(width: 150);
                View.Property(p => p.Result).ShowInList(width: 150);
                View.Property(p => p.Remark).ShowInList(width: 150);
                View.Property(p => p.Confirmer).ShowInList(width: 150);
                View.Property(p => p.ConfirmTime).ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 执行视图
        /// </summary>
        protected void ExecuteView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.Process).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProjectCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProjectName).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProjectType).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProjectDesc).Readonly().ShowInList(width: 150);
                View.Property(p => p.Result).ShowInList(width: 150);
                View.Property(p => p.Remark).ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }


    }
}
