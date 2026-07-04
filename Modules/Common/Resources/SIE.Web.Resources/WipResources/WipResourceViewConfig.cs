using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.WipResources;
using SIE.Web.Common;
using SIE.Web.Extensions;
using SIE.Web.Resources.WipResources.Commands;
using System.Collections.Generic;

namespace SIE.Web.Resources.WipResources
{
    /// <summary>
    /// 生产资源视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class WipResourceViewConfig : WebViewConfig<WipResource>
    {
        /// <summary>
        /// 生产资源明细信息视图
        /// </summary>
        public const string WipResourceDetail = "WipResourceDetail";

        /// <summary>
        /// 未启用或停用状态可编辑
        /// </summary>
        private const string STOPCANEDIT = "未启用或停用状态可编辑";

        /// <summary>
        /// 启用状态可编辑
        /// </summary>
        private const string ACTIVEDCANEDIT = "启用状态可编辑";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WipResourceDetail);
            if (ViewGroup == WipResourceDetail)
                WipResourceDetailView();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.RequierModels(typeof(SynWipResSetting));
            View.RequireCalendarResource();
            View.UseClientOrder();
            View.RequirModuleResource("SIE.Web.Resources.WipResources.Scripts.WipResourceLayout.js");
            View.UseCommands("SIE.Web.Resources.WipResources.Commands.WipResourceEditCommand", WebCommandNames.Save,
                typeof(SynWipResSettingCommand).FullName, typeof(WipResourceRefreshCommand).FullName,
                typeof(WipResourceEnableCommand).FullName, typeof(WipResourceStopCommand).FullName, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().HasOrderNo(10);
                View.Property(p => p.Name).Readonly().HasOrderNo(20);
                View.Property(p => p.SourceType).Readonly().HasOrderNo(30);
                View.Property(p => p.WorkShopId).HasLabel("所属车间").Readonly().HasOrderNo(40);
                View.Property(p => p.FactoryId).Readonly().HasOrderNo(45);
                View.Property(p => p.AndonUphold).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AndonCode), nameof(e.AndonUphold.AndonCode));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
                View.Property(p => p.ProcessTechTypeId)
                    .Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect)
                .UseListSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(50)
                   .UseDataSource((source, pagingInfo, keyword) =>
                   {
                       var result = RT.Service.Resolve<ProcessTechTypeController>().GetProcessTechTypeList(pagingInfo, keyword);
                       if (result == null) return new EntityList<ProcessTechType>();
                       return result;
                   });
                View.Property(p => p.SchemeId).UseSchemeLookUpEditor().UseListSetting(e => { e.HelpInfo = "显示可用日历方案"; }).HasOrderNo(60);
                View.Property(p => p.ResourceState).Readonly().HasOrderNo(70);
                View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1).Readonly(p => p.ResourceState == ResourceState.Stop)
                .UseListSetting(e => { e.HelpInfo = ACTIVEDCANEDIT; }).HasOrderNo(80);
                View.Property(p => p.AutomationType).Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect)
                     .UseListSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(85);
                View.Property(p => p.Sequence).UseSpinEditor(e => e.MinValue = 0).HasOrderNo(90);
                View.AttachDetailChildrenProperty(typeof(WipResource), (sr) =>
                {
                    var wipResource = sr.Parent as WipResource;
                    wipResource = RF.GetById<WipResource>(wipResource.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return wipResource;
                }, WipResourceDetail).HasLabel("资源信息").HasOrderNo(10);
            }
        }

        /// <summary>
        /// 表单视图配置(弹窗视图配置)
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Resources.WipResources.Behaviors.WipResourceBehavior");
            View.ClearCommands();
            View.UseCommand(WebCommandNames.FormSave);
            View.HasDetailColumnsCount(value: 3);

            View.Property(p => p.Code).Readonly().HasOrderNo(10);
            View.Property(p => p.Name).Readonly().HasOrderNo(20);
            View.Property(p => p.SourceType).Readonly().HasOrderNo(30);
            View.Property(p => p.WorkShopId).HasLabel("所属车间").Show(ShowInWhere.All).Readonly().HasOrderNo(40);
            View.Property(p => p.FactoryId).Show(ShowInWhere.All).Readonly().HasOrderNo(45);
            View.Property(p => p.ProcessTechTypeId).Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect || p.ParentResourceId > 0)
                .UseListSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(50);
            View.Property(p => p.SchemeId).Readonly(p => p.ParentResourceId > 0).UseSchemeLookUpEditor().HasLabel("日历方案").Show(ShowInWhere.All).HasOrderNo(60);
            View.Property(p => p.ResourceState).Readonly().HasOrderNo(70);
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1).Readonly(p => p.ResourceState == ResourceState.Stop)
                .UseListSetting(e => { e.HelpInfo = ACTIVEDCANEDIT; }).HasOrderNo(80);
            View.Property(p => p.TaktTime).Readonly(p => p.ResourceState == ResourceState.Stop)
                .UseListSetting(e => { e.HelpInfo = ACTIVEDCANEDIT; }).HasOrderNo(90);
            View.Property(p => p.AutomationType).Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect)
                .UseFormSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(95);
            View.Property(p => p.Sequence).UseSpinEditor(e => e.MinValue = 0).HasOrderNo(100);
            View.Property(p => p.IsOutMade).Show(ShowInWhere.All).HasOrderNo(110);
            View.Property(p => p.AndonUphold).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.AndonCode), nameof(e.AndonUphold.AndonCode));
                m.DicLinkField = keyValues;
            }).ShowInList(width: 200);
            View.Property(p => p.AndonCode).ShowInList(width: 150);
        }

        /// <summary>
        /// 生产资源明细信息视图
        /// </summary>
        protected void WipResourceDetailView()
        {
            View.AddBehavior("SIE.Web.Resources.WipResources.Behaviors.WipResourceBehavior");
            View.AssignAuthorize(typeof(WipResource));
            View.ClearCommands();
            View.UseCommand(WebCommandNames.FormSave);
            View.HasDetailColumnsCount(value: 3);
            View.Property(p => p.Code).Show().Readonly().HasOrderNo(10);
            View.Property(p => p.Name).Show().Readonly().HasOrderNo(20);
            View.Property(p => p.ResourceType).Show().HasOrderNo(21).UseCatalogEditor(e => { e.CatalogType = WipResource.ResourceTypeString;e.CatalogReloadData = true; })
                .UseListSetting(e => e.HelpInfo = "资源类型快码类型（" + WipResource.ResourceTypeString + "）")
                .Readonly(p => p.ResourceState == ResourceState.Actived);
            View.Property(p => p.SourceType).Show().Readonly().HasOrderNo(30);
            View.Property(p => p.WorkShopId).HasLabel("所属车间").Show(ShowInWhere.All).Readonly().HasOrderNo(40);
            View.Property(p => p.FactoryId).Show(ShowInWhere.All).Readonly().HasOrderNo(45);
            View.Property(p => p.ProcessTechTypeId).Show().Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect || p.ParentResourceId > 0)
                .UseListSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(50);
            View.Property(p => p.SchemeId).Readonly(p => p.ParentResourceId > 0).UseSchemeLookUpEditor().HasLabel("日历方案").Show(ShowInWhere.All).HasOrderNo(60);
            View.Property(p => p.ResourceState).Show().Readonly().HasOrderNo(70);
            View.Property(p => p.Qty).Show().UseSpinEditor(e => e.MinValue = 1).Readonly(p => p.ResourceState == ResourceState.Stop)
                .UseListSetting(e => { e.HelpInfo = ACTIVEDCANEDIT; }).HasOrderNo(80);
            View.Property(p => p.TaktTime).Show().Readonly(p => p.ResourceState == ResourceState.Stop)
                .UseListSetting(e => { e.HelpInfo = ACTIVEDCANEDIT; }).HasOrderNo(90);
            View.Property(p => p.AutomationType).Readonly(p => p.ResourceState == ResourceState.Actived || p.ResourceState == ResourceState.Diseffect)
                .UseFormSetting(e => { e.HelpInfo = STOPCANEDIT; }).HasOrderNo(95).Show(ShowInWhere.All);
            View.Property(p => p.Sequence).Show().UseSpinEditor(e => e.MinValue = 0).HasOrderNo(100);
            View.Property(p => p.IsOutMade).Show().Show(ShowInWhere.All).HasOrderNo(110);
        }

        /// <summary>
        /// 选择视图配置(选择框视图配置)
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.UseClientOrder();
            View.Property(p => p.Code).HasLabel("资源编号").Show(ShowInWhere.All);
            View.Property(p => p.Name).HasLabel("资源名称").Show(ShowInWhere.All);
            View.Property(p => p.SourceType).Show(ShowInWhere.All);
            View.Property(p => p.FactoryId)/*.UseShopEditor()*/.HasLabel("所属工厂").Show(ShowInWhere.All);
            View.Property(p => p.WorkShopId)/*.UseShopEditor()*/.HasLabel("所属车间").Show(ShowInWhere.All);
            View.Property(p => p.ProcessTechTypeId).HasLabel("资源类型").Show(ShowInWhere.All);
        }
    }
}
