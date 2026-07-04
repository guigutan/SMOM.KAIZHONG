using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Kit.APS.FactoryConfirms;

namespace SIE.Web.Pcb.APS.FactoryConfirms
{
    /// <summary>
    /// 分厂方案设置
    /// </summary>
    public class BranchFactoryProgrammeViewConfig : WebViewConfig<BranchFactoryProgramme>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FactoryConfirmsViewModel));
            View.HasDelegate(BranchFactoryProgramme.IdProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete, WebCommandNames.Save);
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.Remark).Show().ShowInList(width: 230);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.WithoutPaging();
            }
        }
    }
}
