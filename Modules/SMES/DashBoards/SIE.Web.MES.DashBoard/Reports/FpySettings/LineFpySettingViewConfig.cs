using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产线直通率设置视图配置
    /// </summary>
    internal class LineFpySettingViewConfig : WebViewConfig<LineFpySetting>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(LineFpySetting.ResourceIdProperty);
            View.AssignAuthorize(typeof(ShopReportViewModel), typeof(LineReportViewModel));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Resource).HasLabel("资源编号").UseDataSource((e, c, r) =>
            {
                var setting = e as LineFpySetting;
                //var setting = RF.GetById<LineFpySetting>(entity.Id);
                if (setting?.ShopFpySetting?.Shop != null)
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, setting.ShopFpySetting.ShopId, c, r);
                }
                return new Domain.EntityList<WipResource>();
            }).UsePagingLookUpEditor(p => p.DisplayField = WipResource.CodeProperty.Name).UsePagingLookUpEditor(
                (m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ResourceName), nameof(WipResource.Name));
                    m.DicLinkField = dic;
                }).UseListSetting(e => { e.HelpInfo = "显示当前不失效车间下的生产资源"; });
            View.Property(p => p.ResourceName).HasLabel("资源名称");
            View.Property(p => p.Desired).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.Alarm).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
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
