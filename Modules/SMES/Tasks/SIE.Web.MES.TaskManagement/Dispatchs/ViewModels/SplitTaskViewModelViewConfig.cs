using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.Resources.WipResources;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 拆分任务ViewModel主视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SplitTaskViewModelViewConfig : WebViewConfig<SplitTaskViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(DispatchTask));
        }

        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 0);
            View.Property(p => p.WipResourceId).Show().UseDataSource((e, p, k) =>
            {
                var entity = e as SplitTaskViewModel;
                if (entity == null)
                    return new EntityList<WipResource>();

                var list = RT.Service.Resolve<WipResourceController>().GetWipResources(RT.IdentityId, p, k);
                return list;
            });
        }
    }
}
