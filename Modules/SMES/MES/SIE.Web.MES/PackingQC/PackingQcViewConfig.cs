using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.Web.MES.PackingQC.Commands;
using System;

namespace SIE.Web.MES.PackingQC
{
    /// <summary>
    /// 装箱QC确认
    /// </summary>
    public class PackingQcViewConfig : WebViewConfig<PackingQc>
    {
        /// <summary>
        /// 装箱QC确认
        /// </summary>
        public const string QcViewStr = "QcViewStr";

        /// <summary>
        /// QC执行
        /// </summary>
        public const string ExecuteViewStr = "ExecuteViewStr";

        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(QcViewStr, ExecuteViewStr);
            if (ViewGroup == QcViewStr)
            {
                QcListView();
            }
            if (ViewGroup == ExecuteViewStr)
            {
                ExecuteView();
            }
        }

        protected void QcListView()
        {
            View.UseCommands("SIE.Web.MES.PackingQC.Commands.PackingQcExecuteCommand");
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(ReportUloadCommand).FullName);
                View.Property(p => p.BlueLabel).Readonly().ShowInList(width: 150);
                View.Property(p => p.OldBlueLabel).Readonly().ShowInList(width: 150);
                View.Property(p => p.BlueLableNum).Readonly().ShowInList(width: 80);
                View.Property(p => p.PackingNum).Readonly().ShowInList(width: 80);
                View.Property(p => p.UnboxedQty).Readonly().ShowInList(width: 80);
                View.Property(p => p.PackIdent).Readonly().ShowInList(width: 80);
                View.Property(p => p.Confirm).Readonly().ShowInList(width: 120);
                View.Property(p => p.Item).Readonly().ShowInList(width: 170);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 250);
                View.Property(p => p.ReportsType).Readonly().ShowInList(width: 80);
                View.Property(p => p.ResourceId).Show().Readonly();
                View.Property(p => p.IsUploadSap).Show().Readonly();
                View.Property(p => p.UploadResult).ShowInList(200).Readonly();
                View.Property(p => p.CreateByName).Readonly().ShowInList(width: 100);
                View.Property(p => p.UpdateDate).Readonly().ShowInList(width: 100);
                View.ChildrenProperty(p => p.PackingDetailList).HasLabel("装箱明细").Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(PackingDetail), (e) =>
                {
                    var args = e as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<PackingQc>();
                    if (parent == null)
                    {
                        return new EntityList<PackingDetail>();
                    }
                    return RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(parent.Id, args.PagingInfo, args.SortInfo);
                }).HasLabel("装箱明细");
                View.ChildrenProperty(p => p.DocList).HasLabel("附件");
            }
        }

        protected void ExecuteView()
        {
            View.DisableEditing();
            View.UseCommand(typeof(ComfrimCommand).FullName);
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).Show();
                //View.Property(p => p.Item).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.PackingNum).Show();
                View.ChildrenProperty(p=>p.DocList).Show(ChildShowInWhere.All);
            }
        }
    }
}
