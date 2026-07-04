using SIE.Defects.InspectionItems;
using SIE.Fixtures.Models;
using SIE.Fixtures.Projects;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具编码保养项目视图配置
    /// </summary>
    internal class FixtureEncodeMaintainProjectViewConfig : WebViewConfig<FixtureEncodeMaintainProject>
    {

        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int displayCoulmnWidth = 20;
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.Property(p => p.MaintainProjectId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProjectConsumable), nameof(e.MaintainProject.Name));
                keyValues.Add(nameof(e.ProjectMethod), nameof(e.MaintainProject.Method));
                keyValues.Add(nameof(e.ProjectTool), nameof(e.MaintainProject.Tool));
                keyValues.Add(nameof(e.MinValue), nameof(e.MaintainProject.MinValue));
                keyValues.Add(nameof(e.CheckTag), nameof(e.MaintainProject.CheckTag));
                keyValues.Add(nameof(e.MaxValue), nameof(e.MaintainProject.MaxValue)); 
                m.DicLinkField = keyValues;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<MaintainProjectController>().GetMaintainProjects(pagingInfo, keyword);
            }).HasLabel("保养项目").ShowInList(displayCoulmnWidth*5);
            View.Property(p => p.InStorageMaintain).ShowInList(displayCoulmnWidth * 5); 
            View.Property(p => p.CommonMaintain).ShowInList(displayCoulmnWidth * 5); 
            View.Property(p => p.OnlineMaintain).ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.ToStorageMaintain).ShowInList(displayCoulmnWidth * 5);
            
            View.Property(p => p.ProjectConsumable).Readonly().ShowInList(displayCoulmnWidth * 5);
            View.Property(p => p.ProjectMethod).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.ProjectTool).Readonly().ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.MinValue).ShowInList(displayCoulmnWidth * 6).Readonly(p=>p.CheckTag== CheckTag.Qualitative);
            View.Property(p => p.MaxValue).ShowInList(displayCoulmnWidth * 6).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.CheckTag).Readonly().ShowInList(displayCoulmnWidth * 5);
        }
    }
}