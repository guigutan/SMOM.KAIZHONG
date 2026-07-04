using SIE.Defects.InspectionItems;
using SIE.Fixtures.Models;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using SIE.Web.Fixtures.Models.Commands;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
	/// 工治具型号保养项目视图配置
	/// </summary>
	internal class FixtureModelMaintainProjectViewConfig : WebViewConfig<FixtureModelMaintainProject>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.Fixtures.Models.Commands.AddMaintainProjCommand", WebCommandNames.Edit, typeof(ImmediateDeleteCommand).FullName);
            View.UseCommands(typeof(FixtureModelMaintainImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.MaintainProject).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.Consumable), nameof(r.MaintainProject.Consumable));
                keyValues.Add(nameof(r.Method), nameof(r.MaintainProject.Method));
                keyValues.Add(nameof(r.Tool), nameof(r.MaintainProject.Tool));
                keyValues.Add(nameof(r.MinValue), nameof(r.MaintainProject.MinValue)); 
                keyValues.Add(nameof(r.MaxValue), nameof(r.MaintainProject.MaxValue));
                keyValues.Add(nameof(r.CheckTag), nameof(r.MaintainProject.CheckTag));
                m.DicLinkField = keyValues;
            }).HasLabel("项目名称").UseListSetting(e => { e.HelpInfo = "显示工治具保养项目数据"; });
            View.Property(p => p.InStorageMaintain);
            View.Property(p => p.CommonMaintain);
            View.Property(p => p.OnlineMaintain);
            View.Property(p => p.ToStorageMaintain);
            
            View.Property(p => p.Consumable).Readonly();
            View.Property(p => p.Method).Readonly();
            View.Property(p => p.Tool).Readonly();
            View.Property(p => p.MinValue).ShowInList(width: 120).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.MaxValue).ShowInList(width: 120).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.CheckTag).Readonly();
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 导入
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.FixtureModel.Code).HasLabel("工治具型号编码");
            View.PropertyRef(p => p.MaintainProject.Name).HasLabel("保养项目名称");
            View.Property(p => p.InStorageMaintain);
            View.Property(p => p.CommonMaintain);
            View.Property(p => p.OnlineMaintain);
            View.Property(p => p.ToStorageMaintain);
            View.Property(p => p.AcceptanceItems);
            View.Property(p => p.MinValue);
            View.Property(p => p.MaxValue);
        }
    }
}
