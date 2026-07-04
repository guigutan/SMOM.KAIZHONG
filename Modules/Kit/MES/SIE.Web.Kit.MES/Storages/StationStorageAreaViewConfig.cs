using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using SIE.Tech.Stations;
using System.Collections.Generic;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 产线工位货区视图配置
    /// </summary>
    internal class StationStorageAreaViewConfig : WebViewConfig<StationStorageArea>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Kit.MES.Storages.Commands.AddStorageSafetyCommand");
            View.Property(p => p.StationCode).HasLabel("工位编码").Readonly();
            View.Property(p => p.Station).HasLabel("工位名称").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.StationCode), nameof(Station.Code));
                m.DicLinkField = dic;
            }).UseDataSource((e, p, s) =>
              {
                  return RT.Service.Resolve<StationController>().GetLoadItemStations(s, p);
              });
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Station);
            View.Property(p => p.StationName).HasLabel("工位名称");
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.StorageArea.Code).HasLabel("工位货区编码");
            View.PropertyRef(p => p.Station.Name).HasLabel("工位名称");
        }
    }
}