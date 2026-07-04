using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Repairs;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Fixtures.Repairs.Commands;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// 工治具报修视图配置
    /// </summary>
    public class FixtureRepairViewConfig : WebViewConfig<FixtureRepair>
    {
        /// <summary>
        /// 维修单明细视图
        /// </summary>
        public const string RepairDetails = "RepairDetails";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(RepairDetails);
            if (ViewGroup == RepairDetails)
                RepairConfigDetailsView();
        }
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.UseCommands(typeof(AddFixtureRepairCommand).FullName, "SIE.Web.Fixtures.Repairs.Commands.FixtureRepairCommand", ConfigCommands.ModuleConfigCommand);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.RepairState);
            View.Property(p => p.ApplyByName).HasLabel("报修人");
            View.Property(p => p.ApplyDate);
            View.Property(p => p.RepairByName).HasLabel("维修人");
            View.Property(p => p.RepairDate);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(FixtureRepairDetail), (e) =>
            {
                var pagingDataArgs = e as ChildPagingDataArgs;
                var fixtureRepair = e.Parent as FixtureRepair;
                if (fixtureRepair == null)
                    return new EntityList<FixtureRepairDetail>();
                return RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairDetailsByRepairId(fixtureRepair.Id, pagingDataArgs.PagingInfo, pagingDataArgs.SortInfo);
            }).HasLabel("异常/维修详情").Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(FixtureRepairAttachment), (e) =>
            {
                var pagingDataArgs = e as ChildPagingDataArgs;
                var fixtureRepair = e.Parent as FixtureRepair;
                if (fixtureRepair == null)
                    return new EntityList<FixtureRepairAttachment>();
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureRepairAttachment(fixtureRepair.Id, pagingDataArgs.PagingInfo, pagingDataArgs.SortInfo);
            }).HasLabel("图片").Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(FixtureRepairRecord), (e) =>
            {
                var pagingDataArgs = e as ChildPagingDataArgs;
                var fixtureRepair = e.Parent as FixtureRepair;
                if (fixtureRepair == null)
                    return new EntityList<FixtureRepairRecord>();
                return RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairRecord(fixtureRepair.Id, pagingDataArgs.PagingInfo, pagingDataArgs.SortInfo);
            }).HasLabel("维修记录").Show(ChildShowInWhere.All);
        }

        ///<summary>
        ///工治具报修-添加视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Repairs.Script.AddFixtureRepairBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(SaveAddRepairCommand).FullName);
            View.HasDetailColumnsCount(3);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.ApplyByName).Readonly().HasLabel("报修人");
            View.Property(p => p.ApplyDate).Readonly();
            View.Property(p => p.RepairState).Readonly();
            View.ChildrenProperty(p => p.Details).UseViewGroup(FixtureRepairDetailViewConfig.AddFixtureRepairDetail).Show(ChildShowInWhere.All);
        }

        ///<summary>
        /// 工治具报修-维修视图配置
        /// </summary>
        protected virtual void RepairConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.AssignAuthorize(typeof(FixtureRepair));
                View.ClearCommands();
                View.UseCommands(typeof(SaveRepairCommand).FullName);
                View.HasDetailColumnsCount(3);
                View.Property(p => p.No).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ApplyByName).Readonly().HasLabel("报修人").Show(ShowInWhere.All);
                View.Property(p => p.ApplyDate).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.RepairState).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.RepairByName).Readonly().HasLabel("维修人").Show(ShowInWhere.All);
                View.Property(p => p.RepairDate).Readonly().Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(FixtureRepairDetail), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var entity = args.Parent as FixtureRepair;
                    var repairDetails = RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairDetailsByRepairId(entity.Id, args.PagingInfo, args.SortInfo);
                    if (repairDetails == null) repairDetails = new EntityList<FixtureRepairDetail>();
                    return repairDetails;
                }, FixtureRepairDetailViewConfig.FixtureRepairDetail).Show(ChildShowInWhere.All).HasLabel("异常/维修详情");
            }
        }
    }
}