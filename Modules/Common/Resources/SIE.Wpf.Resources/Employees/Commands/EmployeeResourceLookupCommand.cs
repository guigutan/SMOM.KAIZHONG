using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 生成资源选择按钮
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = CommandGroupType.Edit, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class EmployeeResourceLookupCommand : LookupCommand
    {
        /// <summary>
        /// 加载待选择数据
        /// </summary>
        /// <param name="ui">一个生成的控件结果</param>
        protected override void LoadSelectionViewData(ControlResult ui)
        {
            ////base.LoadSelectionViewData(ui);
            var queryView = (ui.MainView as ListLogicalView).QueryView;
            if (queryView != null)
            {
                queryView.DataProvider = (e) => MyQueryDataProvider(e);
            }
        }

        /// <summary>
        /// 查询的数据源
        /// </summary>
        /// <param name="e">查询参数</param>
        /// <returns>生产资源集合</returns>
        private EntityList<WipResource> MyQueryDataProvider(Criteria e)
        {
            var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResourcesExcludeSpecifyStateType(e as WipResourceCriteria);
            return wipResources;
        }
    }
}
