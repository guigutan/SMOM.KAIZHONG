using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Web.Common.Sort.Commands;

namespace SIE.Web.Pcb.APS.FactoryConfirms
{
    /// <summary>
    /// 分厂方案明细设置
    /// </summary>
    public class BranchFactoryProgrammeDetailViewConfig : WebViewConfig<BranchFactoryProgrammeDetail>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FactoryConfirmsViewModel));
            View.HasDelegate(BranchFactoryProgrammeDetail.IdProperty);
        }
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete,
                typeof(MoveUpCommand).FullName, typeof(MoveDownCommand).FullName
              );
                View.Property(p => p.ProgrammeRule).ShowInList(width: 230);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.WithoutPaging();
            }
        }
    }
}

