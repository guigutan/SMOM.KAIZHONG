using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Web.MES.ProjectDesigns.ChildCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 基础属性
    /// </summary>
    public class DesignBasicPropertyViewConfig : WebViewConfig<DesignBasicProperty>
    {

        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(BasicPropertyAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, typeof(BasicPropertySaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.BasicProperty).ShowInList();
                View.Property(p => p.BasicProValue).ShowInList();
                View.Property(p => p.BasicProUnit).ShowInList();
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.BasicProperty).ShowInList();
                View.Property(p => p.BasicProValue).ShowInList();
                View.Property(p => p.BasicProUnit).ShowInList();
            }
        }
    }
}
