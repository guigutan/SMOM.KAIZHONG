using SIE.MES.ProjectDesigns.ChildInfos;
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
    /// 项目号需求设计附件信息视图配置
    /// </summary>
    public class DesignTreeDocumentWPFViewConfig : WPFViewConfig<DesignTreeDocument>
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
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.DocCode).ShowInList(gridWidth: 120);
                View.Property(p => p.DocName).ShowInList(gridWidth: 120);
                View.Property(p => p.DocVer).ShowInList(gridWidth: 120);
                View.Property(p => p.DocType).ShowInList(gridWidth: 120);
                View.Property(p => p.Product).HasLabel("产品编码").ShowInList(gridWidth: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(gridWidth: 120);
                View.Property(p => p.Process).ShowInList(gridWidth: 120);
                View.ChildrenProperty(p => p.AttachmentList).UseViewGroup(DesignTreeDocumentAttachmentWPFViewConfig.LookUpViewGroup);
            }
        }
    }
}
