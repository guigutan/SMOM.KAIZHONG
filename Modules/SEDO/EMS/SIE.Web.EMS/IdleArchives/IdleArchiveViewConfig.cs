using SIE.Core.Equipments;
using SIE.Domain;
using SIE.EMS.IdleArchives;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.EMS.Common.Commands;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.IdleArchives.Commands;
using SIE.Web.Resources;

namespace SIE.Web.EMS.IdleArchives
{
    /// <summary>
    /// 闲置与封存视图配置
    /// </summary>
    public class IdleArchiveViewConfig : WebViewConfig<IdleArchive>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.IdleArchives.IdleArchivesListBehavior");
            View.UseCommands("SIE.Web.EMS.IdleArchives.Commands.AddIdleArchivesCommand", "SIE.Web.EMS.FixedAssets.Commands.EditIdleArchivesCommand",
                typeof(PromptlyDeleteCommand).FullName,
                typeof(SubmitCommand).FullName,
                typeof(ApprovalCommand).FullName,
                typeof(CancelCommand).FullName
               );
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInList(120);
                View.Property(p => p.IdleArchiveType).Readonly().ShowInList(80);
                View.Property(p => p.FactoryId).HasLabel("工厂");
                View.Property(p => p.ApprovalStatus).ShowInList();
                View.Property(p => p.DepartmentId).HasLabel("管理部门");
                View.Property(p => p.UseDepartmentId).HasLabel("使用部门");
                View.Property(p => p.TypeCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; }).Readonly().ShowInList(80);
                View.Property(p => p.IsAsset).Readonly().ShowInList(80);
                View.Property(p => p.ApplicantId).HasLabel("申请人").ShowInList(120);
                View.Property(p => p.ApplyDate).UseDateEditor().Readonly().ShowInList(120);
                View.Property(p => p.Remark).ShowInList(250).Readonly();

                View.ChildrenProperty(p => p.IdleArchiveDetailList).HasLabel("设备明细").HasOrderNo(1);
                View.ChildrenProperty(p => p.IdleArchiveAttachmentList).HasLabel("附件").UseViewGroup("SeeView").HasOrderNo(2);
                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent as IdleArchive;
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }

                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                        typeof(IdleArchive).FullName, args.SortInfo, args.PagingInfo);

                }, ListView).HasLabel("审核记录").HasOrderNo(3);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommand(typeof(SaveIdleArchivesCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.IdleArchives.IdleArchivesBehavior");
            View.UseDetail(4);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.IdleArchiveType);
            View.Property(p => p.FactoryId).HasLabel("工厂").UseFactoryEditor().Cascade(p => p.DepartmentId, null).Cascade(p => p.UseDepartmentId, null);
            View.Property(p => p.DepartmentId).HasLabel("管理部门").UseUserBussinessDepartmentEditor(factoryIdPropertyName: "FactoryId");
            View.Property(p => p.UseDepartmentId).HasLabel("使用部门").UseUserBussinessDepartmentEditor(factoryIdPropertyName: "FactoryId");
            View.Property(p => p.TypeCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.CatalogReloadData = true; }); ;
            View.Property(p => p.IsAsset);
            View.Property(p => p.ApplyDate).UseDateEditor();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).HasLabel("申请人");
            View.Property(p => p.Remark);
            View.ChildrenProperty(p => p.IdleArchiveDetailList).HasLabel("设备明细").UseViewGroup("EditView").HasOrderNo(1);
            View.ChildrenProperty(p => p.IdleArchiveAttachmentList).HasLabel("附件").HasOrderNo(2);

        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.DisableEditing();
            View.Property(p => p.No);
            View.Property(p => p.TypeCategory);
            View.Property(p => p.IsAsset);
            View.Property(p => p.ApplyDate);
            View.Property(p => p.IdleArchiveType);
        }
    }
}