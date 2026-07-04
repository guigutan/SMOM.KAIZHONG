using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台帐-使用履历-界面
    /// </summary>
    public class FixtureAccountUseResumeViewConfig : WebViewConfig<FixtureAccountUseResume>
    {
        /// <summary>
        /// 自定义编码类工治具台帐的使用履历视图
        /// </summary>
        public const string CodeUseResumeView = "CodeUseResumeView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CodeUseResumeView);
            if (ViewGroup == CodeUseResumeView)
            {
                ConfigCodeUseResumeView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));
            View.Property(p => p.OperationType).Readonly();
            View.Property(p => p.OperationTime).ShowInList(150).Readonly();
            View.Property(p => p.Resource).HasLabel("产线").Readonly();
            View.Property(p => p.EquipAccount).HasLabel("设备").Readonly();
            View.Property(p => p.Subarea).Readonly();
            View.Property(p => p.Stance).Readonly();
            View.Property(p => p.WorkOrderId).ShowInList(150).HasLabel("工单").Readonly();
            View.Property(p => p.Item).ShowInList(150).HasLabel("物料").Readonly();
            View.Property(p => p.UseNum).Readonly();
            View.Property(p => p.DrawQty).Readonly();
            View.Property(p => p.ThrowQty).Readonly();
            View.Property(p => p.OnlineDate).ShowInList(150).Readonly();
            View.Property(p => p.OfflineDate).ShowInList(150).Readonly();
            View.Property(p => p.CreateByName).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Readonly().Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置编码类工治具台帐的使用履历视图
        /// </summary>
        void ConfigCodeUseResumeView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));

            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.Resource).Readonly().HasLabel("产线").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderId).Readonly().HasLabel("工单").ShowInList(150);
                View.Property(p => p.OperationTime).Readonly().ShowInList(150);
                View.Property(p => p.OperationBy).HasLabel("操作人").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.OperationType).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.OperationQty).Readonly().HasLabel("数量").Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Readonly().Show(ShowInWhere.Hide);
            }
        }
    }
}
