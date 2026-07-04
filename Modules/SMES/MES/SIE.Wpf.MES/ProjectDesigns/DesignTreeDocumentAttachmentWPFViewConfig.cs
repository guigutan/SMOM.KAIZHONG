using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Attachments;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ProjectDesigns
{
    /// <summary>
    /// 附件子表
    /// </summary>
    public class DesignTreeDocumentAttachmentWPFViewConfig : WPFViewConfig<DesignTreeDocumentAttachment>
    {

        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(BatchAssemblyViewModel), typeof(RepairViewModel), typeof(BatchRepairViewModel), typeof(TemporaryRepairViewModel), typeof(InspectViewModel), typeof(BatchInspectViewModel), typeof(InspectByItemViewModel));

            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.RemoveCommands(typeof(AddAttachmentCommand), WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.FileName).ShowInList(gridWidth: 150);
                View.Property(p => p.FilePath).ShowInList(gridWidth: 150);
                View.Property(p => p.FileExtesion).ShowInList(gridWidth: 150);
                View.Property(p => p.FileSize).ShowInList(gridWidth: 150);
            }
        }
    }
}
