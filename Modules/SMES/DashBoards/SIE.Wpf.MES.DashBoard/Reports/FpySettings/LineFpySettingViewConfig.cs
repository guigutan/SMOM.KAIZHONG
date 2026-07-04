using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产线直通率设置视图配置
    /// </summary>
    internal class LineFpySettingViewConfig : WPFViewConfig<LineFpySetting>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(LineFpySetting.ResourceIdProperty);
            View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ShopReportViewModel), typeof(LineReportViewModel));
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.Property(p => p.Resource).HasLabel("资源编号").UseDataSource((e, c, r) =>
            {
                var setting = e as LineFpySetting;
                if (setting?.ShopFpySetting?.Shop != null)
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, setting.ShopFpySetting.ShopId, c, r);
                }
                return new Domain.EntityList<WipResource>();
            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = WipResource.CodeProperty.Name; });
            View.Property(p => p.Resource.Name).HasLabel("资源名称");
            View.Property(p => p.Desired).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.Property(p => p.Alarm).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}