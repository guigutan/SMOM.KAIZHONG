using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Stations;
using System.Collections.Generic;

namespace SIE.Web.Tech.Stations
{
    /// <summary>
    /// 工位视图配置
    /// </summary>
    internal class StationViewConfig : WebViewConfig<Station>
    {

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Copy, WebCommandNames.Save,
                    "SIE.Web.Tech.Stations.Commands.StationImportCommand",
                    WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);

                View.AddBehavior("SIE.Web.Tech.Scripts.Behaviors.StationBehavior");
                View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInList(150)
                    .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
                View.Property(p => p.Name).ShowInList(150);
                View.Property(p => p.Resource).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ResourceName),nameof(e.Resource.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(150).UseListSetting(e => { e.HelpInfo = "排除自定义类型的生产资源"; });
                View.Property(p => p.ResourceName).HasLabel("资源名称").ShowInList(150).Readonly();
                View.Property(p => p.CreateByName);
                View.Property(p => p.CreateDate).ShowInList(150);
                View.Property(p => p.UpdateByName);
                View.Property(p => p.UpdateDate).ShowInList(150);
                View.ChildrenProperty(p => p.StationProcessList).Show(ChildShowInWhere.All).HasLabel("关联工序");
                View.ChildrenProperty(p => p.StationEquipmentList).Show(ChildShowInWhere.All).HasLabel("设备列表");

            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Resource);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ResourceName);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.PropertyRef(p => p.Resource.Code).HasLabel("资源编码");
        }
    }
}