using SIE.Resources.WipResources;
using SIE.Wpf.MES.Editors;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工作站 视图配置
    /// </summary>
    public class KZWorkstationViewConfig : WPFViewConfig<KZWorkstation>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(4);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Resource).UsePagingLookUpEditor(c =>
                {
                    c.DisplayMember = WipResource.NameProperty.Name;
                }).Readonly();
                //View.Property(p => p.Process).Readonly();
                //View.Property(p => p.Station).Readonly();
                View.Property(p => p.ChangeWorkstation)
                    .UseEditor(ChangeWorkStationEditor.EditorName)
                    .ShowInDetail(height: 40, hideLabel: true);
            }
        }


    }
}
