using SIE.Domain;
using SIE.Items.ProductModels;
using SIE.MES.TeamManagement.Vacancies;

namespace SIE.Web.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编查看工单视图配置
    /// </summary>    
    public class WoModelViewConfig : WebViewConfig<WorkOrderViewModel>
    {
        /// <summary>
        /// 班组缺编查看工单视图ViewGroup
        /// </summary>
        public const string WorkGroupView = "WorkGroupView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkGroupView);

            if (ViewGroup == WorkGroupView) 
            {
                WorkGroupConfigListView();
            }
        }

        /// <summary>
        /// 默认工单视图
        /// </summary>
        public void WorkGroupConfigListView()
        {
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList().Readonly();
                View.Property(p => p.ProductCode).ShowInList().Readonly();
                View.Property(p => p.ProductModel).ShowInList().Readonly();
                View.Property(p => p.ProductName).ShowInList().Readonly();
                View.Property(p => p.PlanQty).ShowInList().Readonly();
                View.Property(p => p.FinishQty).ShowInList().Readonly();
                View.Property(p => p.StateName).ShowInList().Readonly();
                View.Property(p => p.PlanBeginDate).ShowInList().Readonly();
                View.Property(p => p.PlanEndDate).ShowInList().Readonly();
                View.Property(p => p.WorkShopName).ShowInList().Readonly();
                View.Property(p => p.ResourceName).ShowInList().Readonly();
                View.AttachChildrenProperty(typeof(ModelSkillViewModel), (o) =>
                {
                    var dp = o as ChildPagingDataWithParentEntityArgs;
                    if (string.IsNullOrEmpty(dp.ParentEntity)) return new EntityList<ModelSkillViewModel>();
                    var ob = dp.ParentEntity.ToJsonObject<WorkOrderViewModel>();
                    if (ob.ProductModelId.HasValue)
                    {
                        var result = RT.Service.Resolve<ProductModelController>().GetModelSkill(ob.EmployeeIds, ob.ProductModelId.Value);
                        return result;
                    }
                    else return new EntityList<ModelSkillViewModel>();
                }, viewGroup: WorkGroupView).HasLabel("机型缺编统计");
            }
        }
    }
}